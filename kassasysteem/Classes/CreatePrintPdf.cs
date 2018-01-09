using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace kassasysteem.Classes
{
    internal class CreatePrintPdf
    {
        public static void CreateReceipt(string name, int bonnummer)
        {
            var document = new PdfDocument();
            var page = document.Pages.Add();
            page.Graphics.DrawString("Groene Vingers\n", new PdfStandardFont(PdfFontFamily.TimesRoman, 32), PdfBrushes.YellowGreen, 20, 10);
            page.Graphics.DrawString(DateTime.Now.Date.Day + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Year +  " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "\n" + name + " \nBonnummer: " + bonnummer, new PdfStandardFont(PdfFontFamily.TimesRoman, 16), PdfBrushes.Black, 10, 60);
            page.Graphics.DrawLine(new PdfPen(Color.Black),10,125, 400, 125);
            page.Graphics.DrawString("Aantal           Omschrijving                                 Bedrag", new PdfStandardFont(PdfFontFamily.TimesRoman, 16), PdfBrushes.Black, 10, 130);
            page.Graphics.DrawLine(new PdfPen(Color.Black), 10, 153, 400, 153);
            page.Graphics.DrawString("1                    Consultancy per uur                      €75,00", new PdfStandardFont(PdfFontFamily.TimesRoman, 16), PdfBrushes.Black, 10, 160);
            page.Graphics.DrawLine(new PdfPen(Color.Black), 10, 183, 400, 183);
            page.Graphics.DrawString("1                    Subtotaal:                                      €75,00", new PdfStandardFont(PdfFontFamily.TimesRoman, 16), PdfBrushes.Black, 10, 190);
            var stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);
            Save(stream, "Bon" + bonnummer + ".pdf");
        }

        private static async void Save(Stream stream, string filename)
        {
            stream.Position = 0;
            StorageFile stFile;
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent($"Windows.Phone.UI.Input.HardwareButtons"))
            {
                var savePicker = new FileSavePicker
                {
                    DefaultFileExtension = ".pdf",
                    SuggestedFileName = filename
                };
                savePicker.FileTypeChoices.Add("Adobe PDF Document", new List<string>() { ".pdf" });
                stFile = await savePicker.PickSaveFileAsync();
            }
            else
            {
                var local = ApplicationData.Current.LocalFolder;
                stFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            if (stFile == null) return;
            var fileStream = await stFile.OpenAsync(FileAccessMode.ReadWrite);
            var st = fileStream.AsStreamForWrite();
            st.Write((stream as MemoryStream)?.ToArray(), 0, (int)stream.Length);
            st.Flush();
            st.Dispose();
            fileStream.Dispose();
        }
    }
}
