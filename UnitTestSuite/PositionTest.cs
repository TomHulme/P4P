using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;

namespace GameLogicUnitTests
{
    [TestClass]
    public class PositionTest
    {
        [TestMethod]
        public void TestEmptyPosition()
        {
            //arrange
            String emptyFEN = "8/8/8/8/8/8/8/8 w - - 0 1";
            //act
            Position empty = new Position();
            //assert
            Assert.AreEqual(emptyFEN, FENConverter.convertPositionToFEN(empty));
        }

        [TestMethod]
        public void TestStartPosition()
        {
            //arrange
            String startFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            //act
            Position position = FENConverter.convertFENToPosition(FENConverter.startPosition);
            //assert
            Assert.AreEqual(startFEN, FENConverter.convertPositionToFEN(position));
        }

        [TestMethod]
        public void TestMakeMove()
        {
            //arrange
            Move simpleMove = new Move(12, 28, PieceType.Empty); //e2e4
            UnMakeInfo unMakeMove = new UnMakeInfo();
            Position startPosition = FENConverter.convertFENToPosition(FENConverter.startPosition);
            //act
            startPosition.makeMove(simpleMove, unMakeMove);
            /*
             * This following string is technically incorrect as in FEN it should indicate e3 as
             * the en passant square. However, our implementation does not set the en passant square
             * if it cannot be used.
             */
            String fenString = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1";
            //assert
            Assert.AreEqual(fenString, FENConverter.convertPositionToFEN(startPosition));
        }

        [TestMethod]
        public void TestUnMakeMove()
        {
            //arrange
            Move simpleMove = new Move(12, 28, PieceType.Empty);
            UnMakeInfo unMakeMove = new UnMakeInfo();
            Position startPosition = FENConverter.convertFENToPosition(FENConverter.startPosition);
            //act
            startPosition.makeMove(simpleMove, unMakeMove);
            String fenString = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1";
            //assert
            Assert.AreEqual(fenString, FENConverter.convertPositionToFEN(startPosition));

            //act
            startPosition.unMakeMove(simpleMove, unMakeMove);
            //assert
            Assert.AreEqual(FENConverter.startPosition, FENConverter.convertPositionToFEN(startPosition));
        }
    }
}
