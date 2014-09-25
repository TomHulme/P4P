using System;
using System.Collections;
using System.Collections.Generic;
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
using GameLogic;
using System.Xml;

namespace Chess
{
    /// <summary>
    /// Interaction logic for Square.xaml
	/// Defines internal workings of Square class
    /// </summary>
    public partial class Square : Canvas
    {
		/**
		 * Class Variables
		 */
        string name;
        int number;
        int squareSize = 75;
        int pieceChildrenNumber = -1;
        internal TagVisualizer tagVis;
        Image image;
        PieceType piece;

		/**
		 * Constructors
		 */
        public Square()
        {
            InitializeComponent();
            InitializeDefinitions();
            MyTagVisualizer.Height = squareSize;
            MyTagVisualizer.Width = squareSize;
        }



        public Square(string nam, int num)
        {
            InitializeComponent();
			// Init Obj Rec
            InitializeDefinitions();
			// Set Square Size
            MyTagVisualizer.Height = squareSize;
            MyTagVisualizer.Width = squareSize;
			// Bring TagVisualizer to the top
            MyTagVisualizer.SetCurrentValue(Panel.ZIndexProperty, 5);
			// Set internal information
            this.name = nam;
            this.number = num;
            this.piece = PieceType.Empty;
            this.Width = squareSize;
            this.Height = squareSize;
			// Set the Square in the middle
            this.SetCurrentValue(Panel.ZIndexProperty, 3);
			// Add the Rectangle Layer
            rectangle.Width = squareSize;
            rectangle.Height = squareSize;
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeThickness = 1;
            rectangle.Opacity = 0.5;
            colourRectangle(Brushes.Transparent);
        }

		/**
		 * Colour the Rectangle the inputted colour
		 */
        public void colourRectangle(Brush colour)
        {
            rectangle.Fill = colour;
        }

		/**
		 * Colour the Border the inputted colour
		 */
        public void colourBorder(Brush colour)
        {
            rectangle.Stroke = colour;
        }

		/****
		 * Tag Visualization add/remove events.
		 * Happen when tags enter/leave this square.
		 */
		 // Removed
        void tagVis_VisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            Console.WriteLine("Tag left square " + name);
        }
		// Added
        void tagVis_VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            // Adding the PieceVisulaization both helps identify squares with the objects AND makes it easier for the screen to see.
            PieceVisualization pv = (PieceVisualization)e.TagVisualization;
            pv.TagBase.Fill = Brushes.DarkTurquoise;
            //Console.WriteLine(pv.VisualizedTag.Value);
            Console.WriteLine("Tag in square " + name);
        }

		/**
		 * Set the piece inside the Square
		 */
        public void setPiece(PieceType p)
        {
            this.piece = p;
        }

		/**
		 * Get the piece inside the Square
		 */
        public PieceType getPiece()
        {
            return this.piece;
        }

		/**
		 * Getters for the Squares Name and Number
		 */
        public string getName()
        {
            return this.name;
        }

        internal int getSquareNumber()
        {
            return this.number;
        }

        public String Name { get { return this.name; } }

		/**
		 * VITAL!!! Overridder OnPreviewTouchDown method
		 * Ignores all non-finger/tag input (i.e. blobs etc.)
		 * This helps tremendously with performance.
		 */
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            bool isFinger = e.TouchDevice.GetIsFingerRecognized();
            bool isTag = e.TouchDevice.GetIsTagRecognized();
            if (isFinger == false && isTag == false)//
            {
				// Tell the throwing class that the event was handled.
                e.Handled = true;
                return;
            }
            base.OnPreviewTouchDown(e);
        }

		/**
		 * Add a tag to the TagVisualizer's definitions
		 */
        internal void AddTagVisualisation(TagVisualizationDefinition tag)
        {
            tagVis.Definitions.Add(tag);
        }

		/**
		 * Clear the piece image from a Square
		 */
        internal void clearPieceImage()
        {
            if (pieceChildrenNumber != -1 && image != null)
            {
                this.Children.Remove(image);
                this.pieceChildrenNumber = -1;
            }
        }

		/**
		 * Get the piece image from a Square
		 */
        internal Image getPieceImage()
        {
            if (pieceChildrenNumber == -1) { return new Image(); }
            IEnumerator x = this.Children.GetEnumerator();
            for (int i = 0; i < pieceChildrenNumber; i++ ) { x.MoveNext(); }
            x.MoveNext();
            return (Image)x.Current;
        }

		/**
		 * Set the piece image in a Square
		 */
        internal void setPieceImage(Image img)
        {
            if (pieceChildrenNumber != -1)
            {
                this.clearPieceImage();
            }
            this.Children.Add(img);
            this.image = img;
            pieceChildrenNumber = this.Children.IndexOf(img);
        }

		/**
		 * Get the size of a Square
		 */
        internal int getSquareSize()
        {
            return squareSize;
        }

        //Used to get the sqaure name from another thread
        public static String CopySquare(Square square)
        {
            String result;
            GetNameDelegate a;

            a = new GetNameDelegate(GetName);
            result = square.Dispatcher.Invoke(a, square) as String;
            return result;
        }

        delegate String GetNameDelegate(Square square);

        public static String GetName(Square square)
        {
            return square.getName();
        }

		// DEPRECATED
        private void Square_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize Square's UI based on this.VisualizedTag here
        }

		/**
		 * Setup the Tag definitions by readinf them from the Tags.xml file in the settings folder.
		 * Allows customisation of Tags used by the owner.
		 */
        private void InitializeDefinitions()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(App.getPath() + @"Settings\Tags.xml");
            
            XmlNodeList nodes = xmldoc.GetElementsByTagName("Tag");
            foreach(XmlNode node in nodes){
                TagVisualizationDefinition tagDef = new TagVisualizationDefinition();
                // The tag value and series that this definition will respond to.
                tagDef.Value = TagValue.FromString(node.Attributes["Value"].Value);
                tagDef.Series = TagValue.FromString(node.Attributes["Series"].Value);

                // The .xaml file for the UI
                tagDef.Source = new Uri("PieceVisualization.xaml", UriKind.Relative);
                // The maximum number for this tag value.
                tagDef.MaxCount = 32;
                // The visualization stays for 2 seconds.
                tagDef.LostTagTimeout = 1000.0;
                //tagDef.
                // Orientation offset (default).
                tagDef.OrientationOffsetFromTag = 0.0;
                // Physical offset (horizontal inches, vertical inches).
                // Tag removal behavior (default).
                //tagDef.TagRemovedBehavior = TagRemovedBehavior.Fade;
                // Orient UI to tag? (default).
                tagDef.UsesTagOrientation = true;
                // Add the definition to the collection.
                MyTagVisualizer.Definitions.Add(tagDef);
            }
        }
    }

    class SquareVisualization : TagVisualizationDefinition
    {
        //public RoutedEventHandler
    }
}
