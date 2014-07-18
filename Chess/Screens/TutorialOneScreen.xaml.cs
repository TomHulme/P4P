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
using Tutorials;
using Tutorials.Challenges;
using GameLogic;
using Chess;
using Chess.Screens.TutorialDialogs;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for TutorialOneScreen.xaml
    /// </summary>
    public partial class TutorialOneScreen : Screen
    {
        enum GameMode { Tutorial, PawnGame, PawnMower};

        TutorialBoard board;
        TutorialOne tutorialOne;
        GameMode currentMode;
        Brush originalColour;

        /// <summary>
        /// Screen for introducing the pieces
        /// </summary>
        /// <param name="parentWindow"></param>
        public TutorialOneScreen(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;

            tutorialOne = new TutorialOne();
            currentMode = GameMode.Tutorial;
            board = new TutorialBoard(false, tutorialOne.GetPosition());
            board.UpdateBoard();

            BoardArea.Content = board;

            originalColour = Board_Button.Background;
        }

        /// <summary>
        /// Go back to the previous screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PopScreen();
        }

        /// <summary>
        /// Reset the board to how it started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Board_Click(object sender, RoutedEventArgs e)
        {
            switch (currentMode)
            {
                case GameMode.Tutorial:
                    tutorialOne.SetInitialPosition();
                    UpdateBoard();
                    break;
                case GameMode.PawnGame:
                    PawnGame pawnGame = new PawnGame();
                    board.SetPosition(pawnGame.GetPosition());
                    board.UpdateBoard();
                    break;
            }
            
        }

        /// <summary>
        /// Start the board tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = new BoardDialog(tutorialOne, board, this);

            tutorialOne.ClearBoard();
            UpdateBoard();

            Board_Button.Background = Brushes.SlateGray;
            Pawn_Button.Background = originalColour;
            King_Button.Background = originalColour;
            Rook_Button.Background = originalColour;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = originalColour;
        }

        /// <summary>
        /// Start the pawn tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pawn_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = null;

            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.P);
            tutorialOne.SetInitialPosition();
            UpdateBoard();

            Board_Button.Background = originalColour;
            Pawn_Button.Background = Brushes.SlateGray;
            King_Button.Background = originalColour;
            Rook_Button.Background = originalColour;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = originalColour;
        }

        /// <summary>
        /// Start the king tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void King_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = null;

            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.K);
            tutorialOne.SetInitialPosition();
            UpdateBoard();

            Board_Button.Background = originalColour;
            Pawn_Button.Background = originalColour;
            King_Button.Background = Brushes.SlateGray;
            Rook_Button.Background = originalColour;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = originalColour;
        }

        /// <summary>
        /// Start the rook tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rook_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = null;

            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.R);
            tutorialOne.SetInitialPosition();
            UpdateBoard();

            Board_Button.Background = originalColour;
            Pawn_Button.Background = originalColour;
            King_Button.Background = originalColour;
            Rook_Button.Background = Brushes.SlateGray;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = originalColour;
        }

        /// <summary>
        /// Start the bishop tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bishop_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = null;

            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.B);
            tutorialOne.SetInitialPosition();
            UpdateBoard();

            Board_Button.Background = originalColour;
            Pawn_Button.Background = originalColour;
            King_Button.Background = originalColour;
            Rook_Button.Background = originalColour;
            Bishop_Button.Background = Brushes.SlateGray;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = originalColour;
        }

        /// <summary>
        /// Start the queen tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Queen_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = null;

            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.Q);
            tutorialOne.SetInitialPosition();
            UpdateBoard();

            Board_Button.Background = originalColour;
            Pawn_Button.Background = originalColour;
            King_Button.Background = originalColour;
            Rook_Button.Background = originalColour;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = Brushes.SlateGray;
            Knight_Button.Background = originalColour;
        }

        /// <summary>
        /// Start the knight tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Knight_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = null;

            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.N);
            tutorialOne.SetInitialPosition();
            UpdateBoard();

            Board_Button.Background = originalColour;
            Pawn_Button.Background = originalColour;
            King_Button.Background = originalColour;
            Rook_Button.Background = originalColour;
            Bishop_Button.Background = originalColour;
            Queen_Button.Background = originalColour;
            Knight_Button.Background = Brushes.SlateGray;
        }
        
        /// <summary>
        /// Start a pawn game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Pawn_Game_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Content = null;

            currentMode = GameMode.PawnGame;
            PawnGame pawnGame = new PawnGame();

            board.SetPosition(pawnGame.GetPosition());
            board.UpdateBoard();
        }

        /// <summary>
        /// Start a pawn mower challenge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Pawn_Mower_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PushScreen(new PawnMowerScreen(parentWindow));
        }

        /// <summary>
        /// Update the board to show the current position
        /// </summary>
        private void UpdateBoard()
        {
            board.SetPosition(tutorialOne.GetPosition());
            board.UpdateBoard();
        }
    }
}