namespace FSharpTraining.Exercise 

// Referenssi toteutus 1
// Toteutus noudattelee Martin & Martinin ratkaisua.
// Ks. http://my.safaribooksonline.com/book/programming/csharp/0131857258/a-programming-episode/ch06lev1sec1.

// Koodia on hieman siistitty F# henkisemmäksi. Suurin muutos on Scorer luokan muuttaminen immutableksi. 
// Pääsääntöisesti se on uskollinen Martin & Martinin ratkaisulle.

// Usein on huono idea lähteä miettimään F#-ratkaisua suoraa C#-koodin pohjalta: tämä johtaa samanlaiseen koodiin
// kuin jos VB 6 koodia kääntäisi C#-koodiksi. Siitä huolimatta suurin osa nykypäivän koodista on C#-koodia.
// Pisteiden laskenta noudattaa logiikkaa joka sallii sivuvaikutukset (mutable).
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