using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using GameLogic;

namespace EngineLogic
{
    class ComputerPlayer
    {
        private StreamReader engineReader;
        private StreamWriter engineWriter;

        private int moveTime;

        public ComputerPlayer(StreamReader engineReader, StreamWriter engineWriter)
        {
            this.engineReader = engineReader;
            this.engineWriter = engineWriter;
            this.moveTime = 5;
        }

        /**
         * Tell the engine to start searching for the best possible move
         * based on the internal position of the engine
         */
        public void StartSearch()
        {
            //uci command to being searching for a move
            //the movetime variable describes how long the engine searches for
            String search = "go movetime " + moveTime;

            engineWriter.WriteLine(search);
        }

        /**
         * Update the internal position of the current engine so it can
         * perform its search correctly
         */
        public void UpdatePosition(ArrayList moves)
        {
            StringBuilder position = new StringBuilder();

            position.Append("position startpos moves ");

            foreach (Move move in moves)
            {
                position.Append(MoveParser.moveObjectToString(move));
                position.Append(" ");
            }

            engineWriter.WriteLine(position.ToString());
        }

        /**
         * Poll the current engine for the best move from the search
         */
        public String GetBestMove()
        {
            String searchResults;
            String bestMove;
            String ponder;
            Boolean searching = true;

            do
            {
                searchResults = engineReader.ReadLine();
                Console.WriteLine(searchResults);
                if (searchResults.StartsWith("bestmove"))
                {
                    searching = false;
                }
            } while (searching);

            bestMove = searchResults.Split(' ')[1];
            //We may find a use for this ponder move
            ponder = searchResults.Split(' ')[3];

            return bestMove;
        }

        /**
         * Getter for moveTime variable
         * Default value is 5
         */
        public int getMoveTime()
        {
            return moveTime;
        }

        /**
         * Setter for moveTime variable
         * Default value is 5
         */
        public void setMoveTime(int moveTime)
        {
            this.moveTime = moveTime;
        }
    }
}
