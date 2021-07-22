using Library.TaskManagement.TaskManagementDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement
{
    public class Task : Item
    {
        public DateTimeOffset DeadlineDate { get; set; }

        public TimeSpan DeadlineTime { get; set; }

        public Task(string name, string description, DateTimeOffset deadline, bool iscompleted = false, bool priority = false)
        {
            Name = name;
            Description = description;
            DeadlineDate = deadline;
            IsCompleted = iscompleted;
            Priority = priority;
            Id = Guid.NewGuid();
        }

        public Task(TaskDTO taskDTO)
        {
            Name = taskDTO.Name;
            Description = taskDTO.Description;
            DeadlineDate = taskDTO.Deadline;
            IsCompleted = taskDTO.IsCompleted;
            Priority = taskDTO.Priority;
        }

        public Task() { }
    }
}
