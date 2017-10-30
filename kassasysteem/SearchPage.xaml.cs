using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using kassasysteem.Classes;

namespace kassasysteem
{
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void btSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchItems = await Rest.getItems("", tbName.Text, tbCode.Text);
            if (lvSearchResults.Items == null) return;
            lvSearchResults.Items.Clear();
            foreach (var item in searchItems)
                lvSearchResults.Items?.Add(item);
        }

        private void lvSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}