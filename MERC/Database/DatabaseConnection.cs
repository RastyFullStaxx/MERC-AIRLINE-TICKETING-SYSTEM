using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // Import MySQL library

namespace MERC.Database
{
    internal class DatabaseConnection
    {
        // Connection string for your MySQL database
        private readonly string connectionString = "server=localhost;port=3307;database=mercdb;uid=root;pwd='';";

        //RASTY = connectionString = "server=localhost;database=mercdb;uid=root;pwd='180503';";
        //Rasty = connectionString = "server=localhost;port=3307;database=mercdb;uid=root;pwd='';";


        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }

        // Method to test the connection, now returns a bool
        public bool TestConnection()
        {
            MySqlConnection connection = GetConnection();

            try
            {
                // Debugging output to confirm connection string
                Console.WriteLine($"Attempting to connect with connection string: {connectionString}");

                connection.Open();
                Console.WriteLine("Connection to database successful!");

                // Optionally close the connection
                connection.Close();

                return true;  // Return true if connection is successful
            }
            catch (MySqlException mysqlEx)
            {
                // Specific MySQL exception handling
                Console.WriteLine($"MySQL Error Code: {mysqlEx.Number}");
                Console.WriteLine($"Error connecting to database: {mysqlEx.Message}");

                return false; // Return false if there's a MySQL error
            }
            catch (Exception ex)
            {
                // General exception handling
                Console.WriteLine($"An error occurred: {ex.Message}");

                return false; // Return false for general errors
            }
        }

        public bool InsertAccountDetails(string email, string username, string password, string phoneNumber)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO AccountInformation (Email, Username, Password, PhoneNumber) VALUES (@Email, @Username, @Password, @PhoneNumber)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    // If the insertion is successful, rowsAffected will be > 0
                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                // Handle SQL exception
                Console.WriteLine($"MySQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
   