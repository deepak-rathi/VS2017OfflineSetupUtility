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
using System.Windows.Controls;
using VS2017OfflineSetupUtility.Mvvm;

namespace VS2017OfflineSetupUtility.Models
{
    internal class Feature : BindableBase
    {
        public ContentControl Icon { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Version { get; set; }
        public Page NavigateToView { get; set; }

        #region IsSelected
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
        #endregion

        #region IsEnabled
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }
        #endregion
    }
}
