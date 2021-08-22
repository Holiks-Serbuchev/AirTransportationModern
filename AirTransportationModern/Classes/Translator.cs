using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTransportationModern.Classes
{
    public static class Translator
    {
        private static List<string> Original = new List<string>() {"TypeAir","Model","TypeAirCombo","ColorCombo","AirportCombo","ColorCombo","AirportCombo","Name","Adress","CityCombo",
            "Name", "NumAirCraftCombo", "AttributeCombo", "Num", "UnitCombo", "CountryNameCombo", "CityName", "ColorName", "NumHangarCombo", "AirCraftCombo", "Count",
            "CountryName", "AirportCombo", "MarksName", "PositionName", "StartCombo", "EndCombo", "DepartDate", "NumAirCombo", "NumRouteCombo", "PilotCombo", "ArrivalDate",
            "Cost", "StatusNameCombo", "StatusName", "StatusName", "Name", "VehicleTypeCombo", "MarkCombo", "Model", "Weight", "Profile", "ServiceCombo", "StatusCombo",
            "VehicleType", "Surname", "Names", "Patronymic", "PositionNameCombo", "TelephoneNum", "Email", "Password", "StatusCombo", "Width", "Height", "Length","Image"};

        private static List<string> Translate = new List<string>() {"Тип авиа","Модель","Тип авиа","Цвет","Аэропорт","Цвет","Аэропорт","Название","Адрес","Город",
            "Название", "Название самолета", "Атрибут", "Значение", "Единица измерения", "Страна", "Название Города", "Цвет", "Ангар", "Название самолета", "Количество",
            "Название страны", "Аэропорт", "Название марки", "Должность", "Начало пути", "Конец пути", "Дата отправления", "Самолет", "Маршрут", "Пилот", "Дата прибытия",
            "Цена(руб)", "Статус", "Статус", "Статус", "Название", "Тип авто", "Марки", "Модель", "Вес(Тонны)", "Габариты", "Рейсы", "Статус",
            "Тип авто", "Фамилия", "Имя", "Отчество", "Должность", "Телефон", "Электронная почта", "Пароль", "Статус", "Ширина(ММ)", "Высота(ММ)", "Длина(ММ)","Изображение"};

        public static string AutoTranslateTo(string OriginalTranslate)
        {
            int Num = Original.IndexOf(OriginalTranslate);
            if (Num != -1)
                return Translate.ElementAt(Num);
            else
                return OriginalTranslate;
        }
    }
}
