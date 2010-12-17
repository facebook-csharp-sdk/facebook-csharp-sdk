using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Facebook;

namespace IFramedInBrowser
{
    public partial class MainPage : UserControl
    {
        FacebookApp fbApp = null;

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        protected void MainPage_Loaded(object sender, EventArgs e)
        {
            var token = "";
            if (App.Current.Resources.Contains("token") && App.Current.Resources["token"] != null)
                token = App.Current.Resources["token"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                fbApp = new FacebookApp(token);
                InfoBox.Visibility = Visibility.Visible;

                // Making Facebook call here!
                fbApp.GetAsync("me", (val) =>
                {
                    if (val.Error == null)
                    {
                        var result = (IDictionary<string, object>)val.Result;
                        Dispatcher.BeginInvoke(() => InfoBox.ItemsSource = result);
                    }
                    else
                    {
                        // TODO: Need to let the user know there was an error
                        //failedLogin();
                    }
                });
            }
        }
    }
}