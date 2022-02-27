/*
    Copyright © 2017-2018 Deepak Rathi 
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System.Windows;
using System.Windows.Controls;

namespace VS2017OfflineSetupUtility.Views
{
    /// <summary>
    /// Interaction logic for CleanUtilPage.xaml
    /// </summary>
    public partial class CleanUtilPage : Page
    {
        public CleanUtilPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var viewModel = (ViewModels.CleanUtilPageViewModel)DataContext;

            if (App.AutoCleanup)
            {
                viewModel.SelectedFolderPath = App.AutoCleanupFolder;
                viewModel.DoClassification();
                viewModel.DeleteOldVersionCommand.Execute();
            }
            else
            {
                viewModel?.DoClassification();
            }
        }
    }
}
