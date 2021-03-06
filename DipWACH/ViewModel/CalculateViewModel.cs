using DipWACH.Helper;
using DipWACH.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls;
using iTextSharp.text.pdf.draw;

namespace DipWACH.ViewModel
{
    public class CalculateViewModel : INotifyPropertyChanged
    {
        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private ObservableCollection<NewRegion> _newRegionCollectionFull;


        private List<NewRegion> _newRegions;
        private Calculater _calculater;


        private double _apSumm = 0;
        private double _buSumm = 0;

        private string _filePath;
        private string _fontPath;

        public CalculateViewModel()
        {
            InicializeCollection();

            _calculater = new Calculater();

            var date = DateTime.Now.ToString().Replace(':', '.');
            _filePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\отчет-" + date + ".pdf";
            _fontPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Fonts\\Arial.ttf";
        }

        private ObservableCollection<string> _filterBoxSource;
        public ObservableCollection<string> FilterBoxSource
        {
            get => _filterBoxSource;
            set
            {
                _filterBoxSource = value;
                OnPropertyChanged("FilterBoxSource");
            }
        }


        private string _selectFilterBox;
        public string SelectFilterBox
        {
            get => _selectFilterBox;
            set
            {
                _selectFilterBox = value;
                _collectionViewSource.View.Refresh();
                OnPropertyChanged("SelectFilterBox");
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

        private string _regionText;
        public string RegionText
        {
            get => _regionText;
            set
            {
                _regionText = value;
                OnPropertyChanged("RegionText");
            }
        }

        private string _buildingSumm;
        public string BuildingSumm
        {
            get => _buildingSumm;
            set
            {
                _buildingSumm = value;
                OnPropertyChanged("BuildingSumm");
            }
        }

        private string _apartmentSumm;
        public string ApartmentSumm
        {
            get => _apartmentSumm;
            set
            {
                _apartmentSumm = value;
                OnPropertyChanged("ApartmentSumm");
            }
        }

        private string _itog;
        public string Itog
        {
            get => _itog;
            set
            {
                _itog = value;
                OnPropertyChanged("Itog");
            }
        }

        private string _allSumm;
        public string AllSumm
        {
            get => _allSumm;
            set
            {
                _allSumm = value;
                OnPropertyChanged("AllSumm");
            }
        }


        private NewRegion _selectedRegion;
        public NewRegion SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                _selectedRegion = value;
                OnPropertyChanged("SelectedRegion");
            }
        }

        private Visibility _docVisible;
        public Visibility DocVisible
        {
            get => _docVisible;
            set
            {
                _docVisible = value;
                OnPropertyChanged("DocVisible");
            }
        }

        private Visibility _visibleGridAutoCalc;
        public Visibility VisibleGridAutoCalc
        {
            get => _visibleGridAutoCalc;
            set
            {
                _visibleGridAutoCalc = value;
                OnPropertyChanged("VisibleGridAutoCalc");
            }
        }

        private void InicializeCollection()
        {
            DocVisible = Visibility.Collapsed;
            VisibleGridAutoCalc = Visibility.Collapsed;

            _newRegions = GetData.GetNewRegion();

            _newRegionCollectionFull = new ObservableCollection<NewRegion>();

            foreach (var region in _newRegions) _newRegionCollectionFull.Add(region);

            _collectionViewSource = new CollectionViewSource { Source = _newRegionCollectionFull };
            _collectionViewSource.Filter += Items_Filter;

        }

        private void GeneratePDF(string title, List<string> nameDataCells, bool isMulti = false)
        {
            try
            {
                var document = new Document();

                var streamObj = new FileStream(_filePath, FileMode.CreateNew);
                BaseFont baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                PDFGenerator pDFGenerator = new PDFGenerator(document, baseFont, streamObj);

                //Create PDF
                document.Open();

                pDFGenerator.GenerateSinglePDF(title, RegionText, nameDataCells, Itog, isMulti);

                document.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Items_Filter(object sender, FilterEventArgs e)
        {

            NewRegion newRegion = e.Item as NewRegion;

            var name = newRegion.Settlements;

            if (!string.IsNullOrEmpty(SearchText))
            {

                if (string.IsNullOrEmpty(SearchText))
                {
                    e.Accepted = true;
                    return;
                }

                if (newRegion != null && !string.IsNullOrEmpty(name))
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


        private RelayCommand _calculate;
        public RelayCommand Calculate
        {
            get
            {
                return _calculate ?? (_calculate = new RelayCommand(obj =>
                {

                    if (SelectedRegion != null)
                    {
                        var apartments = GetData.GetApartments(SelectedRegion.IDRegion);
                        var buildings = GetData.GetBuildings(SelectedRegion.IDRegion);

                        _apSumm = _calculater.CalculateApartmentSumm(apartments);
                        _buSumm = _calculater.CalculateBuildingSumm(buildings);

                        DocVisible = Visibility.Visible;

                        RegionText = SelectedRegion.Region;

                        BuildingSumm = _buSumm.ToString();
                        ApartmentSumm = _apSumm.ToString();

                        Itog = (_buSumm + _apSumm).ToString();
                    }
                    else
                    {
                        MessageBox.Show("Выберите регион");
                    }

                }));
            }
        }

        private RelayCommand _docClose;
        public RelayCommand DocClose
        {
            get
            {
                return _docClose ?? (_docClose = new RelayCommand(obj =>
                {

                    DocVisible = Visibility.Collapsed;

                }));
            }
        }

        private RelayCommand _saveInPDF;
        public RelayCommand SaveInPDF
        {
            get
            {
                return _saveInPDF ?? (_saveInPDF = new RelayCommand(obj =>
                {
                    List<string> nameDataCells = new List<string>
                    {
                        $"Сумма по частным домам-{BuildingSumm}",
                        $"Сумма по квартирам-{ApartmentSumm}"
                    };
                    GeneratePDF("Отчет по региону", nameDataCells);
                    DocVisible = Visibility.Collapsed;

                }));
            }
        }

        private RelayCommand _autoCalculate;
        public RelayCommand AutoCalculate
        {
            get
            {
                return _autoCalculate ?? (_autoCalculate = new RelayCommand(obj =>
                {
                    var summ = _calculater.AutoCalculate();
                    var summs = _calculater.AutoCalculateForPDF();
                    Itog = summ.ToString();
                    GeneratePDF("Отчет по всем регионам", summs, true);

                    AllSumm = summ.ToString() + " руб.";
                    VisibleGridAutoCalc = Visibility.Visible;
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
                    VisibleGridAutoCalc = Visibility.Collapsed;

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
