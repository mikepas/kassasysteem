using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using kassasysteem.Classes;

namespace kassasysteem
{
    public sealed partial class SearchPage : Page
    {
        //private readonly ObservableCollection<OrderItems> _orderItems;

        private readonly ObservableCollection<Items> _items;

        public SearchPage()
        {
            InitializeComponent();
            _items = new ObservableCollection<Items>();
            //OrderItems._orderItems = new ObservableCollection<OrderItems>();
        }

        private async void btSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchItems = await Rest.getItems("", tbName.Text, tbCode.Text);
            if (lvSearchResults.Items == null) return;
            _items.Clear();
            foreach (var item in searchItems)
            {
                _items.Add(new Items {Description = item.Description, Code = item.Code, CostPriceStandard = item.CostPriceStandard});
            }
            lvSearchResults.ItemsSource = _items;
        }

        private void lvSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (ListView)sender;
            if (!(lv.SelectedItem is Items selectedItem)) return;
            var description = selectedItem.Description;
            var costPrice = selectedItem.CostPriceStandard;
            OrderItems._orderItems.Add(new OrderItems { Description = description, Amount = "1", CostPriceStandard = costPrice });
            var dashboard = new Dashboard();
            dashboard.UpdateOrderItems();
        }

        public class Items
        {
            public string Description { get; set; }
            public string Code { get; set; }
            public string CostPriceStandard { get; set; }
        }
    }
}