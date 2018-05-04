using System.Windows;
using System.Windows.Controls;

namespace VS2017OfflineSetupUtility
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        //TODO://Use Prism Naivgation. Due to lack of time I am using this variable, it will  be removed in future.
        public static Frame CurrentFrame;
    }
}
