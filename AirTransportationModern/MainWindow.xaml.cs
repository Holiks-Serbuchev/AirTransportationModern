using AirTransportationModern.Classes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using Word = Microsoft.Office.Interop.Word;

namespace AirTransportationModern
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        private string directoryPDF = Directory.GetCurrentDirectory() + @"\PDFFiles";
        string Table = String.Empty;
        string FIO = String.Empty;
        string IDS = String.Empty;
        int CountCapcha = 0;
        bool CheckPass = false;
        bool block = true;
        List<MenuContent> MenuList = new List<MenuContent>();
        List<string> Colums = new List<string>();
        List<string> ListCity = new List<string>();
        List<string> ID = new List<string>();
        List<MenuContent> Main;
        List<CardRoutes> cardRoutes = new List<CardRoutes>();
        AccountClass accountClass = new AccountClass();
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(directoryPDF))
                System.IO.Directory.CreateDirectory(directoryPDF);
            List<MenuContent> Routes = new MenuContent().CreateRoutesList();
            List<MenuContent> Workers = new MenuContent().CreateUsersList();
            Main = new MenuContent().CreateMainList();
            List<MenuContent> Country = new MenuContent().CreateCountryList();
            List<MenuContent> Directory = new MenuContent().CreateDirectoryList();
            List<MenuContent> Statistic = new MenuContent().CreateStatisticList();
            List<MenuContent> Word = new MenuContent().CreateWordList();
            List<MenuContent> Vehicle = new MenuContent().CreateVehicleList();
            TypeCombo.ItemsSource = ClassLogic.LoadCombo("SELECT * FROM vehicle_type", 0, "");
            MarksCombo.ItemsSource = ClassLogic.LoadCombo("SELECT * FROM marks", 0, "");
            StartCombo.ItemsSource = ClassLogic.LoadCombo("SELECT * FROM country", 0, "");
            EndCombo.ItemsSource = ClassLogic.LoadCombo("SELECT * FROM country", 0, "");
            ///////////////////////////////////////////
            MenuListV.ItemsSource = Main;
            MenuWorkers.ItemsSource = Workers;
            MenuRoutes.ItemsSource = Routes;
            MenuCountry.ItemsSource = Country;
            MenuDirectory.ItemsSource = Directory;
            MenuStatistic.ItemsSource = Statistic;
            MenuVehicle.ItemsSource = Vehicle;
            MenuWord.ItemsSource = Word;
            MenuListV.SelectedIndex = 0;
            MenuListV.SelectionChanged += (s, a) =>
            {
                SwapWindow((ListView)s, Main);
            };
            MenuCountry.SelectionChanged += (s, a) => SwapWindow((ListView)s, Country);
            MenuRoutes.SelectionChanged += (s, a) => SwapWindow((ListView)s, Routes);
            MenuDirectory.SelectionChanged += (s, a) => SwapWindow((ListView)s, Directory);
            MenuWorkers.SelectionChanged += (s, a) => SwapWindow((ListView)s, Workers);
            MenuVehicle.SelectionChanged += (s, a) => SwapWindow((ListView)s, Vehicle);
            MenuStatistic.SelectionChanged += (s, a) => { SwapWindow((ListView)s, Statistic); CreateChart((ListView)s, Statistic); };
            MenuWord.SelectionChanged += (s, a) => SwapWindow((ListView)s, Word);
            ThemeCombo.SelectionChanged += (s, a) =>
            {
                Uri Uri;
                if (ThemeCombo.SelectedIndex == 0)
                    Uri = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
                else
                    Uri = new Uri("Themes/LightTheme.xaml", UriKind.Relative);
                ResourceDictionary resourceDict = Application.LoadComponent(Uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            };
            SearchTB.TextChanged += (s, a) =>
            {
                try
                {
                    Load(Table);
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        int Count = 0;
                        List<object> sa = DT.Rows[i].ItemArray.ToList();
                        foreach (var item in sa)
                            if (item.ToString().ToLower().Contains(SearchTB.Text.ToLower()))
                                Count++;
                        if (Count == 0)
                        {
                            DT.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                }
                catch (Exception) { }
            };
            CheckButton.Click += (s, a) =>
            {
                List<TextBox> textBoxes = new List<TextBox>() { TypeTB, MarkTB, ModelTB, WeightTB, WidthTB, HeightTB, LengthTB, CityFirstTB, AirportFirstTB, DateFirstTB, CitySecondTB, AirportSecondTB, DateSecondTB };
                try
                {
                    ClassLogic.Conn.Open();
                    MySqlCommand Cmd = new MySqlCommand("SELECT vehicle.VehicleTypeCombo, vehicle.MarkCombo, vehicle.Model, vehicle.Weight, " +
                        "vehicle.Width, vehicle.Height, vehicle.Length, c1.CityName, air2.`Name`, " +
                        "service.DepartDate, city.CityName, airports.`Name`,service.ArrivalDate, vehicle.StatusCombo,service.Cost" +
                        " FROM vehicle JOIN service ON vehicle.ServiceCombo = service.IDService JOIN routes ON" +
                        " service.NumRouteCombo = routes.IDRoute JOIN airports ON routes.StartCombo = airports.IDAirport " +
                        "JOIN airports AS air2 ON routes.EndCombo = air2.IDAirport JOIN city ON " +
                        $"airports.CityCombo = city.IDCity JOIN city AS c1 ON air2.CityCombo = c1.IDCity WHERE VehicleID = '{CheckTB.Text}'", ClassLogic.Conn);
                    MySqlDataReader DR = Cmd.ExecuteReader();
                    if (DR.Read())
                    {
                        for (int i = 0; i < textBoxes.Count; i++)
                            textBoxes[i].Text = DR[i].ToString();
                        StatusLa.Content = "Статус: " + DR[13].ToString();
                        CostLa.Content = "Цена: " + DR[14].ToString() + ".00₽";
                    }
                    else
                    {
                        for (int i = 0; i < textBoxes.Count; i++)
                            textBoxes[i].Text = String.Empty;
                        StatusLa.Content = "Статус:";
                        CostLa.Content = "Цена:";
                        AdvancedMessageBox.InfoBox("Такого заказа не существует!");
                    }
                }
                catch (Exception) { ClassLogic.Conn.Close(); AdvancedMessageBox.ErrorBox("Произошла ошибка сервера"); }
                ClassLogic.Conn.Close();
            };
            AddB.Click += (s, a) =>
            {
                int CountTextBox = LoadContainer.Children.OfType<TextBox>().Count();
                if (LoadContainer.Children.OfType<Xceed.Wpf.Toolkit.MaskedTextBox>().Count() != 0)
                    CountTextBox = CountTextBox - 1;
                int Counts = LoadContainer.Children.OfType<ComboBox>().Count() + CountTextBox + LoadContainer.Children.OfType<PasswordBox>().Count() + LoadContainer.Children.OfType<Xceed.Wpf.Toolkit.MaskedTextBox>().Count() + LoadContainer.Children.OfType<Xceed.Wpf.Toolkit.DateTimePicker>().Count();
                if (CheckInputs() == Counts)
                {
                    ClassLogic.AddValuesToList(LoadContainer);
                    if (Data.Columns.Count() != 0)
                    {
                        ClassLogic.Add(Table, Colums, ClassLogic.Values);
                        Load(Table);
                    }
                }
                else
                    AdvancedMessageBox.InfoBox("Заполните все поля");
            };
            UpdateB.Click += (s, a) =>
            {
                int CountTextBox = LoadContainer.Children.OfType<TextBox>().Count();
                if (LoadContainer.Children.OfType<Xceed.Wpf.Toolkit.MaskedTextBox>().Count() != 0)
                    CountTextBox = CountTextBox - 1;
                int Counts = LoadContainer.Children.OfType<ComboBox>().Count() + CountTextBox + LoadContainer.Children.OfType<PasswordBox>().Count() + LoadContainer.Children.OfType<Xceed.Wpf.Toolkit.MaskedTextBox>().Count() + LoadContainer.Children.OfType<Xceed.Wpf.Toolkit.DateTimePicker>().Count();
                if (CheckInputs() == Counts)
                {
                    ClassLogic.AddValuesToList(LoadContainer);
                    if (Data.Columns.Count() != 0)
                    {
                        ClassLogic.Update(Table, Colums, ClassLogic.Values, IDS.ToString(), Data.Columns.ElementAt(0).SortMemberPath.ToString());
                        Load(Table);
                    }
                }
                else
                    AdvancedMessageBox.InfoBox("Заполните все поля");
            };
            DeleteB.Click += (s, a) =>
            {
                string Text = String.Empty;
                foreach (DataRowView Row in Data.SelectedItems)
                    Text = Row.Row.ItemArray[0].ToString();
                if (Text != "")
                {
                    if (MessageBox.Show("Вы точно хотите удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        ClassLogic.Delete(Data.Columns.ElementAt(Data.Columns.Count - 1).SortMemberPath.ToString(), Table, Data.Columns.ElementAt(0).SortMemberPath.ToString(), Text);
                        Load(Table);
                    }
                }
                else
                    AdvancedMessageBox.InfoBox("Пожалуйста нажмите на соответствующую ячейку в таблице");
            };
            RegAutoB.Click += (s, a) =>
            {
                try
                {
                    if (TypeCombo.SelectedIndex != -1 && MarksCombo.SelectedIndex != -1 && ModelSTB.Text != "" && WeightSTB.Text != "" && WidthSTB.Text != "" && HeightSTB.Text != "" && LengthSTB.Text != "" && StartSCombo.SelectedIndex != -1 && EndSCombo.SelectedIndex != -1 && EmailTB.Text != "")
                    {
                        if (ListVRoutes.SelectedIndex != -1)
                        {
                            MailClass mailClass = new MailClass();
                            if (mailClass.IsValidEmail(EmailTB.Text) == true)
                            {
                                ClassLogic.Conn.Open();
                                MySqlCommand Cmd = new MySqlCommand("INSERT INTO vehicle (`VehicleTypeCombo`, `MarkCombo`, `Model`, `Weight`, `Width`,`Height`, `Length`,`Email`,`ServiceCombo`,`StatusCombo`) VALUES ('" + TypeCombo.Text + "','" + MarksCombo.Text + "','" + ModelSTB.Text + "','" + WeightSTB.Text + "','" + WidthSTB.Text + "','" + HeightSTB.Text + "','" + LengthSTB.Text + "','" + EmailTB.Text + "','" + cardRoutes.ElementAt(ListVRoutes.SelectedIndex).ServiceID + "','Ожидает')", ClassLogic.Conn);
                                Cmd.ExecuteNonQuery();
                                ClassLogic.Conn.Close();
                                ClassLogic.Conn.Open();
                                MySqlCommand CmdSecond = new MySqlCommand("SELECT MAX(VehicleID) FROM `vehicle`", ClassLogic.Conn);
                                MySqlDataReader DR = CmdSecond.ExecuteReader();
                                int numOrder = 0;
                                if (DR.Read())
                                    numOrder = Convert.ToInt32(DR[0]);
                                ClassLogic.Conn.Close();
                                mailClass.Send(numOrder, EmailTB.Text);
                            }
                            else
                                AdvancedMessageBox.InfoBox("Email имеет неверный формат");
                        }
                        else
                            AdvancedMessageBox.InfoBox("Выберите рейс");
                    }
                    else
                        AdvancedMessageBox.InfoBox("Заполните поля!");
                }
                catch (Exception) { ClassLogic.Conn.Close(); }
            };
            AuthButton.Click += (s, a) =>
            {
                if ((LoginTB.Text != "" && PassTB.Password != "") || (LoginTB.Text != "" && PassTBF.Text != ""))
                {
                    string Pass = String.Empty;
                    if (CheckPass == true)
                        Pass = PassTBF.Text;
                    else
                        Pass = PassTB.Password;
                    try
                    {
                        ClassLogic.Conn.Open();
                        MySqlCommand Cmd = new MySqlCommand("SELECT * FROM workers Where Email='" + LoginTB.Text + "' AND `PositionNameCombo` <> 'Пилот' AND `StatusCombo` <> 'Удален'", ClassLogic.Conn);
                        MySqlDataReader DR = Cmd.ExecuteReader();
                        if (DR.Read())
                        {
                            if (DEncrypt.Decrypt(DR[7].ToString()) == Pass)
                            {
                                if (CapchaTB.Text == CapchaClass.TextCapcha)
                                {
                                    byte[] Img = null;
                                    try { Img = (byte[])DR[8]; }
                                    catch (Exception) { }
                                    accountClass.SetAccount(Convert.ToInt32(DR[0]), DR[1].ToString(), DR[2].ToString(), DR[3].ToString(), DR[6].ToString(), DEncrypt.Decrypt(DR[7].ToString()), DR[4].ToString(), DR[5].ToString(), Img);
                                    RoleLa.Content = "Роль: " + accountClass.GetPostionAccount();
                                    ProfileLa.Content = "Пользователь: " + accountClass.GetEmailAccount();
                                    FIO = accountClass.GetSurnameAccount() + " " + accountClass.GetNameAccount() + " " + accountClass.GetPatronymicAccount();
                                    ClassLogic.AccessTable(accountClass.GetPostionAccount(), ref CountryExpander, ref DirectoryExpander, ref RoutesExpander, ref WorkersExpander, ref StatisticExpander, ref WordExpander, ref VehicleExpander);
                                    foreach (var item in ClassLogic.FunctionAccess)
                                    {
                                        if (item == "Add")
                                            AddB.Visibility = Visibility.Visible;
                                        else if (item == "Update")
                                            UpdateB.Visibility = Visibility.Visible;
                                        else if (item == "Delete")
                                            DeleteB.Visibility = Visibility.Visible;
                                    }
                                    CountCapcha = 0;
                                    CapchaClass.TextCapcha = String.Empty;
                                    CapchaTB.Text = "";
                                    Main.ElementAt(2).ContainerOpen = "";
                                    Main.ElementAt(2).Rectangle = "M10.09,15.59L11.5,17l5,-5 -5,-5 -1.41,1.41L12.67,11H3v2h9.67l-2.58,2.59zM19,3H5c-1.11,0 -2,0.9 -2,2v4h2V5h14v14H5v-4H3v4c0,1.1 0.89,2 2,2h14c1.1,0 2,-0.9 2,-2V5c0,-1.1 -0.9,-2 -2,-2z";
                                    Main.ElementAt(2).Name = "Выход";
                                    Main.Add(new MenuContent() { ContainerOpen = "ProfileContainer", Name = "Профиль", Rectangle = "M12,2C6.48,2 2,6.48 2,12s4.48,10 10,10 10,-4.48 10,-10S17.52,2 12,2zM12,5c1.66,0 3,1.34 3,3s-1.34,3 -3,3 -3,-1.34 -3,-3 1.34,-3 3,-3zM12,19.2c-2.5,0 -4.71,-1.28 -6,-3.22 0.03,-1.99 4,-3.08 6,-3.08 1.99,0 5.97,1.09 6,3.08 -1.29,1.94 -3.5,3.22 -6,3.22z" });
                                    MenuListV.ItemsSource = null;
                                    MenuListV.ItemsSource = Main;
                                    MenuListV.SelectedIndex = 3;
                                    LoadProfile(Main);
                                }
                            }
                            else
                            { AdvancedMessageBox.InfoBox("Неверный логин или пароль"); CountCapcha++; }
                        }
                        else
                        { AdvancedMessageBox.InfoBox("Неверный логин или пароль"); CountCapcha++; }
                        ClassLogic.Conn.Close();
                    }
                    catch (Exception) { ClassLogic.Conn.Close(); }
                }
                else
                { AdvancedMessageBox.InfoBox("Заполните все данные"); CountCapcha++; }
                if (CountCapcha >= 3)
                {
                    Img.Visibility = Visibility.Visible;
                    CapchaTB.Visibility = Visibility.Visible;
                    Img.Source = CapchaClass.ConvertS(CapchaClass.SetBitmap(Convert.ToInt32(Img.Width), Convert.ToInt32(Img.Height)));
                }
                else if (CountCapcha == 0)
                {
                    Img.Visibility = Visibility.Collapsed;
                    CapchaTB.Visibility = Visibility.Collapsed;
                }
            };
            DateTB.SelectedDateChanged += (s, a) =>
            {
                if (DateTB.SelectedDate != null && ListCity.Count != 0 && StartSCombo.SelectedIndex != -1 && EndSCombo.SelectedIndex != -1)
                {
                    ListVRoutes.ItemsSource = null;
                    cardRoutes = CardRoutes.Create($"SELECT service.IDService, service.DepartDate, service.StatusNameCombo, service.Cost, aircraft.Model FROM service JOIN routes ON service.NumRouteCombo = routes.IDRoute JOIN airports ON routes.StartCombo = airports.IDAirport AND airports.CityCombo = '{ListCity.ElementAt(StartSCombo.SelectedIndex)}' JOIN airports AS air2 ON routes.EndCombo = air2.IDAirport AND air2.CityCombo = '{ListCity.ElementAt(EndSCombo.SelectedIndex)}' JOIN aircraft ON service.NumAirCombo = aircraft.IDAir WHERE service.StatusNameCombo = 'Ожидает' AND DepartDate LIKE '%{DateTB.SelectedDate.Value.ToString("yyyy/MM/dd").Replace(".", "-")}%'");
                    ListVRoutes.ItemsSource = cardRoutes;
                }
            };
        }
        int Count = 0;
        private void EyePassBClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Count % 2 == 0)
                {
                    CheckPass = true;
                    Rec.Clip = Geometry.Parse("M12,7c2.76,0 5,2.24 5,5 0,0.65 -0.13,1.26 -0.36,1.83l2.92,2.92c1.51,-1.26 2.7,-2.89 3.43,-4.75 -1.73,-4.39 -6,-7.5 -11,-7.5 -1.4,0 -2.74,0.25 -3.98,0.7l2.16,2.16C10.74,7.13 11.35,7 12,7zM2,4.27l2.28,2.28 0.46,0.46C3.08,8.3 1.78,10.02 1,12c1.73,4.39 6,7.5 11,7.5 1.55,0 3.03,-0.3 4.38,-0.84l0.42,0.42L19.73,22 21,20.73 3.27,3 2,4.27zM7.53,9.8l1.55,1.55c-0.05,0.21 -0.08,0.43 -0.08,0.65 0,1.66 1.34,3 3,3 0.22,0 0.44,-0.03 0.65,-0.08l1.55,1.55c-0.67,0.33 -1.41,0.53 -2.2,0.53 -2.76,0 -5,-2.24 -5,-5 0,-0.79 0.2,-1.53 0.53,-2.2zM11.84,9.02l3.15,3.15 0.02,-0.16c0,-1.66 -1.34,-3 -3,-3l-0.17,0.01z");
                    PassTBF.Text = PassTB.Password;
                    PassTB.Visibility = Visibility.Collapsed;
                    PassTBF.Visibility = Visibility.Visible;
                }
                else
                {
                    CheckPass = false;
                    Rec.Clip = Geometry.Parse("M12,4.5C7,4.5 2.73,7.61 1,12c1.73,4.39 6,7.5 11,7.5s9.27,-3.11 11,-7.5c-1.73,-4.39 -6,-7.5 -11,-7.5zM12,17c-2.76,0 -5,-2.24 -5,-5s2.24,-5 5,-5 5,2.24 5,5 -2.24,5 -5,5zM12,9c-1.66,0 -3,1.34 -3,3s1.34,3 3,3 3,-1.34 3,-3 -1.34,-3 -3,-3z");
                    PassTB.Password = PassTBF.Text;
                    PassTBF.Visibility = Visibility.Collapsed;
                    PassTB.Visibility = Visibility.Visible;
                }
                Count++;
            }
            catch (Exception){}
        }
        private void TextBoxPreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = InputProtection.Number(e);
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((ComboBox)sender).Name == "StartCombo" && ((ComboBox)sender).SelectedItem != null)
                {
                    StartSCombo.ItemsSource = ClassLogic.LoadCombo("SELECT * FROM city WHERE CountryNameCombo = '" + ((ComboBox)sender).SelectedItem.ToString() + "'", 1, "");
                    ListCity = ClassLogic.LoadCombo("SELECT * FROM city WHERE CountryNameCombo = '" + ((ComboBox)sender).SelectedItem.ToString() + "'", 0, "");
                }
                else if(((ComboBox)sender).SelectedItem != null)
                {
                    EndSCombo.ItemsSource = ClassLogic.LoadCombo("SELECT * FROM city WHERE CountryNameCombo = '" + ((ComboBox)sender).SelectedItem.ToString() + "'", 1, "");
                    ListCity = ClassLogic.LoadCombo("SELECT * FROM city WHERE CountryNameCombo = '" + ((ComboBox)sender).SelectedItem.ToString() + "'", 0, "");
                }
            }
            catch (Exception){}
        }
        private int CheckInputs()
        {
            int Count = 0;
            foreach (object item in LoadContainer.Children)
            {
                if ((item.GetType() == typeof(TextBox) && ((TextBox)item).Text != "") || ((item.GetType() == typeof(ComboBox)) && ((ComboBox)item).SelectedIndex != -1) || (item.GetType() == typeof(PasswordBox) && ((PasswordBox)item).Password != "") || (item.GetType() == typeof(Xceed.Wpf.Toolkit.MaskedTextBox) && !((Xceed.Wpf.Toolkit.MaskedTextBox)item).Text.Contains("_")) || (item.GetType() == typeof(Xceed.Wpf.Toolkit.DateTimePicker) && ((Xceed.Wpf.Toolkit.DateTimePicker)item).Text != null && ((Xceed.Wpf.Toolkit.DateTimePicker)item).Text != ""))
                    Count++;
            }
            return Count;
        }
        void SwapWindow(ListView listView, List<MenuContent> elements)
        {
            if (block)
            {
                block = false;
                List<ListView> listViews = new List<ListView>() { MenuCountry, MenuDirectory, MenuListV, MenuRoutes, MenuWorkers, MenuStatistic, MenuWord, MenuVehicle };
                foreach (ListView item in listViews)
                    if (item.Name != listView.Name)
                        item.UnselectAll();
                try
                {
                    if (elements.ElementAt(listView.SelectedIndex).Name == "Выход")
                    {
                        AddB.Visibility = Visibility.Collapsed;
                        UpdateB.Visibility = Visibility.Collapsed;
                        DeleteB.Visibility = Visibility.Collapsed;
                        ClassLogic.AccessTable("", ref CountryExpander, ref DirectoryExpander, ref RoutesExpander, ref WorkersExpander, ref StatisticExpander, ref WordExpander, ref VehicleExpander);
                        MenuListV.ItemsSource = null;
                        Main = new MenuContent().CreateMainList();
                        MenuListV.ItemsSource = Main;
                        MenuListV.SelectedIndex = 0;
                        AccountClass accountClass = new AccountClass();
                        accountClass.ResetAccount();
                        ProfileLa.Content = "Пользователь: Гость";
                        RoleLa.Content = "Роль: Гость";
                    }
                    foreach (var item in MainContainer.Children)
                    {
                        if (item.GetType() == typeof(Grid))
                        {
                            if (((Grid)item).Name == elements.ElementAt(listView.SelectedIndex).ContainerOpen)
                                ((Grid)item).Visibility = Visibility.Visible;
                            else
                                ((Grid)item).Visibility = Visibility.Collapsed;
                        }
                        else if (item.GetType() == typeof(ScrollViewer))
                        {
                            if (((ScrollViewer)item).Name == elements.ElementAt(listView.SelectedIndex).ContainerOpen)
                                ((ScrollViewer)item).Visibility = Visibility.Visible;
                            else
                                ((ScrollViewer)item).Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            if (((StackPanel)item).Name == elements.ElementAt(listView.SelectedIndex).ContainerOpen)
                                ((StackPanel)item).Visibility = Visibility.Visible;
                            else
                                ((StackPanel)item).Visibility = Visibility.Collapsed;
                        }
                    }
                    if (elements.ElementAt(listView.SelectedIndex).TableSQl != "" && elements.ElementAt(listView.SelectedIndex).TableSQl != null && !elements.ElementAt(listView.SelectedIndex).TableSQl.Contains("Statistic"))
                    {
                        Table = elements.ElementAt(listView.SelectedIndex).TableSQl;
                        Load(Table);
                    }
                }
                catch (Exception) { }
                block = true;
            }
        }
        DataTable DT = new DataTable();
        private void Load(string Value)
        {
            DT.Clear();
            Colums.Clear();
            LoadContainer.Children.Clear();
            if (ClassLogic.CheckStatus("SELECT * FROM " + Value) != "")
                DT = ClassLogic.LoadData("SELECT * FROM " + Value + " Where " + ClassLogic.CheckStatus("SELECT * FROM " + Value) + "<> 'Удален'");
            else
                DT = ClassLogic.LoadData("SELECT * FROM " + Value);
            DT = ClassLogic.DTConvert(DT);
            if (Value == "workers")
            {
                foreach (DataRow item in DT.Rows)
                    item["Password"] = DEncrypt.Decrypt(item["Password"].ToString());
            }
            Data.DataContext = DT;
            foreach (var item in Data.Columns)
            {
                if (!item.Header.ToString().Contains("Image"))
                {
                    if (item.Header.ToString() == Translator.AutoTranslateTo(item.Header.ToString()))
                        item.Visibility = Visibility.Hidden;
                }
                else
                    item.Visibility = Visibility.Collapsed;
                item.Header = Translator.AutoTranslateTo(item.Header.ToString());
            }
            foreach (DataColumn item in DT.Columns)
            {
                if (item.ColumnName.ToString().Contains("Combo"))
                {
                    Colums.Add(item.ColumnName.ToString());
                    Label La = new Label();
                    La.Content = Translator.AutoTranslateTo(item.ColumnName) + ":";
                    LoadContainer.Children.Add(La);
                    ComboBox Combo = new ComboBox();
                    Combo.Name = item.ColumnName + "Combo";
                    Combo.HorizontalAlignment = HorizontalAlignment.Left;
                    Combo.Width = 200;
                    Combo.Height = 25;
                    List<string> Coll = ClassLogic.LoadCombo("SELECT REFERENCED_TABLE_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE REFERENCED_TABLE_SCHEMA = '" + ClassLogic.Conn.Database + "' AND TABLE_NAME = '" + Value + "' AND COLUMN_NAME = '" + item.ColumnName.ToString() + "';", 0, "");
                    DataTable DTF = ClassLogic.LoadData("SELECT * FROM " + Coll[0]);
                    List<string> GetColumns = new List<string>();
                    foreach (var DTItem in DTF.Columns)
                        GetColumns.Add(DTItem.ToString());
                    try
                    {
                        bool Check = false;
                        foreach (var itemt in Data.Columns)
                        {
                            if (itemt.Header.ToString() == "NumRouteCombo")
                            {
                                Combo.ItemsSource = ClassLogic.LoadCombo("SELECT DISTINCT " + Coll[0] + "." + GetColumns[0] + " FROM " + Value + " CROSS JOIN " + Coll[0], 0, "");
                                Check = true;
                            }
                        }
                        if (Check != true)
                        {
                            List<string> Items = ClassLogic.LoadCombo("SELECT DISTINCT " + Coll[0] + "." + GetColumns[0] + "," + Coll[0] + "." + GetColumns[1] + " FROM " + Value + " CROSS JOIN " + Coll[0], 1, "");
                            Combo.ItemsSource = Items;
                            List<string> Tags = ClassLogic.LoadCombo("SELECT DISTINCT " + Coll[0] + "." + GetColumns[0] + "," + Coll[0] + "." + GetColumns[1] + " FROM " + Value + " CROSS JOIN " + Coll[0], 0, "");
                            foreach (DataRow DTR in DT.Rows)
                            {
                                try
                                {
                                    int NumS = ((List<string>)Tags).IndexOf(DTR[item].ToString());
                                    DTR[item] = Items.ElementAt(NumS);
                                }
                                catch (Exception) { DTR[item] = DTR[item]; }
                            }
                            if (Int32.TryParse(Tags[0], out int Num) == true)
                                Combo.Tag = Tags;
                        }
                    }
                    catch (Exception)
                    {
                        Combo.ItemsSource = ClassLogic.LoadCombo("SELECT DISTINCT " + Coll[0] + "." + GetColumns[0] + " FROM " + Value + " CROSS JOIN " + Coll[0], 0, "");
                    }
                    LoadContainer.Children.Add(Combo);
                }
                else if (item.ColumnName.ToString().Contains("Password"))
                {
                    Colums.Add(item.ColumnName.ToString());
                    Label La = new Label();
                    La.Content = Translator.AutoTranslateTo(item.ColumnName) + ":";
                    LoadContainer.Children.Add(La);
                    PasswordBox Pass = new PasswordBox();
                    Pass.Name = item.ColumnName + "TB";
                    Pass.HorizontalAlignment = HorizontalAlignment.Left;
                    Pass.Width = 200;
                    Pass.Height = 25;
                    Pass.PasswordChar = '*';
                    LoadContainer.Children.Add(Pass);
                }
                else if (item.ColumnName.Contains("Date"))
                {
                    Colums.Add(item.ColumnName.ToString());
                    Label La = new Label();
                    La.Content = Translator.AutoTranslateTo(item.ColumnName) + ":";
                    LoadContainer.Children.Add(La);
                    Xceed.Wpf.Toolkit.DateTimePicker Date = new Xceed.Wpf.Toolkit.DateTimePicker();
                    Date.Name = item.ColumnName + "TB";
                    Date.HorizontalAlignment = HorizontalAlignment.Left;
                    Date.Width = 200;
                    Date.Height = 25;
                    Date.Format = Xceed.Wpf.Toolkit.DateTimeFormat.Custom;
                    Date.FormatString = "dd.MM.yyyy HH:mm:ss";
                    LoadContainer.Children.Add(Date);
                }
                else if (item.ColumnName.ToString().Contains("Telephone"))
                {
                    Colums.Add(item.ColumnName.ToString());
                    Label La = new Label();
                    La.Content = Translator.AutoTranslateTo(item.ColumnName) + ":";
                    LoadContainer.Children.Add(La);
                    Xceed.Wpf.Toolkit.MaskedTextBox MTB = new Xceed.Wpf.Toolkit.MaskedTextBox();
                    MTB.Name = item.ColumnName + "TB";
                    MTB.HorizontalAlignment = HorizontalAlignment.Left;
                    MTB.Width = 200;
                    MTB.Height = 25;
                    MTB.Mask = "+#(###)###-##-##";
                    LoadContainer.Children.Add(MTB);
                }
                else if (!item.ColumnName.ToString().Contains("ID") && !item.ColumnName.ToString().Contains("Image"))
                {
                    Colums.Add(item.ColumnName.ToString());
                    Label La = new Label();
                    La.Content = Translator.AutoTranslateTo(item.ColumnName) + ":";
                    LoadContainer.Children.Add(La);
                    TextBox TB = new TextBox();
                    TB.Name = item.ColumnName + "TB";
                    TB.HorizontalAlignment = HorizontalAlignment.Left;
                    TB.Width = 200;
                    TB.Height = 25;
                    ProtectedCellsClass.ProtectCells(ref TB, item.ColumnName + item.Table);
                    LoadContainer.Children.Add(TB);
                }
                else if (item.ColumnName.Contains("Image"))
                {
                    Label La = new Label();
                    La.Content = Translator.AutoTranslateTo(item.ColumnName) + ":";
                    LoadContainer.Children.Add(La);
                    Image image = new Image();
                    image.Width = 200;
                    image.Stretch = Stretch.Fill;
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.Source = new BitmapImage(new Uri("Image/noImage.jpg", UriKind.Relative));
                    LoadContainer.Children.Add(image);
                }
            }
        }
        private void DataMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int Count = 0;
                object[] Mass = new object[Data.Columns.Count];
                foreach (DataRowView Row in Data.SelectedItems)
                {
                    for (int i = 0; i < Data.Columns.Count; i++)
                        Mass[i] = Row.Row.ItemArray[i];
                }
                if (Mass[0] != null)
                {
                    IDS = Mass[0].ToString();
                    if (Int32.TryParse(IDS, out int S) == true & Data.Columns[0].Header.ToString().Contains("ID"))
                        Count++;
                    foreach (var item in LoadContainer.Children)
                    {
                        if (item.GetType() != typeof(Label))
                        {
                            if (item.GetType() == typeof(TextBox))
                            {
                                ((TextBox)item).Text = Mass[Count].ToString();
                                Count++;
                            }
                            else if (item.GetType() == typeof(ComboBox))
                            {
                                var Tags = ((ComboBox)item).Tag;
                                if (Tags != null)
                                {
                                    try
                                    {
                                        int Num = ((List<string>)Tags).IndexOf(Mass[Count].ToString());
                                        List<string> Items = (List<string>)((ComboBox)item).ItemsSource;
                                        ((ComboBox)item).SelectedItem = Items.ElementAt(Num);
                                    }
                                    catch (Exception) { ((ComboBox)item).SelectedItem = Mass[Count]; }
                                }
                                else
                                    ((ComboBox)item).SelectedItem = Mass[Count];
                                Count++;
                            }
                            else if (item.GetType() == typeof(PasswordBox))
                            {
                                ((PasswordBox)item).Password = Mass[Count].ToString();
                                Count++;
                            }
                            else if (item.GetType() == typeof(Xceed.Wpf.Toolkit.DateTimePicker))
                            {
                                ((Xceed.Wpf.Toolkit.DateTimePicker)item).Text = Mass[Count].ToString();
                                Count++;
                            }
                            else if (item.GetType() == typeof(Xceed.Wpf.Toolkit.MaskedTextBox))
                            {
                                ((Xceed.Wpf.Toolkit.MaskedTextBox)item).Text = Mass[Count].ToString();
                                Count++;
                            }
                            else if (item.GetType() == typeof(Image))
                            {
                                try
                                {
                                    if ((byte[])Mass[Count] != null)
                                        ((Image)item).Source = BitmapFrame.Create(new MemoryStream((byte[])Mass[Count]), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                                    else
                                        ((Image)item).Source = new BitmapImage(new Uri("Image/noImage.jpg", UriKind.Relative));
                                }
                                catch (Exception) { ((Image)item).Source = new BitmapImage(new Uri("Image/noImage.jpg", UriKind.Relative)); }
                                Count++;
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }
        private void FilterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> NameLa = new List<string>();
                List<string> ComboValue = new List<string>();
                foreach (var item in LoadContainer.Children)
                {
                    if (item is Label)
                        NameLa.Add(((Label)item).Content.ToString().Replace(":", ""));
                    else
                    {
                        if (item.GetType() == typeof(ComboBox))
                            ComboValue.Add(((ComboBox)item).Text.ToString());
                        else
                            ComboValue.Add("");
                    }
                }
                Load(Table);
                int i = 0;
                List<DataRow> DR = new List<DataRow>();
                foreach (var item in ComboValue)
                {
                    i++;
                    if (item != "")
                    {
                        foreach (DataRow item2 in DT.Rows)
                            if (item != item2.ItemArray[i].ToString())
                                DR.Add(item2);

                    }
                }
                foreach (var item in DR)
                {
                    try { DT.Rows.Remove(item); }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }
        private void ClearBClick(object sender, RoutedEventArgs e) => Load(Table);
        public SeriesCollection seriesCollection { get; set; }
        void CreateChart(ListView listView, List<MenuContent> menuContents)
        {
            if (block)
            {
                PanelStatistic.Visibility = Visibility.Hidden;
                List<string> Value = new List<string>();
                List<string> Count = new List<string>();
                seriesCollection = null;
                Chart.DataContext = null;
                StatisticB.Click += (s, a) =>
                {
                    if (FirstDate.SelectedDate != null & SecondDate.SelectedDate != null)
                    {
                        Value = ClassLogic.LoadCombo("SELECT vehicle.MarkCombo, count(*) FROM vehicle INNER JOIN service ON service.IDService = vehicle.ServiceCombo AND service.ArrivalDate BETWEEN '" + FirstDate.SelectedDate.Value.ToString("yyyy/MM/dd").Replace(".", "-") + "' AND '" + SecondDate.SelectedDate.Value.ToString("yyyy/MM/dd").Replace(".", "-") + "' AND service.StatusNameCombo = 'Прилетел' WHERE vehicle.StatusCombo <> 'Удален' GROUP BY MarkCombo ORDER BY count(*) DESC", 0, "");
                        Count = ClassLogic.LoadCombo("SELECT vehicle.MarkCombo, count(*) FROM vehicle INNER JOIN service ON service.IDService = vehicle.ServiceCombo AND service.ArrivalDate BETWEEN '" + FirstDate.SelectedDate.Value.ToString("yyyy/MM/dd").Replace(".", "-") + "' AND '" + SecondDate.SelectedDate.Value.ToString("yyyy/MM/dd").Replace(".", "-") + "' AND service.StatusNameCombo = 'Прилетел' WHERE vehicle.StatusCombo <> 'Удален' GROUP BY MarkCombo ORDER BY count(*) DESC", 1, "");
                        CreateChartEnd(Value, Count);
                    }
                    else
                        AdvancedMessageBox.InfoBox("Выберите даты");
                };
                if (menuContents.ElementAt(listView.SelectedIndex).StatisticElement == "0")
                    PanelStatistic.Visibility = Visibility.Visible;
                else
                {
                    Value = ClassLogic.LoadCombo("SELECT workers.PositionNameCombo,count(*) FROM workers GROUP BY workers.PositionNameCombo ORDER BY count(*) DESC", 0, "");
                    Count = ClassLogic.LoadCombo("SELECT workers.PositionNameCombo,count(*) FROM workers GROUP BY workers.PositionNameCombo ORDER BY count(*) DESC", 1, "");
                    CreateChartEnd(Value, Count);
                }
            }
        }
        void CreateChartEnd(List<string> Value, List<string> Count)
        {
            seriesCollection = null;
            Chart.DataContext = null;
            seriesCollection = new SeriesCollection();
            seriesCollection.Clear();
            for (int i = 0; i < Value.Count; i++)
                seriesCollection.Add(new PieSeries { Title = Value[i], Values = new ChartValues<ObservableValue> { new ObservableValue(Convert.ToDouble(Count[i])) }, DataLabels = true });
            Chart.DataContext = this;
            Chart.Update(true, true);
        }
        byte[] ImageProfile = null;
        private void ChangeImageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images(*.jpg,*.png,*.jpeg)| *.jpg;*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ProfileImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImageProfile = File.ReadAllBytes(openFileDialog.FileName);
                }
                catch (Exception) { }
            }
        }

        private void ClearImageBClick(object sender, RoutedEventArgs e)
        {
            AccountClass accountClass = new AccountClass();
            if (accountClass.GetImageAccount() == null)
            {
                ProfileImage.Source = new BitmapImage(new Uri("Image/noImage.jpg", UriKind.Relative));
                ImageProfile = null;
            }
            else
            {
                ProfileImage.Source = BitmapFrame.Create(new MemoryStream(accountClass.GetImageAccount()), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                ImageProfile = accountClass.GetImageAccount();
            }
        }
        void LoadProfile(List<MenuContent> main)
        {
            if (main.ElementAt(3).Name == "Профиль")
            {
                SurnameProfileTB.Text = accountClass.GetSurnameAccount();
                NameProfileTB.Text = accountClass.GetNameAccount();
                PatrProfileTB.Text = accountClass.GetPatronymicAccount();
                RoleProfileTB.Text = accountClass.GetPostionAccount();
                TelephoneProfileTB.Text = accountClass.GetTelephoneAccount();
                EmailProfileTB.Text = accountClass.GetEmailAccount();
                PasswordProfileTB.Password = accountClass.GetPasswordAccount();
                if (accountClass.GetImageAccount() == null)
                {
                    ProfileImage.Source = new BitmapImage(new Uri("Image/noImage.jpg", UriKind.Relative));
                    ImageProfile = null;
                }
                else
                {
                    ProfileImage.Source = BitmapFrame.Create(new MemoryStream(accountClass.GetImageAccount()), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    ImageProfile = accountClass.GetImageAccount();
                }
            }
        }

        private void ChangeProfileClick(object sender, RoutedEventArgs e)
        {
            if (TelephoneProfileTB.Text != "" && PasswordProfileTB.Password != "" && EmailProfileTB.Text != "")
            {
                try
                {
                    ClassLogic.Conn.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand($"UPDATE `workers` SET `TelephoneNum` = '{TelephoneProfileTB.Text}', `Email` = '{EmailProfileTB.Text}',`Password` = '{DEncrypt.Encrypt(PasswordProfileTB.Password)}',`Image` = @img WHERE `IDWorker` = {accountClass.GetIDAccount()}", ClassLogic.Conn);
                    mySqlCommand.Parameters.Add(new MySqlParameter("@img", ImageProfile));
                    mySqlCommand.ExecuteNonQuery();
                    AdvancedMessageBox.InfoBox("Данные учетной записи успешно изменены");
                    ClassLogic.Conn.Close();
                }
                catch (Exception) { ClassLogic.Conn.Close(); }
            }
        }
        void FindAndReplace(object FindText, object ReplaceText, Word.Application App) => App.Selection.Find.Execute(ref FindText, true, true, false, false, false, true, false, 1, ref ReplaceText, 2, false, false, false, false);
        private void WordBClick(object sender, RoutedEventArgs e)
        {
            if (SDateP.SelectedDate != null && FDateP.SelectedDate != null)
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "DOCX files(*.docx)|*.docx|All files(*.*)|*.*";
                if (SFD.ShowDialog() == true)
                {
                    try
                    {
                        Word.Application App = new Word.Application();
                        object objMiss = Missing.Value;
                        DataTable DT = ClassLogic.LoadData($"SELECT vehicle.MarkCombo AS 'Марка', count(*) AS 'Количество заказов за период',service.Cost AS 'Цена(руб)' FROM vehicle INNER JOIN service ON service.IDService = vehicle.ServiceCombo AND service.ArrivalDate BETWEEN '{FDateP.SelectedDate.Value.ToString("yyyy/MM/dd").Replace(".", "-")}' AND '{SDateP.SelectedDate.Value.ToString("yyyy/MM/dd").Replace(".", "-")}' AND service.StatusNameCombo = 'Прилетел' WHERE vehicle.StatusCombo <> 'Удален' GROUP BY vehicle.MarkCombo, service.Cost ORDER BY count(*) DESC");
                        if (DT.Columns != null)
                        {
                            DataRow dataRow = DT.NewRow();
                            int CountS = 0;
                            double SumDouble = 0;
                            foreach (DataRow item in DT.Rows)
                            {
                                CountS += Convert.ToInt32(item.ItemArray[1]);
                                SumDouble += Convert.ToInt32(item.ItemArray[2]) * Convert.ToInt32(item.ItemArray[1]);
                            }
                            dataRow[0] = "Итого";
                            dataRow[1] = CountS;
                            dataRow[2] = SumDouble;
                            DT.Rows.Add(dataRow);
                            Word.Document Doc = new Word.Document();
                            Doc.ReadOnlyRecommended = true;
                            string Sa = Directory.GetCurrentDirectory();
                            Doc = App.Documents.Open(Directory.GetCurrentDirectory() + @"\Template\Форма.docx");
                            FindAndReplace("USER", FIO, App);
                            FindAndReplace("DATE", DateTime.Now.ToString("dd.MM.yyyy"), App);
                            FindAndReplace("DATENOW", DateTime.Now.ToString("dd.MM.yyyy"), App);
                            Word.Table Tab = Doc.Tables[1];
                            for (int i = 0; i < DT.Rows.Count - 1; i++)
                                Tab.Rows.Add();
                            for (int i = 0; i < DT.Columns.Count - 1; i++)
                                Tab.Columns.Add();
                            int Row = 0, Colums = 0, RowS = 0, Count = 0;
                            foreach (Word.Row row in Tab.Rows)
                            {
                                object[] sa = DT.Rows[RowS].ItemArray;
                                Colums = 0;
                                Row = 0;
                                foreach (Word.Cell item in row.Cells)
                                {
                                    if (item.RowIndex == 1)
                                    {
                                        item.Range.Bold = 3;
                                        item.Range.Text = DT.Columns[Colums].ColumnName;
                                        Colums++;
                                    }
                                    else
                                    {
                                        item.Range.Text = sa[Row].ToString();
                                        Row++;
                                    }
                                }
                                Count++;
                                RowS++;
                                if (Count == RowS)
                                    RowS = RowS - 1;
                            }
                            Doc.SaveAs(FileName: SFD.FileName);
                            App.Quit();
                            try
                            {
                                string newXPSDocumentName = String.Concat(System.IO.Path.GetDirectoryName(SFD.FileName), "\\", System.IO.Path.GetFileNameWithoutExtension(SFD.FileName), ".xps");
                                DocumentView.Document = XPSConverter.ConvertWordDocToXPSDoc(SFD.FileName, newXPSDocumentName).GetFixedDocumentSequence();
                            }
                            catch (Exception){}
                        }
                    }
                    catch (Exception) { ClassLogic.Conn.Close(); }
                }
            }
            else
                AdvancedMessageBox.InfoBox("Заполните период");
        }

        private void ClearWordBClick(object sender, RoutedEventArgs e)
        {
            DocumentView.Document = null;
            FDateP.Text = "";
            SDateP.Text = "";
        }

        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e) => e.Handled = InputProtection.BlockSpace(e);
    }
}
