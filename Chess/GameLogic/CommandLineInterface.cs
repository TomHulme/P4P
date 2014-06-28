using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GameLogic
{
    class CommandLineInterface
    {
        private Position currentPosition;
        private Move currentMove;
        private UnMakeInfo unMakeInfo;
        private Boolean gameFinished;
        private Boolean undoMove;
        public TextWriter myConsoleOut;
        public TextReader myConsoleIn;


        public CommandLineInterface() 
        {
            GameInit();
        }

        public CommandLineInterface(TextWriter consoleOut, TextReader consoleIn)
        {
            GameInit();
            myConsoleOut = consoleOut;
            myConsoleIn = consoleIn;
        }

        public void StartGame()
        {
            myConsoleOut.WriteLine("Welcome, press enter to start a new game.");
            myConsoleOut.WriteLine("To quit, simply type quit and hit enter.");
            myConsoleOut.WriteLine("To undo the last move made, type undo and hit enter.");
            myConsoleOut.WriteLine("To make a move, type the move in long algebraic notation.");
            myConsoleOut.WriteLine("An example would be 'e2e4'.");
            myConsoleIn.ReadLine();

            while (!gameFinished)
            {
                myConsoleOut.WriteLine(FENConverter.asciiBoard(currentPosition));
                String userInput = myConsoleIn.ReadLine();

                switch (userInput)
                {
                    case ("quit"):
                    case ("Quit"):
                        gameFinished = true;
                        break;
                    case ("undo"):
                    case ("Undo"):
                        undoMove = true;
                        break;
                    default:
                        break;
                }

                //Continue if user has not quit
                if (!gameFinished)
                {
                    //undo previous moce
                    if (undoMove)
                    {
                        if (unMakeInfo == null)
                        {
                            myConsoleOut.WriteLine("No move to undo.");
                        }
                        else
                        {
                            currentPosition.unMakeMove(currentMove, unMakeInfo);
                        }
                    }
                    //performs valid move
                    else
                    {
                        if (ParseMove(userInput))
                        {
                            String originString = userInput.Substring(0, 2);
                            String destinationString = userInput.Substring(2, 2);

                            int origin = FENConverter.getSquare(originString);
                            int destination = FENConverter.getSquare(destinationString);

                            currentMove = new Move(origin, destination, PieceType.Empty);

                            currentPosition.makeMove(currentMove, unMakeInfo);
                        }
                    }
                }
            }
        }

        private void GameInit()
        {
            currentPosition = FENConverter.convertFENToPosition(FENConverter.startPosition);
            unMakeInfo = new UnMakeInfo();
            gameFinished = false;
            undoMove = false;
        }

        private Boolean ParseMove(String input)
        {
            if (input.Length > 4)
            {
                return false;
            }
            else if (!((char)input[0] >= 'a' && (char)input[0] <= 'h'))
            {
                return false;
            }
            else if (!((char)input[2] >= 'a' && (char)input[2] <= 'h'))
            {
                return false;
            }
            else if (!((char)input[1] >= '1' && (char)input[1] <= '8'))
            {
                return false;
            }
            else if (!((char)input[3] >= '1' && (char)input[3] <= '8'))
            {
                return false;
            }
            else return true;
        }
    }
}
