using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GameLogic;
using System.Windows.Controls;
using System.Windows.Media;
using EngineLogic;
using System.IO;

namespace Chess
{
    class GameController
    {
        private bool blackIsAI = true;
        private bool whiteIsAI = true;
        private Queue<Square> moveQueue = new Queue<Square>();
        public event EventHandler<ControllerEvent> RaiseControllerEvent;
        private ArrayList previousMoves = new ArrayList();
        internal Board board;
        private bool oneClick = false;
        private UnMakeInfo unmake = new UnMakeInfo();
        internal Position position;
        private MoveGenerator movegen;
        public event EventHandler<BoardEvent> RaiseBoardEvent;
        private SFEngine engine;
        private ComputerPlayer blackAI;
        private ComputerPlayer whiteAI;
        private StreamReader blackIn;
        private StreamWriter blackOut;
        private StreamReader whiteIn;
        private StreamWriter whiteOut;

        public GameController(bool b, Position pos)
        {
            this.board = new Board(b, pos, this);
            board.setup();
            this.position = pos;
            this.movegen = new MoveGenerator();
            this.engine = new SFEngine();
            if (blackIsAI)
            {
                blackIn = engine.engineProcess.StandardOutput;
                blackOut = engine.engineProcess.StandardInput;
                blackAI = new ComputerPlayer(blackIn, blackOut);
                blackAI.setMoveTime(100);
            }
            if (whiteIsAI)
            {
                whiteIn = engine.engineProcess.StandardOutput;
                whiteOut = engine.engineProcess.StandardInput;
                whiteAI = new ComputerPlayer(whiteIn, whiteOut);
                whiteAI.setMoveTime(100);
                //this.PerformAIMove(whiteAI); // if uncommented, entire game plays out before screen is displayed.
            }
        }

        private PieceType getPromotion(PieceType piece)
        {

            if (piece.Equals(PieceType.p))
            {
                return PieceType.q;
            }
            else
            {
                return PieceType.Q;
            }
            //return PieceType.Empty;
        }




        public void movePiece(Move current)
        {
            Square originSquare = board.getSquareForNumber(current.origin);
            Square destinationSquare = board.getSquareForNumber(current.destination);
            PieceType originPiece = originSquare.getPiece();
            destinationSquare.setPiece(originSquare.getPiece());
            originSquare.setPiece(PieceType.Empty);
            if (position.getEpSquare() == current.destination && (originPiece == PieceType.p || originPiece == PieceType.P))
            {
                // EN PASSANT!
                Move last = (Move)this.previousMoves[this.previousMoves.Count];
                Square enPassantPawn = board.getSquareForNumber(last.destination);
                enPassantPawn.setPiece(PieceType.Empty);
                enPassantPawn.Children.Clear();
            }

            if (MoveParser.moveObjectToString(current, this.position).Contains("O-O"))
            {
                // CASTLING!
                board.ColourBoard();
                Console.WriteLine("Castling");

                Image[] img = new Image[1];
                Square rookOrigin;
                Square rookDestination;
                if (current.destination == current.origin + 2)
                {
                    rookOrigin = board.getSquareForNumber(current.origin + 3);
                    rookDestination = board.getSquareForNumber(current.origin + 1);
                }
                else if (current.destination == current.origin - 2)
                {
                    rookOrigin = board.getSquareForNumber(current.origin - 4);
                    rookDestination = board.getSquareForNumber(current.origin - 1);
                }
                else
                {
                    throw new Exception("No.");
                }
                originSquare.Children.CopyTo(img, 0);
                originSquare.Children.Clear();
                destinationSquare.Children.Add(img[0]);
                rookOrigin.Children.CopyTo(img, 0);
                rookOrigin.Children.Clear();
                rookDestination.Children.Add(img[0]);
                rookDestination.setPiece(rookOrigin.getPiece());
                rookOrigin.setPiece(PieceType.Empty);
            }
            else
            {
                Image[] img = new Image[1];
                if (destinationSquare.Children.Count > 0)
                {
                    destinationSquare.Children.Clear();
                }
                originSquare.Children.CopyTo(img, 0);
                originSquare.Children.Clear();
                destinationSquare.Children.Add(img[0]);
            }
        }

        private void promotePiece(Square sq, PieceType p)
        {
            sq.Children.Clear();
            board.drawPiece(p, sq);
        }

        public void MoveHandler(string MoveString)
        {
            if (ParseMove(MoveString))
            {
                Square orig = board.getSquareForName(MoveString.Substring(0,2));
                Square dest = board.getSquareForName(MoveString.Substring(2,2));
                this.MoveHandler(orig);
                this.MoveHandler(dest);
            }
        }

        private static Boolean ParseMove(String input)
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

