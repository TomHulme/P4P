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
    /// Interaction logic for GameInfoDialog.xaml
    /// </summary>
    public partial class GameInfoDialog : UserControl
    {
        GameController gameController;

        public GameInfoDialog(GameController gameController)
        {
            InitializeComponent();

            this.gameController = gameController;

            this.gameController.RaiseControllerEvent +=new EventHandler<ControllerEvent>(gameController_MoveText);
        }

        void gameController_MoveText(object sender, ControllerEvent e)
        {
            if (gameController.position.whiteMove)
            {
                SetGameInfoText("White To Move");
            }
            else
            {
                SetGameInfoText("Black To Move");
            }
        }

        internal void SetGameInfoText(String text)
        {
            GameInfoText.Text = text;
        }


    }
}
