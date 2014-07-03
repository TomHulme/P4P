using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class MoveParser
    {
        private class MoveComposition
        {
            public PieceType piece;
            public int originFile, originRank;
            public int destinationFile, destinationRank;
            public PieceType promotionPiece;

            public MoveComposition()
            {
                piece = PieceType.Empty;
                originFile = originRank = -1;
                destinationFile = destinationRank = -1;
                promotionPiece = PieceType.Empty;
            }
        }

        /*
         * Converts a UCI move string (long algebraic notation) into 
         * a move object
         */
        public static Move moveStringToObject(String uci, Position position)
        {
            return moveStringToObject(uci, position, null);
        }

        /*
         * Converts a UCI move string (long algebraic notation into
         * a move object, a list of moves can be supplied to search 
         * through.
         */
        public static Move moveStringToObject(String moveString, Position position, ArrayList moves)
        {
            if (moveString.Equals("--"))
            {
                return new Move(0, 0, 0);
            }

            moveString = moveString.Replace("=", "");
            moveString = moveString.Replace("\\+", "");
            moveString = moveString.Replace("#", "");
            Boolean whiteToMove = position.whiteMove;

            MoveComposition moveComp = new MoveComposition();
            Boolean capture = false;
            if (moveString.Equals("O-O") || moveString.Equals("0-0") || moveString.Equals("o-o"))
            {
                moveComp.piece = whiteToMove ? PieceType.K : PieceType.k;
                moveComp.originFile = 4;
                moveComp.destinationFile = 6;
                moveComp.originRank = moveComp.destinationRank = whiteToMove ? 0 : 7;
                moveComp.promotionPiece = PieceType.Empty;
            }
            else if (moveString.Equals("O-O-O") || moveString.Equals("0-0-0") || moveString.Equals("o-o-o"))
            {
                moveComp.piece = whiteToMove ? PieceType.K : PieceType.k;
                moveComp.originFile = 4;
                moveComp.destinationFile = 2;
                moveComp.originRank = moveComp.destinationRank = whiteToMove ? 0 : 7;
                moveComp.promotionPiece = PieceType.Empty;
            }
            else
            {
                Boolean attackToSquare = false;
                for (int i = 0; i < moveString.Length; i++)
                {
                    char c = moveString[i];
                    if (i == 0)
                    {
                        PieceType piece = charToPieceType(c);
                        if (piece != PieceType.Empty)
                        {
                            moveComp.piece = piece;
                            continue;
                        }
                    }
                    int tempFile = c - 'a';
                    if ((tempFile >= 0) && (tempFile < 8))
                    {
                        if (attackToSquare || (moveComp.originFile >= 0))
                        {
                            moveComp.destinationFile = tempFile;
                        }
                        else
                        {
                            moveComp.originFile = tempFile;
                        }
                    }
                    int tempRank = c - '1';
                    if ((tempRank >= 0) && (tempFile < 8))
                    {
                        if (attackToSquare || (moveComp.originRank >= 0))
                        {
                            moveComp.destinationRank = tempRank;
                        }
                        else
                        {
                            moveComp.originRank = tempRank;
                        }
                    }

                    if ((c == 'x') || (c == '-'))
                    {
                        attackToSquare = true;
                        if (c == 'x')
                        {
                            capture = true;
                        }
                    }

                    if (i == moveString.Length - 1)
                    {
                        PieceType promotionPiece = charToPieceType(c);
                        if (promotionPiece != PieceType.Empty)
                        {
                            moveComp.promotionPiece = promotionPiece;
                        }
                    }
                }
                if ((moveComp.originFile >= 0) && (moveComp.destinationFile < 0))
                {
                    moveComp.destinationFile = moveComp.originFile;
                    moveComp.originFile = -1;
                }
                if ((moveComp.originRank >= 0) && (moveComp.destinationRank < 0))
                {
                    moveComp.destinationRank = moveComp.originRank;
                    moveComp.originFile = -1;
                }

                if (moveComp.piece == PieceType.Empty)
                {
                    Boolean haveAll = (moveComp.originFile >= 0) && (moveComp.destinationFile >= 0) &&
                                      (moveComp.originRank >= 0) && (moveComp.destinationRank >= 0);
                    if (!haveAll)
                    {
                        moveComp.piece = whiteToMove ? PieceType.P : PieceType.p;
                    }
                }
            }

            if (moves == null)
            {
                moves = MoveGenerator.mgInstance.legalMoves(position);
            }
            ArrayList matches = new ArrayList(2);
            foreach (Move listMove in moves)
            {
                PieceType piece = position.getPiece(listMove.origin);
                Boolean match = true;
                if ((moveComp.piece >= 0) && (moveComp.piece != piece))
                {
                    match = false;
                }
                if ((moveComp.originFile >= 0) && (moveComp.originFile != Position.getFile(listMove.origin)))
                {
                    match = false;
                }
                if ((moveComp.originRank >= 0) && (moveComp.originRank != Position.getRank(listMove.origin)))
                {
                    match = false;
                }
                if ((moveComp.destinationFile >= 0) && (moveComp.destinationFile != Position.getFile(listMove.destination))) 
                {
                    match = false;
                }
                if ((moveComp.destinationRank >= 0) && (moveComp.destinationRank != Position.getRank(listMove.destination)))
                {
                    match = false;
                }
                if ((moveComp.promotionPiece >= 0) && (moveComp.promotionPiece != listMove.promoteTo))
                {
                    match = false;
                }
                if (match)
                {
                    matches.Add(listMove);
                }
            }

            int numMatches = matches.Count;
            if (numMatches == 0)
            {
                return null;
            }
            else if (numMatches == 1)
            {
                return (Move)matches[0];
            }
            if (!capture)
            {
                return null;
            }

            Move move = null;
            foreach (Move listMove in matches)
            {
                PieceType capturedPiece = position.getPiece(listMove.destination);
                if (capturedPiece != PieceType.Empty)
                {
                    if (move == null)
                    {
                        move = listMove;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return move;
        }

        /*
         * Converts a move object into a UCI move string (long algebraic
         * notation)
         */
        public static String moveObjectToString(Move move, Position position)
        {
            return moveObjectToString(move, position, null);
        }

        /*
         * Converts a move object into a UCI move string (long algebraic
         * notation), a move list can be supplied to search through.
         */
        public static String moveObjectToString(Move move, Position position, ArrayList moves)
        {
            if ((move == null) || move.Equals(new Move(0, 0, 0)))
            {
                return "--";
            }
            StringBuilder moveString = new StringBuilder();
            int whiteKingOrigin = Position.getSquare(4, 0);
            int blackKingOrigin = Position.getSquare(4, 7);
            if ((move.origin == whiteKingOrigin) && (position.getPiece(whiteKingOrigin) == PieceType.K))
            {
                //Check white castle
                if (move.destination == Position.getSquare(6, 0))
                {
                    moveString.Append("O-O");
                }
                else if (move.destination == Position.getSquare(2, 0))
                {
                    moveString.Append("O-O-O");
                }
            }
            else if ((move.origin == blackKingOrigin) && (position.getPiece(blackKingOrigin) == PieceType.k))
            {
                //Check black castle
                if (move.destination == Position.getSquare(6, 7))
                {
                    moveString.Append("O-O");
                }
                else if (move.destination == Position.getSquare(2, 7))
                {
                    moveString.Append("O-O-O");
                }
            }
            if (moveString.Length == 0)
            {
                PieceType piece = position.getPiece(move.origin);
                moveString.Append("" + piece);

                int originFile = Position.getRank(move.origin);
                int originRank = Position.getFile(move.origin);
                int destinationFile = Position.getRank(move.destination);
                int destinationRank = Position.getFile(move.destination);

                //Long Algebraic Notation-----
                moveString.Append((char)(originFile + 'a'));
                moveString.Append((char)(originRank + '1'));
                moveString.Append(isMoveCapture(move, position) ? 'x' : '-');
                //----------------------------
                moveString.Append((char)(destinationFile + 'a'));
                moveString.Append((char)(destinationRank + '1'));
                if (move.promoteTo != PieceType.Empty)
                {
                    moveString.Append("" + piece);
                }
            }
            UnMakeInfo unMake = new UnMakeInfo();
            position.makeMove(move, unMake);
            Boolean inCheck = MoveGenerator.inCheck(position);
            if (inCheck)
            {
                ArrayList nextMoves = MoveGenerator.mgInstance.legalMoves(position);
                if (nextMoves.Count == 0)
                {
                    moveString.Append('#');
                }
                else
                {
                    moveString.Append('+');
                }
            }
            position.unMakeMove(move, unMake);

            return moveString.ToString();
        }

        /*
         * Convert a move object to UCI string format
         */
        public static String moveObjectToString(Move move)
        {
            String moveString = squareToString(move.origin);
            moveString += (squareToString(move.destination));
            switch (move.promoteTo)
            {
                case PieceType.Q:
                case PieceType.q:
                    moveString += "q";
                    break;
                case PieceType.R:
                case PieceType.r:
                    moveString += "r";
                    break;
                case PieceType.B:
                case PieceType.b:
                    moveString += "b";
                    break;
                case PieceType.N:
                case PieceType.n:
                    moveString += "n";
                    break;
                default:
                    break;
            }
            return moveString;
        }

        /*
         * Returns true if a move is valid for a given position
         */
        public static Boolean isMoveValid(Move move, Position position)
        {
            if (move == null)
            {
                return false;
            }
            ArrayList moves = new MoveGenerator().legalMoves(position);
            foreach (Move listMove in moves)
            {
                //Move is only valid if it appears in legal moves list
                if (move.Equals(listMove))
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Returns true if a move results in a caputre for a given
         * position
         */
        public static Boolean isMoveCapture(Move move, Position position)
        {
            if (position.getPiece(move.destination) == PieceType.Empty)
            {
                //Deal with En Passant capture
                PieceType piece = position.getPiece(move.origin);
                if ((piece == (position.whiteMove ? PieceType.P : PieceType.p)) && (move.destination == position.getEpSquare()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /*
         * Returns a piece based on a char representation
         */
        public static PieceType charToPieceType(char c)
        {
            switch (c)
            {
                //white pieces
                case 'K': return PieceType.K;
                case 'Q': return PieceType.Q;
                case 'B': return PieceType.B;
                case 'N': return PieceType.N;
                case 'R': return PieceType.R;
                case 'P': return PieceType.P;
                //black pieces
                case 'k': return PieceType.k;
                case 'q': return PieceType.q;
                case 'b': return PieceType.b;
                case 'n': return PieceType.n;
                case 'r': return PieceType.r;
                case 'p': return PieceType.p;
                default: return PieceType.Empty;
            }
        }

        /*
         * Return a string representing a square number
         */
        public static String squareToString(int square)
        {
            StringBuilder squareString = new StringBuilder();
            int file = Position.getFile(square);
            int rank = Position.getRank(square);
            squareString.Append((char)(file + 'a'));
            squareString.Append((char)(rank + '1'));
            return squareString.ToString();
        }
    }
}