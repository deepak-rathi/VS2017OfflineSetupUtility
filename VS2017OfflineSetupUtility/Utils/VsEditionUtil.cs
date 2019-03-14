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
using VS2017OfflineSetupUtility.Models;

namespace VS2017OfflineSetupUtility.Utils
{
    internal static class VsEditionUtil
    {
        internal static List<VsEdition> GetAllVisualStudioEditions()
        {
            return new List<VsEdition>()
            {
                new VsEdition() {
                Name = "Community",
                SetupUri = "https://aka.ms/vs/15/release/vs_community.exe",
                WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-community?view=vs-2017",
                WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2017/workload-component-id-vs-community.md",
                },

                new VsEdition() {
                Name = "Professional",
                SetupUri = "https://aka.ms/vs/15/release/vs_professional.exe",
                WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-professional?view=vs-2017",
                WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2017/workload-component-id-vs-professional.md",
                },
                new VsEdition()  {
                Name = "Enterprise",
                SetupUri = "https://aka.ms/vs/15/release/vs_enterprise.exe",
                WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-enterprise?view=vs-2017",
                WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2017/workload-component-id-vs-enterprise.md",
                },
            };
        }
    }
}
