using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Screen
    {
        internal static int CurrentSetting = Chess.Properties.Settings.Default.DifficultySetting;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Settings(ScreenControl parentWindow): base(parentWindow)
        {
            InitializeComponent();
        }

        private void Go_Back_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.PopScreen();
        }

        private void Difficulty_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Difficulty_Slider != null)
            {
                Difficulty_Slider.Value = (int)Difficulty_Slider.Value;
                Chess.Properties.Settings.Default.DifficultySetting = ((int)Difficulty_Slider.Value);
                if (Difficulty_Label != null)
                {
                    String Difficulty_String = "";
                    switch((int)Difficulty_Slider.Value)
                    {
                        case 1:
                            Difficulty_String = "Beginner";
                            break;
                        case 2:
                            Difficulty_String = "Super Super Simple";
                            break;
                        case 3:
                            Difficulty_String = "Super Simple";
                            break;
                        case 4:
                            Difficulty_String = "Simple";
                            break;
                        case 5:
                            Difficulty_String = "Not so Simple";
                            break;
                        case 6:
                            Difficulty_String = "Played Before";
                            break;
                        case 7:
                            Difficulty_String = "Novice";
                            break;
                        case 8:
                            Difficulty_String = "Amateur";
                            break;
                        case 9:
                            Difficulty_String = "Casual";
                            break;
                        case 10:
                            Difficulty_String = "Regular";
                            break;
                        case 11:
                            Difficulty_String = "Medium";
                            break;
                        case 12:
                            Difficulty_String = "Getting Difficult";
                            break;
                        case 13:
                            Difficulty_String = "Almost Difficult";
                            break;
                        case 14:
                            Difficulty_String = "Difficult";
                            break;
                        case 15:
                            Difficulty_String = "Quite Difficult";
                            break;
                        case 16:
                            Difficulty_String = "Almost Hard";
                            break;
                        case 17:
                            Difficulty_String = "Hard";
                            break;
                        case 18:
                            Difficulty_String = "Very Hard";
                            break;
                        case 19:
                            Difficulty_String = "Very Very Hard";
                            break;
                        case 20:
                            Difficulty_String = "Impossible";
                            break;
                    }
                    Difficulty_Label.Content = Difficulty_String;
                }
            }
        }

    }
}