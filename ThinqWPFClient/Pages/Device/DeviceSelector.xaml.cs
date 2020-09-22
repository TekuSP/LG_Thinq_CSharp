using LGThingApi.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ThinqAClient.Pages.Device
{
    /// <summary>
    /// Interaction logic for DeviceSelector.xaml
    /// </summary>
    public partial class DeviceSelector : Page
    {
        LGThingApi.Structures.Device[] devices;
        public DeviceSelector(LGThingApi.Structures.Device[] devices)
        {
            InitializeComponent();
            this.devices = devices;
            Loaded += DeviceSelector_Loaded;
        }

        private void DeviceSelector_Loaded(object sender, RoutedEventArgs e)
        {
           var wash = new Washer(devices.First());
           wash.StartMonitor(500);
            stack.Children.Add(new Devices.Washer() { DataContext = wash });
        }
    }
}
