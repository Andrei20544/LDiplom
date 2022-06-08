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
using System.Windows.Shapes;

namespace DipWACH.View
{
    public partial class RegLogWindow : Window
    {
        private static RegLogWindow _instance;

        public static RegLogWindow GetInstance()
        {
            if (_instance == null)
                _instance = new RegLogWindow();

            return _instance;
        }

        public RegLogWindow()
        {
            InitializeComponent();

            DataContext = new LogINViewModel(this);

            LTextBox.Focus();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();
    }
}
