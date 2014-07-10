using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLogic;


namespace Tutorials.Challenges
{

    /**
     * A game of chess with only the pawn line and
     * the king. The first player to promote a pawn
     * wins the game.
     */
    class PawnGame : TutorialBase
    {
        public PawnGame()
        {
            SetUpPosition();
        }

        private void SetUpPosition()
        {
            String pawnGamePosition = "4k3/pppppppp/8/8/8/8/PPPPPPPP/4K3 w KQkq - 0 1";

            currentPosition = FENConverter.convertFENToPosition(pawnGamePosition);
        }
    }
}
