using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirTransportationModern.Classes
{
    public static class AdvancedMessageBox
    {
        public static void ErrorBox(string text) => MessageBox.Show(text, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        public static void QuestionBox(string text) => MessageBox.Show(text, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        public static void InfoBox(string text) => MessageBox.Show(text, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
