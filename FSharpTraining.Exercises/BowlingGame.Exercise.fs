namespace FSharpTraining.Exercise 
module BowlingGame =
    open Contract 
    type Game () =
        interface IGame with
            member x.Score with get() = 0 // calculate total score here
            member x.Add(pins : int) =  ()  // add new trhow to scorecard
            member x.ScoreForFrame(theFrame) = 0 // calculate total score of particular frama
    module Tests = 
        open Microsoft.VisualStudio.TestTools.UnitTesting
        [<TestClass>]
        type OOBowlingTest () =
            inherit BowlingTestsBase() 
            override x.newGame ()  = (new Game()):>IGame