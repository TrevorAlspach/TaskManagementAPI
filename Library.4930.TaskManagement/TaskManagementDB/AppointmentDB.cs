using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Library.TaskManagement.TaskManagementDB
{
    public class AppointmentDB
    {
        public Guid Id { get; set; }

        public Guid ListId { get; set; }

        public bool IsCompleted { get; set; }

        public bool Priority { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public AppointmentDB(bool iscompleted, bool priority, string name, string desc, DateTimeOffset starttime, DateTimeOffset endtime)
        {
            this.Id = Guid.NewGuid();
            this.IsCompleted = iscompleted;
            this.Priority = priority;
            this.Name = name;
            this.Description = desc;
            this.StartTime = starttime;
            this.EndTime = endtime;
        }

        public AppointmentDB(Appointment appt, Guid ListId)
        {
            this.Id = Guid.NewGuid();
            this.IsCompleted = appt.IsCompleted;
            this.Priority = appt.Priority;
            this.Name = appt.Name;
            this.Description = appt.Description;
            this.StartTime = appt.StartTime;
            this.EndTime = appt.EndTime;
            this.ListId = ListId;
        }

        public AppointmentDB() { }
    }
}
