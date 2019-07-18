using Newtonsoft.Json;
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
using ThinqAClient.Browser;
using ThinqAClient.Pages.Device;
using ThinqAClient.Pages.Login;
using LGThingApi.Structures;

namespace ThinqAClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded; ;
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadSavedData())
            {
                GoToLoggedAs();
            }
            else
            {
                GoToNotLogged();
            }
        }
        UserSaveInfo userInfo;
        public void ClearSavedData()
        {
            File.Delete("user.json");
            userInfo = null;
        }
        public bool LoadSavedData()
        {
            if (!File.Exists("user.json"))
                return false;
            try
            {
                string text = File.ReadAllText("user.json");
                userInfo = JsonConvert.DeserializeObject<UserSaveInfo>(text);
            }
            catch
            {
                MessageBox.Show("Nepodařilo se načíst data o uživateli!", "Pozor", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public void GoToDevicePage(Device[] indata)
        {
            _NavigationFrame.Navigate(new DeviceSelector(indata));
        }

        LoggingIn logIN;
        NotLoggedIn notIN;
        LoggedAs inAS;
        public void GoToLogginIn(AuthorizationStructure author)
        {
            logIN = new LoggingIn(this, author);
            _NavigationFrame.Navigate(logIN);
        }
        public void GoToNotLogged()
        {
            notIN = new NotLoggedIn(this);
            _NavigationFrame.Navigate(notIN);

        }
        public void GoToLoggedAs()
        {
            inAS = new LoggedAs(this, userInfo);
            _NavigationFrame.Navigate(inAS);

        }
    }
}
