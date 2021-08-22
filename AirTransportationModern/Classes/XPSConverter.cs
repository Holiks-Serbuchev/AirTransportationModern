using Microsoft.Office.Interop.Word;
using System;
using System.Windows.Xps.Packaging;

namespace AirTransportationModern.Classes
{
    public static class XPSConverter
    {
        public static XpsDocument ConvertWordDocToXPSDoc(string wordDocName, string xpsDocName)
        {
            Application wordApplication = new Application();
            wordApplication.Documents.Add(wordDocName);
            Document doc = wordApplication.ActiveDocument;
            try
            {
                doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS);
                wordApplication.Quit();
                XpsDocument xpsDoc = new XpsDocument(xpsDocName, System.IO.FileAccess.Read);
                return xpsDoc;
            }
            catch (Exception exp){string str = exp.Message; AdvancedMessageBox.ErrorBox("Нельзя перезаписывать файл"); }
            return new XpsDocument("", System.IO.FileAccess.Read);
        }
    }
}
