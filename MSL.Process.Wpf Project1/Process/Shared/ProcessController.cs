using MSL.Core;
using MSL.Process;
using MSL.Process.Wpf;
using MSL.Process.Wpf.UI;

using RESTHandler.Process.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace MSL.Process.Wpf_Project1.Process.Shared
{
    internal class ProcessController : ProcessControllerBaseWpf, IProcessController
    {
        public override string EquipmentIdentity { get; set; }

        public ProcessQueue Production { get; private set; }
        public ProcessQueue HealthCheck { get; private set; }

        private readonly IUnityContainer _Container;


        public ProcessController(IMslApplication app, IUnityContainer container) : base(app)
        {
            _Container = container;

            // The ProcessController will have control handed to it by the operator start screen.
            // We can configure how that screen behaves:
            ConfigureOperatorStartBehaviour();

            Production = CreateQueue("Production");
            Production.Q<RestHandler>();
        }

        private void ConfigureOperatorStartBehaviour()
        {
            //Specify the part numbers that the operator start screen will accept
            SkillCheckItems.Add("A-0000-1000", 50, 100, "Variant A");
            SkillCheckItems.Add("A-0000-2000", 50, 100, "Variant B");


#if DEBUG  // When debugging: 

            // We always allow VALIDATION jobs to be used
            OperatorStartSettings.AlwaysAllowedJobJumbers.Add("VALIDATION");

            // We Automate the operator login screen to move past it quicker
            OperatorStartSettings.AutoFillUserID = "NB138509";
            OperatorStartSettings.AutoFillJobNumber = "VALIDATION";
            OperatorStartSettings.AutoStartChecks = true;
            OperatorStartSettings.AutoStartProcessAfterInputsValidated = false;
#endif
        }

        /// <summary>
        /// Once the operator start sceen has validated the job / user, it will pass off to this method
        /// </summary>
        public override async void StartAppropriateQueue(User user, Job job)
        {
            try
            {
                // Otherwise, work out what production test to run (if any)
                if (job.SkillCheckItem?.Key == "Variant A" || job.JobNumber.Value == "VALIDATION")
                {
                    // Start the queue
                    Production.Start();
                }
                else
                {
                    WarningLogger.Log("This software is currently only set up to build Variant A (or validation jobs)");
                }
            }
            catch
            {
                await WarningLogger.LogAsync("The ProcessController failed to start the specified queue. \nCheck the exception log for details.");
            }
        }
    }
}
