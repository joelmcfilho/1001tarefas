using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using _1001tarefas.Models;

namespace _1001tarefas.Repository
{
    public class TaskRepository
    {
        // Folder which will the JSON file will be saved
        private readonly string _folder;
        private readonly string _filePath;
        
        // This Method will "put" the JSON "tasks.json" in the folder created above
        public TaskRepository()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _folder = Path.Combine(basePath,"1001Tasks");
            _filePath = Path.Combine(_folder,"tasks.json");
        }
        
        // This method will ensure the JSON and its containing folder exists. It will be called
        // everytime the program starts
        public void EnsureJsonExists()
        {
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath,"[]");
            }
            
        }

        public List<TaskModel> GetTasks()
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TaskModel>>(json) ?? new();
            }
            catch
            {
                File.WriteAllText(_filePath,"[]");
                return new();
            }
            
        }

        // Create a new Task, then save it in the JSON
        public void CreateTask(TaskModel task)
        {
            var list = GetTasks();
            list.Add(task);
            SaveData(list);

        }

        // Delete an existing task, then save the changes in the JSON
        public void DeleteTask(Guid id)
        {
            var list = GetTasks();
            list.RemoveAll(x => x.id == id);
            SaveData(list);
        }

        // Update an existing
        public void UpdateTask(TaskModel task)
        {
            var list = GetTasks();
            var index = list.FindIndex(x => x.id == task.id);
            if (index >= 0)
            {
                list[index] = task;
                SaveData(list);
            }
        }

        // Save the changes in JSON
        public void SaveData(List<TaskModel> newlist)
        {
            var json = JsonSerializer.Serialize(newlist, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath,json);
        }

    }
}