using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1001tarefas.Repository;

namespace _1001tarefas.Sources
{
    public class Utils
    {
        public void NewTaskInput()
        {
            bool isDateRight = false;
            DateTime dateFormated = DateTime.MinValue;
            TaskRepository taskRep = new();

            Console.WriteLine("Type your new task infos below.");

            Console.Write("Title: ");
            string title = Console.ReadLine().Trim();
            
            Console.Write("Description: ");
            string desc = Console.ReadLine().Trim();
            
            while(isDateRight == false)
            {
                Console.Write("Date (Format 'DD/MM/YY' or 'DD/MM/YYYY')");
                string dateAndTime = Console.ReadLine().Trim();
                
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
    }
}