using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



// panel 3 = contains guidelines
// panel 1 = gets user input
// panel 2 = displays the users inputs for confirmation



namespace MERC
{
    public partial class Booking1 : Form
    {
        public Booking1()
        {
            InitializeComponent();

        }

        private void btnConfirmBooking_Click(object sender, EventArgs e)
        {
            panel1.Visible = false; // Hide panel1
            panel2.Visible = true;  // Show panel2
            imgConfirmationHeader.Visible = true;
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
    }
}
