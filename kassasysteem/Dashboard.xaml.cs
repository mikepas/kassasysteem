using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kassasysteem
{
    public sealed partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void BtExit_OnClick(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }
    }
}