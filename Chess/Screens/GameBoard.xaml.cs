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

        public GameBoard(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();

            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition));
            this.gameInfoDialog = new GameInfoDialog();

            BoardArea.Content = gameController.board;
            BottomCenterControl.Content = this.gameInfoDialog;
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
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), false, false);
            BoardArea.Content = gameController.board;

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }

        private void PVC_Click(object sender, RoutedEventArgs e)
        {
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), true, false);
            BoardArea.Content = gameController.board;

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }

        private void CVC_Click(object sender, RoutedEventArgs e)
        {
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), true, true);
            BoardArea.Content = gameController.board;

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }
    }
}
