using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Chess
{
    public abstract class Screen : UserControl
    {
        public ScreenControl parentWindow;

        public ScreenControl ParentWindow { get { return parentWindow; } }

        protected Screen() { }

        public Screen(ScreenControl parentWindow)
        {
            this.parentWindow = parentWindow;
        }
    }
}
