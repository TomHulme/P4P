using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;

namespace UnitTestSuite
{
    [TestClass]
    public class MoveTest
    {
        [TestMethod]
        public void TestLegalKingMoves()
        {
            //arrange
            String position = "3k4/8/8/8/8/8/8/3K4 w - - 0 1";
            Position testPosition = FENConverter.convertFENToPosition(position);
            //act
            ArrayList legalMoves = MoveGenerator.mgInstance.legalMoves(testPosition);
            //assert
            Assert.AreEqual(5, legalMoves.Count);
        }

        [TestMethod]
        public void TestLegalBishopMoves()
        {
            //arrange
            String position = "3k4/8/8/8/8/8/1B6/8 w - - 0 1";
            Position testPosition = FENConverter.convertPiecePlacementToPosition(position);
            //act
            ArrayList legalMoves = MoveGenerator.mgInstance.legalMoves(testPosition);
            //assert
            Assert.AreEqual(9, legalMoves.Count);
        }

        [TestMethod]
        public void TestLegalRookMoves()
        {
            //arrange
            String position = "3k4/8/8/8/8/8/6R1/8 w - - 0 1";
            Position testPosition = FENConverter.convertPiecePlacementToPosition(position);
            //act
            ArrayList legalMoves = MoveGenerator.mgInstance.legalMoves(testPosition);
            //assert
            Assert.AreEqual(14, legalMoves.Count);
        }

        [TestMethod]
        public void TestLegalMovesWithCapture()
        {
            //arrange
            String positionB = "3k4/6p1/8/8/8/8/1B6/8 w - - 0 1";
            String positionR = "3k4/6p1/8/8/8/8/6R1/8 w - - 0 1";
            Position testPositionB = FENConverter.convertPiecePlacementToPosition(positionB);
            Position testPositionR = FENConverter.convertPiecePlacementToPosition(positionR);
            //act
            ArrayList legalMovesB = MoveGenerator.mgInstance.legalMoves(testPositionB);
            ArrayList legalMovesR = MoveGenerator.mgInstance.legalMoves(testPositionR);

            Move captureB = new Move(FENConverter.getSquare("b2"), FENConverter.getSquare("g7"), PieceType.Empty);
            Move captureR = new Move(FENConverter.getSquare("g2"), FENConverter.getSquare("g7"), PieceType.Empty);
            //assert
            Assert.AreEqual(8, legalMovesB.Count);
            Assert.AreEqual(13, legalMovesR.Count);
            Assert.AreEqual(true, MoveParser.isMoveCapture(captureB, testPositionB));
            Assert.AreEqual(true, MoveParser.isMoveCapture(captureR, testPositionR));
        }
    }
}
