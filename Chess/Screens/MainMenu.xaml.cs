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

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Screen
    {
        /// <summary>
        /// Main screen upon entering the application
        /// </summary>
        public MainMenu(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();
            App.ReadOptionsFile();
            App.EnsureOptionsSet();
            App.CreateChessEngine();
            SetBackground();
        }

        /// <summary>
        /// Takes you to start game screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PushScreen(new GameBoard(parentWindow));
        }

        /// <summary>
        /// Takes you to the learning selection screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Learn_Button_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PushScreen(new TutorialOneScreen(parentWindow));
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PushScreen(new Settings(parentWindow));
            Console.WriteLine(Chess.Properties.Settings.Default.PreviousMove);
        }

        private void SetBackground()
        {
            // Create Image Element
            Image myImage = new Image();
            myImage.Width = 1280;
            myImage.Height = 720;

            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\MainMenuSplash.png");
            myBitmapImage.DecodePixelWidth = 1280;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            MainMenuScreen.Background = new ImageBrush(myImage.Source);
            



            // Create Image Element
            myImage = new Image();
            myImage.Width = 62;
            myImage.Height = 62;

            // Create source
            myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\Icons\game.png");
            myBitmapImage.DecodePixelWidth = 500;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            Start_Icon.Background = new ImageBrush(myImage.Source);
            Start_Icon.IsHitTestVisible = false;




            // Create Image Element
            myImage = new Image();
            myImage.Width = 62;
            myImage.Height = 62;

            // Create source
            myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\Icons\learn.png");
            myBitmapImage.DecodePixelWidth = 500;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            Learn_Icon.Background = new ImageBrush(myImage.Source);
            Learn_Icon.IsHitTestVisible = false;



            // Create Image Element
            myImage = new Image();
            myImage.Width = 62;
            myImage.Height = 62;

            // Create source
            myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(App.getPath() + @"Images\Icons\settings.png");
            myBitmapImage.DecodePixelWidth = 500;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.IsHitTestVisible = false;
            Settings_Icon.Background = new ImageBrush(myImage.Source);
            Settings_Icon.IsHitTestVisible = false;
        }
    }
}
