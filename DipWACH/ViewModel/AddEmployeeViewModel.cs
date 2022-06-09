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
        private ObservableCollection<NewEmployee> _employeeCollection = new ObservableCollection<NewEmployee>();

        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private string _fIO;

        public AddEmployeeViewModel()
        {
            SetComboBox();
        }

        public AddEmployeeViewModel(List<NewEmployee> employees, string fio)
        {
            InicializeEmployee(employees);
            _fIO = fio;
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

        private NewEmployee _selectedEmployee;
        public NewEmployee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
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
                                UpdateEmployeeCollection();
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

        private RelayCommand _deleteEmployee;
        public RelayCommand DeleteEmployee
        {
            get
            {
                return _deleteEmployee ?? (_deleteEmployee = new RelayCommand(obj =>
                {

                    if (SelectedEmployee != null)
                    {
                        using (ModelBD model = new ModelBD())
                        {

                            Employee employee = model.Employee.Where(p => p.ID == SelectedEmployee.ID).FirstOrDefault();

                            if (!employee.FIO.Equals(_fIO))
                            {
                                model.Employee.Remove(employee);
                                model.SaveChanges();

                                UpdateEmployeeCollection();

                                MessageBox.Show($"Пользователь '{employee.FIO}' удален");
                            }
                            else
                            {
                                MessageBox.Show($"Невозможно удалить пользователя!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Что-то пошло не так");
                    }

                }));
            }
        }

        private RelayCommand _changeEmployee;
        public RelayCommand ChangeEmployee
        {
            get
            {
                return _changeEmployee ?? (_changeEmployee = new RelayCommand(obj =>
                {
                    MessageBox.Show("Упс!!");
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

        private void SetComboBox()
        {

            ComboItems = new ObservableCollection<string>();

            List<string> _types = GetData.GetEmployeeTypes();

            if (_types != null)
            {
                foreach (var type in _types)
                {
                    ComboItems.Add(type);
                }
            }

        }

        private void InicializeEmployee(List<NewEmployee> employees)
        {
            foreach (var sub in employees) _employeeCollection.Add(sub);

            _collectionViewSource = new CollectionViewSource { Source = _employeeCollection };
        }

        private void UpdateEmployeeCollection()
        {
            var employees = GetData.GetEmployees();

            _employeeCollection.Clear();
            _collectionViewSource = null;

            foreach (var sub in employees) _employeeCollection.Add(sub);

            _collectionViewSource = new CollectionViewSource { Source = _employeeCollection };

            _collectionViewSource.View.Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
