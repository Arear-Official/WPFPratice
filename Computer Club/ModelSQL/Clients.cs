using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Microsoft.SqlServer.Server;

namespace Computer_Club.ModelSQL
{
    internal class ClientsSQL
    {
        public string ID { get; set; }
        public string Iinitials { get; set; }
        public string phoneNumber { get; set; }
        public string eMail { get; set; }

        public ClientsSQL(string ID, string Iinitials, string phoneNumber, string eMail)
        {
            if (ID == null)
                ID= "";
            if (Iinitials == null)
                Iinitials = "";
            if (phoneNumber == null)
                phoneNumber = "";
            if (eMail == null)
                eMail = "";
            this.ID = ID;
            this.Iinitials = Iinitials;
            this.phoneNumber = phoneNumber;
            this.eMail = eMail;
        }
        public static List<ClientsSQL> SetClients(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Clients";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<ClientsSQL> computers = new List<ClientsSQL>();

                for (int i = 0; reader.Read(); i++)
                {
                    computers.Add(new ClientsSQL(reader[0].ToString(), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }

                return computers;
            }
        }
        public static bool IsRepeat(string connectionString, string Initials)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT Count(*) FROM Clients WHERE Initials = '{Initials}'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader r = command.ExecuteReader();
                r.Read();
                return Convert.ToBoolean(r.GetInt32(0));
            }
        }
        public static List<ClientsSQL> SetClients(string connectionString, string name, string text)
        {
            List<ClientsSQL> computers = new List<ClientsSQL>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                
                switch (name)
                {
                    case "Инициалы":
                        name = "Initials"; break;
                    case "Номер":
                        name = "Phonenumber"; break;
                    default:
                        name = "*"; break;
                }
                string query = $"SELECT * FROM Clients";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; reader.Read(); i++)
                {
                    computers.Add(new ClientsSQL(reader[0].ToString(), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
                if (name != "*")
                {
                    if (name == "Initials")
                    {
                        query = $"SELECT * FROM Clients WHERE {name} LIKE @text";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@text", $"%{text}%");
                        reader.Close();
                        reader = command.ExecuteReader();
                        computers.Clear();
                        for (int i = 0; reader.Read(); i++)
                        {
                            computers.Add(new ClientsSQL(reader[0].ToString(), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                        }
                        return computers;
                    }

                    if (name == "Phonenumber")
                    {
                        query = $"SELECT * FROM Clients WHERE {name} LIKE '%{text}%'";
                        command = new SqlCommand(query, connection);
                        reader.Close();
                        reader = command.ExecuteReader();
                        computers.Clear();
                        for (int i = 0; reader.Read(); i++)
                        {
                            computers.Add(new ClientsSQL(reader[0].ToString(), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                        }
                        return computers;
                    }
                }

                return computers;
            }
        }

        public static void AddClient(string connectionString, string Initials, string PhoneNumber, string Email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO Clients VALUES (@init,'{PhoneNumber}','{Email}')";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@init", Initials);
                command.ExecuteNonQuery();                              
            }
        }

        public static void UpdateClient(string connectionString, string Initials, string PhoneNumber, string Email, string id)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                    
                    connection.Open();
                    string query = $"UPDATE Clients SET Initials = @init, Phonenumber = '{PhoneNumber}', Email = '{Email}' WHERE ID = {id}";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@init", Initials );
                    command.ExecuteNonQuery();

            }
        }
        public static void DeleteClient(string connectionString, string id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string query = $"DELETE FROM Clients WHERE ID = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();

            }
        }
        public static List<ClientsSQL> SetUselessClients(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Clients.ID, Clients.Initials FROM Clients Where Clients.ID NOT IN(SELECT ClientID FROM Services)";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<ClientsSQL> Clients = new List<ClientsSQL>();

                for (int i = 0; reader.Read(); i++)
                {
                    Clients.Add(new ClientsSQL(reader[0].ToString(), reader[1].ToString(), "", ""));
                }

                return Clients;
            }
        }
        public override string ToString()
        {
            return Iinitials;
        }
    }
}
