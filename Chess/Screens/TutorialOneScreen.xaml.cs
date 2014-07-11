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
using Chess;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for TutorialOneScreen.xaml
    /// </summary>
    public partial class TutorialOneScreen : Screen
    {
        TutorialBoard board;
        TutorialOne template;

        /// <summary>
        /// Screen for introducing the pieces
        /// </summary>
        /// <param name="parentWindow"></param>
        public TutorialOneScreen(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;

            template = new TutorialOne();
            board = new TutorialBoard(false, template.GetPosition());
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
        private void Reset_Challenge_Click(object sender, RoutedEventArgs e)
        {
            template.SetInitialPosition();
            UpdateBoard();
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
            template.SetPiece(GameLogic.PieceType.P);
            template.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the king tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void King_Click(object sender, RoutedEventArgs e)
        {
            template.SetPiece(GameLogic.PieceType.K);
            template.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the rook tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rook_Click(object sender, RoutedEventArgs e)
        {
            template.SetPiece(GameLogic.PieceType.R);
            template.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the bishop tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bishop_Click(object sender, RoutedEventArgs e)
        {
            template.SetPiece(GameLogic.PieceType.B);
            template.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the queen tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Queen_Click(object sender, RoutedEventArgs e)
        {
            template.SetPiece(GameLogic.PieceType.Q);
            template.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Start the knight tutorial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Knight_Click(object sender, RoutedEventArgs e)
        {
            template.SetPiece(GameLogic.PieceType.N);
            template.SetInitialPosition();
            UpdateBoard();
        }

        /// <summary>
        /// Update the board to show the current position
        /// </summary>
        private void UpdateBoard()
        {
            board.SetPosition(template.GetPosition());
            board.UpdateBoard();
        }
    }
}
