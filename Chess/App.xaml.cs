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

        public static readonly int Height = 720;
        public static readonly int Width = 1280;


        public static string getPath(){
            string PATH = System.Reflection.Assembly.GetAssembly(typeof(Game)).Location;
            int index = PATH.LastIndexOf("bin\\Debug\\Chess.exe");
            string p = PATH.Remove(index);
            return p;
        }
    }
}