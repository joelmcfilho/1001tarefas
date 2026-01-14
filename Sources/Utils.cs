using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1001tarefas.Sources
{
    public class Utils
    {
        public void NewTaskInput()
        {
            Console.WriteLine("Type your new task infos below.");
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Description: ");
            string desc = Console.ReadLine();
            Console.Write("Date and Time (Format 'DD/MM/YY - HH:mm', 24h time format)");
            string dateandtime = Console.ReadLine();

            var newtask = new _1001tarefas.Models.Task
            {
                Name = title,
                Description = desc

            };           
        }
    }
}