using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using _1001tarefas.Models;
using _1001tarefas.Repository;

namespace _1001tarefas.Sources
{
    public class Utils
    {
        // Dynamic string builder, with the possibility to cancel the operation anytime.
        static bool ReadLineWithEscape(out string stringFinal)
        {   
            var input = new StringBuilder();

            while(true)
            {
                var key = Console.ReadKey(true);

                if(key.Key == ConsoleKey.Escape)
                {
                    stringFinal = null;
                    return false;
                }
                else if(key.Key == ConsoleKey.Enter)
                {
                    stringFinal = input.ToString();
                    Console.WriteLine();
                    return true;
                }
                else if(key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length-1,1);
                    Console.Write("\b \b");
                }
                else if(!char.IsControl(key.KeyChar))
                {
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }


        // Task Creation main method
        public void NewTaskInput()
        {
            bool isDateRight = false;
            DateTime dateFormated = DateTime.MinValue;
            TaskRepository taskRep = new();

            Console.WriteLine("Type your new task infos below. Press ESC anytime to return to Main Menu!");
            
            Console.Write("Title: ");
            // This is an bool condition check... if the user press ESC anytime, it will get FALSE 
            // in value, and will trigger the IF below, and return to the main menu loop
            if(!ReadLineWithEscape(out string title)) return;
                
            Console.Write("Description: ");
            if(!ReadLineWithEscape(out string desc)) return;
            
            while(isDateRight == false)
            {
                Console.Write("Date (Format 'DD/MM/YY' or 'DD/MM/YYYY')");
                if(!ReadLineWithEscape(out string dateAndTime)) return;
                
                try
                {
                    dateFormated = Convert.ToDateTime(dateAndTime);
                    if(dateFormated >= DateTime.Today)
                    {
                        isDateRight = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date. The date entered must be equal to or later than today's date.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The Date must be in the 'DD/MM/YY' or 'DD/MM/YYYY'. Try again");
                }
            
            }
                        
            var newTask = new _1001tarefas.Models.TaskModel
            {
                Name = title,
                Description = desc,
                Dateandtime = dateFormated,
                // Status will be, for default, 'PENDENT'. The user must change it manually to 
                // 'DONE' or Cancel/Delete it by an specific operation'.
                Status = Enums.StatusOfTask.Pendent
            };

            taskRep.CreateTask(newTask);
            Console.WriteLine($"'{title}' task successfully created! Press any key to return");
            Console.ReadKey();

            return;        
        }

        // Method to delete task, with dynamic selection mode
        public void TaskDeletion()
        {
            int choiceIndex = 0;
            TaskRepository taskRep = new();
            while(true)
            {
                    
                    Console.Clear();
                    Console.WriteLine("Select an Task to Delete: (Press ESC anytime to return to Main Menu!)\n");
                    List<TaskModel> deleteList = taskRep.GetTasks();

                    
                    //Dynamic selection of tasks
                    for(int i = 0; i < deleteList.Count; i++)
                    {                
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
                        Console.WriteLine($"{i+1}: {deleteList[i].Name} - {deleteList[i].Dateandtime.ToString("d")} - {deleteList[i].Status}");
                        
                        
                    }
                                        
                    ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                
                    if(MoveCursor.Key == ConsoleKey.UpArrow)
                    {
                        if(choiceIndex == 0)
                        {
                            choiceIndex = deleteList.Count - 1;
                        }
                        else if(choiceIndex > 0)
                        {
                            choiceIndex--;
                        }
                    }
                    else if(MoveCursor.Key == ConsoleKey.DownArrow)
                    {
                        if(choiceIndex == deleteList.Count - 1)
                        {
                            choiceIndex = 0;
                        }
                        else
                        {
                            choiceIndex++;
                        }
                    }
                    else if(MoveCursor.Key == ConsoleKey.Enter)
                    {
                        
                        Console.Clear();
                        Console.WriteLine("Are you sure? You can't undo this action!");
                        Console.WriteLine("Press Y to DELETE, or any other Key to Cancel:");
                        ConsoleKeyInfo deleteDecision = Console.ReadKey(true);
                        if(deleteDecision.Key == ConsoleKey.Y)
                            {
                                //Deletes Task searching for its Guid.id attribute
                                Console.Clear();
                                Console.WriteLine($"{deleteList[choiceIndex].Name} task deleted sucessfully!");
                                taskRep.DeleteTask(deleteList[choiceIndex].id);
                                
                            }
                        else
                            {
                                return;
                            }
                    }
                    else if(MoveCursor.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
             }
            }
            
        //Read the desserialized infos from JSON and list them in an dynamic selection
        public void ShowAllTasks()
            {
                TaskRepository taskRepository = new();
                int choiceIndex = 0;
                DateTime showTodayDate = new();
                Console.WriteLine($"Existing Tasks - Today: {showTodayDate.Day.ToString("DD/MM/YY")}");

                while(true)
                {
                    Console.Clear();
                    List<TaskModel> showList = taskRepository.GetTasks();
                    for(int i = 0; i < showList.Count; i++)
                            {                
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
                                Console.WriteLine($"{i+1}: {showList[i].Name} - {showList[i].Dateandtime.ToString("d")} - {showList[i].Status}");
                                
                                
                            }
                                                
                            ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                        
                            if(MoveCursor.Key == ConsoleKey.UpArrow)
                            {
                                if(choiceIndex == 0)
                                {
                                    choiceIndex = showList.Count - 1;
                                }
                                else if(choiceIndex > 0)
                                {
                                    choiceIndex--;
                                }
                            }
                            else if(MoveCursor.Key == ConsoleKey.DownArrow)
                            {
                                if(choiceIndex == showList.Count - 1)
                                {
                                    choiceIndex = 0;
                                }
                                else
                                {
                                    choiceIndex++;
                                }
                            }
                            else if(MoveCursor.Key == ConsoleKey.Enter)
                            {
                                // User can see details of every task, then go back to the task selection loop
                                Console.Clear();
                                Console.WriteLine($"Task: {showList[choiceIndex].Name}");
                                Console.WriteLine($"Description: {showList[choiceIndex].Description}");
                                Console.WriteLine($"Date: {showList[choiceIndex].Dateandtime}");
                                Console.WriteLine($"Status: {showList[choiceIndex].Status}");
                                Console.WriteLine("--------------------------------------------");
                                Console.WriteLine("Press Any Key to exit!");
                                Console.ReadKey();
                            }
                            else if(MoveCursor.Key == ConsoleKey.Escape)
                            {
                                break;
                            }
                }
            }
            
            // Method to edit (update) an existing task
            public void EditTask()
                {
                    TaskRepository taskRepository = new();
                    int choiceIndex = 0;

                    while(true)
                    {
                        Console.Clear();
                        List<TaskModel> editList = taskRepository.GetTasks();
                        for(int i = 0; i < editList.Count; i++)
                                {                
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
                                    Console.WriteLine($"{i+1}: {editList[i].Name} - {editList[i].Dateandtime.ToString("d")} - {editList[i].Status}");
                                    
                                    
                                }
                                                    
                                ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                            
                                if(MoveCursor.Key == ConsoleKey.UpArrow)
                                {
                                    if(choiceIndex == 0)
                                    {
                                        choiceIndex = editList.Count - 1;
                                    }
                                    else if(choiceIndex > 0)
                                    {
                                        choiceIndex--;
                                    }
                                }
                                else if(MoveCursor.Key == ConsoleKey.DownArrow)
                                {
                                    if(choiceIndex == editList.Count - 1)
                                    {
                                        choiceIndex = 0;
                                    }
                                    else
                                    {
                                        choiceIndex++;
                                    }
                                }
                                else if(MoveCursor.Key == ConsoleKey.Enter)
                                {
                                    var selectedTask = editList[choiceIndex];
                                    Console.Clear();
                                    Console.WriteLine("Please write the new informations for your task (Press ESC anytime to cancel):");
                                    Console.WriteLine("--------------------------------------------------------");
                                    Console.WriteLine($"Current Title: {editList[choiceIndex].Name} (press ENTER to keep current value)\n");
                                    Console.WriteLine($"Current Title: {editList[choiceIndex].Name}");

                                }
                                else if(MoveCursor.Key == ConsoleKey.Escape)
                                {
                                    break;
                                }
                    }
                }

        }
        
    }
