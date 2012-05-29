// namespace FSharpTrainin
#r "System.Net.dll"
(* 
Esimerkki perustuu Tomas Petricekic koodiframenttiin, joka löytyy osoitteesta http://fssnip.net/6e

Esimerkin idea on demonstroida seuraavia ominaisuuksia:
1) Luokkien laajentaminen ja hyödyntäminen ilman periyttämistä tai muita vastaavia raskaita ratkaisuja ongelmaan. 
2) Async tyyppi, AsynBuilder ja niiden käyttö
*)
module HttpListenerHelpers = 
    open System
    open System.IO
    open System.Net
    open System.Threading
    open System.Collections.Generic

    // Alla HttpListener ja WebClient luokiin laajenentaan tuki F#:n Async tyypille. 
    // Näistä WebClient luokan laajennus on vaikea selkoisempi. Olennaisesti 
    // Async.FromContinuations tuottaa Async luokasta sellaisen instanssin joka kuuntelee kolmea asyncronista tapahtumaa:
    // - Tuli vastaus yritykseen ladata url. Ei ongelma
    // - Jotain meni pieleen.
    // - Lataus peruutettiin.
    // 
    // Async luokka toimii eräänlaisena proxynä WebClientin DownloadDataComplited eventille. 
    //
    // Ensimmäisessä tapauksessa luodaan Async proxy HttpListenerin BeginGetContext ja EndGetContext delegaatille. 
    // Käytännössä tämä sallii sivukyselyn käsittelyn syncronisesti ja vastuksen kirjoittamisen HttpResponsen 
    // tulostevirtaan kunhan sellainen web clientilta saapuu.
    type System.Net.HttpListener with
        member x.AsyncGetContext() = 
            Async.FromBeginEnd(x.BeginGetContext, x.EndGetContext)

    type System.Net.WebClient with
        /// Asynchronously downloads data from the 
        member x.AsyncDownloadData(uri) = 
            Async.FromContinuations(fun (cont, econt, ccont) ->
                x.DownloadDataCompleted.Add(fun res ->
                    if res.Error <> null then econt res.Error
                    elif res.Cancelled then ccont (new OperationCanceledException())
                    else cont res.Result)
                x.DownloadDataAsync(uri) )

    // Itseäni on rasittanut se, C# extension metodeilla pystyy luomaan vain instanssin metodeja mutta ei staattisia metodeja. F#:ssa ei ole tätä rajoitusta.
    type System.Net.HttpListener with 
        /// Starts an HTTP server on the specified URL with the
        /// specified asynchronous function for handling requests
        static member Start(url, f) = 
            let tokenSource = new CancellationTokenSource()
            Async.Start
                ((async { 
                    use listener = new HttpListener()
                    listener.Prefixes.Add(url)
                    listener.Start()
                    while true do 
                        let! context = listener.AsyncGetContext()
                        Async.Start(f context, tokenSource.Token)}),
                    cancellationToken = tokenSource.Token)
            tokenSource

      /// Starts an HTTP server on the specified URL with the
      /// specified synchronous function for handling requests
        static member StartSynchronous(url, f) =
            HttpListener.Start(url, f >> async.Return) // Location where the proxy copies content from


