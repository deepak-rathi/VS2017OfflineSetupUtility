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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VS2017OfflineSetupUtility.Models;
using VS2017OfflineSetupUtility.Mvvm;

namespace VS2017OfflineSetupUtility.ViewModels
{
    class HomePageViewModel : BindableBase
    {
        #region Selected Feature
        private Feature _selectedFeature;

        public Feature SelectedFeature
        {
            get { return _selectedFeature ??= Features.FirstOrDefault(); }
            set { SetProperty(ref _selectedFeature, value); }
        }

        #endregion Selected Feature

        #region Features
        private List<Feature> _features;

        public List<Feature> Features
        {
            get { return _features ??= Utils.FeatureUtil.GetFeatures(); }
            set { SetProperty(ref _features, value); }
        }
        #endregion

        #region FeatureClickedCommand
        private DelegateCommand<Feature> _featureClickedCommand;

        public DelegateCommand<Feature> FeatureClickedCommand
        {
            get
            {
                return _featureClickedCommand ??= new DelegateCommand<Feature>((feature) => 
                { 
                    SelectedFeature.IsSelected = false;
                    SelectedFeature = feature; 
                    SelectedFeature.IsSelected = true;
                    ProceedCommand.Execute();
                });
            }
        }
        #endregion  FeatureClickedCommand

        #region Proceed Command
        private DelegateCommand _proceedCommand;

        public DelegateCommand ProceedCommand
        {
            get
            {
                return _proceedCommand ??= new DelegateCommand(() => { App.CurrentFrame.Navigate(Features.FirstOrDefault(feature => feature.IsSelected).NavigateToView); });
            }
        }
        #endregion  Proceed Command

        #region Exit Command
        private DelegateCommand _exitCommand;

        public DelegateCommand ExitCommand
        {
            get
            {
                return _exitCommand ??= new DelegateCommand(Application.Current.Shutdown);
            }
        }
        #endregion  Exit Command
    }
}
