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

            if (viewModel.SelectedVsEdition == null)
                viewModel.SelectedVsEdition = viewModel.AllVisualStudioEditions.FirstOrDefault( x=>x.Version.Equals("17C"));

            viewModel.GenerateCli(viewModel.SelectedVsEdition);
        }
    }
}
