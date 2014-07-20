using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using GameLogic;

namespace Chess
{
    class Board : Canvas
    {
        /*
         * Class Vars
         */
        private Square[] squares;


        
        public bool flipped;
        private bool isVsAI = false;
        private Position position;
        private GameController gamecon;
        private GameScreen parent;
        

        /*
         * Constructor
         */
        public Board(bool b, Position pos, GameController gc)
        {
            this.flipped = b;
            this.position = pos;
            this.gamecon = gc;
        }

        /*
         * Public Interface
         */
        public PieceType getPieceForSquareNumber(int square)
        {
            return this.getSquareForNumber(square).getPiece();
        }


        /*
         * internal Methods
         */
        internal void setup()
        {
            this.drawBoard();
            this.placePieces();
            this.printNextTurn();
        }

        internal void drawBoard()
        {
            this.Width = 600;
            this.Height = 600;
            squares = new Square[64];
            for (int i = 0; i < 8; i++)
            {
                squares[i] = new Square(getSquareName(i, 0), this.getSquareNumber(i, 0));
                squares[i].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                squares[i].AddHandler(ButtonBase.TouchDownEvent, new RoutedEventHandler(TappedSquare), true);
                this.Children.Add(squares[i]);
                Canvas.SetTop(squares[i], 0);
                Canvas.SetLeft(squares[i], i * 75);
                for (int j = 1; j < 8; j++)
                {
                    squares[i + j * 8] = new Square(getSquareName(i, j), this.getSquareNumber(i, j));
                    squares[i + j * 8].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                    squares[i + j * 8].AddHandler(ButtonBase.TouchDownEvent , new RoutedEventHandler(TappedSquare), true);
                    this.Children.Add(squares[i + j * 8]);
                    Canvas.SetTop(squares[i + j * 8], j * 75);
                    Canvas.SetLeft(squares[i + j * 8], i * 75);
                }
            }
            this.ColourBoard();
        }

        internal void ColourBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (this.flipped)
                    {
                        if ((i + j) % 2 == 0) { this.squares[i * 8 + j].Background = Brushes.DarkGray; }
                        else { this.squares[i * 8 + j].Background = Brushes.WhiteSmoke; }
                    }
                    else
                    {
                        if ((i + j) % 2 == 0) { this.squares[i * 8 + j].Background = Brushes.WhiteSmoke; }
                        else { this.squares[i * 8 + j].Background = Brushes.DarkGray; }
                    }
                }
            }
        }

        public void ColourSquare(int square, Brush colour)
        {
            getSquareForNumber(square).Background = colour;
        }

        internal string getSquareName(int row, int col)
        {
            int rank, file;
            if (this.flipped)
            {
                rank = 7 - row;
                file = col;
            }
            else
            {
                rank = col;
                file = row;
            }

            string name = "";

            name = string.Concat(name, (char)('a' + (char)file));
            name = string.Concat(name, (char)('8' - (char)rank));
            return name;
        }

        internal string getSquareName(int squareNum)
        {
            int row, col;
            if (this.flipped)
            {
                row = squareNum % 8;
                col = squareNum / 8;
            }
            else
            {
                row = 7 - (squareNum / 8);
                col = squareNum % 8;
            }

            return getSquareName(row, col);
        }

        internal int getSquareNumber(int i, int j)
        {
            if (this.flipped)
            {
                return (i * 8 + j);
            }
            else
            {
                return ((7 - j) * 8 + i);
            }
        }

        internal void placePieces()
        {
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceType current = this.position.getPiece((((7-i) * 8) + j));
                    if (current != PieceType.Empty)
                    {
                        this.drawPiece(current, squares[((i * 8) + j)]);
                    }
                    else
                    {
                        squares[((i * 8) + j)].Children.Clear();
                    }
                }
            }
        }

        internal Square getSquareForNumber(int square)
        {
            foreach (Square s in squares)
            {
                if (s.getSquareNumber() == square) { return s; }
            }
            throw new Exception("ERROR! Could not find square.");
        }

        internal Square getSquareForName(string name)
        {
            foreach (Square s in squares)
            {
                if (s.getName() == name) { return s; }
            }
            throw new Exception("ERROR! Could not find square.");
        }

        internal void drawPiece(PieceType piece, Square sq)
        {
            string[] pieces = { "black_king", "black_queen", "black_rook", "black_bishop", "black_knight", "black_pawn", "white_king", "white_queen", "white_rook", "white_bishop", "white_knight", "white_pawn" };
            string pieceString = "";
            switch (piece)
            {
                case PieceType.k:
                    pieceString = "black_king";
                    break;
                case PieceType.q:
                    pieceString = "black_queen";
                    break;
                case PieceType.r:
                    pieceString = "black_rook";
                    break;
                case PieceType.b:
                    pieceString = "black_bishop";
                    break;
                case PieceType.n:
                    pieceString = "black_knight";
                    break;
                case PieceType.p:
                    pieceString = "black_pawn";
                    break;
                case PieceType.K:
                    pieceString = "white_king";
                    break;
                case PieceType.Q:
                    pieceString = "white_queen";
                    break;
                case PieceType.R:
                    pieceString = "white_rook";
                    break;
                case PieceType.B:
                    pieceString = "white_bishop";
                    break;
                case PieceType.N:
                    pieceString = "white_knight";
                    break;
                case PieceType.P:
                    pieceString = "white_pawn";
                    break;
            }
            if (pieceString.Length == 0) 
            {
                sq.setPiece(PieceType.Empty);
                return;
            }
            // From site
            // Create Image Element
            Image myImage = new Image();
            myImage.Width = 75;

            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\" + pieceString + ".jpg");
            myBitmapImage.DecodePixelWidth = 75;
            if (this.flipped) myBitmapImage.Rotation = Rotation.Rotate90;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            //myImage.
            sq.Children.Add(myImage);
            sq.setPiece(piece);
            myImage.SetValue(TextBlock.TextProperty, pieceString);
            
        }

        /*
         * Square Tapped Event
         */
        internal void TappedSquare(object sender, RoutedEventArgs e)
        {
            Square tapped = (Square)sender;
            this.gamecon.MoveHandler(tapped);
        }

        /*
         * Move related methods
         */

        internal void printNextTurn()
        {
            Console.WriteLine(this.position.whiteMove ? "White to move." : "Black to move.");

            // Prints legal moves.
            if (false)
            {
                Console.WriteLine("Legal moves are:");
                foreach (Move x in new MoveGenerator().legalMoves(this.position))
                {
                    Console.WriteLine("\t" + x.origin + " to " + x.destination + " with promoteTo " + x.promoteTo);
                }
            }
        }


        /*
         * 
         *
        protected void Surscribe(GameController gameController)
        {
            gameController.RaiseControllerEvent += HandleControllerEvent;
        }

        void HandleControllerEvent(object sender, ControllerEvent e)
        {
            Console.WriteLine("Controller " + e.Name + " is talking!");
        }*/
    }
}

public class BoardEvent : EventArgs
{
    public BoardEvent(Move m, String ms, Boolean cm)
    {
        move = m;
        moveString = ms;
        checkMate = cm;
    }
    private Move move;
    private String moveString;
    private Boolean checkMate;

    public Boolean CheckMate
    {
        get { return checkMate; }
    }

    public String MoveString
    {
        get { return moveString; }
    }

    public Move Move
    {
        get { return move; }
    }
}