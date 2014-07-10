using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLogic;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using System.Windows;

namespace Chess
{
    class GameScreen : Grid
    {
        public bool flipped;
        private Board board;
        private Position position;
        private TextBlock whiteText;
        private TextBlock blackText;

        public GameScreen(bool b, Position pos){
            this.flipped = b;
            this.position = pos;
            this.board = new Board(b, pos, this);
            board.setup();
            this.Surscribe(this.board);
            this.whiteText = new TextBlock();
            this.whiteText.Width = 200;
            this.whiteText.Height = 700;
            this.whiteText.Background = Brushes.White;
            this.whiteText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.blackText = new TextBlock();
            this.blackText.Width = 200;
            this.blackText.Height = 700;
            this.blackText.Background = Brushes.White;
            this.blackText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            ColumnDefinition paddingCol1 = new ColumnDefinition();
            ColumnDefinition whiteTextCol = new ColumnDefinition();
            ColumnDefinition boardCol = new ColumnDefinition();
            ColumnDefinition blackTextCol = new ColumnDefinition();
            ColumnDefinition paddingCol2 = new ColumnDefinition();
            paddingCol1.Width = new System.Windows.GridLength(90);
            this.ColumnDefinitions.Add(paddingCol1);
            whiteTextCol.Width = new System.Windows.GridLength(250);
            this.ColumnDefinitions.Add(whiteTextCol);
            boardCol.Width = new System.Windows.GridLength(600);
            this.ColumnDefinitions.Add(boardCol);
            blackTextCol.Width = new System.Windows.GridLength(250);
            this.ColumnDefinitions.Add(blackTextCol);
            paddingCol2.Width = new System.Windows.GridLength(90);
            this.ColumnDefinitions.Add(paddingCol2);


            Grid.SetColumn(this.whiteText, 1);
            Grid.SetColumn(this.board, 2);
            Grid.SetColumn(this.blackText, 3);
            this.board.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.blackText.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            
            this.Children.Add(this.board);
            this.Children.Add(this.whiteText);
            this.Children.Add(this.blackText);

            this.whiteText.Foreground = Brushes.Black;
            this.blackText.Foreground = Brushes.Black;
        }



        private void Checkmate()
        {
            Console.WriteLine("CHECKMATE!" + ((this.position.whiteMove) ? "\nBlack wins!" : "\nWhite wins!"));
            Canvas winScreen = new Canvas();
            winScreen.Width = 600;
            winScreen.Height = 300;
            winScreen.Background = Brushes.GreenYellow;
            winScreen.Opacity = 1.0;
            TextBlock winText = new TextBlock();
            winText.Text = "CHECKMATE!" + ((this.position.whiteMove) ? "\nBlack wins!" : "\nWhite wins!");
            winText.FontSize = 80;
            winText.TextAlignment = TextAlignment.Center;
            winText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            winText.Foreground = ((this.position.whiteMove) ? Brushes.Black : Brushes.White);
            winScreen.Children.Add(winText);
            winScreen.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            winScreen.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Grid.SetColumnSpan(winScreen, 5);
            this.Children.Add(winScreen);
        }


        private void Surscribe(Board board)
        {
            board.RaiseBoardEvent += HandleBoardEvent;
        }

        // Define what actions to take when the event is raised. 
        void HandleBoardEvent(object sender, BoardEvent e)
        {
            Console.WriteLine("Handled Move from " + e.Move.origin + " to " + e.Move.destination);
            Console.WriteLine(e.MoveString);
            String move = MoveParser.moveObjectToString(e.Move) + "\n";
            // Virus line
            //move += char.IsLower(((char)this.board.getPieceForSquareNumber(e.Move.origin))) ? "Black " : "White ";
            if (char.IsLower(board.getPieceForSquareNumber(e.Move.destination).ToString()[0]))
            {
                blackText.Text = blackText.Text + move;
            }
            else
            {
                whiteText.Text = whiteText.Text + move;
            }
            

            if (e.CheckMate)
            {
                this.Checkmate();
            }
            //(char.IsLower(this.position.getPiece(e.Move.origin).ToString(), 0)) ? (this.blackText.Text += "\nBlack " + this.position.getPiece(e.Move.origin)) : ("White");
        }
    }
}
