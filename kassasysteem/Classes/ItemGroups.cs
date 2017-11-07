namespace kassasysteem.Classes
{
    internal class ItemGroups
    {
        public string Code = "";
        public string Description = "";
        public string ID = "";

        public ItemGroups(string i, string c, string d)
        {
            ID = i;
            Code = c;
            Description = d;
        }

        public ItemGroups()
        {
        }

        public override string ToString()
        {
            return $"{Description}";
        }
    }
}