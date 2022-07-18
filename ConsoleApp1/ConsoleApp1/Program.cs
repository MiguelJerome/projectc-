// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
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



//Game Engine version 1  
var gameEngine = new GameEngine(); // play engine
++gameEngine; // ceci incremente GameEngine de 1
gameEngine.StartGameEngine();
Console.WriteLine($"id engine 1: {gameEngine.IdGameEngine}");
Thread.Sleep(3000);


/*
//Game Engine version 2
var gameEngine2 = new GameEngine(); // play engine
++gameEngine2;
gameEngine2.StartGameEngine();
Console.WriteLine($"id engine 2: {gameEngine2.IdGameEngine}");
Thread.Sleep(3000);


//Game Engine version 3
var gameEngine3 = new GameEngine(); // play engine
++gameEngine3;
gameEngine3.StartGameEngine();
Console.WriteLine($"id engine 3 : {gameEngine3.IdGameEngine}");
Thread.Sleep(3000);
*/


