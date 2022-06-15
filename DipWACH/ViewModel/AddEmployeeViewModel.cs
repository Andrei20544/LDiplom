using DipWACH.Helper;
using DipWACH.Model;
using DipWACH.View;
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
        private ObservableCollection<NewEmployee> _employeeCollection;

        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private List<NewEmployee> _employees;

        private FilterEventHandler _filter;

        private string _fIO;

        public AddEmployeeViewModel()
        {
            SetComboBox();
            VisibleGrid = Visibility.Collapsed;
        }

        public AddEmployeeViewModel(string fio)
        {
            _employeeCollection = new ObservableCollection<NewEmployee>();

            InicializeEmployee();

            _fIO = fio;
            VisibleGrid = Visibility.Collapsed;
        }

        private Visibility _visibleGrid;
        public Visibility VisibleGrid
        {
            get => _visibleGrid;
            set
            {
                _visibleGrid = value;
                OnPropertyChanged("VisibleGrid");
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

        private string _changePasswordTextBox;
        public string ChangePasswordTextBox
        {
            get => _changePasswordTextBox;
            set
            {
                _changePasswordTextBox = value;
                OnPropertyChanged("ChangePasswordTextBox");
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
                        MessageBox.Show($"Пользователь не выбран!");
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
                    if (SelectedEmployee != null)
                    {
                        //using (ModelBD model = new ModelBD())
                        //{
                        //    _dipDialog = null;
                        //    _dipDialog = new DipDialog();

                        //    if (_dipDialog.ShowDialog() == true)
                        //    {
                        //        if (!string.IsNullOrEmpty(_dipDialog.Password))
                        //        {

                        //            Employee employee = model.Employee.Where(p => p.ID == SelectedEmployee.ID).FirstOrDefault();
                        //            employee.Password = CryptoPass.GetHashPassword(_dipDialog.Password);

                        //            model.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                        //            model.SaveChanges();

                        //            MessageBox.Show($"Пароль для пользователя {employee.FIO} изменен");

                        //            UpdateEmployeeCollection();

                        //        }
                        //    }

                        //}
                        ChangePassGridVisible(Visibility.Visible);
                    }
                    else
                    {
                        MessageBox.Show($"Пользователь не выбран!");
                    }
                }));
            }
        }

        private RelayCommand _acceptBTN;
        public RelayCommand AcceptBTN
        {
            get
            {
                return _acceptBTN ?? (_acceptBTN = new RelayCommand(obj =>
                {
                    if (SelectedEmployee != null)
                    {
                        using (ModelBD model = new ModelBD())
                        {
                            if (!string.IsNullOrEmpty(ChangePasswordTextBox))
                            {

                                Employee employee = model.Employee.Where(p => p.ID == SelectedEmployee.ID).FirstOrDefault();
                                employee.Password = CryptoPass.GetHashPassword(ChangePasswordTextBox);

                                model.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                                model.SaveChanges();

                                VisibleGrid = Visibility.Collapsed;
                                
                                MessageBox.Show($"Пароль для пользователя {employee.FIO} изменен");

                                UpdateEmployeeCollection();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Пользователь не выбран!");
                    }
                }));
            }
        }

        private RelayCommand _cancelBTN;
        public RelayCommand CancelBTN
        {
            get
            {
                return _cancelBTN ?? (_cancelBTN = new RelayCommand(obj =>
                {
                    ChangePassGridVisible(Visibility.Collapsed);
                }));
            }
        }

        private void Items_Filter(object sender, FilterEventArgs e)
        {

            NewEmployee newEmployee = e.Item as NewEmployee;

            var name = newEmployee.Name + newEmployee.MiddleName + newEmployee.Sername;

            if (!string.IsNullOrEmpty(SearchText))
            {

                if (string.IsNullOrEmpty(SearchText))
                {
                    e.Accepted = true;
                    return;
                }

                if (newEmployee != null)
                {

                    if (name.ToUpper().Contains(SearchText.ToUpper()))
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

        private void ChangePassGridVisible(Visibility visibility)
        {
            ChangePasswordTextBox = "";
            VisibleGrid = visibility;
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

        private void InicializeEmployee()
        {
            _employees = GetData.GetEmployees();

            foreach (var sub in _employees) _employeeCollection.Add(sub);

            _collectionViewSource = new CollectionViewSource { Source = _employeeCollection };

            _filter = new FilterEventHandler(Items_Filter);
            _collectionViewSource.Filter += _filter;
        }

        private void UpdateEmployeeCollection()
        {
            _employeeCollection.Clear();
            _collectionViewSource.Filter -= _filter;

            InicializeEmployee();

            _collectionViewSource.View.Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
