using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using GameLogic;
using System.Threading;
using Microsoft.Surface.Presentation.Controls;

namespace Chess
{
    class Board : Canvas
    {
        /*
         * Class Vars
         */
        private Square[] squares;
        private ArrayList tagVisualisations;
        public bool flipped;
        private Position position;
        private GameController gamecon;
        private bool blackReverse = false;
        

        /**
         * Constructors for the Board Class
         */
        public Board(bool b, Position pos, GameController gc)
        {
            this.flipped = b; // Not implemented. Would be used to change the orientation of the game board.
            this.position = pos;
            this.gamecon = gc;
        }
        public Board(bool b, Position pos, GameController gc, bool blkRev) : this(b, pos, gc)
        {
            blackReverse = blkRev;
        }

        /**
         * Public Interface
         */
        public PieceType getPieceForSquareNumber(int square)
        {
            return this.getSquareForNumber(square).getPiece();
        }


        /**
         * Sets the board, drawing Squares and placing pieces.
         */
        internal void setup()
        {
            this.drawBoard();
            this.placePieces();
            this.printNextTurn();
        }
		
		/**
		 * Draws the Board. 600x600 Pixels.
		 * Adds EventHandlers to the Squares
		 */
        internal void drawBoard()
        {
            this.Width = 600;
            this.Height = 600;
            squares = new Square[64];
            for (int i = 0; i < 8; i++)
            {
                squares[i] = new Square(getSquareName(i, 0), this.getSquareNumber(i, 0));
                if (gamecon.debugging)
                {
                    squares[i].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                }
                squares[i].AddHandler(ButtonBase.TouchDownEvent, new RoutedEventHandler(TappedSquare), true);
                
                this.Children.Add(squares[i]);
                Canvas.SetTop(squares[i], 0);
                Canvas.SetLeft(squares[i], i * 75);
                for (int j = 1; j < 8; j++)
                {
                    squares[i + j * 8] = new Square(getSquareName(i, j), this.getSquareNumber(i, j));
                    if (gamecon.debugging)
                    {
                        squares[i + j * 8].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                    }
                    squares[i + j * 8].AddHandler(ButtonBase.TouchDownEvent , new RoutedEventHandler(TappedSquare), true);
                    
                    this.Children.Add(squares[i + j * 8]);
                    Canvas.SetTop(squares[i + j * 8], j * 75);
                    Canvas.SetLeft(squares[i + j * 8], i * 75);
                }
            }
			
			// Add TagVisalizers to the Squares, routing to the RecognizedSquare method
            foreach (Square s in squares)
            {
                
                s.MyTagVisualizer.AddHandler(TagVisualizer.VisualizationAddedEvent, new RoutedEventHandler(RecognizedSquare), true);
                s.MyTagVisualizer.AddHandler(TagVisualizer.VisualizationRemovedEvent, new RoutedEventHandler(RecognizedSquare), true);
            }

            this.ColourBoard();
        }

