using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace AirTransportationModern.Classes
{
    public class CardRoutes
    {
        static List<CardRoutes> MenuRoutes = new List<CardRoutes>();
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public string Cost { get; set; }
        public Brush ColorBorder { get; set; } = Brushes.White;
        public static List<CardRoutes> Create(string sqlCommand)
        {
            MenuRoutes.Clear();
            DataTable dataTable = ClassLogic.LoadData(sqlCommand);
            int Count = 0;
            foreach (DataRow item in dataTable.Rows)
            {
                if (Count % 2 == 0)
                    MenuRoutes.Add(new CardRoutes { Name = item[4].ToString(), Time = item[1].ToString(), Cost = "Цена: " + item[3].ToString() + ".00₽", ServiceID = Convert.ToInt32(item[0])});
                else
                    MenuRoutes.Add(new CardRoutes { Name = item[4].ToString(), Time = item[1].ToString(), ColorBorder = Brushes.Orange, Cost = "Цена: " + item[3].ToString() + ".00₽", ServiceID = Convert.ToInt32(item[0]) });
                Count++;
            }
            return MenuRoutes;
        } 
    }
}
