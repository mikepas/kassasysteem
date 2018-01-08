using System;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using kassasysteem.Classes;

namespace kassasysteem
{
    public sealed partial class ContantPage : Page
    {
        private string _totaal;
        public ContantPage()
        {
            InitializeComponent();
        }

        private async void btOk_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var messageDialogBevestiging = new MessageDialog("Betaling afgerond!", "Bevestiging");
            await messageDialogBevestiging.ShowAsync();
            CreatePrintPdf.CreateReceipt();
            Frame.Navigate(typeof(Dashboard));
        }

        private void tbOntvangen_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;
            //var totaalReplace = _totaal.Replace(".", ",");
            var ontvangen = double.Parse(tbOntvangen.Text);
            var totaal = double.Parse(_totaal);
            var teruggeven = ontvangen - totaal;
            tbTeruggeven.Text = teruggeven.ToString("C2");
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            var totaal = e.Parameter as string;
            tbTotaal.Text = totaal ?? throw new InvalidOperationException();
            var totaalReplace = totaal.Replace("€ ", "");
            _totaal = totaalReplace;
        }
    }
}
