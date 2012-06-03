namespace FSharpTraining.Exercise 

// Referenssi toteutus 1
// Toteutus noudattelee Martin & Martinin ratkaisua.
// Ks. http://my.safaribooksonline.com/book/programming/csharp/0131857258/a-programming-episode/ch06lev1sec1.

// Koodia on hieman siistitty F# henkisemmäksi. Suurin muutos on Scorer luokan muuttaminen immutableksi. 
// Pääsääntöisesti se on uskollinen Martin & Martinin ratkaisulle.

// Usein on huono idea lähteä miettimään F#-ratkaisua suoraa C#-koodin pohjalta: tämä johtaa samanlaiseen koodiin
// kuin jos VB 6 koodia kääntäisi C#-koodiksi. Siitä huolimatta suurin osa nykypäivän koodista on C#-koodia.
// Pisteiden laskenta noudattaa logiikkaa joka sallii sivuvaikutukset (mutable).







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
                let completeItem (itemToComplate) = 
                    match itemToComplate with 
                    | CompleteAndIncludeNextOne total -> CompleteFrame (total+pins)
                    | CompleteAndIncludeNextTwo total -> CompleteAndIncludeNextOne (total+pins)
                    | _ -> itemToComplate
                scorecard |> List.map completeItem
            let addThrow (scorecard:Frame list) =
                let setLastFrame last tail (scorecard:Frame list) = 
                    let round = scorecard |> List.length
                    match last with
                    // Last throw after score or strike in the last round
                    | LastRoundIncludeNextOne (total) -> CompleteFrame (total+pins)::tail
                    // Last round spare
                    | FirstThrow (lastThrow ) when pins + lastThrow = 10 && round = 10 -> 
                        (LastRoundIncludeNextOne (10))::tail
                    // Spare
                    | FirstThrow (lastThrow) when pins + lastThrow = 10 && round <> 10 -> 
                        (CompleteAndIncludeNextOne (10))::tail                    
                    // last round strike or score
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

    type Game (list:Frame list) =
        let mutable scorecard  = list
        new() = Game (List.Empty)
        interface IGame with
            member x.Score = scorecard |> List.sumBy (fun frame -> frame.FrameScore)
            member x.Add(pins : int) = 
                scorecard <- Frame.Add pins scorecard
            member x.ScoreForFrame(theFrame) =  
                scorecard |> List.rev |> List.toSeq |> Seq.take theFrame |> Seq.sumBy (fun f -> f.FrameScore)
             
    module Tests = 
        open Microsoft.VisualStudio.TestTools.UnitTesting
        [<TestClass>]
        type FPBowlingTest () =
            inherit BowlingTestsBase() 
            override x.newGame ()  = (new Game()):>IGame




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