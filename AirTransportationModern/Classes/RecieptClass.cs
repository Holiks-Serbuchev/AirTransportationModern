using MySql.Data.MySqlClient;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTransportationModern.Classes
{
    public class RecieptClass
    {
        private int numOrderS { get; set; }
        private string email { get; set; }
        private string cost { get; set; }

        private string directoryPDF = Directory.GetCurrentDirectory() + @"\PDFFiles";
        public string GenerateReciept(int numOrder)
        {
            DataTable dataTable = ClassLogic.LoadData("SELECT aircraft.Model,c1.CountryNameCombo,c1.CityName,service.DepartDate,city.CountryNameCombo," +
                "city.CityName,service.ArrivalDate,vehicle.Email,service.Cost FROM vehicle" +
                " JOIN service ON vehicle.ServiceCombo = service.IDService " +
                "JOIN routes ON service.NumRouteCombo = routes.IDRoute " +
                "JOIN airports ON routes.EndCombo = airports.IDAirport JOIN airports AS a1 ON routes.StartCombo = a1.IDAirport " +
                "JOIN city ON airports.CityCombo = city.IDCity JOIN city AS c1 ON a1.CityCombo = c1.IDCity " +
                $"JOIN aircraft ON service.NumAirCombo = aircraft.IDAir  Where vehicle.VehicleID = {numOrder}");
            object[] mass = dataTable.Rows[0].ItemArray;
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage page = pdfDocument.Pages.Add();
            page.Width = 200;
            page.Height = 300;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Epson", 10, XFontStyle.Regular);
            AddCenterTextToPDF(gfx, font, page, "ООО \"Air-Legend\"", 0);
            AddCenterTextToPDF(gfx, font, page, "Номер заказа: " + numOrder, 10);
            AddLeftTextToPDF(gfx, font, page, "------------------------------------------------------------", 16);
            AddLeftTextToPDF(gfx, font, page, $"Самолет: {mass[0]}", 26);
            AddLeftTextToPDF(gfx, font, page, $"Страна(Откуда): {mass[1]}", 36 + 5);
            AddLeftTextToPDF(gfx, font, page, $"Город(Откуда): {mass[2]}", 46 + 10);
            AddLeftTextToPDF(gfx, font, page, $"Время вылета: {mass[3]}", 56 + 15);
            AddLeftTextToPDF(gfx, font, page, $"Страна(Куда): {mass[4]}", 66 + 20);
            AddLeftTextToPDF(gfx, font, page, $"Город(Куда): {mass[5]}", 76 + 25);
            AddLeftTextToPDF(gfx, font, page, $"Время прибытия: {mass[6]}", 86 + 30);
            AddLeftTextToPDF(gfx, font, page, $"Email: {mass[7]}", 96 + 35);
            AddLeftTextToPDF(gfx, new XFont("Epson", 16,XFontStyle.Bold), page, $"Цена: {mass[8]}.00₽", 106 + 40);
            numOrderS = numOrder;
            email = mass[7].ToString();
            cost = mass[8].ToString() + ".00₽";
            DrawImage(gfx, GenerateQRCode(),100 / 2,131 + 45,100,100,page);
            pdfDocument.Save(directoryPDF + @"\Doc"+ numOrder +".pdf");
            pdfDocument.Close();
            return directoryPDF + @"\Doc" + numOrder + ".pdf";
        }
        private Bitmap GenerateQRCode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"Номер заказа: {numOrderS}\nEmail: {email}\nЦена: {cost}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }
        private void DrawImage(XGraphics gfx, Bitmap bitmap, int x, int y, int width, int height, PdfPage page)
        {
            var stream = new System.IO.MemoryStream();
            GenerateQRCode().Save(stream, ImageFormat.Png);
            stream.Position = 0;
            XImage image = XImage.FromStream(stream);
            gfx.DrawImage(image, x, y, width, height);
        }
        private void AddCenterTextToPDF(XGraphics gfx, XFont font, PdfPage page,string text,int position)
        {
            gfx.DrawString(text, font, XBrushes.Black,new XRect(0, position, page.Width, page.Height),XStringFormats.TopCenter);
        }
        private void AddLeftTextToPDF(XGraphics gfx, XFont font, PdfPage page, string text, int position)
        {
            gfx.DrawString(text, font, XBrushes.Black, new XRect(0, position, page.Width, page.Height), XStringFormats.TopLeft);
        }
    }
}
