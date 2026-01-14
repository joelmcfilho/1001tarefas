using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace _1001tarefas.Repository
{
    public class TaskRepository
    {
        // Folder which will the JSON file will be saved
        private readonly string _folder = "Data";
        private readonly string _filePath;
        // This Method will "put" the JSON "tasks.json" in the folder created above
        public TaskRepository()
        {
            _filePath = Path.Combine(_folder,"tasks.json");
        }
        
        // This method will ensure the JSON and its containing folder exists. It will be called
        // everytime the program starts
        public void EnsureJsonExists()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath,"[]");
            }
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
        }

        public List<Task> GetTasks()
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Task>>(json) ?? new();
            }
            catch
            {
                File.WriteAllText(_filePath,"[]");
                return new();
            }
            
        }

        // Create a new Task, then save it in the JSON
        public void CreateTask(Task task)
        {
            var list = GetTasks();
            list.Add(task);
            SaveData(list);

        }

        // Delete an existing task, then save the changes in the JSON
        public void DeleteTask(Task task)
        {
            var list = GetTasks();
            list.Remove(task);
            SaveData(list);
        }

        // Update an existing
        public void UpdateTask(Task task)
        {
            var list = GetTasks();
            var index = list.FindIndex(x => x.Id == task.Id);
            if (index >= 0)
            {
                list[index] = task;
                SaveData(list);
            }
        }

        // Save the changes in JSON
        public void SaveData(List<Task> newlist)
        {
            var json = JsonSerializer.Serialize(newlist, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath,json);
        }

    }
}