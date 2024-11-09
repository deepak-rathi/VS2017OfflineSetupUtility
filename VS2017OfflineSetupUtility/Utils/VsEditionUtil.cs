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
                    Version = "15C",
                    Name = "Visual Studio 2017 Community",
                    SetupUri = "https://aka.ms/vs/15/release/vs_community.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-community?view=vs-2017",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/d31aa83d1e31658ca5d82c318a766a36e5746962/docs/install/includes/vs-2017/workload-component-id-vs-community.md",
                },

                new VsEdition() {
                    Version = "15P",
                    Name = "Visual Studio 2017 Professional",
                    SetupUri = "https://aka.ms/vs/15/release/vs_professional.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-professional?view=vs-2017",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/d31aa83d1e31658ca5d82c318a766a36e5746962/docs/install/includes/vs-2017/workload-component-id-vs-professional.md",
                },

                new VsEdition()  {
                    Version = "15E",
                    Name = "Visual Studio 2017 Enterprise",
                    SetupUri = "https://aka.ms/vs/15/release/vs_enterprise.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-enterprise?view=vs-2017",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/d31aa83d1e31658ca5d82c318a766a36e5746962/docs/install/includes/vs-2017/workload-component-id-vs-enterprise.md",
                },

                new VsEdition() {
                    Version ="16C",
                    Name = "Visual Studio 2019 Community",
                    SetupUri = "https://aka.ms/vs/16/release/vs_community.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-community?view=vs-2019",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2019/workload-component-id-vs-community.md",
                },

                new VsEdition() {
                    Version = "16P",
                    Name = "Visual Studio 2019 Professional",
                    SetupUri = "https://aka.ms/vs/16/release/vs_professional.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-professional?view=vs-2019",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2019/workload-component-id-vs-professional.md",
                },

                new VsEdition()  {
                    Version = "16E",
                    Name = "Visual Studio 2019 Enterprise",
                    SetupUri = "https://aka.ms/vs/16/release/vs_enterprise.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-enterprise?view=vs-2019",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2019/workload-component-id-vs-enterprise.md",
                },

                new VsEdition() {
                    Version ="17C",
                    Name = "Visual Studio 2022 Community",
                    SetupUri = "https://aka.ms/vs/17/release/vs_community.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-community?view=vs-2022",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2022/workload-component-id-vs-community.md",
                },

                new VsEdition() {
                    Version = "17P",
                    Name = "Visual Studio 2022 Professional",
                    SetupUri = "https://aka.ms/vs/17/release/vs_professional.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-professional?view=vs-2022",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2022/workload-component-id-vs-professional.md",
                },

                new VsEdition()  {
                    Version = "17E",
                    Name = "Visual Studio 2022 Enterprise",
                    SetupUri = "https://aka.ms/vs/17/release/vs_enterprise.exe",
                    WorkloadDocUri = "https://docs.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-enterprise?view=vs-2022",
                    WorkloadGitHubUri = "https://raw.githubusercontent.com/MicrosoftDocs/visualstudio-docs/master/docs/install/includes/vs-2022/workload-component-id-vs-enterprise.md",
                }
            };
        }
    }
}
