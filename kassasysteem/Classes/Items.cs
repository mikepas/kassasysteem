namespace kassasysteem.Classes
{
    internal class Items
    {
        public string Amount = "";
        public string Code = "";
        public string SalesPrice = "";
        public string Description = "";
        public string ID = "";
        public string Stock = "";

        public Items(string i, string c, string d, string cp, string a, string s)
        {
            ID = i;
            Code = c;
            Description = d;
            SalesPrice = cp;
            Amount = a;
            Stock = s;
        }

        public Items()
        {
        }

        public override string ToString()
        {
            return $"{Description}";
        }
    }
}