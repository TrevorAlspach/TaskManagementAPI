using Library.TaskManagement.TaskManagementDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement
{
    public class NamedList<T> //Used in order to display names of list in listview
    {
        public string name { get; set; }
        public Guid Id { get; set; }
        public List<T> list { get; set; }


        public NamedList(string name)
        {
            this.name = name;
            this.list = new List<T>();
            Id = Guid.NewGuid();
        }

        public NamedList(string name, List<T> list)
        {
            this.name = name;
            this.list = new List<T>(list);
            this.list.AddRange(list);
            Id = Guid.NewGuid();
        }

        public NamedList(NamedList<T> namedList)
        {
            this.list = new List<T>(namedList.list);
            this.name = name;
            Id = namedList.Id;
        }

        public NamedList(NamedListDB namedListDB)
        {
            this.name = namedListDB.name;
            this.Id = namedListDB.Id;
            list = null;
        }

        public NamedList() 
        {
            this.list = new List<T>();
        }

        public override string ToString()
        {
            return name;
        }
    }
}
