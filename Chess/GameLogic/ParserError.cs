using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class ParserError : Exception 
    {
        public Position pos;
        public int resourceID = -1;

        public ParserError(String msg) : base(msg)
        {
            this.pos = null;
        }

        public ParserError(String msg, Position pos) : base(msg)
        {
            this.pos = pos;
        }

        public ParserError(int resourceID) : base("")
        {
            this.pos = null;
            this.resourceID = resourceID;
        }

        public ParserError(int resourceID, Position pos) : base("")
        {
            this.pos = pos;
            this.resourceID = resourceID;
        }
    }
}
