module M7_MuutaDotNetSälää = 
    (*
    7. .NET yleisiä toiminnallisuuksia
     - Resurssien käyttö
     - Virheiden käsittely
     - Attribuutit
    *)

    //Resurssien käyttö:
    // c# using System.jotain käsky on open. Mutta IDisposable using { ... } on use:

    open System.IO
    let readFirstLine filename =
        use file = File.OpenText filename
        //...
        file.ReadLine() 

    // Virheitä ei yleensä kannata käsitellä. Ohjelmakoodia ei saisi rakentaa virheiden varaan.
    // Joskus ulkoisten rajapintojen virheet on kuitenkin hyvä ottaa logille.
    try
        failwith("Error!")
    with
    | :? System.DivideByZeroException -> "Should not happen..."
    | x -> raise x
     
    // Attribuutit merkitään [<...>], esim. Win32 API Interop olisi näin:
    //    [<DllImport("User32.dll", SetLastError=true)>]
    //    extern bool MessageBeep(UInt32 beepType);
    //
    //    let InterOpSample1() = 
    //        let r = MessageBeep(0xFFFFFFFFu)
    //        printfn "message beep result = %A" r


module M8_Syvemmälle = 
    (*
    8. "Advanced juttuja"
     - Event-käsittely
     - Quotations
     - Numeric literals
     - Builder pattern (computational expressions, monads)
     - Agents: Mailbox processor
    *)

    // Oletuksena F# tukee reaktiivista ohjelmointia, ja sitä voi laajentaa halutessaan esim. Microsoft Reactive Extensionilla
    // Tässä eventit ovat ikuinen laiska lista, joka täyttyy sitä mukaa kun tapahtumia tulee, ja siitä filtteröidään kiinnostavat...
    let myEvent = new Microsoft.FSharp.Control.Event<int>()

    let myListener = 
            myEvent.Publish 
            |> Event.filter(fun i -> i % 2 = 0)
            |> Observable.add(fun x -> printfn "Kuulin %d" x)

    [0 .. 5] |> List.iter (fun i -> myEvent.Trigger i)
    //    Kuulin 0
    //    Kuulin 2
    //    Kuulin 4


    // Quotations, voidaan siirtää F#-kieltä päättelypuina esim. muihin kieliin.
    let tyypitettyExpressio = <@ 1 + 1 @>
    let tyypittämätönExpressio = <@@ 1 + 1 @@>
    let f x = x+1
    let example = <@ f @>

    // Numeric literals (varatut: Q, R, Z, I, N, G), DSL-kieliin, kun halutaan tehdä omat aritmeettiset operaatiot.
    // Usein käytetään määrittelemällä ensin oma jäsennelty unioni ja ylikuormittamalla sen metodeita
    // (siten sopisivat mainiosti myös ylempänä olleeseen validointi-esimerkkiin).
    module NumericLiteralN =
        let FromZero() = ""
        let FromOne() = "."
        let FromInt32 x = String.replicate x "."

    // Calls FromOne():
    let x1 = 1N 
    // val x1 : string = "."

    // Calls FromInt32(7):
    let x2 = 7N
    // val x1 : string = "......."

    //Calls operator (+) on strings.
    let x3 = 2N + 3N
    // val x3 : string = "....."


    // Builder pattern (computational expressions, monads)

    // F#:ssa on oletuksena muutamia aihealueita/konteksteja, joiden toiminnallisuutta voi ohjelmoida normaaliin tapaan, tietämättä 
    // miten ne oikeasti toimivat. Yksi tälläinen on aiemmin esillä ollut seq { ... } ja toinen on asynkroninen async { ... }
    // Näitä voi tehdä itse lisää, nyt näytetään miten. 

    // Kapseli, vastaava kuin Async<T>, IEnumerable<T>, jne:
    type SataKertainen(n:int) =
        member x.Arvo = n
        with //Sivuvaikutus vasta kun arvo haetaan ulos tyypistä:
            override x.ToString() = (string)(x.Arvo * 100)

    // Konteksti: 
    type KontekstiBuilder() =        
        member t.Return(x) = SataKertainen(x)
        member t.Bind(x:SataKertainen, rest) = 
            printfn "Kurkattiin %d" x.Arvo 
            rest(x.Arvo)

    let konteksti = KontekstiBuilder()

    let test =
        konteksti{
            let! a = SataKertainen(3) //"let!" kutsuu builderin Bind
            let! b = SataKertainen(5) //"let!" kutsuu builderin Bind

            //Kontekstin sisällä ohjelmoidaan välittämättä SataKertaisista:
            let mult = a * b   
            let sum = mult + 1 

            return sum //"return" kutsuu builderin Return(x)
        }

    // Kontekstin sisällä huutomerkki-käskyt ("syntaktisokeria") ohjaavat builder-"rajapinnan" vastaaviin metodeihin.
    // "Rajapinnasta" ei tarvitse täyttää kuin oleelliset metodit. Homma perustuu continuationiin (tarkemmin: call-cc) ja reify:yn.
    // Tarkempi kuvaus niistä ja rajapinnan sisäisestä toiminnasta löytyy netistä.
    // http://msdn.microsoft.com/en-us/library/dd233182.aspx
    // http://blogs.msdn.com/b/dsyme/archive/2007/09/22/some-details-on-f-computation-expressions-aka-monadic-or-workflow-syntax.aspx



    // MailboxProcessor, perustuu Agents-ohjelmointimalliin, vähän kuin oma async tausta-thread ylläpitämään tilaa.
    // Ideana, että voidaan synkronoida asioita ilman lukkoja tai sivuvaikutuksia: Agentti ei paljasta mutable-muuttujia ulos.
    open System

    type Metodit =
    | Lisää of string
    | HaeKaikki of AsyncReplyChannel<string list>

    type Varasto() =
        let varasto = MailboxProcessor.Start(fun komento ->
            let rec msgPassing kaikki =
                async { let! k = komento.Receive()
                        match k with
                        | Lisää(juttu) ->

                            return! msgPassing(juttu :: kaikki)
                        | HaeKaikki(reply) ->

                            reply.Reply(kaikki)
                            return! msgPassing(kaikki)
                }
            msgPassing [])

        member x.Tallenna item =  
            item |> Lisää |> varasto.Post
            "saved"

        member x.Inventaario() = 
            async {
                let! tuotteet = varasto.PostAndAsyncReply(fun rep -> HaeKaikki(rep))
                tuotteet |> List.rev |> List.iter(printfn "%s")
            } |> Async.Start

    let v = new Varasto()
    v.Tallenna("viivotin") |> ignore
    v.Tallenna("kirja") |> ignore
    v.Inventaario()

