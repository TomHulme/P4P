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
            parentWindow.PushScreen(new Learn(parentWindow));
        }

        private void Tutorial_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
