namespace FSharpTraining.Exercise 
// Game
// Bownling game implemented more or less like in 
// "Agile Principles, Patterns, and Practices in C#" by Robert Martin, Micah Martin
module Contract =
    open System
    open Microsoft.VisualStudio.TestTools.UnitTesting
    type IGame = 
        abstract member Add : int -> unit
        abstract member Score : int with get
        abstract member ScoreForFrame : int -> int

    [<AbstractClass>]  
    type BowlingTestsBase() = 
        abstract member newGame : unit -> IGame
        [<TestMethod>] 
        member x.TestTwoThrowsNoMark() = 
            let game = x.newGame()
            game.Add 5
            game.Add 4 
            Assert.AreEqual(9, game.Score); 
          
        [<TestMethod>] 
        member x.TestFourThrowsNoMark() =
            let game = x.newGame()
            game.Add(5); 
            game.Add(4); 
            game.Add(7); 
            game.Add(2); 
            Assert.AreEqual(9, game.ScoreForFrame(1)) 
            Assert.AreEqual(18, game.ScoreForFrame(2))  
            Assert.AreEqual(18, game.Score); 

    
        [<TestMethod>] 
        member x.TestSimpleSpare() = 
            let game = x.newGame()
            game.Add(3); 
            game.Add(7); 
            game.Add(3); 
            Assert.AreEqual(13, game.ScoreForFrame(1))  

        [<TestMethod>] 
        member x.TestSimpleFrameAfterSpare() =
            let game = x.newGame()
            game.Add(3); 
            game.Add(7); 
            game.Add(3); 
            game.Add(2); 
            Assert.AreEqual(13, game.ScoreForFrame(1)); 
            Assert.AreEqual(18, game.ScoreForFrame(2)); 
            Assert.AreEqual(18, game.Score)

        [<TestMethod>] 
        member x.TestSimpleStrike() = 
            let game = x.newGame()
            game.Add(10); 
            game.Add(3); 
            game.Add(6); 
            Assert.AreEqual(19, game.ScoreForFrame(1)) 
            Assert.AreEqual(28, game.Score)  

        [<TestMethod>] 
        member x.TestPerfectGame() = 
            let game = x.newGame()
            for i = 0 to 11 do
                game.Add(10) 
            Assert.AreEqual(300, game.Score)   

        [<TestMethod>]
        member x.TestEndOfArray() =
            let game = x.newGame()
            for i = 0 to 8 do 
                game.Add(0); 
                game.Add(0);
            game.Add(2); 
            game.Add(8); 
            // 10th frame spare 
            game.Add(10); 
            // Strike in last position of array. 
            Assert.AreEqual(20, game.Score)  

        [<TestMethod>]
        member x.TestSampleGame() = 
            let game = x.newGame()
            game.Add(1); 
            game.Add(4); 
            game.Add(4); 
            game.Add(5); 
            game.Add(6); 
            game.Add(4); 
            game.Add(5); 
            game.Add(5); 
            game.Add(10); 
            game.Add(0); 
            game.Add(1); 
            game.Add(7); 
            game.Add(3); 
            game.Add(6); 
            game.Add(4); 
            game.Add(10); 
            game.Add(2); 
            game.Add(8); 
            game.Add(6); 
            Assert.AreEqual(133, game.Score);  
        
        [<TestMethod>] 
        member x.TestHeartBreak() =
            let game = x.newGame()
            for i=0 to 10 do
                game.Add(10)
            game.Add(9)
            Assert.AreEqual(299, game.Score)
        
        [<TestMethod>] 
        member x.TestTenthFrameSpare()  =
            let game = x.newGame()        
            for i = 0 to 8 do game.Add(10) 
            game.Add(9) 
            game.Add(1) 
            game.Add(1) 
            Assert.AreEqual(270, game.Score)

module MyImplementation =
    open Contract 
    type Game () =
        interface IGame with
            member x.Score = 0 // tähän koko tämän hetkisen pelin lasku logiikka
            member x.Add(pins : int) = () // tähän logiikka kuinka kirjaa muistin yhden heiton peristeet
            member x.ScoreForFrame(theFrame) = 0 // Tähän logiikka joka palauttaa pisteet freimiin theFrame asti

    module Tests = 
        open Microsoft.VisualStudio.TestTools.UnitTesting
        [<TestClass>]
        type BowlingTest () =
            inherit BowlingTestsBase() 
            override x.newGame ()  = new Game():>IGame


// Referenssi toteutus 1
// Toteutus noudattelee Martin & Martinin ratkaisua.
// Koodia on hieman siistitty F# henkisemmäksi. Suurin muutos on Scorer luokan muuttaminen immutableksi. 
// Pisteiden laskenta noudattaa logiikkaa joka sallii sivuvaikutukset (mutable)
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
