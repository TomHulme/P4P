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
using Tutorials.Challenges;
using Tutorials;
using Chess;
using Chess.Screens.Dialogs;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for PawnMowerScreen.xaml
    /// </summary>
    public partial class PawnMowerScreen : Screen
    {
        PieceType userPiece;
        PawnMower pawnMower;
        Brush originalColour;
        GameController gameController;
        int count = 4;

        public PawnMowerScreen(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();

            userPiece = PieceType.R;
            pawnMower = new PawnMower(userPiece, count);

            gameController = new GameController(false, pawnMower.GetPosition());
            gameController.ignoreSuggestion = true;
            BoardArea.Content = gameController.board;

            Dialog.Content = new PawnMowerDialog(this);

            originalColour = Rook_Button.Background;
            Rook_Button.Background = Brushes.SlateGray;
        }

        private void Rook_Click(object sender, RoutedEventArgs e)
        {
            userPiece = PieceType.R;

            NewChallenge();

            Rook_Button.Background = Brushes.SlateGray;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = originalColour;
        }

        private void Bishop_Click(object sender, RoutedEventArgs e)
        {
            userPiece = PieceType.B;

            NewChallenge();

            Rook_Button.Background = originalColour;
            Bishop_Button.Background = Brushes.SlateGray;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = originalColour;
        }

        private void Queen_Click(object sender, RoutedEventArgs e)
        {
            userPiece = PieceType.Q;

            NewChallenge();

            Rook_Button.Background = originalColour;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = Brushes.SlateGray;
            Knight_Button.Background = originalColour;
        }

        private void Knight_Click(object sender, RoutedEventArgs e)
        {
            userPiece = PieceType.N;

            NewChallenge();

            Rook_Button.Background = originalColour;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = Brushes.SlateGray;
        }

        private void Reset_Board_Click(object sender, RoutedEventArgs e)
        {
            pawnMower.ResetPosition();
            UpdateBoard();
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PopScreen();
        }

        private void UpdateBoard()
        {
            gameController.SetPosition(pawnMower.GetPosition());
        }

        public void NewChallenge()
        {
            //create new pawn mower challenge
            pawnMower = new PawnMower(userPiece, count);
            //update board
            UpdateBoard();
        }

        /*
         * Used for the slider in the pawn mower dialog
         */
        public int GetCount()
        {
            return count;
        }

        public void SetCount(int count)
        {
            this.count = count;
        }
    }
}
