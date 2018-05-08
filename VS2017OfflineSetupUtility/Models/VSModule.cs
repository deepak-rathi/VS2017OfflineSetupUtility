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
namespace VS2017OfflineSetupUtility.Models
{
    internal class VSModule
    {
        #region Name
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region Version
        private string _version;

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                VersionObject = new System.Version(value.Split('=')[1]);
            }
        }
        #endregion

        public string FullPath;
        public System.Version VersionObject { get; private set; }
    }
}
