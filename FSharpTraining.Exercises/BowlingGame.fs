namespace FSharpTraining.Exercise 
// Game
// Classic bownling game implemented more or less like in 
// "Agile Principles, Patterns, and Practices in C#" by Robert Martin, Micah Martin
module Implementation = 
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
    let newGame () = {currentFrame = 0; isFirstThrow = true; scorer = Scorer.init}
        
        

module Tests = 
// Actual code
    open System
    open Microsoft.VisualStudio.TestTools.UnitTesting
    open Implementation
    [<TestClass>]
    type BowlingTests() = 

        [<TestMethod>] 
        member x.TestTwoThrowsNoMark() = 
            let game = newGame ()
            game.Add 5
            game.Add 4 
            Assert.AreEqual(9, game.Score); 
          
        [<TestMethod>] 
        member x.TestFourThrowsNoMark() =
            let game = newGame ()
            game.Add(5); 
            game.Add(4); 
            game.Add(7); 
            game.Add(2); 
            Assert.AreEqual(9, game.ScoreForFrame(1)) 
            Assert.AreEqual(18, game.ScoreForFrame(2))  
            Assert.AreEqual(18, game.Score); 

    
        [<TestMethod>] 
        member x.TestSimpleSpare() = 
            let game = newGame ()
            game.Add(3); 
            game.Add(7); 
            game.Add(3); 
            Assert.AreEqual(13, game.ScoreForFrame(1))  

        [<TestMethod>] 
        member x.TestSimpleFrameAfterSpare() =
            let game = newGame ()
            game.Add(3); 
            game.Add(7); 
            game.Add(3); 
            game.Add(2); 
            Assert.AreEqual(13, game.ScoreForFrame(1)); 
            Assert.AreEqual(18, game.ScoreForFrame(2)); 
            Assert.AreEqual(18, game.Score)

        [<TestMethod>] 
        member x.TestSimpleStrike() = 
            let game = newGame ()
            game.Add(10); 
            game.Add(3); 
            game.Add(6); 
            Assert.AreEqual(19, game.ScoreForFrame(1)) 
            Assert.AreEqual(28, game.Score)  

        [<TestMethod>] 
        member x.TestPerfectGame() = 
            let game = newGame()
            for i = 0 to 11 do
                game.Add(10) 
            Assert.AreEqual(300, game.Score)   

        [<TestMethod>]
        member x.TestEndOfArray() =
            let game = newGame()
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
            let game = newGame()
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
            let game = newGame()
            for i=0 to 10 do
                game.Add(10)
            game.Add(9)
            Assert.AreEqual(299, game.Score)
        
        [<TestMethod>] 
        member x.TestTenthFrameSpare()  =
            let game = newGame()          
            for i = 0 to 8 do game.Add(10) 
            game.Add(9) 
            game.Add(1) 
            game.Add(1) 
            Assert.AreEqual(270, game.Score)