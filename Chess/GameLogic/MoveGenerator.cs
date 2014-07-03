using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class MoveGenerator
    {
        public static MoveGenerator mgInstance;

        static MoveGenerator()
        {
            mgInstance = new MoveGenerator();
        }

        /*
         * Generate and return a list of legal moves
         */
        public ArrayList legalMoves(Position position)
        {
            ArrayList moveList = psuedoLegalMoves(position);
            moveList = MoveGenerator.removeIllegalMoves(position, moveList);
            return moveList;
        }

        /*
         * Generate and return a list of psuedo-legal moves.
         * Pseudo-legal moves are those that don't necessarily defend
         * from check threats.
         */
        public ArrayList psuedoLegalMoves(Position position)
        {
            ArrayList moveList = getMoveListObject();
            Boolean whiteToMove = position.whiteMove;

            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    int square = Position.getSquare(file, rank);
                    PieceType piece = position.getPiece(square);
                    if ((piece == PieceType.Empty) || (((int)piece < 6) != whiteToMove))
                    {
                        continue;
                    }
                    if ((piece == PieceType.R) || (piece == PieceType.r) || (piece == PieceType.Q) || (piece == PieceType.q))
                    {
                        if (addDirection(moveList, position, square, 7 - file, 1)) return moveList;
                        if (addDirection(moveList, position, square, 7 - rank, 8)) return moveList;
                        if (addDirection(moveList, position, square, file, -1)) return moveList;
                        if (addDirection(moveList, position, square, rank, -8)) return moveList;
                    }
                    if ((piece == PieceType.B) || (piece == PieceType.b) || (piece == PieceType.Q) || (piece == PieceType.q))
                    {
                        if (addDirection(moveList, position, square, Math.Min(7 - file, 7 - rank), 9)) return moveList;
                        if (addDirection(moveList, position, square, Math.Min(file, 7 - rank), 7)) return moveList;
                        if (addDirection(moveList, position, square, Math.Min(file, rank), -9)) return moveList;
                        if (addDirection(moveList, position, square, Math.Min(7 - file, rank), -7)) return moveList;
                    }
                    if ((piece == PieceType.N) || (piece == PieceType.n))
                    {
                        if (file < 6 && rank < 7 && addDirection(moveList, position, square, 1, 10)) return moveList;
                        if (file < 7 && rank < 6 && addDirection(moveList, position, square, 1, 17)) return moveList;
                        if (file > 0 && rank < 6 && addDirection(moveList, position, square, 1, 15)) return moveList;
                        if (file > 1 && rank < 7 && addDirection(moveList, position, square, 1, 6)) return moveList;
                        if (file > 1 && rank > 0 && addDirection(moveList, position, square, 1, -10)) return moveList;
                        if (file > 0 && rank > 1 && addDirection(moveList, position, square, 1, -17)) return moveList;
                        if (file < 7 && rank > 1 && addDirection(moveList, position, square, 1, -15)) return moveList;
                        if (file < 6 && rank > 0 && addDirection(moveList, position, square, 1, -6)) return moveList;
                    }
                    if ((piece == PieceType.K) || (piece == PieceType.k))
                    {
                        if (file < 7 && addDirection(moveList, position, square, 1, 1)) return moveList;
                        if (file < 7 && rank < 7 && addDirection(moveList, position, square, 1, 9)) return moveList;
                        if (rank < 7 && addDirection(moveList, position, square, 1, 8)) return moveList;
                        if (file > 0 && rank < 7 && addDirection(moveList, position, square, 1, 7)) return moveList;
                        if (file > 0 && addDirection(moveList, position, square, 1, -1)) return moveList;
                        if (file > 0 && rank > 0 && addDirection(moveList, position, square, 1, -9)) return moveList;
                        if (rank > 0 && addDirection(moveList, position, square, 1, -8)) return moveList;
                        if (file < 7 && rank > 0 && addDirection(moveList, position, square, 1, -7)) return moveList;

                        int kingOrigin = whiteToMove ? Position.getSquare(4, 0) : Position.getSquare(4, 7);

                        if (Position.getSquare(file, rank) == kingOrigin)
                        {
                            int aCastle = whiteToMove ? Position.A1_CASTLE : Position.A8_CASTLE;
                            int hCastle = whiteToMove ? Position.H1_CASTLE : Position.H8_CASTLE;
                            PieceType rook = whiteToMove ? PieceType.R : PieceType.r;
                            if (((position.getCastleMask() & (1 << hCastle)) != 0) &&
                                (position.getPiece(kingOrigin + 1) == PieceType.Empty) &&
                                (position.getPiece(kingOrigin + 2) == PieceType.Empty) &&
                                (position.getPiece(kingOrigin + 3) == rook) &&
                                !squareAttacked(position, kingOrigin) &&
                                !squareAttacked(position, kingOrigin + 1))
                            {
                                moveList.Add(getMoveObject(kingOrigin, kingOrigin + 2, PieceType.Empty));
                            }
                            if (((position.getCastleMask() & (1 << aCastle)) != 0) &&
                                (position.getPiece(kingOrigin - 1) == PieceType.Empty) &&
                                (position.getPiece(kingOrigin - 2) == PieceType.Empty) &&
                                (position.getPiece(kingOrigin - 3) == PieceType.Empty) &&
                                (position.getPiece(kingOrigin - 4) == rook) &&
                                !squareAttacked(position, kingOrigin) &&
                                !squareAttacked(position, kingOrigin - 1))
                            {
                                moveList.Add(getMoveObject(kingOrigin, kingOrigin - 2, PieceType.Empty));
                            }
                        }
                    }

                    if ((piece == PieceType.P) || (piece == PieceType.p))
                    {
                        int rankDir = whiteToMove ? 8 : -8;
                        if (position.getPiece(square + rankDir) == PieceType.Empty)
                        {
                            //non capture moves
                            addPawnMoves(moveList, square, square + rankDir);
                            if ((rank == (whiteToMove ? 1 : 6)) &&
                                (position.getPiece(square + 2 * rankDir) == PieceType.Empty))
                            {
                                //double step moves
                                addPawnMoves(moveList, square, square + rankDir * 2);
                            }
                        }
                        //Capture to the left
                        if (file > 0)
                        {
                            int toSquare = square + rankDir - 1;
                            PieceType captured = position.getPiece(toSquare);
                            if (captured != PieceType.Empty)
                            {
                                if (((int)captured < 6) != whiteToMove)
                                {
                                    if (captured == (whiteToMove ? PieceType.k : PieceType.K))
                                    {
                                        returnMoveList(moveList);
                                        moveList = getMoveListObject();
                                        moveList.Add(getMoveObject(square, toSquare, PieceType.Empty));
                                        return moveList;
                                    }
                                    else
                                    {
                                        addPawnMoves(moveList, square, toSquare);
                                    }
                                }
                            }
                            else if (toSquare == position.getEpSquare())
                            {
                                addPawnMoves(moveList, square, toSquare);
                            }
                        }
                        //Capture to the right
                        if (file < 7)
                        {
                            int toSquare = square + rankDir + 1;
                            PieceType captured = position.getPiece(toSquare);
                            if (captured != PieceType.Empty)
                            {
                                if (((int)captured < 6) != whiteToMove)
                                {
                                     if (captured == (whiteToMove ? PieceType.k : PieceType.K))
                                    {
                                        returnMoveList(moveList);
                                        moveList = getMoveListObject();
                                        moveList.Add(getMoveObject(square, toSquare, PieceType.Empty));
                                        return moveList;
                                    }
                                    else
                                    {
                                        addPawnMoves(moveList, square, toSquare);
                                    }
                                }
                            }
                            else if (toSquare == position.getEpSquare())
                            {
                                addPawnMoves(moveList, square, toSquare);
                            }
                        }
                    }
                }
            }
            return moveList;
        }

        /*
         * Return true if the side to move is in check
         */
        public static Boolean inCheck(Position position)
        {
            int kingSquare = position.getKingSquare(position.whiteMove);
            if (kingSquare < 0)
            {
                return false;
            }
            return squareAttacked(position, kingSquare);
        }

        /*
         * Return true if a square is attacked by the opposite side
         */
        public static Boolean squareAttacked(Position position, int square)
        {
            int file = Position.getFile(square);
            int rank = Position.getRank(square);
            Boolean isWhiteMove = position.whiteMove;

            PieceType opppositeQueen = isWhiteMove ? PieceType.q : PieceType.Q;
            PieceType opppositeRook = isWhiteMove ? PieceType.r : PieceType.R;
            PieceType opppositeBishop = isWhiteMove ? PieceType.b : PieceType.B;
            PieceType opppositeKnight = isWhiteMove ? PieceType.n : PieceType.N;

            PieceType piece;

            if (rank > 0)
            {
                piece = checkDirection(position, square, rank, -8); 
                if ((piece == opppositeQueen) || (piece == opppositeRook)) return true;
                piece = checkDirection(position, square, Math.Min(file, rank), -9); 
                if ((piece == opppositeQueen) || (piece == opppositeBishop)) return true;
                piece = checkDirection(position, square, Math.Min(7 - file, rank), -7); 
                if ((piece == opppositeQueen) || (piece == opppositeBishop)) return true;

                if (file > 1) 
                { 
                    piece = checkDirection(position, square, 1, -10); 
                    if (piece == opppositeKnight) return true; 
                }
                if (file > 0 && rank > 1) 
                { 
                    piece = checkDirection(position, square, 1, -17); 
                    if (piece == opppositeKnight) return true; 
                }
                if (file < 7 && rank > 1) 
                { 
                    piece = checkDirection(position, square, 1, -15); 
                    if (piece == opppositeKnight) return true; 
                }
                if (file < 6) 
                { 
                    piece = checkDirection(position, square, 1, -6); 
                    if (piece == opppositeKnight) return true; 
                }

                if (!isWhiteMove)
                {
                    if (file < 7 && rank > 1) 
                    { 
                        piece = checkDirection(position, square, 1, -7); 
                        if (piece == PieceType.P) return true; 
                    }
                    if (file > 0 && rank > 1) 
                    { 
                        piece = checkDirection(position, square, 1, -9); 
                        if (piece == PieceType.P) return true; 
                    }
                }
            }
            if (rank < 7)
            {
                piece = checkDirection(position, square, 7 - rank, 8); 
                if ((piece == opppositeQueen) || (piece == opppositeRook)) return true;
                piece = checkDirection(position, square, Math.Min(7 - file, 7 - rank), 9); 
                if ((piece == opppositeQueen) || (piece == opppositeBishop)) return true;
                piece = checkDirection(position, square, Math.Min(file, 7 - rank), 7); 
                if ((piece == opppositeQueen) || (piece == opppositeBishop)) return true;

                if (rank < 6) 
                { 
                    piece = checkDirection(position, square, 1, 10); 
                    if (piece == opppositeKnight) return true; 
                }
                if (file < 7 && rank < 6) 
                { 
                    piece = checkDirection(position, square, 1, 17); 
                    if (piece == opppositeKnight) return true; 
                }
                if (file > 0 && rank < 6) 
                { 
                    piece = checkDirection(position, square, 1, 15); 
                    if (piece == opppositeKnight) return true; 
                }
                if (file > 1) 
                { 
                    piece = checkDirection(position, square, 1, 6); 
                    if (piece == opppositeKnight) return true; 
                }

                if (isWhiteMove)
                {
                    if (file < 7 && rank < 6) 
                    { 
                        piece = checkDirection(position, square, 1, 9); 
                        if (piece == PieceType.p) return true; 
                    }
                    if (file > 0 && rank < 6) 
                    { 
                        piece = checkDirection(position, square, 1, 7); 
                        if (piece == PieceType.p) return true; 
                    }
                }
            }

            piece = checkDirection(position, square, 7 - file, 1); 
            if ((piece == opppositeQueen) || (piece == opppositeRook)) return true;
            piece = checkDirection(position, square, file, -1); 
            if ((piece == opppositeQueen) || (piece == opppositeRook)) return true;

            int opppositeKingSquare = position.getKingSquare(!isWhiteMove);
            if (opppositeKingSquare >= 0)
            {
                int oFile = Position.getFile(opppositeKingSquare);
                int oRank = Position.getRank(opppositeKingSquare);
                if ((Math.Abs(file - oFile) <= 1) && (Math.Abs(rank - oRank) <= 1))
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Removes illegal moves from the given move list
         * The move list contains psuedo-legal moves
         * This removes the moves that don't defend from check positions
         */
        public static ArrayList removeIllegalMoves(Position position, ArrayList moveList)
        {
            ArrayList finalList = new ArrayList();
            UnMakeInfo moveInfo = new UnMakeInfo();
            int moveListCount = moveList.Count;
            for (int i = 0; i < moveListCount; i++)
            {
                Move move = (Move)moveList[i];
                position.makeMove(move, moveInfo);
                position.setWhiteMove(!position.whiteMove);
                if (!inCheck(position))
                {
                    finalList.Add(move);
                }
                position.setWhiteMove(!position.whiteMove);
                position.unMakeMove(move, moveInfo);
            }
            return finalList;
        }

        /*
         * Add all moves from a given square in direction delta
         * @param maxSteps Max steps until reaching a border, Set to 1 for non-sliding pieces
         * Returns True if the enemy king could be captured, false otherwise
         */
        private Boolean addDirection(ArrayList moveList, Position position, int square, int maxSteps, int delta)
        {
            int destination = square;
            Boolean whiteToMove = position.whiteMove;
            PieceType opppositeKing = (whiteToMove ? PieceType.k : PieceType.K);
            while (maxSteps > 0)
            {
                destination += delta;
                PieceType piece = position.getPiece(destination);
                if (piece == PieceType.Empty)
                {
                    moveList.Add(getMoveObject(square, destination, PieceType.Empty));
                }
                else
                {
                    //check if piece is white
                    if (((int)piece < 6) != whiteToMove)
                    {
                        if (piece == opppositeKing)
                        {
                            returnMoveList(moveList);
                            moveList = getMoveListObject();
                            moveList.Add(getMoveObject(square, destination, PieceType.Empty));
                            return true;
                        }
                        else
                        {
                            moveList.Add(getMoveObject(square, destination, PieceType.Empty));
                        }
                    }
                    break;
                }
                maxSteps--;
            }
            return false;
        }

        /*
         * Generate all possible pawn moves from (file1, rank1) tp (file2, rank2)
         * Pawn promotions are taken into account
         */
        private void addPawnMoves(ArrayList moveList, int square1, int square2)
        {
            //White Promotion
            if (square2 >= 56)
            {
                moveList.Add(getMoveObject(square1, square2, PieceType.Q));
                moveList.Add(getMoveObject(square1, square2, PieceType.N));
                moveList.Add(getMoveObject(square1, square2, PieceType.R));
                moveList.Add(getMoveObject(square1, square2, PieceType.B));
            }
            //Black Promotion
            else if (square2 < 8)
            {
                moveList.Add(getMoveObject(square1, square2, PieceType.q));
                moveList.Add(getMoveObject(square1, square2, PieceType.n));
                moveList.Add(getMoveObject(square1, square2, PieceType.r));
                moveList.Add(getMoveObject(square1, square2, PieceType.b));
            }
            //No Promotion
            else
            {
                moveList.Add(getMoveObject(square1, square2, PieceType.Empty));
            }
        }

        /*
         * Check if there is an attacking piece in a given direction starting from
         * a given square. The direction is given by delta
         * @param maxSteps Max steps until reaching a border, Set to 1 for non-sliding pieces
         * Returns the first piece in the given direction, or Empty if there is no piece
         * in that direction
         */
        private static PieceType checkDirection(Position position, int square, int maxSteps, int delta)
        {
            while (maxSteps > 0)
            {
                square += delta;
                PieceType piece = position.getPiece(square);
                if (piece != PieceType.Empty)
                {
                    return piece;
                }
                maxSteps--;
            }
            return PieceType.Empty;
        }

        //---------------- Move Cache Handlers -------------------------

        private Move[] moveCache = new Move[2048];
        private int movesInCache = 0;
        private Object[] moveListCache = new Object[200];
        private int moveListsInCache = 0;

        private Move getMoveObject(int origin, int destination, PieceType promoteTo)
        {
            if (movesInCache > 0)
            {
                Move move = moveCache[--movesInCache];
                move.origin = origin;
                move.destination = destination;
                move.promoteTo = promoteTo;
                return move;
            }
            return new Move(origin, destination, promoteTo);
        }

        private ArrayList getMoveListObject()
        {
            if (moveListsInCache > 0)
            {
                return (ArrayList)moveListCache[--movesInCache];
            }
            return new ArrayList(60);
        }

        /*
         * Return all move objects in moveList to the move cache
         */
        public void returnMoveList(ArrayList moveList)
        {
            if (movesInCache + moveList.Count <= moveCache.Length)
            {
                int moveListCount = moveList.Count;
                for (int i = 0; i < moveListCount; i++)
                {
                    moveCache[movesInCache++] = (Move)moveList[i];
                }
            }
            moveList.Clear();
            if (moveListsInCache < moveListCache.Length)
            {
                moveListCache[moveListsInCache++] = moveList;
            }
        }

        public void returnMove(Move move)
        {
            if (movesInCache < moveCache.Length)
            {
                moveCache[movesInCache++] = move;
            }
        }
    }
}