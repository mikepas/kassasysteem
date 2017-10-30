using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using kassasysteem.Classes;

namespace kassasysteem
{
    public sealed partial class Dashboard : Page
    {
        private bool _setFocus = true;
        private readonly ObservableCollection<OrderItems> _orderItems;

        public Dashboard()
        {
            InitializeComponent();
            tbFocus.Visibility = Visibility.Collapsed;
            _orderItems = new ObservableCollection<OrderItems>();
            getData();
        }

        private async Task getData()
        {
            await setItemGroups();
        }

        private async Task setItemGroups()
        {
            var itemGroups = await Rest.getItemGroups();
            foreach (var itemGroup in itemGroups)
            {
                lvItemGroups.Items?.Add(itemGroup);
                lvItemGroups.SelectedIndex = 0;
            }
        }

        private async Task setItems(string itemGroup)
        {
            var items = await Rest.getItems(itemGroup);
            if (lvItems.Items != null)
            {
                lvItems.Items.Clear();
                foreach (var item in items)
                {
                    lvItems.Items?.Add(item);
                }
            }
        }

        private void BtExit_OnClick(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void BtSearch_OnClick(object sender, RoutedEventArgs e)
        {
            OpenNewWindow();
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_setFocus)
            {
                tbFocus.Focus(FocusState.Programmatic);
            }
        }

        private async void OpenNewWindow()
        {
            var newView = CoreApplication.CreateNewView();
            var newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(SearchPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = false;
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = true;
        }

        private async void lvItemGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedItem != null) await setItems(lv.SelectedItem.ToString());
        }

        private void lvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (!(lv.SelectedItem is Items selectedItem)) return;
            var description = selectedItem.Description;
            var costPrice = selectedItem.CostPriceStandard;
            _orderItems.Add(new OrderItems { Description = description, Amount = "1", CostPriceStandard = costPrice });
            lvOrderItems.ItemsSource = _orderItems;
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrderItems.SelectedItem == null)
            {
                _orderItems?.Clear();
            }
            else
            {
                _orderItems?.Remove(lvOrderItems.SelectedItem as OrderItems);
            }
            tbFocus.Focus(FocusState.Programmatic);
        }

        public class OrderItems
        {
            public string Description { get; set; }
            public string Amount { get; set; }
            public string CostPriceStandard { get; set; }
        }

        private void btCheckOut_Click(object sender, RoutedEventArgs e)
        {
            var order = lvOrderItems.Items;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btRetour_Click(object sender, RoutedEventArgs e)
        {
            tbFocus.Focus(FocusState.Programmatic);
        }
    }
}