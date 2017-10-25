using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kassasysteem
{
    public sealed partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            tbFocus.Focus(FocusState.Programmatic);
            //tbFocus.Visibility = Visibility.Collapsed;
        }

        private void BtExit_OnClick(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private async void BtSearch_OnClick(object sender, RoutedEventArgs e)
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

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Focus(FocusState.Programmatic);
        }
    }
}