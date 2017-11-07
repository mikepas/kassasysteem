using System;
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

        public Dashboard()
        {
            InitializeComponent();
        }

        private static async Task OpenCustomerPage()
        {
            var newView = CoreApplication.CreateNewView();
            var newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(CustomerPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async Task SetItemGroups()
        {
            var itemGroups = await Rest.getItemGroups();
            foreach (var itemGroup in itemGroups)
            {
                lvItemGroups.Items?.Add(itemGroup);
            }
            lvItemGroups.SelectedIndex = 0;
        }

        private async Task SetItems(string itemGroup)
        {
            var items = await Rest.getItems(itemGroup);
            if (lvItems.Items != null)
            {
                lvItems.Items.Clear();
                foreach (var item in items)
                {
                    lvItems.Items.Add(item);
                }
            }
        }

        private void BtExit_OnClick(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void BtSearch_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSearchPage();
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_setFocus)
            {
                tbFocus.Focus(FocusState.Programmatic);
            }
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = false;
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = true;
        }

        private static async void OpenSearchPage()
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

        private async void lvItemGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedItem != null) await SetItems(lv.SelectedItem.ToString());
        }

        private void lvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (ListView)sender;
            if (!(lv.SelectedItem is Items selectedItem)) return;
            var description = selectedItem.Description;
            var costPrice = selectedItem.CostPriceStandard;
            OrderItems._orderItems.Add(new OrderItems { Description = description, Amount = "1", CostPriceStandard = costPrice });
            lvOrderItems.ItemsSource = OrderItems._orderItems;
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrderItems.SelectedItem == null)
            {
                OrderItems._orderItems?.Clear();
            }
            else
            {
                OrderItems._orderItems?.Remove(lvOrderItems.SelectedItem as OrderItems);
            }
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btCheckOut_Click(object sender, RoutedEventArgs e)
        {
            //var order = lvOrderItems.Items;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btRetour_Click(object sender, RoutedEventArgs e)
        {
            tbFocus.Focus(FocusState.Programmatic);
        }

        public void UpdateOrderItems()
        {
            lvOrderItems.ItemsSource = OrderItems._orderItems;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await SetItemGroups();
            await OpenCustomerPage();
        }
    }
}