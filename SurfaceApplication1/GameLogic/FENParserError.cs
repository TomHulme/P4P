using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class FENParserError : Exception
    {
        public Position pos;
        public int resourceID = -1;

        public FENParserError(String msg) : base(msg)
        {
            this.pos = null;
        }

        public FENParserError(String msg, Position pos) : base(msg)
        {
            this.pos = pos;
        }

        public FENParserError(int resourceID) : base("")
        {
            this.pos = null;
            this.resourceID = resourceID;
        }

        public FENParserError(int resourceID, Position pos) : base("")
        {
            this.pos = pos;
            this.resourceID = resourceID;
        }
    }
}
