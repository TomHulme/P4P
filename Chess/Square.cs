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

namespace Chess
{
    public class Square : Canvas
    {
        string name;
        int number;
        int squareSize = 75;

        internal TagVisualizer tagVis;

        PieceType piece;

        public Square(string nam, int num)
        {
            this.name = nam;
            this.number = num;
            this.piece = PieceType.Empty;
            this.Width = squareSize;
            this.Height = squareSize;

            tagVis = new TagVisualizer();
        }

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
            if (isFinger == false && isTag == false)
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