module ASyncLister = 
    open System
    open System.IO
    open System.Net
    open HttpListenerHelpers
    let mutable token = null
    let mutable mirrorRoot = null
    
    let getProxyUrl (ctx:HttpListenerContext) = 
        Uri(mirrorRoot + ctx.Request.Url.PathAndQuery)
    
    // Funktio käsittelee poikkeukset asyncronisesti
    // Tämä funktio kääntyy muotoon 
    // [1] AsyncBuilder.Delay(fun () -> 
    //    use wr = new StreamWriter(ctx.Response.OutputStream)
    //     [clip]
    //   ctx.Response.Close()
    // Kun tapahtuu virhe:
    // do! asyncHandleError ctx e 
    // kääntyy muotoon 
    //
    // AsyncBuilder.Bind([1], [seurava async operaatio]) (ks. alla)
    //
    // Bind metodi suorittaa operaation jonka Delay on lykännyt. 
    let asyncHandleError (ctx:HttpListenerContext) (e:exn) = async {
       use wr = new StreamWriter(ctx.Response.OutputStream)
       wr.Write("<h1>Request Failed</h1>")
       wr.Write("<p>" + e.Message + "</p>")
       ctx.Response.Close() }

    // AsyncBuilder mahdollistaa asynkronisten opreatioitioiden putkittamisen 
    // minimi määrällä pään rapsutusta. Tässä kokonaisuudessa putkitetaan kolme
    // asynkronista operaatiota. 
    // 1) Sivu pyyntö tulee kun on tullakseen. Kun ase tulee aloitetaan tämän koodi blokin suoritus.
    // 2) Ladataan asynrkonisesti data verkosta.
    // 3a) kun lataus byte[] on käytetävissä kirjoitetaan data asynknoisesti kuunneltavan konteksin 
    // tulostevirtaan 
    // 3b) jos homma meni kaikki meni pieleen niin kirjoitetaan virhe ja tuli poikkeus. Kirjoitetaan virhe tulosvirtaan.
    // "let! data = [...]" ja "do! ctx.Response.OutputStream.AsyncWrite(data)" putkittaa opreaatio automaattisesti siten että 
    // jälkimmäinen suoritetaan vasta kun ensimmäinen palauttaa arvon. Sovellus ei jää odottamaan että näin tapahtuu.
    // saman voisi toteuttaa myös ilman syntaktista sokeria ja lopputulos näyttäisi tämän tapaiselta.
    // let async1 = new AsyncBuilder()
    // [...]
    // async1.Delay(fun () ->
    //    let wc = new WebClient()
    //    try
    //      async1.Bind(wc.AsyncDownloadData(getProxyUrl(ctx)),(fun data ->
    //           async1.Bind(ctx.Response.OutputStream.AsyncWrite(data),(fun () ->
    //                  async1.Return()))
    //    catch e -> 
    //         async1.Bind(asyncHandleError ctx e, async1.Return()
    //
    // F#:n async laskentailmaus (computational expression) tekee asynknonisesta ohjelmoinnista huomattaasti 
    // helpompaa ja asynkronisesta koodista huomattaasti helppolukuisempaa ja tiiviimpää.
    let asyncHandleRequest (ctx:HttpListenerContext) = async {
        let wc = new WebClient()
        try
            let! data = wc.AsyncDownloadData(getProxyUrl(ctx))
            do! ctx.Response.OutputStream.AsyncWrite(data) 
        with e ->
            do! asyncHandleError ctx e }
    
    let StartMirroring (url) =
        // Start HTTP proxy that handles requests asynchronously
        mirrorRoot <- url
        token <- HttpListener.Start("http://localhost:8080/", asyncHandleRequest)
    let Stop () = 
        if (token <> null) then token.Cancel()

module SyncVersion =
    open System
    open System.IO
    open System.Net
    open HttpListenerHelpers
    let mutable token = null
    let mutable mirrorRoot = null
    
    let getProxyUrl (ctx:HttpListenerContext) = 
        Uri(mirrorRoot + ctx.Request.Url.PathAndQuery)
    // Handle exception - generate page with message
    let handleError (ctx:HttpListenerContext) (e:exn) =
       use wr = new StreamWriter(ctx.Response.OutputStream)
       wr.Write("<h1>Request Failed</h1>")
       wr.Write("<p>" + e.Message + "</p>")
       ctx.Response.Close()

    let handleRequest (ctx:HttpListenerContext) =
        let wc = new WebClient()
        try
          let data = wc.DownloadData(getProxyUrl(ctx))
          ctx.Response.OutputStream.Write(data, 0, data.Length)
          ctx.Response.Close()
        with e ->
         handleError ctx e
 
    let StartMirroring (url) =
        // Start HTTP proxy that handles requests asynchronously
        token <- HttpListener.StartSynchronous("http://localhost:8080/", handleRequest)
    let Stop () = 
        if (token <> null) then token.Cancel()

(*
ASyncLister.StartMirroring "http://msdn.microsoft.com"
ASyncLister.Stop()

SyncVersion.StartMirroring "http://msdn.microsoft.com"
SyncVersion.Stop()
*)