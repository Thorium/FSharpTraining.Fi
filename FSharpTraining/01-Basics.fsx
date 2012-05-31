namespace FSharpTarinin.Basics
module M1_HelloFSharp = 
     (*
     1. Mikä on F# ja miksi käyttäisin sitä?

    Mitä F# on?
        * F#  on multiparadigma-ohjelmointikieli, jonka painopiste on funktionaalisessa ohjelmointiparadigmassa.
        * Yksi kolmesta Visual Studion mukana tulevista kielistä. Se perustuu OCaml-kieleen.
    
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

module M2_TunnisteetJaLiteraalit_Osa2_Lisäkikkailua = 
    
    // Castausta ei tietenkään suositella, koska tyyppit hoituvat itsekseen. 
    // Jos sitä tarvii, niin näin sen voi tehdä:
    let myObject1 = box("juttu")
    let myString1 = unbox<string>(myObject1)
    let myObject2 :obj = upcast "juttu"
    let myString2 :string = downcast myObject2
    let myObject3 = "juttu" :> obj
    let myString3 = myObject3 :?> string
    let integerType = typeof<int>


    // Välimerkkejä voi käyttää, jos haluaa pitkiä muuttujia:
    let ``tämä vakio vaatii välimerkkejä ja on pitkä, mutta selkeä`` = "juttu"

    // Funktio ilman parametreja (vs. sulkuja ilman vakio / property):
    let Action() = 1

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
     - Yhdistetty funktio
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

    //Sama plus voidaan ilmaista näin:
    let plus2 = (+)

    //fun on varattu sana Lambda-expressioille. Sama plus voidaan ilmaista siis myös näin:
    let plus3 = (fun x -> fun y -> x + y)

    // Yhdistetty funktio, function composition
    // Funktioita voidaan yhdistellä, eli jos on funktio h joka kutsuu parametrilla x ensin funktiota f ja sitten funktiota g:
    let h f g x = g(f(x))
    // tämä voidaan merkitä myös:
    let h f g x = (f>>g)x
    // tällöin pääsemme eroon parametrista:
    let h f g = f>>g
    // Tämä mahdollistaa top-down-koodauksen tuntematta parametreja: let prosessoi = tallenna >> validoi >> lähetä
    // Tähän palataan listojen yhteydessä...

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
    let Method1 input = 
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
    //val Method2 : 't -> string

    // Generics .NET-tapaan eksplisiittisesti:
    let Method4<'t>(input:'t) =
        input.ToString()

    // Kääntäjä tekee tarvittaessa inline-funktion kaikkiin missä kyseistä käytetään:
    // (Todella geneerinen mutta suorituskyvyltä huono)
    let inline Method5 input = 
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
    6. Loopit ja listaoperaatiot
     - for
     - komentojen putkitus (pipelines)
     - Seq ja List -operaatiot
    *)

<<<<<<< HEAD
    //             Immutable items  Immutable list  Element lookup
    //F# list        Kyllä!           Kyllä!          O(n), eli O(1) listan alkuun
    //Array          Ei               Kyllä!          O(1)
    //Generic List   Ei               Ei              O(1)

    // Best practice on käyttää listoja immutableina (ei muuttaa arvoja) ja ei viitata suoraa indekseihin.
    // Sen sijaan että poistat listasta alkion, tee uusi lista, josta on suodatettu poistettava alkio.
=======
>>>>>>> remotes/ilmirajat/fsharptraining.fi/master

    // Klassinen for loopi toimii näin:
    for i = 1 to 3 do printfn "Jee %d" i
    for i = 3 downto 1 do printfn "Jee %d" i

<<<<<<< HEAD
    // For looppi voi käyttää listojen generointiin. 
=======
    // C#:n foreachia vastaava rakenne näyttää F#:ssa tältä. 
    let simpleList = [1;2;3]
    for i in [1;2;3] do printfn "Jee %d" i

    // Usein for loopeja tehokkaampaa ja elegantimpaa on putkittaa komentoja. |> operaattorilla
    // (Yksinkertaisissa tapauksissa tosin putkitus ei tosin selvennä juurikaan koodia.)
    let anotherList = [1;2;3]
    anotherList  |> List.iter (printfn "Jee %d") 
    // For looppia voi käyttää listojen generointiin. 
