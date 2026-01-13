using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace _1001tarefas.Repository
{
    public class TaskRepository
    {
        
        private readonly string _folder = "Data";
        private readonly string _filePath;
        public TaskRepository()
        {
            _filePath = Path.Combine(_folder,"tasks.json");
        }
        

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

    }
}