using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLogic;

namespace Tutorials
{
    /**
     * Superclass of tutorial objects. Holds
     * the position and methods pertaining to it.
     */
    public class TutorialBase
    {
        protected Position currentPosition;

        public TutorialBase()
        {
            currentPosition = FENConverter.convertPiecePlacementToPosition(FENConverter.emptyPosition);
        }

        public void ClearBoard()
        {
            currentPosition = FENConverter.convertPiecePlacementToPosition(FENConverter.emptyPosition);
        }

        public Position GetPosition()
        {
            return currentPosition;
        }

        public virtual void ResetPosition()
        {
            ClearBoard();
        }
    }
}
