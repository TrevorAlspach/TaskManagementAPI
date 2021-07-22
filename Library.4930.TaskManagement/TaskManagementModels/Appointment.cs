using Library.TaskManagement;
using Library.TaskManagement.TaskManagementDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement
{
    public class Appointment : Item
    {
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public List<string> Atendees { get; set; }

        public Appointment(string name, string description, DateTimeOffset starttime, DateTimeOffset endtime, List<string> atendees, bool iscompleted = false, bool priority = false)
        {
            Name = name;
            Description = description;
            StartTime = starttime;
            EndTime = endtime;
            IsCompleted = iscompleted;
            Priority = priority;
            Atendees = new List<string>(atendees);
            Id = Guid.NewGuid();
        }

        public Appointment(AppointmentDTO appointmentDTO)
        {
            IsCompleted = appointmentDTO.IsCompleted;
            Priority = appointmentDTO.Priority;
            Name = appointmentDTO.Name;
            Description = appointmentDTO.Description;
            StartTime = appointmentDTO.StartTime;
            EndTime = appointmentDTO.EndTime;
            Atendees = appointmentDTO.atendees;
        }

        public Appointment() 
        {
            Atendees = new List<string>();
        }
    }
}
