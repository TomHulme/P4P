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
using Microsoft.Surface.Presentation.Controls;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Screen
    {
        GameController gameController;
        //TagVisualizer MyTagVisualizer;

        public GameBoard(ScreenControl parentWindow) : base(parentWindow)
        {
            InitializeComponent();
            //his.MyTagVisualizer = new TagVisualizer();
            //InitializeDefinitions();

            this.gameController = new GameController(false, FENConverter.convertFENToPosition(FENConverter.startPosition), false, false);

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

        /*private void InitializeDefinitions()
        {
            for (byte k = 1; k <= 5; k++)
            {
                TagVisualizationDefinition tagDef =
                    new TagVisualizationDefinition();
                // The tag value that this definition will respond to.
                tagDef.Value = k;
                // The .xaml file for the UI
                tagDef.Source =
                    new Uri("PieceVisualization.xaml", UriKind.Relative);
                // The maximum number for this tag value.
                tagDef.MaxCount = 2;
                // The visualization stays for 2 seconds.
                tagDef.LostTagTimeout = 2000.0;
                // Orientation offset (default).
                tagDef.OrientationOffsetFromTag = 0.0;
                // Physical offset (horizontal inches, vertical inches).
                tagDef.PhysicalCenterOffsetFromTag = new Vector(2.0, 2.0);
                // Tag removal behavior (default).
                tagDef.TagRemovedBehavior = TagRemovedBehavior.Fade;
                // Orient UI to tag? (default).
                tagDef.UsesTagOrientation = true;
                // Add the definition to the collection.
                MyTagVisualizer.Definitions.Add(tagDef);
            }
        }


        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            Console.WriteLine("BLEH");
            PieceVisualization pv = (PieceVisualization)e.TagVisualization;
            switch (pv.VisualizedTag.Value)
            {
                case 1:
                    pv.John.Fill = Brushes.Chartreuse;
                    break;
                case 2:
                    pv.John.Fill = Brushes.Chartreuse;
                    break;
                case 3:
                    pv.John.Fill = Brushes.Chartreuse;
                    break;
                case 4:
                    pv.John.Fill = Brushes.Chartreuse;
                    break;
                default:
                    pv.John.Fill = Brushes.Orange;
                    break;
            }
        }*/
    }
}
