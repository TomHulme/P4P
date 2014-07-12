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
        PawnMower pawnMower;

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
                case GameMode.PawnMower:
                    Console.Out.WriteLine("position before reset");
                    Console.Out.WriteLine(FENConverter.convertPositionToFEN(pawnMower.GetPosition()));

                    pawnMower.ResetPosition();
                    Console.Out.WriteLine("position after reset");
                    Console.Out.WriteLine(FENConverter.convertPositionToFEN(pawnMower.GetPosition()));

                    board.SetPosition(pawnMower.GetPosition());
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

        }

        /// <summary>
        /// Start the pawn tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pawn_Click(object sender, RoutedEventArgs e)
        {
            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.P);
            tutorialOne.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the king tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void King_Click(object sender, RoutedEventArgs e)
        {
            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.K);
            tutorialOne.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the rook tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rook_Click(object sender, RoutedEventArgs e)
        {
            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.R);
            tutorialOne.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the bishop tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bishop_Click(object sender, RoutedEventArgs e)
        {
            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.B);
            tutorialOne.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the queen tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Queen_Click(object sender, RoutedEventArgs e)
        {
            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.Q);
            tutorialOne.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the knight tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Knight_Click(object sender, RoutedEventArgs e)
        {
            currentMode = GameMode.Tutorial;
            tutorialOne.SetPiece(GameLogic.PieceType.N);
            tutorialOne.SetInitialPosition();
            UpdateBoard();
        }
        
        /// <summary>
        /// Start a pawn game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Pawn_Game_Click(object sender, RoutedEventArgs e)
        {
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
            currentMode = GameMode.PawnMower;
            //Default piece for pawn mower challenges is a Rook
            PieceType piece = PieceType.R;

            switch (tutorialOne.GetPiece())
            {
                case PieceType.P:
                case PieceType.p:
                    break;
                case PieceType.K:
                case PieceType.k:
                    break;
                case PieceType.R:
                case PieceType.r:
                    piece = PieceType.R;
                    break;
                case PieceType.B:
                case PieceType.b:
                    piece = PieceType.B;
                    break;
                case PieceType.Q:
                case PieceType.q:
                    piece = PieceType.Q;
                    break;
                case PieceType.N:
                case PieceType.n:
                    piece = PieceType.N;
                    break;
                default:
                    break;
            }
            pawnMower = new PawnMower(piece, 5);

            Console.Out.WriteLine(FENConverter.convertPositionToFEN(pawnMower.GetPosition()));

            board.SetPosition(pawnMower.GetPosition());
            board.UpdateBoard();
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