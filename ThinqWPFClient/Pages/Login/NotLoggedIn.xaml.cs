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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThinqAClient.Browser;

namespace ThinqAClient.Pages.Login
{
    /// <summary>
    /// Interaction logic for NotLoggedIn.xaml
    /// </summary>
    public partial class NotLoggedIn : Page
    {
        MainWindow instance;
        public NotLoggedIn(MainWindow mainWindow)
        {
            InitializeComponent();
            instance = mainWindow;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            string url = "";
            try
            {
               await LGThingApi.Communication.LGGateway.OpenConnection("CZ", "cs-CZ");
                url = LGThingApi.Communication.LGGateway.LGOAuth.GetOauthUrl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
            LoginWindow login = new LoginWindow(url);
            login.ShowDialog();
            try
            {
                LGThingApi.Communication.LGGateway.LGOAuth.AuthorizeBasedOnOauth(login.ReturnValue);
                LGThingApi.Communication.LGGateway.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
            instance.GoToLogginIn(LGThingApi.Communication.LGGateway.LGOAuth.AuthorizationData);
        }
    }
}
