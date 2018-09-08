using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.ActiveRecords
{
    public class Campo : Attribute
    {
        public string Name;

        public Campo(string name)
        {
            Name = name;
        }
    }
}
