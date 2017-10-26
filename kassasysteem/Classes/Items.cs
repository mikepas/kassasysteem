using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kassasysteem.Classes
{
    class Items
    {
        public string ID = "";
        public string Code = "";
        public string Description = "";

        public Items(string i, string c, string d)
        {
            ID = i;
            Code = c;
            Description = d;
        }

        public Items() { }

        public override string ToString()
        {
            return $"{Description}";
        }
    }
}
