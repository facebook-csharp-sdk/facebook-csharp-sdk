using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace CS_WinRT.Views
{
    public sealed partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FacebookLoginView).FullName);
        }
    }
}
