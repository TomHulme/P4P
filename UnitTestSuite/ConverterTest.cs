using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;

namespace GameLogicUnitTests
{
    [TestClass]
    public class ConverterTest
    {
        [TestMethod]
        public void TestGetSquare()
        {
            //arrange
            String a1 = "a1";
            String e3 = "e3";
            //act
            int a1Index = FENConverter.getSquare(a1);
            int e3Index = FENConverter.getSquare(e3);
            //assert
            Assert.AreEqual(0, a1Index);
            Assert.AreEqual(20, e3Index);
        }

        [TestMethod]
        public void TestSquareToString()
        {
            //arrange
            int a1 = 0;
            int e3 = 20;
            //assert
            Assert.AreEqual("a1", FENConverter.squareToString(a1));
            Assert.AreEqual("e3", FENConverter.squareToString(e3));
        }

        [TestMethod]
        public void TestEnPassantFEN()
        {
            //arrange
            Position position = FENConverter.convertFENToPosition(FENConverter.startPosition);
            String fenExpected = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq e3 0 1";
            //act
            position.setEpSquare(20);
            //assert
            Assert.AreEqual(fenExpected, FENConverter.convertPositionToFEN(position));
        }
            
    }
}
