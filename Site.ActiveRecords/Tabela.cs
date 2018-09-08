using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.ActiveRecords
{
    public class Tabela : Attribute
    {
        public string Name;

        public Tabela(string name)
        {
            Name = name;
        }
    }
}
