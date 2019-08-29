using Lex.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexCoreLib;

namespace LexCreateData
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new DbInstance("demo"))
            {
                db.Map<Person>().Automap(i => i.Id);
                db.Initialize();
                db.Save(new Person() { FirstName = "Ken", Id = 1, LastName = "Tucker" }, 
                    new Person() { FirstName = "Tony", Id = 2, LastName = "Stark" },
                    new Person() { FirstName = "John", Id = 3,LastName = "Papa" });
            }
        }
    }
}
