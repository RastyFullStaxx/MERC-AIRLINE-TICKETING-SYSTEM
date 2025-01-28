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

namespace MERC
{
    public partial class Register_LogIn : BaseForm
    {
        DatabaseConnection dbConnection = new DatabaseConnection();
        String email, username, password, confirmpassword;
        String phoneNumber;



        public Register_LogIn()
        {
            InitializeComponent();

        }
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
            this.Hide();
        }

        private void btnLogInExistingAcc_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void btnConfirmAccountRegister_Click(object sender, EventArgs e)
        {
        /*   TESTING CONNECTION
         *   
         *   
         *   dbConnection.TestConnection();
            bool isConnected = dbConnection.TestConnection();
            if (isConnected)
            {
                MessageBox.Show("Connection Successful!");
            }
            else
            {
                MessageBox.Show("Database connection failed.");
            }
            */

            email = txtRegister_Email.Text;
            username = txtRegister_Username.Text;
            password = txtRegister_Password.Text.Trim();
            confirmpassword = txtConfrimRegister_Password.Text.Trim();
            phoneNumber = txtRegister_PhoneNumber.Text;

            try
            {
                // Check if the email contains the '@' symbol
                if (!email.Contains("@"))
                {
                    MessageBox.Show("Please enter a valid email address.");
                    return;  // Exit the method if email is invalid
                }

                // Check if the phone number contains any letters
                if (phoneNumber.Any(char.IsLetter))
                {
                    MessageBox.Show("Phone number should not contain letters.");
                    return;  // Exit the method if phone number contains letters
                }

                // Check if the passwords match
                if (!password.Equals(confirmpassword))
                {
                    MessageBox.Show("Password does not match!");
                    return;  // Exit the method if passwords don't match
                }

                bool isInserted = dbConnection.InsertAccountDetails(email, username, password, phoneNumber);

                if (isInserted)
                {
                    MessageBox.Show("Account created successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to create account. Please try again.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
    }
}
