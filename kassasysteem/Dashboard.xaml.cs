﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Globalization;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using kassasysteem.Classes;

namespace kassasysteem
{
    public sealed partial class Dashboard : Page
    {
        private bool _setFocus = true;
        private readonly List<float> _totalCost = new List<float>();
        private int _selectedSearchOption = 1;
        private int _selectedSaleRetour = 1;
        private string _cassiereName = "";

        public Dashboard()
        {
            InitializeComponent();
            ApplicationLanguages.PrimaryLanguageOverride = "nl";
            tbTotal.Text = _totalCost.Sum().ToString("c2");
            tbFocus.Focus(FocusState.Programmatic);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await SetItemGroups();
        }

        private async Task SetItemGroups()
        {
            var itemGroups = await Rest.getItemGroups();
            foreach (var itemGroup in itemGroups)
            {
                lvItemGroups.Items?.Add(itemGroup);
            }
            lvItemGroups.SelectedIndex = 0;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private async Task SetItems(string itemGroup)
        {
            var priceItems = new List<object>();
            var items = await Rest.getItems(itemGroup);
            if (lvItems.Items != null)
            {
                lvItems.Items.Clear();
                imgLoading.Visibility = Visibility.Visible;
                foreach (var item in items)
                {
                    var salesPrice = await Rest.getItemPrice(item.ID);
                    if (salesPrice == "")
                    {
                        salesPrice = "0";
                    }
                    item.SalesPrice = salesPrice;
                    priceItems.Add(item);
                }
                imgLoading.Visibility = Visibility.Collapsed;
                foreach (var item in priceItems)
                {
                    lvItems.Items.Add(item);
                }
            }
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_setFocus)
            {
                tbFocus.Focus(FocusState.Programmatic);
            }
        }

        private async void lvItemGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedItem != null) await SetItems(lv.SelectedItem.ToString());
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void lvItems_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var item = (ListView)sender;
            if (!(item.SelectedItem is Items selectedItem)) return;
            var description = selectedItem.Description;
            var costPrice = float.Parse(selectedItem.SalesPrice, CultureInfo.InvariantCulture.NumberFormat);
            _totalCost.Add(costPrice);
            
            if (lvOrderItems.Items != null && lvOrderItems.Items.Contains(description))
            {
                OrderItems._orderItems.Add(new OrderItems
                {
                    Description = description,
                    Amount = "2",
                    CostPriceStandard = costPrice.ToString("C2")
                });
            }
            else
            {
                OrderItems._orderItems.Add(new OrderItems
                {
                    Description = description,
                    Amount = "1", 
                    CostPriceStandard = costPrice.ToString("C2")
                });
            }

