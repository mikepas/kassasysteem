﻿using System;
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
            getAuthorization();
        }

        private void getAuthorization()
        {
            string request = Constants.BASE_URI + "/api/oauth2/auth" +
                            "?client_id={" + Constants.CLIENT_ID + "}" +
                            "&redirect_uri=" + Constants.CALLBACK_URL +
                            "&state=" + Constants.CLIENT_STATE +
                            "&response_type=code";

            webBrowser.Source = new Uri(request);
            webBrowser.Focus(FocusState.Programmatic);
        }

        private void GetCode(string url)
        {
            if (url.IndexOf(Constants.BASE_URI) < 0)
            {
                int c = url.IndexOf("?code=");
                int s = url.IndexOf("&state=");
                OAuth.Code = url.Substring(c + 6, s - c - 6);
                OAuth.State = url.Substring(s + 7);
                Frame.Navigate(typeof(Dashboard), 0);
            }
        }

        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            GetCode(args.Uri.ToString());
            string script = "document.documentElement.style.overflow ='hidden';"
                            + "document.body.style.backgroundColor = 'green';"
                            + "document.getElementById('LoginForm').style.width='400px';"
                            + "document.getElementById('LoginForm').style.align='center';"
                            + "document.getElementById('LoginForm').style.marginTop='-100px';"
                            + "document.getElementById('LoginButton').style.background='white';"
                            + "document.getElementById('LoginButton').style.color='black';"
                            + "document.getElementsByClassName('enhanced-navigation__top')[0].style.visibility = 'hidden';"
                            + "document.getElementsByClassName('enhanced-navigation__bottom')[0].style.visibility = 'hidden';"
                ;
            try
            {
                WebView wb = (WebView)sender;
                await wb.InvokeScriptAsync("eval", new string[] { script });
            }
            catch { }
        }

        private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            GetCode(args.Uri.ToString());
        }
    }
}