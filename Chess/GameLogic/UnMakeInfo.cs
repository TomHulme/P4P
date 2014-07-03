using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    /*
     * Object that holds enough info to undo a move
     */
    public class UnMakeInfo
    {
        PieceType capturedPiece;
        int castleRights;
        int epSquare;
        int halfMoveClock;

        /*
         * Set capturedPiece
         */
        public void setCapturedPiece(PieceType piece)
        {
            this.capturedPiece = piece;
        }

        /*
         * Get capturedPiece
         */
        public PieceType getCapturedPiece()
        {
            return capturedPiece;
        }

        /*
         * Set castleRights
         */
        public void setCastleRights(int castleRights)
        {
            this.castleRights = castleRights;
        }

        /*
         * Get castleRights
         */
        public int getCastleRights()
        {
            return castleRights;
        }

        /*
         * Set en passant square
         */
        public void setEPSquare(int epSquare)
        {
            this.epSquare = epSquare;
        }

        /*
         * Get en passant square
         */
        public int getEPSquare()
        {
            return epSquare;
        }

        /*
         * Set halfMoveClock
         */
        public void setHalfMoveClock(int halfMoveClock)
        {
            this.halfMoveClock = halfMoveClock;
        }

        /*
         * Get halfMoveClock
         */
        public int getHalfMoveClock()
        {
            return halfMoveClock;
        }
    }
}