using System.Windows;

namespace E_Scooter
{
    public partial class App : Application
    {
            protected override void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);
                var splash = new SplashScreen();
                splash.Show();
            }

    }
}
