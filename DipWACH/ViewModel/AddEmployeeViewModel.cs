using DipWACH.Helper;
using DipWACH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DipWACH.ViewModel
{
    class AddEmployeeViewModel : INotifyPropertyChanged
    {
        public AddEmployeeViewModel()
        {
            ComboItems = new ObservableCollection<string>();

            List<string>  _types = GetData.GetEmployeeTypes();

            if (_types != null)
            {
                foreach (var type in _types)
                {
                    ComboItems.Add(type);
                }
            }
        }

        private string _fio;
        public string FIO
        {
            get => _fio;
            set
            {
                _fio = value;
                OnPropertyChanged("FIO");
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged("Type");
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


        private ObservableCollection<string> _comboItems;
        public ObservableCollection<string> ComboItems
        {
            get => _comboItems;
            set
            {
                _comboItems = value;
                OnPropertyChanged("ComboItems");
            }
        }


        private RelayCommand _registration;
        public RelayCommand Registration
        {
            get
            {
                return _registration ?? (_registration = new RelayCommand(obj =>
                {
                    using (ModelBD model = new ModelBD())
                    {

                        try
                        {
                            var employee = new Employee()
                            {
                                FIO = FIO,
                                IDTypeEmployee = int.Parse(Type?.Split('-')[0]),
                                Login = Login,
                                Password = CryptoPass.GetHashPassword(Password)
                            };

                            if (employee != null)
                            {
                                model.Employee.Add(employee);
                                model.SaveChanges();

                                PropClear();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось добавить сотрудника!");
                            }

                            MessageBox.Show("Пользователь успешно добавлен");
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Не все поля заполненны");
                        }

                    }
                }));
            }
        }

        private void PropClear()
        {

            FIO = "";
            Type = "";
            Login = "";
            Password = "";

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
