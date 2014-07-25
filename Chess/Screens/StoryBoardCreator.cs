using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


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

        public static Storyboard FadeInFadeOutSquare(Square square, Brush colour, int delay)
        {
            int beginFadeIn = (delay - 1) * 1000;
            int beginFadeOut = beginFadeIn + 500;

            SolidColorBrush originalBackground = square.Background as SolidColorBrush;
            SolidColorBrush highLightColour = colour as SolidColorBrush;

            square.Background = highLightColour;
            square.Opacity = 0;

            DoubleAnimation fadeIn = new DoubleAnimation();
            fadeIn.From = 0;
            fadeIn.To = 1;
            fadeIn.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            fadeIn.BeginTime = TimeSpan.FromMilliseconds(beginFadeIn);
            
            DoubleAnimation fadeOut = new DoubleAnimation();
            fadeOut.From = 1;
            fadeOut.To = 0;
            fadeIn.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            fadeIn.BeginTime = TimeSpan.FromMilliseconds(beginFadeOut);

            //ColorAnimation fadeIn = new ColorAnimation();
            //fadeIn.From = originalBackground.Color;
            //fadeIn.To = highLightColour.Color;
            //fadeIn.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            //fadeIn.BeginTime = TimeSpan.FromMilliseconds(0);

            //ColorAnimation fadeOut = new ColorAnimation();
            //fadeOut.From = highLightColour.Color;
            //fadeOut.To = originalBackground.Color;
            //fadeOut.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            //fadeOut.BeginTime = TimeSpan.FromMilliseconds(500);

            //Storyboard.SetTargetName(fadeIn, square.getName());
            //Storyboard.SetTargetName(fadeOut, square.getName());
            //Storyboard.SetTargetProperty(fadeIn, new PropertyPath(Canvas.BackgroundProperty));
            //Storyboard.SetTargetProperty(fadeOut, new PropertyPath(Canvas.BackgroundProperty));
            Storyboard.SetTarget(fadeIn, square);
            Storyboard.SetTarget(fadeOut, square);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(Canvas.OpacityProperty));
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(Canvas.OpacityProperty));

            Storyboard fadeInOut = new Storyboard();
            fadeInOut.Children.Add(fadeIn);
            fadeInOut.Children.Add(fadeOut);

            return fadeInOut;
        }


        //another way this could work is assinging our own brush to the background property
        //then adding in an animation that would animate the colour of that assigned brush
        //that should technically work

        //also need to add in the input to allow staggering to occur
        //should just be a 1 to 8 thing as i
        //then the begin time equals i - 1

        public static Storyboard PulseSquareBackground(Square square, int delay)
        {
            int beginFadeIn = (delay - 1) * 1000;
            int beginFadeOut = beginFadeIn + 500;

            SolidColorBrush animatedBrush = square.Background as SolidColorBrush;
            square.Background = animatedBrush;
            
            SolidColorBrush originalColour = square.Background as SolidColorBrush;
            SolidColorBrush highlightColour = Brushes.Crimson as SolidColorBrush;

            ColorAnimation fadeIn = new ColorAnimation();
            fadeIn.From = originalColour.Color;
            fadeIn.To = highlightColour.Color;
            fadeIn.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            fadeIn.BeginTime = TimeSpan.FromMilliseconds(beginFadeIn);

            ColorAnimation fadeOut = new ColorAnimation();
            fadeOut.From = highlightColour.Color;
            fadeOut.To = originalColour.Color;
            fadeOut.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            fadeOut.BeginTime = TimeSpan.FromMilliseconds(beginFadeOut);

            Storyboard.SetTarget(fadeIn, animatedBrush);
            Storyboard.SetTarget(fadeOut, animatedBrush);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(SolidColorBrush.ColorProperty));
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(SolidColorBrush.ColorProperty));

            Storyboard fadeInOut = new Storyboard();
            fadeInOut.Children.Add(fadeIn);
            fadeInOut.Children.Add(fadeOut);

            return fadeInOut;
        }

    }
}
