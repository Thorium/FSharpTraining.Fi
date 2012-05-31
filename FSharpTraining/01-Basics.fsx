namespace FSharpTarinin.Basics
module M1_HelloFSharp = 
     (*
     1. Mikä on F# ja miksi käyttäisin sitä?

    Mitä F# on?
        * F#  on multiparadigma-ohjelmointikieli, jonka painopiste on funktionaalisessa ohjelmointiparadigmassa.
        * Yksi kolmesta Visual Studion mukana tulevista kielistä.
    
    Miksi?
        - F# on ilmaisuvoimainen ja syntaksiltaan tiivis kieli. 
            * Vaikka F# vahvasti tyypitetty kieli, sen monet rakenteet muistuttavat keveydessään dynaamisesti tyypitettyjä skripti kieliä (kuten Python ja Ruby).
            * F# koodin rivimäärä on usein 2-5 alhaisempi kuin vastaavan C#-ratkaisun. Koodin luettavuus on samaa tasoa kuin C#- tai Java-koodin - joskin eri asiat ovat 
              helppolukuisia/helppolukuisia. F#:ssa on enemmän kryptisiä lynenteitä ja operaattoreja, siinä missä Javassa ja C#:ssa on enemmän avainsanoja ja idiomeja 
              muistavaksi. (lukijan näkemys)
            * F# on abstraktiotasoltaan korkeampi kieli kuin C#. Vastaavasti kuin C# on abstraktiotasoltaan korkeampi kieli kuin CIL/assembler.
            * F# ohjaa oletuksena sivuvaikutuksettomaan koodaukseen. Muutoinkin F# antaa huomattavasti näkemystä siitä *miksi* tehdä asioita myös 
              muissa kielissä jollain tavalla. 
            * Interactive-ympäristön (REPL-loop) avulla kehittäjä pääsee keskittymään ongelman ytimeen samalla kun testaa koodiaan.
        - C:n suorituskykyprofiili, eli vain hieman (=selvästi alle kymmenen kertaa) hitaampi kuin hyvin optimoitu C-koodi, siten suunnilleen 
              yhtä tehokas kuin Java ja C#. Dynaamiset, skriptikielet (kuten PHP, Python, ja Ruby) ovat 10-1000-kertaa optimoitua C-koodia hitaampia.
              Ero korostuu laskentaintensiivisissä tehtävissä.
        - Kaikki .NET-frameworkin luokkakirjastot ovat käytettävissä. F# on .NET-kieli ja kääntyy CIL:ksi (kuten C# ja VB.NET; CIL = Common intermediate language, 
              AKA Microsoft Intermediate lanuguage (MSIL)).

    Missä? 
        - F# soveltuu parhaiten Business-logiikan ja datankäsittelyn toteuttamiseen. (Ks. Don Symen esitys F# 3.0: data, services, Web, cloud, at your fingertips 
          http://channel9.msdn.com/Events/BUILD/BUILD2011/SAC-904T)
        - F#:lla voi toteuttaa myös käyttöliittymäkerroksen ja tiedonesityskerroksen. WebSharper on kiinnostava esimerkki siitä, kuinka käyttöliittymän voi toteuttaa 
          funktionaalisen ohjelmointi padigman hengessä (http://websharper.com/home).
     
     Esimerkki-koodi?

     Helpoin tapa suorittaa alla oleva koodi ja katsoa mitä tapahtuu on mennä ensimmäselle koodi riville ja painaa Alt-ä 
     (Jos maalaat ja suoritat koodin (alt-enter), ota rivien alut mukaan, sillä sisennyksellä on F#:ssa oletuksena merkitystä).
     
     Esim. siirry alle olevalle riville "System.Console.Beep ()" ja paina alt+ä. Jos äänet ovat päällä pitäisi kuulua "beep".
     *)
     System.Console.Beep ()

     // Alla oleva merkkijono tulostuu F#-interaktiven tulosjonoon muodossa: 
     // val it : string = "Hello world"'
     "Hello world"

     // Klassinen hello world ikkunalla. 
     // Apufunktiot show ja withControl helpottavat koodin suorittamista rivi riviltä
     open System.Windows.Forms
     let form = new Form()
     form.Controls.Add(new Label(Text = "Hello world!"))
     form.Show()

     
     // Klassinen "Hello, nimi!" muunnos Hello Worldista.
     let form2 = new Form()
     let question = new Label(Dock = DockStyle.Top, Text = "Kuka olet?") 
     let namefield = new TextBox(Dock = DockStyle.Top);
     let hello = new Label(Dock = DockStyle.Top); 
     namefield.KeyUp.Add(fun e-> hello.Text <- match namefield.Text with "" -> "" | text -> "Hello, " + text + "!")
     form2.Controls.AddRange [|hello; namefield; question|]
     form2.Show()

     // F# interactiven sisällä voi muokata luotu lomaketta "lennossa".
     let beep () = for i = 0 to 5 do System.Console.Beep()
     namefield.KeyUp.Add(fun e-> hello.Text <- match namefield.Text with | "" -> "" | "test" | "Test" -> beep (); "Syötä oikea nimi!" | text -> "Hello, " + text + "!")

module M2_TunnisteetJaLiteraalit = 
    (*
    2. Tunnisteet, primitiivityypit ja literaalit

    Teemat:
     - let
     - literaalit
     - vahvatyypistys
    *)

    // F# tunnisteet esitellään let-avainsanalla, tunnisteet tyypin näkee viemällä hiiren kursori nimen päälle
    // Koodin suorittaminen tapahtuu maalaamalla teksti ja painamalla alt+enter. (Valikoiden kautta saa aikaan saman.)
    let x = 1 
    let y = 1.0 
    let str = "merkkijono"

    // Useat muuttujat esitellään eri tavalla kuin C#:ssa:
    let a, b, c = "a", "b", "c"

    let list = [1;2;3]
    let array = [|1;2;3|] 
    // lista on immutable F#-lista, array (tai paremminkin sequence) on .NET yhteensopiva IEnumerable<T>

    // Siinä missä listat ovat n kappaletta yhtä tyyppiä, niin tuple on yksi kappale n:ää tyyppiä:
    let tupple = (1,"a",0.4)
    
    // Sulut ovat vapaaehtoisia:
    let tupple2 = 1,"k"
    // Ja tuplen voi purkaa kivasti:
    let eka, toka = tupple2
    
    // Välimerkkejä voi käyttää, jos haluaa pitkiä muuttujia:
    let ``tämä muuttuja vaatii välimerkkejä ja on pitkä, mutta selkeä`` = "juttu"


    // Oletuksena koodi on sivuvaikutuksetonta "immutable"-koodia. 
    // Ohjelmistojen bugit johtuvat usein siitä, että jokin muuttuja ei ole siinä tilassa missä oletetaan.
    // Tämän takia muuttujien käyttöä on syytä välttää. Tee mieluummin uusi vakio.

    // Oletuksena kaikki ovat vakioita. Muuttujat pitää erikseen merkata mutable arvain sanalla 
    // = merkki tarkoittaa yhtäsuuruutta aina paitsi asetettaessa tunnisteeseen arvo. Muuttujaan sijoitus tapahtuu <- operaattorilla
    x = 1 // palauttaa arvon true. x:n arvo ei muutu.
    let mutable z = 1
    z = 1 // true

    // Muuttujan arvon muuttamiseen käytetään '<-' operaatoria. =-operaattori käyttäytyy kutakuinkin niin kuin matemaatikko voisin sen olettaa 
    // käyttäytyvän: Aino tapaus jossa sitä käytetään arvon asettamiseen on tunnisteen esittely. Aina muulloin = tarkoittaa yhtä suuruus vertailua.
    // F#:ssa ei ole ==-operaattoria, mikä saattaa hämmentää C#- ja Java-koodaajaa. 
    z <- 2  
    z = 1 // false, koska arvo on nyt 2

    // Interaktiven tulosvirtaan voi kirjoittaa komennolla printf printfn (jälkimmäinen kirjoittaa perään rivin vaihdon).
    // printtaus metodi tekee tyyppitarkastuksen
    // %d = kokonaisluku
    // %f = liukuluku
    // %s = merkkijono
    // %O = objekti 
    // %A = taulukko"
    printfn "%d %f %s %O %A" x y str list array 
    // (tai .NET-perinteisesti System.Console.WriteLine)

module M3_Funktiot = 
    (*
    3. Funktiot

    Teemat
     - funktiot ovat ensimmäisen luokan kansalaisia
     - currying
     - rekursio
    *)

    // Funtitiot esitellään täsmälleen samoin
    // function tyyppi on int->int->int
    let plus x y = x + y
    plus 1 2

    // Yksi kiinnostavimmista ominaisuuksista on funktioiden ketjutus
    // Idean hahmottaa helpoiten sulkeistamalla. Kun ensimmäisen int:n korvaa 5:llä tyypiksi int->int (itse mielessäni usein sulkeistan tähän malliin int->(int->(int)). 
    // Kun sijoitan ensimmäisen "slottiin" numeron 5 (5->(int->(int)), saan funktion int->int kuin sijoitan seuraavan slottiin 5 saan kokonaisluvun int.
    let lisää_viiteen = plus 5

    // Sijoittamalla ensimmäiseksi arvoksi 5 funktioon tyyppiä int->int, jäljelle jää int, joka itse asiassa saadaan suorittamalla funktio loppuun asti.
    lisää_viiteen 5
    lisää_viiteen 7

    // Tämä on konseptina suurempi abstraktio kuin C# optionaaliset parametrit, ja mahdollistaa paremman laiskan evaluoinnin.
    

    // Rekursiivisen function esittelyyn pitää lisätä rec (pitkälti F# vahvan tyypityksen takia): 
    // Rekursiolla ei ole vaikutusta funktion tyyppiin (eli se ei vaihdu).

    let rec fibs a b = 
        match a + b with c when c < 10000 -> c :: fibs b c | _ -> [] 
    let fibonacci = 0::1::(fibs 0 1) 

    // Huomaa funktion voi määritellä myös toisen funktion sisään. Rekursiivisten functioden osalta tämä on näppärä sääntö.
    let rec factorialPlus x =
        let rec factorialRec (x:int64) acc =
            if x > 1L then factorialRec (x - 1L) (x + acc)
            else acc
        factorialRec x 1L    

    factorialPlus 5L // 5 + 4 + 3 + 2 + 1 = 15
    factorialPlus 20000000L // val it : int64 = 200000010000000L

    /// Käytettäessä häntärekursiota F# hanskaa tilanteen jossa pino vuotaisi muuten yli (Stackoverflow). Häntärekursioksi kutsutaan rekursion erityistapausta, 
    /// jossa rekursiivisen kutsun paluuarvosta tulee ilman lisäoperaatioita kutsuvan instanssin paluuarvo. Tämä tarkoittaa sitä, että rekursioiden "purkautuessa" 
    // ei ole enää mitään tekemistä.
    let rec factorialPlusNonTail (x:int64) = 
        if(x > 1L) then
            // rekursiivisen kutsun jälkeen pitää lisätä sen palauttama 
            // arvo x:än ja sitten vasta palautetaan arvo. Koska yhteenlasku tapahtuu rekursiivisen 
            // kutsun palautettua arvon, kyseessä ei ole häntärekursio.
            x + (factorialPlusNonTail (x - 1L))
        else x
    factorialPlusNonTail  5L // 5 + 4 + 3 + 2 + 1 = 15
    factorialPlusNonTail  20000000L // Process is terminated due to StackOverflowException.

module M4_PatternMatchin = 
    (*
    4. Pattern matching vaihtoehtona if-elselle 
     - swich case rakenne tehtynä oikein
     - F#:ssa erittäin ilmaisuvoimainen rakenne
     - http://msdn.microsoft.com/en-us/library/dd547125.aspx
    *)

    // Pattern matchingiä voi käyttää samaan tapaan kuin if-else sykliä voisi. 
    // Suoritus vertaa kaikkea ensimmäiseen arvoon, ja palauttaa tuloksena palautuksen _ tarkoittaa "mikä tahansa"
    let barDay = 
        match System.DateTime.Now.DayOfWeek with
        | System.DayOfWeek.Friday -> "bar?"
        | System.DayOfWeek.Saturday -> "bar!"
        | loput -> "no bar"
    
    // Lopputulos on usein helppo lukuisempi ja tiiviimpi:
    //          |
    //     1    |     2
    //          |
    //  ---------------------
    //          |
    //     3    |     4
    //          |
    let resolveQuartile point =
        match point with
        | (x,y) when x >= 0 && y >= 0 -> 2
        | (x,y) when x < 0 && y >= 0 -> 1
        | (x,y) when x >= 0 && y < 0 -> 4
        | _ -> 3

    resolveQuartile (1,2)
    resolveQuartile (-1,2)
    resolveQuartile (1,-2)
    resolveQuartile (-1,-2)

    // F# Pattern matching tukee kymmenkuntaa eri "hahmontunnistus-kaavaa". Esimerkiksi parametrina annetun objektin tyyppi.
    // Esimerkki 2:
    let format (o: obj) =
        match o with
        | :? System.DateTime as day -> "Päivämäärä: " + day.ToString("dd.MM")
        | :? System.Int32 as i -> "Kokonaisluku: " + i.ToString()
        | _ -> "Jokin muu objekti"

    format (System.DateTime.Now)
    format 1
    format 1.0

    //Aktiiviset patternit: Haarautumis-ehto voidaan irrottaa kontekstistaan (vähän kuten Clojure multi-methods ("polyphormism ala carte"):
    let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd
    let TestNumber input = match input with Even -> printfn "%d is even" input | Odd -> printfn "%d is odd" input
    TestNumber 4
    TestNumber 7


module M5_TyypitJaObjektiOrientoitunutOhjelmointi = 
    (*
    5. Tyypit 
    - Luokat ja rajapinnat
    - Vaihtoehdot luokille ja rajapinnoille
        - Tietue tyyppi (Record type)
        - Jäsennetty yhdiste (Discriminate union) ja Pattern matching
        - Objekti ilmaukset (Object expression)
    *)

    // F# on täysiverinen olio orientoitunut ohjelmointi kieli, joskin sen rakenteet kannustavat hyödyntämään muunlaisia 
    // rakenteita luokkia ja rajapintoja. Itse asiassa luokat ja rajapinnat eivät välttämättä ole edes paras mahdollinen 
    // lähtökohta uudelleenkäytettävälle ja elegantille olio-orientoitunutta koodille.

    // Alla oleva esimerkki havainnollistaa kuinka klassinen validointi dekoraattori on mahdollista toteuttaa olio 
    // orientoituneesta käyttäen luokkia ja rajapintoja ja suunnilleen yhtä olio-orientoituneesti käyttämättä suoranaisesti 
    // kumpaakaan em. rakenteista.

    // Luodaan ensin vertailun vuoksi validointi logiikka OOP paradigman mukaisesti hyödyntäen rajapintoja ja luokkia.
    type IValidateInt = 
        abstract Validate: int -> bool 

    type LessThanValidator (max) =
        let max = max
        interface IValidateInt with 
            member x.Validate (intToValidate) = intToValidate < max
        
    type GreaterThanValidator (min) =
        let min = min
        interface IValidateInt with 
            member x.Validate (intToValidate) = intToValidate > min

    let lessThan10Validator = new LessThanValidator(10) :> IValidateInt
    let moreThan5Validator  = new GreaterThanValidator(5) :> IValidateInt
    lessThan10Validator.Validate 9 // true
    lessThan10Validator.Validate 12 // false
    moreThan5Validator.Validate 2 // false
    moreThan5Validator.Validate 8 // true

    // Rajanpinnan käyttö tekee rakenteesta asteen joustavammat, mutta koodia pahaisen ominaisuuden määrittelyyn 
    // tarvitsee kirjoittaa enemmän kuin itse sovellus logiikkan monimutkaisuus soisi. 
    // F# jäsennelty unioni (discrimate union) tuo ratkaisun tähän. Oheinen ratkaisu on huomattavasti monipuolisempi mutta ei juurikaan monimutkaisempi tai pidempi.
    // Tarkasti ottaen jäsennelty unioni kääntyy abstraktiksi luokaksi, jolla on sen itsensä periviä sisäluokkia. Perinteisesti ymmärrettynä se ei ole luokka vaan jotain muuta.
    type ValidateInt =
        | GreaterThan of int
        | LessThan of int
        | Predicate of (int -> bool)
        | And of ValidateInt * ValidateInt
        | Or of ValidateInt * ValidateInt
        member x.Validate intToValidate =
            match x with
            | GreaterThan max -> intToValidate > max
            | LessThan min -> intToValidate < min
            | Predicate validationFunction -> validationFunction intToValidate
            | And (validator1, validator2) -> (validator1.Validate intToValidate) && (validator2.Validate intToValidate)
            | Or (validator1, validator2) -> (validator1.Validate intToValidate) || (validator2.Validate intToValidate)

    let lessThan10Validator_2 = LessThan  10 
    let moreThan10Validator_2  = GreaterThan  10 
    lessThan10Validator_2.Validate 9 // true
    lessThan10Validator_2.Validate 12 // false
    moreThan10Validator_2.Validate 9 // false
    moreThan10Validator_2.Validate 12 // true

    // Ja sitten jotain ihan muuta. Joko parillinen ja yli 10 tai pariton ja alle kymmenen
    let complexValidator1 = 
        let isOddValidator = ValidateInt.Predicate (fun x -> (x % 2) = 1)
        let isEvenValidator = ValidateInt.Predicate (fun x -> (x % 2) = 0)
        Or (And ((GreaterThan 10), isEvenValidator), (And ((LessThan 10), isOddValidator)))  
    complexValidator1.Validate 9 // true
    complexValidator1.Validate 13 // false
    complexValidator1.Validate 8 // false
    complexValidator1.Validate 12 // true

    // Ratkaisu toimii mutta syntaksin edellyttämä sulkuhässäkästä on erittäin vaikea lukuinen.
    // F# sallii luettavuutta helpottavien prefix ja infiksi operaattorien luonnin.
    // Harmittavasti CLI (Common Language Infrastructure) ei anna määrittää && tai || operaattoreja 
    // jotka palauttavat jotain muuta kuin booleanin. +& ja +| ovat hieman kryptisiä nimiä. 

    // On myös syytä huomata että F# sallii luokan määrittämisen ja laajentamisen partiaalisesti jopa sen jälkeen 
    // kun siitä on luotu instansseja. Tässä ValidateInt luokkaa laajennetaan "lennosta" kahdella staattisella jäsenellä.
    // Koodi on kuitenkin edelleen vahvasti tyypitettyä, sillä tätä tyyppi laajennusta ei hyödynnetä tätä ennen.
    type ValidateInt with
        static member (+&) (fst:ValidateInt, snd : ValidateInt) =
            ValidateInt.And (fst,snd)
        static member (+|) (fst:ValidateInt, snd:ValidateInt) =
            ValidateInt.Or (fst,snd)

    let complexValidator2 = 
        let isOddValidator = ValidateInt.Predicate (fun x -> (x % 2) = 1)
        let isEvenValidator = ValidateInt.Predicate (fun x -> (x % 2) = 0)
        (GreaterThan 10 +& isEvenValidator) +| (LessThan 10 +& isOddValidator)
    complexValidator2.Validate 9 // true
    complexValidator2.Validate 13 // false
    complexValidator2.Validate 8 // false
    complexValidator2.Validate 12 // true

module M6_LoopitJaListaOperaatiot = 
    (*
    5. Loopit ja lista operaatiot
     - for
     - komentojen putkitus (pipelines)
     - Seq ja List -operaatiot
    *)


    // Klassinen for loopi toimii näin:
    for i = 1 to 3 do printfn "Jee %d" i
    for i = 3 downto 1 do printfn "Jee %d" i

    // C#:n foreachia vastaava rakenne näyttää F#:ssa tältä. 
    let simpleList = [1;2;3]
    for i in [1;2;3] do printfn "Jee %d" i

    // Usein for loopeja tehokkaampaa ja elegantimpaa on putkittaa komentoja. |> operaattorilla
    // (Yksinkertaisissa tapauksissa tosin putkitus ei tosin selvennä juurikaan koodia.)
    let anotherList = [1;2;3]
    anotherList  |> List.iter (printfn "Jee %d") 

    // For looppia voi käyttää listojen generointiin. 
    open System
    let firstday2012 = new System.DateTime(2012,1,1)
    let year2012 = seq {for i in 0.0 .. 365.0 -> firstday2012.AddDays(i)}
    
    // Kun loopin logiikka monimutkaistuu, komentojen putkitus alkaa selkeyttämään koodia enevässä määrin.
    // Funktionaalinen ohjelmointiparadigma on vahvimmillaan nimenomaan monimutkaisten ongelmien parissa puuhatessa.
    year2012 
    |> Seq.filter (fun day -> day.DayOfWeek = DayOfWeek.Friday && day.Day = 13)
    |> Seq.iter (printfn "%O")

    // Seuraavassa esimerkissä näytetään tämän vuoden perjantai 13. -päivät ja milloin vastaava päivämäärä on perjantai seuraavan kerran.
    let rec findNextSameFriday13 (day : DateTime) =
        let next = new DateTime(day.Year + 1, day.Month, day.Day)
        match next.DayOfWeek with
        | DayOfWeek.Friday -> next
        | _ -> findNextSameFriday13 next
    
    year2012 
    |> Seq.filter (fun day -> day.DayOfWeek = DayOfWeek.Friday && day.Day = 13)
    |> Seq.map (fun day -> (day, findNextSameFriday13 day))
    |> Seq.iter(fun (day2012, dayN) ->  (printfn "%O on seuraavan kerran perjantai 13. vuonna %d" day2012 dayN.Year))

    // F# sallii äärettömät sekvenssit. Seuraava funktio etsii ensimmäisen perjantaina joka on kolmastoista päivä ja jolle 
    // vuosi * kuukausi * päivä on suurempi kuin 1 000 000. Esim. jos 13.12.2012 olisi perjantai vuoden, kuukauden ja päivän tulo olisi 313872

    // 1. Ensin koodia selventävä apufunktio seuraavan kolmannentoista päivän hakemiseen. 
    let rec findNextFriday (day : DateTime) =
        let next = day.AddDays(1.0)
        match (next.Day, next.DayOfWeek) with
        | 13, DayOfWeek.Friday -> next
        | _ -> findNextFriday next

    // 2. Tämän jälkeen luodaan ääretön sekvenssi perjantai kolmastoista päiviä
    let fri13seq = Seq.unfold (fun state -> Some(state, (findNextFriday state))) (new DateTime(2012,1,13))
    // 3. Lopuksi iteroidaan sekvenssiä läpi kunnes ehdot täyttävä päivä löytyy.
    fri13seq |> Seq.skipWhile (fun day -> day.Year * day.Month * day.Day < 1000000) |> Seq.head