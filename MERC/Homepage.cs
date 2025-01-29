using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using MySql.Data.MySqlClient;
using System.Data.Common;
using MERC.Database;


// panel 1 = (Booking Process) gets user input
// panel 2 = (Booking COnfirmation) displays the users inputs for confirmation
// panel 3 = (Booking Guidlines) contains guidelines
// panel 4 = (About Us)
// panel 5 = (View Flight Schedule)
// panel 6 = (Homepage)
// panel 7 = (View Bookings)
// panel 8 = (View Transactions)
// panel9_DetailedFlightView = (View Flight Details)



namespace MERC
{
    public partial class Homepage : Form
    {

        // Encapsulated properties for logged-in account information
        public int AccountID { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string FullName { get; private set; }

        // Database connection instance (readonly to ensure it's not reassigned)
        private readonly DatabaseConnection dbConnection = new DatabaseConnection();

        // Constructor to initialize the form with account details
        public Homepage(int accountId, string username, string email, string phoneNumber, string fullName)
        {
            InitializeComponent();

            // Assign the values to encapsulated properties
            this.AccountID = accountId;
            this.Username = username;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.FullName = fullName;

            // Initialize the database connection
            this.dbConnection = new DatabaseConnection();

            // Attach event handlers
            cbOrigin.SelectedIndexChanged += cbOrigin_SelectedIndexChanged;
            cbDestination.SelectedIndexChanged += cbDestination_SelectedIndexChanged;
            cbNumberofPassengers.SelectedIndexChanged += cbNumberofPassengers_SelectedIndexChanged;

            // Set pricing
            lblClassFare.Text = "0.00";
            lblTravelTax.Text = "0.00";
            lblTransactionFee.Text = "0.00";

            // Attach event handleres
            foreach (RadioButton radio in grpTravelType.Controls.OfType<RadioButton>())
            {
                radio.CheckedChanged += grpTravelType_CheckedChanged;
            }

            foreach (RadioButton radio in grpClassType.Controls.OfType<RadioButton>())
            {
                radio.CheckedChanged += grpClassType_CheckedChanged;
            }
        }

        private void btnConfirmBooking_Click(object sender, EventArgs e)
        {
            // Input validation
            try
            {
                // 1. Retrieve and validate Name
                string fullName = txtName.Text.Trim();
                if (string.IsNullOrEmpty(fullName))
                {
                    ShowError(4, "Please enter the passenger's full name.");
                    return;
                }

                // 2. Retrieve and validate Age
                if (!int.TryParse(txtAge.Text.Trim(), out int age) || age <= 0)
                {
                    ShowError(3, "Please enter a valid age greater than 0.");
                    return;
                }

                // 3. Retrieve and validate ComboBox selections
                string origin = cbOrigin.SelectedItem?.ToString();
                string destination = cbDestination.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination))
                {
                    ShowError(5, "Please select both origin and destination.");
                    return;
                }
                if (origin == destination)
                {
                    ShowError(5, "Origin and destination cannot be the same.");
                    return;
                }

                // 4. Validate Number of Passengers
                if (!int.TryParse(cbNumberofPassengers.SelectedItem?.ToString(), out int numberOfPassengers) || numberOfPassengers <= 0)
                {
                    ShowError(4, "Please select a valid number of passengers.");
                    return;
                }

                // 5. Retrieve and validate Travel Type
                string travelType = grpTravelType.Controls.OfType<RadioButton>()
                                     .FirstOrDefault(r => r.Checked)?.Text;
                if (string.IsNullOrEmpty(travelType))
                {
                    ShowError(4, "Please select a travel type (One-way or Round-trip).");
                    return;
                }

                // 6. Retrieve and validate Class Type
                string classType = grpClassType.Controls.OfType<RadioButton>()
                                     .FirstOrDefault(r => r.Checked)?.Text;
                if (string.IsNullOrEmpty(classType))
                {
                    ShowError(4, "Please select a class type (Private, Business, or Regular).");
                    return;
                }

                // 7. Retrieve Travel Insurance Option
                bool hasTravelInsurance = chkbTravelInsurance.Checked;

                // Fetch pricing data and calculate total cost
                float totalClassFare = 0.0f, totalTravelTax = 0.0f, transactionFee = 0.0f, insuranceFee = 0.0f;

                try
                {
                    using (var connection = dbConnection.GetConnection())
                    {
                        connection.Open();

                        // Query to fetch fare from FlightInformation table
                        string fareQuery = "SELECT PrivateFare, BusinessFare, RegularFare FROM FlightInformation " +
                                           "WHERE Origin = @Origin AND Destination = @Destination";
                        MySqlCommand fareCommand = new MySqlCommand(fareQuery, connection);
                        fareCommand.Parameters.AddWithValue("@Origin", origin);
                        fareCommand.Parameters.AddWithValue("@Destination", destination);

                        using (var reader = fareCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                float farePerPerson = classType switch
                                {
                                    "Private" => reader.GetFloat("PrivateFare"),
                                    "Business" => reader.GetFloat("BusinessFare"),
                                    "Regular" => reader.GetFloat("RegularFare"),
                                    _ => 0.0f
                                };
                                totalClassFare = farePerPerson * numberOfPassengers;
                            }
                        }

                        // Fetch Travel Tax
                        totalTravelTax = GetTravelTaxFromDB(classType) * numberOfPassengers;

                        // Fetch Transaction Fee
                        transactionFee = GetTransactionFeeFromDB(classType);

                        // Fetch Insurance Fee (if applicable)
                        insuranceFee = hasTravelInsurance ? GetInsuranceFeeFromDB(classType) * numberOfPassengers : 0.0f;
                    }
                }
                catch (Exception ex)
                {
                    ShowError(7, $"Database error: {ex.Message}");
                    return;
                }

