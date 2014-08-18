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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.Screens.Dialogs
{
    /// <summary>
    /// Interaction logic for GameSettings.xaml
    /// </summary>
    public partial class GameSettingsDialog : UserControl
    {
        ArrayList PawnList;
        ArrayList RookList;
        ArrayList BishopList;
        ArrayList KnightList;
        GameController gameController;

        public GameSettingsDialog(GameController gameController)
        {
            InitializeComponent();

            this.gameController = gameController;

            PopulatePawnList();
            PopulateRookList();
            PopulateBishopList();
            PopulateKnightList();

            foreach (TextBlock t in PawnList)
            {
                t.Background = Brushes.DarkGray;
                t.Text = "P";
            }

            foreach (TextBlock t in RookList)
            {
                t.Background = Brushes.DarkGray;
                t.Text = "R";
            }

            foreach (TextBlock t in BishopList)
            {
                t.Background = Brushes.DarkGray;
                t.Text = "B";
            }

            foreach (TextBlock t in KnightList)
            {
                t.Background = Brushes.DarkGray;
                t.Text = "N";
            }

            Queen.Background = Brushes.DarkGray;
            Queen.Text = "Q";
        }

        internal void PopulatePawnList() 
        {
            PawnList = new ArrayList();

            PawnList.Add(Pawn1);
            PawnList.Add(Pawn2);
            PawnList.Add(Pawn3);
            PawnList.Add(Pawn4);
            PawnList.Add(Pawn5);
            PawnList.Add(Pawn6);
            PawnList.Add(Pawn7);
            PawnList.Add(Pawn8);
        }

        internal void PopulateRookList()
        {
            RookList = new ArrayList();

            RookList.Add(Rook1);
            RookList.Add(Rook2);
        }

        internal void PopulateBishopList()
        {
            BishopList = new ArrayList();

            BishopList.Add(Bishop1);
            BishopList.Add(Bishop2);
        }

        internal void PopulateKnightList()
        {
            KnightList = new ArrayList();

            KnightList.Add(Knight1);
            KnightList.Add(Knight2);
        }

        private void HighlightMoves_Click(object sender, RoutedEventArgs e)
        {
            gameController.ShowHighlightedMoves = gameController.ShowHighlightedMoves ? false : true;
        }

        private void Attacked_Click(object sender, RoutedEventArgs e)
        {
            gameController.ShowAttackedPieces = gameController.ShowAttackedPieces ? false : true;
        }

        private void Defended_Click(object sender, RoutedEventArgs e)
        {
            gameController.ShowDefendedPieces = gameController.ShowDefendedPieces ? false : true;
        }
    }
}
