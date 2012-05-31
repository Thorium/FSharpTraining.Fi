namespace FSharpTarinin.Basics
module M1_HelloFSharp = 
     (*
     1. Mikä on F# ja miksi käyttäisin sitä?

    Mitä F# on?
        * F# on multiparadigma-ohjelmointikieli, jonka painopiste on funktionaalisessa ohjelmointiparadigmassa.
        * F# on yksi kolmesta Visual Studion mukana tulevista kielestä. 
        * F#:n juuret ovat OCaml ja ML-kielessä. Myös Haskell on keskeinen vaikute. (Ks. http://www.cs.helsinki.fi/u/poikelin/FSharp/taustaa.html)
    
    Miksi?
        - F# on ilmaisuvoimainen ja syntaksiltaan tiivis kieli
            * Vaikka F# on vahvasti tyypitetty kieli, sen monet rakenteet muistuttavat keveydessään 
              dynaamisesti tyypitettyjä skripti kieliä (kuten Python ja Ruby). Interactive-ympäristön 
              (REPL-loop) avulla kehittäjä pääsee keskittymään ongelman ytimeen samalla kun 
              testaa koodiaan.
            * F# koodin rivimäärä on usein 2-5 alhaisempi kuin vastaavan C#-ratkaisun. Koodin luettavuus 
              on samaa tasoa kuin C#- tai Java-koodin - joskin eri asiat ovat helppolukuisia/helppolukuisia. 
              F#:ssa on enemmän kryptisiä lynenteitä ja operaattoreja, siinä missä Javassa ja C#:ssa on enemmän 
              avainsanoja ja idiomeja muistavaksi.
            * F# on abstraktiotasoltaan korkeampi kieli kuin C#. Vastaavasti kuin C# on abstraktiotasoltaan korkeampi 
              kieli kuin CIL/assembler.
            * F# ohjaa oletuksena sivuvaikutuksettomaan koodaukseen. Muutoinkin F# antaa huomattavasti näkemystä 
              siitä *miksi* tehdä asioita myös muissa kielissä jollain tavalla. 
        - F#:lla on C:n suorituskykyprofiili.  Toisin sanoen se on vain hieman (=selvästi alle kymmenen kertaa) hitaampi 
            kuin hyvin optimoitu C-koodi, ja siten suunnilleen yhtä tehokas kuin Java ja C#. Dynaamiset, skriptikielet 
            (kuten PHP, Python, ja Ruby) ovat 10-1000-kertaa optimoitua C-koodia hitaampia. Ero korostuu 
            laskentaintensiivisissä tehtävissä.
        - Kaikki .NET-frameworkin luokkakirjastot ovat käytettävissä. F# on .NET-kieli ja kääntyy CIL:ksi (kuten C# ja 
            VB.NET; CIL = Common intermediate language; AKA Microsoft Intermediate lanuguage (MSIL)).

    Missä? 
        - F# soveltuu parhaiten Business-logiikan, monimutkaisten algoritmien ja datan käsittelyn toteuttamiseen. 
            (Ks. Don Symen esitys F# 3.0: data, services, Web, cloud, at your fingertips http://channel9.msdn.com/Events/BUILD/BUILD2011/SAC-904T)
        - F#:lla voi toteuttaa myös käyttöliittymäkerroksen ja tiedonesityskerroksen. WebSharper on kiinnostava esimerkki siitä, kuinka käyttöliittymän voi toteuttaa 
            funktionaalisen ohjelmointi padigman hengessä (http://websharper.com/home).
     
    Käytännössä?
        - Tämä esitys on laadittu mahdollisimman käytännön läheiseksi sukellukseksi F#:n syntaksiin. Suurin osa esityksestä 
          onkin koodia.
        - Helpoin tapa suorittaa alla oleva koodi ja katsoa mitä tapahtuu on mennä ensimmäselle koodi riville ja painaa Alt-ä.
          Alt-ä suorittaa vain yhden rivin ja usein ilmaus jatkuu toisella rivillä. Tälläin maalaat ja suoritat koodi 
          alt-enter-yhdistelmällä. Huom. ota rivien alut mukaan, sillä sisennyksellä *on* F#:ssa merkitystä. 
          (Valikoiden kautta saa aikaan saman.)
     
    Esim. siirry alle olevalle riville "System.Console.Beep ()" ja paina alt+ä. Jos äänet ovat päällä pitäisi kuulua "beep".
    *)
     System.Console.Beep ()

     // Alla oleva merkkijono tulostuu F#-interaktiven tulosjonoon muodossa: 
     // val it : string = "Hello world"'
     "Hello world"

     // Klassinen hello world teksti ikkunassa.
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
     let beep () = for i = 0 to 2 do System.Console.Beep()
     namefield.KeyUp.Add(fun e-> hello.Text <- match namefield.Text with | "" -> "" | "test" | "Test" -> beep (); "Syötä oikea nimi!" | text -> "Hello, " + text + "!")

module M2_LiteraalitJaTunnisteet = 
    (*
    2. Tunnisteet, primitiivityypit ja muutettavuus
    *)

    // F# tunnisteet esitellään let-avainsanalla. 
    // Tunnisteen tyypin näkee viemällä hiiren kursori nimen päälle 
    // Primitiivi tyyppien literaalit ovat pääsääntöisesti samat kuin C#:ssa.
    let x = 1 // 32 bittinen kokonaisluku
    // tunniteen tyypin voi määittää syntaksilla let nimi : tyyppi
    let x2 : System.Int64 = 1L // 64 bittinen kokonaisluku 
    let y : float = 1.0  // System.Double, 64 bittinen liukuluku; C#:ssa double, F# float (HUOM!)
    let y = 1.0f // System.Single, 32 bittinen liukuluku; C#:ssa float, F#:ssa float32 (HUOM!)
    let str = "merkkijono"
    let chr = 'm'
    
    // Välimerkkejä voi käyttää, jos haluaa pitkiä muuttujia. Tosin suomenkielisessä näppiksessä 
    // ` (backtick)löytyy hankalasti yhdistelmällä shift-´-[jokin merkki]:
    let ``tämä tunniste vaatii välimerkkejä ja on pitkä, mutta selkeä`` = "juttu"
    ``tämä tunniste vaatii välimerkkejä ja on pitkä, mutta selkeä``

    // Toisin kuin C#:ssa oletuksena tunnisteet eivät ole muutettavissa (tunniste on ei-muutettava ("immutable")).
    // Muuttujien (muutettavien tunnisteiden) esittely pitää tehdä explisiittisesti käyttäen mutable-avainsanaa.
    // (C#:n oletus arvo on päin vastainen, ei-muutettavan tunnisteen esittelussa pitää käyttää readonly-avainsanaa.)
    let mutable z = 1
    // Muuttujaan sijoitus tapahtuu '<-' -operaattorilla
    z <- 5
    z
    // Yhtäsuuruus merkki ei toimi koskaan (!!!) muuttujaan sijoituksena:
    z = 1 // palauttaa arvon true. x:n arvo ei muutu.
    
    // Matematiikan tapaan yhtäsuuruutta käytetään vain seuraavassa kahdessa merkityksessä:
    // 1) uuden tunnisteen/nimen esittelyyn
    // 2) yhtäsuuruus vertailuissa

    // Tämä voi tuntua aluksi hämmentävältä. Toisaalta: ohjelmistojen bugit johtuvat usein siitä, 
    // että jokin muuttuja ei ole siinä tilassa missä oletetaan. Kun laskentaa aletaan tehdä hajautettusti 
    // usealla ytimellä ongelma räjähtää helposti käsiin. Tässä mielessä ei-muokattavissa olevat tunnisteet 
    // on parempi oletusarvo kuin se, että tunnisteet ovat oletuksena muutettavia.

    // Ei-muutettavuus ei C# näkökulmasta tarkoita sitä, että kyseessä olisi vakio. Tunnisteen arvo ei leivota sisään
    // luokkakirjaston CIL-koodiin; sitä ei vaan voi muuttaa sen jälkeen kun se on kerran asetettu. 
           
    // Itse asissa tarkasti ottaen F# koodissa ei juurikaan käytetä C#-vakiota vastaavaa rakennetta. C#:n:
    // public const int Vakio = 1;
    // on F#:ksi
    [<Literal>]     
    let Vakio = 1
    // Yleistäen ja mutkia oikoen: muuttuja = muutettavissa-oleva tunniste (mutable identifier) ja vakio = ei-muutettava 
    // tunniste (immutable identifier). Ei-muutettava tyyppi (immutable type) on sellainen jonka instanssien tilaa ei 
    // voi muuttaa. Esim. primitiivi tyypit. string ja DateTime. Muutettavissa-olevat tyyppi (mutable type) on 
    // sellainen, jonka instanssien tila voi muokata (suurin osa .NET:n Frameworkin luokista). 
    // Ohjelmoijan näkökulmasta seuraava on käytännössö vakio:
    let Vakio = 1 
                                                                                                    
module M3_Listat =     
    // Listat ja taulukot
    // Toisin kuin C# listat ja taulukot ovat muuttumattomia (immutable). 
    let list = [1;2;3]
    let array = [|1;2;3|] 
    
    // Lista list on F# spesifi linkitettynä listana toteutetta rakenne. Array on C#:n System.Array
    // Huom. vaikka itse array tunniste on merkittu ei-muokattavaksi, System.Array on muokattava
    // näin ollen seuraava on sallittua
    array.[0]<-5
    array
    // Se että tunnisteet asettaa tarkoittaa vain sitä, että po tunnisteen arvo ei voi muuttua: 
    // array <- [|1;2;3|]    // Virhe: This value is not mutable

    // Listan indeksointi on mahdollista
    list.[0]
    // Mutta muokaaminen ei:
    // list.[1] <- 5  // Virhe: "Property Item kannot be set"

    // .NET:n muutettavissa olevia voi toki myös käyttää ja net toimivat normaaliin tapaan. 
    let genericList = System.Collections.Generic.List<string>()
                                                                                       
    // Siinä missä listat ovat n kappaletta yhtä tyyppiä, niin monikko (tuple) on yksi kappale n:ää tyyppiä:
    let tuple = (1,"a",0.4)
    
    // Sulut ovat vapaaehtoisia:
    let tuple2 = 1,"k"
    // Ja monikon (tuplen) voi purkaa kivasti:
    let eka, toka = tuple2
    // Useat muuttujam esittely samalla rivillä hyödyntää monikkoa:
    let a, b, c = "a", "b", "c" 
    


    // Funktio ilman parametreja (vs. sulkuja ilman vakio / property):
    let Action() = 1



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
    // Huomaa, että funktion voi määritellä myös toisen funktion sisään. Rekursiivisten functioden osalta tämä on näppärä sääntö.
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

    // Kääntäjä tyypittää automaattisesti, 'a -> string on funktio joka ottaa geneerisen tyypin sisään ja palauttaa stringin ulos:
    let Method1(input) = 
        input.ToString()
    //val Method2 : 'a -> string
    
    // Yleensä automatiikka toimii hyvin, joten manuaalinen eksplisiittinen tyypitys on usein turhaa.
    // Joskus silti kääntäjä voi tarvita pientä vinkkiä...

    // Tyypittää voi eksplisiittisesti:
    let Method2(input:int) :string = 
        input.ToString()
    //val Method1 : int -> string

    // Generics:
    let Method3(input:'t) =
        input.ToString()

    // Generics .NET-tapaan eksplisiittisesti:
    let Method4<'t>(input:'t) =
        input.ToString()

    // Lista-parametri OCaml-tapaan eksplisiittisesti: 
    let l2 : int list = [1;2;3]

    // Lista-parametri .NET-tapaan eksplisiittisesti:
    // (syntaksi eri, lopputulos käytännössä sama)
    let l1 : list<int> = [1;2;3]

    // Generics .NET-tapaan, tyypillä:
    type myType1<'t> = MyType1 of 't

    // Vastaava Generics OCaml-tapaan:
    // (syntaksi eri, lopputulos käytännössä sama)
    type 't myType2 = MyType2 of 't

    // Käyttö:
    let myThree1 = MyType1("something")
    let myThree2 = MyType2("something")


    // F# on täysiverinen olio orientoitunut ohjelmointi kieli, joskin sen rakenteet kannustavat hyödyntämään muunlaisia 
    // rakenteita luokkia ja rajapintoja. Itse asiassa luokat ja rajapinnat eivät välttämättä ole edes paras mahdollinen 
    // lähtökohta uudelleenkäytettävälle ja elegantille olio-orientoituneelle koodille.
    // Usein tarkempi tekninen implementaatio (rajapinta/luokka/...) ei ole käyttäjälle merkityksellinen.

    // Vertaa noisen määrää, perus C#-luokka:
    //    public class MyClass{
    //       public int Property { get ; private set; }
    //       public MyClass(int property){
    //            Property = property;
    //       }
    //    }

    // F#-luokka:
    type MyClass(property) =
      member x.Property = property

    // instanssin voi tehdä näin:
    let instance1 = MyClass()
    // tai näin:
    let instance2 = new MyClass()

    // F# 3.0 (Visual Studio 11) Auto-property, luokalla on property julkisella getterillä ja setterillä, 
    // sekä konstruktori, joka asettaa propertyn alkuarvoon:

    // type MyClass2(property) =
    //  member val Property = property


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
    //...

    let lessThan10Validator = new LessThanValidator(10) :> IValidateInt
    let moreThan5Validator  = new GreaterThanValidator(5) :> IValidateInt
    lessThan10Validator.Validate 9 // true
    lessThan10Validator.Validate 12 // false
    moreThan5Validator.Validate 2 // false
    moreThan5Validator.Validate 8 // true

    // Tätä voisi jatkaa ylikuormittamalla && ja || operaattorit tuottamaan AndValidator-luokan, jne.
    // Mutta koodin määrä paisuu yli simppelin esimerkin.

    // Voisi tehdä myös anonyymilla luokalla, jos instansseja on vain yksi (Object Expression):
    let lessThan7Validator = { new IValidateInt with member x.Validate(i) = i<7 }

    // Rajanpinnan käyttö tekee rakenteesta asteen joustavamman, mutta koodia pahaisen ominaisuuden määrittelyyn 
    // tarvitsee kirjoittaa enemmän kuin itse sovellus logiikkan monimutkaisuus soisi. 
    // F# jäsennelty unioni (discrimate union) tuo ratkaisun tähän. Oheinen ratkaisu on huomattavasti monipuolisempi mutta ei juurikaan monimutkaisempi tai pidempi.
    // Tarkasti ottaen jäsennelty unioni kääntyy abstraktiksi luokaksi, jolla on sen itsensä periviä sisäluokkia. 
    // Perinteisesti ymmärrettynä se ei ole luokka vaan jotain muuta. (Vähän kuin "joko-tai-luokka".)
    
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

    let lessThan20Validator = LessThan  20 
    let moreThan20Validator  = GreaterThan  20 
    lessThan20Validator.Validate 9 // true
    lessThan20Validator.Validate 22 // false
    moreThan20Validator.Validate 9 // false
    moreThan20Validator.Validate 22 // true

    // Ja sitten jotain ihan muuta. Joko parillinen ja yli 10 tai pariton ja alle kymmenen
    let complexValidator1 = 
        let isOddValidator = ValidateInt.Predicate (fun x -> (x % 2) = 1)
        let isEvenValidator = ValidateInt.Predicate (fun x -> (x % 2) = 0)
        Or (And ((GreaterThan 10), isEvenValidator), (And ((LessThan 10), isOddValidator)))  
    complexValidator1.Validate 9 // true
    complexValidator1.Validate 13 // false
    complexValidator1.Validate 8 // false
    complexValidator1.Validate 12 // true

module M5_TyypitJaObjektiOrientoitunutOhjelmointi_Osa2 = 

    // Ratkaisu toimii mutta syntaksin edellyttämä sulkuhässäkästä on erittäin vaikea lukuinen.
    // F# sallii luettavuutta helpottavien prefix ja infiksi operaattorien luonnin.
    // Harmittavasti CLI (Common Language Infrastructure) ei anna määrittää && tai || operaattoreja 
    // jotka palauttavat jotain muuta kuin booleanin.  (op_BooleanAnd toimisi, mutta return-tyyppi on väärä)
    // Käytössä ovat esim.  &&& ja |||  tai  +& ja +|  mutta molemmat ovat hieman kryptisiä nimiä. 

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
        static member (&&&) (fst:ValidateInt, snd : ValidateInt) =
            ValidateInt.And (fst,snd)
        static member (|||) (fst:ValidateInt, snd:ValidateInt) =
            ValidateInt.Or (fst,snd)

    let complexValidator2 = 
        let isOddValidator = ValidateInt.Predicate (fun x -> (x % 2) = 1)
        let isEvenValidator = ValidateInt.Predicate (fun x -> (x % 2) = 0)
        (GreaterThan 10 &&& isEvenValidator) ||| (LessThan 10 &&& isOddValidator)
    complexValidator2.Validate 9 // true
    complexValidator2.Validate 13 // false
    complexValidator2.Validate 8 // false
    complexValidator2.Validate 12 // true

    // On myös syytä huomata että F# sallii luokan määrittämisen ja laajentamisen partiaalisesti jopa sen jälkeen 
    // kun siitä on luotu instansseja. Tässä ValidateInt luokkaa laajennetaan "lennosta" staattisella jäsenellä.
    // Koodi on kuitenkin edelleen vahvasti tyypitettyä, sillä tätä tyyppilaajennusta ei hyödynnetä tätä ennen.

    type ValidateInt with
        static member Lisäys (fst:ValidateInt, snd : ValidateInt) =
            ValidateInt.And (fst,snd)

    // Tyyppejä voi käyttää aliaksina:
    type dt = System.DateTime

module M6_LoopitJaListaOperaatiot = 
    (*
    5. Loopit ja listaoperaatiot
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

    // seq { ... } ja muihin Computation Expressioneihin (monad) palataan myöhemmin...

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

    // F# lista on linkitetty lista.
    // Listoja voi yhdistellä (merge):
    let merged1to6 = [1;2;3] @ [4;5;6]

    //lisäksi usein käsitellään ensimmäistä alkiota, ja välitetään loput rekursiolle. :: erottaa ensimmäisen alkion ja loput:
    let mylist = "head" :: ["tail1"; "tail2"]

    let rec fibs a b = 
        match a + b with c when c < 10000 -> c :: fibs b c | _ -> [] 
    let fibonacci = 0::1::(fibs 0 1) 