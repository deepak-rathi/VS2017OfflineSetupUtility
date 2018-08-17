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
        public List<VsEdition> allVisualStudioEditions = VsEditionUtil.GetAllVisualStudioEditions();

        #region Constructor
        public DownloadUtilPageViewModel()
        {
            DownloadWorkloadFromWeb(SelectedVsEdition);
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
                            GenerateCli(allVisualStudioEditions.FirstOrDefault(edition => edition.Name.Equals(SelectedVsEdition)));
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

        #region VsEdition
        public string SelectedVsEdition = "Community";

        private DelegateCommand<string> _vsEditionCommand;

        public DelegateCommand<string> VsEditionCommand
        {
            get
            {
                return _vsEditionCommand ?? (_vsEditionCommand = new DelegateCommand<string>((selectedContent) =>
                {
                    SelectedVsEdition = selectedContent;
                    DownloadWorkloadFromWeb(SelectedVsEdition);
                }));
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
                    GenerateCli(allVisualStudioEditions.FirstOrDefault(f => f.Name.Equals(SelectedVsEdition)));
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
        private void DownloadWorkloadFromWeb(string vsEdition)
        {
            var selectedVsEdition = allVisualStudioEditions.FirstOrDefault(edition => edition.Name.Equals(vsEdition));
            string markdownText = null;
            try
            {
                markdownText = new WebClient().DownloadString(selectedVsEdition.WorkloadGitHubUri);

            }
            catch (Exception exception)
            {
                MessageBox.Show("Error occured:" + exception.GetType().ToString() + ".Make sure internet connection is available.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            WorkloadProcesser.ProcessMarkDownText(markdownText, selectedVsEdition.vsEditonWorkloads.Workloads);
            selectedVsEdition.SaveAllWorkloadsToFile();
            Workloads = selectedVsEdition.vsEditonWorkloads.Workloads.ToObservableCollection();
            GenerateCli(selectedVsEdition);
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
                    var selectedVsEdition = allVisualStudioEditions.FirstOrDefault(f => f.Name.Equals(SelectedVsEdition));
                    var dirInfo = new DirectoryInfo(SelectedFolderPath);
                    var subdirInfo = dirInfo.CreateSubdirectory("Setup");
                    try
                    {
                        //Download Setup exe from web
                        new WebClient().DownloadFile(new Uri(selectedVsEdition.SetupUri), subdirInfo.FullName + @"\vs_" + selectedVsEdition.Name + ".exe");
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
