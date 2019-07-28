/*
    Copyright © 2017-2019 Deepak Rathi 
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VS2017OfflineSetupUtility.Models
{
    internal class VsEdition
    {
        public string Name { get; set; }
        public string Version;
        public string SetupUri;
        public string WorkloadGitHubUri;
        public string WorkloadDocUri;
        public VsEditionWorkloads vsEditonWorkloads = new VsEditionWorkloads();
        public string CliToDownload, CliToInstall;

        public void LoadJSON()
        {
            var path = Environment.CurrentDirectory + "/" + Name + ".json";
            if (File.Exists(path))
                vsEditonWorkloads = JsonConvert.DeserializeObject<VsEditionWorkloads>(File.ReadAllText(path));

            // sync existing tree
            foreach (var load in vsEditonWorkloads.Workloads)
                foreach (var comp in load.Components)
                    comp.CommponentWorkload = load;

        }

        public void SaveAllWorkloadsToFile()
        {
            vsEditonWorkloads.WorkloadModifiedAt = DateTime.Now;
            var path = Environment.CurrentDirectory + "/" + Name + ".json";
            File.WriteAllText(path, JsonConvert.SerializeObject(vsEditonWorkloads, Formatting.Indented));
        }

        public void GenerateCLICommand(string vsEdition, string filePath, string language, bool IsRecommendedSelected, bool IsOptionalSelected)
        {
            var exe = vsEdition.Replace(' ', '_') + ".exe ";
            var layout = "--layout \"" + filePath + "\" ";
            var body = new List<string>();
            var suffix = "";
            var lang = "--lang " + language;
            foreach (var workload in vsEditonWorkloads.Workloads)
            {
                if (!string.IsNullOrEmpty(workload.ID) && workload.IsSelected)
                    body.Add("--add " + workload.ID + " ");

                foreach (var component in workload.Components.Where(comp => comp.IsSelfSelected && !comp.IsSelectedInWorkload()))
                    body.Add("--add " + component.ID + " ");
            }

            if (IsRecommendedSelected)
                suffix = "--includeRecommended " + suffix;

            if (IsOptionalSelected)
                suffix = "--includeOptional " + suffix;

            var bodystr = string.Join("", body.Distinct());
            CliToDownload = (exe + layout + bodystr + suffix + lang).Replace("  ", " ");
            CliToInstall = exe + bodystr + suffix + (Name == "Enterprise" ? "--noWeb" : "");
        }
    }
}
