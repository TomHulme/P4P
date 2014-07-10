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
using System.Diagnostics;
using System.IO;
using GameLogic;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : SurfaceWindow
    {
        Process myProcess;
        StreamReader myStreamReader;
        StreamWriter myStreamWriter;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
            StartEngine();
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
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
        
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("start click");
            Start_Button.Visibility = System.Windows.Visibility.Collapsed;
            Learn_Button.Visibility = System.Windows.Visibility.Collapsed;
            Tutorial_Button.Visibility = System.Windows.Visibility.Collapsed;
            Settings_Button.Visibility = System.Windows.Visibility.Collapsed;
            GameScreen g = new GameScreen(false, new Position(FENConverter.convertFENToPosition(FENConverter.startPosition)));
            screenHolder.Content = g;

        }

        private void Learn_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("learn click");
        }

        private void Tutorial_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("tutorial click");
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("settings click");
        }

        private void StartEngine()
        {
            Setup();
            String engine = "Engine name not found";
            if (myProcess == null)
            {
                Console.WriteLine("Engine failed to launch!");
                return;
            }
            myStreamWriter.WriteLine("uci");
            String line;
            try
            {
                do
                {
                    line = myStreamReader.ReadLine();
                    Console.WriteLine(line);
                    if (line.StartsWith("id name"))
                    {
                        engine = line.Substring(8);
                    }
                }
                while (line != "uciok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //ContainedText.Text = ContainedText.Text + engine;
            Console.WriteLine(engine);
        }

        private void Setup()
        {
            if (myProcess != null)
            {
                Console.WriteLine("Error! Engine Process Already Started!");
                return;
            }
            myProcess = new Process();

            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                // You can start any process, HelloWorld is a do-nothing example.
                // CHANGE THIS 
                myProcess.StartInfo.FileName = App.getPath() + @"Resources\stockfish-dd-32.exe";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.RedirectStandardInput = true;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.Start();
                myStreamReader = myProcess.StandardOutput;
                myStreamWriter = myProcess.StandardInput;
                Console.WriteLine("Process Started.");
                Console.WriteLine(myStreamReader.ReadLine());
                // This code assumes the process you are starting will terminate itself.  
                // Given that is is started without a window so you cannot terminate it  
                // on the desktop, it must terminate itself or you can do it programmatically 
                // from this application using the Kill method.
                myStreamWriter.WriteLine("position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}