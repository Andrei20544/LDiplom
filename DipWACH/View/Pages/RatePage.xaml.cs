using DipWACH.ViewModel;
using System.Windows.Controls;

namespace DipWACH.View.Pages
{
    public partial class RatePage : Page
    {
        public RatePage()
        {
            InitializeComponent();
            DataContext = new RateViewModel();
        }
    }
}
