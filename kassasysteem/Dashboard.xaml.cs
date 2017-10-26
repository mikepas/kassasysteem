using System;
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
        private bool setFocus = true;

        public Dashboard()
        {
            InitializeComponent();
            tbFocus.Visibility = Visibility.Collapsed;
            //setItemGroups();
            setItems();
        }

        private async void setItemGroups()
        {
            var itemGroups = await Rest.getItemGroups();
            foreach (var itemGroup in itemGroups)
            {
                lvItemGroups.Items?.Add(itemGroup);
                lvItemGroups.SelectedIndex = 0;
            }
        }

        private async void setItems()
        {
            var items = await Rest.getItems();
            foreach (var item in items)
            {
                lvItems.Items?.Add(item);
                lvItems.SelectedIndex = 0;
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
            if (setFocus)
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
            setFocus = false;
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            setFocus = true;
        }
    }
}