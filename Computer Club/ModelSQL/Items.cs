using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Computer_Club.ModelSQL
{
    internal class Items
    {
        public string Id { get; set; }
        public string productName { get; set; }
        public string quantity { get; set; }
        public string productType { get; set; }
        public Items(string Id,string productName, string quantity, string productType)
        { 
            this.Id = Id;
            this.productName = productName;
            this.quantity = quantity;
            this.productType = productType;
        }

        public static List<Items> SetItems(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Inventory";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Items> items = new List<Items>();

                for (int i = 0; reader.Read(); i++)
                {
                    items.Add(new Items(reader[0].ToString(), reader.GetString(1), reader.GetString(3), reader.GetString(2)));
                }

                return items;
            }
        }
        public static List<Items> SetItems(string connectionString, string name, string text)
        {
            List<Items> items = new List<Items>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                switch (name)
                {
                    case "Название":
                        name = "Productname"; break;
                    case "Тип":
                        name = "Producttype"; break;
                    case "Качество":
                        name = "Quantity"; break;
                    default:
                        name = "*"; break;
                }
                connection.Open();
                string query = "SELECT * FROM Inventory";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (name != "*")
                {
                    query = $"SELECT * FROM Inventory WHERE {name} LIKE @text";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@text", $"%{text}%");
                    reader.Close();
                    reader = command.ExecuteReader();
                    items.Clear();
                }


                for (int i = 0; reader.Read(); i++)
                {
                    items.Add(new Items(reader[0].ToString(), reader.GetString(1), reader.GetString(3), reader.GetString(2)));
                }

                return items;
            }
        }
        public static void AddItem(string connectionString, string productName, string quantity, string productType)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO Inventory VALUES (@name,@type,@quant)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", productName);
                command.Parameters.AddWithValue("@type", productType);
                command.Parameters.AddWithValue("@quant", quantity);
                command.ExecuteNonQuery();
            }
        }
        public static void UpdateItem(string connectionString, string productName, string quantity, string productType, string id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string query = $"UPDATE Inventory SET Productname = @name, Quantity = @quant, Producttype = @type WHERE Id = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", productName);
                command.Parameters.AddWithValue("@type", productType);
                command.Parameters.AddWithValue("@quant", quantity);
                command.ExecuteNonQuery();

            }
        }
        public static void DeleteItem(string connectionString, string id)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string query = $"DELETE FROM Inventory WHERE Id = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();

            }
        }
    }
}
