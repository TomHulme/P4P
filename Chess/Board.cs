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
         * Constructors 
         */
        public bool flipped;
        private Position position;
        private UnMakeInfo unmake = new UnMakeInfo();
        
        public Board(bool b, Position pos)
        {
            this.flipped = b;
            this.position = pos;
        }


        /*
         * Class Vars
         */
        private Square[] squares;

        private Queue<Square> moveQueue = new Queue<Square>();

        private bool oneClick = false;

        private void arrangePieces(bool p)
        {
            foreach (UIElement piece in this.Children)
            {
                if (piece.GetValue(TextBlock.TextProperty).ToString().Length > 0)
                {
                    switch (piece.GetValue(TextBlock.TextProperty).ToString())
                    {
                        case "a8":
                            ((Canvas)piece).VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            break;
                    }
                }
            }
        }

        private void placePieces(Position pos)
        {

            for (int i = 0; i < 64; i++)
            {
                PieceType current = pos.getPiece(i);
                if (current != PieceType.Empty)
                {
                    this.drawPiece(this.flipped, current, squares[i]);
                }
            }
        }

        public void movePiece(int origin, int destination)
        {
            squares[destination].setPiece(squares[origin].getPiece());
            squares[origin].setPiece(PieceType.Empty);

            Image[] img = new Image[1];
            if (squares[destination].Children.Count > 0)
            {
                squares[destination].Children.Clear();
            }
            squares[origin].Children.CopyTo(img, 0);
            squares[origin].Children.Clear();
            squares[destination].Children.Add(img[0]);
        }

        private void addPiece(Square sq, PieceType p)
        {
            
        }

        private void removePiece(Square sq)
        {
            
        }

        private void drawPieces(bool flipped)
        {
            string[] pieces = { "black_king", "black_queen", "black_rook", "black_bishop", "black_knight", "black_pawn", "white_king", "white_queen", "white_rook", "white_bishop", "white_knight", "white_pawn" };
            foreach (String piece in pieces.AsEnumerable())
            {
                // From site
                // Create Image Element
                Image myImage = new Image();
                myImage.Width = 75;

                // Create source
                BitmapImage myBitmapImage = new BitmapImage();

                // BitmapImage.UriSource must be in a BeginInit/EndInit block
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\" + piece + ".jpg");
                myBitmapImage.DecodePixelWidth = 75;
                if (flipped) myBitmapImage.Rotation = Rotation.Rotate90;
                myBitmapImage.EndInit();
                myImage.Source = myBitmapImage;
                myImage.IsHitTestVisible = false;
                //myImage.
                this.Children.Add(myImage);
                myImage.SetValue(TextBlock.TextProperty, piece);
            }
        }

        private void drawPiece(bool flipped, PieceType piece, Square sq)
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
            if (pieceString.Length == 0) return;
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
            if (flipped) myBitmapImage.Rotation = Rotation.Rotate90;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            //myImage.
            sq.Children.Add(myImage);
            myImage.SetValue(TextBlock.TextProperty, pieceString);
            
        }

        private Canvas drawBoard(bool flipped)
        {
            Canvas board = this;
            board.Width = 600;
            board.Height = 600;
            squares = new Square[64];
            for (int i = 0; i < 8; i++)
            {
                squares[i * 8] = new Square(getSquareName(i, 0, flipped), (i*8));
                squares[i * 8].Uid = getSquareName(i, 0, flipped);
                squares[i * 8].SetValue(TextBlock.TextProperty, getSquareName(i, 0, flipped));
                squares[i * 8].Width = 75;
                squares[i * 8].Height = 75;
                squares[i * 8].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                board.Children.Add(squares[i * 8]);
                Canvas.SetTop(squares[i * 8], i * 75);
                Canvas.SetLeft(squares[i * 8], 0);
                for (int j = 1; j < 8; j++)
                {
                    squares[i * 8 + j] = new Square(getSquareName(i, j, flipped), (i*8+j));
                    squares[i * 8 + j].Uid = getSquareName(i, j, flipped);
                    squares[i * 8 + j].SetValue(TextBlock.TextProperty, getSquareName(i, j, flipped));
                    squares[i * 8 + j].Width = 75;
                    squares[i * 8 + j].Height = 75;
                    squares[i * 8 + j].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                    board.Children.Add(squares[i * 8 + j]);
                    Canvas.SetTop(squares[i * 8 + j], i * 75);
                    Canvas.SetLeft(squares[i * 8 + j], j * 75);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (flipped)
                    {
                        if ((i + j) % 2 == 0) { squares[i * 8 + j].Background = Brushes.DarkGray; }
                        else { squares[i * 8 + j].Background = Brushes.White; }
                    }
                    else
                    {
                        if ((i + j) % 2 == 0) { squares[i * 8 + j].Background = Brushes.White; }
                        else { squares[i * 8 + j].Background = Brushes.DarkGray; }
                    }
                }
            }
            return board;
        }

        private string getSquareName(int i, int j, bool flipped)
        {
            int row, col;
            if (flipped)
            {
                row = i;
                col = 7 - j;
            }
            else
            {
                row = j;
                col = i;
            }

            string name = "";

            switch (row)
            {
                case 0:
                    name = string.Concat(name, "a");
                    break;
                case 1:
                    name = string.Concat(name, "b");
                    break;
                case 2:
                    name = string.Concat(name, "c");
                    break;
                case 3:
                    name = string.Concat(name, "d");
                    break;
                case 4:
                    name = string.Concat(name, "e");
                    break;
                case 5:
                    name = string.Concat(name, "f");
                    break;
                case 6:
                    name = string.Concat(name, "g");
                    break;
                case 7:
                    name = string.Concat(name, "h");
                    break;
                default:
                    break;
            }
            switch (col)
            {
                case 0:
                    name = string.Concat(name, "8");
                    break;
                case 1:
                    name = string.Concat(name, "7");
                    break;
                case 2:
                    name = string.Concat(name, "6");
                    break;
                case 3:
                    name = string.Concat(name, "5");
                    break;
                case 4:
                    name = string.Concat(name, "4");
                    break;
                case 5:
                    name = string.Concat(name, "3");
                    break;
                case 6:
                    name = string.Concat(name, "2");
                    break;
                case 7:
                    name = string.Concat(name, "1");
                    break;
                default:
                    break;
            }
            return name;
        }

        private int getSquareNumber(int i, int j, bool flipped)
        {
            if (flipped)
            {
                return (8 * i + j);
            }
            else
            {
                return (8 * j + i);
            }
        }

        private void TappedSquare(object sender, RoutedEventArgs e)
        {
            Square tapped = (Square)sender;
            Console.WriteLine(tapped.getName());
            moveQueue.Enqueue(tapped);
            if (this.oneClick)
            {
                Move current = new Move(moveQueue.Dequeue().getSquareNumber(), moveQueue.Dequeue().getSquareNumber(), tapped.getPiece());
                if (MoveCheck(current))
                {
                    Console.WriteLine("AN ACTUAL VALID MOVE");
                    this.position.makeMove(current, this.unmake);
                    this.movePiece(current.origin, current.destination);
                }
                else
                {
                    Console.WriteLine("Typical John...");
                }

                this.printNextTurn();
                this.oneClick = false;
                
            }
            else
            {
                this.oneClick = true;
            }
        }

        internal void setup(Position pos)
        {
            this.drawBoard(this.flipped);
            this.placePieces(pos);
            this.printNextTurn();
            //this.drawPieces(this.flipped);
            //this.arrangePieces(this.flipped);
        }

        private bool MoveCheck(Move m)
        {
            return (new MoveGenerator().legalMoves(new Position(FENConverter.convertFENToPosition(FENConverter.startPosition)))).Contains(m);
        }

        private void printNextTurn()
        {
            Console.WriteLine(this.position.whiteMove ? "White to move." : "Black to move.");
        }
    }
}
