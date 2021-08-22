using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AirTransportationModern.Classes
{
    public static class ClassLogic
    {
        public static MySqlConnection Conn = new MySqlConnection("server=localhost;port=3306;database=airtransportation2;user=root;password=;charset=utf8");
        public static List<string> Values = new List<string>();
        public static List<string> RolesAccess = new List<string>() { "" };
        public static List<string> RusRolesAccess = new List<string>();
        public static List<string> FunctionAccess = new List<string>() { "" };
        public static string SendTable = String.Empty;
        public static bool AuthCheck = false;

        public static List<string> LoadCombo(string Command, int SetColumn, string ExtraCommand)
        {
            List<string> Collection = new List<string>();
            try
            {
                Conn.Open();
                MySqlCommand Cmd = new MySqlCommand(Command + ExtraCommand, Conn);
                MySqlDataReader DR = Cmd.ExecuteReader();
                if (DR.HasRows)
                {
                    while (DR.Read())
                        Collection.Add(DR[SetColumn].ToString());
                }
                Conn.Close();
            }
            catch (Exception) { Conn.Close(); }
            return Collection;
        }
        public static void AccessTable(string Position, ref Expander CountryExpander, ref Expander DirectoryExpander, ref Expander RoutesExpander, ref Expander WorkersExpander, ref Expander StatisticExpander, ref Expander  WordExpander, ref Expander VehicleExpander)
        {
            CountryExpander.Visibility = Visibility.Collapsed;
            DirectoryExpander.Visibility = Visibility.Collapsed;
            RoutesExpander.Visibility = Visibility.Collapsed;
            WorkersExpander.Visibility = Visibility.Collapsed;
            StatisticExpander.Visibility = Visibility.Collapsed;
            WordExpander.Visibility = Visibility.Collapsed;
            VehicleExpander.Visibility = Visibility.Collapsed;
            if (Position == "Администратор")
            {
                VehicleExpander.Visibility = Visibility.Visible;
                CountryExpander.Visibility = Visibility.Visible;
                DirectoryExpander.Visibility = Visibility.Visible;
                RoutesExpander.Visibility = Visibility.Visible;
                WorkersExpander.Visibility = Visibility.Visible;
                FunctionAccess = new List<string>() { "Add", "Update", "Delete" };
            }
            else if (Position == "Менеджер по продажам")
            {
                RoutesExpander.Visibility = Visibility.Visible;
                WordExpander.Visibility = Visibility.Visible;
                FunctionAccess = new List<string>() { "Add", "Update", "Delete" };
            }
            else if (Position == "Менеджер по заказам")
            {
                WordExpander.Visibility = Visibility.Visible;
                VehicleExpander.Visibility = Visibility.Visible;
                FunctionAccess = new List<string>() { "Add", "Update", "Delete" };
            }
            else if (Position == "Грузчик")
            {
                VehicleExpander.Visibility = Visibility.Visible;
                FunctionAccess = new List<string>() { };
            }
            else if (Position == "Диспетчер")
            {
               RoutesExpander.Visibility = Visibility.Visible;
               FunctionAccess = new List<string>() { "Add", "Update", "Delete" };
            }
            else if (Position == "Директор")
            {
                VehicleExpander.Visibility = Visibility.Visible;
                CountryExpander.Visibility = Visibility.Visible;
                DirectoryExpander.Visibility = Visibility.Visible;
                RoutesExpander.Visibility = Visibility.Visible;
                WorkersExpander.Visibility = Visibility.Visible;
                StatisticExpander.Visibility = Visibility.Visible;
                WordExpander.Visibility = Visibility.Visible;
                FunctionAccess = new List<string>() { "Watching", "Statistics`" };
            }
            else
            {
                FunctionAccess = new List<string>();
            }
            ClassLogic.Conn.Close();
        }
        public static string CheckStatus(string Command)
        {
            string Value = String.Empty;
            try
            {
                DataTable DT = new DataTable();
                Conn.Open();
                MySqlCommand Cmd = new MySqlCommand(Command, Conn);
                DT.Load(Cmd.ExecuteReader());
                if (DT.Columns[DT.Columns.Count - 1].ColumnName.Contains("Status"))
                    Value = DT.Columns[DT.Columns.Count - 1].ColumnName;
            }
            catch (Exception) { Value = ""; }
            Conn.Close();
            return Value;
        }
        public static DataTable LoadData(string Command)
        {
            DataTable DT = new DataTable();
            try
            {
                Conn.Open();
                MySqlCommand Cmd = new MySqlCommand(Command, Conn);
                DT.Load(Cmd.ExecuteReader());
                Conn.Close();
            }
            catch (Exception) { Conn.Close(); }
            return DT;
        }
        public static DataTable DTConvert(DataTable dataTable)
        {
            DataTable dataTable1 = dataTable.Clone();
            for (int i = 0; i < dataTable1.Columns.Count; i++)
                if (dataTable1.Columns[i].DataType != typeof(byte[]))
                    dataTable1.Columns[i].DataType = typeof(String);
            for (int j = 0; j < dataTable.Rows.Count; j++)
                dataTable1.LoadDataRow(dataTable.Rows[j].ItemArray, true);
            return dataTable1;
        }
        public static void AddValuesToList(StackPanel SP)
        {
            Values.Clear();
            foreach (var item in SP.Children)
            {
                if (item.GetType() == typeof(TextBox))
                    Values.Add(((TextBox)item).Text);
                else if (item.GetType() == typeof(Xceed.Wpf.Toolkit.MaskedTextBox))
                    Values.Add(((TextBox)item).Text);
                else if (item.GetType() == typeof(ComboBox))
                {
                    if (((ComboBox)item).Tag != null)
                    {
                        int Num = ((ComboBox)item).Items.IndexOf(((ComboBox)item).Text);
                        List<string> Tag = (List<string>)((ComboBox)item).Tag;
                        Values.Add(Tag.ElementAt(Num));
                    }
                    else
                        Values.Add(((ComboBox)item).Text);
                }
                else if (item.GetType() == typeof(PasswordBox))
                    Values.Add(DEncrypt.Encrypt(((PasswordBox)item).Password));
                else if (item.GetType() == typeof(Xceed.Wpf.Toolkit.DateTimePicker))
                {
                    if (((Xceed.Wpf.Toolkit.DateTimePicker)item).Text != null || ((Xceed.Wpf.Toolkit.DateTimePicker)item).Text != "")
                    {
                        DateTime Date = DateTime.Parse(((Xceed.Wpf.Toolkit.DateTimePicker)item).Text);
                        string dates = Date.Year + "-" + Date.Month + "-" + Date.Day + " " + Date.Hour + ":" + Date.Minute + ":" + Date.Second;
                        Values.Add(dates);
                    }
                }
            }
        }
        public static void Delete(string EndColumn, string Table, string StartCombo, string Text)
        {
            try
            {
                Conn.Open();
                MySqlCommand Cmd;
                if (EndColumn != StartCombo & EndColumn.Contains("Status"))
                    Cmd = new MySqlCommand("UPDATE " + Table + " SET " + EndColumn + " = 'Удален' WHERE " + StartCombo + " = " + Text, Conn);
                else
                    Cmd = new MySqlCommand("DELETE FROM " + Table + " WHERE " + StartCombo + " = '" + Text + "'", Conn);
                Cmd.ExecuteNonQuery();
                MessageBox.Show("Данные были успешно удалены");
                Conn.Close();
            }
            catch (Exception) { Conn.Close(); }
        }
        public static void Add(string NameTable, List<string> Column, List<string> Value)
        {
            try
            {
                string Start = "INSERT INTO `" + NameTable + "` (";
                foreach (var item in Column)
                    Start += "`" + item + "`,";
                Start = Start.Remove(Start.Length - 1) + ") VALUES (";
                foreach (var item in Value)
                    Start += "'" + item + "',";
                Start = Start.Remove(Start.Length - 1) + ");";
                Conn.Open();
                MySqlCommand Cmd = new MySqlCommand(Start, Conn);
                Cmd.ExecuteNonQuery();
                MessageBox.Show("Данные успешно добавлены");
                Conn.Close();
            }
            catch (Exception) { Conn.Close(); }
        }
        public static void Update(string NameTable, List<string> Column, List<string> Value, string WhereValue, string WhereColumn)
        {
            try
            {
                string Start = "UPDATE `" + NameTable + "` SET ";
                for (int i = 0; i < Column.Count; i++)
                    Start += "`" + Column[i] + "` = '" + Value[i] + "',";
                Start = Start.Remove(Start.Length - 1) + " WHERE `" + WhereColumn + "` = '" + WhereValue + "'";
                Conn.Open();
                MySqlCommand Cmd = new MySqlCommand(Start, Conn);
                Cmd.ExecuteNonQuery();
                MessageBox.Show("Данные успешно изменены");
                Conn.Close();
            }
            catch (Exception) { Conn.Close(); }
        }
    }
}
