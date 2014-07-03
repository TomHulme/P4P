using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Chess;

namespace EngineLogic
{
    class SFEngine
    {
        public Process engineProcess;
        public StreamReader engineOutput;
        public StreamWriter engineInput;

        public SFEngine()
        {
            StartEngine();
        }

        /**
         * This involves linking this process to the engine application
         */
        public void Setup()
        {
            if (engineProcess != null)
            {
                Console.WriteLine("Error! Engine Process Already Started!");
                return;
            }
            engineProcess = new Process();

            try
            {
                engineProcess.StartInfo.UseShellExecute = false; 
                engineProcess.StartInfo.FileName = App.getPath() + @"Resources\stockfish-dd-32.exe";
                engineProcess.StartInfo.CreateNoWindow = true;
                engineProcess.StartInfo.RedirectStandardInput = true;
                engineProcess.StartInfo.RedirectStandardOutput = true;
                engineProcess.Start();
                engineOutput = engineProcess.StandardOutput;
                engineInput = engineProcess.StandardInput;
                Console.WriteLine("Process Started.");
                Console.WriteLine(engineOutput.ReadLine());
                engineInput.WriteLine("ucinewgame");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /**
         * Start UCI engine by inputing the "uci" command
         */
        public void StartEngine()
        {
            Setup();
            String engine = "Engine name not found";
            if (engineProcess == null)
            {
                Console.WriteLine("Engine failed to launch.");
                return;
            }

            engineInput.WriteLine("uci");
            String line;
            try
            {
                do
                {
                    line = engineOutput.ReadLine();
                    //Console.WriteLine(line);
                    if (line.StartsWith("id name"))
                    {
                        engine = line.Substring(8);
                    }
                } while (line != "uciok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(engine);
        }

        public Boolean StopEngine()
        {
            return true;
        }

    }
}