            lvOrderItems.ItemsSource = OrderItems._orderItems;
            tbTotal.Text = _totalCost.Sum().ToString("c2");
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrderItems.SelectedItem == null)
            {
                OrderItems._orderItems?.Clear();
                _totalCost.Clear();
            }
            else
            {
                if (lvOrderItems.SelectedItem is OrderItems orderItem)
                {
                    orderItem.CostPriceStandard = "-" + orderItem.CostPriceStandard.Replace("€ ", "").Replace(",00", "");
                    var costPrice = float.Parse(orderItem.CostPriceStandard, CultureInfo.InvariantCulture.NumberFormat);
                    _totalCost.Add(costPrice);
                    OrderItems._orderItems?.Remove(orderItem);
                }
            }
            tbTotal.Text = _totalCost.Sum().ToString("c2");
            tbFocus.Focus(FocusState.Programmatic);
        }

        private async void btCheckOut_Click(object sender, RoutedEventArgs e)
        {
            switch (_selectedSaleRetour)
            {
                case 1:
                    IsEnabled = false;
                    var salesItems = lvOrderItems.Items;
                    var newView = CoreApplication.CreateNewView();
                    var newViewId = 0;
                    await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        var frame = new Frame();
                        frame.Navigate(typeof(CheckoutPage), salesItems);
                        Window.Current.Content = frame;
                        Window.Current.Activate();

                        newViewId = ApplicationView.GetForCurrentView().Id;
                    });
                    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
                    Frame.Navigate(typeof(Dashboard));
                    break;
                case 2:
                    IsEnabled = false;
                    var messageDialog = new MessageDialog("Neem het product aan en geef " + _totalCost.Sum().ToString("c2") + " terug.", "Retour");
                    await messageDialog.ShowAsync();

                    //add SalesOrder
                    //after succes show messageDialog

                    Frame.Navigate(typeof(Dashboard));
                    break;
            }
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btRetour_Click(object sender, RoutedEventArgs e)
        {
            OrderItems._orderItems?.Clear();
            _totalCost.Clear();
            tbTotal.Text = _totalCost.Sum().ToString("c2");
            btRetour.Background = new SolidColorBrush(Colors.YellowGreen);
            btVerkoop.Background = new SolidColorBrush(Colors.LightGray);
            _selectedSaleRetour = 2;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btVerkoop_Click(object sender, RoutedEventArgs e)
        {
            OrderItems._orderItems?.Clear();
            _totalCost.Clear();
            tbTotal.Text = _totalCost.Sum().ToString("c2");
            btVerkoop.Background = new SolidColorBrush(Colors.YellowGreen);
            btRetour.Background = new SolidColorBrush(Colors.LightGray);
            _selectedSaleRetour = 1;
            tbFocus.Focus(FocusState.Programmatic);
        }

        public void UpdateOrderItems()
        {
            lvOrderItems.ItemsSource = OrderItems._orderItems;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = "";
        }

        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = "Wat wilt u zoeken?";
            tbFocus.Focus(FocusState.Programmatic);
        }

        private async void tbSearch_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (tbSearch.Text)
            {
                case "Wat wilt u zoeken?":
                    return;
                case "":
                    return;
            }
            if (e.Key != VirtualKey.Enter) return;
            lvItems.Items?.Clear();
            lvItemGroups.SelectedItem = null;
            List<Items> items;
            var priceItems = new List<object>();
            imgLoading.Visibility = Visibility.Visible;
            if (_selectedSearchOption == 1)
            {
                items = await Rest.getItems("", tbSearch.Text);
                foreach (var item in items)
                {
                    var salesPrice = await Rest.getItemPrice(item.ID);
                    if (salesPrice == "")
                    {
                        salesPrice = "0";
                    }
                    item.SalesPrice = salesPrice;
                    priceItems.Add(item);
                }
                foreach (var priceItem in priceItems)
                {
                    lvItems.Items?.Add(priceItem);
                }
            }
            else if (_selectedSearchOption == 2)
            {
                items = await Rest.getItems("", "", tbSearch.Text);
                foreach (var item in items)
                {
                    var salesPrice = await Rest.getItemPrice(item.ID);
                    if (salesPrice == "")
                    {
                        salesPrice = "0";
                    }
                    item.SalesPrice = salesPrice;
                    priceItems.Add(item);
                }
                foreach (var priceItem in priceItems)
                {
                    lvItems.Items?.Add(priceItem);
                }
            }
            imgLoading.Visibility = Visibility.Collapsed;
        }

        private void tbtName_Click(object sender, RoutedEventArgs e)
        {
            btName.Background = new SolidColorBrush(Colors.YellowGreen);
            btBarcode.Background = new SolidColorBrush(Colors.LightGray);
            _selectedSearchOption = 1;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void tbtBarcode_Click(object sender, RoutedEventArgs e)
        {
            btBarcode.Background = new SolidColorBrush(Colors.YellowGreen);
            btName.Background = new SolidColorBrush(Colors.LightGray);
            _selectedSearchOption = 2;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = false;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = true;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btEntry_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EntryPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _cassiereName = e.Parameter as string;
            if (_cassiereName != null) tbCassiereName.Text += " " + _cassiereName;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void BtInleveren_OnClick(object sender, RoutedEventArgs e)
        {
            tbFocus.Focus(FocusState.Programmatic);
        }

        private async void tbFocus_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;
            var scannedNumber = tbFocus.Text;
            tbFocus.Text = "";
            var priceItems = new List<object>();
            var items = await Rest.getItems("", "", scannedNumber);
            foreach (var item in items)
            {
                var salesPrice = await Rest.getItemPrice(item.ID);
                if (salesPrice == "")
                {
                    salesPrice = "0";
                }
                item.SalesPrice = salesPrice;
                priceItems.Add(item);
            }
            foreach (Items item in priceItems)
            {
                var costPrice = float.Parse(item.SalesPrice, CultureInfo.InvariantCulture.NumberFormat);
                _totalCost.Add(costPrice);
                OrderItems._orderItems.Add(new OrderItems
                {
                    Description = item.Description,
                    Amount = "1",
                    CostPriceStandard = costPrice.ToString("C2")
                });
            }
            lvOrderItems.ItemsSource = OrderItems._orderItems;
            tbTotal.Text = _totalCost.Sum().ToString("c2");
            tbFocus.Focus(FocusState.Programmatic);
        }

        private async void tbClient_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (tbClient.Text == "") tbKorting.Text = "Kortingspunten: Voer de klant in";
            if (e.Key != VirtualKey.Enter) return;
            tbFocus.Text = "";
            var customers = await Rest.getCustomers(tbClient.Text);
            foreach (var customer in customers)
            {
                tbClient.Text = customer.Name;
            }
            var kortingsPunten = "0";
            tbKorting.Text = "Kortingspunten: " + kortingsPunten;
        }
    }
}