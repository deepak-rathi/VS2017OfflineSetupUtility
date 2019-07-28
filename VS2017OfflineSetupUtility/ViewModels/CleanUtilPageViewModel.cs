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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VS2017OfflineSetupUtility.Models;
using VS2017OfflineSetupUtility.Mvvm;

namespace VS2017OfflineSetupUtility.ViewModels
{
    class CleanUtilPageViewModel : BindableBase
    {
        #region DirectoryNames
        /// <summary>
        /// Contain all directory names for selected folder. Left for future purpose; binding to UI list
        /// </summary>
        private ObservableCollection<VSModule> _moduleCollection = new ObservableCollection<VSModule>();

        public ObservableCollection<VSModule> ModuleCollection
        {
            get { return _moduleCollection; }
            set { SetProperty(ref _moduleCollection, value); }
        }
        #endregion

        #region OldVersionModule
        private ObservableCollection<VSModule> _oldVersionModule = new ObservableCollection<VSModule>();
        /// <summary>
        /// Contain all directory names for selected folder
        /// </summary>
        public ObservableCollection<VSModule> OldVersionModule
        {
            get { return _oldVersionModule; }
            set
            {
                if (SetProperty(ref _oldVersionModule, value))
                    DeleteOldVersionCommand.RaiseCanExecuteChanged();
            }
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
                    DeleteOldVersionCommand.RaiseCanExecuteChanged();
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
                            DoClassification();

                            if (!OldVersionModule.Any())
                                MessageBox.Show("Old version folder does not exist.");
                        }
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception.Message);
                    }
                }));
            }
        }

        public void DoClassification()
        {
            if (string.IsNullOrWhiteSpace(SelectedFolderPath))
                return;

            ModuleCollection.Clear();
            OldVersionModule.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(SelectedFolderPath);
            if(dirInfo!=null && !dirInfo.Exists)
            {
                Properties.Settings.Default.LastSelectedFolder = "";
                SelectedFolderPath = "";
                return;
            }

            //classification
            DirectoryInfo archiveDirectoryInfo = null;
            var directories = dirInfo.GetDirectories();
            foreach (var directory in directories)
            {
                var vsModule = new VSModule();
                if (directory.Name.Contains(","))
                {
                    var stringSplit = directory.Name.Split(',').ToList();
                    vsModule.Name = stringSplit.FirstOrDefault();
                    vsModule.Version = stringSplit[1];
                    stringSplit.Remove(vsModule.Name);
                    stringSplit.Remove(vsModule.Version);
                    if (stringSplit.Count() > 0)
                    {
                        foreach (var item in stringSplit)
                            vsModule.Name = vsModule.Name + "," + item;
                    }
                }
                else if (directory.Name.Equals("Archive"))
                {
                    archiveDirectoryInfo = directory;
                }
                else
                {
                    continue;
                }
                vsModule.FullPath = directory.FullName;
                ModuleCollection.Add(vsModule);
            }

            //Select all the Modules with same name from ModuleCollection
            var duplicateModules =
            ModuleCollection.Where(module =>
            ModuleCollection
            .Except(new ObservableCollection<VSModule> { module })
            .Any(x => x.Name == module.Name)
            ).ToObservableCollection();

            //Get all the old version modules/folder from duplicateModules
            OldVersionModule =
            duplicateModules.Where(module =>
            duplicateModules
            .Except(new ObservableCollection<VSModule> { module })
            .Any(x => x.Name == module.Name && x.VersionObject.CompareTo(module.VersionObject) > 0)
            ).ToObservableCollection();

            //Add archive folder to old version module
            if (archiveDirectoryInfo != null)
                OldVersionModule.Add(new VSModule()
                {
                    FullPath = archiveDirectoryInfo.FullName,
                    Name = archiveDirectoryInfo.Name
                });
        }
        #endregion

        #region DeleteOldVersionCommand
        private DelegateCommand _deleteOldVersionCommand;

        public DelegateCommand DeleteOldVersionCommand
        {
            get
            {
                return _deleteOldVersionCommand ?? (_deleteOldVersionCommand = new DelegateCommand(async () =>
                {
                    try
                    {
                        //Delete old version folder and files
                        await Task.Run(() =>
                        {
                            foreach (var folder in OldVersionModule)
                            {
                                Directory.Delete(folder.FullPath, true);
                            }
                        }).ConfigureAwait(false);

                        App.Current.Dispatcher.Invoke(() =>
                         {
                             OldVersionModule.Clear();
                             ModuleCollection.Clear();
                             MessageBox.Show("Operation successful.");
                         });
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception.Message);
                    }
                }, () => !string.IsNullOrWhiteSpace(SelectedFolderPath) && OldVersionModule?.Count > 0));
            }
        }

        #endregion

        #region GoBack Command
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
        #endregion  Exit Command
    }

}
