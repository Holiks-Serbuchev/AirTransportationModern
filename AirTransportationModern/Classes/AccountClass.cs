using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTransportationModern.Classes
{
    public class AccountClass
    {
        private int ID { get; set; }
        private string Surname { get; set; }
        private string Name { get; set; }
        private string Patronymic { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string Position { get; set; }
        private string TelephoneNum { get; set; }
        private byte[] Image { get; set; }
        public void SetAccount(int id, string surname, string name, string patronymic, string email, string password, string position,string telephone, byte[] image)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Email = email;
            Password = password;
            Position = position;
            TelephoneNum = telephone;
            Image = image;
        }
        public void ResetAccount()
        {
            ID = -1;
            Surname = String.Empty;
            Name = String.Empty;
            Patronymic = String.Empty;
            Email = String.Empty;
            Password = String.Empty;
            Position = String.Empty;
            TelephoneNum = String.Empty;
            Image = null;
        }
        public int GetIDAccount() => ID;
        public string GetSurnameAccount() => Surname;
        public string GetPatronymicAccount() => Patronymic;
        public string GetNameAccount() => Name;
        public string GetEmailAccount() => Email;
        public string GetPasswordAccount() => Password;
        public string GetPostionAccount() => Position;
        public string GetTelephoneAccount() => TelephoneNum;
        public byte[] GetImageAccount() => Image;
    }
}
