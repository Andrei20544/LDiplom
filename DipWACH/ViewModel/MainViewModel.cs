using DipWACH.Helper;
using DipWACH.Model;
using DipWACH.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DipWACH.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private RegLogWindow _regLog;
        private string _employeeType;

        private Page _subscribePage;
        private Page _applicationsPage;
        private Page _addEmployeePage;
        private Page _employeePage;

        public MainViewModel(RegLogWindow window, string emplType)
        {
            _regLog = window;
            _employeeType = emplType;

            SetAvatar();

            _subscribePage = new View.Pages.SubscribePage();
            _applicationsPage = new View.Pages.ApplicationsPage();
            _addEmployeePage = new View.Pages.AddEmployeePage();
            _employeePage = new View.Pages.EmployeePage(FIO);

            FrameOpacity = 1;
        }

        public MainViewModel(List<NewSubscriber> subscribers)
        {
            ObservableCollection<NewSubscriber> SubCollection = new ObservableCollection<NewSubscriber>();

            foreach (var sub in subscribers) SubCollection.Add(sub);

            _collectionViewSource = new CollectionViewSource { Source = SubCollection };
            _collectionViewSource.Filter += Items_Filter;

        }

        public MainViewModel(bool app) { }


        private WindowState _winState;
        public WindowState WinState
        {
            get => _winState;
            set
            {
                _winState = value;
                OnPropertyChanged("WinState");
            }
        }

        private Visibility _userUniBTNVisibility;
        public Visibility UserUniBTNVisibility
        {
            get => _userUniBTNVisibility;
            set
            {
                _userUniBTNVisibility = value;
                OnPropertyChanged("UserUniBTNVisibility");
            }
        }

        private Visibility _adminUniBTNVisibility;
        public Visibility AdminUniBTNVisibility
        {
            get => _adminUniBTNVisibility;
            set
            {
                _adminUniBTNVisibility = value;
                OnPropertyChanged("AdminUniBTNVisibility");
            }
        }


        private string _profileIMG;
        public string ProfileIMG
        {
            get => _profileIMG;
            set
            {
                _profileIMG = value;
                OnPropertyChanged("ProfileIMG");
            }
        }

        private double _frameOpacity;
        public double FrameOpacity
        {
            get => _frameOpacity;
            set
            {
                _frameOpacity = value;
                OnPropertyChanged("FrameOpacity");
            }
        }

        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }


        private string _fIO;
        public string FIO
        {
            get => _fIO;
            set
            {
                _fIO = value;
                OnPropertyChanged("FIO");
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                _collectionViewSource.View.Refresh();
                OnPropertyChanged("SearchText");
            }
        }


        //Закрытие приложения
        private RelayCommand _closeApp;
        public RelayCommand CloseApp
        {
            get
            {
                return _closeApp ?? (_closeApp = new RelayCommand(obj =>
                {
                    Application.Current.Shutdown();
                }));
            }
        }

        //Сворачивание приложения
        private RelayCommand _minimizeApp;
        public RelayCommand MinimizeApp
        {
            get
            {
                return _minimizeApp ?? (_minimizeApp = new RelayCommand(obj =>
                {
                    WinState = WindowState.Minimized;
                }));
            }
        }


        //Выход
        private RelayCommand _logOutApp;
        public RelayCommand LogOutApp
        {
            get
            {
                return _logOutApp ?? (_logOutApp = new RelayCommand(obj =>
                {
                    if (_regLog != null)
                    {
                        _regLog.Show();
                    }

                    Application.Current.MainWindow.Hide();
                }));
            }
        }


        //Список абонентов
        private RelayCommand _showSubscribers;
        public RelayCommand ShowSubscribers
        {
            get
            {
                return _showSubscribers ?? (_showSubscribers = new RelayCommand(obj =>
                {
                    SlowOpacity(_subscribePage);
                }));
            }
        }

        //Заявки
        private RelayCommand _showApplications;
        public RelayCommand ShowApplications
        {
            get
            {
                return _showApplications ?? (_showApplications = new RelayCommand(obj =>
                {
                    SlowOpacity(_applicationsPage);
                }));
            }
        }

        //Добавление сотрудников
        private RelayCommand _addEmployee;
        public RelayCommand AddEmployee
        {
            get
            {
                return _addEmployee ?? (_addEmployee = new RelayCommand(obj =>
                {
                    SlowOpacity(_addEmployeePage);
                }));
            }
        }

        //Список сотрудников
        private RelayCommand _showEmployee;
        public RelayCommand ShowEmployee
        {
            get
            {
                return _showEmployee ?? (_showEmployee = new RelayCommand(obj =>
                {
                    SlowOpacity(_employeePage);
                }));
            }
        }


        //Get Hash
        private string hash;
        public string Hash
        {
            get => hash;
            set
            {
                hash = value;
                OnPropertyChanged("Hash");
            }
        }

        private string unhash;
        public string UnHash
        {
            get => unhash;
            set
            {
                unhash = value;
                OnPropertyChanged("Unhash");
            }
        }

        private RelayCommand getHash;
        public RelayCommand GetHash
        {
            get
            {
                return getHash ?? (getHash = new RelayCommand(obj =>
                {
                    var password = "123456";

                    var encrPass = CryptoPass.GetEncryptPassword(password);

                    Hash = encrPass;

                    UnHash = CryptoPass.GetDecryptPassword(encrPass);

                    //FillSubData fillSubData = new FillSubData();
                    //fillSubData.Fill(30);
                }));
            }
        }


        private void Items_Filter(object sender, FilterEventArgs e)
        {

            NewSubscriber newSubscriber = e.Item as NewSubscriber;

            var fio = newSubscriber.Sername + " " + newSubscriber.Name;

            if (!string.IsNullOrEmpty(SearchText))
            {

                if (string.IsNullOrEmpty(SearchText))
                {
                    e.Accepted = true;
                    return;
                }

                if (newSubscriber != null)
                {
                    if (fio.ToUpper().Contains(SearchText.ToUpper()))
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }

            }

        }

        private async void SlowOpacity(Page page)
        {
            if (CurrentPage != page)
            {
                await Task.Factory.StartNew(() =>
                {
                    for (double i = 1.0; i > 0.0; i -= 0.25)
                    {
                        FrameOpacity = i;
                        Thread.Sleep(50);
                    }

                    CurrentPage = page;

                    for (double i = 0.0; i < 1.1; i += 0.25)
                    {
                        FrameOpacity = i;
                        Thread.Sleep(50);
                    }
                });
            }
        }

        private void SetAvatar()
        {

            if (_employeeType.Equals("Admin"))
            {
                ProfileIMG = @"/Resource/Avatars/admin.png";
                UserUniBTNVisibility = Visibility.Collapsed;
                AdminUniBTNVisibility = Visibility.Visible;
            }
            else
            {
                ProfileIMG = @"/Resource/Avatars/user.png";
                UserUniBTNVisibility = Visibility.Visible;
                AdminUniBTNVisibility = Visibility.Collapsed;
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
