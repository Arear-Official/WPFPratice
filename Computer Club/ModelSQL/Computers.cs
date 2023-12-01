using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Computer_Club.ModelSQL
{
    internal class Computers
    {
        public string ID { get; set; }
        public string computerType{get;set;}
        public string computerState{ get;set;}
        public string computerName { get;set;}
        public string serviceName{ get;set;}
        public string serviceState{ get;set;}
        public string imagePath { get;set;}
        public Visibility visible { get; set; } = Visibility.Hidden;

        public Computers(string ID, string computerType, string computerState,string computerName, string serviceName, string serviceState)
        {
            this.ID = ID;
            this.computerType = computerType;
            this.computerState = computerState;
            this.computerName = computerName;
            this.serviceName = serviceName;
            this.serviceState = serviceState;
            this.imagePath = "\\Images\\Nothing.png";
            if (computerState == "On")
            this.imagePath = "\\Images\\Computer.png";
            if (serviceState == "Wait")
            this.imagePath = "\\Images\\ComputerWait.png";
            if (serviceState == "Work") 
                this.imagePath = "\\Images\\ComputerWork.png";
            if (computerState == "Off")
                this.imagePath = "\\Images\\ComputerOff.png";
            if (computerState == "Break")
                this.imagePath = "\\Images\\ComputerBreak.png";
            if (serviceState == "None")
            visible = Visibility.Visible;
        }
        public static List<Computers> SetComputers(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Computerclub";
                SqlCommand command = new SqlCommand(query,connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Computers> computers = new List<Computers>();

                for (int i = 0; reader.Read(); i++)
                {
                    computers.Add(new Computers(reader[0].ToString(), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)));
                }               
                
                return computers;
            }
        }

        public static List<Computers> SetComputers(string connectionString, string name, string text)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                switch(name)
                {
                    case "Тип компьютера":
                        name = "Computertype"; break;
                    case "Имя":
                        name = "Computername"; break;
                    case "Сервис":
                        name = "Servicename"; break;
                    default: 
                        name = "*"; break;
                }
                string query = $"SELECT * FROM Computerclub WHERE {name} LIKE '%{text}%'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Computers> computers = new List<Computers>();

                for (int i = 0; reader.Read(); i++)
                {
                    computers.Add(new Computers(reader[0].ToString(), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)));
                }

                return computers;
            }
        }        
    }
}
