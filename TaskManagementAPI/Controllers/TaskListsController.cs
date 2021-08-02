using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Library.TaskManagement;
using Newtonsoft.Json;
using TaskManagementAPI.Controllers.Enterprise;
using Library.TaskManagement.TaskManagementDB;
using TaskManagementAPI.Persistence;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("TaskLists")]
    public class TaskListsController : ControllerBase
    {
        private readonly TMContext db;
        public TaskListsController(TMContext context)
        {
            db = context;
        }

        [HttpGet("test")]
        public string Test()
        {
            return "Test Successful!!";
        }

        [HttpGet("AllListIDs")]
        public async Task<ActionResult<Dictionary<string, Guid>>> GetGuids()
        {
            return Ok(await new TaskListsEC(db).GetListIds());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Item>>> GetList(Guid id)
        {
            var namedlist = await new TaskListsEC(db).GetList(id, true);
            var json = JsonConvert.SerializeObject(namedlist, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return Ok(json);
        }

        [HttpGet("{id}/Incomplete")]
        public async Task<ActionResult<List<Item>>> GetListIncomplete(Guid id)
        {
            var namedlist = await new TaskListsEC(db).GetList(id, false);
            var json = JsonConvert.SerializeObject(namedlist, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return Ok(json);
        }

        [HttpPost("AddList")]
        public async Task<ActionResult<NamedListDB>> AddList([FromBody] string name)
        {
            var newList = new NamedList<Item>(name);
            return Ok(await new TaskListsEC(db).AddList(newList));
        }

        [HttpPost("DeleteList")]
        public async Task<ActionResult<NamedList<Item>>> DeleteList([FromBody] Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var listToRemove = db.TaskLists.FirstOrDefault(n => n.Id.Equals(id));
            if (listToRemove?.Id != Guid.Empty)
            {
                //DataContext.Lists.Remove(listToRemove);
                await new TaskListsEC(db).DeleteList(id);
            }

            return Ok(listToRemove);
        }

        [HttpPost("{Id?}/AddOrUpdateTask")]
        public async Task<ActionResult<Library.TaskManagement.Task>> AddOrUpdateTask(Guid Id, [FromBody]Library.TaskManagement.Task task)
        {
            if (task == null || Id == null)
            {
                return BadRequest();
            }
            var oldTask = await new TaskListsEC(db).AddOrUpdateTask(Id, task);

            return Ok(oldTask);

        }

        [HttpPost("{Id?}/AddOrUpdateAppointment")]
        public async Task<ActionResult<Appointment>> AddOrUpdateAppointment(Guid Id, [FromBody] Appointment appt)
        {
            if (appt == null)
            {
                return BadRequest();
            }
            var oldAppt = await new TaskListsEC(db).AddOrUpdateAppointment(Id, appt);

            return Ok(oldAppt);

        }

        [HttpPost("{Id?}/RemoveItem")]
        public async Task<ActionResult<Item>> DeleteItem(Guid Id, [FromBody] Guid itemId)
        {
            if (itemId == Guid.Empty || Id == Guid.Empty)
            {
                return BadRequest();
            }
            var itemToRemove = await new TaskListsEC(db).RemoveItem(itemId);
            if (itemToRemove == null)
            {
                return BadRequest();
            }
            return Ok(itemToRemove);

        }

        [HttpGet("Search/{query?}")]
        public ActionResult<List<Item>> Search(string query)
        {
            if (query is null)
            {
                return BadRequest();
            }
            var results = new TaskListsEC(db).Search(query);
            
            var json = JsonConvert.SerializeObject(results, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return Ok(json);
        }
    }
}
