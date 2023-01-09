using MSL.Connectivity;
using MSL.Core;
using MSL.Core.Wpf;
using MSL.Process;
using MSL.Process.Wpf.UI;
using MSL.UI;
using Prism.Ioc;
using Prism.Unity;
using RESTHandler.Process;
using RESTHandler.Process.Screens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace RESTHandler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MslApplication
    {
        protected override void MSL_BeforeStartup()
        {
            WindowTitle = "App Window";
            ApplicationTitle =  WindowTitle = "Rest Handler";
            ApplicationIdentity = "1.0.0.1";
            ApplicationVersion = "1.0.0.1";
        }

        protected override async void MSL_OnInitialised()
        {
            // Process
            ProcessController pController = Resolve<ProcessController>();
            Container.GetContainer().RegisterInstance<IProcessController>(pController);

            //Create List of engineering Views
            List<EngineeringView> engViews = new List<EngineeringView>()
            {
            };

            // Use template and process screens
            UIConfig.UseTemplate(this, typeof(OperatorStart),
                                engineeringViews: engViews,
                                engineerAuthorisation: SytelineService.IsEngineer);
        }

        protected override void MSL_RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Process 
            containerRegistry.RegisterSingleton<ProcessController>();
            

            // Set up location for usable config file when in MSIX installer
            ConfigHelper config = new ConfigHelper($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RTP Commissioning\\");
            containerRegistry.RegisterInstance(config);
        }
     
        protected override void MSL_ConfigureWindow(Window window)
        {
            // Window Visibility
            base.MSL_ConfigureWindow(window);
            window.MinHeight = 800;
            window.MinWidth = 1000;
            window.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// Runs at application close
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                
            }
            catch { }
        }
    }
}
