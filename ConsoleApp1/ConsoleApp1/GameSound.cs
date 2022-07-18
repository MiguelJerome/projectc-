using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Resources;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Media;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


    class GameSound
    {
        /// <summary>
        /// Ceci est le constructeur pour creer un object de GameSound
        /// </summary>
        public GameSound()
        {
            
        }

        /// <summary>
        /// Ceci fait jouer un son en entrant dans le menu de Game Title Menu
        /// </summary>
        public void PlayOpeningGameTitleMenu()
        {
            StringGameSound = @"pacman_beginning.wav";

            var myPlayer = new System.Media.SoundPlayer(StringGameSound);
            myPlayer.Play();
        }

        /// <summary>
        /// Ceci fait jouer un son en entrant dans le menu de Game Main Menu
        /// </summary>
    public void PlayOpeningGameMainMenu()
        {
            StringGameSound = @"pacman_intermission.wav";

            var myPlayer = new System.Media.SoundPlayer(StringGameSound);
            myPlayer.Play();
        }
    /// <summary>
    /// ceci est le setter et getter pour la variable _stringGameSound, qui a la string pour le fichier du son
    /// </summary>
        
    public String StringGameSound
        {
            get { return this._stringGameSound; } // read
            set
            {
                this._stringGameSound = value;
            }
        }


    private String _stringGameSound;

}

