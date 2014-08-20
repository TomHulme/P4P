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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLogic;
using Tutorials;
using Chess.Screens.Dialogs;
using Microsoft.Surface.Presentation.Controls;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Screen
    {
        GameController gameController;
        GameInfoDialog gameInfoDialog;
        GameSettingsDialog gameSettingsDialog;

        public GameBoard(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();

            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition));
            ResetDialogs();
        }

        private void ResetDialogs()
        {
            this.gameInfoDialog = new GameInfoDialog(gameController);
            this.gameSettingsDialog = new GameSettingsDialog(gameController);

            BottomCenterControl.Content = this.gameInfoDialog;
            BottomRightControl.Content = this.gameSettingsDialog;

            BoardArea.Content = gameController.board;
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PopScreen();
        }

        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            Storyboard expandButton = this.FindResource("Expand") as Storyboard;

            this.New_Game_Buttons.Visibility = Visibility.Visible;
            expandButton.Begin();
        }

        private void PVP_Click(object sender, RoutedEventArgs e)
        {
            this.gameController.EndCvCGame = true;
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), false, false);
            ResetDialogs();

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }

        private void PVC_Click(object sender, RoutedEventArgs e)
        {
            this.gameController.EndCvCGame = true;
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), true, false);
            ResetDialogs();

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }

        private void CVC_Click(object sender, RoutedEventArgs e)
        {
            this.gameController.EndCvCGame = true;
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), true, true);
            ResetDialogs();

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }
    }
}
