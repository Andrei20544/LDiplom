using DipWACH.Helper;
using DipWACH.Model;
using DipWACH.ViewModel;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DipWACH.View.Pages
{
    public partial class SubscribePage : Page
    {
        public SubscribePage()
        {
            InitializeComponent();

            DataContext = new CalculateViewModel();
        }
    }
}
