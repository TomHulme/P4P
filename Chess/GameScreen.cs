using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLogic;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;

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
            this.board = new Board(b, pos);
            board.setup();
            this.Surscribe(this.board);
            this.whiteText = new TextBlock();
            this.whiteText.Width = 300;
            this.whiteText.Height = 700;
            this.whiteText.Background = Brushes.White;
            this.whiteText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.blackText = new TextBlock();
            this.blackText.Width = 300;
            this.blackText.Height = 700;
            this.blackText.Background = Brushes.White;
            this.blackText.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            this.ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinition boardCol = new ColumnDefinition();
            boardCol.Width = new System.Windows.GridLength(700);
            this.ColumnDefinitions.Add(boardCol);
            this.ColumnDefinitions.Add(new ColumnDefinition());

            Grid.SetColumn(this.whiteText, 0);
            Grid.SetColumn(this.board, 1);
            Grid.SetColumn(this.blackText, 2);
            
            this.Children.Add(this.board);
            this.Children.Add(this.whiteText);
            this.Children.Add(this.blackText);

        }


        private void Surscribe(Board board)
        {
            board.RaiseBoardEvent += HandleBoardEvent;
        }

        // Define what actions to take when the event is raised. 
        void HandleBoardEvent(object sender, BoardEvent e)
        {
            Console.WriteLine("Handled Move from " + e.Move.origin + " to " + e.Move.destination);
            //(char.IsLower(this.position.getPiece(e.Move.origin).ToString(), 0)) ? (this.blackText.Text += "\nBlack " + this.position.getPiece(e.Move.origin)) : ("White");
        }
    }
}
