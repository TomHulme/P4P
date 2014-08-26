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
        GameInfoDialog gameInfoDialogBottom;
        GameSettingsDialog gameSettingsDialogBottom;

        GameInfoDialog gameInfoDialogTop;
        GameSettingsDialog gameSettingsDialogTop;

        public GameBoard(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();

            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), false, false);
            ResetDialogs();
            SetTopDialogs();
            ColourBackgrounds();
        }

        private void ColourBackgrounds()
        {
            if (Chess.Properties.Settings.Default.WoodTextures)
            {
                this.TopPanel.Background = getTextureImageBrush(false);
                this.BottomPanel.Background = getTextureImageBrush(true);
            }
            else
            {
                this.TopPanel.Background = Brushes.DarkGray;
                this.BottomPanel.Background = Brushes.WhiteSmoke;
            }
        }

        private ImageBrush getTextureImageBrush(bool isWhite)
        {
            // Create Image Element
            Image myImage = new Image();
            myImage.Width = 1280;
            myImage.Height = 360;

            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\" + (isWhite ? "light" : "dark") + "wood.jpg");
            myBitmapImage.DecodePixelWidth = 1280;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            return new ImageBrush(myImage.Source);
        }

        private void ResetDialogs()
        {
            this.gameInfoDialogBottom = new GameInfoDialog(gameController);
            this.gameSettingsDialogBottom = new GameSettingsDialog(gameController, true);

            BottomCenterControl.Content = this.gameInfoDialogBottom;
            BottomRightControl.Content = this.gameSettingsDialogBottom;

            BoardArea.Content = gameController.board;
        }

        private void SetTopDialogs()
        {
            this.gameInfoDialogTop = new GameInfoDialog(gameController);
            this.gameSettingsDialogTop = new GameSettingsDialog(gameController, false);

            RotateTransform rotateInfo = new RotateTransform(180, 300, 30);
            RotateTransform rotateSettings = new RotateTransform(180, 170, 180);

            this.gameInfoDialogTop.RenderTransform = rotateInfo;
            this.gameSettingsDialogTop.RenderTransform = rotateSettings;

            TopCenterControl.Content = this.gameInfoDialogTop;
            TopLeftControl.Content = this.gameSettingsDialogTop;
        }

        private void ClearTopDialogs()
        {
            this.gameInfoDialogTop = null;
            this.gameSettingsDialogTop = null;

            TopCenterControl.Content = this.gameInfoDialogTop;
            TopLeftControl.Content = this.gameSettingsDialogTop;
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
            SetTopDialogs();

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }

        private void PVC_Click(object sender, RoutedEventArgs e)
        {
            this.gameController.EndCvCGame = true;
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), true, false);
            ResetDialogs();
            ClearTopDialogs();

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }

        private void CVC_Click(object sender, RoutedEventArgs e)
        {
            this.gameController.EndCvCGame = true;
            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), true, true);
            ResetDialogs();
            ClearTopDialogs();

            this.New_Game_Buttons.Height = 0;
            this.New_Game_Buttons.Visibility = Visibility.Collapsed;
        }
    }
}