		/**
		 * Colours the Board to its set texture
		 * Wood Textures decided at random from list of wood square textures
		 */
        internal void ColourBoard()
        {
            if (!Chess.Properties.Settings.Default.WoodTextures)
            {
				// Grey/White Squares
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
                        this.squares[i * 8 + j].colourBorder(Brushes.Black);
                    }
                }
            }
            else
            {
				// Wood Textures
                Random r = new Random();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (this.flipped)
                        {
                            if ((i + j) % 2 == 0) { this.squares[i * 8 + j].Background = getTextureImageBrush(false, r.Next(1, 6)); }
                            else { this.squares[i * 8 + j].Background = getTextureImageBrush(true, r.Next(1, 6)); }
                        }
                        else
                        {
                            if ((i + j) % 2 == 0) { this.squares[i * 8 + j].Background = getTextureImageBrush(true, r.Next(1, 6)); }
                            else { this.squares[i * 8 + j].Background = getTextureImageBrush(false, r.Next(1, 6)); }
                        }
                        this.squares[i * 8 + j].colourBorder(Brushes.Black);
                    }
                }
            }
        }
		
		/**
		 * Gets a texture brush for a texture image for the selected square colour.
		 * Currently the directory only contains Wooden Textures
		 */
        private ImageBrush getTextureImageBrush(bool isWhite, int imageNum){
			// From http://msdn.microsoft.com/en-us/library/vstudio/aa970269(v=vs.100).aspx
            // Create Image Element
            Image myImage = new Image();
            myImage.Width = 75;
            myImage.Height = 75;

            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\SquareImages\" + (isWhite ? "W" : "B" ) + imageNum.ToString() + ".png");
            myBitmapImage.DecodePixelWidth = 75;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            return new ImageBrush(myImage.Source);
        }

		/**
		 * Removes colours from the Rectangle layer of Squares.
		 * Does not affect the Canvas Background which is used for the background texture.
		 */
        internal void UnColourBoard(Brush colour)
        {
            foreach (Square s in squares)
            {
                if (s.rectangle.Fill == colour)
                {
                    s.colourRectangle(Brushes.Transparent);
                }
            }
        }

		/**
		 * Removes colours from the Borders of all squares.
		 */
        internal void UnColourBorders()
        {
            foreach (Square s in squares)
            {
                s.rectangle.StrokeThickness = 1;
                s.rectangle.Stroke = Brushes.Black;
            }
        }

		/**
		 * Sets the internal position of the Board
		 * Resets piece images to match this change
		 */
        internal void SetPosition(Position position)
        {
            this.position = position;
            this.placePieces();
            this.printNextTurn();
        }

		/**
		 * Colour a square a certain colour based off its number.
		 */
        public void ColourSquare(int square, Brush colour)
        {
            getSquareForNumber(square).colourRectangle(colour);
        }

		/**
		 * Colour a square's border a certain colour based off its number.
		 */
        public void ColourSquareBorder(int square, Brush colour)
        {
            getSquareForNumber(square).rectangle.StrokeThickness = 3;
            getSquareForNumber(square).colourBorder(colour);
        }

		/**
		 * Gets the algebraic name for a square based on its row/coloumn on the Board.
		 */
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

		/**
		 * Gets the algebraic name for a square based on its square number.
		 */
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

		/**
		 * Gets the number for a square based on its position in the squares array
		 */
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

		/**
		 * Sets the pieces on the board. Removes pieces if they should not be there.
		 */
        internal void placePieces()
        {
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceType current = this.position.getPiece((((7-i) * 8) + j));
                    if (current != PieceType.Empty)
                    {
                        squares[((i * 8) + j)].clearPieceImage();
                        this.drawPiece(current, squares[((i * 8) + j)]);
                    }
                    else
                    {
                        squares[((i * 8) + j)].clearPieceImage();
                    }
                }
            }
        }

		/**
		 * Gets the instance of a Square based off its Square Number
		 */
        internal Square getSquareForNumber(int square)
        {
            foreach (Square s in squares)
            {
                if (s.getSquareNumber() == square) { return s; }
            }
            throw new Exception("ERROR! Could not find square.");
        }

		/**
		 * Gets the instance of a Square based off its algebraic name
		 */
        internal Square getSquareForName(string name)
        {
            foreach (Square s in squares)
            {
                if (s.getName() == name) { return s; }
            }
            throw new Exception("ERROR! Could not find square.");
        }

		/**
		 * Draws the image for a Piece on top of a Square
		 */
        internal void drawPiece(PieceType piece, Square sq)
        {
            string[] pieces = { "black_king", "black_queen", "black_rook", "black_bishop", "black_knight", "black_pawn", "white_king", "white_queen", "white_rook", "white_bishop", "white_knight", "white_pawn" };
            string pieceString = "";
			// Get the Piece String (filename) for the piece given
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
				// None of the above, set as empty and return.
                sq.setPiece(PieceType.Empty);
                return;
            }
            // From site http://msdn.microsoft.com/en-us/library/vstudio/aa970269(v=vs.100).aspx
            // Create Image Element
            Image myImage = new Image();
            myImage.Width = 75;

            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\PieceImages\" + pieceString + ".png");
            myBitmapImage.DecodePixelWidth = 75;
            if (this.flipped & this.blackReverse & pieceString.StartsWith("b")) { myBitmapImage.Rotation = Rotation.Rotate270; }
            else if (this.blackReverse & pieceString.StartsWith("b")) { myBitmapImage.Rotation = Rotation.Rotate180; }
            else if (this.flipped) { myBitmapImage.Rotation = Rotation.Rotate90; }
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            sq.setPieceImage(myImage);
            sq.setPiece(piece);
            myImage.SetValue(TextBlock.TextProperty, pieceString);
            
        }

        /**
         * Square Tapped Event
		 * Routes tapped square events to GameController
         */
        internal void TappedSquare(object sender, RoutedEventArgs e)
        {
            Square tapped = (Square)sender;
            if (this.gamecon.debugging)
            {
                Console.WriteLine(tapped.getPiece());
            }
            if (!Chess.Properties.Settings.Default.UseObjectRecognition)
            {
                this.gamecon.MoveHandler(tapped);
            }
        }

        /**
         * Square with Object Recognized Event
		 * Routes square to GameController
         */
        internal void RecognizedSquare(object sender, RoutedEventArgs e)
        {
            TagVisualizer tapped = (TagVisualizer)sender;
            this.gamecon.MoveHandler((Square)((Grid)tapped.Parent).Parent);
            Console.WriteLine("Recognized tag on {0}", ((Square)((Grid)tapped.Parent).Parent).getName());
        }

        /*
         * Move related methods
         */

        internal void printNextTurn()
        {
            Console.WriteLine(this.position.whiteMove ? "White to move." : "Black to move.");
        }

    }
}

/*** DEPRECATED
 ** BoardEvent Class 
 ** WAS Used to give information on Board Events
 **/
public class BoardEvent : EventArgs
{
	// Board Event Constructor
    public BoardEvent(Move m, String ms, Boolean cm)
    {
        move = m;
        moveString = ms;
        checkMate = cm;
    }
	// Class Variables
    private Move move;
    private String moveString;
    private Boolean checkMate;

	/*
	 Getters and Setters
	*/
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