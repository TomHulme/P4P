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
        private MoveGenerator movegen;
        public event EventHandler<BoardEvent> RaiseBoardEvent;
        
        public Board(bool b, Position pos)
        {
            this.flipped = b;
            this.position = pos;
            this.movegen = new MoveGenerator();
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
            this.drawPiece(this.flipped, p, sq);
        }

        private void removePiece(Square sq)
        {
            sq.Children.Clear();
            sq.setPiece(PieceType.Empty);
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
            if (flipped) myBitmapImage.Rotation = Rotation.Rotate90;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            //myImage.
            sq.Children.Add(myImage);
            sq.setPiece(piece);
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
                Square orig = moveQueue.Dequeue();
                Square dest = moveQueue.Dequeue();
                
                // Debug prints origin and destination piece types contained in squares
                Console.WriteLine("Origin: " + orig.getPiece());
                Console.WriteLine("Destination: " + dest.getPiece());
                
                PieceType promoteTo = ((dest.getSquareNumber() <= 7 | dest.getSquareNumber() > 55) & (orig.getPiece().Equals(PieceType.p) | orig.getPiece().Equals(PieceType.P))) ? getPromotion(orig.getPiece()) : PieceType.Empty;
                
                // Debug prints promotion piece
                Console.WriteLine("Promote To: " + promoteTo);
                Move current = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promoteTo);
                if (MoveCheck(current))
                {
                    Console.WriteLine("AN ACTUAL VALID MOVE");
                    this.position.makeMove(current, this.unmake);
                    OnRaiseBoardEvent(new BoardEvent(current));
                    if (current.promoteTo != PieceType.Empty)
                    {
                        this.removePiece(orig);
                        this.addPiece(dest, current.promoteTo);
                    }
                    else
                    {
                        this.movePiece(current.origin, current.destination);
                    }
                    if (movegen.legalMoves(this.position).Count == 0)
                    {
                        Console.WriteLine("CHECKMATE!" + ((this.position.whiteMove) ? "\nBlack wins!" : "\nWhite wins!"));
                        Canvas winScreen = new Canvas();
                        winScreen.Width = 600;
                        winScreen.Height = 300;
                        winScreen.Background = Brushes.GreenYellow;
                        TextBlock winText = new TextBlock();
                        winText.Text = "CHECKMATE!" + ((this.position.whiteMove) ? "\nBlack wins!" : "\nWhite wins!");
                        winText.FontSize = 80;
                        winText.TextAlignment = TextAlignment.Center;
                        winText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        winText.Foreground = ((this.position.whiteMove) ? Brushes.Black : Brushes.White);
                        winScreen.Children.Add(winText);
                        winScreen.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        this.Children.Add(winScreen);
                    }
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

        private PieceType getPromotion(PieceType piece)
        {
            if (piece.Equals(PieceType.p)){
                return PieceType.q;
            }else{
                return PieceType.Q;
            }
            //return PieceType.Empty;
        }

        internal void setup()
        {
            this.drawBoard(this.flipped);
            this.placePieces(this.position);
            this.printNextTurn();
            //this.drawPieces(this.flipped);
            //this.arrangePieces(this.flipped);
        }

        private bool MoveCheck(Move m)
        {
            // Prints move tested
            //Console.WriteLine("Testing move from " + m.origin + " to " + m.destination + " with promoteTo " + m.promoteTo);
            return (movegen.legalMoves(this.position).Contains(m));
        }

        private void printNextTurn()
        {
            Console.WriteLine(this.position.whiteMove ? "White to move." : "Black to move.");

            // Prints legal moves.
            Console.WriteLine("Legal moves are:");
            foreach(Move x in new MoveGenerator().legalMoves(this.position)){
                Console.WriteLine("\t" + x.origin + " to " + x.destination + " with promoteTo " + x.promoteTo);
            }
        }

        // Event handling best practice from http://msdn.microsoft.com/en-us/library/w369ty8x.aspx
        protected virtual void OnRaiseBoardEvent(BoardEvent e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler<BoardEvent> handler = RaiseBoardEvent;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
    }
}

public class BoardEvent : EventArgs
{
    public BoardEvent(Move m)
    {
        move = m;
    }
    private Move move;
    public Move Move
    {
        get { return move; }
    }
}