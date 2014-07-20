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
using GameLogic;
using Tutorials;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Screen
    {
        //ScreenControl parentWindow;
        TutorialBoard board;

        public GameBoard(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();

            //this.parentWindow = parentWindow;

            this.board = new TutorialBoard(false, FENConverter.convertFENToPosition(FENConverter.startPosition));

            BoardArea.Content = board;
            board.UpdateBoard();
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PopScreen();
        }
    }
}
