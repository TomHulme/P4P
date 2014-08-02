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
using Tutorials;
using GameLogic;
using System.Threading;
using System.ComponentModel;

namespace Chess.Screens.TutorialDialogs
{
    /// <summary>
    /// Interaction logic for BoardDialog.xaml
    /// </summary>
    partial class BoardDialog : UserControl
    {
        TutorialOne tutorialOne;
        GameController gameController;
        ArrayList squareList;
        Square tappedSquare;

        BackgroundWorker quiz;

        public BoardDialog(TutorialOne tutorialOne, GameController gameController)
        {
            InitializeComponent();

            this.tutorialOne = tutorialOne;
            this.gameController = gameController;

            SetIntroText();

            tappedSquare = new Square("hello", -1);

            quiz = new BackgroundWorker();
            quiz.WorkerReportsProgress = true;
            quiz.WorkerSupportsCancellation = true;

            quiz.DoWork += new DoWorkEventHandler(quiz_DoWork);
            quiz.ProgressChanged += new ProgressChangedEventHandler(quiz_ProgressChanged);
            quiz.RunWorkerCompleted += new RunWorkerCompletedEventHandler(quiz_RunWorkerCompleted);
        }

        private void SetIntroText()
        {
            DialogText.Text = "The board is made up of rows and columns. ";
            DialogText.Text += "The columns, called files, are labeled by the letters A to H from left to right from the white player's point of view. ";
            DialogText.Text += "The rows, called ranks, are labeled by the numbers 1 to 8, with 1 being closest to the white player. ";
        }

        private void Ranks_Click(object sender, RoutedEventArgs e)
        {
            SetIntroText();

            BackgroundWorker highlight = new BackgroundWorker();
            highlight.WorkerReportsProgress = true;
            highlight.DoWork += new DoWorkEventHandler(highlight_DoWork);
            highlight.ProgressChanged += new ProgressChangedEventHandler(highlight_ProgressChanged);
            highlight.RunWorkerCompleted += new RunWorkerCompletedEventHandler(highlight_RunWorkerCompleted);

            highlight.RunWorkerAsync("Ranks");
        }

        private void Files_Click(object sender, RoutedEventArgs e)
        {
            SetIntroText();

            BackgroundWorker highlight = new BackgroundWorker();
            highlight.WorkerReportsProgress = true;
            highlight.DoWork += new DoWorkEventHandler(highlight_DoWork);
            highlight.ProgressChanged += new ProgressChangedEventHandler(highlight_ProgressChanged);
            highlight.RunWorkerCompleted += new RunWorkerCompletedEventHandler(highlight_RunWorkerCompleted);

            highlight.RunWorkerAsync("Files");
        }

        private void Ranks_Files_Click(object sender, RoutedEventArgs e)
        {
            //check that background worker isnt busy
            //if it is, dont start new quiz

            gameController.tutorialQueue = new Queue<Square>();

            if (quiz.IsBusy == false)
            {
                Tuple<GameController, String> argument = Tuple.Create<GameController, String>(gameController, "RanksFiles");
                quiz.RunWorkerAsync(argument);
            }
        }

        private void Squares_Click(object sender, RoutedEventArgs e)
        {
            //check that background worker isnt busy
            //if it is, dont start new quiz

            gameController.tutorialQueue = new Queue<Square>();

            if (quiz.IsBusy == false)
            {
                Tuple<GameController, String> argument = Tuple.Create<GameController, String>(gameController, "Squares");
                quiz.RunWorkerAsync(argument);
            }
        }

        void highlight_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Worker completed.");
            //gameController.board = new Board(false, tutorialOne.GetPosition(), gameController);
            //gameController.board.setup();
            gameController.SetPosition(tutorialOne.GetPosition());
        }

        void highlight_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ArrayList storyboards = new ArrayList();

