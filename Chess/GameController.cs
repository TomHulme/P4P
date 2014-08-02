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
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;

namespace Chess
{
    public class GameController
    {
        private bool blackIsAI = false;
        private bool whiteIsAI = false;
        private Queue<Square> moveQueue = new Queue<Square>();
        private ArrayList previousMoves = new ArrayList();
        internal Board board;
        private bool oneClick = false;
        private UnMakeInfo unmake = new UnMakeInfo();
        internal Position position;
        private MoveGenerator movegen;
        public event EventHandler<BoardEvent> RaiseBoardEvent;
        event EventHandler<ControllerEvent> RaiseControllerEvent;
        private SFEngine engine;
        private ComputerPlayer AI;
        internal BackgroundWorker bw;
        private bool playerHasMoved = false;
        private bool blackReversed = true;

        public GameController(bool b, Position pos)
        {
            this.board = new Board(b, pos, this);
            board.setup();
            this.position = pos;
            this.movegen = new MoveGenerator();
            this.Subscribe(this);
            
        }

        public GameController(bool b, Position pos, bool blackIsAI, bool whiteIsAI)
        {

            this.board = new Board(b, pos, this, !blackIsAI);
            board.setup();
            this.position = pos;
            this.movegen = new MoveGenerator();
            this.Subscribe(this);


            this.blackIsAI = blackIsAI;
            this.whiteIsAI = whiteIsAI;
            if (blackIsAI | whiteIsAI)
            {
                this.engine = new SFEngine();
                AI = new ComputerPlayer(engine.engineProcess.StandardOutput, engine.engineProcess.StandardInput);
                AI.setMoveTime(2000);
            }
            if (blackIsAI & whiteIsAI)
            {
                bw = new BackgroundWorker();

                bwSetup();
            }
            
        }

