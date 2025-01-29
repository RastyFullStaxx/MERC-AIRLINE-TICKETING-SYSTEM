using MERC.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace MERC
{
    public partial class Register_LogIn : BaseForm
    {
        DatabaseConnection dbConnection = new DatabaseConnection();
        // Encapsulated fields for form-level use
        public int AccountID { get; private set; }  // Added AccountID
        public string FullName { get; private set; } // Added FullName
        private string Email { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string ConfirmPassword { get; set; }
        private string PhoneNumber { get; set; }

        public Register_LogIn()
        {
            InitializeComponent();
        }

        // Hash Password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private void ShowError(int errorCode, string description)
        {
            ReportError errorForm = new ReportError();
            errorForm.Controls["lblErrorCode"].Text = errorCode.ToString();
            errorForm.Controls["lblErrorDescription"].Text = description;
            errorForm.ShowDialog();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            // Retrieve input from login fields
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                // Validate that both fields are not empty
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ShowError(4, "Please enter both username and password.");
                    return;
                }

                // Hash the entered password for comparison
                string hashedPassword = HashPassword(password);

                // Query the database to check if credentials are correct
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT AccountID, Email, PhoneNumber, FullName FROM AccountInformation WHERE Username = @Username AND Password = @Password";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Account found, retrieve account information
                            AccountID = reader.GetInt32("AccountID"); // Assign to the encapsulated property
                            Email = reader.GetString("Email");
                            PhoneNumber = reader.GetString("PhoneNumber");
                            FullName = reader.GetString("FullName"); // Assign to the encapsulated property
                            Username = txtUsername.Text; // Retrieve from the textbox input

                            // Pass the account information to the next form (LandingPage)
                            LandingPage landingPage = new LandingPage(AccountID, Username, Email, PhoneNumber, FullName);
                            landingPage.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Show an error if the account is not found
                            ShowError(7, "Invalid username or password. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"An error occurred: {ex.Message}");
            }
        }


        private void btnLogInExistingAcc_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void btnConfirmAccountRegister_Click(object sender, EventArgs e)
        {
            Email = txtRegister_Email.Text.Trim();
            Username = txtRegister_Username.Text.Trim();
            Password = txtRegister_Password.Text.Trim();
            ConfirmPassword = txtConfrimRegister_Password.Text.Trim();
            PhoneNumber = txtRegister_PhoneNumber.Text.Trim();

            try
            {
                // Validate email format
                if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ShowError(4, "Please enter a valid email address.");
                    return;
                }

                // Validate phone number
                if (!System.Text.RegularExpressions.Regex.IsMatch(PhoneNumber, @"^\d{10,15}$"))
                {
                    ShowError(4, "Phone number must be 10-15 digits and contain no letters.");
                    return;
                }

                // Validate password strength
                if (Password.Length < 8 || !Password.Any(char.IsDigit) || !Password.Any(char.IsUpper))
                {
                    ShowError(4, "Password must be at least 8 characters long, include a number, and an uppercase letter.");
                    return;
                }

                // Check if passwords match
                if (!Password.Equals(ConfirmPassword))
                {
                    ShowError(4, "Passwords do not match!");
                    return;
                }

                // Hash password
                string hashedPassword = HashPassword(Password);

                // Insert account details into the database
                bool isInserted = dbConnection.InsertAccountDetails(Email, Username, hashedPassword, PhoneNumber);

                if (isInserted)
                {
                    MessageBox.Show("Account created successfully! Please log in.");
                    panel2.Visible = false; // Hide registration panel
                    panel1.Visible = true; // Show login panel
                    txtUsername.Text = Username; // Auto-fill username
                    txtPassword.Text = ""; // Clear password field
                }
                else
                {
                    ShowError(7, "Failed to create account. Please try again.");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException mysqlEx) when (mysqlEx.Number == 1062)
            {
                ShowError(7, "The email or username is already registered. Please try a different one.");
            }
            catch (Exception ex)
            {
                ShowError(7, $"An error occurred: {ex.Message}");
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            bool isConnected = dbConnection.TestConnection();
            if (isConnected)
            {
                MessageBox.Show("Connection to the database is successful!");
            }
            else
            {
                ShowError(7, "Database connection failed. Check your server configuration.");
            }
        }
    }
}
