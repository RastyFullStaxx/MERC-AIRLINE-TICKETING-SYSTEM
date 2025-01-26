namespace MERC
{
    partial class LandingPage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LandingPage));
            btnBook = new Button();
            btnFlightSchedule = new Button();
            btnHomePage = new Button();
            btnTransactionHistory = new Button();
            btnViewPage = new Button();
            SuspendLayout();
            // 
            // btnBook
            // 
            btnBook.BackColor = Color.Transparent;
            btnBook.Cursor = Cursors.Hand;
            btnBook.FlatAppearance.BorderSize = 0;
            btnBook.FlatAppearance.MouseDownBackColor = Color.White;
            btnBook.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnBook.FlatStyle = FlatStyle.Flat;
            btnBook.ForeColor = Color.Transparent;
            btnBook.Image = (Image)resources.GetObject("btnBook.Image");
            btnBook.Location = new Point(561, 589);
            btnBook.Name = "btnBook";
            btnBook.Size = new Size(317, 61);
            btnBook.TabIndex = 0;
            btnBook.UseVisualStyleBackColor = false;
            btnBook.Click += btnBook_Click;
            // 
            // btnFlightSchedule
            // 
            btnFlightSchedule.BackColor = Color.Transparent;
            btnFlightSchedule.Cursor = Cursors.Hand;
            btnFlightSchedule.FlatAppearance.BorderSize = 0;
            btnFlightSchedule.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnFlightSchedule.FlatStyle = FlatStyle.Flat;
            btnFlightSchedule.Image = (Image)resources.GetObject("btnFlightSchedule.Image");
            btnFlightSchedule.Location = new Point(436, 937);
            btnFlightSchedule.Name = "btnFlightSchedule";
            btnFlightSchedule.Size = new Size(204, 30);
            btnFlightSchedule.TabIndex = 1;
            btnFlightSchedule.UseVisualStyleBackColor = false;
            btnFlightSchedule.Click += btnFlightSchedule_Click;
            // 
            // btnHomePage
            // 
            btnHomePage.BackColor = Color.Transparent;
            btnHomePage.Cursor = Cursors.Hand;
            btnHomePage.FlatAppearance.BorderSize = 0;
            btnHomePage.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnHomePage.FlatStyle = FlatStyle.Flat;
            btnHomePage.Image = (Image)resources.GetObject("btnHomePage.Image");
            btnHomePage.Location = new Point(1209, 937);
            btnHomePage.Name = "btnHomePage";
            btnHomePage.Size = new Size(132, 30);
            btnHomePage.TabIndex = 2;
            btnHomePage.UseVisualStyleBackColor = false;
            btnHomePage.Click += btnHomePage_Click;
            // 
            // btnTransactionHistory
            // 
            btnTransactionHistory.BackColor = Color.Transparent;
            btnTransactionHistory.Cursor = Cursors.Hand;
            btnTransactionHistory.FlatAppearance.BorderSize = 0;
            btnTransactionHistory.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnTransactionHistory.FlatStyle = FlatStyle.Flat;
            btnTransactionHistory.Image = (Image)resources.GetObject("btnTransactionHistory.Image");
            btnTransactionHistory.Location = new Point(791, 937);
            btnTransactionHistory.Name = "btnTransactionHistory";
            btnTransactionHistory.Size = new Size(262, 30);
            btnTransactionHistory.TabIndex = 3;
            btnTransactionHistory.UseVisualStyleBackColor = false;
            btnTransactionHistory.Click += btnTransactionHistory_Click;
            // 
            // btnViewPage
            // 
            btnViewPage.BackColor = Color.Transparent;
            btnViewPage.Cursor = Cursors.Hand;
            btnViewPage.FlatAppearance.BorderSize = 0;
            btnViewPage.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnViewPage.FlatStyle = FlatStyle.Flat;
            btnViewPage.Image = (Image)resources.GetObject("btnViewPage.Image");
            btnViewPage.Location = new Point(96, 937);
            btnViewPage.Name = "btnViewPage";
            btnViewPage.Size = new Size(186, 30);
            btnViewPage.TabIndex = 4;
            btnViewPage.UseVisualStyleBackColor = false;
            btnViewPage.Click += btnViewPage_Click;
            // 
            // LandingPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1440, 1024);
            Controls.Add(btnViewPage);
            Controls.Add(btnTransactionHistory);
            Controls.Add(btnHomePage);
            Controls.Add(btnFlightSchedule);
            Controls.Add(btnBook);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LandingPage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += LandingPage_Load;
            ResumeLayout(false);
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            // Add logic for button click here
            //test
            
        }

        private void btnFlightSchedule_Click(object sender, EventArgs e)
        {
            // Add logic for button click here
            
        }

        private void btnHomePage_Click(object sender, EventArgs e)
        {
            // Create an instance of the Homepage form
            Homepage homepage = new Homepage();

            // Show the Homepage form
            homepage.Show();

            // Optionally hide or close the current form
            this.Hide();

        }

        private void btnTransactionHistory_Click(object sender, EventArgs e)
        {
            // Add logic for button click here

        }

        private void btnViewPage_Click(object sender, EventArgs e)
        {
            // Add logic for button click here

        }

        private void LandingPage_Load(object sender, EventArgs e)
        {
            // Add logic for button click here

        }

        #endregion

        private Button btnBook;
        private Button btnFlightSchedule;
        private Button btnHomePage;
        private Button btnTransactionHistory;
        private Button btnViewPage;
    }
}
