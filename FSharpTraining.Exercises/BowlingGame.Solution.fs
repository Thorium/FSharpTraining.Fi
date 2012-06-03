namespace FSharpTraining.Exercise 
// Alla on kaksi referenssi ratkaisua ongelmaan.

// Näistä ensimmäinen on pyritty rakentamaan mahd. pitkälti funtonaalisen ohjelmointiparadigman hengen mukaisesti 
// valitettavasti arvoa palauttamaton Add metodi tekee täysin sivuvaikutuksettoman ratkaisun kirjoittamisen 
// mahdottomaksi. Esimerkissä on hyödynnetty laajasti joko-tai-luokkia (discriminate union) sekä 
// hahmontunnistusta (pattern matching).

// Myös toisessa ratkaisussa ratkaisua on viety selvästi kohti funtionaalista ohjelmointi paradigmaa karsimalla 
// alku peräisen ratkaisun muokattavissa olevia kenttiä (mutable fields) minimiin. Täysin olio orientoitunut verrokin löytyy
// Martin & Martinin kirjasta Agile Patterns and Pratices in C#, luvusta 6:
// Ks. http://my.safaribooksonline.com/book/programming/csharp/0131857258/a-programming-episode/ch06lev1sec1.


module FuntionalSolution =
    open Contract 

    type Frame =
        | FirstThrow of int 
        | CompleteAndIncludeNextTwo of int
        | CompleteAndIncludeNextOne of int
        | LastRoundIncludeNextOne of int
        | CompleteFrame of int 
        | Empty
        member x.FrameScore = 
            match x with         
            | LastRoundIncludeNextOne current | FirstThrow current | CompleteFrame current 
            | CompleteAndIncludeNextTwo current | CompleteAndIncludeNextOne current -> current
            | _ -> 0
        static member Add (pins:int) (scorecard:Frame list) =
            let updateSparesAndStrikes (scorecard:Frame list) : Frame list  =
                let completeItem (itemToComplete) = 
                    match itemToComplete with 
                    | CompleteAndIncludeNextOne total -> CompleteFrame (total+pins)
                    | CompleteAndIncludeNextTwo total -> CompleteAndIncludeNextOne (total+pins)
                    | _ -> itemToComplete
                scorecard |> List.map completeItem
            let addThrow (scorecard:Frame list) =
                let setLastFrame last tail (scorecard:Frame list) = 
                    let round = scorecard |> List.length
                    match last with
                    // Last throw after spare or strike in the last round
                    | LastRoundIncludeNextOne (total) -> CompleteFrame (total+pins)::tail
                    // Last round spare
                    | FirstThrow (lastThrow ) when pins + lastThrow = 10 && round = 10 -> (LastRoundIncludeNextOne (10))::tail
                    // normal Spare
                    | FirstThrow (lastThrow) when pins + lastThrow = 10 -> (CompleteAndIncludeNextOne (10))::tail                    
                    // last round strike
                    | _ when pins  = 10 && round = 9 -> (LastRoundIncludeNextOne (10))::scorecard
                    // normal strike
                    | _ when pins = 10 -> (CompleteAndIncludeNextTwo (10))::scorecard
                    // second throw; not spare
                    | FirstThrow (lastThrow ) when pins + lastThrow <> 10 && round <> 10 -> (CompleteFrame (lastThrow + pins))::tail
                    // first throw but not first in the whole game
                    | _ -> FirstThrow (pins)::scorecard
                match scorecard with 
                    | [] -> setLastFrame Empty [] scorecard
                    | last::tail -> setLastFrame last tail scorecard
            scorecard |> updateSparesAndStrikes |> addThrow

    type Game () =
        let mutable scorecard : Frame list = []
        interface IGame with
            member x.Score = scorecard |> List.sumBy (fun frame -> frame.FrameScore)
            member x.Add(pins : int) =  scorecard <- Frame.Add pins scorecard
            member x.ScoreForFrame(theFrame) =  
                // Notice that in scorecard Framelist the last played frame is the frist one.
                scorecard |> List.rev |> List.toSeq |> Seq.take theFrame |> Seq.sumBy (fun f -> f.FrameScore)
             
    module Tests = 
        open Microsoft.VisualStudio.TestTools.UnitTesting
        [<TestClass>]
        type FPBowlingTest () =
            inherit BowlingTestsBase() 
            override x.newGame ()  = (new Game()):>IGame

// Referenssi toteutus 2

// Toteutus noudattelee Martin & Martinin ratkaisua. Koodia on hieman siistitty F# henkisemmäksi. Suurin muutos on 
// Scorer luokan muuttaminen ei-muutettavaksi (immutableksi). Yhtäkaikki pääsääntöisesti se on uskollinen 
// Martin & Martinin ratkaisulle.

