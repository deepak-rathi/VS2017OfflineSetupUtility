using System.Windows;
using System.Windows.Controls;

namespace VS2017OfflineSetupUtility
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //TODO://Use proper Naivgation Service. Due to lack of time I am using this variable, it will  be removed in future.
        public static Frame CurrentFrame;
        public static bool AutoCleanup;
        public static string AutoCleanupFolder;
    }
}
