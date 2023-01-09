using MSL.Core;
using MSL.Process;
using MSL.Process.Wpf.UI;
using MSL.UI.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;
using Unity;

namespace RESTHandler.Process
{
    public class ProcessController : ProcessControllerBase

    {
        private IUnityContainer Container;

        readonly new UIController UIController;

        public ProcessQueue AssmbleQ { get; private set; }
        public override Type DefaultPassScreen { get; set; } = typeof(PassScreen);
        public override Type DefaultFailScreen { get; set; } = typeof(FailScreen);
        public override Type DefaultStatusBar { get; set; } = typeof(ProcessStatusBar);
        public override string EquipmentIdentity { get; set; }


        public ProcessController(IMslApplication  mslApplication , IUnityContainer container , UIController uiController) : base(mslApplication)
        {
            Container = container;
            UIController = uiController;

            //this is where the list of views to run the test will be
        }

        public override void StartAppropriateQueue(User user, Job job)
        {
            AssmbleQ.Start();
        }
    }
}
