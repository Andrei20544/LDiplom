using DipWACH.Helper;
using DipWACH.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfSharp.Pdf.Printing;
using PdfSharp.Xps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace DipWACH.ViewModel
{
    public class SpecialViewModel : INotifyPropertyChanged
    {
        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private ObservableCollection<NewApartment> _newRegionCollectionFull;


        private List<NewApartment> _newApartments;
        private Calculater _calculater;


        private double _apSumm = 0;
        private double _buSumm = 0;

        private string _filePath;
        private string _fontPath;

        public SpecialViewModel()
        {
            InicializeCollection();

            SetFilterBox();

            _calculater = new Calculater();

            var date = DateTime.Now.ToString().Replace(':', '.');
            _filePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\ отчет-" + date + ".pdf";
            _fontPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Fonts\\Arial.ttf";
        }

        private ObservableCollection<string> _filterRegionSource;
        public ObservableCollection<string> FilterRegionSource
        {
            get => _filterRegionSource;
            set
            {
                _filterRegionSource = value;
                OnPropertyChanged("FilterRegionSource");
            }
        }

        private ObservableCollection<string> _filterSettlementSource;
        public ObservableCollection<string> FilterSettlementSource
        {
            get => _filterSettlementSource;
            set
            {
                _filterSettlementSource = value;
                OnPropertyChanged("FilterSettlementSource");
            }
        }


        private string _filterRegionText;
        public string FilterRegionText
        {
            get => _filterRegionText;
            set
            {
                _filterRegionText = value;
                _collectionViewSource.View.Refresh();
                OnPropertyChanged("FilterRegionText");
            }
        }

        private string _filterSettlementText;
        public string FilterSettlementText
        {
            get => _filterSettlementText;
            set
            {
                _filterSettlementText = value;
                OnPropertyChanged("FilterSettlementText");
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
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

        private string _ratePriceWIn;
        public string RatePriceWIn
        {
            get => _ratePriceWIn;
            set
            {
                _ratePriceWIn = value;
                OnPropertyChanged("RatePriceWIn");
            }
        }

        private string _ratePriceWOut;
        public string RatePriceWOut
        {
            get => _ratePriceWOut;
            set
            {
                _ratePriceWOut = value;
                OnPropertyChanged("RatePriceWOut");
            }
        }

        private string _rateSummWIn;
        public string RateSummWIn
        {
            get => _rateSummWIn;
            set
            {
                _rateSummWIn = value;
                OnPropertyChanged("RateSummWIn");
            }
        }

        private string _rateSummWOut;
        public string RateSummWOut
        {
            get => _rateSummWOut;
            set
            {
                _rateSummWOut = value;
                OnPropertyChanged("RateSummWOut");
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


        private NewApartment _selectedRegion;
        public NewApartment SelectedRegion
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

        private void InicializeCollection()
        {
            DocVisible = Visibility.Collapsed;

            _newApartments = GetData.GetNewApartments();

            _newRegionCollectionFull = new ObservableCollection<NewApartment>();

            foreach (var region in _newApartments) _newRegionCollectionFull.Add(region);

            _collectionViewSource = new CollectionViewSource { Source = _newRegionCollectionFull };
            _collectionViewSource.Filter += Items_Filter;
        }

        private void SetFilterBox()
        {
            FilterRegionSource = new ObservableCollection<string>();

            var regions = GetData.GetRegions();

            foreach (var item in regions) FilterRegionSource.Add(item.Name);
        }

        private void GeneratePDF()
        {
            var document = new Document();

            var streamObj = new FileStream(_filePath, System.IO.FileMode.CreateNew);
            BaseFont baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            PDFGenerator pDFGenerator = new PDFGenerator(document, baseFont, streamObj);

            List<string> nameDataCells = new List<string>
            {
                $"Водоснабжение-{RateSummWIn}",
                $"Водоотведение-{RateSummWOut}"
            };

            //Create PDF
            document.Open();

            pDFGenerator.GenerateSinglePDF("Квитанция", RegionText, nameDataCells, Itog);

            document.Close();
        }

        private void Items_Filter(object sender, FilterEventArgs e)
        {

            NewApartment newApartment = e.Item as NewApartment;

            var name = newApartment.RegionName;

            if (!string.IsNullOrEmpty(FilterRegionText))
            {

                if (string.IsNullOrEmpty(FilterRegionText))
                {
                    e.Accepted = true;
                    return;
                }

                if (newApartment != null)
                {
                    if (name.ToUpper().Contains(FilterRegionText.ToUpper()))
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
                        DocVisible = Visibility.Visible;

                        var summWIn = _calculater.CalculateSpecialSummWIn(SelectedRegion);
                        var summWOut = _calculater.CalculateSpecialSummWOut(SelectedRegion);

                        Date = DateTime.Now;

                        RegionText = SelectedRegion.RegionName;
                        RatePriceWIn = SelectedRegion.RatePriceWIn.ToString();
                        RatePriceWOut = SelectedRegion.RatePriceWOut.ToString();
                        RateSummWIn = summWIn.ToString();
                        RateSummWOut = summWOut.ToString();

                        Itog = (summWIn + summWOut).ToString();
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
                    GeneratePDF();
                    DocVisible = Visibility.Collapsed;

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
