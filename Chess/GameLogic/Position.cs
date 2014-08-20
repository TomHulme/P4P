using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    /*
     * Stores the state of a chess position as described 
     * in Forsyth-Edwards Notation
     */
    public class Position
    {
        //State of board
        private PieceType[] pieceLayout = new PieceType[64];

        //Active Colour
        public Boolean whiteMove;

        //Castling Rights
        public static int A1_CASTLE = 0;    //White long castle
        public static int H1_CASTLE = 1;    //White short castle
        public static int A8_CASTLE = 2;    //Black long castle
        public static int H8_CASTLE = 3;    //Black short castle

        private int castleMask;

        //En Passant Target Square
        private int epSquare;

        //Number of half moves since last 50 move reset.
        public int halfMoveClock;

        // Game move number, starting from 1.
        public int fullMoveCounter;

        //Cached king positions
        private int wKingSquare, bKingSquare;

        //True if active colour sholuld reamin the same
        public Boolean sameActiveColor = false;

        //-----------------Position Constructors---------------------

        /*
         * Initialise board to empty position
         */
        public Position()
        {
            for (int i = 0; i < pieceLayout.Length; i++)
            {
                pieceLayout[i] = PieceType.Empty;
            }
            whiteMove = true;
            castleMask = 0;
            epSquare = -1;
            halfMoveClock = 0;
            fullMoveCounter = 1;
            wKingSquare = bKingSquare = -1;
        }

        /*
         * Initialise board to a given position
         */
        public Position(Position position)
        {
            pieceLayout = position.pieceLayout;
            whiteMove = position.whiteMove;
            castleMask = position.castleMask;
            epSquare = position.epSquare;
            halfMoveClock = position.halfMoveClock;
            fullMoveCounter = position.fullMoveCounter;
            wKingSquare = position.wKingSquare;
            bKingSquare = position.bKingSquare;
            sameActiveColor = position.sameActiveColor;
        }

        //-----------------Board and Pieces Methods---------------------

        /*
         * Returns the piece occupying the given square
         */
        public PieceType getPiece(int square)
        {
            if (square > 63 | square < 0)
            {
                Console.WriteLine("SQUARE " + square + " DOESNT EXIST!");
                return pieceLayout[0];
            }
            return pieceLayout[square];
        }

        /*
         * Set a given sqaure to a given piece
         */
        public void setPiece(int square, PieceType piece)
        {
            //Update hash key?

            //Update Board
            pieceLayout[square] = piece;

            //Update king position
            if (piece == PieceType.K)
            {
                wKingSquare = square;
            } 
            else if (piece == PieceType.k) 
            {
                bKingSquare = square;
            }
        }

        /*
         * Return the square containting the King of
         * the active colour
         */
        public int getKingSquare(Boolean whiteMove)
        {
            return whiteMove ? wKingSquare : bKingSquare;
        }

        /*
         * Set the active colour of the position
         */
        public void setWhiteMove(Boolean whiteMove)
        {
            if (whiteMove != this.whiteMove)
            {
                //Update Hash key
                this.whiteMove = whiteMove;
            }
        }

        /*
         * Count number of pieces of a certain type
         */
        public int pieceCount(PieceType piece)
        {
            int count = 0;
            for (int square = 0; square < 64; square++)
            {
                if (pieceLayout[square] == piece)
                {
                    count++;
                }
            }
                return count;
        }

        //-----------------En Passant Methods---------------------

        /*
         * Return the En Passant Target Square
         */
        public int getEpSquare()
        {
            return epSquare;
        }

        /*
         * Set the En Passant Target Square
         */
        public void setEpSquare(int epSquare)
        {
            //Update the hash key


            this.epSquare = epSquare;
        }

        //-----------------Castling Right's Methods---------------

        /*
         * Return true if white long castling right has not been lost
         */
        public Boolean a1Castle()
        {
            return (castleMask & (1 << A1_CASTLE)) != 0;
        }

        /*
         * Return true if white short castling right has not been lost
         */
        public Boolean h1Castle()
        {
            return (castleMask & (1 << H1_CASTLE)) != 0;
        }

        /*
         * Return true if black long castling has not been lost
         */
        public Boolean a8Castle()
        {
            return (castleMask & (1 << A8_CASTLE)) != 0;
        }

        /*
         * Return true if black short castling has not been lost
         */
        public Boolean h8Castle()
        {
            return (castleMask & (1 << H8_CASTLE)) != 0;
        }

        /*
         * Bitmask describing castling rights
         */
        public int getCastleMask()
        {
            return castleMask;
        }

        /*
         * Set the castleMask of the position
         */
        public void setCastleMask(int castleMask)
        {
            //Update hash key

            this.castleMask = castleMask;
        }

        //-----------------Move Methods---------------------------

        /*
         * Apply a move to the current position
         */
        public void makeMove(Move move, UnMakeInfo moveInfo)
        {
            //Store info to undo move if needed
            moveInfo.setCapturedPiece(pieceLayout[move.destination]);
            moveInfo.setCastleRights(castleMask);
            moveInfo.setEPSquare(epSquare);
            moveInfo.setHalfMoveClock(halfMoveClock);

            Boolean whiteToMove = whiteMove;

            PieceType piece = pieceLayout[move.origin];
            PieceType capturedPiece = pieceLayout[move.destination];

            Boolean nullMove = (move.origin == 0) && (move.destination == 0);

            if (nullMove || (capturedPiece != PieceType.Empty) || (piece == (whiteToMove ? PieceType.P : PieceType.p)))
            {
                halfMoveClock = 0;
            }
            else
            {
                halfMoveClock++;
            }
            if (!whiteToMove)
            {
                fullMoveCounter++;
            }

            //Handle Castling
            PieceType king = whiteToMove ? PieceType.K : PieceType.k;
            int kingOrigin = move.origin;
            if (piece == king)
            {
                if (move.destination == kingOrigin + 2)
                {
                    setPiece(kingOrigin + 1, pieceLayout[kingOrigin + 3]);
                    setPiece(kingOrigin + 3, PieceType.Empty);
                }
                else if (move.destination == kingOrigin - 2)
                {
                    setPiece(kingOrigin - 1, pieceLayout[kingOrigin - 4]);
                    setPiece(kingOrigin - 4, PieceType.Empty);
                }

                if (whiteToMove)
                {
                    setCastleMask(castleMask & ~(1 << Position.A1_CASTLE));
                    setCastleMask(castleMask & ~(1 << Position.H1_CASTLE));
                }
                else
                {
                    setCastleMask(castleMask & ~(1 << Position.A8_CASTLE));
                    setCastleMask(castleMask & ~(1 << Position.H8_CASTLE));
                }
            }

            if (!nullMove)
            {
                PieceType rook = whiteToMove ? PieceType.R : PieceType.r;
                if (piece == rook)
                {
                    removeCastleRights(move.origin);
                }
                PieceType opppositeRook = whiteToMove ? PieceType.r : PieceType.R;
                if (capturedPiece == opppositeRook)
                {
                    removeCastleRights(move.destination);
                }
            }

            //Handle En Passant
            int prevEpSquare = epSquare;
            setEpSquare(-1);
            if (piece == PieceType.P)
            {
                if (move.destination - move.origin == 2 * 8)
                {
                    int file = Position.getFile(move.destination);
                    if (((file > 0) && (pieceLayout[move.destination - 1] == PieceType.p)) ||
                        ((file < 7) && (pieceLayout[move.destination + 1] == PieceType.p)))
                    {
                        setEpSquare(move.origin + 8);
                    }
                }
                else if (move.destination == prevEpSquare)
                {
                        setPiece(move.destination - 8, PieceType.Empty);
                }
            }
            else if (piece == PieceType.p)
            {
                if (move.destination - move.origin == -2 * 8)
                {
                    int file = Position.getFile(move.destination);
                    if (((file > 0) && (pieceLayout[move.destination - 1] == PieceType.P)) ||
                        ((file < 7) && (pieceLayout[move.destination + 1] == PieceType.P)))
                    {
                        setEpSquare(move.origin - 8);
                    }
                }
                else if (move.destination == prevEpSquare)
                {
                    setPiece(move.destination + 8, PieceType.Empty);
                }
            }

            //Perform Move
            setPiece(move.origin, PieceType.Empty);
            //Handle Promotion
            if (move.promoteTo != PieceType.Empty)
            {
                setPiece(move.destination, move.promoteTo);
            }
            else
            {
                setPiece(move.destination, piece);
            }

            if (!sameActiveColor)
            {
                setWhiteMove(!whiteToMove);
            }
        }

        /*
         * Unmake a move to the current position
         */
        public void unMakeMove(Move move, UnMakeInfo moveInfo)
        {
            if (!sameActiveColor)
            {
                setWhiteMove(!whiteMove);
            }
            PieceType piece = pieceLayout[move.destination];
            setPiece(move.origin, piece);
            setPiece(move.destination, moveInfo.getCapturedPiece());
            setCastleMask(moveInfo.getCastleRights());
            setEpSquare(moveInfo.getEPSquare());
            halfMoveClock = moveInfo.getHalfMoveClock();

            Boolean whiteToMove = whiteMove;
            if (move.promoteTo != PieceType.Empty)
            {
                piece = whiteToMove ? PieceType.P : PieceType.p;
                setPiece(move.origin, piece);
            }
            if (!whiteToMove)
            {
                fullMoveCounter--;
            }

            //Handle Castling
            PieceType king = whiteToMove ? PieceType.K : PieceType.k;
            int kingOrigin = move.origin;
            if (piece == king)
            {
                if (move.destination == kingOrigin + 2)
                {
                    setPiece(kingOrigin + 3, pieceLayout[kingOrigin + 1]);
                    setPiece(kingOrigin + 1, PieceType.Empty);
                }
                else if (move.destination == kingOrigin - 2)
                {
                    setPiece(kingOrigin - 4, pieceLayout[kingOrigin - 1]);
                    setPiece(kingOrigin - 1, PieceType.Empty);
                }
            }

            //Handle En Passant
            if (move.destination == epSquare)
            {
                if (piece == PieceType.P)
                {
                    setPiece(move.destination - 8, PieceType.p);
                }
                else if (piece == PieceType.p)
                {
                    setPiece(move.destination + 8, PieceType.P);
                }
            }
        }

        /*
         * Removes the castling right from position
         */
        private void removeCastleRights(int square)
        {
            if (square == Position.getSquare(0, 0))
            {
                setCastleMask(castleMask & ~(1 << Position.A1_CASTLE));
            }
            else if (square == Position.getSquare(7, 0))
            {
                setCastleMask(castleMask & ~(1 << Position.H1_CASTLE));
            }
            else if (square == Position.getSquare(0, 7))
            {
                setCastleMask(castleMask & ~(1 << Position.A8_CASTLE));
            }
            else if (square == Position.getSquare(7, 7))
            {
                setCastleMask(castleMask & ~(1 << Position.H8_CASTLE));
            }
        }

        //-----------------Static Methods-------------------------

        /*
         * Returns the corresponding square index based on rank and 
         * file
         */
        public static int getSquare(int file, int rank)
        {
            return rank * 8 + file;
        }

        /*
         * Returns the file corresponding to a given square
         */
        public static int getFile(int square)
        {
            return square & 7;
        }

        /*
         * Returns the rank corresponding to a given square
         */
        public static int getRank(int square)
        {
            return square >> 3;
        }

        /*
         * Returns true if (file, rank) is a dark square
         */
        public static Boolean darkSquare(int file, int rank)
        {
            return (file & 1) == (rank & 1);
        }

        //-----------------Equals Methods-------------------------

        /*
         * Overrides the Object.Equals method
         */
        public override Boolean Equals(Object obj)
        {
            if ((obj == null) || (obj.GetType() != this.GetType()))
            {
                return false;
            }
            Position other = (Position)obj;
            if (!drawRuleEquals(other))
                return false;
            if (halfMoveClock != other.halfMoveClock)
                return false;
            if (fullMoveCounter != other.fullMoveCounter)
                return false;
            return true;
        }

        /*
         * Determines if two positions are equal in the sense of the draw by
         * the repetition rule.
         * Return true if positions are equal, false otherwise.
         */
        public Boolean drawRuleEquals(Position pos)
        {
            for (int i = 0; i < 64; i++)
            {
                if (pieceLayout[i] != pos.pieceLayout[i])
                    return false;
            }
            if (whiteMove != pos.whiteMove)
                return false;
            if (epSquare != pos.epSquare)
                return false;
            return true;
        }

        /*
         * Overrides the getHashCode method
         */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
