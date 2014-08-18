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
    /// </summary>
    public partial class Square : Canvas
    {

        string name;
        int number;
        int squareSize = 75;
        int pieceChildrenNumber = -1;
        internal TagVisualizer tagVis;
        Image image;

        PieceType piece;

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
            InitializeDefinitions();
            MyTagVisualizer.Height = squareSize;
            MyTagVisualizer.Width = squareSize;
            this.name = nam;
            this.number = num;
            this.piece = PieceType.Empty;
            this.Width = squareSize;
            this.Height = squareSize;
            this.SetCurrentValue(Panel.ZIndexProperty, 3);

            rectangle.Width = squareSize;
            rectangle.Height = squareSize;
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeThickness = 1;
            colourRectangle(Brushes.Transparent);
        }

        public void colourRectangle(Brush colour)
        {
            rectangle.Fill = colour;
        }

        public void colourBorder(Brush colour)
        {
            rectangle.Stroke = colour;
        }

        void tagVis_VisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            Console.WriteLine("Tag left square " + name);
        }

        void tagVis_VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            PieceVisualization pv = (PieceVisualization)e.TagVisualization;
            pv.John.Fill = Brushes.Chartreuse;
            Console.WriteLine(pv.VisualizedTag.Value);
            Console.WriteLine("Tag in square " + name);
        }

        public void setPiece(PieceType p)
        {
            this.piece = p;
        }

        public PieceType getPiece()
        {
            return this.piece;
        }

        public string getName()
        {
            return this.name;
        }

        internal int getSquareNumber()
        {
            return this.number;
        }

        public String Name { get { return this.name; } }

        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            bool isFinger = e.TouchDevice.GetIsFingerRecognized();
            bool isTag = e.TouchDevice.GetIsTagRecognized();
            if (isFinger == false && isTag == false)//
            {
                e.Handled = true;
                return;
            }
            base.OnPreviewTouchDown(e);
        }

        internal void AddTagVisualisation(TagVisualizationDefinition tag)
        {
            tagVis.Definitions.Add(tag);
        }

        internal void clearPieceImage()
        {
            if (pieceChildrenNumber != -1 && image != null)
            {
                this.Children.Remove(image);
                this.pieceChildrenNumber = -1;
            }
        }

        internal Image getPieceImage()
        {
            if (pieceChildrenNumber == -1) { return new Image(); }
            IEnumerator x = this.Children.GetEnumerator();
            for (int i = 0; i < pieceChildrenNumber; i++ ) { x.MoveNext(); }
            x.MoveNext();
            return (Image)x.Current;
        }

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


        /*internal void ClearPiece(){
            foreach (object x in this.Children)
            {
                if (x.GetType() == typeof(TagVisualizer))
                {
                    continue;
                }
                else
                {
                    this.Children.RemoveAt(this.Children.IndexOf((UIElement)x));
                }
            }
        }*/



        private void Square_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize Square's UI based on this.VisualizedTag here
        }

        private void InitializeDefinitions()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(App.getPath() + @"Settings\Tags.xml");
            
            XmlNodeList nodes = xmldoc.GetElementsByTagName("Tag");
            foreach(XmlNode node in nodes){
                TagVisualizationDefinition tagDef = new TagVisualizationDefinition();
                // The tag value and series that this definition will respond to.
                Console.WriteLine(node.Attributes["Value"].Value);
                tagDef.Value = TagValue.FromString(node.Attributes["Value"].Value);
                tagDef.Series = TagValue.FromString(node.Attributes["Series"].Value);

                // The .xaml file for the UI
                tagDef.Source = new Uri("PieceVisualization.xaml", UriKind.Relative);
                // The maximum number for this tag value.
                tagDef.MaxCount = 32;
                // The visualization stays for 2 seconds.
                tagDef.LostTagTimeout = 100.0;
                // Orientation offset (default).
                tagDef.OrientationOffsetFromTag = 0.0;
                // Physical offset (horizontal inches, vertical inches).
                tagDef.PhysicalCenterOffsetFromTag = new Vector(0.5, 0.5);
                // Tag removal behavior (default).
                //tagDef.TagRemovedBehavior = TagRemovedBehavior.Fade;
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
                default:
                    pv.John.Fill = Brushes.Orange;
                    break;
            }
        }
    }
}
