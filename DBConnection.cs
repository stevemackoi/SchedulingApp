using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchedulingApp
{
    public static class DBConnection
    {
        private static readonly string server = "localhost";
        private static readonly string database = "client_schedule";
        private static readonly string uid = "sqlUser";
        private static readonly string password = "Passw0rd!";
        private static readonly string connectionString =
            $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password for database.");
                        break;
                    default:
                        MessageBox.Show($"Database error: {ex.Message}");
                        break;
                }
                return false;
            }
        }
    }
}
