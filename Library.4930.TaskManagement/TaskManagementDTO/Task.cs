using Library.TaskManagement.TaskManagementDB;
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

        public Task(TaskDB taskDB)
        {
            Name = taskDB.Name;
            Description = taskDB.Description;
            DeadlineDate = taskDB.Deadline;
            IsCompleted = taskDB.IsCompleted;
            Priority = taskDB.Priority;
            Id = taskDB.Id;
        }

        public Task() { }
    }
}
