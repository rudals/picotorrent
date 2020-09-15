﻿using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Windows.Input;

namespace PicoTorrentBootstrapper.ViewModels
{
    public sealed class InstallWaitingViewModel : PropertyNotifyBase
    {
        private readonly BootstrapperApplication _bootstrapper;

        private bool _downloadDependencies;
        private string _installLocation;
        private ICommand _changeInstallLocationCommand;

        public InstallWaitingViewModel(BootstrapperApplication bootstrapper)
        {
            _bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper));
            ShouldDownloadDependencies = true;

            if (_bootstrapper.Engine.StringVariables.Contains("InstallFolder"))
            {
                _installLocation = _bootstrapper.Engine.FormatString(_bootstrapper.Engine.StringVariables["InstallFolder"]);
            }
        }

        public ICommand ChangeInstallLocationCommand
        {
            get
            {
                if (_changeInstallLocationCommand == null)
                {
                    _changeInstallLocationCommand = new RelayCommand(param => ChangeInstallLocation());
                }

                return _changeInstallLocationCommand;
            }
        }

        public string InstallLocation
        {
            get { return _installLocation; }
            set
            {
                _bootstrapper.Engine.StringVariables["InstallFolder"] = value;
                _installLocation = value;
                OnPropertyChanged(nameof(InstallLocation));
            }
        }

        public bool ShouldDownloadDependencies
        {
            get { return _downloadDependencies; }
            set { _downloadDependencies = value; OnPropertyChanged(nameof(ShouldDownloadDependencies)); }
        }

        private void ChangeInstallLocation()
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog
            {
                SelectedPath = InstallLocation
            };

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            InstallLocation = dlg.SelectedPath;
        }
    }
}
