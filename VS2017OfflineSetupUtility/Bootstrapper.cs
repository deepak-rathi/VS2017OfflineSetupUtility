using Prism.Unity;
using System.Windows;
using VS2017OfflineSetupUtility.Views;

namespace VS2017OfflineSetupUtility
{
    class Bootstrapper: UnityBootstrapper
    {
        /// <summary>
        /// The shell object
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<MainWindow>();
        }

        /// <summary>
        /// Initialize shell (MainWindow)
        /// </summary>
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
