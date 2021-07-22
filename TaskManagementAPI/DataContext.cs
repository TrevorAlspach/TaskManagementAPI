using Library.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementAPI
{
    public class DataContext
    {
        public static List<NamedList<Item>> Lists = new List<NamedList<Item>>() { 
            new NamedList<Item>("TESTLIST") 
            { 
                list = new List<Item> 
                {
                 new Appointment("TEST", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Appointment("TEST", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Appointment("TEST", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Library.TaskManagement.Task("test", "test", DateTimeOffset.Now),
                 new Appointment("TEST", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Appointment("TEST", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Appointment("TEST", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Library.TaskManagement.Task("test", "test33dfd", DateTimeOffset.Now),
                } 
            },
            
            new NamedList<Item>("TESTLIST2")
            {
                list = new List<Item>
                {
                 new Appointment("TEST23", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), true, true),
                 new Appointment("TEST23", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Appointment("TEST23", "TEST", DateTimeOffset.Now, DateTimeOffset.Now, new List<string>(), false, false),
                 new Library.TaskManagement.Task ("test22", "teshht2", DateTimeOffset.Now),
                }
            }
        };
    }
}
