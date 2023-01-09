using Chess;
using HelixToolkit.Wpf.SharpDX.Utilities;
using MSL.Connectivity;
using MSL.Core;
using MSL.Core.Wpf;
using MSL.Process;
using MSL.Process.Wpf.UI;
using MSL.Process.Wpf_Project1.Game;
using MSL.Process.Wpf_Project1.Models;
using MSL.Process.Wpf_Project1.Models.Piece;
using MSL.Process.Wpf_Project1.Process.Shared;
using MSL.UI;
using Prism.Common;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using RESTHandler.Process.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity.Resolution;
using NavigationResult = Prism.Regions.NavigationResult;

namespace MSL.Process.Wpf_Project1
{
    public partial class App : PrismApplication
    {
        //static NVOptimusEnabler nvEnabler = new NVOptimusEnabler();
        //protected override void MSL_BeforeStartup()
        //{
        //    ApplicationTitle = WindowTitle = "Rest Handler";
        //    ApplicationIdentity = "F-0000-0000";
        //    ApplicationVersion = "0.1.0";
        //    AllowMultipleInstances = false;
        //    ExceptionLogger.CrashLogDirectory = Path.Combine(Environment.SpecialFolder.CommonApplicationData.ToString(), ApplicationTitle, "CrashLogs");
        //    SharedConstants.UIThreadContext = SynchronizationContext.Current;
        //    // Anything else you want to do pre-startup can go here

        //}

        //protected override void MSL_OnInitialised()
        //{
        //    Application.Current.MainWindow.WindowState = WindowState.Maximized;

        //    var engViews = new List<EngineeringView>()
        //    {
        //    };

        //    UIConfig.UseTemplate(this,
        //                         startView: typeof(RestHandler),
        //                         homeButtonView: typeof(OperatorStart),
        //                         engineeringViews: engViews,
        //                         engineerAuthorisation: SytelineService.IsEngineer);
        //}

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any types that we want to provide via dependency injection
            // Only need to register types that have specific lifetime requirements
            // Container will serve a new instance by default unless registered otherwise (singleton, instance etc)
            containerRegistry.RegisterSingleton<IProcessController, ProcessController>()
                             .RegisterSingleton<ProcessController>();

            containerRegistry.RegisterSingleton<GameManager>();
            containerRegistry.RegisterSingleton<FEN>();
            containerRegistry.Register<PieceBase>();
        }

        //public IRegionManager RegionManager { get; set; }

        protected override void Initialize()
        {
            SharedConstants.UIThreadContext = SynchronizationContext.Current;

            base.Initialize();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;

            var RegionManager = Container.Resolve<IRegionManager>();
            var view = Container.Resolve<RestHandler>();
            _ = RegionManager.AddToRegion("MainRegion", view);
            //NavigateShell(typeof(RestHandler));
        }


        //public void NavigateShell<T>([Optional] T view)
        //{
        //    Type navigatingTo = (view as Type) ?? typeof(T);
        //    Navigate("MainRegion", navigatingTo);
        //}

        ///// <summary>
        ///// Navigate view to a specified region
        ///// </summary>
        ///// <typeparam name="T">The view to navigate to</typeparam>
        ///// <param name="regionName">The name of the region displaying the view</param>
        ///// <param name="view">The view to be displayed</param>
        ///// <param name="navigationMetadata">Metadata to be given to the viewmodel of the new view</param>
        ///// <param name="removePreviousView">Determines if the old view should remain underneath the new view, or be replaced by it</param>
        //public void Navigate<T>(string regionName, [Optional] T view, bool removePreviousView = true, params object[] navigationMetadata)
        //{
        //    Type type = (view as Type) ?? typeof(T);
        //    _ = NavigateInternal( type, regionName, removePreviousView, navigationMetadata);
        //}



        //private async Task NavigateInternal(Type userControl, string regionName, bool removePreviousView, params object[] navigationMetadata)
        //{
        //    // Validate parameters
        //    if (userControl == null) throw new NavigationException(342525, $"View parameter is null");
        //    if (userControl.UnderlyingSystemType.BaseType != typeof(UserControl)) throw new NavigationException(964574, $"View being injected [{userControl}] must be a UserControl");

        //    // Get the prism region
        //    IRegion PrismRegion = RegionManager.Regions[regionName];
        //    if (PrismRegion == null) throw new NavigationException(678849, $"Region [{regionName}] could not be found in the regionmanager");

        //    // If we need to remove the old view(s)
        //    if (removePreviousView)
        //    {
        //        foreach (UserControl OldView in PrismRegion.Views)
        //        {
        //            // Get a reference to the old viewmodel, so that we can call Shutdown methods
        //            ViewModelBase OldViewModel = (ViewModelBase)OldView?.DataContext;

        //            // Tell the viewmodel that its view is about to be removed
        //            await OldViewModel?.BeforeShutDown();

        //            // Cancel the ViewModelBase Cancellation token
        //            OldViewModel?.TokenSource?.Cancel();

        //            // We are now finished with the old view - remove it
        //            PrismRegion.RemoveAll();

        //            // Tell the viewmodel that it should now fully shut down
        //            await OldViewModel?.ShutDown();
        //        }
        //    }
        //    else // Otherwise, temporarily deactivate the old view
        //    {
        //        UserControl OldView = (UserControl)PrismRegion.ActiveViews.FirstOrDefault();
        //        ViewModelBase OldViewModel = (ViewModelBase)OldView?.DataContext;

        //        if (OldView != null)
        //        {
        //            PrismRegion.Deactivate(OldView);
        //            await OldViewModel?.OnViewHidden();
        //        }
        //    }

        //    // Add the next view to the region
        //    UserControl NewView = (UserControl)Container.Resolve(userControl);
        //    PrismRegion.Add(NewView);
        //    string viewName = NewView.GetType().Name;

        //    Trace.TraceInformation($"Navigated to {viewName}.");

        //    // Check that the viewmodel derives from ViewModelBase
        //    if (!(NewView.DataContext is ViewModelBase)) throw new NavigationException(672411, $"Viewmodel for [{userControl}] must inherit from ViewModelBase");

        //    // Get hold of the new viewmodel and Initialise it
        //    ViewModelBase NewViewModel = (ViewModelBase)NewView.DataContext;
        //    string viewmodelName = NewViewModel.GetType().Name;

        //    try
        //    {
        //        await NewViewModel.Initialise(navigationMetadata);
        //        //NavigationResultHelper.SetViewModelInitialised();
        //    }
        //    catch (StackOverflowException e)
        //    {
        //        // Stack overflow is currently considered a non-recoverable situation, and as a result, will never be caught.
        //        // Long term it would be good to come up with a way of catching the cause before the exception occurs.
        //        // Maybe check that the datacontext of the new view is not the same as the old view? Only issue is that sometimes you may want to allow that.
        //        throw new NavigationException(176433, $"The View: [{viewName}] has a viewModel: [{viewmodelName}].  The viewmodel is likely re-navigating the view every time initialise is called. Missed AutoWireViewModel?", e);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new NavigationException(176434, $"Navigation controller failed to call Initialise() one the viewmodel. {Environment.NewLine} View: [{viewName}] {Environment.NewLine} ViewModel: [{viewmodelName}]", e);
        //    }
        //}


    }
}
