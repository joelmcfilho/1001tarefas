using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using _1001tarefas.Enums;
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

        // If an task is with an date earlier than today's date, it will automatically labelled as LATE by the system
        public void AutoLateLabeller()
        {
            TaskRepository taskRep = new();
            List<TaskModel> lateList = taskRep.GetTasks().Where(x => x.Status == StatusOfTask.Pendent).ToList();
            

            for(int i=0; i < lateList.Count;i++)
            {
                if(lateList[i].Dateandtime < DateTime.Today)
                {
                    lateList[i].Status = StatusOfTask.Late;
                    taskRep.UpdateTask(lateList[i]);
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
                // The system will autoflag it 'LATE' if its date has passed
                Status = Enums.StatusOfTask.Pendent
            };

            taskRep.CreateTask(newTask);
            Console.WriteLine($"'{title}' task successfully created! Press any key to return");
            Console.ReadKey();

            return;        
        }

        // Method to the Mark as Done option
        public void MarkAsDone()
        {
            while(true)
                {
                TaskRepository taskRepository = new();
                int choiceIndex = 0;                
                List<TaskModel> query = taskRepository.GetTasks().Where(x => x.Status == StatusOfTask.Pendent || x.Status == StatusOfTask.Late).ToList();
                if(query.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("There is no tasks PENDENT or LATE in the system. Press Any Key to return.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                while(true)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"Choose an task to mark as DONE:\n");
                    for(int i = 0; i < query.Count; i++)
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
                            Console.WriteLine($"{i+1}: {query[i].Name} - {query[i].Dateandtime.ToString("d")} - {query[i].Status}");
                            
                            
                        }
                                            
                        ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                    
                        if(MoveCursor.Key == ConsoleKey.UpArrow)
                        {
                            if(choiceIndex == 0)
                            {
                                choiceIndex = query.Count - 1;
                            }
                            else if(choiceIndex > 0)
                            {
                                choiceIndex--;
                            }
                        }
                        else if(MoveCursor.Key == ConsoleKey.DownArrow)
                        {
                            if(choiceIndex == query.Count - 1)
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
                            var selectedTask = query[choiceIndex];
                            while(true)
                            {
                            Console.Clear();
                            Console.WriteLine($"Are you sure? {selectedTask.Name} task will be marked as DONE. It can be changed in the 'Edit an Task' option in the Main Menu.");
                            Console.WriteLine("Press Y to confirm the operation, or any other key to Cancel!");
                            ConsoleKeyInfo decision = Console.ReadKey(true);
                            if(decision.Key == ConsoleKey.Y)
                            {
                                selectedTask.Status = StatusOfTask.Done;
                                taskRepository.UpdateTask(selectedTask);
                                Console.WriteLine("Marked as Done! Press any key to continue.");
                                Console.ReadKey();
                                break;
                            }
                            else
                            {
                                break;
                            }
                            }
                        }
                        else if(MoveCursor.Key == ConsoleKey.Escape)
                        {
                            return;
                        }
                }
                }
                }
        }

        // Method to delete task, with dynamic selection mode
        public void TaskDeletion()
        {
            int choiceIndex = 0;
            TaskRepository taskRep = new();
            List<TaskModel> deleteList = taskRep.GetTasks();
            if(deleteList.Count == 0)
            {
                Console.WriteLine("There is no Tasks in the system. Press any key to return.");
                Console.ReadKey();
                return;
            }
            else
            {
            while(true)
            {                    
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Select an Task to Delete: (Press ESC anytime to return to Main Menu!)\n");
                                
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
                            Console.WriteLine($"{deleteList[choiceIndex].Name} task deleted sucessfully! Press any key to continue");
                            taskRep.DeleteTask(deleteList[choiceIndex].id);
                            deleteList.RemoveAt(choiceIndex);
                            Console.ReadKey();
                            Console.Clear();
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
            }

        // This method returns an query with dynamic selection of the tasks that are scheduled for today's date
        public void OnlyToday()
        {
            while(true)
                {
                TaskRepository taskRepository = new();
                int choiceIndex = 0;                               
                List<TaskModel> query = taskRepository.GetTasks().Where(x => x.Dateandtime == DateTime.Today).ToList();
                if(query.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"There is no tasks for Today in the system.\nSystem date: {DateTime.Today:dd/MM/yy}. Press Any Key to return.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                while(true)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"Existing Tasks marked as PENDENT - Today: {DateTime.Today:dd/MM/yy}");
                    for(int i = 0; i < query.Count; i++)

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
                                Console.WriteLine($"{i+1}: {query[i].Name} - {query[i].Dateandtime.ToString("d")} - {query[i].Status}");
                                
                                
                            }
                                                
                            ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                        
                            if(MoveCursor.Key == ConsoleKey.UpArrow)
                            {
                                if(choiceIndex == 0)
                                {
                                    choiceIndex = query.Count - 1;
                                }
                                else if(choiceIndex > 0)
                                {
                                    choiceIndex--;
                                }
                            }
                            else if(MoveCursor.Key == ConsoleKey.DownArrow)
                            {
                                if(choiceIndex == query.Count - 1)
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
                                Console.WriteLine($"Task: {query[choiceIndex].Name}");
                                Console.WriteLine($"Description: {query[choiceIndex].Description}");
                                Console.WriteLine($"Date: {query[choiceIndex].Dateandtime}");
                                Console.WriteLine($"Status: {query[choiceIndex].Status}");
                                Console.WriteLine("--------------------------------------------");
                                Console.WriteLine("Press Any Key to exit!");
                                Console.ReadKey();
                            }
                            else if(MoveCursor.Key == ConsoleKey.Escape)
                            {
                                return;
                            }
                }
                }
                }
        }
            
        //This one returns an dynamic selection of all tasks in the system.
        public void ShowAllTasks()
            {
                TaskRepository taskRepository = new();
                int choiceIndex = 0;
                List<TaskModel> showList = taskRepository.GetTasks();
                
                if(showList.Count == 0)
                {
                    Console.WriteLine("There is no Tasks in the system. Press any key to return.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                while(true)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"Existing Tasks - Today: {DateTime.Today:dd/MM/yy}");
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
            }

            // This method returns an dynamic selection of tasks flagged with the PENDENT status
            public void OnlyPendent()
            {
                while(true)
                {
                TaskRepository taskRepository = new();
                int choiceIndex = 0;                
                List<TaskModel> query = taskRepository.GetTasks().Where(x => x.Status == StatusOfTask.Pendent).ToList();
                if(query.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("There is no tasks PENDENT in the system. Press Any Key to return.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                while(true)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"Existing Tasks marked as PENDENT - Today: {DateTime.Today:dd/MM/yy}");
                    for(int i = 0; i < query.Count; i++)

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
                                Console.WriteLine($"{i+1}: {query[i].Name} - {query[i].Dateandtime.ToString("d")} - {query[i].Status}");
                                
                                
                            }
                                                
                            ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                        
                            if(MoveCursor.Key == ConsoleKey.UpArrow)
                            {
                                if(choiceIndex == 0)
                                {
                                    choiceIndex = query.Count - 1;
                                }
                                else if(choiceIndex > 0)
                                {
                                    choiceIndex--;
                                }
                            }
                            else if(MoveCursor.Key == ConsoleKey.DownArrow)
                            {
                                if(choiceIndex == query.Count - 1)
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
                                Console.WriteLine($"Task: {query[choiceIndex].Name}");
                                Console.WriteLine($"Description: {query[choiceIndex].Description}");
                                Console.WriteLine($"Date: {query[choiceIndex].Dateandtime}");
                                Console.WriteLine($"Status: {query[choiceIndex].Status}");
                                Console.WriteLine("--------------------------------------------");
                                Console.WriteLine("Press Any Key to exit!");
                                Console.ReadKey();
                            }
                            else if(MoveCursor.Key == ConsoleKey.Escape)
                            {
                                return;
                            }
                }
                }
                }
            }

            // This method returns an dynamic selection of tasks flagged with the DONE status
            public void OnlyDone()
            {
                TaskRepository taskRepository = new();
                int choiceIndex = 0;                
                List<TaskModel> query = taskRepository.GetTasks().Where(x => x.Status == StatusOfTask.Done).ToList();          
                if(query.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("There is no DONE tasks in the system. Press Any Key to return.");
                    Console.ReadKey();
                }
                else
                {
                while(true)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"Existing Tasks marked as DONE - Today: {DateTime.Today:dd/MM/yy}");
                    for(int i = 0; i < query.Count; i++)

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
                                Console.WriteLine($"{i+1}: {query[i].Name} - {query[i].Dateandtime.ToString("d")} - {query[i].Status}");
                                
                                
                            }
                                                
                            ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                        
                            if(MoveCursor.Key == ConsoleKey.UpArrow)
                            {
                                if(choiceIndex == 0)
                                {
                                    choiceIndex = query.Count - 1;
                                }
                                else if(choiceIndex > 0)
                                {
                                    choiceIndex--;
                                }
                            }
                            else if(MoveCursor.Key == ConsoleKey.DownArrow)
                            {
                                if(choiceIndex == query.Count - 1)
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
                                Console.WriteLine($"Task: {query[choiceIndex].Name}");
                                Console.WriteLine($"Description: {query[choiceIndex].Description}");
                                Console.WriteLine($"Date: {query[choiceIndex].Dateandtime}");
                                Console.WriteLine($"Status: {query[choiceIndex].Status}");
                                Console.WriteLine("--------------------------------------------");
                                Console.WriteLine("Press Any Key to exit!");
                                Console.ReadKey();
                            }
                            else if(MoveCursor.Key == ConsoleKey.Escape)
                            {
                                return;
                            }
                }
                
                }
            }

            // This method returns an dynamic selection of tasks flagged with the LATE status
            public void OnlyLate()
            {
                TaskRepository taskRepository = new();
                int choiceIndex = 0;
                List<TaskModel> showList = taskRepository.GetTasks();
                List<TaskModel> query = showList.Where(x => x.Status == StatusOfTask.Late).ToList();
                while(true)
                {
                
                if(query.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("There is no LATE tasks in the system. Press Any Key to return.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine($"Existing Tasks marked as LATE - Today: {DateTime.Today:dd/MM/yy}");
                    for(int i = 0; i < query.Count; i++)

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
                                Console.WriteLine($"{i+1}: {query[i].Name} - {query[i].Dateandtime.ToString("d")} - {query[i].Status}");
                                
                                
                            }
                                                
                            ConsoleKeyInfo MoveCursor = Console.ReadKey(true);
                                        
                            if(MoveCursor.Key == ConsoleKey.UpArrow)
                            {
                                if(choiceIndex == 0)
                                {
                                    choiceIndex = query.Count - 1;
                                }
                                else if(choiceIndex > 0)
                                {
                                    choiceIndex--;
                                }
                            }
                            else if(MoveCursor.Key == ConsoleKey.DownArrow)
                            {
                                if(choiceIndex == query.Count - 1)
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
                                Console.WriteLine($"Task: {query[choiceIndex].Name}");
                                Console.WriteLine($"Description: {query[choiceIndex].Description}");
                                Console.WriteLine($"Date: {query[choiceIndex].Dateandtime}");
                                Console.WriteLine($"Status: {query[choiceIndex].Status}");
                                Console.WriteLine("--------------------------------------------");
                                Console.WriteLine("Press Any Key to exit!");
                                Console.ReadKey();
                            }
                            else if(MoveCursor.Key == ConsoleKey.Escape)
                            {
                                return;
                            }
                }
                }
                }
            }



            
            // Method to edit (update) an existing task
            public void EditTask()
                {
                    TaskRepository taskRepository = new();
                    int choiceIndex = 0;
                    bool isDateRight = false;
                    DateTime dateFormated = DateTime.MinValue;
                    List<TaskModel> editList = taskRepository.GetTasks();
                    if(editList.Count == 0)
                    {
                        Console.WriteLine("There is no Tasks in the system. Press any key to return.");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                    while(true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Showing all Tasks in the System:\n Today{DateTime.Today:dd/MM/yy}\n");
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
                                    Console.WriteLine($"Current Title: {selectedTask.Name} (press ENTER to keep current value)\n");
                                    if(!ReadLineWithEscape(out string title)) return;
                                    if(!String.IsNullOrWhiteSpace(title))
                                        {
                                            selectedTask.Name = title;
                                        }

                                    Console.WriteLine($"Current Description: {selectedTask.Description} (press ENTER to keep current value)\n");
                                    if(!ReadLineWithEscape(out string desc)) return;
                                    if(!String.IsNullOrWhiteSpace(desc))
                                        {
                                            selectedTask.Description = desc;
                                        }


                                    while(isDateRight == false)
                                    {
                                        Console.Write($"Current Date: {selectedTask.Dateandtime} (press ENTER to keep current value)\n");
                                        if(!ReadLineWithEscape(out string dateAndTime)) return;
                                        if(!String.IsNullOrWhiteSpace(dateAndTime))
                                            {
                                                try
                                                {
                                                    dateFormated = Convert.ToDateTime(dateAndTime);
                                                    if(dateFormated >= DateTime.Today)
                                                    {
                                                        selectedTask.Dateandtime = dateFormated;
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
                                        else
                                            {
                                                isDateRight = true;
                                            }
                                    }
                                    
                                    if(selectedTask.Status == StatusOfTask.Done)
                                    {
                                        Console.Clear();
                                        while(true)
                                        {
                                        Console.WriteLine($"The task {selectedTask.Name} is marked as DONE. You want to change it to PENDENT?");
                                        Console.WriteLine("Press any key to keep as DONE or Y to change to PENDENT.");
                                        ConsoleKeyInfo decision = Console.ReadKey(true);
                                        if(decision.Key == ConsoleKey.Y)
                                        {
                                            selectedTask.Status = StatusOfTask.Pendent;
                                            Console.WriteLine("Changes made sucessfully. Task is now PENDENT. Press any key to continue.");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                        }
                                    }
                                    
                                    taskRepository.UpdateTask(selectedTask);
                                    Console.WriteLine("\nTask updated sucessfully! Press Any key...");
                                    Console.ReadKey();
                                    isDateRight = false;
                                        

                                }
                                else if(MoveCursor.Key == ConsoleKey.Escape)
                                {
                                    break;
                                }
                    }
                }
                }

        }
        
    }
