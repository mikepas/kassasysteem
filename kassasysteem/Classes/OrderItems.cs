using System.Collections.ObjectModel;
namespace kassasysteem.Classes
{
    internal class OrderItems
    {
        //public string Description;
        //public string Amount;
        //public string CostPriceStandard;
        public static ObservableCollection<OrderItems> _orderItems = new ObservableCollection<OrderItems>();
        public string Description { get; set; }
        public string Amount { get; set; }
        public string CostPriceStandard { get; set; }
        public string Code { get; set; }

        /*public OrderItems(string d, string a, string c)
        {
            Description = d;
            Amount = a;
            CostPriceStandard = c;
        }

        public OrderItems() {}*/
    }   
}
