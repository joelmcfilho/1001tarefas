using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1001tarefas.Models
{
    public class Task
    {
        public Guid id {get;set;} = Guid.NewGuid();
        public string Name {get;set;}
        public string Description {get;set;}
        public DateTime Dateandtime {get;set;}
        public TaskStatus Status {get;set;}
    }
}