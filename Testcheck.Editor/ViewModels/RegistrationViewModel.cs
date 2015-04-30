﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TAlex.License;
using TAlex.Mvvm.Commands;
using TAlex.Mvvm.Services;
using TAlex.Mvvm.ViewModels;
using TAlex.Testcheck.Editor.Properties;
using TAlex.Testcheck.Editor.Services.Windows;


namespace TAlex.Testcheck.Editor.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        #region Fields

        protected readonly ILicenseDataManager LicenseDataManager;
        protected readonly IMessageService MessageService;
        protected readonly IApplicationService ApplicationService;

        private string _licenseName;
        private string _licenseKey;

        #endregion

        #region Properties

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string LicenseName
        {
            get
            {
                return _licenseName;
            }

            set
            {
                Set(ref _licenseName, value);
                RaiseCanExecuteRegisterCommand();
            }
        }

        [Required]
        public string LicenseKey
        {
            get
            {
                return _licenseKey;
            }

            set
            {
                Set(ref _licenseKey, value);
                RaiseCanExecuteRegisterCommand();
            }
        }

        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructors

        public RegistrationViewModel(ILicenseDataManager licenseDataManager, IMessageService messageService, IApplicationService applicationService)
        {
            LicenseDataManager = licenseDataManager;
            MessageService = messageService;
            ApplicationService = applicationService;

            RegisterCommand = new RelayCommand(RegisterCommandExecute, RegisterCommandCanExecute);
        }

        #endregion

        #region Methods

        private void RegisterCommandExecute()
        {
            LicenseDataManager.Save(new LicenseData { LicenseName = LicenseName.Trim(), LicenseKey = LicenseKey.Trim() });
            MessageService.ShowInformation(Resources.locPleaseRestartToVerifyLicense, Resources.locInformationMessageCaption);
            ApplicationService.Shutdown();
        }

        private bool RegisterCommandCanExecute()
        {
            return !String.IsNullOrWhiteSpace(LicenseName) && !String.IsNullOrWhiteSpace(LicenseKey);
        }

        private void RaiseCanExecuteRegisterCommand()
        {
            var command = RegisterCommand as RelayCommand;
            if (command != null)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
