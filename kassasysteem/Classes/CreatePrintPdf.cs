using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace kassasysteem.Classes
{
    internal class CreatePrintPdf
    {
        public static void CreateReceipt()
        {
            var document = new PdfDocument();
            var page = document.Pages.Add();
            page.Graphics.DrawString("Groene Vingers", new PdfStandardFont(PdfFontFamily.TimesRoman, 32), PdfBrushes.YellowGreen, 10, 10);
            page.Graphics.DrawString("", new PdfStandardFont(PdfFontFamily.TimesRoman, 12), PdfBrushes.Black, 10,10);
            var stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);
            Save(stream, "Result.pdf");
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
                    SuggestedFileName = "receipt"
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
