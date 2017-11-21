using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
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
    // Aanmeldknop aanmaken en zorgen dat een cassiere zich kan aanmelden

    public sealed partial class Dashboard : Page
    {
        private bool _setFocus = true;
        private readonly List<float> _totalCost = new List<float>();
        private int _selectedSearchOption = 1;
        private string _cassiereName = "";

        public Dashboard()
        {
            InitializeComponent();
            ApplicationLanguages.PrimaryLanguageOverride = "nl";
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await SetItemGroups();
            //await OpenCustomerPage();
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

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        /*private async void BtSearch_OnClick(object sender, RoutedEventArgs e)
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

            tbFocus.Focus(FocusState.Programmatic);
        }*/

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
        }

        /*private void lvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (ListView)sender;
            if (!(lv.SelectedItem is Items selectedItem)) return;
            var description = selectedItem.Description;
            var costPrice = selectedItem.CostPriceStandard;
            OrderItems._orderItems.Add(new OrderItems { Description = description, Amount = "1", CostPriceStandard = costPrice });
            lvOrderItems.ItemsSource = OrderItems._orderItems;
        }*/

        private void lvItems_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var item = (ListView)sender;
            if (!(item.SelectedItem is Items selectedItem)) return;
            var description = selectedItem.Description;
            var costPrice = float.Parse(selectedItem.CostPriceStandard, CultureInfo.InvariantCulture.NumberFormat);
            _totalCost.Add(costPrice);

            // werkt nog niet
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
            _totalCost.Clear();
            tbTotal.Text = _totalCost.Sum().ToString("c2");
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btCheckOut_Click(object sender, RoutedEventArgs e)
        {
            // Laat een window zien met alle orderItems en of ze kortingspunten willen invoeren en laat ze kiezen tussen 'afrekenen' of 'annuleren'
            var orderItems = lvOrderItems.Items;
            tbFocus.Focus(FocusState.Programmatic);
        }

        private void btRetour_Click(object sender, RoutedEventArgs e)
        {
            // laat een window zien met wat je retourt en laat ze kiezen tussen 'retourneren' of 'annuleren'
            tbFocus.Focus(FocusState.Programmatic);
        }

        public void UpdateOrderItems()
        {
            lvOrderItems.ItemsSource = OrderItems._orderItems;
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

        private async void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            switch (tbSearch.Text)
            {
                case "Wat wilt u zoeken?":
                    return;
                case "":
                    return;
            }
            List<Items> items;
            if (_selectedSearchOption == 1)
            {
                items = await Rest.getItems("", tbSearch.Text);
                lvItems.Items?.Clear();
                foreach (var item in items)
                {
                    lvItems.Items?.Add(item);
                }
            }
            else if (_selectedSearchOption == 2)
            {
                items = await Rest.getItems("", "", "", tbSearch.Text);
                lvItems.Items?.Clear();
                foreach (var item in items)
                {
                    lvItems.Items?.Add(item);
                }
            }
        }

        private void tbtName_Click(object sender, RoutedEventArgs e)
        {
            btName.Background = new SolidColorBrush(Colors.YellowGreen);
            btBarcode.Background = new SolidColorBrush(Colors.LightGray);
            _selectedSearchOption = 1;
        }

        private void tbtBarcode_Click(object sender, RoutedEventArgs e)
        {
            btBarcode.Background = new SolidColorBrush(Colors.YellowGreen);
            btName.Background = new SolidColorBrush(Colors.LightGray);
            _selectedSearchOption = 2;

        }

        private void tbFocus_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Zoek het item dat overeenkomt met de barcode en zet het in lvOrderItems en maak de textbox weer leeg
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = false;
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _setFocus = true;
        }

        private void btEntry_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EntryPage));
            /*var newView = CoreApplication.CreateNewView();
            var newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(EntryPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);*/
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _cassiereName = e.Parameter as string;
            if (_cassiereName != null) tbCassiereName.Text += " " + _cassiereName;
        }
    }
}