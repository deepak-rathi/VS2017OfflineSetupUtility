using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VS2017OfflineSetupUtility.Views
{
    /// <summary>
    /// Interaction logic for DownloadUtilPage.xaml
    /// </summary>
    public partial class DownloadUtilPage : Page
    {
        public DownloadUtilPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (this.DataContext as ViewModels.DownloadUtilPageViewModel);
            if (viewModel == null)
                return;

            viewModel.GenerateCli(viewModel.SelectedVsEdition);
        }
    }
}
