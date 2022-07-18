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


class GameEngine
{
    /// <summary>
    /// Ceci est le constructeur pour creer un object de GameEngine
    /// </summary>
    public GameEngine()
    {
        this._gameState = GameInitialization;
        ScreenWidth = 200;
        ScreenHeight = 40;
    }

    /// <summary>
    /// ceci est la methode qui commence le game Engine
    /// </summary>
    public void StartGameEngine()
    {
        this.StartGameMain();
    }

    /// <summary>
    /// ceci est la methode qui est le moteur du game engine
    /// </summary>
    public void StartGameMain()
    {
        while (this._gameState != _GAME_STOP)
        {
            switch (this._gameState)
            {
                    case _GAME_INITIALIZATION:
                        this.RunInitialization();
                        this.SetGameState(_GAME_TITLE_MENU);
                    break;
                    case _GAME_TITLE_MENU:
                        Console.Clear();
                        this.RunMenuTitle();
                        break;
                    case _GAME_MAIN_MENU:
                        Console.Clear();
                    this.RunMenuMain();
                        break;
                    case _GAME_LOOP:
                        this.RunGameLogic();
                        this.SetGameState(_GAME_RESTART_OR_QUIT_MENU);
                    break;
                case _GAME_RESTART_OR_QUIT_MENU:
                        Console.Clear();
                        this.RestartOrQuitMenu();
                        break;
                    case _GAME_EXIT:
                        Console.Clear();
                        this.RunQuitMenu();
                        this.SetGameState(_GAME_STOP);
                        this.StopGameEngine();
                    break;
                    default: break;
            }
        }
    }

    /// <summary>
    /// ceci est la methode qui change les etats de la game
    /// </summary>
    public void SetGameState(int gameState)
    {
        this._gameState = gameState;
    }

    /// <summary>
    /// ceci est la methode qui fait tous les intialisations necessaires avant le commencencemnt de la game 
    /// </summary>
    public void RunInitialization()
    {
        //setting screen colors and characters
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;

        // initialise Output stuff
        Console.CursorLeft = 0; // cursor position left
        Console.CursorTop = 0;  // cursor position top
        Console.SetWindowSize(ScreenWidth, ScreenHeight); // set the window size
        Console.CursorVisible = false; // cursor is off

    }

