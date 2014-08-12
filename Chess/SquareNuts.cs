using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using GameLogic;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Input;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;

namespace Chess
{
    public class SquareNuts : Canvas
    {
        string name;
        int number;
        int squareSize = 75;

        internal TagVisualizer tagVis;

        PieceType piece;

        public SquareNuts(string nam, int num)
        {
            this.name = nam;
            this.number = num;
            this.piece = PieceType.Empty;
            this.Width = squareSize;
            this.Height = squareSize;
            this.SetCurrentValue(Panel.ZIndexProperty, 3);
            tagVis = new TagVisualizer();
            //tagVis.VisualizationAdded += this.tagVis_VisualizationAdded;
            //tagVis.VisualizationRemoved += this.tagVis_VisualizationRemoved;
            tagVis.Height = squareSize;
            tagVis.Width = squareSize;
            tagVis.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            tagVis.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            tagVis.SetCurrentValue(Panel.ZIndexProperty, 4);
            tagVis.Background = Brushes.Chocolate;

        }

        /*void tagVis_VisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            Console.WriteLine("Tag left square " + name);
        }

        void  tagVis_VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            PieceVisualization pv = (PieceVisualization)e.TagVisualization;
            pv.John.Fill = Brushes.Chartreuse;
            Console.WriteLine(pv.VisualizedTag.Value);
            Console.WriteLine("Tag in square " + name);
        }*/

        public void setPiece(PieceType p)
        {
            this.piece = p;
        }

        public PieceType getPiece()
        {
            return this.piece;
        }

        public string getName(){
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
            if (isTag == false)//isFinger == false && 
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
    }
}
