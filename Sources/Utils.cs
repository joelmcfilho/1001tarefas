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
                // 'DONE' or 'CANCELLED'.
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
            Console.WriteLine("Select an Task to Delete: (Press ESC anytime to return to Main Menu!)");
        }

        public void ShowAllTasks()
        {
            TaskRepository taskRepository = new();

            List<TaskModel> showList = taskRepository.GetTasks();
            foreach(var x in showList)
            {
                Console.WriteLine($"{showList.IndexOf(x) + 1}: {x.Name} - {x.Dateandtime.ToString("d")} - {x.Status}");
                
            }
            Console.ReadKey();

        }
    }
}