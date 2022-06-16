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

namespace DipWACH.ViewModel
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<NewMConnection> _connectionCollection = new ObservableCollection<NewMConnection>();

        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private List<NewMConnection> _connections;

        public ApplicationViewModel()
        {
            InicializeRate();
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

        private NewMConnection _selectedMConnection;
        public NewMConnection SelectedMConnection
        {
            get => _selectedMConnection;
            set
            {
                _selectedMConnection = value;
                OnPropertyChanged("SelectedMConnection");
            }
        }


        private RelayCommand _acceptBTN;
        public RelayCommand AcceptBTN
        {
            get
            {
                return _acceptBTN ?? (_acceptBTN = new RelayCommand(obj =>
                {
                    if (SelectedMConnection != null)
                    {
                        if (SelectedMConnection.Accept != 0)
                        {
                            using (ModelBD model = new ModelBD())
                            {
                                MConnection connection = model.MConnection.Where(p => p.ID == SelectedMConnection.ID).FirstOrDefault();
                                connection.Status = "Подтверждена";

                                model.Entry(connection).State = System.Data.Entity.EntityState.Modified;
                                model.SaveChanges();

                                MessageBox.Show($"Заявка подтверждена");

                                UpdateEmployeeCollection();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Заявка не соответсвует критериям!");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Заявка не выбрана!");
                    }
                }));
            }
        }

        private RelayCommand _rejectedBTN;
        public RelayCommand RejectedBTN
        {
            get
            {
                return _rejectedBTN ?? (_rejectedBTN = new RelayCommand(obj =>
                {
                    if (SelectedMConnection != null)
                    {
                        using (ModelBD model = new ModelBD())
                        {
                            MConnection connection = model.MConnection.Where(p => p.ID == SelectedMConnection.ID).FirstOrDefault();
                            connection.Status = "Отклонена";

                            model.Entry(connection).State = System.Data.Entity.EntityState.Modified;
                            model.SaveChanges();

                            MessageBox.Show($"Заявка отклонена");

                            UpdateEmployeeCollection();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Заявка не выбрана!");
                    }
                }));
            }
        }

        private void Items_Filter(object sender, FilterEventArgs e)
        {

            NewMConnection newConnection = e.Item as NewMConnection;

            var name = newConnection.Address;

            if (!string.IsNullOrEmpty(SearchText))
            {

                if (string.IsNullOrEmpty(SearchText))
                {
                    e.Accepted = true;
                    return;
                }

                if (newConnection != null)
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

        private void InicializeRate()
        {
            _connections = GetData.GetConnection();

            foreach (var connection in _connections) _connectionCollection.Add(connection);

            _collectionViewSource = new CollectionViewSource { Source = _connectionCollection };

            _collectionViewSource.Filter += Items_Filter;
        }

        private void UpdateEmployeeCollection()
        {
            _connectionCollection.Clear();
            _collectionViewSource = null;

            InicializeRate();

            _collectionViewSource.View.Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
