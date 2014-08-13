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

namespace Chess.Screens.Dialogs
{
    /// <summary>
    /// Interaction logic for PawnMowerDialog.xaml
    /// </summary>
    public partial class PawnMowerDialog : UserControl
    {
        PawnMowerScreen parentScreen;

        public PawnMowerDialog(PawnMowerScreen parentScreen)
        {
            this.parentScreen = parentScreen;

            InitializeComponent();

            Count_Slider.Value = 4;
            Count_Label.Content = Count_Slider.Value + " Pieces to Capture.";
        }

        private void Count_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Count_Slider != null)
            {
                Count_Slider.Value = (int)Count_Slider.Value;
                parentScreen.SetCount((int)Count_Slider.Value);
                if (Count_Label != null)
                {
                    Count_Label.Content = Count_Slider.Value + " Pieces to Capture.";
                }
            }
        }

        private void New_Challenge_Button_Click(object sender, RoutedEventArgs e)
        {
            parentScreen.NewChallenge();
        }
    }
}
