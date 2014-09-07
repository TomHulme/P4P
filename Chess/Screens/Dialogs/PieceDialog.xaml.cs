using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLogic;
using Chess;
using Tutorials;
using System.ComponentModel;
using System.Threading;

namespace Chess.Screens.Dialogs
{
    /// <summary>
    /// Interaction logic for PieceDialog.xaml
    /// </summary>
    public partial class PieceDialog : UserControl
    {
        PieceType piece;
        TutorialOne tutorialOne;
        GameController gameController;
        BackgroundWorker movesWorker;
        ArrayList squareList;

        BackgroundWorker quiz;

        public PieceDialog(PieceType piece, TutorialOne tutorialOne, GameController gameController)
        {
            InitializeComponent();

            this.piece = piece;
            this.tutorialOne = tutorialOne;
            this.gameController = gameController;

            FillInText();

            movesWorker = new BackgroundWorker();
            movesWorker.WorkerReportsProgress = true;
            movesWorker.DoWork += new DoWorkEventHandler(movesWorker_DoWork);
            movesWorker.ProgressChanged += new ProgressChangedEventHandler(movesWorker_ProgressChanged);
            movesWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(movesWorker_RunWorkerCompleted);

            quiz = new BackgroundWorker();
            quiz.WorkerReportsProgress = true;
            quiz.DoWork += new DoWorkEventHandler(quiz_DoWork);
            quiz.ProgressChanged += new ProgressChangedEventHandler(quiz_ProgressChanged);
            quiz.RunWorkerCompleted += new RunWorkerCompletedEventHandler(quiz_RunWorkerCompleted);
        }

        private void FillInText()
        {
            switch (this.piece)
            {
                case PieceType.P:
                    DialogText.Text = "The Pawn moves forward exactly one space, or optionally, two spaces when on its starting square, toward the opponent's side of the board. ";
                    DialogText.Text += "If the pawn reaches a square on the back rank of the opponent, it can be promoted to the player's choice of a Queen, Rook, Bishop, or Knight. ";
                    DialogText.Text += "A Pawn may capture an enemy piece one square diagonally ahead of the Pawn, either left or right.";
                    break;
                case PieceType.K:
                    DialogText.Text = "The King moves exactly one vacant square in any direction, forwards, backwards, left, right, or diagonally. ";
                    break;
                case PieceType.R:
                    DialogText.Text = "The Rook can move any number of vacant squares forwards, backwards, left, or right in a straight line. ";
                    break;
                case PieceType.B:
                    DialogText.Text = "The Bishop can move any number of vacant squares diagonally in a straight line. ";
                    DialogText.Text += "Consequently, a Bishop stays on the squares of the same colour throughout the game. ";
                    DialogText.Text += "The two Bishops each player starts with move on squares of opposite colours.";
                    break;
                case PieceType.Q:
                    DialogText.Text = "The Queen can move any number of vacant squares in any direction, forwards, backwards, left, right, or diagonally, in a straight line. ";
                    break;
                case PieceType.N:
                    DialogText.Text = "The Knight moves on an extended diagonal from one corner of any 2 by 3 rectangle of squares to the furthest opposite corner. ";
                    DialogText.Text += "Consequently, the Knight alternates its squares colour each time it moves. ";
                    DialogText.Text += "The Knight is the only piece that jumps over any intervening pieces when moving.";
                    break;
            }
        }

        private void Moves_Quiz_Click(object sender, RoutedEventArgs e)
        {
            if (!quiz.IsBusy)
            {
                quiz.RunWorkerAsync();
                Moving.Background = Brushes.Green;
            }
        }

        private void Moves_Click(object sender, RoutedEventArgs e)
        {
            //explain how a piece moves in writing.
            //also show possible moves that can be made
            if (!movesWorker.IsBusy)
            {
                movesWorker.RunWorkerAsync();
            }
        }

        //called when the moves worker is completed
        void movesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Moves highlighted.");
        }

