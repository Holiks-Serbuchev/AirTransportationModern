using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTransportationModern.Classes
{
    public static class ProtectedCellsClass
    {
        private enum func
        {
            None,
            Text,
            Number,
            NumberPoint
        }
        private static List<string> originalCells = new List<string>() {
            "Modelvehicle","Weightvehicle","Widthvehicle","Heightvehicle","Lengthvehicle",
            "Emailvehicle","Surnameworkers","Namesworkers","Patronymicworkers",
            "Emailworkers","CountryNamecountry","CityNamecity","Costservice",
            "ColorNamecolors","MarksNamemarks","PositionNameposition","StatusNamestatus_route",
            "StatusNamestatuses","VehicleTypevehicle_type","Numcharacteristic","Nameunit","Nameattribute",
            "Nameairports","Adressairports","Countcount_air","Modelaircraft","TypeAirair_transportation"
        };
        private static List<func> funcCells = new List<func>() {
            func.None,func.NumberPoint,func.Number,func.Number,func.Number,
            func.None,func.Text,func.Text,func.Text,
            func.None,func.Text,func.None,func.NumberPoint,
            func.Text,func.None,func.Text,func.Text,
            func.Text,func.Text,func.NumberPoint,func.None,func.Text,
            func.None,func.None,func.Number,func.None,func.Text
        };
        public static void ProtectCells(ref TextBox textBox, string originalCell)
        {
            try
            {
                int num = originalCells.IndexOf(originalCell);
                if (funcCells.ElementAt(num) == func.Number)
                {
                    textBox.MaxLength = 11;
                    textBox.PreviewTextInput += (s, a) => a.Handled = InputProtection.Number(a);
                    textBox.PreviewKeyDown += (s, a) => a.Handled = InputProtection.BlockSpace(a);
                }
                else if (funcCells.ElementAt(num) == func.Text)
                {
                    textBox.MaxLength = 45;
                    textBox.PreviewTextInput += (s, a) => a.Handled = InputProtection.Letter(a);
                }
                else if (funcCells.ElementAt(num) == func.NumberPoint)
                {
                    textBox.MaxLength = 11;
                    textBox.PreviewTextInput += (s, a) => a.Handled = InputProtection.Number(a);
                    textBox.PreviewKeyDown += (s, a) => a.Handled = InputProtection.BlockSpace(a);
                }
                else
                    textBox.MaxLength = 45;
            }
            catch (Exception){}
        }
    }
}
