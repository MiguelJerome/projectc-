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


class GameMenu
    {
        /// <summary>
        /// Ceci est le constructeur pour creer un object de GameMenu
        /// </summary>
    public GameMenu()
        {
            
        }
    /// <summary>
    /// ceci est la methode qui demontrer le logo du jeu en string a  l ecran
    /// </summary>
        public void ShowMenuTitleInfo()
        {
        String StringMenuInfo = "cccccccccccccccccccccccc                            #######           ######\n" +
                                "cccccccccccccccccccccccc                            #######           ######\n" +
                                "cccccc            cccccc                      ####################################\n" +
                                "cccccc            cccccc                      ####################################\n" +
                                "cccccc                    ***  ***  ***  ***        #######           ######\n" +
                                "cccccc                      *  * *    *    *        #######           ######\n" +
                                "cccccc                    ***  * *  ***  ***        #######           ######\n" +
                                "cccccc                    *    * *  *    *          #######           ######\n" +
                                "cccccc                    ***  ***  ***  ***        #######           ######\n" +
                                "cccccc            cccccc                      ####################################\n" +
                                "cccccc            cccccc                      ####################################\n" +
                                "cccccccccccccccccccccccc                            #######           ######\n" +
                                "cccccccccccccccccccccccc                            #######           ######\n";
        Console.WriteLine($"{StringMenuInfo}");
        }

    /// <summary>
    /// ceci est la methode qui demontrer de l information pour mentionner les touches que l utilasateur peut utiliser quand il joue le jeu a  l ecran
    /// </summary>
    public void ShowMenuMainInfo()
        {
            StringMenuInfo = "VOUS POUVER DEPLACER LE PACMAN EN APPUYANT\n\n\n" +
                             "[UP ARROW KEY] pour bouger le PACMAN vers le haut\n" +
                             "[DOWN ARROW KEY] pour bouger le PACMAN vers le bas\n" +
                             "[LEFT ARROW KEY] pour bouger le PACMAN vers la gauche\n" +
                             "[RIGHT ARROW KEY] pour bouger le PACMAN vers la droite\n\n\n" +
                             "[ESCAPE KEY] pour quitter la partie\n";

            Console.WriteLine($"{StringMenuInfo}");
        }

    /// <summary>
    /// ceci est la methode qui demontrer quelle touche pour recommencer le jeu ou quelle touche pour quitter le jeu  l ecran
    /// </summary>

    public void ShowMenuRestartOrQuitInfo()
        {
            StringMenuInfo = "VOUS POUVEZ RECOMMENCER UNE PARTIE EN APPUYANT SUR LA TOUCHE [SPACE]\n\n\n" +
                             "VOUS POUVEZ AUSSI QUITTER LA PARTIE EN APPUYANT [ESCAPE]\n";
            Console.WriteLine($"{StringMenuInfo}");
         }

    /// <summary>
    /// ceci est la methode qui demontrer le game credits l ecran
    /// </summary>
    public void ShowMenuQuitInfo()
        {
            StringMenuInfo = "GAME CREDITS\n\n\n" +
                             "DEVELOPPEUR\n\n" +
                             "MIGUEL JEROME\n" +
                             "SYNN SLOAN IGONDJO\n\n\n\n";
            Console.WriteLine($"{StringMenuInfo}");
        }


    /// <summary>
    /// ceci est le setter et getter pour la variable _stringMenuInfo, qui a la string pour de l info des Menus
    /// </summary>
    public String StringMenuInfo
        {
            get { return this._stringMenuInfo; } // read
            set
            {
                this._stringMenuInfo = value;
            }
        }

        private String _stringMenuInfo;
}

