/*
    Copyright © 2017-2023 Deepak Rathi 
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using VS2017OfflineSetupUtility.Models;
using VS2017OfflineSetupUtility.Mvvm;
using VS2017OfflineSetupUtility.Utils;
using System.Threading.Tasks;

namespace VS2017OfflineSetupUtility.ViewModels
{
    class DownloadUtilPageViewModel : BindableBase
    {
        public List<VsEdition> AllVisualStudioEditions { get; set; }

        #region Constructor
        public DownloadUtilPageViewModel()
        {
            AllVisualStudioEditions = VsEditionUtil.GetAllVisualStudioEditions();
        }
        #endregion

        #region SelectedFolderPath
        private string _selectedFolderPath;
        /// <summary>
        /// Contain SelectedFolderPath string
        /// </summary>
        public string SelectedFolderPath
        {
            get { return _selectedFolderPath; }
            set
            {
                if (SetProperty(ref _selectedFolderPath, value))
                {
                    DownloadCommand.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region SelectFolderCommand
        private DelegateCommand _selectFolderCommand;

        public DelegateCommand SelectFolderCommand
        {
            get
            {
                return _selectFolderCommand ??= new DelegateCommand(() =>
                {
                    var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                    try
                    {
                        folderBrowserDialog.Description = "Select VS2017 or VS2019 or VS2022 offline setup folder";

                        var dialogResult = folderBrowserDialog.ShowDialog();
                        if (dialogResult != System.Windows.Forms.DialogResult.OK && string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                            return;

                        SelectedFolderPath = folderBrowserDialog.SelectedPath;
                        GenerateCli(SelectedVsEdition);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }
                    finally
                    {
                        folderBrowserDialog.Dispose();
                    }
                });
            }
        }
        #endregion

        #region VsEdition
        private VsEdition _vsEdition;

        public VsEdition SelectedVsEdition
        {
            get { return _vsEdition; }
            set
            {
                if (SetProperty(ref _vsEdition, value) && value != null)
                {
                    _ = Task.Run(() => DownloadWorkloadFromWebAsync(value));
                    GenerateCli(SelectedVsEdition);
                }
            }
        }
        #endregion

        #region Language

        public static List<string> Languagelist
        {
            get
            {
                return new List<string>()
                {
                    "cs-CZ",
                    "de-DE",
                    "en-US",
                    "es-ES",
                    "fr-FR",
                    "it-IT",
                    "ja-JP",
                    "ko-KR",
                    "pl-PL",
                    "pt-BR",
                    "ru-RU",
                    "tr-TR",
                    "zh-CN",
                    "zh-TW",
                };
            }
        }

        private string _selectedLanguage = "en-US";

        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { SetProperty(ref _selectedLanguage, value); }
        }

        #endregion

        #region VsRecommended
        private bool? _isRecommendedSelected = default(bool);

        private DelegateCommand<bool?> _vsRecommendedCommand;

        public DelegateCommand<bool?> VsRecommendedCommand
        {
            get
            {
                return _vsRecommendedCommand ??= new DelegateCommand<bool?>((IsChecked) =>
                {
                    _isRecommendedSelected = IsChecked;
                });
            }
        }

        #endregion

        #region VsOptional
        private bool? _isOptionalSelected = default(bool);

        private DelegateCommand<bool?> _vsOptionalCommand;

        public DelegateCommand<bool?> VsOptionalCommand
        {
            get
            {
                return _vsOptionalCommand ??= new DelegateCommand<bool?>((IsChecked) => { _isOptionalSelected = IsChecked; });
            }
        }

        #endregion

        #region Workload ComponentChanged
        private DelegateCommand _workloadComponentChanged;

        public DelegateCommand WorkloadComponentChanged
        {
            get
            {
                return _workloadComponentChanged ??= new DelegateCommand(() => { GenerateCli(SelectedVsEdition); });
            }
        }

        #endregion

        #region Workloads
        private ObservableCollection<Workload> _workloads = new ObservableCollection<Workload>();

        public ObservableCollection<Workload> Workloads
        {
            get { return _workloads; }
            set { SetProperty(ref _workloads, value); }
        }

        #endregion

        #region CliText
        private string _cliText;

        public string CliText
        {
            get { return _cliText; }
            set { SetProperty(ref _cliText, value); }
        }

        #endregion

        #region Download Workload from web
        private async Task DownloadWorkloadFromWebAsync(VsEdition vsEdition)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync((string)vsEdition.WorkloadGitHubUri);
                response.EnsureSuccessStatusCode();
                var markdownText = await response.Content.ReadAsStringAsync();

                WorkloadProcesser.ProcessMarkDownText(markdownText, vsEdition.vsEditonWorkloads.Workloads);
                vsEdition.SaveAllWorkloadsToFile();
                Workloads = vsEdition.vsEditonWorkloads.Workloads.ToObservableCollection();
                GenerateCli(vsEdition);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error occured:" + exception.GetType().ToString() + ".Make sure internet connection is available.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        #endregion

        #region Generat CLI
        public void GenerateCli(VsEdition selectedVsEdition)
        {
            selectedVsEdition.GenerateCLICommand(selectedVsEdition.Name, SelectedFolderPath, SelectedLanguage, (bool)_isRecommendedSelected, (bool)_isOptionalSelected);
            CliText = selectedVsEdition.CliToDownload;
        }
        #endregion

        #region Download 
        private DelegateCommand _downloadCommand;

        public DelegateCommand DownloadCommand
        {
            get
            {
                return _downloadCommand ??= new DelegateCommand(async () =>
                {
                    var dirInfo = new DirectoryInfo(SelectedFolderPath);
                    var subdirInfo = dirInfo.CreateSubdirectory("Setup");
                    try
                    {
                        using var httpClient = new HttpClient();
                        //Download Setup exe from web
                        var response = await httpClient.GetAsync(new Uri(SelectedVsEdition.SetupUri));
                        response.EnsureSuccessStatusCode();
                        var fileBytes = await response.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(Path.Combine(subdirInfo.FullName, $"{SelectedVsEdition.Name.Replace(' ', '_')}.exe"), fileBytes);
                        File.WriteAllText(Path.Combine(subdirInfo.FullName, "CliCommand.bat"), CliText);
                        Process.Start(new ProcessStartInfo()
                        {
                            FileName = Path.Combine(subdirInfo.FullName, "CliCommand.bat"),
                            WorkingDirectory = subdirInfo.FullName
                        });
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Error occured:" + exception.GetType().ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }, () => !string.IsNullOrWhiteSpace(SelectedFolderPath));
            }
        }

        #endregion

        #region GoBack
        private DelegateCommand _goBackCommand;

        public DelegateCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ??= new DelegateCommand(App.CurrentFrame.GoBack );
            }
        }
        #endregion
    }
}
