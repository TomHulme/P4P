using System.Windows.Media;
using EngineLogic;
namespace Chess.Properties {
    
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings {

		/**
		 * SETTINGS. 
		 */
        int difficultySetting = 1;
        bool woodTextures = false;
        Brush attackedPieces;
        Brush defendedPieces;
        Brush highlightMove;
        Brush previousMove;
        Brush takablePieces;
        Brush suggestedMove;
        bool useObjectRecognition = false;
        SFEngine chessEngine;
        
        public Settings() {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Add code to handle the SettingChangingEvent event here.
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
        }

		/**
		 ** GETTERS AND SETTERS FOR THE SETTINGS
		 **/
        internal int DifficultySetting{
            get{ return difficultySetting;}
            set{ difficultySetting = value;}
        }

        internal bool WoodTextures
        {
            get { return woodTextures; }
            set { woodTextures = value; }
        }

        internal Brush AttackedPieces
        {
            get { return attackedPieces; }
            set { attackedPieces = value; }
        }

        internal Brush DefendedPieces
        {
            get { return defendedPieces; }
            set { defendedPieces = value; }
        }

        internal Brush HighlightMove
        {
            get { return highlightMove; }
            set { highlightMove = value; }
        }

        internal Brush TakablePieces
        {
            get { return takablePieces; }
            set { takablePieces = value; }
        }

        internal Brush PreviousMove
        {
            get { return previousMove; }
            set { previousMove = value; }
        }

        internal Brush SuggestedMove
        {
            get { return suggestedMove; }
            set { suggestedMove = value; }
        }

        internal bool UseObjectRecognition
        {
            get { return useObjectRecognition; }
            set { useObjectRecognition = value; }
        }

        internal SFEngine ChessEngine
        {
            get { return chessEngine; }
            set { chessEngine = value; }
        }
    }
}
