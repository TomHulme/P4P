using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Move
    {
        /*
         * Origin square, 0 - 63
         */
        public int origin;

        /*
         * Destination square, 0 - 63
         */
        public int destination;

        /*
         * Promotion piece
         */
        public PieceType promoteTo;

        /*
         * Creates a Move Object with given inputs
         */
        public Move(int origin, int destination, PieceType promoteTo)
        {
            this.origin = origin;
            this.destination = destination;
            this.promoteTo = promoteTo;
        }

        /*
         * Creates a Move object with given input
         */
        public Move(Move m)
        {
            this.origin = m.origin;
            this.destination = m.destination;
            this.promoteTo = m.promoteTo;
        }

        //-----------------Equals Methods-------------------------

        /*
         * Overrides the Object.Equals method
         */
        public override Boolean Equals(Object obj)
        {
            if ((obj == null) || (obj.GetType() != this.GetType()))
            {
                return false;
            }
            Move other = (Move)obj;
            if (origin != other.origin)
            {
                return false;
            }
            if (destination != other.destination)
            {
                return false;
            }
            if (promoteTo != other.promoteTo)
            {
                return false;
            }
            return true;
        }

        /*
         * Overrides the getHashCode method
         */
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /*
         * Returns a string representation of a move
         */
        public override string ToString()
        {
            return MoveParser.moveObjectToString(this);
        }
    }
}
