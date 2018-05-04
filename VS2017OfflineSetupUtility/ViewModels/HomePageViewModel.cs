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
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace VS2017OfflineSetupUtility.ViewModels
{
    class HomePageViewModel : BindableBase
    {
        #region Proceed Command
        private DelegateCommand _proceedCommand;

        public DelegateCommand ProceedCommand
        {
            get
            {
                return _proceedCommand ?? (_proceedCommand = new DelegateCommand(() =>
                {
                    App.CurrentFrame.Navigate(new Views.CleanUtilPage());
                }));
            }
        }
        #endregion  Proceed Command

        #region Exit Command
        private DelegateCommand _exitCommand;

        public DelegateCommand ExitCommand
        {
            get
            {
                return _exitCommand ?? (_exitCommand = new DelegateCommand(() =>
                {
                    // Shutdown the application.
                    Application.Current.Shutdown();
                }));
            }
        }
        #endregion  Exit Command
    }
}
