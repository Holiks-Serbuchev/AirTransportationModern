using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirTransportationModern.Classes
{
    public static class InputProtection
    {
        public static bool NumberLetter(TextCompositionEventArgs e)
        {
            if (Char.IsLetter(Convert.ToChar(e.Text)) == true || Char.IsDigit(e.Text, 0) == true)
                return false;
            else { return true; }
        }
        public static bool NumberPoint(TextCompositionEventArgs e, bool Check)
        {
            if (Char.IsDigit(e.Text, 0) == true || (e.Text == "." && Check == false))
                return false;
            else
                return true;
        }
        public static bool BlockSpace(KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
                return true;
            return false;
        }
        public static bool Number(TextCompositionEventArgs e) => Char.IsDigit(e.Text, 0) == true ? false : true;
        public static bool Letter(TextCompositionEventArgs e) => Char.IsLetter(Convert.ToChar(e.Text)) == true ? false : true;
    }
}
