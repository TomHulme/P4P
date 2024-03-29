﻿using System;
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
		// Debug Option
        public bool debugging = false;
		
		/* * * * * * * * * *\
		 * Class Variables *
		\* * * * * * * * * */
        internal Board board;
        internal Position position;
        private MoveGenerator movegen;
		
		// AI Related
        private bool blackIsAI = false;
        private bool whiteIsAI = false;
        private SFEngine engine = Chess.Properties.Settings.Default.ChessEngine;
        private ComputerPlayer AI;
        private ComputerPlayer TurnGenerator;
        internal BackgroundWorker bw;
        private bool endCvCGame = false;
		
		// Move Related
        internal event EventHandler<BoardEvent> RaiseBoardEvent;
        internal event EventHandler<ControllerEvent> RaiseControllerEvent;
        private Queue<Square> moveQueue = new Queue<Square>();
        private ArrayList previousMoves = new ArrayList();
        private bool oneClick = false;
        private UnMakeInfo unmake = new UnMakeInfo();
		
		// Help Related
        private bool playerHasMoved = false;
        private bool showHighlightMoves = false;
        private bool showDefendedPieces = false;
        private bool showAttackedPieces = false;
        private bool showOnlyDefendedPiecesUnderAttack = false;
        private bool suggestingMove = false; //true when we search for the best move before the players turn
        private Move currentSuggestedMove = new Move(11,27,PieceType.Empty);

		// Other
        internal Boolean tutorialFlag;
        private bool blackReversed = true;
        internal Boolean ignoreSuggestion = false;
        internal volatile Queue<Square> tutorialQueue = new Queue<Square>();
		

		/**
		 * GameController Constructors
		 * Create and Setup Board, initialise class variables.
		 */
        public GameController(bool b, Position pos)
        {
            this.board = new Board(b, pos, this);
            board.setup();
            this.position = pos;
            this.movegen = new MoveGenerator();
            this.tutorialFlag = false;

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

            TurnGenerator = SetupTurnGenerator();
            

            if (blackIsAI & whiteIsAI)
            {
                bw = new BackgroundWorker();
                bwSetup();
            }else{
                AI = new ComputerPlayer(engine.engineProcess.StandardOutput, engine.engineProcess.StandardInput);
                ResetEngineDifficulty();
            }
            
        }

		/**
		 * Get a Turn Generator using the ComputerPlayer class
		 */
        private ComputerPlayer SetupTurnGenerator(){
            ComputerPlayer turnGen = new ComputerPlayer(engine.engineProcess.StandardOutput, engine.engineProcess.StandardInput);
            turnGen.setMoveDepth(5);
            return turnGen;
        }

		/**
		 * Reset the skill level based on the Difficulty set in the Settings
		 */
        private void ResetEngineDifficulty(){
            if (engine != null)
            {
                engine.setSkillLevel(Chess.Properties.Settings.Default.DifficultySetting);
            }
            if (AI != null)
            {
                AI.setMoveDepth(Chess.Properties.Settings.Default.DifficultySetting);
            }
        }

		/**
		 * Computer vs Computer Background Worker
		 * Used to Asynchronously play a Computer vs Computer game.
		 */
        private void bwSetup()
        {
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            Move test = new Move(0, 0, PieceType.Empty);
            bw.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    int i = 0;
                    bool gameCompleted = false;
                    while (!gameCompleted && !endCvCGame) 
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
                                if (MoveParser.isMoveCapture(newMove, position))
                                {
                                    Console.WriteLine("Computer captured something");
                                    b.ReportProgress(1, newMove);
                                }
                                position.makeMove(newMove, new UnMakeInfo());
                                previousMoves.Add(newMove);
                                test = newMove;
                            }
                        }
                        if (movegen.legalMoves(position).Count == 0)
                        {
							// Checkmate Condition
                            i = 100;
                            gameCompleted = true;
                        }
                        else
                        {
                            i = (i == 0) ? 50 : 0;
                        }
                        b.ReportProgress(i);
                        this.playerHasMoved = false;
                        Thread.Sleep(2000);
                    }
                });
            bw.ProgressChanged += new ProgressChangedEventHandler(
                delegate(object o, ProgressChangedEventArgs args)
                {
                    if (args.ProgressPercentage == 1)
                    {
                        Move move = args.UserState as Move;
                        OnRaiseControllerEvent(new ControllerEvent(board.getSquareForNumber(move.destination).getPiece()));
                    }
                    else
                    {
                        this.SetPosition(position);
						board.UnColourBorders();
						ColourPreviousMove(test);
                        OnRaiseControllerEvent(new ControllerEvent());
                    }
                }
            );
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate(object o, RunWorkerCompletedEventArgs args)
                {
                    this.SetPosition(position);
                    Console.WriteLine("Computer vs Computer game completed.");
                });

            bw.RunWorkerAsync();
        }

		/**
		 * Subscribe to the Handler
		 */
        private void Subscribe(GameController gc)
        {
            gc.RaiseControllerEvent += HandleControllerEvent;
        }

		/**
		 * Get the piece type of the piece being promoted.
		 * Always a Queen. TODO: Underpromotion.
		 */ 
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
        }

		/**
		 * Move a piece based of an inputted Move object.
		 * Also deals with moving the actual piece image
		 */
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
                Move last = (Move)this.previousMoves[this.previousMoves.Count-1];
                Square enPassantPawn = board.getSquareForNumber(last.destination);
                enPassantPawn.setPiece(PieceType.Empty);
                enPassantPawn.clearPieceImage();
            }

            if (MoveParser.moveObjectToString(current, this.position).Contains("O-O"))
            {
                // CASTLING!
                Console.WriteLine("Castling");

                Image img;
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
                img = originSquare.getPieceImage();
                originSquare.clearPieceImage();
                destinationSquare.setPieceImage(img);
                img = rookOrigin.getPieceImage();
                rookOrigin.clearPieceImage();
                rookDestination.setPieceImage(img);
                rookDestination.setPiece(rookOrigin.getPiece());
                rookOrigin.setPiece(PieceType.Empty);
            }
            else
            {
                Image img;
                destinationSquare.clearPieceImage();
                img = originSquare.getPieceImage();
                originSquare.clearPieceImage();
                destinationSquare.setPieceImage(img);
            }
        }

		/**
		 * Promotes a piece by redrawing it.
		 */
        private void promotePiece(Square sq, PieceType p)
        {
            sq.Children.Clear();
            board.drawPiece(p, sq);
        }

		/**
		 * Takes a string and parses whether it is valid. For use in the MoveHandler
		 */
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

		/**
		 * Performs a move based on a move string. Used to play AI moves
		 */
        public void MoveHandler(string MoveString)
        {
            if (ParseMove(MoveString))
            {
                Square orig = board.getSquareForName(MoveString.Substring(0,2));
                Square dest = board.getSquareForName(MoveString.Substring(2,2));
                this.MoveHandler(orig,dest);
            }
        }

		/**
		 * Performs a move based on the origin and destination squares
		 */
        public void MoveHandler(Square orig, Square dest)
        {
            PieceType promoteTo = ((dest.getSquareNumber() <= 7 | dest.getSquareNumber() > 55) & (orig.getPiece().Equals(PieceType.p) | orig.getPiece().Equals(PieceType.P))) ? getPromotion(orig.getPiece()) : PieceType.Empty;

            Move current = new Move(orig.getSquareNumber(), dest.getSquareNumber(), promoteTo);
            if (MoveCheck(current))
            {
                performMove(current);
            }
        }

		/**
		 * Two Part MoveHandler
		 * First Square received is pushed onto a stack.
		 * Second Square is then received, and the First Square gets popped. They are combined to create a move.
		 * If the Move is valid it is played. If it is not, the Second Square gets pushed onto the stack, and the cycle repeats.
		 */
        public void MoveHandler(Square tapped){

            if (tutorialFlag)
            {
				// Play differently if in tutorial mode
                Console.WriteLine("Tutorial square " + tapped.Name + " tapped");
                tutorialQueue.Enqueue(tapped);
            }
            else if (blackIsAI && whiteIsAI)
            {
				// Ignore non-AI input
                Console.WriteLine("Both players AI, square tap ignored.");
            }
            else if ((blackIsAI && !position.whiteMove) || (whiteIsAI & position.whiteMove))
            {
				// Ignore input during AI turn
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
                        DoColourations();
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
                        DoColourations();
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
		
		/**
		 * Performs a move on the internal position, based on a Move object.
		 * Updates are resyncs the Board.
		 */
        protected void performMove(Move current)
        {
            if (current.promoteTo != PieceType.Empty)
            {
                this.promotePiece(board.getSquareForNumber(current.origin), current.promoteTo);
            }
            OnRaiseBoardEvent(new BoardEvent(current, board.getSquareForNumber(current.origin).getName() + board.getSquareForNumber(current.destination).getName(), (movegen.legalMoves(this.position).Count == 0)));
            if (MoveParser.isMoveCapture(current, position))
            {
                OnRaiseControllerEvent(new ControllerEvent(board.getSquareForNumber(current.destination).getPiece()));
            }
            this.movePiece(current);
            this.position.makeMove(current, this.unmake);
            this.previousMoves.Add(current);
            if (!ignoreSuggestion && movegen.legalMoves(position).Count != 0)
            {
                String s = GetAIMove(TurnGenerator);
                currentSuggestedMove = new Move(board.getSquareForName(s.Substring(0, 2)).getSquareNumber(), board.getSquareForName(s.Substring(2, 2)).getSquareNumber(), PieceType.Empty);
            }
            board.printNextTurn();
            this.oneClick = false;
            DoColourations();
            ColourPreviousMove(current);
            AsyncAIMoveCheck();
            OnRaiseControllerEvent(new ControllerEvent());

            Console.WriteLine(MoveParser.moveObjectToString(current));
        }

		/**
		 * Colours the Legal Moves based from the selected Square
		 */
        private void ColourLegalMoves(int originSquare)
        {
            if(board.getSquareForNumber(originSquare).getPiece() != PieceType.Empty)
            {
                if (char.IsLower(board.getSquareForNumber(originSquare).getPiece().ToString()[0]) && !position.whiteMove
                    ||
                    !char.IsLower(board.getSquareForNumber(originSquare).getPiece().ToString()[0]) && position.whiteMove
                    )
                {
                    board.ColourSquare(originSquare, Chess.Properties.Settings.Default.HighlightMove);
                }
            }
            if (!showHighlightMoves) return;
            foreach (Move x in new MoveGenerator().legalMoves(this.position))
            {
                if (x.origin == originSquare)
                {
                    if (board.getSquareForNumber(x.destination).getPiece() != PieceType.Empty)
                    {
                        board.ColourSquare(x.destination,Chess.Properties.Settings.Default.TakablePieces);
                    }
                    else if (x.destination == this.position.getEpSquare() && (board.getSquareForNumber(x.origin).getPiece() == PieceType.P || board.getSquareForNumber(x.origin).getPiece() == PieceType.p))
                    {
                        board.ColourSquare(x.destination, Chess.Properties.Settings.Default.TakablePieces);
                        board.ColourSquare(((Move)this.previousMoves[this.previousMoves.Count]).destination, Chess.Properties.Settings.Default.TakablePieces);
                    }
                    else
                    {
                        board.ColourSquare(x.destination, Chess.Properties.Settings.Default.HighlightMove);
                    }
                }
            }
        }

		/**
		 * Colours the Pieces currently under attack based on the whos turn it is
		 */
        internal void ColourPiecesUnderAttack()
        {
            List<int> controlledSquares = getControlledSquares(position);
            foreach(int i in controlledSquares)
            {
                if (MoveGenerator.squareAttacked(position, i))
                {
                    board.ColourSquare(i, Chess.Properties.Settings.Default.AttackedPieces);
                }
            }
        }

		/**
		 * Colours the Pieces currently being defended based on the whos turn it is
		 */
        internal void ColourPiecesDefending()
        {
            List<int> enemyControlledSquares = getEnemyControlledSquares(position);
            foreach (int i in enemyControlledSquares)
            {
                if (MoveGenerator.squareAttacked(position, i))
                {
                    board.ColourSquare(i, Chess.Properties.Settings.Default.DefendedPieces);
                }
            }
        }

		/**
		 * Colours the Pieces currently being defended IFF they are attackable
		 */
        internal void ColourOnlyDefendedPiecesUnderAttack()
        {
            List<int> enemyControlledSquares = getEnemyControlledSquares(position);
            List<int> attackedSquares = new List<int>();
            // Remove the enemy pieces which are not under attack
            foreach (int i in enemyControlledSquares)
            {
                if (MoveGenerator.squareAttacked(position, i))
                {
                    attackedSquares.Add(i);
                }
            }

            if (attackedSquares.Count == 0) return;

            Position tempPos;

            foreach (int i in attackedSquares)
            {
                tempPos = FENConverter.convertFENToPosition(FENConverter.convertPositionToFEN(position));
                
                tempPos.setPiece(i, ((position.whiteMove) ? PieceType.Q : PieceType.q));
                tempPos.setWhiteMove(!position.whiteMove);
                if (MoveGenerator.squareAttacked(tempPos, i))
                {
                    board.ColourSquare(i, Chess.Properties.Settings.Default.DefendedPieces);
                }
            }
        }

		/**
		 * Colours the borders to show the last 2 Squares involved in the previous move played. Uses 2 square numbers
		 */
        internal void ColourPreviousMove(int pos1, int pos2)
        {
            board.ColourSquareBorder(pos1, Chess.Properties.Settings.Default.PreviousMove);
            board.ColourSquareBorder(pos2, Chess.Properties.Settings.Default.PreviousMove);
        }

		/**
		 * Colours the borders to show the last 2 Squares involved in the previous move played. Uses 1 Move object
		 */
        internal void ColourPreviousMove(Move move)
        {
            ColourPreviousMove(move.origin, move.destination);
        }

		/**
		 * Colours the Suggested Move to show the best move to play. Uses 2 square numbers
		 */
        internal void ColourSuggestedMove(int pos1, int pos2)
        {
            board.ColourSquareBorder(pos1, Chess.Properties.Settings.Default.SuggestedMove);
            board.ColourSquareBorder(pos2, Chess.Properties.Settings.Default.SuggestedMove);
        }

		/**
		 * Colours the Suggested Move to show the best move to play. Uses 1 Move object
		 */
        internal void ColourSuggestedMove(Move move)
        {
            ColourSuggestedMove(move.origin, move.destination);
        }

		/**
		 * Invokes all the Colouration methods based on the options set to enable/disable them.
		 */
        internal void DoColourations()
        {
            board.UnColourBoard(Chess.Properties.Settings.Default.HighlightMove);
            board.UnColourBoard(Chess.Properties.Settings.Default.AttackedPieces);
            board.UnColourBoard(Chess.Properties.Settings.Default.DefendedPieces);
            board.UnColourBoard(Chess.Properties.Settings.Default.SuggestedMove);
            board.UnColourBoard(Chess.Properties.Settings.Default.TakablePieces);
            board.UnColourBorders();
            if (suggestingMove)
            {
                this.ColourSuggestedMove(currentSuggestedMove);
            }
            if (showOnlyDefendedPiecesUnderAttack)
            {
                this.ColourOnlyDefendedPiecesUnderAttack();
            }
            if (showAttackedPieces)
            {
                this.ColourPiecesUnderAttack();
            }
            if (showDefendedPieces)
            {
                this.ColourPiecesDefending();
            }
        }

		/**
		 * Gets a list of all the Square numbers of Sqaures controlled by the current player
		 */
        private List<int> getControlledSquares(Position position)
        {
            List<int> controlledSquares = new List<int>();
            for (int i = 0; i < 64; i++)
            {
                if (position.getPiece(i).Equals(PieceType.Empty)) continue;
                if ((Char.IsLower(position.getPiece(i).ToString()[0]) && !position.whiteMove) | (Char.IsUpper(position.getPiece(i).ToString()[0]) && position.whiteMove))
                {
                    controlledSquares.Add(i);
                }
            }

            return controlledSquares;
        }

		/**
		 * Gets a list of all the Square numbers of Sqaures controlled by the enemy player
		 */
        private List<int> getEnemyControlledSquares(Position position)
        {
            List<int> controlledSquares = new List<int>();
            for (int i = 0; i < 64; i++)
            {
                if (position.getPiece(i).Equals(PieceType.Empty)) continue;
                if ((Char.IsLower(position.getPiece(i).ToString()[0]) && position.whiteMove) | (Char.IsUpper(position.getPiece(i).ToString()[0]) && !position.whiteMove))
                {
                    controlledSquares.Add(i);
                }
            }

            return controlledSquares;
        }

		/**
		 * If this method is called, it will get a turn for a computer player and execute it.
		 */
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

		/**
		 * Checks if a Move is legal
		 */
        private bool MoveCheck(Move m)
        {
            return (movegen.legalMoves(this.position).Contains(m));
        }

		// Calls the GetAIMove method with the default AI input
        private String GetAIMove()
        {
            return GetAIMove(AI);
        }

		/**
		 * Gets the best move playable by an AI player.
		 */
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

            if (showOnlyDefendedPiecesUnderAttack)
            {
                this.ColourOnlyDefendedPiecesUnderAttack();
            }
            if (showAttackedPieces)
            {
                this.ColourPiecesUnderAttack();
            }
            if (showDefendedPieces)
            {
                this.ColourPiecesDefending();
            }
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


        
        /* 
		 * Event handling best practice from http://msdn.microsoft.com/en-us/library/w369ty8x.aspx
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



        /* 
		 * Event handling best practice from http://msdn.microsoft.com/en-us/library/w369ty8x.aspx
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

		/**
		 * DEPRECATED. Obviously no longer in use, but not deleted so we can easily implement it again.
		 */
        void HandleControllerEvent(object sender, ControllerEvent e)
        {
        }

		/**============================================================================================
		 * Background Worker for Player vs Computer Games.
		 */
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
            OnRaiseControllerEvent(new ControllerEvent());
            ColourPreviousMove((Move)previousMoves[previousMoves.Count - 1]);
            String s = GetAIMove(TurnGenerator);
            if(!s.Equals("(none)")){
            currentSuggestedMove = new Move(board.getSquareForName(s.Substring(0, 2)).getSquareNumber(), board.getSquareForName(s.Substring(2, 2)).getSquareNumber(), PieceType.Empty);
            }
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
                Thread.Sleep(2000);
            }
            ((BackgroundWorker)sender).ReportProgress(100);
        }
		/**============================================================================================
		 * End of PvC
		 */
		
		/*
		 * Getters/Setters for Class Variables
		 */
        public bool ShowHighlightedMoves
        {
            get { return this.showHighlightMoves; }
            set { this.showHighlightMoves = value; }
        }

        public bool ShowDefendedPieces 
        {
            get { return this.showDefendedPieces; }
            set { this.showDefendedPieces = value; }
        }

        public bool ShowAttackedPieces
        {
            get { return this.showAttackedPieces; }
            set { this.showAttackedPieces = value; }
        }

        public bool ShowOnlyDefendedPiecesUnderAttack
        {
            get { return this.showOnlyDefendedPiecesUnderAttack; }
            set { this.showOnlyDefendedPiecesUnderAttack = value; }
        }

        public bool SuggestingMove
        {
            get { return this.suggestingMove; }
            set { this.suggestingMove = value; }
        }

        public bool EndCvCGame
        {
            get { return this.endCvCGame; }
            set { this.endCvCGame = value; }
        }

		/**
		 * Debug method to print the position
		 */ 
        internal void printPosition()
        {
            Console.WriteLine(FENConverter.convertPositionToFEN(position));
            foreach (Move m in movegen.legalMoves(position)){
                Console.Write(MoveParser.moveObjectToString(m, position) + ", ");
            }
        }
    }
    
	
	/***
	 ** Controller Event Class
	 **/
    public class ControllerEvent : EventArgs
    {
        internal PieceType p;
        
        public ControllerEvent()
        {
            p = PieceType.Empty;
        }

        public ControllerEvent(PieceType p)
        {
            this.p = p;
            Console.WriteLine(p.ToString() + " captured");
        }
    }
}
