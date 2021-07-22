using Library.TaskManagement.JsonSerialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item
    {
        public Guid Id { get; set; }

        public bool IsCompleted
        {
            get;
            set;
        }
        public bool Priority {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }

        public Item()
        {
        }
    }
}
