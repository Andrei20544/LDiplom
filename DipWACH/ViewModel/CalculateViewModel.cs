using DipWACH.Helper;
using DipWACH.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows;
using System.Windows.Data;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Windows.Xps.Serialization;
using System.Windows.Documents;
using System.Windows.Controls;

namespace DipWACH.ViewModel
{
    public class CalculateViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<NewRegion> _newRegionCollection = new ObservableCollection<NewRegion>();

        private CollectionViewSource _collectionViewSource;
        public ICollectionView collectionView => _collectionViewSource.View;

        private ObservableCollection<NewRegion> _newRegionCollectionFull;


        private List<NewRegion> _newRegions;
        private Calculater _calculater;


        private double _apSumm = 0;
        private double _buSumm = 0;


        public CalculateViewModel()
        {
            SetComboBox();
            InicializeCollection();

            _calculater = new Calculater();

        }

        private void InicializeCollection()
        {
            DocVisible = Visibility.Collapsed;

            _newRegions = GetData.GetNewRegion();

            _newRegionCollectionFull = new ObservableCollection<NewRegion>();

            foreach (var region in _newRegions) _newRegionCollectionFull.Add(region);

            _collectionViewSource = new CollectionViewSource { Source = _newRegionCollectionFull };
            _collectionViewSource.Filter += Items_Filter;

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

        private void SetComboBox()
        {

            FilterBoxSource = new ObservableCollection<string>();

            List<Region> regions = GetData.GetRegions();

            FilterBoxSource.Add("Все");

            if (regions != null)
            {
                foreach (var type in regions)
                {
                    FilterBoxSource.Add(type.Name);
                }
            }

        }

        private void GeneratePDF(FlowDocumentReader document)
        {
            //var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\calc.pdf";
            //var fontPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Fonts\\Arial.ttf";

            //Document doc = new Document();

            //using (var writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create)))
            //{
            //    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            //    TextRange txt = new TextRange(document.Document.ContentStart, document.Document.ContentEnd);
            //    try
            //    {
            //        doc.Open();
            //        iTextSharp.text.Paragraph pr1 = new iTextSharp.text.Paragraph(txt.Text, font);
            //        doc.Add(pr1);
            //        doc.Close();
            //        writer.Close();
            //        MessageBox.Show("PDF-файл успешно создан");
            //    }
            //    catch (Exception EX)
            //    {
            //        MessageBox.Show(EX.Message);
            //    }
            //}

            //using (var stream = new FileStream("doc.xps", FileMode.Create))
            //{
            //    using (var package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite))
            //    {
            //        using (var xpsDoc = new XpsDocument(package, CompressionOption.Maximum))
            //        {
            //            var rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
            //            var paginator = ((IDocumentPaginatorSource)document).DocumentPaginator;
            //            rsm.SaveAsXaml(paginator);
            //            rsm.Commit();
            //        }
            //    }
            //    stream.Position = 0;
            //    var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(stream);
            //    PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, "doc.pdf", 0);
            //}
        }

        private void Items_Filter(object sender, FilterEventArgs e)
        {

            NewRegion newRegion = e.Item as NewRegion;

            var name = newRegion.Region;

            if (!string.IsNullOrEmpty(SelectFilterBox))
            {

                if (string.IsNullOrEmpty(SelectFilterBox))
                {
                    e.Accepted = true;
                    return;
                }

                if (newRegion != null)
                {
                    if (!SelectFilterBox.Equals("Все"))
                    {
                        if (name.ToUpper().Contains(SelectFilterBox.ToUpper()))
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
                    GeneratePDF(obj as FlowDocumentReader);
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
                    MessageBox.Show($"Общая сумма: {summ}");
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
