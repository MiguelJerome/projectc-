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

class GameInput
{
    /// <summary>
    /// Ceci est le constructeur pour creer un object de GameInput
    /// </summary>
    public GameInput()
    {

    }

    /// <summary>
    /// ceci est la methode qui demontrer les choix de menu pour le menu Game Title a  l ecran
    /// </summary>

    public void ShowMenuTitleLabelChoice()
    {
        StringMenuInputChoice = "PRESS [ENTER] TO GO IN MAIN MENU                  PRESS [ESCAPE] TO QUIT";
        Console.WriteLine(StringMenuInputChoice);
    }
    /// <summary>
    /// ceci est la methode qui demontrer les choix de menu pour le menu Game Main a  l ecran
    /// </summary>

    public void ShowMenuMainLabelChoice()
    {
        StringMenuInputChoice = "PRESS [ENTER] TO START THE GAME                 PRESS [ESCAPE] TO QUIT";
        Console.WriteLine(StringMenuInputChoice);
    }

    /// <summary>
    /// ceci est la methode qui demontrer les choix de menu pour le menu Game Restart or Quit a  l ecran
    /// </summary>
    public void ShowMenuRestartOrQuitLabelChoice()
    {
        StringMenuInputChoice = "PRESS [SPACE] TO RESTART                   PRESS [ESCAPE] TO QUIT";
        Console.WriteLine(StringMenuInputChoice);
    }

    /// <summary>
    /// ceci est la methode qui demontrer les choix de menu pour le menu Game Quit a  l ecran
    /// </summary>

    public void ShowMenuQuitLabelChoice()
    {
        StringMenuInputChoice = "PRESS [ESCAPE] TO QUIT";
        Console.WriteLine(StringMenuInputChoice);
    }

    /// <summary>
    /// ceci est le setter et getter pour la variable _stringMenuInputChoice, qui a la string pour les choix de menu
    /// </summary>

    public String StringMenuInputChoice
    {
        get { return this._stringMenuInputChoice; } // read
        set
        {
            this._stringMenuInputChoice = value;
        }
    }


    private String _stringMenuInputChoice;
}
