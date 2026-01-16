using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1001tarefas.Enums;

namespace _1001tarefas.Models
{
    public class TaskModel
    {
        public Guid id {get;set;} = Guid.NewGuid();
        public string Name {get;set;}
        public string Description {get;set;}
        public DateTime Dateandtime {get;set;}
        public StatusOfTask Status {get;set;}
    }
}