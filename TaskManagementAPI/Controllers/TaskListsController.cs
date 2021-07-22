using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Library.TaskManagement;
using Newtonsoft.Json;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("TaskLists")]
    public class TaskListsController : ControllerBase
    {
        [HttpGet("test")]
        public string Test()
        {
            return "Test Successful!!";
        }

        [HttpGet("AllListIDs")]
        public ActionResult<Dictionary<string, Guid>> GetGuids()
        {
            //return Ok(DataContext.Lists.Select(list => list.id).ToDictionary(list => list.name));
            return Ok(DataContext.Lists.ToDictionary(list => list.name, list => list.Id));

        }

        [HttpGet("{id}")]
        public ActionResult<List<Item>> GetList(Guid id)
        {
            var namedlist = new NamedList<Item>(DataContext.Lists.FirstOrDefault(list => list.Id.Equals(id)));
            namedlist.list = namedlist.list.OrderByDescending(item => item.Priority.Equals(true)).ToList();
            var json = JsonConvert.SerializeObject(namedlist, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return Ok(json);
        }

        [HttpGet("{id}/Incomplete")]
        public ActionResult<List<Item>> GetListIncomplete(Guid id)
        {
            var namedlist = new NamedList<Item>(DataContext.Lists.FirstOrDefault(list => list.Id.Equals(id)));
            namedlist.list = namedlist.list.Where(item => item.IsCompleted.Equals(false)).OrderByDescending(item => item.Priority.Equals(true)).ToList();
            var json = JsonConvert.SerializeObject(namedlist, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return Ok(json);
        }

        [HttpPost("AddList")]
        public ActionResult<NamedList<Item>> AddList([FromBody] string name)
        {
            var newList = new NamedList<Item>(name);
            DataContext.Lists.Add(newList);
            return Ok(newList);
        }

        [HttpPost("DeleteList")]
        public ActionResult<NamedList<Item>> DeleteList([FromBody] Guid id)
        {
            var listToRemove = DataContext.Lists.FirstOrDefault(List => List.Id.Equals(id));
            if (listToRemove?.Id != Guid.Empty)
            {
                DataContext.Lists.Remove(listToRemove);
            }

            return Ok(listToRemove);
        }

        [HttpPost("{Id?}/AddOrUpdateTask")]
        public ActionResult<Library.TaskManagement.Task> AddOrUpdateTask(Guid Id, [FromBody]Library.TaskManagement.Task task)
        {
            var ListToAdd = DataContext.Lists.FirstOrDefault(List => List.Id.Equals(Id));
            Library.TaskManagement.Task newTask = null;
            if (task == null)
            {
                return BadRequest();
            }
            
            if (task.Id == Guid.Empty)
            {
                newTask = new Library.TaskManagement.Task(task.Name, task.Description, task.DeadlineDate, task.IsCompleted, task.Priority);
                ListToAdd.list.Add(newTask);
            }
            else
            {
                newTask = (Library.TaskManagement.Task) ListToAdd.list.FirstOrDefault(t => t.Id.Equals(task.Id));
                var index = ListToAdd.list.IndexOf(newTask);
                ListToAdd.list.RemoveAt(index);
                ListToAdd.list.Insert(index, task);
            }

            return Ok(new Library.TaskManagement.Task());

        }

        [HttpPost("{Id?}/AddOrUpdateAppointment")]
        public ActionResult<Appointment> AddOrUpdateAppointment(Guid Id, [FromBody] Appointment appt)
        {

            var ListToAdd = DataContext.Lists.FirstOrDefault(List => List.Id.Equals(Id));
            Appointment newAppointment = null;
            if (appt == null)
            {
                return BadRequest();
            }

            if (appt.Id == Guid.Empty)
            {
                newAppointment = new Appointment(appt.Name, appt.Description, appt.StartTime, appt.EndTime, appt.Atendees, appt.IsCompleted, appt.Priority);
                ListToAdd.list.Add(newAppointment);
            }
            else
            {
                newAppointment = (Appointment)ListToAdd.list.FirstOrDefault(t => t.Id.Equals(appt.Id));
                var index = ListToAdd.list.IndexOf(newAppointment);
                ListToAdd.list.RemoveAt(index);
                ListToAdd.list.Insert(index, appt);
            }

            return Ok(new Appointment());

        }

        [HttpPost("{Id?}/RemoveItem")]
        public ActionResult<Item> DeleteItem(Guid Id, [FromBody] Guid itemId)
        {
            var ListToEdit = DataContext.Lists.FirstOrDefault(List => List.Id.Equals(Id));

            if (itemId == Guid.Empty)
            {
                return BadRequest();
            }
            else
            {
                var itemToRemove = ListToEdit.list.FirstOrDefault(i => i.Id.Equals(itemId));
                ListToEdit.list.Remove(itemToRemove);
                return Ok(itemToRemove);
            }

        }

        [HttpGet("Search/{query?}")]
        public ActionResult<List<Item>> Search(string query)
        {
            List<Item> results = new List<Item>();
            foreach (NamedList<Item> namedlist in DataContext.Lists)
            {
                results.AddRange((from Item in namedlist.list
                                          where Item.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) || Item.Description.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                                          select Item).ToList());

                var appointments = from Item in namedlist.list where (Item is Appointment && !results.Contains(Item)) select Item;
                //select those Items which are appointments and not already in the list

                foreach (Appointment appointment in appointments)
                {
                    if ((from atendee in appointment.Atendees where atendee.Contains(query, StringComparison.InvariantCultureIgnoreCase) select atendee).Any())
                        results.Add(appointment);
                    //add those appointments where atendees list contains query string
                }
            }
            var json = JsonConvert.SerializeObject(results, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return Ok(json);
        }
    }
}
