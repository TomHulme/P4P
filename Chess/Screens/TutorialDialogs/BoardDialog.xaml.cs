using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tutorials;
using GameLogic;
using System.Threading;

namespace Chess.Screens.TutorialDialogs
{
    /// <summary>
    /// Interaction logic for BoardDialog.xaml
    /// </summary>
    partial class BoardDialog : UserControl
    {
        TutorialOne tutorialOne;
        GameController gameController;

        public BoardDialog(TutorialOne tutorialOne, GameController gameController)
        {
            InitializeComponent();

            this.tutorialOne = tutorialOne;
            this.gameController = gameController;
        }

        private void Ranks_Click(object sender, RoutedEventArgs e)
        {
            DialogText.Text = "The board is made up of ranks and files. Ranks are the rows of squares and are labeled 1 through 8.";

            String[] squares;

            for (int i = 1; i <= 8; i++)
            {
                //get square objects
                //squares = TutorialOne.HighLightRank(i);
                //ArrayList squareList = new ArrayList();
                //foreach (String s in squares)
                //{
                //    squareList.Add(gameController.board.getSquareForName(s));
                //}

                ////create animations
                //ArrayList storyboardList = new ArrayList();
                //foreach (Square s in squareList)
                //{
                //    storyboardList.Add(StoryBoardCreator.PulseSquareBackground(s, i));
                //}

                ////start storyboards
                //foreach (Storyboard s in storyboardList)
                //{
                //    s.Begin();
                //}
            }
        }

        private void Files_Click(object sender, RoutedEventArgs e)
        {
            DialogText.Text = "The board is made up of ranks and files. Files are the columns and are labeled A through H.";

            String[] files = { "a", "b", "c", "d", "e", "f", "g", "h" };

            for (int i = 0; i < 8; i++)
            {
                //String[] squares = TutorialOne.HighLightFile(files[i]);

                //foreach (String square in squares)
                //{
                //    tutorialBoard.ColourSquareBlue(FENConverter.getSquare(square));
                //    tutorialBoard.UpdateBoard();
                //}

                //    tutorialBoard.UpdateBoard();
            }
        }

        private void Ranks_Files_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Squares_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