// Usein on huono idea lähteä miettimään F#-ratkaisua suoraa C#-koodin pohjalta: tämä johtaa samanlaiseen koodiin
// kuin jos VB 6 koodia kääntäisi C#-koodiksi. Siitä huolimatta suurin osa nykypäivän koodista on C#-koodia.
// Pisteiden laskenta noudattaa logiikkaa joka sallii sivuvaikutukset (mutable), mutta toisaalta muuttamatta rajapintaa 
// Add-metodin osalta täysin sivuvaikutukseton, ei-muutettavissa oleviin rakenteisiin perustuva ratkaisu on mahdoton.

// Huomaa että tässä tapauksessa mahdollisimman pitkälti funktionaalisen paradigman mukaisesi rakennettu ratkaisu 
// ei ole mitenkään automaattisesti parempi kuin semi-funktionaalinen ajatellen koodin tiiviyttä ja luettavuutta
// molemmat ratkaisut ovat alkuperäistä C# toteutusta jonkin verran selkeämpiä ja tiiviimpiä.

// Kannustan vertailemaan ratkaisua ja pohtimaan kumpi ratkaisusta on selkämpi ja luettavampi. Muilta osin ratkaisut ovat pitkälti 
// toisiaan vastaavat: Ne ovat suunnilleen yhtä helposti laajennettavia ja ylläpidettäviä jne.
 
// Oletan että olio-ohjelmointi taustalla ja rutiinilla varustetusta funtinaaliseen ohjelmointiin enintään pintapuolisesti 
// tutustuneet pitävätä seuraavaa esimerkkiä hieman selkeämpänä. Tähän on pari selkeää syystä:
// 1) Käyttämäni funktionaalisen ratkaisun kaksi keskeisintä tietorakennetta (joko-tai-luokat ja hahmontunnistut) ovat outoja C# 
// rakenteisiin tottuneille.
// 2) Olio-ohjelmoinnin rakenteisiin tottuneet eivät enää osaa kiinnittää huomiota kaikkiin sellaisiin rakenteisiin, jotka 
// tekevät siitä sekavahkon ja hahkalasti seurattavan. 
module AlmostOriginalImplementation =
    open Contract 
    type Scorer =
        {
            throws : int array 
        }
        static member init = {throws = Array.empty<int>; }
        member x.AddThrow(pins : int) =
            let newArray = Array.append x.throws [|pins|] 
            {throws = newArray;}
        member x.ScoreForFrame(theFrame:int) = 
            let nextTwoBallsForStrike (ball) = x.throws.[ball+1] + x.throws.[ball+2]
            let nextBallForSpare (ball) = x.throws.[ball+2]
            let strike(ball) = x.throws.[ball] = 10;   
            let twoBallsInFrame (ball) =  x.throws.[ball] + x.throws.[ball+1]
            let spare (ball) = (x.throws.[ball] + x.throws.[ball+1]) = 10; 

            let mutable ball= 0; 
            let mutable score=0; 
            for currentFrame = 0 to (theFrame-1) do
                if x.throws.Length > ball then 
                    if strike(ball) then 
                        score <- score + 10 + nextTwoBallsForStrike(ball)
                        ball <- ball + 1 
                    else if spare(ball) then
                        score <- score + 10 + nextBallForSpare(ball) 
                        ball <- ball + 2
                    else 
                        score <- score + twoBallsInFrame (ball)
                        ball <- ball + 2;   
            score  
    type Game =
        { 
            mutable currentFrame : int 
            mutable isFirstThrow : bool
            mutable scorer : Scorer
        }
        interface IGame with
            member x.Score with get() = x.scorer.ScoreForFrame(x.currentFrame)
            member x.Add(pins : int) =
                let Strike(pins) = x.isFirstThrow && pins = 10;   
                let AdvanceFrame () = 
                    x.currentFrame <- x.currentFrame + 1 
                    if(x.currentFrame > 10) then 
                        x.currentFrame <- 10
                let LastBallInFrame(pins) = Strike(pins) || (not x.isFirstThrow);   
                let adjustCurrentFrame(pins) = 
                    if (LastBallInFrame(pins)) then 
                        AdvanceFrame() 
                    else 
                        x.isFirstThrow <- false;   
                x.scorer <- x.scorer.AddThrow(pins) 
                adjustCurrentFrame(pins) 
            member x.ScoreForFrame(theFrame) = x.scorer.ScoreForFrame(theFrame); 

    module Tests = 
        open Microsoft.VisualStudio.TestTools.UnitTesting
        [<TestClass>]
        type OOBowlingTest () =
            inherit BowlingTestsBase() 
            override x.newGame ()  = ({currentFrame = 0; isFirstThrow = true; scorer = Scorer.init}):>IGame