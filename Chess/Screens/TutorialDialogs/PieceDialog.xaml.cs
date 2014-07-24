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
            if (piece == PieceType.P)
            {
                //explain that pawns capture differently to their normal move
            }
            else
            {
                //explain that a piece can capture an enemy piece by moving to the square
                //that piece occupies.
            }

            //setup board with a possible capture scenario
        }

        private void Moves_Click(object sender, RoutedEventArgs e)
        {
            //explain how a piece moves in writing.
            //also show possible moves that can be made
        }
    }
}
