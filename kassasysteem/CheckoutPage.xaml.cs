using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace kassasysteem
{
    public sealed partial class CheckoutPage : Page
    {
        private List<object> _items = new List<object>();

        public CheckoutPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;
            _items = e.Parameter as List<object>;

        }
    }
}
