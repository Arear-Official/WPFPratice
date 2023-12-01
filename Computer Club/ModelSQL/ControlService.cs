using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Computer_Club.ModelSQL
{
    internal class ControlService
    {
        public static List<Computers> SetReadyComputers(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Computerclub WHERE Servicestate = 'Off' OR Servicestate = 'On'";
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
        public static List<Computers> SetWorkComputers(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Computerclub WHERE Servicestate = 'Work";
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
        public static List<string> FillClients(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Clients.ID, Initials FROM Clients, Services WHERE ID NOT IN (SELECT DISTINCT ClientsID FROM Services)";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<string> Clients = new List<string>();

                for (int i = 0; reader.Read(); i++)
                {
                    Clients.Add(reader[0].ToString() + reader.GetString(1));
                }

                return Clients;
            }
        }
        public static void AddService(string connectionString, string clientid,string computerId, string servicename, int time, string cost)
        {

        }
    }

    public class Services
    {
        public string Id { get; set; }
        public string clientId { get; set; }
        public string computerId { get; set; }
        public string computerState { get; set; }
        public string computerName { get; set; }
        public string serviceName { get; set; }
        public string serviceState { get; set; }
        public string imagePath { get; set; }
        public int time { get; set; }
        public int cost { get; set; }

        public Services(string id, string clientId, string computerId, string computerName, string serviceName, int time, int cost,string computerState,string serviceState)
        {
            this.Id = id;
            this.clientId = clientId;
            this.computerId = computerId;
            this.computerState = computerState;
            this.computerName = computerName;
            this.serviceName = serviceName;
            this.serviceState = serviceState;
            this.time = time;
            this.cost = cost;
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
        }

        public static List<Services> SetServices(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Services.Id, Services.ClientID, Services.ComputerId, Computerclub.Computername, Services.Servicename, Services.Time, Services.Cost, Computerclub.Computerstate, Computerclub.Servicestate FROM Computerclub, Services Where Computerclub.Id = Services.ComputerId";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Services> computers = new List<Services>();

                for (int i = 0; reader.Read(); i++)
                {
                    computers.Add(new Services(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader.GetString(3), reader.GetString(4), reader.GetInt32(5), int.Parse(reader.GetString(6)), reader.GetString(7), reader.GetString(8)));
                }

                return computers;
            }
        }
        public static void AddSecvices(string connectionString, int clientId, string computerId, string servicename, int time, string cost)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                    connection.Open();
                    string query = $"UPDATE Services SET Servicename = @servicename, ClientId = @clientId,Time = @time,Cost = @cost  WHERE computerId = @computerId"; ;
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.Parameters.AddWithValue("@computerId", int.Parse(computerId));
                    command.Parameters.AddWithValue("@servicename", servicename);
                    command.Parameters.AddWithValue("@time", time);
                    command.Parameters.AddWithValue("@cost", cost);
                    command.ExecuteNonQuery();
                    query = $"UPDATE Computerclub SET Servicename = @servicename, Servicestate = 'Wait' WHERE Id = @computerId";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@computerId", int.Parse(computerId));
                    command.Parameters.AddWithValue("@servicename", servicename);
                    command.ExecuteNonQuery();               
            }
        }
        public static void DeleteServices(string connectionString, string computerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"UPDATE Services SET  ClientId = 0 WHERE computerId = @computerId"; 
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@computerId", int.Parse(computerId));
                command.ExecuteNonQuery();
                query = $"UPDATE Computerclub SET Servicestate = 'None' WHERE Id = @computerId";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@computerId", int.Parse(computerId));
                command.ExecuteNonQuery();
            }
        }
        public static bool IsRepeat(string connectionString, string Name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT Count(*) FROM Services WHERE Servicename = '{Name}'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader r = command.ExecuteReader();
                r.Read();
                return Convert.ToBoolean(r.GetInt32(0));
            }
        }
        public static bool IsHadService(string connectionString, string ComputerID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT Count(*) FROM Computerclub WHERE Id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", int.Parse(ComputerID));
                SqlDataReader r = command.ExecuteReader();
                r.Read();
                return Convert.ToBoolean(r.GetInt32(0));
            }
        }
    }

}
