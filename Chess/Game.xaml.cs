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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Controls.Primitives;

namespace Chess
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Game()
        {
            InitializeComponent();
            Canvas board = drawBoard(true);
            game.Content = board;
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
        }

        private Canvas drawBoard(bool flipped)
        {
            Canvas board = new Canvas();
            board.Width = 600;
            board.Height = 600;
            Canvas[] squares = new Canvas[64];
            for (int i = 0; i < 8; i++)
            {
                squares[i*8] = new Canvas();
                squares[i * 8].Uid = getSquareName(i, 0, flipped);
                squares[i * 8].Width = 75;
                squares[i * 8].Height = 75;
                squares[i * 8].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                board.Children.Add(squares[i * 8]);
                Canvas.SetTop(squares[i * 8], i*75);
                Canvas.SetLeft(squares[i * 8], 0);
                for (int j = 1; j < 8; j++)
                {
                    squares[i * 8 + j] = new Canvas();
                    squares[i * 8 + j].Uid = getSquareName(i, j, flipped);
                    squares[i * 8 + j].Width = 75;
                    squares[i * 8 + j].Height = 75;
                    squares[i * 8 + j].AddHandler(ButtonBase.MouseLeftButtonDownEvent, new RoutedEventHandler(TappedSquare), true);
                    board.Children.Add(squares[i * 8 + j]);
                    Canvas.SetTop(squares[i * 8 + j], i*75);
                    Canvas.SetLeft(squares[i * 8 + j], j*75);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (flipped)
                    {
                        if ((i+j) % 2 == 0) { squares[i * 8 + j].Background = Brushes.Black; }
                        else { squares[i * 8 + j].Background = Brushes.White; }
                    }
                    else
                    {
                        if ((i+j) % 2 == 0) { squares[i * 8 + j].Background = Brushes.White; }
                        else { squares[i * 8 + j].Background = Brushes.Black; }
                    }
                }
            }
            return board;
        }

        private string getSquareName(int i, int j, bool flipped)
        {
            int row, col;
            if (flipped)
            {
                row = i;
                col = 7-j;
            }
            else
            {
                row = j;
                col = i;
            }

            string name = "";

            switch (row)
            {
                case 0:
                    name = string.Concat(name,"a");
                    break;
                case 1:
                    name = string.Concat(name,"b");
                    break;
                case 2:
                    name = string.Concat(name,"c");
                    break;
                case 3:
                    name = string.Concat(name,"d");
                    break;
                case 4:
                    name = string.Concat(name,"e");
                    break;
                case 5:
                    name = string.Concat(name,"f");
                    break;
                case 6:
                    name = string.Concat(name,"g");
                    break;
                case 7:
                    name = string.Concat(name,"h");
                    break;
                default:
                    break;
            }
            switch (col)
            {
                case 0:
                    name = string.Concat(name,"8");
                    break;
                case 1:
                    name = string.Concat(name,"7");
                    break;
                case 2:
                    name = string.Concat(name,"6");
                    break;
                case 3:
                    name = string.Concat(name,"5");
                    break;
                case 4:
                    name = string.Concat(name,"4");
                    break;
                case 5:
                    name = string.Concat(name,"3");
                    break;
                case 6:
                    name = string.Concat(name,"2");
                    break;
                case 7:
                    name = string.Concat(name,"1");
                    break;
                default:
                    break;
            }
            return name;
        }

        private void TappedSquare(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(((Canvas)sender).Uid);
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

    }
}