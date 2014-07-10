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
    /// Interaction logic for EmptyBoard.xaml
    /// </summary>
    public partial class EmptyBoard : Screen
    {
        GameScreen gameScreen;

        public EmptyBoard(ScreenControl parentWindow, GameScreen gameScreen) : base(parentWindow)
        {
            InitializeComponent();

            this.gameScreen = gameScreen;

            boardArea.Content = gameScreen;
        }
    }
}
