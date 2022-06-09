using DipWACH.Helper;
using DipWACH.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DipWACH.ViewModel
{
    class LogINViewModel : INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private LoadWindow _loadWindow;

        private RegLogWindow _regLog;

        private string _data;

        public LogINViewModel(RegLogWindow window)
        {
            _loadWindow = new LoadWindow();
            _regLog = window;
            LoadVisible = Visibility.Collapsed;
        }

        private Visibility _loadVisible;
        public Visibility LoadVisible
        {
            get => _loadVisible;
            set
            {
                _loadVisible = value;
                OnPropertyChanged("LoadVisible");
            }
        }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged("ErrorText");
            }
        }

        //Вход
        private RelayCommand _logInApp;
        public RelayCommand LogInApp
        {
            get
            {
                return _logInApp ?? (_logInApp = new RelayCommand(obj =>
                {
                    Application.Current.MainWindow = _regLog;
                    _mainWindow = null;

                    _loadWindow.Show();
                    _data = DataCompare.CompareEmployeeData(Login, Password);
                    _loadWindow.Hide();

                    if (_data != null)
                    {
                        //_mainWindow = MainWindow.GetInstance(_regLog, _data);
                        _mainWindow = new MainWindow(_regLog, _data);

                        LogIN(_mainWindow, _data.Split('-')[0]);

                        Application.Current.MainWindow = _mainWindow;
                    }
                    else
                    {
                        ErrorText = "Неверные логин или пароль";
                    }
                }));
            }
        }

        //private void Logg()
        //{
        //    MainWindow main = new MainWindow();

        //    LoadVisible = Visibility.Visible;
        //    _data = DataCompare.CompareEmployeeData(Login, Password);
        //    LoadVisible = Visibility.Collapsed;

        //    LogIN(main, _data);
        //}
        private void LogIN(MainWindow main, string data)
        {
            ErrorText = "";

            //main.FioText.Text = data;
            main.Show();

            Application.Current.MainWindow.Hide();

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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