    /// <summary>
    /// ceci est la methode qui commence le game Title Menu
    /// </summary>
    public void RunMenuTitle()
    {
        ConsoleKeyInfo cki;

        var sound = new GameSound(); 
        sound.PlayOpeningGameTitleMenu();

        var menu = new GameMenu();
        menu.ShowMenuTitleInfo();

        var input = new GameInput();
        input.ShowMenuTitleLabelChoice();

        var myPlayerGameExit = new System.Media.SoundPlayer(@"pacman_death.wav");
        var myPlayerGameMain = new System.Media.SoundPlayer(@"pacman_eatfruit.wav");

        // check input key press by the user 
        do
        {
            cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.Escape)
            {
                myPlayerGameExit.Play();
                Thread.Sleep(1000);

                this.SetGameState(_GAME_EXIT);
            }
            else if (cki.Key == ConsoleKey.Enter)
            {
                myPlayerGameMain.Play();
                Thread.Sleep(1000);
                this.SetGameState(_GAME_MAIN_MENU);
            }
            // press the proper Escape or Enter to get out this loop          
        } while (cki.Key != ConsoleKey.Escape && cki.Key != ConsoleKey.Enter);
    }

    /// <summary>
    /// ceci est la methode qui commence le game Main Menu
    /// </summary>
    public void RunMenuMain()
    {
        ConsoleKeyInfo cki;
        
        var sound = new GameSound();
        sound.PlayOpeningGameMainMenu();

        var menu = new GameMenu();
        menu.ShowMenuMainInfo();

        var input = new GameInput();
        input.ShowMenuMainLabelChoice();

        var myPlayerGameExit = new System.Media.SoundPlayer(@"pacman_death.wav");
        var myPlayerGameLoop = new System.Media.SoundPlayer(@"pacman_eatfruit.wav");

        // check input key press by the user 
        do
        {

            cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.Escape)
            {
                myPlayerGameExit.Play();
                Thread.Sleep(1000);
                this.SetGameState(_GAME_EXIT);
            }
            else if (cki.Key == ConsoleKey.Enter)
            {
                myPlayerGameLoop.Play();
                Thread.Sleep(1000);
                this.SetGameState(_GAME_LOOP);
            }
            // press the proper Escape or Enter to get out this loop          
        } while (cki.Key != ConsoleKey.Escape && cki.Key != ConsoleKey.Enter);

    }

    /// <summary>
    /// ceci est la methode qui commence le game Logic
    /// </summary>
    public void RunGameLogic()
    {
        //setting screen colors and characters
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;

        // initialise Output stuff
        Console.CursorLeft = 0; // cursor position left
        Console.CursorTop = 0;  // cursor position top
        
        Console.CursorVisible = false; // cursor is off
        const int refreshScreen = 15;

        // getting input stuff
        ConsoleKeyInfo cki;

        // initialize starting pacman position
        const int STARTING_PACMAN_X = 1;
        const int STARTING_PACMAN_Y = 1;
        int xPacmanPosition = STARTING_PACMAN_X;
        int yPacmanPosition = STARTING_PACMAN_Y;

        // Pacman Graphic with movement
        String[] stringPacmanGraphic = new String[4];
        stringPacmanGraphic[0] = "-";
        stringPacmanGraphic[1] = "/";
        stringPacmanGraphic[2] = "|";
        stringPacmanGraphic[3] = "\\";
        int counterPacmanAnimationFrame = 0;

        // set map boudaries
        const int RIGHT_SIDE_ROAD_BOUNDARY = 24;
        const int LEFT_SIDE_ROAD_BOUNDARY = 1;
        const int TOP_SIDE_ROAD_BOUNDARY = 1;
        const int BOTTOM_SIDE_ROAD_BOUNDARY = 4;

        // set map size
        const int LEVEL_NUMBER_COLUMNS = 27;
        const int LEVEL_NUMBER_ROWS = 7;


        // ressources level Graphics
        const String WALL = "*";
        const String ROAD = " ";

        // MAP DEAD ZONE
        const String DEAD_ZONE = " ";

        // the Graphic of the whole map
        String[,] map = new String[LEVEL_NUMBER_ROWS, LEVEL_NUMBER_COLUMNS];

        map[0, 0] = WALL;
        map[0, 1] = WALL;
        map[0, 2] = WALL;
        map[0, 3] = WALL;
        map[0, 4] = WALL;
        map[0, 5] = WALL;
        map[0, 6] = WALL;
        map[0, 7] = WALL;
        map[0, 8] = WALL;
        map[0, 9] = WALL;
        map[0, 10] = WALL;
        map[0, 11] = WALL;
        map[0, 12] = WALL;
        map[0, 13] = WALL;
        map[0, 14] = WALL;
        map[0, 15] = WALL;
        map[0, 16] = WALL;
        map[0, 17] = WALL;
        map[0, 18] = WALL;
        map[0, 19] = WALL;
        map[0, 20] = WALL;
        map[0, 21] = WALL;
        map[0, 22] = WALL;
        map[0, 23] = WALL;
        map[0, 24] = WALL;
        map[0, 25] = WALL;
        map[0, 26] = WALL;
        map[1, 0] = WALL;
        map[1, 1] = ROAD;
        map[1, 2] = ROAD;
        map[1, 3] = ROAD;
        map[1, 4] = ROAD;
        map[1, 5] = ROAD;
        map[1, 6] = ROAD;
        map[1, 7] = ROAD;
        map[1, 8] = ROAD;
        map[1, 9] = ROAD;
        map[1, 10] = ROAD;
        map[1, 11] = ROAD;
        map[1, 12] = ROAD;
        map[1, 13] = ROAD;
        map[1, 14] = ROAD;
        map[1, 15] = ROAD;
        map[1, 16] = ROAD;
        map[1, 17] = ROAD;
        map[1, 18] = ROAD;
        map[1, 19] = ROAD;
        map[1, 20] = ROAD;
        map[1, 21] = ROAD;
        map[1, 22] = ROAD;
        map[1, 23] = ROAD;
        map[1, 24] = ROAD;
        map[1, 25] = ROAD;
        map[1, 26] = WALL;
        map[2, 0] = WALL;
        map[2, 1] = ROAD;
        map[2, 2] = WALL;
        map[2, 3] = WALL;
        map[2, 4] = WALL;
        map[2, 5] = WALL;
        map[2, 6] = WALL;
        map[2, 7] = WALL;
        map[2, 8] = WALL;
        map[2, 9] = ROAD;
        map[2, 10] = WALL;
        map[2, 11] = WALL;
        map[2, 12] = WALL;
        map[2, 13] = WALL;
        map[2, 14] = WALL;
        map[2, 15] = WALL;
        map[2, 16] = WALL;
        map[2, 17] = WALL;
        map[2, 18] = WALL;
        map[2, 19] = ROAD;
        map[2, 20] = WALL;
        map[2, 21] = WALL;
        map[2, 22] = WALL;
        map[2, 23] = WALL;
        map[2, 24] = WALL;
        map[2, 25] = ROAD;
        map[2, 26] = WALL;
        map[3, 0] = WALL;
        map[3, 1] = ROAD;
        map[3, 2] = WALL;
        map[3, 3] = DEAD_ZONE;
        map[3, 4] = DEAD_ZONE;
        map[3, 5] = DEAD_ZONE;
        map[3, 6] = DEAD_ZONE;
        map[3, 7] = DEAD_ZONE;
        map[3, 8] = WALL;
        map[3, 9] = ROAD;
        map[3, 10] = WALL;
        map[3, 11] = DEAD_ZONE;
        map[3, 12] = DEAD_ZONE;
        map[3, 13] = DEAD_ZONE;
        map[3, 14] = DEAD_ZONE;
        map[3, 15] = DEAD_ZONE;
        map[3, 16] = DEAD_ZONE;
        map[3, 17] = DEAD_ZONE;
        map[3, 18] = WALL;
        map[3, 19] = ROAD;
        map[3, 20] = WALL;
        map[3, 21] = DEAD_ZONE;
        map[3, 22] = DEAD_ZONE;
        map[3, 23] = DEAD_ZONE;
        map[3, 24] = WALL;
        map[3, 25] = ROAD;
        map[3, 26] = WALL;
        map[4, 0] = WALL;
        map[4, 1] = ROAD;
        map[4, 2] = WALL;
        map[4, 3] = WALL;
        map[4, 4] = WALL;
        map[4, 5] = WALL;
        map[4, 6] = WALL;
        map[4, 7] = WALL;
        map[4, 8] = WALL;
        map[4, 9] = ROAD;
        map[4, 10] = WALL;
        map[4, 11] = WALL;
        map[4, 12] = WALL;
        map[4, 13] = WALL;
        map[4, 14] = WALL;
        map[4, 15] = WALL;
        map[4, 16] = WALL;
        map[4, 17] = WALL;
        map[4, 18] = WALL;
        map[4, 19] = ROAD;
        map[4, 20] = WALL;
        map[4, 21] = WALL;
        map[4, 22] = WALL;
        map[4, 23] = WALL;
        map[4, 24] = WALL;
        map[4, 25] = ROAD;
        map[4, 26] = WALL;
        map[5, 0] = WALL;
        map[5, 1] = ROAD;
        map[5, 2] = ROAD;
        map[5, 3] = ROAD;
        map[5, 4] = ROAD;
        map[5, 5] = ROAD;
        map[5, 6] = ROAD;
        map[5, 7] = ROAD;
        map[5, 8] = ROAD;
        map[5, 9] = ROAD;
        map[5, 10] = ROAD;
        map[5, 11] = ROAD;
        map[5, 12] = ROAD;
        map[5, 13] = ROAD;
        map[5, 14] = ROAD;
        map[5, 15] = ROAD;
        map[5, 16] = ROAD;
        map[5, 17] = ROAD;
        map[5, 18] = ROAD;
        map[5, 19] = ROAD;
        map[5, 20] = ROAD;
        map[5, 21] = ROAD;
        map[5, 22] = ROAD;
        map[5, 23] = ROAD;
        map[5, 24] = ROAD;
        map[5, 25] = ROAD;
        map[5, 26] = WALL;
        map[6, 0] = WALL;
        map[6, 1] = WALL;
        map[6, 2] = WALL;
        map[6, 3] = WALL;
        map[6, 4] = WALL;
        map[6, 5] = WALL;
        map[6, 6] = WALL;
        map[6, 7] = WALL;
        map[6, 8] = WALL;
        map[6, 9] = WALL;
        map[6, 10] = WALL;
        map[6, 11] = WALL;
        map[6, 12] = WALL;
        map[6, 13] = WALL;
        map[6, 14] = WALL;
        map[6, 15] = WALL;
        map[6, 16] = WALL;
        map[6, 17] = WALL;
        map[6, 18] = WALL;
        map[6, 19] = WALL;
        map[6, 20] = WALL;
        map[6, 21] = WALL;
        map[6, 22] = WALL;
        map[6, 23] = WALL;
        map[6, 24] = WALL;
        map[6, 25] = WALL;
        map[6, 26] = WALL;

        void ClearRoad()
        {
            const String ROAD_GRAPHIC = " ";

            map[1, 1] = ROAD_GRAPHIC;
            map[1, 2] = ROAD_GRAPHIC;
            map[1, 3] = ROAD_GRAPHIC;
            map[1, 4] = ROAD_GRAPHIC;
            map[1, 5] = ROAD_GRAPHIC;
            map[1, 6] = ROAD_GRAPHIC;
            map[1, 7] = ROAD_GRAPHIC;
            map[1, 8] = ROAD_GRAPHIC;
            map[1, 9] = ROAD_GRAPHIC;
            map[1, 10] = ROAD_GRAPHIC;
            map[1, 11] = ROAD_GRAPHIC;
            map[1, 12] = ROAD_GRAPHIC;
            map[1, 13] = ROAD_GRAPHIC;
            map[1, 14] = ROAD_GRAPHIC;
            map[1, 15] = ROAD_GRAPHIC;
            map[1, 16] = ROAD_GRAPHIC;
            map[1, 17] = ROAD_GRAPHIC;
            map[1, 18] = ROAD_GRAPHIC;
            map[1, 19] = ROAD_GRAPHIC;
            map[1, 20] = ROAD_GRAPHIC;
            map[1, 21] = ROAD_GRAPHIC;
            map[1, 22] = ROAD_GRAPHIC;
            map[1, 23] = ROAD_GRAPHIC;
            map[1, 24] = ROAD_GRAPHIC;
            map[1, 25] = ROAD_GRAPHIC;
            map[2, 1] = ROAD_GRAPHIC;
            map[2, 9] = ROAD_GRAPHIC;
            map[2, 19] = ROAD_GRAPHIC;
            map[2, 25] = ROAD_GRAPHIC;
            map[3, 1] = ROAD_GRAPHIC;
            map[3, 9] = ROAD_GRAPHIC;
            map[3, 19] = ROAD_GRAPHIC;
            map[3, 25] = ROAD_GRAPHIC;
            map[4, 1] = ROAD_GRAPHIC;
            map[4, 9] = ROAD_GRAPHIC;
            map[4, 19] = ROAD_GRAPHIC;
            map[4, 25] = ROAD_GRAPHIC;
            map[5, 1] = ROAD_GRAPHIC;
            map[5, 2] = ROAD_GRAPHIC;
            map[5, 3] = ROAD_GRAPHIC;
            map[5, 4] = ROAD_GRAPHIC;
            map[5, 5] = ROAD_GRAPHIC;
            map[5, 6] = ROAD_GRAPHIC;
            map[5, 7] = ROAD_GRAPHIC;
            map[5, 8] = ROAD_GRAPHIC;
            map[5, 9] = ROAD_GRAPHIC;
            map[5, 10] = ROAD_GRAPHIC;
            map[5, 11] = ROAD_GRAPHIC;
            map[5, 12] = ROAD_GRAPHIC;
            map[5, 13] = ROAD_GRAPHIC;
            map[5, 14] = ROAD_GRAPHIC;
            map[5, 15] = ROAD_GRAPHIC;
            map[5, 16] = ROAD_GRAPHIC;
            map[5, 17] = ROAD_GRAPHIC;
            map[5, 18] = ROAD_GRAPHIC;
            map[5, 19] = ROAD_GRAPHIC;
            map[5, 20] = ROAD_GRAPHIC;
            map[5, 21] = ROAD_GRAPHIC;
            map[5, 22] = ROAD_GRAPHIC;
            map[5, 23] = ROAD_GRAPHIC;
            map[5, 24] = ROAD_GRAPHIC;
            map[5, 25] = ROAD_GRAPHIC;
        }

        void RenderScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int mx = 0; mx < LEVEL_NUMBER_ROWS; mx++)
            {
                if (mx != 0)
                {
                    Console.CursorLeft = Console.CursorLeft;
                    Console.WriteLine();
                }

                for (int my = 0; my < LEVEL_NUMBER_COLUMNS; my++)
                {
                    Console.CursorLeft = Console.CursorLeft;
                    Console.Write($"{map[mx, my]}");
                }
            }
            Console.SetCursorPosition(0, 0);
        }


        void CheckIfPacmanOnTheRoad(int xPacmanPosition, int yPacmanPosition)
        {
            switch (yPacmanPosition)
            {
                case 1:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            map[yPacmanPosition, xPacmanPosition] = stringPacmanGraphic[counterPacmanAnimationFrame];
                            break;
                        default: break;
                    }
                    break;
                case 2:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            map[yPacmanPosition, xPacmanPosition] = stringPacmanGraphic[counterPacmanAnimationFrame];
                            break;
                        default: break;
                    }

                    break;
                case 3:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            map[yPacmanPosition, xPacmanPosition] = stringPacmanGraphic[counterPacmanAnimationFrame];
                            break;
                        default: break;
                    }
                    break;
                case 4:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            map[yPacmanPosition, xPacmanPosition] = stringPacmanGraphic[counterPacmanAnimationFrame];
                            break;
                        default: break;
                    }
                    break;
                case 5:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            map[yPacmanPosition, xPacmanPosition] = stringPacmanGraphic[counterPacmanAnimationFrame];
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        void ChangePacmanCoordoneeForUp(int xPacmanPosition, int yPacmanPosition)
        {
            switch (yPacmanPosition)
            {
                case 1:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanUp();
                            break;
                        default: break;
                    }

                    break;
                case 2:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanUp();
                            break;
                        default: break;
                    }

                    break;
                case 3:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanUp();
                            break;
                        default: break;
                    }
                    break;
                case 4:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanUp();
                            break;
                        default: break;
                    }
                    break;
                case 5:

                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanUp();
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        void ChangePacmanCoordoneeForDown(int xPacmanPosition, int yPacmanPosition)
        {
            switch (yPacmanPosition)
            {
                case 1:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanDown();
                            break;
                        default: break;
                    }

                    break;
                case 2:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanDown();
                            break;
                        default: break;
                    }

                    break;
                case 3:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanDown();
                            break;
                        default: break;
                    }
                    break;
                case 4:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanDown();
                            break;
                        default: break;
                    }
                    break;
                case 5:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanDown();
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        void ChangePacmanCoordoneeForLeft(int xPacmanPosition, int yPacmanPosition)
        {
            switch (yPacmanPosition)
            {
                case 1:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanLeft();
                            break;
                        default: break;
                    }

                    break;
                case 2:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanLeft();
                            break;
                        default: break;
                    }
                    break;
                case 3:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanLeft();
                            break;
                        default: break;
                    }
                    break;
                case 4:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanLeft();
                            break;
                        default: break;
                    }
                    break;
                case 5:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanLeft();
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        void ChangePacmanCoordoneeForRight(int xPacmanPosition, int yPacmanPosition)
        {
            switch (yPacmanPosition)
            {
                case 1:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanRight();
                            break;
                        default: break;
                    }

                    break;
                case 2:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanRight();
                            break;
                        default: break;
                    }

                    break;
                case 3:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanRight();
                            break;
                        default: break;
                    }
                    break;
                case 4:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 9:
                        case 19:
                        case 25:
                            MovePacmanRight();
                            break;
                        default: break;
                    }
                    break;
                case 5:
                    switch (xPacmanPosition)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                            MovePacmanRight();
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        void MovePacmanUp()
        {
            ClearRoad();
            CheckIfPacmanOnTheRoad(xPacmanPosition, --yPacmanPosition);
            RenderScreen();
            Thread.Sleep(refreshScreen);
            counterPacmanAnimationFrame++;
        }

        void MovePacmanDown()
        {
            ClearRoad();
            CheckIfPacmanOnTheRoad(xPacmanPosition, ++yPacmanPosition);
            RenderScreen();
            Thread.Sleep(refreshScreen);
            counterPacmanAnimationFrame++;
        }

        void MovePacmanLeft()
        {
            ClearRoad();
            CheckIfPacmanOnTheRoad(--xPacmanPosition, yPacmanPosition);
            RenderScreen();
            Thread.Sleep(refreshScreen);
            counterPacmanAnimationFrame++;
        }

        void MovePacmanRight()
        {
            ClearRoad();
            CheckIfPacmanOnTheRoad(++xPacmanPosition, yPacmanPosition);
            RenderScreen();
            Thread.Sleep(refreshScreen);
            counterPacmanAnimationFrame++;
        }


        void DisplayPacmanOnHisStartingPosition()
        {
            Console.SetCursorPosition(0, 0);
            ClearRoad();
            map[xPacmanPosition, yPacmanPosition] = stringPacmanGraphic[counterPacmanAnimationFrame];
            RenderScreen();
            Thread.Sleep(refreshScreen);
        }
        
        DisplayPacmanOnHisStartingPosition();

        var myPlayer = new System.Media.SoundPlayer(@"pacman_chomp.wav");
        // check input key press by the user 
        do
        {
            while (Console.KeyAvailable == false)
                Thread.Sleep(refreshScreen);
            cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.UpArrow)
            {
                if (yPacmanPosition > TOP_SIDE_ROAD_BOUNDARY)
                {
                    myPlayer.Play();
                    int yTempPacmanPosition = yPacmanPosition;
                    ChangePacmanCoordoneeForUp(xPacmanPosition, --yTempPacmanPosition);
                }
            }
            else if (cki.Key == ConsoleKey.DownArrow)
            {
                if (yPacmanPosition <= BOTTOM_SIDE_ROAD_BOUNDARY)
                {
                    myPlayer.Play();
                    int yTempPacmanPosition = yPacmanPosition;
                    ChangePacmanCoordoneeForDown(xPacmanPosition, ++yTempPacmanPosition);
                }
            }
            else if (cki.Key == ConsoleKey.LeftArrow)
            {
                if (xPacmanPosition > LEFT_SIDE_ROAD_BOUNDARY)
                {
                    myPlayer.Play();
                    int xTempPacmanPosition = xPacmanPosition;
                    ChangePacmanCoordoneeForLeft(--xTempPacmanPosition, yPacmanPosition);
                }
            }
            else if (cki.Key == ConsoleKey.RightArrow)
            {
                if (xPacmanPosition <= RIGHT_SIDE_ROAD_BOUNDARY)
                {
                    myPlayer.Play();
                    int xTempPacmanPosition = xPacmanPosition;
                    ChangePacmanCoordoneeForRight(++xTempPacmanPosition, yPacmanPosition);
                }
            }
            if (counterPacmanAnimationFrame == 4)
                counterPacmanAnimationFrame = 0;

            

        } while (cki.Key != ConsoleKey.Escape);  // press key logic of the game

    }

    /// <summary>
    /// ceci est la methode qui commence le game Restart or Quit Menu
    /// </summary>
    public void RestartOrQuitMenu()
    {
        ConsoleKeyInfo cki;

        var menu = new GameMenu();
        menu.ShowMenuRestartOrQuitInfo();

        var input = new GameInput();
        input.ShowMenuRestartOrQuitLabelChoice();

        var myPlayerGameExit = new System.Media.SoundPlayer(@"pacman_death.wav");
        var myPlayerGameRestart = new System.Media.SoundPlayer(@"pacman_eatghost.wav");

        // check input key press by the user 
        do
        {
            
            cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.Escape)
            {
                myPlayerGameExit.Play();
                Thread.Sleep(1000);
                this.SetGameState(_GAME_EXIT);
            }
            else if (cki.Key == ConsoleKey.Spacebar)
            {
                myPlayerGameRestart.Play();
                Thread.Sleep(1000);
                this.SetGameState(_GAME_LOOP);
            }
            // press the proper Escape or Spacebar to get out this loop          
        } while (cki.Key != ConsoleKey.Escape && cki.Key != ConsoleKey.Spacebar);  

    }

    /// <summary>
    /// ceci est la methode qui commence le game Quit Menu
    /// </summary>
    public void RunQuitMenu()
    {
        ConsoleKeyInfo cki;
       
        var menu = new GameMenu();
        menu.ShowMenuQuitInfo();

        var input = new GameInput();
        input.ShowMenuQuitLabelChoice();

        var myPlayerGameLevel = new System.Media.SoundPlayer(@"Intermission.wav");
        myPlayerGameLevel.Play();
        
        do
        {
            cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.Escape)
            {
                myPlayerGameLevel.Stop();
            }
        } while (cki.Key != ConsoleKey.Escape);  // press key logic of the game
    }


    /// <summary>
    /// ceci est la methode qui commence le game Stop Engine
    /// </summary>
    public void StopGameEngine()
    {
        Console.Clear();
        Console.WriteLine($"Merci d avoir jouer au jeu de PACMAN");
        Console.WriteLine($"Bye");
    }


    /// <summary>
    /// ceci est le enum pour les differents etats de la game
    /// </summary>
    public enum GameMode
    {
        Initialization = 1, TitleMenu = 2, MainMenu = 3, GameLoop = 4, RestartOrQuitMenu = 5 , GameExit = 6, GameStop = 7 
    }

    /// <summary>
    /// ceci est le setter et getter pour la variable _screenWidth, qui a la valeur pour la largeur de l ecran window
    /// </summary>
    public int ScreenWidth
    {
        get { return this._screenWidth; } // read
        set
        {
            _screenWidth = value; // write
        }
    }

    /// <summary>
    /// ceci est le setter et getter pour la variable _screenHeight, qui a la valeur pour la hauteur du l ecran window
    /// </summary>
    public int ScreenHeight
    {
        get { return this._screenHeight; } // read
        set
        {
            _screenHeight = value; // write
        }

    }

    /// <summary>
    /// ceci est le setter et getter pour la variable _gameState, qui a la valeur pour l etat du jeu 
    /// </summary>
    public int GameState
    {
        get { return this._gameState; } // read
        set
        {
            _gameState = value; // write
        }

    }

    /// <summary>
    /// ceci est le getter pour la variable _GAME_INITIALIZATION, qui lit seulement l etat Game Initialisation 
    /// </summary>
    public int GameInitialization
    {
        get { return _GAME_INITIALIZATION; } // read
        private set
        {
            
        }

    }

    /// <summary>
    /// ceci est le getter pour la variable _GAME_TITLE_MENU, qui lit seulement l etat Game Title Menu 
    /// </summary>
    public int GameTitleMenu
    {
        get { return _GAME_TITLE_MENU; } // read
        private set
        {

        }

    }

    /// <summary>
    /// ceci est le getter pour la variable _GAME_MAIN_MENU, qui lit seulement l etat Game Main Menu 
    /// </summary>
    public int GameMainMenu
    {
        get { return _GAME_MAIN_MENU; } // read
        private set
        {

        }
    }

    /// <summary>
    /// ceci est le getter pour la variable _GAME_LOOP, qui lit seulement l etat Game Loop Menu 
    /// </summary>
    public int GameLoop
    {
        get { return _GAME_LOOP; } // read
        private set
        {

        }
    }

    /// <summary>
    /// ceci est le getter pour la variable _GAME_RESTART_OR_QUIT_MENU, qui lit seulement l etat Game Restart or Quit Menu 
    /// </summary>
    public int GameRestartOrQuitMenu
    {
        get { return _GAME_RESTART_OR_QUIT_MENU; } // read
        private set
        {

        }
    }

    /// <summary>
    /// ceci est le getter pour la variable _GAME_EXIT, qui lit seulement l etat Game Exit 
    /// </summary>
    public int GameExit
    {
        get { return _GAME_EXIT; } // read
        private set
        {

        }
    }

    /// <summary>
    /// ceci est le getter pour la variable _GAME_STOP, qui lit seulement l etat Game Stop
    /// </summary>
    public int GameStop
    {
        get { return _GAME_STOP; } // read
        private set
        {

        }
    }

    /// <summary>
    /// ceci est l operateur de surcharge, qui augmente le Game Engine id de 1
    /// </summary>
    public static GameEngine operator ++(GameEngine gameEngine)
    {
        GameEngine gameEngineVersion = new GameEngine();
        gameEngineVersion.IdGameEngine = gameEngine.IdGameEngine;
        return gameEngineVersion;
    }

    /// <summary>
    /// ceci est le setter et getter pour la variable _idGameEngine, qui lit ou incrementer la valeur _idGameEngine de 1
    /// </summary>
    public int IdGameEngine
    {
        get { return _idGameEngine; } // read
        private set
         {
             _idGameEngine = ++value; // write
        }
    }

    private int _screenWidth;
    private int _screenHeight;
    private const int _GAME_INITIALIZATION = (int) GameMode.Initialization;
    private const int _GAME_TITLE_MENU = (int) GameMode.TitleMenu;
    private const int _GAME_MAIN_MENU = (int) GameMode.MainMenu;
    private const int _GAME_LOOP = (int) GameMode.GameLoop;
    private const int _GAME_RESTART_OR_QUIT_MENU = (int) GameMode.RestartOrQuitMenu;
    private const int _GAME_EXIT = (int) GameMode.GameExit;
    private const int _GAME_STOP = (int) GameMode.GameStop;
    private int _gameState;
    private static int _idGameEngine;
}