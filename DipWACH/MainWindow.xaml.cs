using DipWACH.View;
using DipWACH.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DipWACH
{
    public partial class MainWindow : Window
    {
        private static MainWindow _instance;

        public static MainWindow GetInstance(RegLogWindow window, string emplType)
        {
            if (_instance == null)
                _instance = new MainWindow(window, emplType);

            return _instance;
        }

        public MainWindow(RegLogWindow window, string emplType)
        {
            InitializeComponent();

            DataContext = new MainViewModel(window, emplType);

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();
    }
}
