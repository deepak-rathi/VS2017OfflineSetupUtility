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
using System.Windows.Media;
using VS2017OfflineSetupUtility.Controls;
using VS2017OfflineSetupUtility.Models;

namespace VS2017OfflineSetupUtility.Utils
{
    internal static class FeatureUtil
    {
        //TODO://Convert class to non-static and use Unity Container to create instance/objects
        internal static List<Feature> GetFeatures()
        {
            //Static Data
            #region Clean Util Feature
            var cleanUtilIcon = new CleanUtilIcon() { IsCheckedColor = (SolidColorBrush)App.Current.Resources["BlueSolidColorBrush"]};
            cleanUtilIcon.SetBinding(CleanUtilIcon.IsCheckedProperty, new System.Windows.Data.Binding("IsSelected") { UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged});

            var cleanUtilFeature = new Feature() { Icon =  cleanUtilIcon, Name = "VS2017 Offline Clean Util", About= "Allow deletion of old version Visual Studio 2017 Offline Setup files and folder, which saves your hard disk space.", Version="1.3.0.0", NavigateToView = new Views.CleanUtilPage() , IsSelected = true};

            #endregion

            #region Download Util Feature
            var downloadUtilIcon = new DownloadUtilIcon() { IsCheckedColor = (SolidColorBrush)App.Current.Resources["BlueSolidColorBrush"]};
            downloadUtilIcon.SetBinding(DownloadUtilIcon.IsCheckedProperty, new System.Windows.Data.Binding("IsSelected") { UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged });

            var downloadUtilFeature = new Feature() { Icon = downloadUtilIcon, Name = "VS2017 Offline Download", About = "Download Visual Studio 2017 Offline Setup files and folder, using command line interface. (Need internet connection)", Version = "1.3.0.0" , NavigateToView  = new Views.DownloadUtilPage()};

            #endregion

            return new List<Feature>() { cleanUtilFeature, downloadUtilFeature  };
        }
    }
}
