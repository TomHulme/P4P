using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLogic;
using Chess;
using Tutorials;

namespace Chess.Screens.TutorialDialogs
{
    /// <summary>
    /// Interaction logic for PieceDialog.xaml
    /// </summary>
    public partial class PieceDialog : UserControl
    {
        PieceType piece;
        TutorialOneScreen parentScreen;

        public PieceDialog(PieceType piece, TutorialOneScreen parentScreen)
        {
            InitializeComponent();

            this.piece = piece;
            this.parentScreen = parentScreen;

            FillInText();
        }

        private void FillInText()
        {
            switch (this.piece)
            {
                case PieceType.P:
                    DialogText.Text = "The Pawn moves forward exactly one space, or optionally, two spaces when on its starting square, toward the opponents side of the board. ";
                    DialogText.Text += "If the pawn reaches a square on the back rank of the opponent, it can be promoted to the player's choice of a Queen, Rook, Bishop, or Knight. ";
                    DialogText.Text += "A Pawn may capture an enemy piece one square diagonally ahead of the Pawn, either left or right.";
                    break;
                case PieceType.K:
                    DialogText.Text = "The King moves exactly one vacant square in any direction, forwards, backwards, left, right, or diagonally. ";
                    break;
                case PieceType.R:
                    DialogText.Text = "The Rook can move any number of vacant squares forwards, backwards, left, or right in a straight line. ";
                    break;
                case PieceType.B:
                    DialogText.Text = "The Bishop can move any number of vacant sqaures diagonally in a straight line. ";
                    DialogText.Text += "Consequently, a Bishop stays on the squares of the same colour throughout the game. ";
                    DialogText.Text += "The two Bishops each player starts with move on squares of opposite colours.";
                    break;
                case PieceType.Q:
                    DialogText.Text = "The Queen can move any number of vacant squares in any direction, forwards, backwards, left, right, or diagonally, in a straight line. ";
                    break;
                case PieceType.N:
                    DialogText.Text = "The Knight moves on an extended diagonal from one corner of any 2 by 3 rectangle of squares to the furthest opposite corner. ";
                    DialogText.Text += "Consequently, the Knight alternates its squares colour each time it moves. ";
                    DialogText.Text += "The Knight is the only piece that jumps over any intervening pieces when moving.";
                    break;
            }
        }

        private void Captures_Quiz_Click(object sender, RoutedEventArgs e)
        {
            //Set up board with an enemy piece. User has to take piece.
            //May need seperate controller for these to ensure continuity.
        }

        private void Moves_Quiz_Click(object sender, RoutedEventArgs e)
        {
            //set up board with a highlighted square. User has to navigate
            //to the lit square
        }

        private void Captures_Click(object sender, RoutedEventArgs e)
        {
            //setup board with a possible capture scenario
        }

        private void Moves_Click(object sender, RoutedEventArgs e)
        {
            //explain how a piece moves in writing.
            //also show possible moves that can be made
        }
    }
}
