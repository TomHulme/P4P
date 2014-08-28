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
using GameLogic;

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
        Boolean isWhite;

        public GameSettingsDialog(GameController gameController, Boolean isWhite)
        {
            InitializeComponent();

            this.gameController = gameController;
            this.isWhite = isWhite;

            this.gameController.RaiseControllerEvent += new EventHandler<ControllerEvent>(gameController_RaiseControllerEvent);

            PopulatePawnList();
            PopulateRookList();
            PopulateBishopList();
            PopulateKnightList();

            foreach (Border b in PawnList)
            {
                b.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
                b.Visibility = Visibility.Hidden;
            }

            foreach (Border b in RookList)
            {
                b.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
                b.Visibility = Visibility.Hidden;
            }

            foreach (Border b in BishopList)
            {
                b.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
                b.Visibility = Visibility.Hidden;
            }

            foreach (Border b in KnightList)
            {
                b.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
                b.Visibility = Visibility.Hidden;
            }

            Queen.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
            Queen.Visibility = Visibility.Hidden;

            if (!isWhite)
            {
                HighlightMoves.Background = Brushes.WhiteSmoke;
                Attacked.Background = Brushes.WhiteSmoke;
                Defended.Background = Brushes.WhiteSmoke;
                SuggestMove.Background = Brushes.WhiteSmoke;

                Title.Background = Brushes.WhiteSmoke;
            }
        }

        void gameController_RaiseControllerEvent(object sender, ControllerEvent e)
        {
            if (!isWhite)
            {
                switch (e.p)
                {
                    case (PieceType.P):
                        foreach (Border b in PawnList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.R):
                        foreach (Border b in RookList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.B):
                        foreach (Border b in BishopList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.N):
                        foreach (Border b in KnightList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.Q):
                        Queen.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.p)
                {
                    case (PieceType.p):
                        foreach (Border b in PawnList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.r):
                        foreach (Border b in RookList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.b):
                        foreach (Border b in BishopList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.n):
                        foreach (Border b in KnightList)
                        {
                            if (b.Visibility == Visibility.Hidden)
                            {
                                b.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        break;
                    case (PieceType.q):
                        Queen.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            
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

            if (gameController.ShowHighlightedMoves)
            {
                HighlightMoves.Background = Chess.Properties.Settings.Default.HighlightMove;
                gameController.DoColourations();
            }
            else
            {
                HighlightMoves.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
            }
        }

        private void Attacked_Click(object sender, RoutedEventArgs e)
        {
            gameController.ShowAttackedPieces = gameController.ShowAttackedPieces ? false : true;

            if (gameController.ShowAttackedPieces)
            {
                Attacked.Background = Chess.Properties.Settings.Default.AttackedPieces;
                gameController.DoColourations();
            }
            else
            {
                Attacked.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
            }
        }

        private void Defended_Click(object sender, RoutedEventArgs e)
        {
            gameController.ShowOnlyDefendedPiecesUnderAttack = gameController.ShowOnlyDefendedPiecesUnderAttack ? false : true;

            if (gameController.ShowOnlyDefendedPiecesUnderAttack)
            {
                Defended.Background = Chess.Properties.Settings.Default.DefendedPieces;
                gameController.DoColourations();
            }
            else
            {
                Defended.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
            }
        }

        private void SuggestMove_Click(object sender, RoutedEventArgs e)
        {
            //this is where you run the engine to search for a best move
            //and then colour the board
            gameController.SuggestingMove = gameController.SuggestingMove ? false : true;

            if (gameController.SuggestingMove)
            {
                SuggestMove.Background = Chess.Properties.Settings.Default.SuggestedMove;
                gameController.DoColourations();
            }
            else
            {
                SuggestMove.Background = isWhite ? Brushes.DarkGray : Brushes.WhiteSmoke;
                gameController.board.UnColourBorders();
            }
        }
    }
}
