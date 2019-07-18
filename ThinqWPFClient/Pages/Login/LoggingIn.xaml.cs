using System;
using System.Collections.Generic;
using System.IO;
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
using LGThingApi.Structures;

namespace ThinqAClient.Pages.Login
{
    /// <summary>
    /// Interaction logic for LoggingIn.xaml
    /// </summary>
    public partial class LoggingIn : Page
    {
        MainWindow main;
        public LoggingIn(MainWindow mainWindow, AuthorizationStructure userInfo)
        {
            InitializeComponent();
            main = mainWindow;
            LGThingApi.Communication.LGGateway.LGOAuth.AuthorizationData = userInfo;
            Loaded += LoggingIn_Loaded;
        }

        private async void LoggingIn_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeStatus("Openning connection...");
                await LGThingApi.Communication.LGGateway.OpenConnection("CZ", "cs-CZ");//Put here your country and language
                ChangeStatus("Checking user data...");
                await LGThingApi.Communication.LGGateway.LGOAuth.Login();
                ChangeStatus("Resetting token and saving...");
                await LGThingApi.Communication.LGGateway.LGOAuth.RefreshToken();
                File.WriteAllText("user.json",Newtonsoft.Json.JsonConvert.SerializeObject(new UserSaveInfo() { TokensInfo = LGThingApi.Communication.LGGateway.LGOAuth.AuthorizationData, UserName = LGThingApi.Communication.LGGateway.LgedmRoot.Account }));
                ChangeStatus("Loading your devices...");
                await LGThingApi.Communication.LGGateway.GetDevices();
                main.GoToDevicePage(LGThingApi.Communication.LGGateway.LgedmRoot.Item);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed, try again! {ex}", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                main.MainWindow_Loaded(this, new RoutedEventArgs());
                return;
            }

        }
        public void ChangeStatus(string text)
        {
            currentAction.Dispatcher.Invoke(() =>
            {
                currentAction.Text = text;
                total.Value += 25;
            });
        }
    }
}