            if (e.ProgressPercentage != 100)
            {
                //create animations
                foreach (Square s in squareList)
                {
                    Storyboard highlight = (StoryBoardCreator.FadeInFadeOutSquare(s, Brushes.Blue, e.ProgressPercentage));
                    Console.WriteLine("creating storyboard " + e.ProgressPercentage);
                    highlight.Begin();
                }
            }
        }

        //need to have one background worker for this class.
        //if ranks is clicked and then files. 
        //check if worker is busy, start a cancel.
        //then fire up the files worker on the cancelling worker

        void highlight_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //create storyboards
            String[] squares;

            switch (e.Argument.ToString())
            {
                case ("Ranks"):
                    {
                        for (int i = 1; i <= 8; i++)
                        {
                            //get square objects for ranks
                            squares = TutorialOne.HighLightRank(i);
                            squareList = new ArrayList();
                            foreach (String s in squares)
                            {
                                squareList.Add(gameController.board.getSquareForName(s));
                            }

                            worker.ReportProgress(i);
                            Thread.Sleep(500);
                        }
                        break;
                    }
                case ("Files"):
                    {
                        String[] files = { "a", "b", "c", "d", "e", "f", "g", "h" };
                        for (int i = 0; i < 8; i++)
                        {
                            //get square objects for files
                            squares = TutorialOne.HighLightFile(files[i]);
                            squareList = new ArrayList();
                            foreach (String s in squares)
                            {
                                squareList.Add(gameController.board.getSquareForName(s));
                            }

                            worker.ReportProgress(i + 1);
                            Thread.Sleep(500);
                        }
                        break;
                    }
                default:
                    break;
            }
            //worker has finished
            Thread.Sleep(500);
            worker.ReportProgress(100);
        }

        void quiz_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                DialogText.Text = "It got canceled!";
            }
            else
            {
                DialogText.Text = "Quiz Complete! Great job! You can click either quiz button to start a new one.";
            }

        }

        void quiz_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //update text, clear board?
            String text = e.UserState as String;
            DialogText.Text = text;
            
        }

        void quiz_DoWork(object sender, DoWorkEventArgs e)
        {
            Random randGen = new Random();
            Tuple<GameController, String> args = e.Argument as Tuple<GameController, String>;

            Boolean quizComplete = false;

            GameController controller = args.Item1 as GameController;

            switch (args.Item2)
            {
                case ("RanksFiles"):
                    {
                        String[] files = { "a", "b", "c", "d", "e", "f", "g", "h" };
                        Boolean quizFiles = true;

                        int questionNumber = 0;

                        while (questionNumber < 4)
                        {
                            //if another quiz has been requested, cancel this one
                            if (quiz.CancellationPending)
                            {
                                Console.WriteLine("trying to cancel.");
                                e.Cancel = true;
                                quiz.ReportProgress(100, e);
                            }
                            else
                            {
                                Boolean questionCorrect = false;

                                //alternates between testing the files and ranks
                                if (quizFiles)
                                {
                                    //choose random file
                                    String file = files[(int)(randGen.NextDouble() * 7)];

                                    String text = "Please tap a square from the '" + file + "' file.";

                                    quiz.ReportProgress((questionNumber + 1) * 25 / 2, text);

                                    do
                                    {
                                        //quiz.ReportProgress((questionNumber + 1) * 25 / 2, text);

                                        if (controller.tutorialQueue.Count != 0)
                                        {
                                            String tapped = Square.CopySquare(controller.tutorialQueue.Dequeue());
                                            //Console.WriteLine("square " + tapped.Name + "was tapped");
                                            //quiz.ReportProgress(questionNumber * 25, "Get Square");
                                            //Thread.Sleep(1000);

                                            Console.WriteLine(tapped);

                                            if (file[0] == tapped[0])
                                            {
                                                //Console.WriteLine("correct file tapped");
                                                quiz.ReportProgress(questionNumber * 25, "Correct!");
                                                questionCorrect = true;
                                                Thread.Sleep(1000);
                                            }
                                            else
                                            {
                                                //Console.WriteLine("incorrect file tapped");
                                                quiz.ReportProgress(questionNumber * 25, "Incorrect, that was the " + tapped[0] + " file.");
                                                Thread.Sleep(1000);
                                            }
                                        }
                                    }
                                    while (!questionCorrect);

                                    //quiz.ReportProgress(questionNumber * 25, "Good Work. You got it right!");

                                    quizFiles = false;
                                }
                                else
                                {
                                    //chose random rank
                                    int rank = (int)(randGen.NextDouble() * 8) + 1;

                                    String text = "Pleaes tap a square from rank " + rank + ".";
                                    //Update text box with quiz question
                                    quiz.ReportProgress((questionNumber + 1) * 25 / 2, text);

                                    do
                                    {
                                        //quiz.ReportProgress((questionNumber + 1) * 25 / 2, text);

                                        if (controller.tutorialQueue.Count != 0)
                                        {
                                            String tapped = Square.CopySquare(controller.tutorialQueue.Dequeue());

                                            if (rank == Convert.ToInt32(new String(tapped[1], 1)))
                                            {
                                                quiz.ReportProgress(questionNumber * 25, "Correct!");
                                                questionCorrect = true;
                                                Thread.Sleep(1000);
                                            }
                                            else
                                            {
                                                quiz.ReportProgress(questionNumber * 25, "Incorrect, that was the " + tapped[1] + " rank.");
                                                Thread.Sleep(1000);
                                            }
                                        }
                                    }
                                    while (!questionCorrect);

                                    //Update text box with feedback on answer
                                    //quiz.ReportProgress(questionNumber * 25, "Good Work. You got it right!");

                                    quizFiles = true;
                                }

                                questionNumber++;
                                if (questionNumber >= 4)
                                {
                                    quizComplete = true;
                                }
                            }
                        }
                        break;
                    }
                case ("Squares"):
                    {
                        //(int)(randomNumber.NextDouble() * 8);

                        int questionNumber = 0;

                        while (questionNumber < 4)
                        {
                            //if another quiz has been requested, cancel this one
                            if (quiz.CancellationPending)
                            {
                                Console.WriteLine("trying to cancel.");
                                e.Cancel = true;
                                quiz.ReportProgress(100, e);
                            }
                            else
                            {
                                Boolean questionCorrect = false;

                                //choose random square
                                Square square = gameController.board.getSquareForNumber((int)(randGen.NextDouble() * 64));

                                String text = "Please tap the square '" + square.getName() + "'.";
                                //Updates the text box to ask the user which square to tap
                                quiz.ReportProgress((questionNumber + 1) * 25 / 2, text);

                                do
                                {
                                    if (controller.tutorialQueue.Count != 0)
                                    {
                                        String tapped = Square.CopySquare(controller.tutorialQueue.Dequeue());

                                        if (square.getName().Equals(tapped))
                                        {
                                            quiz.ReportProgress(questionNumber * 25, "Correct!");
                                            questionCorrect = true;
                                            Thread.Sleep(1000);
                                        }
                                        else
                                        {
                                            quiz.ReportProgress(questionNumber * 25, "Incorrect, that was square " + tapped);
                                            Thread.Sleep(1000);
                                        }
                                    }
                                }
                                while (!questionCorrect);
                                
                                //Gives the user feedback on tapped square
                                //quiz.ReportProgress(questionNumber * 25, "Good Work. You got it right!");

                                questionNumber++;
                                if (questionNumber >= 4)
                                {
                                    quizComplete = true;
                                }
                            }
                        }
                        break;
                    }
                default:
                    break;
            }

            //this only updates the text box if they finish the quiz
            if (quizComplete)
            {
                quiz.ReportProgress(100);
            }
        }
    }
}
