using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ThinqAClient.Browser
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string ReturnValue { get; private set; }
        string url;
        public LoginWindow(string URL)
        {
            InitializeComponent();
            browser.LoadCompleted += Browser_LoadCompleted;
            Loaded += LoginWindow_Loaded;
            url = URL;
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            browser.Navigate(url);
        }

        private void Browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (browser.Source.ToString().Contains("iabClose"))
            {
                ReturnValue = browser.Source.ToString();
                DialogResult = true;
            }
        }
    }
}
