using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using kassasysteem.Classes;

namespace kassasysteem
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            GetAuthorization();
        }

        private void GetAuthorization()
        {
            var request = Constants.BASE_URI + "/api/oauth2/auth" +
                          "?client_id={" + Constants.CLIENT_ID + "}" +
                          "&redirect_uri=" + Constants.CALLBACK_URL +
                          "&state=" + Constants.CLIENT_STATE +
                          "&response_type=code";

            webBrowser.Source = new Uri(request);
            webBrowser.Focus(FocusState.Programmatic);
        }

        private async void GetCode(string url)
        {
            if (url.IndexOf(Constants.BASE_URI, StringComparison.Ordinal) >= 0) return;
            var c = url.IndexOf("?code=", StringComparison.Ordinal);
            var s = url.IndexOf("&state=", StringComparison.Ordinal);
            OAuth.Code = url.Substring(c + 6, s - c - 6);
            OAuth.State = url.Substring(s + 7);

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

            //Frame.Navigate(typeof(Dashboard), 0);
            Frame.Navigate(typeof(EntryPage));
        }

        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //GetCode(args.Uri.ToString());
            var script = "document.documentElement.style.overflow ='hidden';"
                         + "document.body.style.backgroundColor = 'YellowGreen';"
                         + "document.getElementById('LoginForm').style.width='400px';"
                         + "document.getElementById('LoginForm').style.align='center';"
                         + "document.getElementById('LoginForm').style.marginTop='-100px';"
                         + "document.getElementById('UserNameField').value='groenevingerserp';"
                         + "document.getElementById('PasswordField').value='H3tBl0kb0ek';"
                         + "document.getElementById('LoginButton').style.background='white';"
                         + "document.getElementById('LoginButton').style.color='black';"
                         + "document.getElementsByClassName('enhanced-navigation__top')[0].style.visibility = 'hidden';"
                         + "document.getElementsByClassName('enhanced-navigation__bottom')[0].style.visibility = 'hidden';"
                ;
            try
            {
                var wb = sender;
                await wb.InvokeScriptAsync("eval", new[] {script});
            }
            catch
            {
                // ignored
            }
        }

        private void WebBrowser_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            GetCode(args.Uri.ToString());
        }
    }
}