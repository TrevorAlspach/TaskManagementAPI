using Library.TaskManagement;
using Library.TaskManagement;
using Library.TaskManagement.TaskManagementDTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.TaskManagement.JsonSerialization
{
    public class ItemJsonConverter : JsonCreationConverter<Item>
    {
        protected override Item Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["Atendees"] != null)
            {
                return new Appointment();
            }
            else if (jObject["DeadlineDate"] != null)
            {
                return new Task();
            }
            else
            {
                return new Item();
            }
        }
    }
}
