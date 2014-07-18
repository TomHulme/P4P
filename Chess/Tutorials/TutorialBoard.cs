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
using Chess;

namespace Tutorials
{
    public class TutorialBoard : Canvas
    {
        public Boolean flipped;

        private Position position;
        private UnMakeInfo unmake = new UnMakeInfo();
        private MoveGenerator movegen;

        public event EventHandler<BoardEvent> RaiseBoardEvent;

        public TutorialBoard(Boolean flipped, Position position)
        {
            this.flipped = flipped;
            this.position = position;
            this.position.sameActiveColor = true;
            this.movegen = new MoveGenerator();
        }

        /*
         * Class Vars
         */
        private Square[] squares;

        private Queue<Square> moveQueue = new Queue<Square>();

        private Boolean oneClick = false;

        public void SetPosition(Position position)
        {
            this.position = position;
        }

        public void UpdateBoard()
        {
            this.DrawBoard();
            this.PlacePieces();
        }

        internal void SetUp()
        {
            this.DrawBoard();
            this.PlacePieces();
        }

        private void DrawBoard()
        {
            this.Width = 600;
            this.Height = 600;
            squares = new Square[64];
            for (int i = 0; i < 8; i++)
            {
                squares[i] = new Square(getSquareName(i, 0), this.getSquareNumber(i, 0));
                squares[i].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                this.Children.Add(squares[i]);
                Canvas.SetTop(squares[i], 0);
                Canvas.SetLeft(squares[i], i * 75);
                for (int j = 1; j < 8; j++)
                {
                    squares[i + j * 8] = new Square(getSquareName(i, j), this.getSquareNumber(i, j));
                    squares[i + j * 8].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                    this.Children.Add(squares[i + j * 8]);
                    Canvas.SetTop(squares[i + j * 8], j * 75);
                    Canvas.SetLeft(squares[i + j * 8], i * 75);
                }
            }
            this.ColourBoard();
        }
        
         private void ColourBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (this.flipped)
                    {
                        if ((i + j) % 2 == 0) { this.squares[i * 8 + j].Background = Brushes.DarkGray; }
                        else { this.squares[i * 8 + j].Background = Brushes.White; }
                    }
                    else
                    {
                        if ((i + j) % 2 == 0) { this.squares[i * 8 + j].Background = Brushes.White; }
                        else { this.squares[i * 8 + j].Background = Brushes.DarkGray; }
                    }
                }
            }
        }

        private string getSquareName(int row, int col)
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

        private string getSquareName(int squareNum)
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

        private int getSquareNumber(int i, int j)
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

        private PieceType getPromotion(PieceType piece)
        {

            if (piece.Equals(PieceType.p))
            {
                return PieceType.q;
            }
            else
            {
                return PieceType.Q;
            }
            //return PieceType.Empty;
        }

        private void PlacePieces()
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
                }
            }
        }

        public void movePiece(int origin, int destination)
        {
            Square originSquare = getPositionSquareNumber(origin);
            Square destinationSquare = getPositionSquareNumber(destination);
            destinationSquare.setPiece(originSquare.getPiece());
            originSquare.setPiece(PieceType.Empty);

            Image[] img = new Image[1];
            if (destinationSquare.Children.Count > 0)
            {
                destinationSquare.Children.Clear();
            }
            originSquare.Children.CopyTo(img, 0);
            originSquare.Children.Clear();
            destinationSquare.Children.Add(img[0]);
        }

        private Square getPositionSquareNumber(int square)
        {
            foreach (Square s in squares)
            {
                if (s.getSquareNumber() == square) { return s; }
            }
            throw new Exception("ERROR! Could not find square.");
        }

        private void drawPiece(PieceType piece, Square sq)
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

        private void ColourLegalMoves(int originSquare)
        {
            this.ColourBoard();
            foreach (Move x in new MoveGenerator().legalMoves(this.position))
            {
                if (x.origin == originSquare)
                {
                    if(getPositionSquareNumber(x.destination).getPiece() != PieceType.Empty){
                        //getPositionSquareNumber(x.destination).Background = Brushes.Red;
                        ColourSquareRed(x.destination);
                    }else{
                        //getPositionSquareNumber(x.destination).Background = Brushes.Blue;
                        ColourSquareBlue(x.destination);
                    }
                }
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
                //Console.WriteLine("Origin: " + orig.getSquareNumber());
                //Console.WriteLine("Destination: " + dest.getSquareNumber());
                
                PieceType promoteTo = ((dest.getSquareNumber() <= 7 | dest.getSquareNumber() > 55) & (orig.getPiece().Equals(PieceType.p) | orig.getPiece().Equals(PieceType.P))) ? getPromotion(orig.getPiece()) : PieceType.Empty;
                
                // Debug prints promotion piece
                //Console.WriteLine("Promote To: " + promoteTo);
                Move current = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promoteTo);
                if (MoveCheck(current))
                {
                    Console.WriteLine("AN ACTUAL VALID MOVE");
                    this.position.makeMove(current, this.unmake);
                    OnRaiseBoardEvent(new BoardEvent(current, orig.getName() + dest.getName(), (movegen.legalMoves(this.position).Count == 0)));
                    if (current.promoteTo != PieceType.Empty)
                    {
                        //handle promotion
                    }
                    this.movePiece(current.origin, current.destination);
                    this.ColourBoard();

                    this.oneClick = false;
                }
                else
                {
                    moveQueue.Enqueue(dest);
                    Console.WriteLine(dest.getName() + " is the new start square");
                    this.ColourLegalMoves(dest.getSquareNumber());
                }
                
            }
            else
            {
                this.ColourLegalMoves(tapped.getSquareNumber());
                this.oneClick = true;
            }
        }

        public void ColourSquareBlue(int square)
        {
            getPositionSquareNumber(square).Background = Brushes.Blue;
        }

        public void ColourSquareRed(int square)
        {
            getPositionSquareNumber(square).Background = Brushes.Red;
        }

        private bool MoveCheck(Move m)
        {
            // Prints move tested
            //Console.WriteLine("Testing move from " + m.origin + " to " + m.destination + " with promoteTo " + m.promoteTo);
            return (movegen.legalMoves(this.position).Contains(m));
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