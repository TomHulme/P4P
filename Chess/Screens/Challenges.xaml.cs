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
    /// Interaction logic for Challenges.xaml
    /// </summary>
    public partial class Challenges : Screen
    {
        public Challenges(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();
        }

        private void Pawn_Game_Click(object sender, RoutedEventArgs e)
        {
            Tutorials.Challenges.PawnGame pawnGame = new Tutorials.Challenges.PawnGame();
            GameScreen gameScreen = new GameScreen(false, pawnGame.GetPosition());

            Screen screen = new EmptyBoard(parentWindow, gameScreen);

            parentWindow.PushScreen(screen);

        }

        private void Pawn_Mower_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PushScreen(new PawnMowerScreen(parentWindow));
        }
    }
}