        //called when the worker calls the report progress method
        void movesWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 100)
            {
                //create animations
                foreach (Square s in squareList)
                {
                    Storyboard highlight = (StoryBoardCreator.NewHighlighter(s, Brushes.Gold, e.ProgressPercentage));
                    highlight.Begin();
                }
            }
        }

        //called when the moves worker is started
        void movesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //go through pieces
            //highlight potential moves
            //going from left to right

            //get moves list
            //get origin squares

            ArrayList moves = MoveGenerator.mgInstance.legalMoves(gameController.position);
            HashSet<int> originSquares = new HashSet<int>();
            squareList = new ArrayList();
            //delay for storyboards
            int count = 1; 

            foreach (Move move in moves)
            {
                originSquares.Add(move.origin);
            }

            foreach (int i in originSquares)
            {
                squareList.Clear();

                //origin square gets highlighted too
                squareList.Add(gameController.board.getSquareForNumber(i));
                //for each move with the same origin square, create storyboards
                foreach (Move move in moves)
                {
                    if (move.origin == i)
                    {
                        squareList.Add(gameController.board.getSquareForNumber(move.destination));
                        Console.WriteLine("Adding in square " + move.destination);
                    }
                }

                movesWorker.ReportProgress(count);
                Thread.Sleep(500);

                count++;
            }
        }

        void quiz_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogText.Text = "Quiz Complete! Great job! You can click the quiz button to start a new one.";
        }

        void quiz_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                //highlight all squares in square list
                foreach (Square s in squareList)
                {
                    DialogText.Text = "Navigate to the highlighted squares!";
                    s.colourRectangle(Brushes.Green);
                }
            }
            else if (e.ProgressPercentage == 2)
            {
                Square s = e.UserState as Square;
                s.colourRectangle(Brushes.Transparent);
            }
        }

        void quiz_DoWork(object sender, DoWorkEventArgs e)
        {
            gameController.ShowHighlightedMoves = false;

            Random random = new Random();
            squareList = new ArrayList();

            int iterations = 7;

            String initialPosition = FENConverter.convertPositionToFEN(gameController.position);

            //generate list of squares to visit
            while (iterations > 0)
            {
                ArrayList generatedMoves = MoveGenerator.mgInstance.psuedoLegalMoves(gameController.position);

                int moveIndex = (int)(random.NextDouble() * generatedMoves.Count);
                Move selectedMove = (Move)generatedMoves.ToArray()[moveIndex];

                squareList.Add(gameController.board.getSquareForNumber(selectedMove.destination));

                gameController.position.setPiece(selectedMove.destination, gameController.position.getPiece(selectedMove.origin));

                iterations--;
            }

            //reset the position to the initial starting position
            gameController.position = FENConverter.convertPiecePlacementToPosition(initialPosition);
            gameController.position.sameActiveColor = true;
            
            //highlight squares to visit
            quiz.ReportProgress(1);

            Boolean quizFinished = false;
            //Boolean halfMove = false;

            while (!quizFinished)
            {
                //if (halfMove)
                //{
                //    String destination = Square.CopySquare(gameController.tutorialQueue.Dequeue());
                //    String origin = Square.CopySquare(gameController.tutorialQueue.Dequeue());

                //    Console.WriteLine("Desitination Square: " + destination);
                //    Console.WriteLine("Origin Square: " + origin);

                //    foreach (Square s in squareList)
                //    {
                //        if (destination.Equals(s.getName()))
                //        {
                //            squareList.Remove(s);
                //            quiz.ReportProgress(2, s);
                //            Console.WriteLine("Removing square " + s.getName());
                //        }
                //    }
                //    halfMove = false;
                //}
                //else if (gameController.tutorialQueue.Count == 1)
                //{
                //    halfMove = true;
                //}
                if (squareList.Count == 0)
                {
                    quizFinished = true;
                }
                else
                {
                    ArrayList tempList = new ArrayList();
                    foreach (Square s in squareList)
                    {
                        if (s.getPiece() != PieceType.Empty)
                        {
                            tempList.Add(s);
                            quiz.ReportProgress(2, s);
                        }
                    }

                    foreach (Square s in tempList)
                    {
                        squareList.Remove(s);
                    }

                }
            }
        }
    }
}
