using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;

namespace GameLogicUnitTests
{
    [TestClass]
    public class PieceTest
    {
        [TestMethod]
        public void TestWhitePiecesToString()
        {
            //arrange
            PieceType wPawn         = PieceType.P;
            PieceType wKnight       = PieceType.N;
            PieceType wBishop       = PieceType.B;
            PieceType wRook         = PieceType.R;
            PieceType wQueen        = PieceType.Q;
            PieceType wKing         = PieceType.K;
            //act
            String wPawnString      = wPawn.ToString();
            String wKnightString    = wKnight.ToString();
            String wBishopString    = wBishop.ToString();
            String wRookString      = wRook.ToString();
            String wQueenString     = wQueen.ToString();
            String wKingString      = wKing.ToString();
            //assert
            Assert.AreEqual("P", wPawnString);
            Assert.AreEqual("N", wKnightString);
            Assert.AreEqual("B", wBishopString);
            Assert.AreEqual("R", wRookString);
            Assert.AreEqual("Q", wQueenString);
            Assert.AreEqual("K", wKingString);
        }

        [TestMethod]
        public void TestBlackPiecesToString()
        {
            //arrange
            PieceType bPawn         = PieceType.p;
            PieceType bKnight       = PieceType.n;
            PieceType bBishop       = PieceType.b;
            PieceType bRook         = PieceType.r;
            PieceType bQueen        = PieceType.q;
            PieceType bKing         = PieceType.k;
            //act
            String bPawnString      = bPawn.ToString();
            String bKnightString    = bKnight.ToString();
            String bBishopString    = bBishop.ToString();
            String bRookString      = bRook.ToString();
            String bQueenString     = bQueen.ToString();
            String bKingString      = bKing.ToString();
            //assert
            Assert.AreEqual("p", bPawnString);
            Assert.AreEqual("n", bKnightString);
            Assert.AreEqual("b", bBishopString);
            Assert.AreEqual("r", bRookString);
            Assert.AreEqual("q", bQueenString);
            Assert.AreEqual("k", bKingString);
        }

        [TestMethod]
        public void TestEmptyPieceToString()
        {
            //arrange
            PieceType emptyPiece    = PieceType.Empty;
            //act
            String emptyPieceString = emptyPiece.ToString();
            //assert
            Assert.AreEqual("Empty", emptyPieceString);
        }

        [TestMethod]
        public void TestWhitePiecesToNum()
        {
            //arrange
            PieceType wPawn = PieceType.P;
            PieceType wKnight = PieceType.N;
            PieceType wBishop = PieceType.B;
            PieceType wRook = PieceType.R;
            PieceType wQueen = PieceType.Q;
            PieceType wKing = PieceType.K;
            //act
            int wPawnInt = (int)wPawn;
            int wKnightInt = (int)wKnight;
            int wBishopInt = (int)wBishop;
            int wRookInt = (int)wRook;
            int wQueenInt = (int)wQueen;
            int wKingInt = (int)wKing;
            //assert
            Assert.AreEqual(0, wPawnInt);
            Assert.AreEqual(1, wKnightInt);
            Assert.AreEqual(2, wBishopInt);
            Assert.AreEqual(3, wRookInt);
            Assert.AreEqual(4, wQueenInt);
            Assert.AreEqual(5, wKingInt);
        }

        [TestMethod]
        public void TestBlackPiecesToNum()
        {
            //arrange
            PieceType bPawn = PieceType.p;
            PieceType bKnight = PieceType.n;
            PieceType bBishop = PieceType.b;
            PieceType bRook = PieceType.r;
            PieceType bQueen = PieceType.q;
            PieceType bKing = PieceType.k;
            //act
            int bPawnInt = (int)bPawn;
            int bKnightInt = (int)bKnight;
            int bBishopInt = (int)bBishop;
            int bRookInt = (int)bRook;
            int bQueenInt = (int)bQueen;
            int bKingInt = (int)bKing;
            //assert
            Assert.AreEqual(6, bPawnInt);
            Assert.AreEqual(7, bKnightInt);
            Assert.AreEqual(8, bBishopInt);
            Assert.AreEqual(9, bRookInt);
            Assert.AreEqual(10, bQueenInt);
            Assert.AreEqual(11, bKingInt);
        }

        [TestMethod]
        public void TestEmptyPieceToNum()
        {
            //arrange
            PieceType emptyPiece = PieceType.Empty;
            //act
            int emptyPieceInt = (int)emptyPiece;
            //assert
            Assert.AreEqual(12, emptyPieceInt);
        }
    }
}
