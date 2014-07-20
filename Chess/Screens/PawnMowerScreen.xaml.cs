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
using Chess.Screens.TutorialDialogs;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for PawnMowerScreen.xaml
    /// </summary>
    public partial class PawnMowerScreen : Screen
    {
        ScreenControl parentWindow;
        PieceType userPiece;
        PawnMower pawnMower;
        Brush originalColour;
        TutorialBoard board;
        int count = 5;

        public PawnMowerScreen(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();

            this.parentWindow = parentWindow;

            userPiece = PieceType.R;
            pawnMower = new PawnMower(userPiece, count);

            board = new TutorialBoard(false, pawnMower.GetPosition());
            BoardArea.Content = board;
            board.UpdateBoard();

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
            Update();
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PopScreen();
        }

        private void Update()
        {
            board.SetPosition(pawnMower.GetPosition());
            board.UpdateBoard();
        }

        public void NewChallenge()
        {
            //create new pawn mower challenge
            pawnMower = new PawnMower(userPiece, count);
            //update board
            Update();
        }

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
