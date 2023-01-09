using MSL.Connectivity;
using MSL.Core;
using MSL.Core.Wpf;
using MSL.Process;
using MSL.Process.Wpf.UI;
using MSL.UI.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MSL.Process.Wpf_Project1
{
    /// <summary>
    /// The purpose of this VM is to configure the app ready for usage. 
    /// This generally consists of:
    /// - Loading the local configuration file and extracting the rig RUK and DB connection strings
    /// - Initialising the rig hardware / peripherals
    /// </summary>
    internal class AppStartupViewModel : ViewModelBase
    {
        private string _Progress;
        public string Progress
        {
            get { return _Progress; }
            set { SetProperty(ref _Progress, value); }
        }

        private readonly IMslApplication _App;
        private readonly UIController _UIController;
        private string _RUK;

        public AppStartupViewModel(UIController uiController, IMslApplication app)
        {
            _UIController = uiController;
            _App = app;
        }

        public override async Task Initialise(object[] initialisationMetadata)
        {
            try
            {
                await PullLocalConfiguration();

                // Feel free to add any further startup tasks here as required

                Trace.WriteLine("Rig Configuration Successful");

                _UIController.NavigateHome();
            }
            catch (Exception)
            {
                Trace.WriteLine("Rig Configuration Failed");
                _UIController.Navigate<FailScreen>(navigationMetadata: new object[] { new ProcessFailureMetadata("The app could not start up. Check the exception log for details") });
            }
        }

        private async Task PullLocalConfiguration()
        {
            Progress = "Retreiving local configuration";
            string configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _App.ApplicationTitle);

            try
            {
                // STAGE 1 - Try to load the config file
                // We store our config file external to the application, so that installation of new versions
                // does not overwrite the config file. The config helper also deals with merging keys when you add or remove
                // The default CommonApplicationData folder is C:\ProgramData
                var config = new ConfigHelper(configDirectory);

                // STAGE 2 - Try to get hold of the RUK from the config file
                _RUK = config.Settings["RUK"].Value;
                if (string.IsNullOrEmpty(_RUK) || _RUK == "Undefined")
                {
                    await WarningLogger.LogAsync("The RUK of the rig must be defined before the app can start");
                    throw new Exception($"RUK field is not set, value = {_RUK}");
                }

                // STAGE 3 - Try to get hold of connection strings from the config file
                ConnectionStrings.Add("Syteline", config.Settings["SytelineConnectionString"].Value);
                ConnectionStrings.Add("EPD", config.Settings["EPDConnectionString"].Value);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to parse the local config at {configDirectory}. To restore the default config, delete the file and restart the app", e);
            }
        }
    }
}
