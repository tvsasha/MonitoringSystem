using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using PdfSharp.Drawing.Layout;
namespace MonitoringSystem2
{
    /// <summary>
    /// Логика взаимодействия для CalibratorsWindows.xaml
    /// </summary>
    public partial class CalibratorsWindows : Window
    {
        MonitoringSystemDBEntities db;
        public int EmployeeID;
        public CalibratorsWindows(int employeeID)
        {
            InitializeComponent();
            db = new MonitoringSystemDBEntities();
            LoadData();
            EmployeeID = employeeID;
        }

        private void LoadData()
        {
            var calibrationResults = from calibration in db.Calibration
                                     join calibrationType in db.CalibrationType on calibration.CalibrationTypeID equals calibrationType.CalibrationTypeID
                                     join employee in db.Employee on calibration.EmployeeID equals employee.EmployeeID
                                     select new
                                     {
                                         CalibrationID = calibration.CalibrationID,
                                         CalibrationDate = calibration.CalibrationDate,
                                         Calibrator = employee.FullName,
                                         CalibrationType = calibrationType.CalibrationName,
                                         Description = calibration.Description
                                     };

            CalibrationResultsDataGrid.ItemsSource = calibrationResults.ToList();
        }

        private void ExportToPdf(string filePath)
        {
            try
            {
                // Создаём новый PDF-документ
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Calibration Results";

                // Добавляем страницу
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Настройки шрифта
                XFont titleFont = new XFont("Verdana", 10);
                XFont headerFont = new XFont("Verdana", 8);
                XFont bodyFont = new XFont("Verdana", 7);

                // Заголовок
                gfx.DrawString("Calibration Results", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, 40), XStringFormats.Center);

                // Координаты для таблицы
                double y = 50;
                double x = 20;
                double cellHeight = 15;

                // Заголовки таблицы
                string[] headers = { "ID", "Date", "Calibrator", "Calibration Type" };
                double[] columnWidths = { 30, 50, 100, 100 };

                // Рисуем заголовки таблицы
                for (int i = 0; i < headers.Length; i++)
                {
                    gfx.DrawRectangle(XPens.Black, x, y, columnWidths[i], cellHeight);
                    gfx.DrawString(headers[i], headerFont, XBrushes.Black, new XRect(x + 2, y + 2, columnWidths[i] - 4, cellHeight), XStringFormats.TopLeft);
                    x += columnWidths[i];
                }

                y += cellHeight;
                x = 20;

                // Добавляем данные из DataGrid
                foreach (var item in CalibrationResultsDataGrid.Items)
                {
                    dynamic row = item;

                    string[] values =
                    {
                row.CalibrationID.ToString(),
                row.CalibrationDate.ToString("yyyy-MM-dd"),
                row.Calibrator.ToString(),
                row.CalibrationType.ToString()
            };

                    for (int i = 0; i < values.Length; i++)
                    {
                        // Расчёт высоты ячейки для многострочного текста
                        var layout = new XTextFormatter(gfx);
                        var text = values[i];
                        var cellRect = new XRect(x, y, columnWidths[i], cellHeight * 2); // Позволяем высоту до двух строк
                        gfx.DrawRectangle(XPens.Black, cellRect); // Рамка ячейки
                        layout.DrawString(text, bodyFont, XBrushes.Black, cellRect, XStringFormats.TopLeft); // Текст с переносом

                        x += columnWidths[i];
                    }

                    y += cellHeight * 2; // Увеличиваем высоту строки (для переноса)
                    x = 20;

                    // Переход на новую страницу при необходимости
                    if (y + cellHeight > page.Height - 40)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = 40;

                        // Перерисовка заголовков на новой странице
                        x = 20;
                        for (int i = 0; i < headers.Length; i++)
                        {
                            gfx.DrawRectangle(XPens.Black, x, y, columnWidths[i], cellHeight);
                            gfx.DrawString(headers[i], headerFont, XBrushes.Black, new XRect(x + 2, y + 2, columnWidths[i] - 4, cellHeight), XStringFormats.TopLeft);
                            x += columnWidths[i];
                        }

                        y += cellHeight;
                        x = 20;
                    }
                }

                // Сохраняем документ
                document.Save(filePath);
                MessageBox.Show("PDF успешно экспортирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте PDF: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "CalibrationResults.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportToPdf(saveFileDialog.FileName);
            }
        }

        private void QuitButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void StartButton(object sender, RoutedEventArgs e)
        {
            CalibratorWindow window = new CalibratorWindow(EmployeeID);
            window.Show();
            this.Close();
        }
    }
}
