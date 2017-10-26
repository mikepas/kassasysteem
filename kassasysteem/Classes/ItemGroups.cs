using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kassasysteem.Classes
{
    class ItemGroups
    {
        public string ID = "";
        public string Code = "";
        public string Description = "";

        public ItemGroups(string i, string c, string d)
        {
            ID = i;
            Code = c;
            Description = d;
        }

        public ItemGroups() { }

        public override string ToString()
        {
            return $"{Description}";
        }
    }
}
