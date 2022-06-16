using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.IO;

namespace DipWACH.Helper
{
    public class PDFGenerator
    {
        private PdfPTable _table;
        private PdfPCell _cell;

        private Chunk _linebreak;
        private Phrase _phrase;
        private Paragraph _paragraph;

        private string _fontPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Fonts\\Arial.ttf";
        private BaseFont _baseFont;
        private Font _font;
        private Font _oldFont = new Font(Font.FontFamily.TIMES_ROMAN, 14f, Font.BOLD, BaseColor.BLACK);
        private Font _defFont = new Font();

        private FileStream _fileStream;

        private Document _document;

        public PDFGenerator(Document document, BaseFont baseFont, FileStream fileStream)
        {
            //_fileStream = new FileStream(_fontPath, FileMode.CreateNew);
            _fileStream = fileStream;
            _document = document;
            PdfWriter.GetInstance(document, _fileStream);

            _baseFont = baseFont;
            //_baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            _font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
        }

        public void GenerateSinglePDF(string title, string reginName, List<string> nameDataCells, string Itog)
        {

            SetParagraph(title, 22f);

            SetLine();

            SetParagraph($"Регион: {reginName}");

            SetLine();

            GenerateTable(2, DateTime.Now.ToString(), nameDataCells);

            SetLine();

            SetParagraph($"ИТОГО: {Itog}");

        }

        public void GenerateMultiPDF(string title, List<string> regions, List<string> nameDataCells, string Itog)
        {

            SetParagraph(title, 22f);

            foreach (var item in regions)
            {

                SetLine();

                SetParagraph($"Регион: {item}");

                SetLine();

                GenerateTable(2, DateTime.Now.ToString(), nameDataCells);

                SetLine();

            }

            SetParagraph($"ИТОГО: {Itog}");

        }

        public void GenerateTable(int columns, string nameTable, List<string> nameDataCells)
        {
            _table = new PdfPTable(columns);

            _phrase = new Phrase(nameTable, _font);

            _cell = new PdfPCell(_phrase);
            _cell.Colspan = columns;
            _cell.HorizontalAlignment = 1;
            _cell.VerticalAlignment = 1;
            _cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _table.AddCell(_cell);

            foreach (var item in nameDataCells)
            {
                var nameCell = item.Split('-')[0];
                var dataCell = item.Split('-')[1];

                _table.AddCell(new PdfPCell(_phrase = new Phrase(nameCell, _font)));
                _table.AddCell(dataCell);
            }

            _document.Add(_table);

        }

        public void SetParagraph(string name)
        {
            _paragraph = new Paragraph(name, _font);
            _document.Add(_paragraph);
        }

        public void SetParagraph(string name, float size)
        {
            _paragraph = new Paragraph(name, new Font(_baseFont, size, Font.NORMAL));
            _document.Add(_paragraph);
        }

        public void SetLine()
        {
            _linebreak = new Chunk(new LineSeparator());
            _document.Add(_linebreak);
        }
    }
}
