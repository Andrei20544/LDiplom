using DipWACH.Helper;
using DipWACH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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


        public SpecialViewModel()
        {
            InicializeCollection();

            SetFilterBox();

            _calculater = new Calculater();
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

            //var ms = new MemoryStream();
            //var package = Package.Open(ms, FileMode.Create);
            //var doc = new XpsDocument(package);
            //var writer = XpsDocument.CreateXpsDocumentWriter(doc);
            //writer.Write(((IDocumentPaginatorSource)document).DocumentPaginator);
            //doc.Close();
            //package.Close();

            //// Get XPS file bytes
            //var bytes = ms.ToArray();
            //ms.Dispose();

            //// Print to PDF
            //var outputFilePath = @"C:\tmp\test.pdf";
            //PdfFilePrinter.PrintXpsToPdf(bytes, outputFilePath, "Document Title");
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
                    GeneratePDF(obj as FlowDocumentReader);
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