>>>>>>> remotes/ilmirajat/fsharptraining.fi/master
    open System
    let firstday2012 = new System.DateTime(2012,1,1)
    let year2012 = seq {for i in 0.0 .. 365.0 -> firstday2012.AddDays(i)}

    // sequenssi voidaan tehdä myös yield-käskyllä, joka on sama kuin muualla .NET:issä.
    // F# tukee seq-operaatioissa myös yield! joka yieldaa koko setin osaksi paluuarvoa:
    let rec iterate f value = 
      seq { yield value; 
            yield! iterate f (f value) }
    
    let xs = iterate (fun f-> f+1) 0
             |> Seq.take(10) |> Seq.toList
    // val x : int list = [0; 1; 2; 3; 4; 5; 6; 7; 8; 9]

    // seq { ... } ja muihin Computation Expressioneihin (monad) palataan myöhemmin...

    // Usein for loopeja tehokkaampaa ja elegantimpaa on putkittaa komentoja. |> operaattorilla
    // (Yksinkertaisissa tapauksissa tosin putkitus ei tosin vielä selvennä juurikaan koodia.)
    simpleList |> List.iter (printfn "Jee %d") 

    // Koontifunktioiden (Aggregate) "isä" on fold:
    simpleList |> List.fold (+) 0

    // Yksittäiseen alkioon viittaaminen, ei yleensä tarvetta:
    let fourth = simpleList.[3]

    // Kun loopin logiikka monimutkaistuu, komentojen putkitus alkaa selkeyttämään koodia enevässä määrin.
    // Funktionaalinen ohjelmointiparadigma on vahvimmillaan nimenomaan monimutkaisten ongelmien parissa puuhatessa.
    year2012 
    |> Seq.filter (fun day -> day.DayOfWeek = DayOfWeek.Friday && day.Day = 13)
    |> Seq.iter (printfn "%O")

    // Yhdistefunktion esimerkki, itse lista voidaan määrittää jälkikäteen ilman parametreja:
    let prosessoi =  List.filter (fun x -> x > 4) >> List.map (fun y -> y+1) >> List.iter(printfn "%d")
    prosessoi [1..10]
    prosessoi [8..-2..4]

    // Operaatiokirjasto on vähän monipuolisempi kuin LINQ, joten sillä pääsee pitkälle.
    // Kun voima loppuu kesken, astuu kuviin rekursio ja pattern matching.


    // Seuraavassa esimerkissä näytetään tämän vuoden perjantai 13. -päivät ja milloin vastaava päivämäärä on perjantai seuraavan kerran.
    let rec findNextSameFriday13 (day : DateTime) =
        let next = new DateTime(day.Year + 1, day.Month, day.Day)
        match next.DayOfWeek with
        | DayOfWeek.Friday -> next
        | _ -> findNextSameFriday13 next
    
    year2012 
    |> Seq.filter (fun day -> day.DayOfWeek = DayOfWeek.Friday && day.Day = 13)  //Suodatus: filter = "where/reduce/..."
    |> Seq.map (fun day -> (day, findNextSameFriday13 day))  //Mappaus tyypistä toiseen: "projektio/select/..."
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

    // F# lista on oletuksena linkitetty lista, jonka ensimmäiseksi alkioksi lisääminen on tehokas operaatio
    // (koska lista on muuttumaton ("immutable"), niin uuden alkion lisäys eteen on vain uusi alkio ja pointteri vanhaan listaan)
    let emptyList = []
    let listOfListOfIntegers = [[1;2;3];[4;5;6]]

    // Listoja voi yhdistellä (merge):
    let merged1to6 = [1;2;3] @ [4;5;6]
    //val merged1to6 : int list = [1; 2; 3; 4; 5; 6]

    // lisäksi usein käsitellään ensimmäistä alkiota, ja välitetään loput rekursiolle. :: erottaa ensimmäisen alkion ja loput:
    let mylist = "head" :: ["tail1"; "tail2"]

    // Tyypillinen match koontifunktiolle on jotain tämän suuntaista:
    // (Yksinkertaistettuna, tämänhän voi tehdä vielä myös perus listaoperaatioilla)
    let rec sample f x =
        match x with
        | [] -> 0
        | h::t -> f(h) + sample f t 

    // Esim. Fibonacci-lukusarja:
    let rec fibs a b = 
        match a + b with c when c < 10000 -> c :: fibs b c | _ -> [] 
<<<<<<< HEAD
    let fibonacci = 0::1::(fibs 0 1) 


    // Matriisit ja moniulotteiset arrayt:
    Array2D.init 3 3 (fun x y -> x+y)
    |> Array2D.map (fun a -> a+1)
    // val it : int [,] = [[1; 2; 3]
    //                     [2; 3; 4]
    //                     [3; 4; 5]]


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
=======
    let fibonacci = 0::1::(fibs 0 1) 
>>>>>>> remotes/ilmirajat/fsharptraining.fi/master
