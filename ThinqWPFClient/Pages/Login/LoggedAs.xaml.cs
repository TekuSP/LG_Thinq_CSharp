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
    /// Interaction logic for LoggedAs.xaml
    /// </summary>
    public partial class LoggedAs : Page
    {
        MainWindow main;
        UserSaveInfo save;
        public LoggedAs(MainWindow mainWindow, UserSaveInfo root)
        {
            InitializeComponent();
            main = mainWindow;
            userName.Text = "Currently is logged in " + root.UserName;
            save = root;
        }

        private void LoginNew_Click(object sender, RoutedEventArgs e)
        {
            main.ClearSavedData();
            main.GoToNotLogged();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            main.GoToLogginIn(save.TokensInfo);
        }
    }
}
