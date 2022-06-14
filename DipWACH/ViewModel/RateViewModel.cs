using DipWACH.Helper;
using DipWACH.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace DipWACH.ViewModel
{
    class RateViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Rate> _rateCollection = new ObservableCollection<Rate>();

        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private List<Rate> _rates;

        public RateViewModel()
        {
            VisibleGrid = Visibility.Collapsed;
            InicializeRate();
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

        private string _priceINTextBox;
        public string PriceINTextBox
        {
            get => _priceINTextBox;
            set
            {
                _priceINTextBox = value;
                OnPropertyChanged("PriceINTextBox");
            }
        }

        private string _priceOUTTextBox;
        public string PriceOUTTextBox
        {
            get => _priceOUTTextBox;
            set
            {
                _priceOUTTextBox = value;
                OnPropertyChanged("PriceOUTTextBox");
            }
        }

        private Rate _selectedRate;
        public Rate SelectedRate
        {
            get => _selectedRate;
            set
            {
                _selectedRate = value;
                OnPropertyChanged("SelectedRate");
            }
        }


        private RelayCommand _changeRate;
        public RelayCommand ChangeRate
        {
            get
            {
                return _changeRate ?? (_changeRate = new RelayCommand(obj =>
                {
                    if (SelectedRate != null)
                    {
                        ChangePassGridVisible(Visibility.Visible);
                        PriceINTextBox = SelectedRate.PriceWInto.ToString();
                        PriceOUTTextBox = SelectedRate.PriceWOut.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Тариф не выбран");
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
                    if (SelectedRate != null)
                    {
                        using (ModelBD model = new ModelBD())
                        {
                            if (!string.IsNullOrEmpty(PriceINTextBox) && !string.IsNullOrEmpty(PriceOUTTextBox))
                            {

                                Rate rate = model.Rate.Where(p => p.ID == SelectedRate.ID).FirstOrDefault();
                                rate.PriceWInto = double.Parse(PriceINTextBox);
                                rate.PriceWOut = double.Parse(PriceOUTTextBox);

                                model.Entry(rate).State = System.Data.Entity.EntityState.Modified;
                                model.SaveChanges();

                                VisibleGrid = Visibility.Collapsed;

                                MessageBox.Show($"Тариф изменен");

                                UpdateEmployeeCollection();

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Тариф не выбран!");
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

        private void InicializeRate()
        {
            _rates = GetData.GetRate();

            foreach (var rate in _rates) _rateCollection.Add(rate);

            _collectionViewSource = new CollectionViewSource { Source = _rateCollection };
        }

        private void UpdateEmployeeCollection()
        {
            _rateCollection.Clear();
            _collectionViewSource = null;

            InicializeRate();

            _collectionViewSource.View.Refresh();
        }

        private void ChangePassGridVisible(Visibility visibility)
        {
            PriceINTextBox = "";
            PriceOUTTextBox = "";
            VisibleGrid = visibility;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
