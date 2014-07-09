using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using GameLogic;

namespace Tutorials
{
    /**
     * This tutorial class will introduce the user to the chess
     * board and the chess pieces.
     * 
     * 1.1: Introduce the board's ranks and files
     * 
     * 1.2: Introduce Pieces
     *      Introduction to the pieces is done using the same layout.
     *      - Display possible moves
     *      - Ask User to navigate to a square
     *      - Show capture
     *      - Ask for them to capture a piece
     *      
     * 1.3: Show understanding of concepts
     *      1.3.1: Place kings and queens on board
     *      1.3.2: Set up full chess line
     *      1.3.3: Pawn game, with king?
     *      1.3.4: Pawn mower puzzle
     */
    public class TutorialOne
    {
        //Current piece being introduced
        private PieceType currentPiece;
        //Current position of board
        private Position currentPosition;
        //list of moves
        private ArrayList moveList;

        /**
         * Initiliase a tutorialOne object with a white pawn as the current
         * piece and an empty board position.
         */
        public TutorialOne()
        {
            currentPiece = PieceType.P;
            currentPosition = FENConverter.convertPiecePlacementToPosition(FENConverter.emptyPosition);
        }

        /**
         * Set the current piece type
         */
        public void setPiece(PieceType piece)
        {
            currentPiece = piece;
        }

        /**
         * Return the current piece type
         */
        public PieceType getPiece()
        {
            return currentPiece;
        }

        /**
         * Place current pieces on board in start position.
         * Returns True if position is set, false if the currentPiece
         * is PieceType.Empty
         */
        public Boolean setInitialPosition()
        {
            Boolean isSet = false;
            //Makes the board empty before putting setting any pieces
            ClearBoard();

            switch (currentPiece)
            {
                case PieceType.P:
                case PieceType.p:
                    //Beginning square of pawn line
                    int startSquare = FENConverter.getSquare("a2");
                    for (int i = startSquare; i < startSquare + 8; i++)
                    {
                        //Set Pawns
                        currentPosition.setPiece(i, PieceType.P);
                    }

                    isSet = true;
                    break;
                case PieceType.R:
                case PieceType.r:
                    //Set Rooks
                    currentPosition.setPiece(FENConverter.getSquare("a1"), PieceType.R);
                    currentPosition.setPiece(FENConverter.getSquare("h1"), PieceType.R);

                    isSet = true;
                    break;
                case PieceType.N:
                case PieceType.n:
                    //Set Knights
                    currentPosition.setPiece(FENConverter.getSquare("b1"), PieceType.N);
                    currentPosition.setPiece(FENConverter.getSquare("g1"), PieceType.N);

                    isSet = true;
                    break;
                case PieceType.B:
                case PieceType.b:
                    //Set Bishops
                    currentPosition.setPiece(FENConverter.getSquare("c1"), PieceType.B);
                    currentPosition.setPiece(FENConverter.getSquare("f1"), PieceType.B);

                    isSet = true;
                    break;
                case PieceType.Q:
                case PieceType.q:
                    //Set Queen
                    currentPosition.setPiece(FENConverter.getSquare("d1"), PieceType.Q);

                    isSet = true;
                    break;
                case PieceType.K:
                case PieceType.k:
                    //Set King
                    currentPosition.setPiece(FENConverter.getSquare("e1"), PieceType.K);

                    isSet = true;
                    break;
                case PieceType.Empty:
                    break;
                default:
                    break;
            }

            return isSet;
        }

        /**
         * Clears the board of any pieces
         */
        private void ClearBoard()
        {
            currentPosition = FENConverter.convertPiecePlacementToPosition(FENConverter.emptyPosition);
        }

        /**
         * Generates and returns the list of moves that can be made
         */
        public ArrayList GenerateMoves()
        {
            moveList = MoveGenerator.mgInstance.legalMoves(currentPosition);
            return moveList;
        }

        /**
         * Highlight File
         * Takes as input letter of file
         * Returns the squares corresponding to that file
         */
        private String[] HighLightFile(String fileLetter)
        {
            String[] squares = new String[8];

            for (int i = 1; i <= 8; i++)
            {
                String square = fileLetter + i.ToString();

                squares[i - 1] = square;
            }

            return squares;
        }

        /**
         * Highlight Rank
         * Take as input number of rank
         * Returns the squares corresponding to that rank
         */
        private String[] HighLightRank(int rankNumber)
        {
            String[] squares = {"a", "b", "c", "d", "e", "f", "g", "h"};

            for (int i = 0; i < 8; i++)
            {
                String square = squares[i] + rankNumber.ToString();

                squares[i] = square; 
            }

            return squares;
        }
    }
}
