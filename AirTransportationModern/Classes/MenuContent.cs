using System.Collections.Generic;

namespace AirTransportationModern.Classes
{
    public class MenuContent
    {
        public string Name { get; set; }
        public string Rectangle { get; set; }
        public string ContainerOpen { get; set; }
        public string TableSQl { get; set; }
        public string StatisticElement { get; set; }
        public List<MenuContent> CreateMainList()
        {
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Регистрация перевозок", ContainerOpen = "OrderReg", Rectangle = "M20.19,4H4C2.9,4 2.01,4.9 2.01,6v4C3.11,10 4,10.9 4,12s-0.89,2 -2,2v4c0,1.1 0.9,2 2,2h16c1.1,0 2,-0.9 2,-2V6C22,4.9 21.19,4 20.19,4zM17.73,13.3l-8.86,2.36l-1.66,-2.88l0.93,-0.25l1.26,0.99l2.39,-0.64l-2.4,-4.16l1.4,-0.38l4.01,3.74l2.44,-0.65c0.51,-0.14 1.04,0.17 1.18,0.68C18.55,12.62 18.25,13.15 17.73,13.3z" });
            menuContents.Add(new MenuContent { Name = "Проверить заказ", ContainerOpen = "CheckOrderContainer", Rectangle = "M12,2C6.48,2 2,6.48 2,12s4.48,10 10,10 10,-4.48 10,-10S17.52,2 12,2zM10,17l-5,-5 1.41,-1.41L10,14.17l7.59,-7.59L19,8l-9,9z" });
            menuContents.Add(new MenuContent { Name = "Авторизация", ContainerOpen = "AuthContainer", Rectangle = "M12,2c-4.97,0 -9,4.03 -9,9 0,4.17 2.84,7.67 6.69,8.69L12,22l2.31,-2.31C18.16,18.67 21,15.17 21,11c0,-4.97 -4.03,-9 -9,-9zM12,4c1.66,0 3,1.34 3,3s-1.34,3 -3,3 -3,-1.34 -3,-3 1.34,-3 3,-3zM12,18.3c-2.5,0 -4.71,-1.28 -6,-3.22 0.03,-1.99 4,-3.08 6,-3.08 1.99,0 5.97,1.09 6,3.08 -1.29,1.94 -3.5,3.22 -6,3.22z" });
            return menuContents;
        }
        public List<MenuContent> CreateUsersList()
        {
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Работники",TableSQl = "workers", ContainerOpen = "TableContainer", Rectangle = "M12,2C6.48,2 2,6.48 2,12s4.48,10 10,10 10,-4.48 10,-10S17.52,2 12,2zM12,5c1.66,0 3,1.34 3,3s-1.34,3 -3,3 -3,-1.34 -3,-3 1.34,-3 3,-3zM12,19.2c-2.5,0 -4.71,-1.28 -6,-3.22 0.03,-1.99 4,-3.08 6,-3.08 1.99,0 5.97,1.09 6,3.08 -1.29,1.94 -3.5,3.22 -6,3.22z" });
            return menuContents;
        }
        public List<MenuContent> CreateRoutesList()
        {
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Рейсы", TableSQl = "service", ContainerOpen = "TableContainer", Rectangle = "M20.19,4H4C2.9,4 2.01,4.9 2.01,6v4C3.11,10 4,10.9 4,12s-0.89,2 -2,2v4c0,1.1 0.9,2 2,2h16c1.1,0 2,-0.9 2,-2V6C22,4.9 21.19,4 20.19,4zM17.73,13.3l-8.86,2.36l-1.66,-2.88l0.93,-0.25l1.26,0.99l2.39,-0.64l-2.4,-4.16l1.4,-0.38l4.01,3.74l2.44,-0.65c0.51,-0.14 1.04,0.17 1.18,0.68C18.55,12.62 18.25,13.15 17.73,13.3z" });
            menuContents.Add(new MenuContent { Name = "Маршруты", TableSQl = "routes", ContainerOpen = "TableContainer", Rectangle = "M4,10.5c-0.83,0 -1.5,0.67 -1.5,1.5s0.67,1.5 1.5,1.5 1.5,-0.67 1.5,-1.5 -0.67,-1.5 -1.5,-1.5zM4,4.5c-0.83,0 -1.5,0.67 -1.5,1.5S3.17,7.5 4,7.5 5.5,6.83 5.5,6 4.83,4.5 4,4.5zM4,16.5c-0.83,0 -1.5,0.68 -1.5,1.5s0.68,1.5 1.5,1.5 1.5,-0.68 1.5,-1.5 -0.67,-1.5 -1.5,-1.5zM7,19h14v-2L7,17v2zM7,13h14v-2L7,11v2zM7,5v2h14L21,5L7,5z" });
            return menuContents;
        }
        public List<MenuContent> CreateCountryList()
        {
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Страны", TableSQl = "country", ContainerOpen = "TableContainer", Rectangle = "M14.4,6L14,4H5v17h2v-7h5.6l0.4,2h7V6z" });
            menuContents.Add(new MenuContent { Name = "Города", TableSQl = "city", ContainerOpen = "TableContainer", Rectangle = "M15,11L15,5l-3,-3 -3,3v2L3,7v14h18L21,11h-6zM7,19L5,19v-2h2v2zM7,15L5,15v-2h2v2zM7,11L5,11L5,9h2v2zM13,19h-2v-2h2v2zM13,15h-2v-2h2v2zM13,11h-2L11,9h2v2zM13,7h-2L11,5h2v2zM19,19h-2v-2h2v2zM19,15h-2v-2h2v2z" });
            return menuContents;
        }
        public List<MenuContent> CreateDirectoryList()
        {
            string imageRec = "M10,10.02h5L15,21h-5zM17,21h3c1.1,0 2,-0.9 2,-2v-9h-5v11zM20,3L5,3c-1.1,0 -2,0.9 -2,2v3h19L22,5c0,-1.1 -0.9,-2 -2,-2zM3,19c0,1.1 0.9,2 2,2h3L8,10L3,10v9z";
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Цвета", TableSQl = "colors", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Марки", TableSQl = "marks", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Должность", TableSQl = "position", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Статус маршрута", TableSQl = "status_route", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Статус пользователя", TableSQl = "statuses", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Тип транcпорта", TableSQl = "vehicle_type", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Характеристика", TableSQl = "characteristic", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Единица измерения", TableSQl = "unit", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Атрибуты", TableSQl = "attribute", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Аэропорты", TableSQl = "airports", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Ангары", TableSQl = "hangar", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Количество самолетов", TableSQl = "count_air", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Самолеты", TableSQl = "aircraft", ContainerOpen = "TableContainer", Rectangle = imageRec });
            menuContents.Add(new MenuContent { Name = "Тип авиа", TableSQl = "air_transportation", ContainerOpen = "TableContainer", Rectangle = imageRec });
            return menuContents;
        }
        public List<MenuContent> CreateWordList()
        {
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Отчет", ContainerOpen = "WordContainer", Rectangle = "M16,3H5C3.9,3 3,3.9 3,5v14c0,1.1 0.9,2 2,2h14c1.1,0 2,-0.9 2,-2V8L16,3zM7,7h5v2H7V7zM17,17H7v-2h10V17zM17,13H7v-2h10V13zM15,9V5l4,4H15z" });
            return menuContents;
        }
        public List<MenuContent> CreateStatisticList()
        {
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Статистика по маркам", ContainerOpen = "StatisticContainer",StatisticElement = "0", Rectangle = "M11,2v20c-5.07,-0.5 -9,-4.79 -9,-10s3.93,-9.5 9,-10zM13.03,2v8.99L22,10.99c-0.47,-4.74 -4.24,-8.52 -8.97,-8.99zM13.03,13.01L13.03,22c4.74,-0.47 8.5,-4.25 8.97,-8.99h-8.97z" });
            menuContents.Add(new MenuContent { Name = "Статистика по должностям", ContainerOpen = "StatisticContainer", StatisticElement = "1", Rectangle = "M11,2v20c-5.07,-0.5 -9,-4.79 -9,-10s3.93,-9.5 9,-10zM13.03,2v8.99L22,10.99c-0.47,-4.74 -4.24,-8.52 -8.97,-8.99zM13.03,13.01L13.03,22c4.74,-0.47 8.5,-4.25 8.97,-8.99h-8.97z" });
            return menuContents;
        }
        public List<MenuContent> CreateVehicleList()
        {
            List<MenuContent> menuContents = new List<MenuContent>();
            menuContents.Add(new MenuContent { Name = "Заказы", ContainerOpen = "TableContainer", TableSQl = "vehicle", Rectangle = "M18,17L6,17v-2h12v2zM18,13L6,13v-2h12v2zM18,9L6,9L6,7h12v2zM3,22l1.5,-1.5L6,22l1.5,-1.5L9,22l1.5,-1.5L12,22l1.5,-1.5L15,22l1.5,-1.5L18,22l1.5,-1.5L21,22L21,2l-1.5,1.5L18,2l-1.5,1.5L15,2l-1.5,1.5L12,2l-1.5,1.5L9,2 7.5,3.5 6,2 4.5,3.5 3,2v20z" });
            return menuContents;
        }
    }
}
