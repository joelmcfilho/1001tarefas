using System;
using System.Drawing;
using _1001tarefas.Models;
using _1001tarefas.Repository;
using _1001tarefas.Sources;

// An Array containing all options in the Main Menu
string[] menuOptions = {"1. Show Tasks","2. Mark an Task as DONE","3. Create New Task","4. Edit an Task","5. Delete Task","6. Exit Program"};

// This will carry the position of the cursor
int choiceIndex = 0;

TaskRepository taskRepository = new();
SubMenus subMenus = new();
Utils utils = new();

// Main Menu Loop!
while(true)
{
    // This methods run every startup...if the JSON don't exists, create a blank new ready one
    taskRepository.EnsureJsonExists();

    // Verify in every startup if any PENDENT task date is early than today's date, then flag it LATE
    utils.AutoLateLabeller();

    // Aesthetics of the CLI...
    Console.BackgroundColor = ConsoleColor.DarkCyan;
    Console.ForegroundColor = ConsoleColor.Black;


    Console.Clear();
    Console.WriteLine($"----------------------** 1001 Tasks **------- Today's date: {DateTime.Today:dd/MM/yy}");
    Console.WriteLine("Created by Joel Menezes. Free to use and change it your way!");
    utils.HeadsUp();
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
        //This leads to every other function in the program. All of them are coded in the Sources folder.
        switch(choiceIndex)
        {
            case 0:
                subMenus.ShowTasksMenu();              
                break;

            case 1:
                utils.MarkAsDone();
                break;
                
            case 2:
                Console.Clear();
                utils.NewTaskInput();
                break;

            case 3:
                Console.Clear();
                utils.EditTask();
                break;
            case 4:
                Console.Clear();
                utils.TaskDeletion();
                break;
            case 5:
                Console.Clear();
                Console.WriteLine("Thanks for using 1001 Tasks! Exiting Program...press any key to continue.");
                Console.ReadKey();
                Environment.Exit(0);
                break;
                
        }
        
            
        }
        if(MoveCursor.Key == ConsoleKey.Escape)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Are you sure you want to exit the program? Press ESC again to EXIT or any other key to return");
                ConsoleKeyInfo escapeSituation = Console.ReadKey(true);
                if(escapeSituation.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Console.WriteLine("Thanks for using 1001 Tasks! Exiting Program...press any key to continue.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    break;
                }
            }
    }
    
        
}

