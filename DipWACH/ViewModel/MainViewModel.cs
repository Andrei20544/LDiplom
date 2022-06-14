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
        private RegLogWindow _regLog;
        private string _employeeType;

        private Page _subscribePage;
        private Page _applicationsPage;
        private Page _addEmployeePage;
        private Page _employeePage;
        private Page _debtsPage;
        private Page _ratePage;

        public MainViewModel(RegLogWindow window, string emplData)
        {
            _regLog = window;
            _employeeType = emplData.Split('-')[1];
            FIO = emplData.Split('-')[0];

            SetAvatar();

            _subscribePage = new View.Pages.SubscribePage();
            _applicationsPage = new View.Pages.ApplicationsPage();
            _addEmployeePage = new View.Pages.AddEmployeePage();
            _employeePage = new View.Pages.EmployeePage(FIO);
            _debtsPage = new View.Pages.DebtsPage();
            _ratePage = new View.Pages.RatePage();

            FrameOpacity = 1;
        }

        public MainViewModel() { }

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

                    Application.Current.MainWindow.Close();
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

        //Задолженности
        private RelayCommand _showDebts;
        public RelayCommand ShowDebts
        {
            get
            {
                return _showDebts ?? (_showDebts = new RelayCommand(obj =>
                {
                    SlowOpacity(_debtsPage);
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

        //Список Тарифов
        private RelayCommand _showRate;
        public RelayCommand ShowRate
        {
            get
            {
                return _showRate ?? (_showRate = new RelayCommand(obj =>
                {
                    SlowOpacity(_ratePage);
                }));
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
                ChangeAvatar(@"/Resource/Avatars/admin.png", Visibility.Collapsed, Visibility.Visible);
            else
                ChangeAvatar(@"/Resource/Avatars/user.png", Visibility.Visible, Visibility.Collapsed);

        }

        private void ChangeAvatar(string path, Visibility userVisible, Visibility adminVisible)
        {
            ProfileIMG = path;
            UserUniBTNVisibility = userVisible;
            AdminUniBTNVisibility = adminVisible;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
