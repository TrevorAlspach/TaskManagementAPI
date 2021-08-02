using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.TaskManagement;
using Library.TaskManagement.TaskManagementDB;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Persistence
{
    public class TMContext : DbContext
    {
        public TMContext(DbContextOptions<TMContext> options) : base(options)
        {
        }

        public virtual DbSet<NamedListDB> TaskLists { get; set; }
        
        public virtual DbSet<AppointmentDB> Appointments { get; set; }

        public virtual DbSet<TaskDB> Tasks { get; set; }

        public virtual DbSet<AtendeeDB> Atendees { get; set; }

    }
}
