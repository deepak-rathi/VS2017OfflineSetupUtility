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
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using VS2017OfflineSetupUtility.Models;
using VS2017OfflineSetupUtility.Mvvm;
using VS2017OfflineSetupUtility.Utils;

namespace VS2017OfflineSetupUtility.ViewModels
{
    class DownloadUtilPageViewModel : BindableBase
    {
        public List<VsEdition> AllVisualStudioEditions { get; set; }

        #region Constructor
        public DownloadUtilPageViewModel()
        {
            AllVisualStudioEditions = VsEditionUtil.GetAllVisualStudioEditions();
            SelectedVsEdition = AllVisualStudioEditions.FirstOrDefault();
            DownloadWorkloadFromWeb(SelectedVsEdition);
            CheckIfPreviouslySelectedFolderStillExist();
        }
        #endregion

        #region SelectedFolderPath
        private string _selectedFolderPath = Properties.Settings.Default.LastSelectedFolder;
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
                return _selectFolderCommand ?? (_selectFolderCommand = new DelegateCommand(() =>
                {
                    try
                    {
                        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                        dialog.InitialDirectory = Properties.Settings.Default.LastSelectedFolder;
                        dialog.IsFolderPicker = true;
                        dialog.AddToMostRecentlyUsedList = false;
                        dialog.Title = "Select VS2017 offline setup folder";

                        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                        {
                            SelectedFolderPath = dialog.FileName;
                            Properties.Settings.Default.LastSelectedFolder = SelectedFolderPath;
                            Properties.Settings.Default.Save();
                            GenerateCli(SelectedVsEdition);
                        }
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception.Message);
                    }
                }));
            }
        }
        #endregion

        #region CheckIfPreviouslySelectedFolderStillExist
        /// <summary>
        /// Check if previously selected folder still exist, if not reset last selected folder value
        /// </summary>
        private void CheckIfPreviouslySelectedFolderStillExist()
        {
            var lastSelectedFolder = Properties.Settings.Default.LastSelectedFolder;
            if (string.IsNullOrWhiteSpace(lastSelectedFolder))
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(SelectedFolderPath);
            if (dirInfo != null && !dirInfo.Exists)
            {
                Properties.Settings.Default.LastSelectedFolder = "";
                SelectedFolderPath = "";
                return;
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
                    DownloadWorkloadFromWeb(value);
                    GenerateCli(SelectedVsEdition);
                }
            }
        }
        #endregion

        #region Language

        public List<string> Languagelist
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
                return _vsRecommendedCommand ?? (_vsRecommendedCommand = new DelegateCommand<bool?>((IsChecked) =>
                {
                    _isRecommendedSelected = IsChecked;
                }));
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
                return _vsOptionalCommand ?? (_vsOptionalCommand = new DelegateCommand<bool?>((IsChecked) =>
                {
                    _isOptionalSelected = IsChecked;
                }));
            }
        }

        #endregion

        #region Workload ComponentChanged
        private DelegateCommand _workloadComponentChanged;

        public DelegateCommand WorkloadComponentChanged
        {
            get
            {
                return _workloadComponentChanged ?? (_workloadComponentChanged = new DelegateCommand(() =>
                {
                    GenerateCli(SelectedVsEdition);
                }));
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
        private void DownloadWorkloadFromWeb(VsEdition vsEdition)
        {
            try
            {
                WebClient webClient = new WebClient();
                IWebProxy webProxy = WebRequest.DefaultWebProxy;
                webProxy.Credentials = CredentialCache.DefaultCredentials;
                webClient.Proxy = webProxy;
                var markdownText = webClient.DownloadString((string)vsEdition.WorkloadGitHubUri);

                WorkloadProcesser.ProcessMarkDownText(markdownText, (List<Workload>)vsEdition.vsEditonWorkloads.Workloads);
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
                return _downloadCommand ?? (_downloadCommand = new DelegateCommand(() =>
                {
                    var dirInfo = new DirectoryInfo(SelectedFolderPath);
                    var subdirInfo = dirInfo.CreateSubdirectory("Setup");
                    try
                    {
                        WebClient webClient = new WebClient();
                        IWebProxy webProxy = WebRequest.DefaultWebProxy;
                        webProxy.Credentials = CredentialCache.DefaultCredentials;
                        webClient.Proxy = webProxy;
                        //Download Setup exe from web
                        webClient.DownloadFile(new Uri(SelectedVsEdition.SetupUri), subdirInfo.FullName + @"\" + SelectedVsEdition.Name.Replace(' ', '_') + ".exe");
                        File.WriteAllText(subdirInfo.FullName + @"\CliCommand.bat", CliText);
                        Process.Start(new ProcessStartInfo()
                        {
                            FileName = subdirInfo.FullName + @"\CliCommand.bat",
                            WorkingDirectory = subdirInfo.FullName
                        });
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Error occured:" + exception.GetType().ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }, () => !string.IsNullOrWhiteSpace(SelectedFolderPath)));
            }
        }

        #endregion

        #region GoBack
        private DelegateCommand _goBackCommand;

        public DelegateCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new DelegateCommand(() =>
                {
                    App.CurrentFrame.GoBack();
                }));
            }
        }
        #endregion
    }
}
