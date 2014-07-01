using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Chess
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //public MainMenu mainmenu = new MainMenu();
        //public Game game = new Game();
        public static string getPath(){
            string PATH = System.Reflection.Assembly.GetAssembly(typeof(Game)).Location;
            int index = PATH.LastIndexOf("bin\\Debug\\Chess.exe");
            string p = PATH.Remove(index);
            return p;
        }
    }
}