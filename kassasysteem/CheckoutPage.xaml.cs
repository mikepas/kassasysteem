using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using kassasysteem.Classes;

namespace kassasysteem
{
    public sealed partial class CheckoutPage : Page
    {
        private string _totalCost;

        public CheckoutPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _totalCost = e.Parameter.ToString();
        }

        private void BtContant_OnClick(object sender, RoutedEventArgs e)
        {
            //add SalesOrder
            //var messageDialog = new MessageDialog("Totaal: " + _totalCost, "Contant betalen");
            //await messageDialog.ShowAsync();
            Frame.Navigate(typeof(ContantPage), _totalCost);
        }
        
        private async void BtPinnen_OnClick(object sender, RoutedEventArgs e)
        {
            //add SalesOrder
            var messageDialog = new MessageDialog(_totalCost, "Betalen via pin");
            await messageDialog.ShowAsync();
            var messageDialogBevestiging = new MessageDialog("Betaling afgerond!", "Bevestiging");
            await messageDialogBevestiging.ShowAsync();
            CreatePrintPdf.CreateReceipt(Dashboard._cassiereName, Dashboard._number);
            Frame.Navigate(typeof(Dashboard));
        }
    }
}