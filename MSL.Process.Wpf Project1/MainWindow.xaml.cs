using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The main window of the application
        /// </summary>
        /// <param name="regionManager"></param>
        public MainWindow(IRegionManager regionManager)
        {
            RegionManager = regionManager;
            InitializeComponent();
        }

        /// <summary>
        /// The RegionManager that should be used for navigation within the shell
        /// </summary>
        public IRegionManager RegionManager { get; private set; }
    }
}

