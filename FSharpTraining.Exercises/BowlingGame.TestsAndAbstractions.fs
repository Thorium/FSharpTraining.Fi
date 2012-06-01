namespace FSharpTraining.Exercise 
// Tässä tiedostoassa on määritelty rajapinta ja testit jotka joista toteutuksen tulisi selvitä.

(* for interactive:

 #r "Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll"

 *)
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
            [1;4;4;5;6;4;5;5;10;0;1;7;3;6;4;10;2;8;6] |> List.iter game.Add
            Assert.AreEqual(133, game.Score) 
        
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