                // Calculate Total Cost
                float totalCost = totalClassFare + totalTravelTax + transactionFee + insuranceFee;

                // Insert booking into database
                try
                {
                    using (var connection = dbConnection.GetConnection())
                    {
                        connection.Open();

                        // Generate a unique Control Number
                        string controlNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                        // Insert into BookingInformation table
                        string insertBookingQuery = "INSERT INTO BookingInformation (ControlNumber, FlightType, TravelType, NumberOfPassengers, " +
                                                    "TransactionFee, TotalCost, AccountID) " +
                                                    "VALUES (@ControlNumber, @FlightType, @TravelType, @NumberOfPassengers, " +
                                                    "@TransactionFee, @TotalCost, @AccountID)";
                        MySqlCommand bookingCommand = new MySqlCommand(insertBookingQuery, connection);
                        bookingCommand.Parameters.AddWithValue("@ControlNumber", controlNumber);
                        bookingCommand.Parameters.AddWithValue("@FlightType", origin == "Manila" || destination == "Manila" ? "Local" : "International");
                        bookingCommand.Parameters.AddWithValue("@TravelType", travelType);
                        bookingCommand.Parameters.AddWithValue("@NumberOfPassengers", numberOfPassengers);
                        bookingCommand.Parameters.AddWithValue("@TransactionFee", transactionFee);
                        bookingCommand.Parameters.AddWithValue("@TotalCost", totalCost);
                        bookingCommand.Parameters.AddWithValue("@AccountID", this.AccountID); // Assume currentAccountID is globally available

                        int rowsAffected = bookingCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Booking confirmed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            ShowError(7, "Failed to confirm booking. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError(7, $"Database error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"An unexpected error occurred: {ex.Message}");
            }
        }



        private void Booking1_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;  // Show panel2
            panel3.Visible = false;
            panel1.Visible = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // implement clearance of user inputs upon click

            Homepage homepage = new Homepage(AccountID, Username, Email, PhoneNumber, FullName);
            homepage.Show();
            this.Hide();
        }

        private void lblConfirm_ConfirmBooking_Click(object sender, EventArgs e)
        {
            Ticket ticket = new Ticket();
            ticket.Show();
            this.Hide();
        }

        private void btnEditBooking_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // implement clearance of user inputs upon click

