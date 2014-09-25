using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using GameLogic;


namespace Tutorials.Challenges
{
    /**
     * Each PawnMower object is it's own challenge. When each object
     * is created they're generated randomly.
     * @param PieceType - user piece in challenge
     * @param Count - number of pawns on board
     */

    class PawnMower : TutorialBase
    {
        //Piece that user controls
        private PieceType userPiece;
        //Number of moves to finish challenge
        private int count;
        //Initial chess position
        private String initialPosition;
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
            currentPosition.sameActiveColor = true;
            moves = new ArrayList();
            randomNumber = new Random();

            SetUpBoard();

            initialPosition = FENConverter.convertPositionToFEN(currentPosition);
        }

        private void SetUpBoard()
        {
            //1. Place piece on board
            int startSquare = (int)(randomNumber.NextDouble() * 64);

            currentPosition.setPiece(startSquare, userPiece);

            int iterations = count;

            while (iterations > 0)
            {
                //2. Generate possible moves for piece in that square
                ArrayList generatedMoves = MoveGenerator.mgInstance.psuedoLegalMoves(currentPosition);

                //3. Randomly select a possible move
                int moveIndex = (int)(randomNumber.NextDouble() * generatedMoves.Count);
                Move selectedMove = (Move)generatedMoves.ToArray()[moveIndex];

                //check if squares of selectedMove is occupied
                if (CheckMoveValidity(selectedMove))
                {
                    //4. Add move to list
                    moves.Add(selectedMove);

                    //5. Place piece on destination square
                    destinationSquare = selectedMove.destination;
                    originSquare = selectedMove.origin;
                    //6. Set pawn at origin to block moves in that direction 
                    currentPosition.setPiece(originSquare, PieceType.p);
                    currentPosition.setPiece(destinationSquare, userPiece);

                    iterations--;
                }
            }

        }

        /**
         * Check if the move correlates to the userPiece and
         * that the destination square of the move is empty.
         */
        private Boolean CheckMoveValidity(Move selectedMove)
        {
            Boolean moveValid = false;

            if (currentPosition.getPiece(selectedMove.origin) == userPiece)
            {
                if (currentPosition.getPiece(selectedMove.destination) == PieceType.Empty)
                {
                    moveValid = true;
                }
            }

            return moveValid;
        }

        public override void ResetPosition()
        {
            currentPosition = FENConverter.convertPiecePlacementToPosition(initialPosition);
            currentPosition.sameActiveColor = true;
        }
    }
}
