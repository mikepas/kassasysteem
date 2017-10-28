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
        public string CostPriceStandard = "";

        public Items(string i, string c, string d, string cp)
        {
            ID = i;
            Code = c;
            Description = d;
            CostPriceStandard = cp;
        }

        public Items() { }

        public override string ToString()
        {
            return $"{Description}";
        }
    }
}
