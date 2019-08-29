using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LexCoreLib;

namespace LexPhoneApp.Model
{
    public class DataItem
    {
        public DataItem(string title)
        {
            Title = title;
        }

        public List<Person> People { get; set; }

        public string Title
        {
            get;
            private set;
        }
    }
}