        private void bwSetup()
        {
            bw.WorkerReportsProgress = true;

            bw.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    int i = 0;
                    bool gameCompleted = false;
                    while (!gameCompleted) 
                    {
                        String move;
                        if ((position.whiteMove & whiteIsAI) | (!position.whiteMove & blackIsAI))
                        {
                            move = GetAIMove();
                        }else{
                            throw (new Exception("ERROR! This loop should only run when there is no Human player."));
                        }

                        if (ParseMove(move))
                        {
                            Console.WriteLine("_" + move + "_");
                            Square orig = board.getSquareForName(move.Substring(0, 2));
                            Square dest = board.getSquareForName(move.Substring(2, 2));
                            Console.WriteLine(move + " " + move.Length);
                            PieceType promotion = PieceType.Empty;
                            if (move.Length == 5)
                            {
                                Console.WriteLine("Trying to promote");
                                promotion = MoveParser.charToPieceType((char)move.Substring(4, 1)[0]);
                            }
                            Move newMove = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promotion);

                            if(MoveParser.isMoveValid(newMove, position))
                            {
                                position.makeMove(newMove, new UnMakeInfo());
                                previousMoves.Add(newMove);
                            }
                        }
                        if (movegen.legalMoves(position).Count == 0)
                        {
                            i = 100;
                            gameCompleted = true;
                        }
                        else
                        {
                            i = (i == 0) ? 50 : 0;
                        }
                        b.ReportProgress(i);
                        
                                this.playerHasMoved = false;
                    }
                });
            bw.ProgressChanged += new ProgressChangedEventHandler(
                delegate(object o, ProgressChangedEventArgs args)
                {
                    this.SetPosition(position);
                }
            );
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate(object o, RunWorkerCompletedEventArgs args)
                {
                    this.SetPosition(position);
                    Console.WriteLine("Checkmate bruv.");
                });

            bw.RunWorkerAsync();
        }

        private void Subscribe(GameController gc)
        {
            gc.RaiseControllerEvent += HandleControllerEvent;
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
                this.MoveHandler(orig,dest);
            }
        }

        private static Boolean ParseMove(String input)
        {
            if (input.Length > 5)
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

        public void MoveHandler(Square orig, Square dest)
        {
            PieceType promoteTo = ((dest.getSquareNumber() <= 7 | dest.getSquareNumber() > 55) & (orig.getPiece().Equals(PieceType.p) | orig.getPiece().Equals(PieceType.P))) ? getPromotion(orig.getPiece()) : PieceType.Empty;

            Move current = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promoteTo);
            if (MoveCheck(current))
            {
                performMove(current);
            }
        }

        public void MoveHandler(Square tapped){
            if (blackIsAI & whiteIsAI)
            {
                Console.WriteLine("Both players AI, square tap ignored.");
            }
            else if ((blackIsAI & !position.whiteMove) | (whiteIsAI & position.whiteMove))
            {
                Console.WriteLine("Safety ignore.");
            }
            else
            {
                Console.WriteLine("Square " + tapped.Name + " tapped");
                moveQueue.Enqueue(tapped);
                if (this.oneClick)
                {
                    Square orig = moveQueue.Dequeue();
                    Square dest = moveQueue.Dequeue();

                    if (orig.getSquareNumber() == dest.getSquareNumber())
                    {
                        // Same square tapped twice! DESELECT
                        board.ColourBoard();
                        this.oneClick = false;
                        return;
                    }

                    PieceType promoteTo = ((dest.getSquareNumber() <= 7 | dest.getSquareNumber() > 55) & (orig.getPiece().Equals(PieceType.p) | orig.getPiece().Equals(PieceType.P))) ? getPromotion(orig.getPiece()) : PieceType.Empty;

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
                        board.ColourSquare(x.destination,Brushes.Red);
                    }
                    else if (x.destination == this.position.getEpSquare() && (board.getSquareForNumber(x.origin).getPiece() == PieceType.P || board.getSquareForNumber(x.origin).getPiece() == PieceType.p))
                    {
                        board.ColourSquare(x.destination, Brushes.Red);
                        board.ColourSquare(((Move)this.previousMoves[this.previousMoves.Count]).destination, Brushes.Red);
                    }
                    else
                    {
                        board.ColourSquare(x.destination, Brushes.Blue);
                    }
                }
            }
            
            this.ColourPiecesUnderAttack();
        }

        private void ColourPiecesUnderAttack()
        {
            List<int> controlledSquares = getControlledSquares(position);
            foreach(int i in controlledSquares)
            {
                if (MoveGenerator.squareAttacked(position, i))
                {
                    board.ColourSquare(i, Brushes.IndianRed);
                }
            }
        }

        private List<int> getControlledSquares(Position position)
        {
            List<int> controlledSquares = new List<int>();
            for (int i = 0; i < 64; i++)
            {
                if(position.getPiece(i).Equals(PieceType.Empty))continue;
                if((Char.IsLower(position.getPiece(i).ToString()[0]) && !position.whiteMove) | (Char.IsUpper(position.getPiece(i).ToString()[0]) && position.whiteMove)){
                    controlledSquares.Add(i);
                }
            }

            return controlledSquares;
        }

        private void ColourPiecesDefending()
        {

        }

        protected void performMove(Move current)
        {
            if (current.promoteTo != PieceType.Empty)
            {
                this.promotePiece(board.getSquareForNumber(current.origin), current.promoteTo);
            }
            OnRaiseBoardEvent(new BoardEvent(current, board.getSquareForNumber(current.origin).getName() + board.getSquareForNumber(current.destination).getName(), (movegen.legalMoves(this.position).Count == 0)));
            this.movePiece(current);
            this.position.makeMove(current, this.unmake);
            this.previousMoves.Add(current);
            
            board.ColourBoard();
            board.printNextTurn();

            this.oneClick = false;
            AsyncAIMoveCheck();
        }

        private void checkAITurn()
        {
            Console.WriteLine("Getting AI Move");
            String move = GetAIMove();
            if (ParseMove(move))
            {
                Console.WriteLine("_" + move + "_");
                Square orig = board.getSquareForName(move.Substring(0, 2));
                Square dest = board.getSquareForName(move.Substring(2, 2));
                Console.WriteLine(move + " " + move.Length);
                PieceType promotion = PieceType.Empty;
                if (move.Length == 5)
                {
                    Console.WriteLine("Trying to promote");
                    promotion = MoveParser.charToPieceType((char)move.Substring(4, 1)[0]);
                }
                Move newMove = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promotion);

                if (MoveParser.isMoveValid(newMove, position))
                {
                    position.makeMove(newMove, new UnMakeInfo());
                    previousMoves.Add(newMove);
                }
            }
        }

        private bool MoveCheck(Move m)
        {
            return (movegen.legalMoves(this.position).Contains(m));
        }

        private String GetAIMove()
        {
            return GetAIMove(AI);
        }

        private String GetAIMove(ComputerPlayer AIPlayer)
        {
            AIPlayer.UpdatePosition(previousMoves);
            AIPlayer.StartSearch();
            String bestMove = AIPlayer.GetBestMove();
            return bestMove;
        }

        /**
         * Allows the board to be set to an arbitrary position
         */
        public void SetPosition(Position position)
        {
            this.position = position;
            board.SetPosition(position);
        }

        /**
         * Asynchronously Plays an AI move
         */
        void AsyncAIMoveCheck(){
            if (blackIsAI | whiteIsAI)
            {
                BackgroundWorker AIbw = WorkerSetup();
                AIbw.RunWorkerAsync();
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

        void HandleControllerEvent(object sender, ControllerEvent e)
        {
        }

        private BackgroundWorker WorkerSetup()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            return worker;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.SetPosition(position);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            String move = GetAIMove();

            if (ParseMove(move))
            {
                Console.WriteLine("_" + move + "_");
                Square orig = board.getSquareForName(move.Substring(0, 2));
                Square dest = board.getSquareForName(move.Substring(2, 2));
                Console.WriteLine(move + " " + move.Length);
                PieceType promotion = PieceType.Empty;
                if (move.Length == 5)
                {
                    Console.WriteLine("Trying to promote");
                    promotion = MoveParser.charToPieceType((char)move.Substring(4, 1)[0]);
                }
                Move newMove = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promotion);

                if (MoveParser.isMoveValid(newMove, position))
                {
                    position.makeMove(newMove, new UnMakeInfo());
                    previousMoves.Add(newMove);
                }
            }
            ((BackgroundWorker)sender).ReportProgress(100);
        }
    }
    

    public class ControllerEvent : EventArgs
    {
        //private bool AIMoved;
        public ControllerEvent()//bool AIMoved)
        {
            //this.AIMoved = AIMoved;
        }

        /*public bool AIMoveCompleted
        {
            get { return AIMoved; }
        }*/
    }
}
