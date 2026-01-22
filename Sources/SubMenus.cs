using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1001tarefas.Sources
{
  
    //This class controls all the needed submenus in the CLI dynamic selection system.
    public class SubMenus
    {
        Utils utils = new();

        // Submenu for the Option 1 in the Main Menu. It break down the selection of tasks to see All of them, or filter by its Status or Today date;
        public void ShowTasksMenu()
        {
            string[] showOptions = {"1. List Today Tasks","2. List All Tasks", "3. List PENDENT Tasks","4. List DONE Tasks", "5. List LATE Tasks"};
            Console.Clear();
            int choiceIndex = 0;
            
            
            while(true)
            {                
                Console.Clear();
                Console.WriteLine("------- Show Current Tasks -------\n");
                for(int n = 0; n < showOptions.Length; n++)
                {
                    if(choiceIndex == n)
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
                    Console.WriteLine(showOptions[n]);
                }

                ConsoleKeyInfo MoveCursor = Console.ReadKey(true);

                if(MoveCursor.Key == ConsoleKey.UpArrow)
                {
                    if(choiceIndex == 0)
                    {
                        choiceIndex = showOptions.Length - 1;
                    }
                    else if(choiceIndex > 0)
                    {
                        choiceIndex--;
                    }
                }
                else if(MoveCursor.Key == ConsoleKey.DownArrow)
                {
                    if(choiceIndex == showOptions.Length - 1)
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
                            utils.OnlyToday();
                            break;
                        case 1:
                            utils.ShowAllTasks();
                            break;
                        case 2:
                            utils.OnlyPendent();
                            break;
                        case 3:
                            utils.OnlyDone();
                            break;
                        case 4:
                            utils.OnlyLate();
                            break;
                    }
                }
                if(MoveCursor.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}