            Homepage homepage = new Homepage(AccountID, Username, Email, PhoneNumber, FullName);
            homepage.Show();
            this.Hide();
        }

        private void navbtnHomepage_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = true;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = false;

            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Active.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Inactive.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");
        }

        private void navbtnAboutUs_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = false;

            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Inactive.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Active.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");

        }

        private void navbtnBbook_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = false;

            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Active.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Inactive.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");
        }

        private void navbtnFlighSchedule_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = true;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = false;

            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Active.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");
        }

        private void navbtnViewBooking_Click(object sender, EventArgs e)
        {

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = true;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = false;

            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Inactive.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Active.png");
        }

        private void navbtnTransactions_Click(object sender, EventArgs e)
        {

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = true;
            panel9_DetailedFlightView.Visible = false;

            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Inactive.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Active.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");
        }

        private void btnBook_HomePageHomePage_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = false;

            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Active.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Inactive.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");
        }

        private void lblViewDetails_Return_Click(object sender, EventArgs e)
        {
            panel9_DetailedFlightView.Visible = false;
        }

        private bool isUpdating = false; // Flag to prevent recursive updates

        private void cbOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdating) return; // Prevent recursion during updates
            isUpdating = true;

            try
            {
                // Get the selected origin
                string selectedOrigin = cbOrigin.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedOrigin)) return;

                // Update the destination combo box to exclude the selected origin
                UpdateComboBoxItems(cbDestination, selectedOrigin);

                // Trigger fare computation if destination and class type are already selected
                string selectedDestination = cbDestination.SelectedItem?.ToString();
                string classType = grpClassType.Controls.OfType<RadioButton>()
                                       .FirstOrDefault(r => r.Checked)?.Text;

                if (!string.IsNullOrEmpty(selectedDestination) && !string.IsNullOrEmpty(classType))
                {
                    UpdateFareAndTaxes(selectedOrigin, selectedDestination, classType);
                }
            }
            finally
            {
                isUpdating = false; // Reset the flag
            }
        }

        private void cbDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdating) return; // Prevent recursion during updates
            isUpdating = true;

            try
            {
                // Get the selected destination
                string selectedDestination = cbDestination.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedDestination)) return;

                // Update the origin combo box to exclude the selected destination
                UpdateComboBoxItems(cbOrigin, selectedDestination);

                // Trigger fare computation if origin and class type are already selected
                string selectedOrigin = cbOrigin.SelectedItem?.ToString();
                string classType = grpClassType.Controls.OfType<RadioButton>()
                                       .FirstOrDefault(r => r.Checked)?.Text;

                if (!string.IsNullOrEmpty(selectedOrigin) && !string.IsNullOrEmpty(classType))
                {
                    UpdateFareAndTaxes(selectedOrigin, selectedDestination, classType);
                }
            }
            finally
            {
                isUpdating = false; // Reset the flag
            }
        }

        private void UpdateComboBoxItems(ComboBox comboBoxToUpdate, string selectedItemToRemove)
        {
            // Temporarily disable the SelectedIndexChanged event to prevent infinite recursion
            comboBoxToUpdate.SelectedIndexChanged -= cbOrigin_SelectedIndexChanged;
            comboBoxToUpdate.SelectedIndexChanged -= cbDestination_SelectedIndexChanged;

            try
            {
                string currentSelection = comboBoxToUpdate.SelectedItem?.ToString();

                // Populate the combo box with all possible values, excluding the selected item
                string[] locations = { "Manila", "Batanes", "Palawan", "South Korea", "Japan", "Vietnam" };

                comboBoxToUpdate.Items.Clear();
                foreach (string location in locations)
                {
                    if (location != selectedItemToRemove)
                    {
                        comboBoxToUpdate.Items.Add(location);
                    }
                }

                // Retain the current selection if valid
                if (!string.IsNullOrEmpty(currentSelection) && comboBoxToUpdate.Items.Contains(currentSelection))
                {
                    comboBoxToUpdate.SelectedItem = currentSelection;
                }
            }
            finally
            {
                // Re-enable the appropriate SelectedIndexChanged event
                comboBoxToUpdate.SelectedIndexChanged += cbOrigin_SelectedIndexChanged;
                comboBoxToUpdate.SelectedIndexChanged += cbDestination_SelectedIndexChanged;
            }
        }




        private void UpdateFareAndTaxes(string origin, string destination, string classType)
        {
            int numberOfPassengers = previousNumberOfPassengers;
            string travelType = grpTravelType.Controls.OfType<RadioButton>()
                                    .FirstOrDefault(r => r.Checked)?.Text;

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    // Query fare data from the database
                    string fareQuery = "SELECT PrivateFare, BusinessFare, RegularFare FROM FlightInformation " +
                                       "WHERE Origin = @Origin AND Destination = @Destination";
                    MySqlCommand fareCommand = new MySqlCommand(fareQuery, connection);
                    fareCommand.Parameters.AddWithValue("@Origin", origin);
                    fareCommand.Parameters.AddWithValue("@Destination", destination);

                    using (var reader = fareCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            float farePerPerson = classType switch
                            {
                                "Private" => reader.GetFloat("PrivateFare"),
                                "Business" => reader.GetFloat("BusinessFare"),
                                "Regular" => reader.GetFloat("RegularFare"),
                                _ => 0.0f
                            };

                            float travelTaxPerPerson = GetTravelTaxFromDB(classType);
                            float transactionFee = GetTransactionFeeFromDB(classType);

                            // Calculate total fares
                            float totalClassFare = farePerPerson * numberOfPassengers;
                            float totalTravelTax = travelTaxPerPerson * numberOfPassengers;

                            // Apply round-trip adjustment
                            if (travelType == "Round-trip")
                            {
                                totalClassFare *= 2;
                            }

                            // Update labels
                            lblClassFare.Text = totalClassFare.ToString("0.00");
                            lblTravelTax.Text = totalTravelTax.ToString("0.00");
                            lblTransactionFee.Text = transactionFee.ToString("0.00");
                        }
                        else
                        {
                            ShowError(5, "No fare information found for the selected route.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
            }
        }


        private float GetFareFromRoute(string origin, string destination, string classType)
        {
            // Example pricing from your documentation
            var routeFares = new Dictionary<(string, string, string), float>
    {
        { ("Manila", "Batanes", "Private"), 8000.0f },
        { ("Manila", "Batanes", "Business"), 12500.0f },
        { ("Manila", "Batanes", "Regular"), 3500.0f },

        { ("Batanes", "Manila", "Private"), 9800.0f },
        { ("Batanes", "Manila", "Business"), 12950.0f },
        { ("Batanes", "Manila", "Regular"), 3900.0f },

        { ("Manila", "Palawan", "Private"), 9100.0f },
        { ("Manila", "Palawan", "Business"), 10500.0f },
        { ("Manila", "Palawan", "Regular"), 3200.0f },

        { ("Palawan", "Manila", "Private"), 9850.0f },
        { ("Palawan", "Manila", "Business"), 10975.0f },
        { ("Palawan", "Manila", "Regular"), 3575.0f },

        { ("Manila", "South Korea", "Business"), 37490.0f },
        { ("Manila", "South Korea", "Regular"), 12055.0f },

        { ("South Korea", "Manila", "Business"), 39650.0f },
        { ("South Korea", "Manila", "Regular"), 13100.0f },

        { ("Manila", "Japan", "Private"), 40450.0f },
        { ("Manila", "Japan", "Business"), 45355.0f },
        { ("Manila", "Japan", "Regular"), 27800.0f },

        { ("Japan", "Manila", "Private"), 43855.0f },
        { ("Japan", "Manila", "Business"), 49780.0f },
        { ("Japan", "Manila", "Regular"), 29400.0f },

        { ("Manila", "Vietnam", "Private"), 8505.0f },
        { ("Manila", "Vietnam", "Business"), 12345.0f },
        { ("Manila", "Vietnam", "Regular"), 3200.0f },

        { ("Vietnam", "Manila", "Private"), 14300.0f },
        { ("Vietnam", "Manila", "Business"), 16320.0f },
        { ("Vietnam", "Manila", "Regular"), 4600.0f }
    };

            return routeFares.TryGetValue((origin, destination, classType), out float fare) ? fare : 0.0f;
        }


        private void ShowError(int errorCode, string description)
        {
            ReportError errorForm = new ReportError();
            errorForm.Controls["lblErrorCode"].Text = errorCode.ToString();
            errorForm.Controls["lblErrorDescription"].Text = description;
            errorForm.ShowDialog();
        }

        private float GetFarePerPerson(string origin, string destination, string classType)
        {
            // Example pricing based on your documentation
            if (classType == "Private")
                return 8000.0f; // Example fare
            else if (classType == "Business")
                return 12500.0f;
            else if (classType == "Regular")
                return 3500.0f;

            return 0.0f; // Default if no match
        }

        private float GetTravelTaxFromDB(string classType)
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT TravelTaxPerPerson FROM TravelTaxInformation WHERE ClassType = @ClassType";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ClassType", classType);

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToSingle(result) : 0.0f;
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Error fetching Travel Tax: {ex.Message}");
                return 0.0f;
            }
        }


        private float GetInsuranceFeeFromDB(string classType)
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT InsuranceFeePerPerson FROM InsuranceInformation WHERE ClassType = @ClassType";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ClassType", classType);

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToSingle(result) : 0.0f;
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Error fetching Insurance Fee: {ex.Message}");
                return 0.0f;
            }
        }


        private float GetTransactionFeeFromDB(string classType)
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT TransactionFeePerBooking FROM TransactionFeeInformation WHERE ClassType = @ClassType";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ClassType", classType);

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToSingle(result) : 0.0f;
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Error fetching Transaction Fee: {ex.Message}");
                return 0.0f;
            }
        }


        private bool SaveBookingToDatabase(string fullName, int age, string origin, string destination,
                                   int numberOfPassengers, string travelType, string classType,
                                   bool hasTravelInsurance, float totalFare)
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "INSERT INTO BookingInformation (FullName, Age, Origin, Destination, " +
                                   "NumberOfPassengers, TravelType, ClassType, HasTravelInsurance, TotalFare) " +
                                   "VALUES (@FullName, @Age, @Origin, @Destination, @NumberOfPassengers, @TravelType, " +
                                   "@ClassType, @HasTravelInsurance, @TotalFare)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Origin", origin);
                    cmd.Parameters.AddWithValue("@Destination", destination);
                    cmd.Parameters.AddWithValue("@NumberOfPassengers", numberOfPassengers);
                    cmd.Parameters.AddWithValue("@TravelType", travelType);
                    cmd.Parameters.AddWithValue("@ClassType", classType);
                    cmd.Parameters.AddWithValue("@HasTravelInsurance", hasTravelInsurance);
                    cmd.Parameters.AddWithValue("@TotalFare", totalFare);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if data was inserted successfully
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        private int previousNumberOfPassengers = 0; // Store the last selected value

        private void cbNumberofPassengers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure a valid selection
            if (!int.TryParse(cbNumberofPassengers.SelectedItem?.ToString(), out int numberOfPassengers) || numberOfPassengers <= 0)
            {
                return; // Exit if no valid selection is made
            }

            // Get current selected values for Origin, Destination, and Class Type
            string origin = cbOrigin.SelectedItem?.ToString();
            string destination = cbDestination.SelectedItem?.ToString();
            string classType = grpClassType.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(r => r.Checked)?.Text;

            // Validate selections
            if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(classType))
            {
                ShowError(4, "Please ensure Origin, Destination, and Class Type are selected.");
                return;
            }

            // Query database to get pricing details
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    // Fetch fare for the selected route and class type
                    string fareQuery = "SELECT PrivateFare, BusinessFare, RegularFare FROM FlightInformation " +
                                       "WHERE Origin = @Origin AND Destination = @Destination";
                    MySqlCommand fareCommand = new MySqlCommand(fareQuery, connection);
                    fareCommand.Parameters.AddWithValue("@Origin", origin);
                    fareCommand.Parameters.AddWithValue("@Destination", destination);

                    using (var reader = fareCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            float farePerPerson = classType switch
                            {
                                "Private" => reader.GetFloat("PrivateFare"),
                                "Business" => reader.GetFloat("BusinessFare"),
                                "Regular" => reader.GetFloat("RegularFare"),
                                _ => 0.0f
                            };

                            // Fetch travel tax and transaction fee for the class type
                            float travelTaxPerPerson = GetTravelTaxFromDB(classType);
                            float transactionFee = GetTransactionFeeFromDB(classType);

                            // Calculate totals
                            float totalClassFare = farePerPerson * numberOfPassengers;
                            float totalTravelTax = travelTaxPerPerson * numberOfPassengers;

                            // Update the labels
                            lblClassFare.Text = totalClassFare.ToString("0.00");
                            lblTravelTax.Text = totalTravelTax.ToString("0.00");
                            lblTransactionFee.Text = transactionFee.ToString("0.00");
                        }
                        else
                        {
                            ShowError(5, "No pricing data found for the selected route.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
            }
        }


        private float GetFarePerPerson(string classType)
        {
            return classType switch
            {
                "Private" => 8000.0f,
                "Business" => 12500.0f,
                "Regular" => 3500.0f,
                _ => 0.0f
            };
        }

        private void grpTravelType_CheckedChanged(object sender, EventArgs e)
        {
            // Get the selected Travel Type
            string travelType = grpTravelType.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(r => r.Checked)?.Text;
            if (string.IsNullOrEmpty(travelType)) return;

            // Get selected values for computation
            string origin = cbOrigin.SelectedItem?.ToString();
            string destination = cbDestination.SelectedItem?.ToString();
            string classType = grpClassType.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(r => r.Checked)?.Text;
            int numberOfPassengers = previousNumberOfPassengers;

            if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(classType))
            {
                ShowError(4, "Ensure that Origin, Destination, and Class Type are selected before selecting Travel Type.");
                return;
            }

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    // Query the FlightInformation table for the fare
                    string fareQuery = "SELECT PrivateFare, BusinessFare, RegularFare FROM FlightInformation " +
                                       "WHERE Origin = @Origin AND Destination = @Destination";
                    MySqlCommand fareCommand = new MySqlCommand(fareQuery, connection);
                    fareCommand.Parameters.AddWithValue("@Origin", origin);
                    fareCommand.Parameters.AddWithValue("@Destination", destination);

                    using (var reader = fareCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            float farePerPerson = classType switch
                            {
                                "Private" => reader.GetFloat("PrivateFare"),
                                "Business" => reader.GetFloat("BusinessFare"),
                                "Regular" => reader.GetFloat("RegularFare"),
                                _ => 0.0f
                            };

                            // Compute the total class fare
                            float totalClassFare = farePerPerson * numberOfPassengers;

                            // Apply round-trip fare adjustment
                            if (travelType == "Round-trip")
                            {
                                totalClassFare *= 2; // Double the fare for round-trip
                            }

                            // Update Class Fare Label
                            lblClassFare.Text = totalClassFare.ToString("0.00");
                        }
                        else
                        {
                            ShowError(5, "No fare information found for the selected route.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
            }
        }


        private void grpClassType_CheckedChanged(object sender, EventArgs e)
        {
            // Get the selected Class Type
            string classType = grpClassType.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(r => r.Checked)?.Text;
            if (string.IsNullOrEmpty(classType)) return;

            // Get selected values for computation
            string origin = cbOrigin.SelectedItem?.ToString();
            string destination = cbDestination.SelectedItem?.ToString();
            string travelType = grpTravelType.Controls.OfType<RadioButton>()
                                    .FirstOrDefault(r => r.Checked)?.Text;
            int numberOfPassengers = previousNumberOfPassengers;

            if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(travelType))
            {
                ShowError(4, "Ensure that Origin, Destination, and Travel Type are selected before selecting Class Type.");
                return;
            }

            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    // Query the FlightInformation table for the fare
                    string fareQuery = "SELECT PrivateFare, BusinessFare, RegularFare FROM FlightInformation " +
                                       "WHERE Origin = @Origin AND Destination = @Destination";
                    MySqlCommand fareCommand = new MySqlCommand(fareQuery, connection);
                    fareCommand.Parameters.AddWithValue("@Origin", origin);
                    fareCommand.Parameters.AddWithValue("@Destination", destination);

                    using (var reader = fareCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            float farePerPerson = classType switch
                            {
                                "Private" => reader.GetFloat("PrivateFare"),
                                "Business" => reader.GetFloat("BusinessFare"),
                                "Regular" => reader.GetFloat("RegularFare"),
                                _ => 0.0f
                            };

                            float travelTaxPerPerson = GetTravelTaxFromDB(classType);
                            float transactionFee = GetTransactionFeeFromDB(classType);

                            // Compute total fares
                            float totalClassFare = farePerPerson * numberOfPassengers;
                            float totalTravelTax = travelTaxPerPerson * numberOfPassengers;

                            // Apply round-trip adjustment
                            if (travelType == "Round-trip")
                            {
                                totalClassFare *= 2;
                            }

                            // Update labels
                            lblClassFare.Text = totalClassFare.ToString("0.00");
                            lblTravelTax.Text = totalTravelTax.ToString("0.00");
                            lblTransactionFee.Text = transactionFee.ToString("0.00");
                        }
                        else
                        {
                            ShowError(5, "No fare information found for the selected route.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
            }
        }



    }
}
