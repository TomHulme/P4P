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
    class Tutorial
    {
        protected Position currentPosition;

        public Tutorial()
        {
            currentPosition = FENConverter.convertFENToPosition(FENConverter.emptyPosition);
        }

        public void ClearBoard()
        {
            currentPosition = FENConverter.convertFENToPosition(FENConverter.emptyPosition);
        }
    }
}
