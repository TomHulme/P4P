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
using Tutorials.Challenges;

namespace Chess.Screens.Dialogs
{
    /// <summary>
    /// Interaction logic for PawnGameDialog.xaml
    /// </summary>
    public partial class PawnGameDialog : UserControl
    {
        GameController gameController;
        TutorialOneScreen parentScreen;
        PawnGame pawnGame;

        public PawnGameDialog(TutorialOneScreen parentScreen, PawnGame pawnGame)
        {
            InitializeComponent();

            this.parentScreen = parentScreen;
            this.pawnGame = pawnGame;
            this.gameController = new GameController(false, pawnGame.GetPosition(), false, false);

            this.gameController.RaiseControllerEvent += new EventHandler<ControllerEvent>(gameController_MoveText);
            this.parentScreen.BoardArea.Content = this.gameController.board;
        }

        void gameController_MoveText(object sender, ControllerEvent e)
        {
            if (gameController.position.whiteMove)
            {
                SetPawnGameText("White To Move");
            }
            else
            {
                SetPawnGameText("Black To Move");
            }
        }

        internal void SetPawnGameText(String text)
        {
            PawnGameText.Text = text;
        }

        private void ShowAttacked_Button_Click(object sender, RoutedEventArgs e)
        {
            gameController.ShowAttackedPieces = gameController.ShowAttackedPieces ? false : true;

            if (gameController.ShowAttackedPieces)
            {
                ShowAttacked_Button.Background = Chess.Properties.Settings.Default.AttackedPieces;
            }
            else
            {
                ShowAttacked_Button.Background = Brushes.DarkGray;
            }
        }

        private void ShowMoves_Button_Click(object sender, RoutedEventArgs e)
        {
            gameController.ShowHighlightedMoves = gameController.ShowHighlightedMoves ? false : true;

            if (gameController.ShowHighlightedMoves)
            {
                ShowMoves_Button.Background = Chess.Properties.Settings.Default.HighlightMove;
            }
            else
            {
                ShowMoves_Button.Background = Brushes.DarkGray;
            }
        }

        private void VSPlayer_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("clicked the versus player button.");
            pawnGame.ResetPosition();

            this.gameController = new GameController(false, pawnGame.GetPosition(), false, false);
            this.gameController.RaiseControllerEvent += new EventHandler<ControllerEvent>(gameController_MoveText);

            parentScreen.BoardArea.Content = gameController.board;

            gameController_MoveText(new object(), new ControllerEvent());
        }

        private void VSComputer_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("clicked the versus computer button.");
            pawnGame.ResetPosition();

            this.gameController = new GameController(false, pawnGame.GetPosition(), true, false);
            this.gameController.RaiseControllerEvent += new EventHandler<ControllerEvent>(gameController_MoveText);

            parentScreen.BoardArea.Content = gameController.board;

            gameController_MoveText(new object(), new ControllerEvent());
        }
    }
}
