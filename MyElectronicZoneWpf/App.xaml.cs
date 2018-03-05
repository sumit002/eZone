using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;

namespace MyElectronicZoneWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Create the SplashScreen Window
            SplashScreen splashScreen = new SplashScreen("Images/dashboard.ico");
            splashScreen.Show(true);

            // show for 2 seconds
            splashScreen.Close(new TimeSpan(0, 0, 2));
        }

    }
}
