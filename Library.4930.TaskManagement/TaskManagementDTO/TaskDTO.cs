using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.TaskManagementDTO
{
    public class TaskDTO
    {
        public bool IsCompleted {get; set;}
        
        public bool Priority { get; set; }

        public string Name { get; set; }
       
        public string Description { get; set; }
        
        public DateTimeOffset Deadline { get; set; }


        public TaskDTO(bool iscompleted, bool priority, string name, string desc, DateTimeOffset deadlinedate)
        {
            this.IsCompleted = iscompleted;
            this.Priority = priority;
            this.Name = name;
            this.Description = desc;
            this.Deadline = deadlinedate;
        }

        public TaskDTO() { }
    }
}
