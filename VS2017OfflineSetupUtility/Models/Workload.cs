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
using System.Collections.Generic;

namespace VS2017OfflineSetupUtility.Models
{
    internal class Workload
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<Component> Components { get; } = new List<Component>();

        //Custom fields
        [JsonIgnore]
        public string FullName { get { return Name; } }

        [JsonIgnore]
        public bool IsSelectable { get { return !string.IsNullOrEmpty(ID); } }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        [JsonIgnore]
        public bool IsExpanded { get; set; }
    }
}
