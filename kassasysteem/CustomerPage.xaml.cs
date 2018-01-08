using System;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kassasysteem
{
    public sealed partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            InitializeComponent();
            lvOrderItems.ItemsSource = Classes.OrderItems._orderItems;
        }
    }
}