        public void MoveHandler(Square tapped){
            Console.WriteLine("Square " + tapped.Name + " tapped");
            moveQueue.Enqueue(tapped);
            if (this.oneClick)
            {
                Square orig = moveQueue.Dequeue();
                Square dest = moveQueue.Dequeue();
                
                // Debug prints origin and destination piece types contained in squares
                //Console.WriteLine("Origin: " + orig.getSquareNumber());
                //Console.WriteLine("Destination: " + dest.getSquareNumber());
                
                PieceType promoteTo = ((dest.getSquareNumber() <= 7 | dest.getSquareNumber() > 55) & (orig.getPiece().Equals(PieceType.p) | orig.getPiece().Equals(PieceType.P))) ? getPromotion(orig.getPiece()) : PieceType.Empty;
                
                // Debug prints promotion piece
                //Console.WriteLine("Promote To: " + promoteTo);
                Move current = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promoteTo);
                if (MoveCheck(current))
                {
                    performMove(current);
                }
                else
                {
                    moveQueue.Enqueue(dest);
                    this.ColourLegalMoves(dest.getSquareNumber());
                }
                
            }
            else
            {
                this.ColourLegalMoves(tapped.getSquareNumber());
                this.oneClick = true;
            }
        }


        private void ColourLegalMoves(int originSquare)
        {
            board.ColourBoard();
            foreach (Move x in new MoveGenerator().legalMoves(this.position))
            {
                if (x.origin == originSquare)
                {
                    if (board.getSquareForNumber(x.destination).getPiece() != PieceType.Empty)
                    {
                        board.getSquareForNumber(x.destination).Background = Brushes.Red;
                    }
                    else if (x.destination == this.position.getEpSquare() && (board.getSquareForNumber(x.origin).getPiece() == PieceType.P || board.getSquareForNumber(x.origin).getPiece() == PieceType.p))
                    {
                        board.getSquareForNumber(x.destination).Background = Brushes.Red;
                        board.getSquareForNumber(((Move)this.previousMoves[this.previousMoves.Count]).destination).Background = Brushes.Red;
                    }
                    else
                    {
                        board.getSquareForNumber(x.destination).Background = Brushes.Blue;
                    }
                }
            }
        }

        protected void performMove(Move current)
        {
            if (current.promoteTo != PieceType.Empty)
            {
                this.promotePiece(board.getSquareForNumber(current.origin), current.promoteTo);
            }
            OnRaiseControllerEvent(new ControllerEvent("Did a" + (((blackIsAI & !position.whiteMove) | (whiteIsAI & position.whiteMove)) ? "n AI " : " non AI ") + " move from " + board.getSquareName(current.origin) + " to " + board.getSquareName(current.destination)));
            OnRaiseBoardEvent(new BoardEvent(current, board.getSquareForNumber(current.origin).getName() + board.getSquareForNumber(current.destination).getName(), (movegen.legalMoves(this.position).Count == 0)));
            this.movePiece(current);
            this.position.makeMove(current, this.unmake);
            this.previousMoves.Add(current);
            //OnRaiseBoardEvent(new BoardEvent(current, this.getSquareForNumber(current.origin).getName() + this.getSquareForNumber(current.destination).getName(), (movegen.legalMoves(this.position).Count == 0)));

            this.oneClick = false;
            if (blackIsAI & !position.whiteMove)
            {
                this.PerformAIMove(blackAI);
                
            }else if (whiteIsAI & position.whiteMove)
            {
                this.PerformAIMove(whiteAI);
            }
            board.ColourBoard();
            board.printNextTurn();
        }

        private bool MoveCheck(Move m)
        {
            // Prints move tested
            //Console.WriteLine("Testing move from " + m.origin + " to " + m.destination + " with promoteTo " + m.promoteTo);
            return (movegen.legalMoves(this.position).Contains(m));
        }

        private void PerformAIMove(ComputerPlayer AIPlayer)
        {
            AIPlayer.UpdatePosition(previousMoves);
            AIPlayer.StartSearch();
            this.MoveHandler(AIPlayer.GetBestMove());
        }


        /* Event handling best practice from http://msdn.microsoft.com/en-us/library/w369ty8x.aspx
         * 
         */
        protected virtual void OnRaiseControllerEvent(ControllerEvent e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler<ControllerEvent> handler = RaiseControllerEvent;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }




        /* Event handling best practice from http://msdn.microsoft.com/en-us/library/w369ty8x.aspx
         * 
         */
        protected virtual void OnRaiseBoardEvent(BoardEvent e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler<BoardEvent> handler = RaiseBoardEvent;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
    }

    class ControllerEvent : EventArgs
    {
        private string text;
        public ControllerEvent(String text)
        {
            this.text = text;
        }

        public String Text
        {
            get { return text; }
        }
    }
}
