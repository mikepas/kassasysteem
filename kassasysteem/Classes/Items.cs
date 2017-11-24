namespace kassasysteem.Classes
{
    internal class Items
    {
        public string Amount = "";
        public string Code = "";
        public string SalesPrice = "";
        public string Description = "";
        public string ID = "";

        public Items(string i, string c, string d, string cp, string a)
        {
            ID = i;
            Code = c;
            Description = d;
            SalesPrice = cp;
            Amount = a;
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