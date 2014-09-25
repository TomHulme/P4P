using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Windows.Media;
using System.Reflection;

namespace Chess
{
    /// <summary>
    /// Interaction logic for App.xaml
	/// Contains methods which need to be accessed across the entire application.
    /// </summary>
    public partial class App : Application
    {
		// Screen Dimensions. Will not change unless hardware changes.
        public static readonly int Height = 720;
        public static readonly int Width = 1280;

		/**
		 * ReadOptionsFile will read the Options.xml file in the Settings folder.
		 * Sets up colour options
		 */
        public static void ReadOptionsFile()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(App.getPath() + @"Settings\Options.xml");

            XmlNodeList nodes = xmldoc.GetElementsByTagName("Option");
            BrushConverter bc = new BrushConverter();
            foreach (XmlNode node in nodes)
            {
                Brush colour = bc.ConvertFromString(node.Attributes["Value"].Value) as Brush;
                switch (node.Attributes["Name"].Value)
                {
                    case "AttackedPieces":
                        Chess.Properties.Settings.Default.AttackedPieces = colour;
                        break;
                    case "DefendedPieces":
                        Chess.Properties.Settings.Default.DefendedPieces = colour;
                        break;
                    case "HighlightMove":
                        Chess.Properties.Settings.Default.HighlightMove = colour;
                        break;
                    case "PreviousMove":
                        Chess.Properties.Settings.Default.PreviousMove = colour;
                        break;
                    case "TakablePieces":
                        Chess.Properties.Settings.Default.TakablePieces = colour;
                        break;
                    case "SuggestedMove":
                        Chess.Properties.Settings.Default.SuggestedMove = colour;
                        break;
                    default:
                        Console.WriteLine("Error! Setting not recognised!");
                        break;
                }
            }
        }

		/**
		 * Gets the Absolute Path of the Chess Application
		 */
        public static string getPath(){
            string PATH = System.Reflection.Assembly.GetAssembly(typeof(App)).Location;
            int index = PATH.LastIndexOf("bin\\Debug\\Chess.exe");
            string p = PATH.Remove(index);
            return p;
        }

		/**
		 * Ensures all options have been set. If any option is left unset, set it to default values.
		 */
        internal static void EnsureOptionsSet()
        {
            if (Chess.Properties.Settings.Default.AttackedPieces == null)
            {
                Chess.Properties.Settings.Default.AttackedPieces = Brushes.IndianRed;
            }
            if (Chess.Properties.Settings.Default.DefendedPieces == null)
            {
                Chess.Properties.Settings.Default.DefendedPieces = Brushes.DarkOliveGreen;
            }
            if (Chess.Properties.Settings.Default.HighlightMove == null)
            {
                Chess.Properties.Settings.Default.HighlightMove = Brushes.Blue;
            }
            if (Chess.Properties.Settings.Default.PreviousMove == null)
            {
                Chess.Properties.Settings.Default.PreviousMove = Brushes.OrangeRed;
            }
            if (Chess.Properties.Settings.Default.TakablePieces == null)
            {
                Chess.Properties.Settings.Default.TakablePieces = Brushes.Red;
            }
        }

		/**
		 * Creates a Chess Engine process
		 */
        internal static void CreateChessEngine()
        {
            Chess.Properties.Settings.Default.ChessEngine = new global::EngineLogic.SFEngine();
        }
    }
}