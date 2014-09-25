using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace Chess.Screens
{
    class StoryBoardCreator
    {
        /**
         * This class is intended to hold templates for any animations that need
         * to be created.
         * This was first created to allow easy creation of storyboards to be able
         * to highlight the board to show the ranks and files of the board seperately.
         */

        /// <summary>
        /// Highlights a square with a given colour. The square flashes this given colour
        /// </summary>
        public static Storyboard NewHighlighter(Square s, Brush brush, int delay)
        {
            int beginFadeIn = 0;
            int beginFadeOut = beginFadeIn + 300;

            Duration duration = new Duration(TimeSpan.FromMilliseconds(200));

            ColorAnimation fadeIn = new ColorAnimation()
            {
                From = ((SolidColorBrush)(s.rectangle.Fill)).Color,
                To = (brush as SolidColorBrush).Color,
                Duration = duration,
                BeginTime = TimeSpan.FromMilliseconds(beginFadeIn)
            };

            ColorAnimation fadeOut = new ColorAnimation()
            {
                From = (brush as SolidColorBrush).Color,
                To = (s.rectangle.Fill as SolidColorBrush).Color,
                Duration = duration,
                BeginTime = TimeSpan.FromMilliseconds(beginFadeOut)
            };

            Storyboard.SetTarget(fadeIn, s.rectangle);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Fill.Color"));
            Storyboard.SetTarget(fadeOut, s.rectangle);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Fill.Color"));

            Storyboard highlight = new Storyboard();
            highlight.Children.Add(fadeIn);
            highlight.Children.Add(fadeOut);

            return highlight;
        }
    }
}
