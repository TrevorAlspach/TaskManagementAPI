using System;
using System.Collections.Generic;
using System.Text;

namespace Library.TaskManagement.TaskManagementDB
{
    public class AtendeeDB
    {
        public int Id { get; set; }

        public Guid ItemId { get; set; }

        public string name { get; set; }

        public AtendeeDB(string name, Guid ItemId)
        {
            this.name = name;
            this.ItemId = ItemId;
        }
    }
}
