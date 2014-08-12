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
                    DialogText.Text = "The Pawn moves forward exactly one space, or optionally, two spaces when on its starting square, toward the opponents side of the board. ";
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
                    DialogText.Text = "The Bishop can move any number of vacant sqaures diagonally in a straight line. ";
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

        private void Captures_Quiz_Click(object sender, RoutedEventArgs e)
        {
            //Set up board with an enemy piece. User has to take piece.
            //May need seperate controller for these to ensure continuity.
        }

        private void Moves_Quiz_Click(object sender, RoutedEventArgs e)
        {
            //set up board with a highlighted square. User has to navigate
            //to the lit square
        }

        private void Captures_Click(object sender, RoutedEventArgs e)
        {
            //setup board with a possible capture scenario
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

            gameController.SetPosition(tutorialOne.GetPosition());
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
            DialogText.Text = "Quiz Complete! Great job! You can click either quiz button to start a new one.";
        }

        void quiz_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            String text = e.UserState as String;
            if (e.ProgressPercentage == 1)
            {
                DialogText.Text += "\n" + text;
            }
            else
            {
                DialogText.Text = text;
            }
        }

        void quiz_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
