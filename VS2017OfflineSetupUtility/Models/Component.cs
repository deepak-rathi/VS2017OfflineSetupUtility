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
using Newtonsoft.Json;

namespace VS2017OfflineSetupUtility.Models
{
    internal class Component
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Depedency Depedency { get; set; }

        [JsonIgnore]
        public Workload CommponentWorkload;

        [JsonIgnore]
        public string FullName { get { return Name + " - " + Depedency.ToString(); } }

        [JsonIgnore]
        public bool IsSelectable { get { return !(!IsSelfSelected & IsSelectedInWorkload()); } }

        [JsonIgnore]
        public bool IsSelected { get { return IsSelfSelected | IsSelectedInWorkload(); } set { if (IsSelectable) IsSelfSelected = value; } }

        [JsonIgnore]
        public bool IsSelfSelected { get; set; }

        public bool IsSelectedInWorkload()
        {
            switch (Depedency)
            {
                case Depedency.Required:
                    return CommponentWorkload.IsSelected;
                case Depedency.Recommended:
                    return CommponentWorkload.IsSelected & ComponentSetting.IsRecommended;
                case Depedency.Optional:
                    return CommponentWorkload.IsSelected & ComponentSetting.IsOptional;
                case Depedency.Independent:
                default:
                    return false;
            }
        }
    }
}
