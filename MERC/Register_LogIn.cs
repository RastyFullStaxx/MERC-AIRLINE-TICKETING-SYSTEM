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
        public Register_LogIn()
        {
            InitializeComponent();
        }

        private void Register_LogIn_Load(object sender, EventArgs e)
        {

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

        }

        private void Register_LogIn_Load_1(object sender, EventArgs e)
        {

        }
    }
}
