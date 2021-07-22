using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.TaskManagementDTO
{
    public class AppointmentDTO
    {
        public bool IsCompleted { get; set; }

        public bool Priority { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public List<string> atendees { get; set; }

        public AppointmentDTO(bool iscompleted, bool priority, string name, string desc, DateTimeOffset starttime, DateTimeOffset endtime, List<string> atendees)
        {
            this.IsCompleted = iscompleted;
            this.Priority = priority;
            this.Name = name;
            this.Description = desc;
            this.StartTime = starttime;
            this.EndTime = endtime;
            this.atendees = atendees;
        }

        public AppointmentDTO() { }
    }
}
