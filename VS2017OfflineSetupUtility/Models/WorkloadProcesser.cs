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
using System;
using System.Collections.Generic;

namespace VS2017OfflineSetupUtility.Models
{
    internal static class WorkloadProcesser
    {
        //Parse Workload data from Markdown
        internal static void ProcessMarkDownText(string markDownText, List<Workload> Workloads)
        {
            Workloads.Clear();

            Workload workload = null;

            var markDownTextLines = markDownText.Replace("\r", "").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < markDownTextLines.Length;)
            {
                var line = markDownTextLines[i].Trim();
                if (line.Length > 3 && line.Substring(0, 3) == "## ")
                {
                    //Break when reached get support text
                    if (line.Substring(3).ToLower().Equals("get support"))
                        break;

                    //Workload title as Name
                    workload = new Workload() { Name = line.Substring(3) };

                    var secondLine = markDownTextLines[++i];
                    if (secondLine.Substring(0, 8) == "**ID:** ")
                    {
                        workload.ID = secondLine.Substring(7);
                        workload.Description = markDownTextLines[++i].Replace("**Description:** ", "");
                    }
                    else
                    {
                        // non-workload components
                        workload.Description = secondLine;
                    }

                    //Skip text and increment i
                    while (i < markDownTextLines.Length && markDownTextLines[i++].Substring(0, 5) != "--- |") { } 
                    //get all components for this workload
                    while (markDownTextLines[i].Substring(0, 3) != "## ")
                    {
                        var thirdLine = markDownTextLines[i++].Split('|');
                        var component = new Component()
                        {
                            ID = thirdLine[0].Trim(),
                            Name = thirdLine[1],
                            Version = thirdLine[2],
                            Depedency = thirdLine.Length > 3 ? (Depedency)Enum.Parse(typeof(Depedency), thirdLine[3], true) : Depedency.Independent,
                            CommponentWorkload = workload,
                        };
                        workload.Components.Add(component);
                    }

                    Workloads.Add(workload);
                }
                else
                    i++;

            }
        }
    }
}
