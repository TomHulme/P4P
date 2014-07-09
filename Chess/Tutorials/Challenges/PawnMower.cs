using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using GameLogic;

namespace Challenges
{
    /**
     * Each PawnMower object is it's own challenge. When each object
     * is created they're generated randomly.
     * @param PieceType - user piece in challenge
     * @param Count - number of pawns on board
     */

    class PawnMower
    {
        //Piece that user controls
        private PieceType userPiece;
        //Number of moves to finish challenge
        private int count;
        //Current chess position
        private Position boardPosition;
        private Position initialPosition;
        //List of moves to complete challenge
        private ArrayList moves;
        //Random number generator
        private Random randomNumber;

        private int originSquare;
        private int destinationSquare;

        public PawnMower(PieceType userPiece, int count)
        {
            this.userPiece = userPiece;
            this.count = count;
            boardPosition = FENConverter.convertPiecePlacementToPosition(FENConverter.emptyPosition);
            boardPosition.sameActiveColor = true;
            moves = new ArrayList();
            randomNumber = new Random();

            SetUpBoard();

            initialPosition = boardPosition;
        }

        private void SetUpBoard()
        {
            //1. Place piece on board
            int startSquare = (int)(randomNumber.NextDouble() * 64);

            boardPosition.setPiece(startSquare, userPiece);

            int iterations = count;

            while (iterations > 0)
            {
                //2. Generate possible moves for piece in that square
                ArrayList generatedMoves = MoveGenerator.mgInstance.psuedoLegalMoves(boardPosition);

                //3. Randomly select a possible move
                int moveIndex = (int)(randomNumber.NextDouble() * generatedMoves.Count);
                Move selectedMove = (Move)generatedMoves.ToArray()[moveIndex];

                //check if squares of selectedMove is occupied
                if (!checkBoard(selectedMove))
                {
                    //4. Add move to list
                    moves.Add(selectedMove);

                    //5. Place piece on destination square
                    destinationSquare = selectedMove.destination;
                    originSquare = selectedMove.origin;
                    //6. Set pawn at origin to block moves in that direction 
                    boardPosition.setPiece(originSquare, PieceType.p);
                    boardPosition.setPiece(destinationSquare, userPiece);

                    iterations--;
                }
            }

            //Place last pawn at destination square
            boardPosition.setPiece(destinationSquare, PieceType.p);

            //6. Iterate through the moves list putting an 
            //   opposing pawn on the destination square
            //foreach (Move move in moves)
            //{
            //    boardPosition.setPiece(move.destination, PieceType.p);
            //}

            //7. Place piece on the first move's origin square
            boardPosition.setPiece(startSquare, userPiece);
        }

        private Boolean checkBoard(Move currentMove)
        {
            Boolean squareOccupied = false;

            //Keep track of squares that are occupied
            //These values should never exceed one
            int originCount = 0;
            int destinationCount = 0;

            //check origin square
            foreach (Move move in moves)
            {
                if ((currentMove.origin == move.origin) || (currentMove.origin == move.destination))
                {
                    originCount++;
                }
            }

            //check destination square
            foreach (Move move in moves)
            {
                if ((currentMove.destination == move.origin) || (currentMove.destination == move.destination))
                {
                    destinationCount++;
                }
            }

            if ((originCount > 1) || (destinationCount > 1))
            {
                squareOccupied = true;
            }

            return squareOccupied;
        }

        public Position getPosition() 
        {
            return this.boardPosition;
        }

        public void ResetPosition()
        {
            boardPosition = initialPosition;
        }
    }
}
