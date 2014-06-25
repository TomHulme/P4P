using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    /*
     * Converts positions and moves to and from text format.
     */
    public class FENConverter
    {
        /*
         * Forsyth-Edwards Notation
         * ------------------------------------------
         * Standard notation for describing positions
         * Six terms make up FEN
         * 1. Piece Placement - divided into ranks and files
         * 2. Active Colour - whose turn next? w = white, b = black
         * 3. Castling Availability - "K" = white can castle kingside
         *                          - "Q" = white can castle queenside
         *                          - "k" = black can castle kingside
         *                          - "q" = black can castle queenside
         *                          - "-" = no castling available
         * 4. En Passant target square - "-" if no ep possible
         * 5. Halfmove clock
         * 6. Full move number
         */
        static public String startPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        /*
         * Parse a FEN string and return a position object
         */
        public static Position convertFENToPosition(String fen)
        {
            Position position = new Position();
            String[] terms = fen.Split(' ');
            if (terms.Length < 2)
            {
                System.Console.WriteLine("Too few terms");
            }
            for (int i = 0; i < terms.Length; i++)
            {
                terms[i] = terms[i].Trim();
            }

            //Piece placement
            int rank = 7;
            int file = 0;
            for (int i = 0; i < terms[0].Length; i++)
            {
                char c = (char)terms[0].ToCharArray().GetValue(i);
                switch (c)
                {
                    case '1': file += 1; break;
                    case '2': file += 2; break;
                    case '3': file += 3; break;
                    case '4': file += 4; break;
                    case '5': file += 5; break;
                    case '6': file += 6; break;
                    case '7': file += 7; break;
                    case '8': file += 8; break;
                    case '/': rank--; file = 0; break;
                    case 'P': setPieceSafely(position, file, rank, PieceType.P); file++; break;
                    case 'N': setPieceSafely(position, file, rank, PieceType.N); file++; break;
                    case 'B': setPieceSafely(position, file, rank, PieceType.B); file++; break;
                    case 'R': setPieceSafely(position, file, rank, PieceType.R); file++; break;
                    case 'Q': setPieceSafely(position, file, rank, PieceType.Q); file++; break;
                    case 'K': setPieceSafely(position, file, rank, PieceType.K); file++; break;
                    case 'p': setPieceSafely(position, file, rank, PieceType.p); file++; break;
                    case 'n': setPieceSafely(position, file, rank, PieceType.n); file++; break;
                    case 'b': setPieceSafely(position, file, rank, PieceType.b); file++; break;
                    case 'r': setPieceSafely(position, file, rank, PieceType.r); file++; break;
                    case 'q': setPieceSafely(position, file, rank, PieceType.q); file++; break;
                    case 'k': setPieceSafely(position, file, rank, PieceType.k); file++; break;
                    default: throw new FENParserError("Invalid Piece", position);
                }
            }

            //Active Colour
            if (terms[1].Length > 0)
            {
                Boolean whiteToMove;
                char c = (char)terms[1].ToCharArray().GetValue(0);
                switch (c)
                {
                    case 'w': whiteToMove = true; break;
                    case 'b': whiteToMove = false; break;
                    default: throw new FENParserError("Invalid Active Colour", position);
                }
                position.setWhiteMove(whiteToMove);
            }
            else
            {
                throw new FENParserError("Invalid Active Colour", position);
            }

            //Castling Rights
            int castleMask = 0;
            if (terms.Length > 2)
            {
                for (int i = 0; i < terms[2].Length; i++)
                {
                    char c = (char)terms[2].ToCharArray().GetValue(i);
                    switch (c)
                    {
                        case 'K':
                            castleMask |= (1 << Position.H1_CASTLE);
                            break;
                        case 'Q':
                            castleMask |= (1 << Position.A1_CASTLE);
                            break;
                        case 'k':
                            castleMask |= (1 << Position.H8_CASTLE);
                            break;
                        case 'q':
                            castleMask |= (1 << Position.A8_CASTLE);
                            break;
                        case '-':
                            break;
                        default:
                            throw new FENParserError("Invalid castling flags", position);
                    }

                }
            }
            position.setCastleMask(castleMask);
            removeInvalidCastleFlags(position);
            
            //En Passant Target Square
            if (terms.Length > 3)
            {
                String epString = terms[3];
                if (!epString.Equals("-"))
                {
                    if (epString.Length < 2)
                    {
                        throw new FENParserError("Invalid En Passent Square", position);
                    }
                    position.setEpSquare(getSquare(epString));
                }
            }

            try
            {
                //Halfmove Clock
                if (terms.Length > 4)
                {
                    position.halfMoveClock = Convert.ToInt32(terms[4]);
                }
                //Full Move Counter
                if (terms.Length > 5)
                {
                    position.fullMoveCounter = Convert.ToInt32(terms[5]);
                }
            }
            catch (ArgumentException ae)
            {
                //Ignore errors here since fields are optional
            }

            //Each side must have exactly one king
            int maxNumber = (int)PieceType.Empty + 1;
            int[] numPieces = new int[maxNumber];
            for (int i = 0; i < maxNumber; i++)
            {
                numPieces[i] = 0;
            }
            for (file = 0; file < 8; file++)
            {
                for (rank = 0; rank < 8; rank++)
                {
                    numPieces[(int)position.getPiece(Position.getSquare(file, rank))]++;
                }
            }
            if (numPieces[(int)PieceType.K] != 1)
            {
                throw new FENParserError("Too many white kings", position);
            }
            if (numPieces[(int)PieceType.k] != 1)
            {
                throw new FENParserError("Too many black kings", position);
            }

            //White must not have too many pieces
            int maxWPawns = 8;
            maxWPawns -= Math.Max(0, numPieces[(int)PieceType.N] - 2);
            maxWPawns -= Math.Max(0, numPieces[(int)PieceType.B] - 2);
            maxWPawns -= Math.Max(0, numPieces[(int)PieceType.R] - 2);
            maxWPawns -= Math.Max(0, numPieces[(int)PieceType.Q] - 1);
            if (numPieces[(int)PieceType.P] > maxWPawns)
            {
                throw new FENParserError("Too many white pieces", position);
            }

            //Black must not have too many pieces
            int maxBPawns = 8;
            maxBPawns -= Math.Max(0, numPieces[(int)PieceType.n] - 2);
            maxBPawns -= Math.Max(0, numPieces[(int)PieceType.b] - 2);
            maxBPawns -= Math.Max(0, numPieces[(int)PieceType.r] - 2);
            maxBPawns -= Math.Max(0, numPieces[(int)PieceType.q] - 1);

            //Make sure king can not be captured
            Position pos2 = new Position(position);
            pos2.setWhiteMove(!position.whiteMove);
            if (MoveGenerator.inCheck(pos2))
            {
                throw new FENParserError("King capture possible", position);
            }

            fixupEPSquare(position);

            return position;
        }

        /*
         * Remove castling flags that arent valid
         */
        private static void removeInvalidCastleFlags(Position position)
        {
            int castleMask = position.getCastleMask();
            int validCastle = 0;
            if (position.getPiece(4) == PieceType.K)
            {
                if (position.getPiece(0) == PieceType.R) validCastle |= (1 << Position.A1_CASTLE);
                if (position.getPiece(7) == PieceType.R) validCastle |= (1 << Position.H1_CASTLE);
            }
            if (position.getPiece(60) == PieceType.k)
            {
                if (position.getPiece(56) == PieceType.r) validCastle |= (1 << Position.A8_CASTLE);
                if (position.getPiece(63) == PieceType.r) validCastle |= (1 << Position.H8_CASTLE);
            }
            castleMask &= validCastle;
            position.setCastleMask(castleMask);
        }

        /*
         * Remove EPSquare from position if it is not legal
         */
        private static void fixupEPSquare(Position position)
        {
            int epSquare = position.getEpSquare();
            if (epSquare >= 0)
            {
                ArrayList moves = MoveGenerator.mgInstance.legalMoves(position);
                Boolean epValid = false;
                foreach (Move move in moves)
                {
                    if (move.destination == epSquare)
                    {
                        if (position.getPiece(move.origin) == (position.whiteMove ? PieceType.P : PieceType.p))
                        {
                            epValid = true;
                            break;
                        }
                    }
                }
                if (!epValid)
                {
                    position.setEpSquare(-1);
                }
            }
        }

        /*
         * Return a Forsyth-Edwards Notation string corresponding to a Position object
         */
        public static String convertPositionToFEN(Position position)
        {
            StringBuilder feNotation = new StringBuilder();
            //Piece placement
            for (int rank = 7; rank >= 0; rank--)
            {
                int emptySquares = 0;
                for (int file = 0; file < 8; file++)
                {
                    PieceType piece = position.getPiece(Position.getSquare(file, rank));
                    if (piece == PieceType.Empty)
                    {
                        emptySquares++;
                    }
                    else
                    {
                        if (emptySquares > 0)
                        {
                            feNotation.Append(emptySquares);
                            emptySquares = 0;
                        }
                        switch (piece)
                        {
                            case PieceType.K:   feNotation.Append('K'); break;
                            case PieceType.Q:   feNotation.Append('Q'); break;
                            case PieceType.R:   feNotation.Append('R'); break;
                            case PieceType.B:   feNotation.Append('B'); break;
                            case PieceType.N:   feNotation.Append('N'); break;
                            case PieceType.P:   feNotation.Append('P'); break;
                            case PieceType.k:   feNotation.Append('k'); break;
                            case PieceType.q:   feNotation.Append('q'); break;
                            case PieceType.r:   feNotation.Append('r'); break;
                            case PieceType.b:   feNotation.Append('b'); break;
                            case PieceType.n:   feNotation.Append('n'); break;
                            case PieceType.p:   feNotation.Append('p'); break;
                            default: throw new FENParserError("Error creating FEN String");
                        }
                    }
                }
                if (emptySquares > 0)
                {
                    feNotation.Append(emptySquares);
                }
                if (rank > 0)
                {
                    feNotation.Append("/");
                }
            }
            //Active Colour
            feNotation.Append(position.whiteMove ? " w " : " b ");

            //Castling Rights
            Boolean anyCastle = false;
            if (position.h1Castle())
            {
                feNotation.Append('K');
                anyCastle = true;
            }
            if (position.a1Castle())
            {
                feNotation.Append('Q');
                anyCastle = true;
            }
            if (position.h8Castle())
            {
                feNotation.Append('k');
                anyCastle = true;
            }
            if (position.a8Castle())
            {
                feNotation.Append('q');
                anyCastle = true;
            }
            if (!anyCastle)
            {
                feNotation.Append('-');
            }

            //En Passant Target Square
            {
                feNotation.Append(" ");
                if (position.getEpSquare() >= 0)
                {
                    int file = Position.getFile(position.getEpSquare());
                    int rank = Position.getRank(position.getEpSquare());
                    feNotation.Append((char)(file + 'a'));
                    feNotation.Append((char)(rank + '1'));
                }
                else
                {
                    feNotation.Append('-');
                }
            }

            //Move Counters
            feNotation.Append(' ');
            feNotation.Append(position.halfMoveClock);
            feNotation.Append(' ');
            feNotation.Append(position.fullMoveCounter);

            return feNotation.ToString();
        }

        /*
         * Set a given piece in a given square
         */
        private static void setPieceSafely(Position position, int file, int rank, PieceType piece)
        {
            if (rank < 0) throw new FENParserError("Too many ranks");
            if (file > 7) throw new FENParserError("Too many columns");
            if ((piece == PieceType.P) || (piece == PieceType.p))
            {
                if ((rank == 0) || (rank == 7))
                {
                    throw new FENParserError("Pawn of first or last rank");
                }
            }
            position.setPiece(Position.getSquare(file, rank), piece);
        }

        /*
         * Converts a string containing rank and file to a square index
         * Returns -1 if not a legal square
         */
        public static int getSquare(String square)
        {
            int file = (char)square.ToCharArray().GetValue(0) - 'a';
            int rank = (char)square.ToCharArray().GetValue(1) - '1';
            if ((file < 0) || (file > 7) || (rank < 0) || (rank > 7))
            {
                return -1;
            }
            return Position.getSquare(file, rank);

        }

        /*
         * Convert a square index to a string describing rank and file
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

        /*
         * Create an ASCII representation of a position
         */
        public static String asciiBoard(Position position)
        {
            StringBuilder boardRepresentation = new StringBuilder(400);
            String newLine = Environment.NewLine;
            boardRepresentation.Append("    +----+----+----+----+----+----+----+----+");
            boardRepresentation.Append(newLine);
            for (int rank = 7; rank >= 0; rank--)
            {
                boardRepresentation.Append("    |");
                for (int file = 0; file < 8; file++)
                {
                    boardRepresentation.Append(" ");
                    PieceType piece = position.getPiece(Position.getSquare(file, rank));
                    if (piece == PieceType.Empty)
                    {
                        Boolean isDark = Position.darkSquare(file, rank);
                        boardRepresentation.Append(isDark ? ".. |" : "   |");
                    }
                    else
                    {
                        //A black piece has prefaced with a *
                        boardRepresentation.Append(((int)piece < 6) ? ' ' : '*');
                        String pieceName = "" + piece;
                        boardRepresentation.Append(pieceName);
                        boardRepresentation.Append(" |");
                    }
                }
                boardRepresentation.Append(newLine);
                boardRepresentation.Append("    +----+----+----+----+----+----+----+----+");
                boardRepresentation.Append(newLine);
            }
            return boardRepresentation.ToString();
        }
    }
}
