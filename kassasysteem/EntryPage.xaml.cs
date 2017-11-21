using System;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace kassasysteem
{
    public sealed partial class EntryPage : Page
    {
        public EntryPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (tbName.Text != "")
            {
                Frame.Navigate(typeof(Dashboard), tbName.Text);
            }
            else
            {
                var messageDialog = new MessageDialog("Je naam mag niet leeg zijn!");
                await messageDialog.ShowAsync();
            }
        }

        private void tbName_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Button_Click(this, new RoutedEventArgs());
            }
        }
    }
}
