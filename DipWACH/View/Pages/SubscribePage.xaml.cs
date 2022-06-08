using DipWACH.Helper;
using DipWACH.Model;
using DipWACH.ViewModel;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DipWACH.View.Pages
{
    public partial class SubscribePage : Page
    {
        private List<NewSubscriber> _subscribers;

        public SubscribePage()
        {
            InitializeComponent();

            _subscribers = GetData.GetSubscribers();

            DataContext = new MainViewModel(_subscribers);

        }
    }
}
