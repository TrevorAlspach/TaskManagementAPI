using Library.TaskManagement;
using Library.TaskManagement.TaskManagementDB;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementAPI.Persistence;

namespace TaskManagementAPI.Controllers.Enterprise
{
    public class TaskListsEC
    {
        private readonly TMContext db;
        public TaskListsEC(TMContext context)
        {
            db = context;
        }
        public async Task<Dictionary<string, Guid>> GetListIds()
        {
            var results = new Dictionary<string, Guid>();
            using (db)
            {
                try
                {
                  results = await (from n in db.TaskLists select n).ToDictionaryAsync(n => n.name, n=>n.Id);
                } catch (Exception e) { }
            }
            return results;
        }

        public async System.Threading.Tasks.Task<NamedListDB> AddList(NamedList<Item> list)
        {
            NamedListDB newList = new NamedListDB { Id = list.Id, name = list.name };
            using (db)
            {
                try
                {
                   db.TaskLists.Add(newList);
                   await db.SaveChangesAsync();
                }
                catch (Exception e) { }
            }
            return newList;
        }

        public async Task<NamedListDB> DeleteList(Guid Id)
        {
            NamedListDB listToRemove = db.TaskLists.FirstOrDefault(n => n.Id.Equals(Id));
            using (db)
            {
                try
                {
                    db.Tasks.RemoveRange(db.Tasks.Where(t => t.ListId.Equals(Id)));

                    var removedAppointments = db.Appointments.Where(t => t.ListId.Equals(Id)).ToList();
                   // db.Atendees.RemoveRange(db.Atendees.Where(a => removedAppointments.Contains(a.ItemId);
                    db.Appointments.RemoveRange(removedAppointments);
                    db.TaskLists.Remove(listToRemove);

                    await db.SaveChangesAsync();
                } catch( Exception e) { }
            }
            return listToRemove;
        }

        public async Task<NamedList<Item>> GetList(Guid Id, bool getComplete)
        {
            using (db)
            {
                NamedListDB listToGet = await db.TaskLists.FirstOrDefaultAsync(n => n.Id.Equals(Id));
                NamedList<Item> listToReturn = new NamedList<Item>(listToGet);
                try
                {
                    listToReturn.list = new List<Item>();
                    var tasks = db.Tasks.Where(t => t.ListId.Equals(Id)).Select(t => new Library.TaskManagement.Task(t)).ToList();
                    var appts = db.Appointments.Where(t => t.ListId.Equals(Id)).Select(t => new Appointment(t)).ToList();
                    appts.ForEach(a => a.Atendees.AddRange(getAtendees(a.Id)));
                    listToReturn.list.AddRange(tasks);
                    listToReturn.list.AddRange(appts);

                    if (!getComplete)
                    {
                        listToReturn.list = listToReturn.list.Where(i => i.IsCompleted.Equals(false)).ToList();
                    }
                    listToReturn.list = listToReturn.list.OrderByDescending(i => i.Priority.Equals(true)).ToList();
                    return listToReturn;
                }
                catch (Exception e) { return new NamedList<Item>(); }
            }
            
        }

        public async Task<TaskDB> AddOrUpdateTask(Guid Id, Library.TaskManagement.Task task)
        {
            TaskDB newTask = null;
            using (db)
            {
                if (task.Id == Guid.Empty)
                {
                    newTask = new TaskDB(task, Id);
                    db.Tasks.Add(newTask);
                    await db.SaveChangesAsync();
                }
                else
                {
                    newTask = new TaskDB(task, Id);
                    var oldTask = db.Tasks.FirstOrDefault(t => t.Id.Equals(task.Id));
                    db.Tasks.Remove(oldTask);
                    db.Tasks.Add(newTask);
                    await db.SaveChangesAsync();
                }
            }
            return newTask;
        }

        public async Task<AppointmentDB> AddOrUpdateAppointment(Guid Id, Appointment appt)
        {
            AppointmentDB newAppt = null;
            using (db)
            {
                if (appt.Id == Guid.Empty)
                {
                    newAppt = new AppointmentDB(appt, Id);
                    await db.Atendees.AddRangeAsync(appt.Atendees.Select(a => new AtendeeDB(a, newAppt.Id)));
                    await db.Appointments.AddAsync(newAppt);
                    await db.SaveChangesAsync();
                }
                else
                {
                    newAppt = new AppointmentDB(appt, Id);
                    var oldAppt = db.Appointments.FirstOrDefault(t => t.Id.Equals(appt.Id));
                    db.Atendees.RemoveRange(db.Atendees.Where(a => a.ItemId.Equals(oldAppt.Id)));
                    db.Appointments.Remove(oldAppt);
                    await db.Atendees.AddRangeAsync(appt.Atendees.Select(a => new AtendeeDB(a, newAppt.Id)));
                    await db.Appointments.AddAsync(newAppt);
                    await db.SaveChangesAsync();
                }
            }
            return newAppt;
        }

        public List<Item> Search(string query)
        {
            List<Item> results = new List<Item>();
            using (db)
            {
                var taskDBs = (from task in db.Tasks.ToList() where task.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) || task.Description.Contains(query, StringComparison.InvariantCultureIgnoreCase) select task).ToList();
                results.AddRange(taskDBs.Select(t => new Library.TaskManagement.Task(t)));
                var apptDBs = (from appt in db.Appointments.ToList() where appt.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) || appt.Description.Contains(query, StringComparison.InvariantCultureIgnoreCase) select appt).ToList();
                results.AddRange(apptDBs.Select(a => new Appointment(a)));

                var atendeeMatch = db.Atendees.ToList().Where(a => a.name.Contains(query, StringComparison.InvariantCultureIgnoreCase)).ToList();
                var atendeeApptIDs = atendeeMatch.Select(a => a.ItemId).Distinct().ToList();
                var atendeeAppts = db.Appointments.Where(a => atendeeApptIDs.Contains(a.Id)).ToList();
                results.AddRange(atendeeAppts.Select(a => new Appointment(a)));

                foreach(Item item in results)
                {
                    if (item is Appointment)
                        (item as Appointment).Atendees.AddRange(getAtendees(item.Id));
                }
            }
            return results.Distinct().ToList();
        }

        public async Task<Item> RemoveItem(Guid ItemId)
        {
            Item RemovedItem = null;
            using (db)
            {
                if (await db.Tasks.AnyAsync(t => t.Id.Equals(ItemId)))
                {
                    var DBTask = await db.Tasks.FirstOrDefaultAsync(t => t.Id.Equals(ItemId));
                    db.Tasks.Remove(DBTask);
                    RemovedItem = new Library.TaskManagement.Task(DBTask);
                }
                else if (await db.Appointments.AnyAsync(t => t.Id.Equals(ItemId)))
                {
                    var DBAppt = await db.Appointments.FirstOrDefaultAsync(a => a.Id.Equals(ItemId));
                    db.Appointments.Remove(DBAppt);
                    db.Atendees.RemoveRange(db.Atendees.Where(a => a.ItemId.Equals(DBAppt.Id)));
                    RemovedItem = new Appointment(DBAppt);
                }
                await db.SaveChangesAsync();
                return RemovedItem;    
            }
        }

        private List<string> getAtendees(Guid Id)
        {
            return db.Atendees.Where(a => a.ItemId.Equals(Id)).Select(at => at.name).ToList();
        }

    }
}
