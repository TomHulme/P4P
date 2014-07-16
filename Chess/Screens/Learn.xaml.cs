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
    /// Interaction logic for Learn.xaml
    /// </summary>
    public partial class Learn : Screen
    {
        public Learn(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();
        }

        private void Challenges_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PushScreen(new Challenges(parentWindow));
        }

        private void Introduction_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PushScreen(new TutorialOneScreen(parentWindow));
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PopScreen();
        }
    }
}
