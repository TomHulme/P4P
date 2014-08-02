using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using GameLogic;

namespace Chess
{
    public class Square : Canvas
    {
        string name;
        int number;
        int squareSize = 75;

        PieceType piece;

        public Square(string nam, int num)
        {
            this.name = nam;
            this.number = num;
            this.piece = PieceType.Empty;
            this.Width = squareSize;
            this.Height = squareSize;

            this.Name = nam;
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
    }
}
