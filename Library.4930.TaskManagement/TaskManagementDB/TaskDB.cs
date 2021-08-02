using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.TaskManagementDB
{
    public class TaskDB
    {
        public Guid Id { get; set; }

        public Guid ListId { get; set; }

        public bool IsCompleted {get; set;}
        
        public bool Priority { get; set; }

        public string Name { get; set; }
       
        public string Description { get; set; }
        
        public DateTimeOffset Deadline { get; set; }


        public TaskDB(bool iscompleted, bool priority, string name, string desc, DateTimeOffset deadlinedate)
        {
            this.IsCompleted = iscompleted;
            this.Priority = priority;
            this.Name = name;
            this.Description = desc;
            this.Deadline = deadlinedate;
        }

        public TaskDB(Task task, Guid ListId)
        {
            this.Id = Guid.NewGuid();
            this.IsCompleted = task.IsCompleted;
            this.Priority = task.Priority;
            this.Name = task.Name;
            this.Description = task.Description;
            this.Deadline = task.DeadlineDate;
            this.ListId = ListId;
        }

        public TaskDB() { }
    }
}
