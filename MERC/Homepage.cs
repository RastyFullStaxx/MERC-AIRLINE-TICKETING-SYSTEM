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
        private string GeneratedControlNumber;  // Store control number


        // Database connection instance (readonly to ensure it's not reassigned)
        private readonly DatabaseConnection dbConnection = new DatabaseConnection();

        // Constructor to initialize the form with account details
        public Homepage(int accountId, string username, string email, string phoneNumber, string fullName, string controlNumber)
        {
            InitializeComponent();

            // Load Data to History
            LoadTransactions();
            tblViewFlightSchedules.CellFormatting += tblViewFlightSchedules_CellFormatting;
            tblViewFlightSchedules.CellClick += tblViewFlightSchedules_CellClick;


            // Assign the values to encapsulated properties
            this.AccountID = accountId;
            this.Username = username;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.FullName = fullName;
            this.GeneratedControlNumber = controlNumber;


            // Initialize the database connection
            this.dbConnection = new DatabaseConnection();

            // Attach event handlers
            cbOrigin.SelectedIndexChanged += cbOrigin_SelectedIndexChanged;
            cbDestination.SelectedIndexChanged += cbDestination_SelectedIndexChanged;
            cbNumberofPassengers.SelectedIndexChanged += cbNumberofPassengers_SelectedIndexChanged;
            chkbTravelInsurance.CheckedChanged += chkbTravelInsurance_CheckedChanged;

            // Set pricing
            lblClassFare.Text = "0.00";
            lblTravelTax.Text = "0.00";
            lblTransactionFee.Text = "0.00";
            lblTotalFare.Text = "0.00";

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
            try
            {
                // 1. Retrieve and validate Passenger Name
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

                // **Pricing Calculation**
                float totalClassFare = 0.0f, totalTravelTax = 0.0f, transactionFee = 0.0f, insuranceFee = 0.0f;
                string flightCode = ""; // To store the fetched flight code

                try
                {
                    using (var connection = dbConnection.GetConnection())
                    {
                        connection.Open();

                        // Query to fetch fare from FlightInformation table
                        string fareQuery = "SELECT FlightCode, PrivateFare, BusinessFare, RegularFare FROM FlightInformation " +
                                           "WHERE Origin = @Origin AND Destination = @Destination";
                        MySqlCommand fareCommand = new MySqlCommand(fareQuery, connection);
                        fareCommand.Parameters.AddWithValue("@Origin", origin);
                        fareCommand.Parameters.AddWithValue("@Destination", destination);

                        using (var reader = fareCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                flightCode = reader.GetString("FlightCode"); // Fetch the flight code

                                float farePerPerson = classType switch
                                {
                                    "Private" => reader.GetFloat("PrivateFare"),
                                    "Business" => reader.GetFloat("BusinessFare"),
                                    "Regular" => reader.GetFloat("RegularFare"),
                                    _ => 0.0f
                                };
                                totalClassFare = farePerPerson * numberOfPassengers;
                            }
                            else
                            {
                                ShowError(5, "No flight data found for the selected route.");
                                return;
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

                // Generate unique Transaction Number (e.g., TXN-20240225-123456)
                string transactionNumber = "TXN-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");

                // Insert booking into database
                try
                {
                    using (var connection = dbConnection.GetConnection())
                    {
                        connection.Open();

                        // Generate a unique Control Number and store it for later use
                        this.GeneratedControlNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                        // Insert into BookingInformation table
                        string insertBookingQuery = "INSERT INTO BookingInformation (ControlNumber, TransactionNumber, PassengerName, PassengerAge, " +
                                                    "NumberOfPassengers, TravelType, Origin, Destination, ClassType, BookingDate, FlightCode, AccountID) " +
                                                    "VALUES (@ControlNumber, @TransactionNumber, @PassengerName, @PassengerAge, @NumberOfPassengers, " +
                                                    "@TravelType, @Origin, @Destination, @ClassType, NOW(), @FlightCode, @AccountID)";
                        MySqlCommand bookingCommand = new MySqlCommand(insertBookingQuery, connection);
                        bookingCommand.Parameters.AddWithValue("@ControlNumber", GeneratedControlNumber);
                        bookingCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                        bookingCommand.Parameters.AddWithValue("@PassengerName", fullName);
                        bookingCommand.Parameters.AddWithValue("@PassengerAge", age);
                        bookingCommand.Parameters.AddWithValue("@NumberOfPassengers", numberOfPassengers);
                        bookingCommand.Parameters.AddWithValue("@TravelType", travelType);
                        bookingCommand.Parameters.AddWithValue("@Origin", origin);
                        bookingCommand.Parameters.AddWithValue("@Destination", destination);
                        bookingCommand.Parameters.AddWithValue("@ClassType", classType);
                        bookingCommand.Parameters.AddWithValue("@FlightCode", flightCode);
                        bookingCommand.Parameters.AddWithValue("@AccountID", this.AccountID);

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

            // Hide booking panel and show confirmation panel
            panel1.Visible = false;
            panel2.Visible = true;

            // Load details into confirmation panel
            LoadBookingDetails();
        }



        private void LoadBookingDetails()
        {
            try
            {
                // Assign values directly from user selections and labels
                lblConfirm_PassengerName.Text = txtName.Text.Trim();
                lblConfirm_PassengerAge.Text = txtAge.Text.Trim();
                lblConfirm_TravelType.Text = grpTravelType.Controls.OfType<RadioButton>()
                                             .FirstOrDefault(r => r.Checked)?.Text ?? "N/A";
                lblConfirm_Origin.Text = cbOrigin.SelectedItem?.ToString() ?? "N/A";
                lblConfirm_Destination.Text = cbDestination.SelectedItem?.ToString() ?? "N/A";
                lblConfirm_ClassType.Text = grpClassType.Controls.OfType<RadioButton>()
                                             .FirstOrDefault(r => r.Checked)?.Text ?? "N/A";

                // Set the current date
                lblConfirm_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Retrieve already calculated fare values
                lblConfirm_ClassFare.Text = lblClassFare.Text;
                lblConfirm_TransactionFee.Text = lblTransactionFee.Text;
                lblConfirm_TravelTax.Text = lblTravelTax.Text;
                lblConfirm_TotalCost.Text = lblTotalFare.Text; // Since lblTotalFare holds the final cost
            }
            catch (Exception ex)
            {
                ShowError(7, $"Error loading booking details: {ex.Message}");
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

        // Confirmation Panel to Ticket.cs
        private void lblConfirm_ConfirmBooking_Click(object sender, EventArgs e)
        {
            // Ensure Control Number is available
            if (string.IsNullOrEmpty(this.GeneratedControlNumber))
            {
                ShowError(7, "Control number is missing.");
                return;
            }

            // Retrieve other booking details from UI
            string flightCode = lblConfirm_Origin.Text + "-" + lblConfirm_Destination.Text;
            string classType = lblConfirm_ClassType.Text;
            string origin = lblConfirm_Origin.Text;
            string destination = lblConfirm_Destination.Text;
            string passengerName = FormatPassengerName(lblConfirm_PassengerName.Text); // Format name correctly
            string passengerAge = lblConfirm_PassengerAge.Text;
            string numberOfPassengers = cbNumberofPassengers.SelectedItem?.ToString() ?? "1";
            string travelInsurance = chkbTravelInsurance.Checked ? "YES" : "NO";
            string travelType = lblConfirm_TravelType.Text;

            // Initialize boarding and arrival details
            string boardingDate = "", boardingTime = "";
            string arrivalDate = "", arrivalTime = "";

            // Retrieve flight schedule from database
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT BoardingDate, BoardingTime, ArrivalDate, ArrivalTime, FlightCode FROM FlightInformation " +
                                   "WHERE Origin = @Origin AND Destination = @Destination";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Origin", origin);
                    command.Parameters.AddWithValue("@Destination", destination);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            boardingDate = DateTime.Parse(reader["BoardingDate"].ToString()).ToString("yyyy-MM-dd");
                            boardingTime = DateTime.Parse(reader["BoardingTime"].ToString()).ToString("HH:mm:ss");
                            arrivalDate = DateTime.Parse(reader["ArrivalDate"].ToString()).ToString("yyyy-MM-dd");
                            arrivalTime = DateTime.Parse(reader["ArrivalTime"].ToString()).ToString("HH:mm:ss");
                            flightCode = reader["FlightCode"].ToString(); // Ensure correct FlightCode format
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
                return;
            }

            // Get the current booking date and time
            string bookingDate = DateTime.Now.ToString("yyyy-MM-dd");
            string bookingTime = DateTime.Now.ToString("HH:mm:ss");

            // Open the Ticket form and pass the details
            Ticket ticket = new Ticket(flightCode, classType, origin, destination,
                passengerName, passengerAge, numberOfPassengers, travelInsurance, travelType,
                boardingDate, boardingTime, arrivalDate, arrivalTime, bookingDate, bookingTime, this.GeneratedControlNumber);

            ticket.Show();
            this.Hide();
        }


        private string FormatPassengerName(string fullName)
        {
            string[] nameParts = fullName.Split(' '); // Split name by spaces

            if (nameParts.Length < 2)
            {
                return fullName; // Return as-is if only one name exists
            }

            string surname = nameParts[nameParts.Length - 1]; // Last part is the surname
            string firstName = nameParts[0]; // First part is the first name
            string middleInitial = nameParts.Length > 2 ? nameParts[1][0].ToString().ToUpper() : ""; // Get middle initial (if exists)

            return $"{surname},\n{firstName}\n{middleInitial}"; // Display on separate lines
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
            LoadFlightSchedules();
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

            LoadActiveBookings();
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

            LoadTransactions();
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
            LoadFlightSchedules();
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

            // Get current selected values
            string origin = cbOrigin.SelectedItem?.ToString();
            string destination = cbDestination.SelectedItem?.ToString();
            string classType = grpClassType.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(r => r.Checked)?.Text;
            bool hasInsurance = chkbTravelInsurance.Checked; // Check if insurance is selected

            // Validate selections
            if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(classType))
            {
                ShowError(4, "Please ensure Origin, Destination, and Class Type are selected.");
                return;
            }

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

                            float travelTaxPerPerson = GetTravelTaxFromDB(classType);
                            float transactionFee = GetTransactionFeeFromDB(classType);
                            float insuranceFeePerPerson = hasInsurance ? GetInsuranceFeeFromDB(classType) : 0.0f;

                            // Calculate totals
                            float totalClassFare = farePerPerson * numberOfPassengers;
                            float totalTravelTax = travelTaxPerPerson * numberOfPassengers;
                            float totalInsuranceFee = insuranceFeePerPerson * numberOfPassengers;
                            float totalFare = totalClassFare + totalTravelTax + transactionFee + totalInsuranceFee;

                            // Update UI Labels
                            lblClassFare.Text = totalClassFare.ToString("0.00");
                            lblTravelTax.Text = totalTravelTax.ToString("0.00");
                            lblTransactionFee.Text = transactionFee.ToString("0.00");
                            //lblInsuranceFee.Text = totalInsuranceFee.ToString("0.00"); // New
                            //lblTotalFare.Text = totalFare.ToString("0.00"); // New
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
            UpdateTotalFare();
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
            bool hasInsurance = chkbTravelInsurance.Checked; // Check if insurance is selected

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
                            float insuranceFeePerPerson = hasInsurance ? GetInsuranceFeeFromDB(classType) : 0.0f;

                            // Compute total fares
                            float totalClassFare = farePerPerson * numberOfPassengers;
                            float totalTravelTax = travelTaxPerPerson * numberOfPassengers;
                            float totalInsuranceFee = insuranceFeePerPerson * numberOfPassengers;
                            float totalFare = totalClassFare + totalTravelTax + transactionFee + totalInsuranceFee;

                            // Apply round-trip adjustment
                            if (travelType == "Round-trip")
                            {
                                totalClassFare *= 2;
                                totalFare *= 2; // Round-trip affects all fare components
                            }

                            // Update labels
                            lblClassFare.Text = totalClassFare.ToString("0.00");
                            lblTravelTax.Text = totalTravelTax.ToString("0.00");
                            lblTransactionFee.Text = transactionFee.ToString("0.00");
                            //lblInsuranceFee.Text = totalInsuranceFee.ToString("0.00"); // New
                            //lblTotalFare.Text = totalFare.ToString("0.00"); // New
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
            UpdateTotalFare();
        }


        private int GetSelectedNumberOfPassengers()
        {
            if (int.TryParse(cbNumberofPassengers.SelectedItem?.ToString(), out int numberOfPassengers))
            {
                return numberOfPassengers;
            }
            return 1; // Default to 1 if not selected
        }

        private void UpdateTotalFare(float insuranceFee = 0.0f)
        {
            try
            {
                // Parse values from labels (default to 0 if empty or invalid)
                float classFare = float.TryParse(lblClassFare.Text, out float cFare) ? cFare : 0.0f;
                float transactionFee = float.TryParse(lblTransactionFee.Text, out float tFee) ? tFee : 0.0f;
                float travelTax = float.TryParse(lblTravelTax.Text, out float tTax) ? tTax : 0.0f;

                // Compute total fare, adding the dynamically calculated insurance fee
                float totalFare = classFare + transactionFee + travelTax + insuranceFee;

                // Update lblTotalFare
                lblTotalFare.Text = totalFare.ToString("0.00");
            }
            catch (Exception ex)
            {
                ShowError(7, $"Error calculating total fare: {ex.Message}");
            }
        }




        private void chkbTravelInsurance_CheckedChanged(object sender, EventArgs e)
        {
            // Get the selected class type
            string classType = grpClassType.Controls.OfType<RadioButton>()
                                   .FirstOrDefault(r => r.Checked)?.Text;
            if (string.IsNullOrEmpty(classType)) return;

            // Get the selected number of passengers
            int numberOfPassengers = previousNumberOfPassengers > 0 ? previousNumberOfPassengers : 1;

            // Fetch insurance fee from the database (returns 0 if unchecked)
            float insuranceFeePerPerson = chkbTravelInsurance.Checked ? GetInsuranceFeeFromDB(classType) : 0.0f;

            // Compute total insurance fee
            float totalInsuranceFee = insuranceFeePerPerson * numberOfPassengers;

            // Update total fare dynamically
            UpdateTotalFare(totalInsuranceFee);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void lblConfirm_CancelBooking_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a booking exists before attempting deletion
                if (string.IsNullOrEmpty(this.GeneratedControlNumber))
                {
                    ShowError(7, "No booking found to cancel.");
                    return;
                }

                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    // Delete the booking from the database using ControlNumber
                    string deleteQuery = "DELETE FROM BookingInformation WHERE ControlNumber = @ControlNumber";
                    MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@ControlNumber", this.GeneratedControlNumber);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Booking has been canceled successfully.", "Cancel Booking", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ShowError(7, "No matching booking found to cancel.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
                return;
            }

            // Clear all input fields
            txtName.Text = "";
            txtAge.Text = "";
            cbOrigin.SelectedIndex = -1;
            cbDestination.SelectedIndex = -1;
            cbNumberofPassengers.SelectedIndex = -1;
            foreach (var rb in grpTravelType.Controls.OfType<RadioButton>()) rb.Checked = false;
            foreach (var rb in grpClassType.Controls.OfType<RadioButton>()) rb.Checked = false;
            chkbTravelInsurance.Checked = false;
            lblClassFare.Text = "0.00";
            lblTransactionFee.Text = "0.00";
            lblTravelTax.Text = "0.00";
            lblTotalFare.Text = "0.00";

            // Navigate back to Landing Page
            LandingPage landingpage = new LandingPage(AccountID, Username, Email, PhoneNumber, FullName);
            landingpage.Show();
            this.Hide();
        }


        private void btnEditBooking_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure that a valid booking exists before attempting deletion
                if (string.IsNullOrEmpty(this.GeneratedControlNumber))
                {
                    ShowError(7, "No booking found to edit.");
                    return;
                }

                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    // Delete the booking from the database using ControlNumber
                    string deleteQuery = "DELETE FROM BookingInformation WHERE ControlNumber = @ControlNumber";
                    MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@ControlNumber", this.GeneratedControlNumber);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        //MessageBox.Show("Booking has been removed. You can now edit your details.", "Edit Booking", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ShowError(7, "No matching booking found to edit.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
                return;
            }

            // Switch back to the input panel while keeping the fields unchanged
            panel2.Visible = false; // Hide confirmation panel
            panel1.Visible = true;  // Show input panel
        }

        private void LoadTransactions()
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"SELECT 
                                TransactionNumber AS 'Transaction ID',
                                PassengerName AS 'Passenger Name',
                                Origin AS 'Origin',
                                Destination AS 'Destination',
                                ClassType AS 'Class Type',
                                BookingDate AS 'Booking Date'
                            FROM BookingInformation 
                            WHERE AccountID = @AccountID";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AccountID", this.AccountID);

                    using (var reader = command.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Debugging: Check if rows exist
                        Console.WriteLine($"Rows Retrieved: {dt.Rows.Count}");

                        // Bind to DataGridView
                        tblViewTransactions.DataSource = dt;

                        // Fix column headers for user-friendly display
                        tblViewTransactions.Columns["Transaction ID"].HeaderText = "Transaction Number";
                        tblViewTransactions.Columns["Passenger Name"].HeaderText = "Passenger Name";
                        tblViewTransactions.Columns["Origin"].HeaderText = "Origin";
                        tblViewTransactions.Columns["Destination"].HeaderText = "Destination";
                        tblViewTransactions.Columns["Class Type"].HeaderText = "Class Type";
                        tblViewTransactions.Columns["Booking Date"].HeaderText = "Booking Date";

                        // Adjust column width to fit content
                        tblViewTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        tblViewTransactions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        tblViewTransactions.RowHeadersVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error while fetching transactions: {ex.Message}");
            }
        }


        private void LoadActiveBookings()
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT 
                        b.ControlNumber AS 'Transaction Number',
                        f.FlightNumber AS 'Flight Number',
                        f.Origin AS 'Origin',
                        f.Destination AS 'Destination',
                        f.BoardingDate AS 'Departure Date',
                        f.BoardingTime AS 'Departure Time',
                        f.ArrivalDate AS 'Arrival Date',
                        f.ArrivalTime AS 'Arrival Time'
                    FROM BookingInformation b
                    JOIN FlightInformation f ON b.FlightCode = f.FlightCode
                    WHERE b.AccountID = @AccountID
                    AND f.BoardingDate >= CURDATE()";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AccountID", this.AccountID);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    tblViewBookings.DataSource = dataTable;

                    // Ensure column names are user-friendly
                    tblViewBookings.Columns["Transaction Number"].HeaderText = "Transaction Number";
                    tblViewBookings.Columns["Flight Number"].HeaderText = "Flight Number";
                    tblViewBookings.Columns["Origin"].HeaderText = "Origin";
                    tblViewBookings.Columns["Destination"].HeaderText = "Destination";
                    tblViewBookings.Columns["Departure Date"].HeaderText = "Departure Date";
                    tblViewBookings.Columns["Departure Time"].HeaderText = "Departure Time";
                    tblViewBookings.Columns["Arrival Date"].HeaderText = "Arrival Date";
                    tblViewBookings.Columns["Arrival Time"].HeaderText = "Arrival Time";

                    // Resize columns to fit the DataGridView properly
                    tblViewBookings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    tblViewBookings.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    tblViewBookings.RowHeadersVisible = false;
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error: {ex.Message}");
            }
        }



        private void LoadFlightSchedules()
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"
        SELECT 
            f.FlightNumber AS 'Flight Number',
            f.Origin AS 'Origin',
            f.Destination AS 'Destination',
            f.ClassType AS 'Class Type',
            f.BoardingDate AS 'Departure Date',
            f.ArrivalDate AS 'Arrival Date',
            f.BoardingTime AS 'Departure Time',
            f.ArrivalTime AS 'Arrival Time',
            CASE 
                WHEN (a.MaxCapacity - COALESCE((SELECT SUM(b.NumberOfPassengers) 
                                               FROM BookingInformation b 
                                               WHERE b.FlightCode = f.FlightCode 
                                               AND b.ClassType = f.ClassType), 0)) > 0 
                THEN 'Available' ELSE 'Unavailable' 
            END AS 'Class Availability'
        FROM FlightInformation f
        JOIN AirplaneInformation a ON f.ClassType = a.Type
        GROUP BY f.FlightNumber, f.Origin, f.Destination, f.ClassType, 
                 f.BoardingDate, f.ArrivalDate, f.BoardingTime, f.ArrivalTime, a.MaxCapacity";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    tblViewFlightSchedules.DataSource = dataTable;

                    // ✅ Set proper column headers for UI clarity
                    tblViewFlightSchedules.Columns["Flight Number"].HeaderText = "Flight No.";
                    tblViewFlightSchedules.Columns["Origin"].HeaderText = "Origin";
                    tblViewFlightSchedules.Columns["Destination"].HeaderText = "Destination";
                    tblViewFlightSchedules.Columns["Class Type"].HeaderText = "Class";
                    tblViewFlightSchedules.Columns["Departure Date"].HeaderText = "Departure Date";
                    tblViewFlightSchedules.Columns["Arrival Date"].HeaderText = "Arrival Date";
                    tblViewFlightSchedules.Columns["Departure Time"].HeaderText = "Departure Time";
                    tblViewFlightSchedules.Columns["Arrival Time"].HeaderText = "Arrival Time";
                    tblViewFlightSchedules.Columns["Class Availability"].HeaderText = "Availability";

                    // ✅ Format colors dynamically with null handling
                    foreach (DataGridViewRow row in tblViewFlightSchedules.Rows)
                    {
                        if (row.Cells["Class Availability"].Value != null && !string.IsNullOrEmpty(row.Cells["Class Availability"].Value.ToString()))
                        {
                            string availability = row.Cells["Class Availability"].Value.ToString(); // ✅ Fix: Use correct column alias
                            if (availability == "Available")
                            {
                                row.Cells["Class Availability"].Style.ForeColor = Color.Green;
                                row.Cells["Class Availability"].Style.Font = new Font(tblViewFlightSchedules.Font, FontStyle.Bold);
                            }
                            else
                            {
                                row.Cells["Class Availability"].Style.ForeColor = Color.Red;
                                row.Cells["Class Availability"].Style.Font = new Font(tblViewFlightSchedules.Font, FontStyle.Bold);
                            }
                        }
                    }


                    // ✅ Resize columns for better fit
                    tblViewFlightSchedules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    tblViewFlightSchedules.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    tblViewFlightSchedules.RowHeadersVisible = false;
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error while fetching flight schedules: {ex.Message}");
            }
        }




        private void ColorCodeClassAvailability()
        {
            foreach (DataGridViewRow row in tblViewFlightSchedules.Rows)
            {
                if (row.Cells["Class Availability"].Value != null)
                {
                    string availability = row.Cells["Class Availability"].Value.ToString();

                    if (availability == "Available")
                    {
                        row.Cells["Class Availability"].Style.ForeColor = Color.Green; // ✅ Green
                        row.Cells["Class Availability"].Style.Font = new Font(tblViewFlightSchedules.Font, FontStyle.Bold);
                    }
                    else if (availability == "Unavailable")
                    {
                        row.Cells["Class Availability"].Style.ForeColor = Color.Red; // ❌ Red
                        row.Cells["Class Availability"].Style.Font = new Font(tblViewFlightSchedules.Font, FontStyle.Bold);
                    }
                }
            }
        }

        private void tblViewFlightSchedules_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Ensure we're formatting the correct column
            if (tblViewFlightSchedules.Columns[e.ColumnIndex].Name == "Class Availability")
            {
                if (e.Value != null)
                {
                    string availability = e.Value.ToString();
                    if (availability == "Available")
                    {
                        e.CellStyle.ForeColor = Color.Green;  // ✅ Green for Available
                        e.CellStyle.Font = new Font(tblViewFlightSchedules.Font, FontStyle.Bold);
                    }
                    else if (availability == "Unavailable")
                    {
                        e.CellStyle.ForeColor = Color.Red;  // ❌ Red for Unavailable
                        e.CellStyle.Font = new Font(tblViewFlightSchedules.Font, FontStyle.Bold);
                    }
                }
            }
        }


        private void tblViewFlightSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure user clicked a valid row
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Check if the clicked column is "Class Availability" (User clicks Available/Unavailable)
                if (tblViewFlightSchedules.Columns[e.ColumnIndex].Name == "Class Availability")
                {
                    string flightNumber = tblViewFlightSchedules.Rows[e.RowIndex].Cells["Flight Number"].Value.ToString();
                    string origin = tblViewFlightSchedules.Rows[e.RowIndex].Cells["Origin"].Value.ToString();
                    string destination = tblViewFlightSchedules.Rows[e.RowIndex].Cells["Destination"].Value.ToString();

                    // Call function to load details into Detailed View Panel
                    LoadFlightDetails(flightNumber, origin, destination);

                    // Hide other panels and show the detailed view panel
                    panel1.Visible = false;
                    panel2.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = false;
                    panel5.Visible = false;
                    panel6.Visible = false;
                    panel7.Visible = false;
                    panel8.Visible = false;
                    panel9_DetailedFlightView.Visible = true;
                }
            }
        }



        private void ShowDetailedFlightViewPanel()
        {
            // Hide all panels
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = true;

            // Update navigation button images
            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Inactive.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Active.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");
        }

        private void LoadFlightDetails(string flightNumber, string origin, string destination)
        {
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
        SELECT 
            f.FlightNumber, f.BoardingTime, f.ArrivalTime, 
            -- Fetch the actual capacity minus crew count
            (SELECT MaxCapacity FROM AirplaneInformation WHERE Type = 'Private') AS PrivateCapacity,
            (SELECT COUNT(*) FROM BookingInformation WHERE FlightCode = f.FlightCode AND ClassType = 'Private') + 
            (SELECT CrewCount FROM AirplaneInformation WHERE Type = 'Private') AS PrivateBooked,
            
            (SELECT MaxCapacity FROM AirplaneInformation WHERE Type = 'Business') AS BusinessCapacity,
            (SELECT COUNT(*) FROM BookingInformation WHERE FlightCode = f.FlightCode AND ClassType = 'Business') + 
            (SELECT CrewCount FROM AirplaneInformation WHERE Type = 'Business') AS BusinessBooked,
            
            (SELECT MaxCapacity FROM AirplaneInformation WHERE Type = 'Regular') AS RegularCapacity,
            (SELECT COUNT(*) FROM BookingInformation WHERE FlightCode = f.FlightCode AND ClassType = 'Regular') + 
            (SELECT CrewCount FROM AirplaneInformation WHERE Type = 'Regular') AS RegularBooked,

            -- Fetch class-specific baggage fees
            (SELECT BaggageFee FROM FlightInformation WHERE FlightNumber = f.FlightNumber AND ClassType = 'Private') AS PrivateBaggageFee,
            (SELECT BaggageFee FROM FlightInformation WHERE FlightNumber = f.FlightNumber AND ClassType = 'Business') AS BusinessBaggageFee,
            (SELECT BaggageFee FROM FlightInformation WHERE FlightNumber = f.FlightNumber AND ClassType = 'Regular') AS RegularBaggageFee,

            -- Fetch Insurance fees
            (SELECT InsuranceFeePerPerson FROM InsuranceInformation WHERE ClassType = 'Private') AS PrivateInsurance,
            (SELECT InsuranceFeePerPerson FROM InsuranceInformation WHERE ClassType = 'Business') AS BusinessInsurance,
            (SELECT InsuranceFeePerPerson FROM InsuranceInformation WHERE ClassType = 'Regular') AS RegularInsurance,

            -- Fetch Travel Taxes
            (SELECT TravelTaxPerPerson FROM TravelTaxInformation WHERE ClassType = 'Private') AS PrivateTax,
            (SELECT TravelTaxPerPerson FROM TravelTaxInformation WHERE ClassType = 'Business') AS BusinessTax,
            (SELECT TravelTaxPerPerson FROM TravelTaxInformation WHERE ClassType = 'Regular') AS RegularTax

        FROM FlightInformation f
        WHERE f.FlightNumber = @FlightNumber AND f.Origin = @Origin AND f.Destination = @Destination";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FlightNumber", flightNumber);
                    command.Parameters.AddWithValue("@Origin", origin);
                    command.Parameters.AddWithValue("@Destination", destination);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 🟢 Update Flight Number, Times
                            lvlViewDetails_FlightNumber.Text = reader["FlightNumber"].ToString();
                            lvlViewDetails_DepartureTime.Text = reader["BoardingTime"].ToString();
                            lvlViewDetails_ArrivalTime.Text = reader["ArrivalTime"].ToString();

                            // 🟢 Get Capacity & Booked Counts (Including Crew)
                            int privateCapacity = Convert.ToInt32(reader["PrivateCapacity"]);
                            int privateBooked = Convert.ToInt32(reader["PrivateBooked"]);
                            int businessCapacity = Convert.ToInt32(reader["BusinessCapacity"]);
                            int businessBooked = Convert.ToInt32(reader["BusinessBooked"]);
                            int regularCapacity = Convert.ToInt32(reader["RegularCapacity"]);
                            int regularBooked = Convert.ToInt32(reader["RegularBooked"]);

                            lvlViewDetails_PrivateCapacity.Text = $"{privateBooked}/{privateCapacity}";
                            lvlViewDetails_BusinessCapacity.Text = $"{businessBooked}/{businessCapacity}";
                            lvlViewDetails_RegularCapacity.Text = $"{regularBooked}/{regularCapacity}";

                            // 🟢 Update Availability Images
                            string availableImagePath = "C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\lblAvailable.png";
                            string unavailableImagePath = "C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\lblUnavailable.png";

                            imgViewDetails_PrivateAvailability.Image = (privateBooked < privateCapacity)
                                ? Image.FromFile(availableImagePath)
                                : Image.FromFile(unavailableImagePath);
                            imgViewDetails_Businessvailability.Image = (businessBooked < businessCapacity)
                                ? Image.FromFile(availableImagePath)
                                : Image.FromFile(unavailableImagePath);
                            imgViewDetails_RegularAvailability.Image = (regularBooked < regularCapacity)
                                ? Image.FromFile(availableImagePath)
                                : Image.FromFile(unavailableImagePath);

                            // 🟢 Update Fees (Fetched from DB)
                            lblViewDetails_PrivateBaggageFee.Text = reader["PrivateBaggageFee"].ToString();
                            lblViewDetails_BusinessBaggageFee.Text = reader["BusinessBaggageFee"].ToString();
                            lblViewDetails_RegularBaggageFee.Text = reader["RegularBaggageFee"].ToString();

                            lblViewDetails_PrivateInsurance.Text = reader["PrivateInsurance"].ToString();
                            lblViewDetails_BusinessInsurance.Text = reader["BusinessInsurance"].ToString();
                            lblViewDetails_RegularInsurance.Text = reader["RegularInsurance"].ToString();

                            lblViewDetails_PrivateTax.Text = reader["PrivateTax"].ToString();
                            lblViewDetails_BusinessTax.Text = reader["BusinessTax"].ToString();
                            lblViewDetails_RegularTax.Text = reader["RegularTax"].ToString();
                        }
                        else
                        {
                            ShowError(5, "Flight details not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error while fetching flight details: {ex.Message}");
            }
        }






        private int GetBookedCount(string flightNumber)
        {
            int bookedCount = 0;
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT COUNT(*) FROM BookingInformation WHERE FlightCode = 
                            (SELECT FlightCode FROM FlightInformation WHERE FlightNumber = @FlightNumber)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FlightNumber", flightNumber);

                    bookedCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error fetching booked count: {ex.Message}");
            }

            return bookedCount;
        }

        private void UpdateClassAvailability(string classType, int maxCapacity, int bookedCount)
        {
            bool isAvailable = bookedCount < maxCapacity;
            string availableImagePath = @"C:\Users\MSI\source\repos\MERC\MERC\assets\lblAvailable.png";
            string unavailableImagePath = @"C:\Users\MSI\source\repos\MERC\MERC\assets\lblUnavailable.png";

            switch (classType)
            {
                case "Private":
                    imgViewDetails_PrivateAvailability.Image = Image.FromFile(isAvailable ? availableImagePath : unavailableImagePath);
                    break;
                case "Business":
                    imgViewDetails_Businessvailability.Image = Image.FromFile(isAvailable ? availableImagePath : unavailableImagePath);
                    break;
                case "Regular":
                    imgViewDetails_RegularAvailability.Image = Image.FromFile(isAvailable ? availableImagePath : unavailableImagePath);
                    break;
            }
        }



        private void UpdateBaggageFee(string classType, float baggageFee)
        {
            switch (classType)
            {
                case "Private":
                    lblViewDetails_PrivateBaggageFee.Text = baggageFee.ToString("0.00");
                    break;
                case "Business":
                    lblViewDetails_BusinessBaggageFee.Text = baggageFee.ToString("0.00");
                    break;
                case "Regular":
                    lblViewDetails_RegularBaggageFee.Text = baggageFee.ToString("0.00");
                    break;
            }
        }

        private void UpdateInsuranceFee(string classType)
        {
            float insuranceFee = classType switch
            {
                "Private" => 4500.00f,
                "Business" => 6500.00f,
                "Regular" => 950.00f,
                _ => 0.00f
            };

            switch (classType)
            {
                case "Private":
                    lblViewDetails_PrivateInsurance.Text = insuranceFee.ToString("0.00");
                    break;
                case "Business":
                    lblViewDetails_BusinessInsurance.Text = insuranceFee.ToString("0.00");
                    break;
                case "Regular":
                    lblViewDetails_RegularInsurance.Text = insuranceFee.ToString("0.00");
                    break;
            }
        }

        private void UpdateTravelTax(string classType)
        {
            float travelTax = classType switch
            {
                "Private" => 4260.00f,
                "Business" => 5700.00f,
                "Regular" => 2500.00f,
                _ => 0.00f
            };

            switch (classType)
            {
                case "Private":
                    lblViewDetails_PrivateTax.Text = travelTax.ToString("0.00");
                    break;
                case "Business":
                    lblViewDetails_BusinessTax.Text = travelTax.ToString("0.00");
                    break;
                case "Regular":
                    lblViewDetails_RegularTax.Text = travelTax.ToString("0.00");
                    break;
            }
        }

        private void BookFlight(string classType)
        {
            string origin = "";
            string destination = "";

            // Fetch Origin & Destination from DB based on the Flight Number
            try
            {
                using (var connection = dbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT Origin, Destination FROM FlightInformation WHERE FlightNumber = @FlightNumber";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FlightNumber", lvlViewDetails_FlightNumber.Text);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            origin = reader["Origin"].ToString();
                            destination = reader["Destination"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, $"Database error fetching flight details: {ex.Message}");
                return;
            }

            // Switch to Booking Panel
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true; // Booking Panel
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9_DetailedFlightView.Visible = false;

            // Update Navigation Icons
            navbtnBbook.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnBook_Active.png");
            navbtnAboutUs.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnAboutUs_Inactive.png");
            navbtnHomepage.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnHomepage_Inactive.png");
            navbtnFlighSchedule.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnFlightSchedule_Inactive.png");
            navbtnTransactions.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnTransactions_Inactive.png");
            navbtnViewBooking.Image = Image.FromFile("C:\\Users\\MSI\\source\\repos\\MERC\\MERC\\assets\\navbtnViewBooking_Inactive.png");

            // Load Origin & Destination into the booking panel
            cbOrigin.SelectedItem = origin;
            cbDestination.SelectedItem = destination;

            // Select the correct Class Type radio button
            foreach (RadioButton rb in grpClassType.Controls.OfType<RadioButton>())
            {
                if (rb.Text.Equals(classType, StringComparison.OrdinalIgnoreCase))
                {
                    rb.Checked = true;
                    break;
                }
            }
        }

        // **Private Class Booking**
        private void btnViewDetails_BookPrivate_Click(object sender, EventArgs e)
        {
            BookFlight("Private");
        }

        // **Business Class Booking**
        private void btnViewDetails_BookBusiness_Click(object sender, EventArgs e)
        {
            BookFlight("Business");
        }

        // **Regular Class Booking**
        private void btnViewDetails_BookRegular_Click(object sender, EventArgs e)
        {
            BookFlight("Regular");
        }


    } // END OF NAME SPACE
}
