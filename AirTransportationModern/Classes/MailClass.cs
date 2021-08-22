using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace AirTransportationModern.Classes
{
    public class MailClass
    {
        private string email = "****";
        private string password = "****";
        public void Send(int numOrder, string emailTo)
        {
            try
            {
                if (numOrder != -1 && emailTo != "")
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(email);
                    mail.To.Add(new MailAddress(emailTo));
                    mail.Subject = "Заказ на транспортировку автомобиля";
                    mail.Body = "Здравствуйте, ваш заказ принят в обработку.\n" + "Ваш номер заказа: " + numOrder + "\nС уважением Администрация Air-Legend";
                    mail.Attachments.Add(new Attachment(new RecieptClass().GenerateReciept(numOrder)));
                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.yandex.ru";
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(email, password);
                    client.Send(mail);
                    AdvancedMessageBox.InfoBox("Ваш номер заказа: " + numOrder + "\nНомер заказа продублирован на вашу электронную почту.");
                }
                else
                    AdvancedMessageBox.InfoBox("Неправильные данные");
            }
            catch (Exception){ AdvancedMessageBox.ErrorBox("Не удалось отправить запрос.\n Убедитесь что интернет подключен"); }
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch {return false;}
        }
    }
}
