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
    public class PawnGame : TutorialBase
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

        public override void ResetPosition()
        {
            SetUpPosition();
        }

        public static Position PawnGamePosition()
        {
            Position position = new Position();
            String pawnGamePosition = "4k3/pppppppp/8/8/8/8/PPPPPPPP/4K3 w KQkq - 0 1";

            position = FENConverter.convertFENToPosition(pawnGamePosition);

            return position;
        }
    }
}
