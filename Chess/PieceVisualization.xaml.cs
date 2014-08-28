using System;
using System.Collections.Generic;
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

namespace Chess
{
    /// <summary>
    /// Interaction logic for PieceVisualization.xaml
    /// </summary>
    public partial class PieceVisualization : TagVisualization
    {
        public PieceVisualization()
        {
            InitializeComponent();
        }

        private void PieceVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize PieceVisualization's UI based on this.VisualizedTag here
        }
    }
}
