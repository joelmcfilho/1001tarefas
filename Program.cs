using System;
using System.Drawing;
using _1001tarefas.Models;
using _1001tarefas.Repository;
using _1001tarefas.Sources;

// An Array containing all options in the Main Menu
string[] menuOptions = {"1. Show Tasks","2. New Tasks","3. Manage Tasks","4. Exit Program"};

// This will carry the position of the cursor
int choiceIndex = 0;

TaskRepository taskRepository = new();
Utils utils = new();

// Main Menu Loop!
while(true)
{
    taskRepository.EnsureJsonExists();
    taskRepository.GetTasks();
    Console.BackgroundColor = ConsoleColor.DarkCyan;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.WriteLine("                       -------** 1001 Tasks **-------");
    Console.WriteLine("To select an Option, move the Cursor with UP and DOWN arrows and press ENTER.\n");

    for(int i = 0; i < menuOptions.Length; i++)
    {
        // choiceIndex communicates with the CursorMove object. If the choiceIndex value equals to the i position
        // in menuOptions[i] element, will get a different set of Background and foreground colors.
        if(choiceIndex == i)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(">");
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ");
        }
        Console.WriteLine(menuOptions[i]);
    }
    
    // MoveCursor will read keys pressed in the keyboard
    ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
    
    // Now with MoveCursor instanced, it will watch for the up and down arrows, and for 
    // the ENTER key too. When UP or DOWN are pressed, it will change the value of choiceIndex, that
    // will be needed in the Main Menu option selection mechanic.
    if(MoveCursor.Key == ConsoleKey.UpArrow)
    {
        if(choiceIndex == 0)
        {
            choiceIndex = menuOptions.Length - 1;
        }
        else if(choiceIndex > 0)
        {
            choiceIndex--;
        }
    }
    else if(MoveCursor.Key == ConsoleKey.DownArrow)
    {
        if(choiceIndex == menuOptions.Length - 1)
        {
            choiceIndex = 0;
        }
        else
        {
            choiceIndex++;
        }
    }
    
    if(MoveCursor.Key == ConsoleKey.Enter)
    {
        switch(choiceIndex)
        {
            case 0:

                Console.Clear();
                utils.ShowAllTasks();
                break;
                
            case 1:
                Console.Clear();
                utils.NewTaskInput();
                break;

            case 2:
                Console.Clear();
                Console.WriteLine($"Opção{choiceIndex}");
                break;
            case 3:
                Console.Clear();
                Console.WriteLine("Thanks for using 1001 Tasks! Exiting Program...press any key to continue.");
                Environment.Exit(0);
                break;
                
        }
    }
    
        
}

