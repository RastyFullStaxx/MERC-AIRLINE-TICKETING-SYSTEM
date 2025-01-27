using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


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
        public Homepage()
        {
            InitializeComponent();

        }

        private void btnConfirmBooking_Click(object sender, EventArgs e)
        {
            panel1.Visible = false; // Hide panel1
            panel2.Visible = true;  // Show panel2
            imgPanelNameHeader.Visible = true;
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

            Homepage homepage = new Homepage();
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

            Homepage homepage = new Homepage();
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
    }
}
