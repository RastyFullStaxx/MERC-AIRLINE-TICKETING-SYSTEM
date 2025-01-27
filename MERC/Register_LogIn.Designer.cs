namespace MERC
{
    partial class Register_LogIn : BaseForm // Update to inherit BaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register_LogIn));
            panel1 = new Panel();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            btnLogIn = new Button();
            btnSignUp = new Button();
            imgLogInPreset = new PictureBox();
            panel2 = new Panel();
            btnConfirmAccountRegister = new Button();
            btnLogInExistingAcc = new Button();
            txtRegister_PhoneNumber = new TextBox();
            txtRegister_Username = new TextBox();
            txtConfrimRegister_Password = new TextBox();
            txtRegister_Password = new TextBox();
            txtRegister_Email = new TextBox();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgLogInPreset).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(btnLogIn);
            panel1.Controls.Add(btnSignUp);
            panel1.Controls.Add(imgLogInPreset);
            panel1.Location = new Point(205, 331);
            panel1.Name = "panel1";
            panel1.Size = new Size(1153, 634);
            panel1.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(16, 267);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(423, 25);
            txtPassword.TabIndex = 21;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(16, 138);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(423, 25);
            txtUsername.TabIndex = 20;
            // 
            // btnLogIn
            // 
            btnLogIn.BackColor = Color.Transparent;
            btnLogIn.Cursor = Cursors.Hand;
            btnLogIn.FlatAppearance.BorderSize = 0;
            btnLogIn.FlatAppearance.MouseDownBackColor = Color.White;
            btnLogIn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnLogIn.FlatStyle = FlatStyle.Flat;
            btnLogIn.ForeColor = Color.Transparent;
            btnLogIn.Image = (Image)resources.GetObject("btnLogIn.Image");
            btnLogIn.Location = new Point(290, 378);
            btnLogIn.Name = "btnLogIn";
            btnLogIn.Size = new Size(163, 73);
            btnLogIn.TabIndex = 19;
            btnLogIn.UseVisualStyleBackColor = false;
            btnLogIn.Click += btnLogIn_Click;
            // 
            // btnSignUp
            // 
            btnSignUp.BackColor = Color.Transparent;
            btnSignUp.Cursor = Cursors.Hand;
            btnSignUp.FlatAppearance.BorderSize = 0;
            btnSignUp.FlatAppearance.MouseDownBackColor = Color.White;
            btnSignUp.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSignUp.FlatStyle = FlatStyle.Flat;
            btnSignUp.ForeColor = Color.Transparent;
            btnSignUp.Image = (Image)resources.GetObject("btnSignUp.Image");
            btnSignUp.Location = new Point(287, 323);
            btnSignUp.Name = "btnSignUp";
            btnSignUp.Size = new Size(166, 18);
            btnSignUp.TabIndex = 18;
            btnSignUp.UseVisualStyleBackColor = false;
            btnSignUp.Click += btnSignUp_Click;
            // 
            // imgLogInPreset
            // 
            imgLogInPreset.BackColor = Color.Transparent;
            imgLogInPreset.BackgroundImage = (Image)resources.GetObject("imgLogInPreset.BackgroundImage");
            imgLogInPreset.Location = new Point(3, 3);
            imgLogInPreset.Name = "imgLogInPreset";
            imgLogInPreset.Size = new Size(450, 307);
            imgLogInPreset.TabIndex = 1;
            imgLogInPreset.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(btnConfirmAccountRegister);
            panel2.Controls.Add(btnLogInExistingAcc);
            panel2.Controls.Add(txtRegister_PhoneNumber);
            panel2.Controls.Add(txtRegister_Username);
            panel2.Controls.Add(txtConfrimRegister_Password);
            panel2.Controls.Add(txtRegister_Password);
            panel2.Controls.Add(txtRegister_Email);
            panel2.Controls.Add(pictureBox1);
            panel2.Location = new Point(205, 331);
            panel2.Name = "panel2";
            panel2.Size = new Size(1153, 631);
            panel2.TabIndex = 1;
            panel2.Visible = false;
            // 
            // btnConfirmAccountRegister
            // 
            btnConfirmAccountRegister.BackColor = Color.Transparent;
            btnConfirmAccountRegister.Cursor = Cursors.Hand;
            btnConfirmAccountRegister.FlatAppearance.BorderSize = 0;
            btnConfirmAccountRegister.FlatAppearance.MouseDownBackColor = Color.White;
            btnConfirmAccountRegister.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnConfirmAccountRegister.FlatStyle = FlatStyle.Flat;
            btnConfirmAccountRegister.ForeColor = Color.Transparent;
            btnConfirmAccountRegister.Image = (Image)resources.GetObject("btnConfirmAccountRegister.Image");
            btnConfirmAccountRegister.Location = new Point(290, 502);
            btnConfirmAccountRegister.Name = "btnConfirmAccountRegister";
            btnConfirmAccountRegister.Size = new Size(163, 73);
            btnConfirmAccountRegister.TabIndex = 29;
            btnConfirmAccountRegister.UseVisualStyleBackColor = false;
            btnConfirmAccountRegister.Click += btnConfirmAccountRegister_Click;
            // 
            // btnLogInExistingAcc
            // 
            btnLogInExistingAcc.BackColor = Color.Transparent;
            btnLogInExistingAcc.Cursor = Cursors.Hand;
            btnLogInExistingAcc.FlatAppearance.BorderSize = 0;
            btnLogInExistingAcc.FlatAppearance.MouseDownBackColor = Color.White;
            btnLogInExistingAcc.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnLogInExistingAcc.FlatStyle = FlatStyle.Flat;
            btnLogInExistingAcc.ForeColor = Color.Transparent;
            btnLogInExistingAcc.Image = (Image)resources.GetObject("btnLogInExistingAcc.Image");
            btnLogInExistingAcc.Location = new Point(228, 447);
            btnLogInExistingAcc.Name = "btnLogInExistingAcc";
            btnLogInExistingAcc.Size = new Size(225, 18);
            btnLogInExistingAcc.TabIndex = 28;
            btnLogInExistingAcc.UseVisualStyleBackColor = false;
            btnLogInExistingAcc.Click += btnLogInExistingAcc_Click;
            // 
            // txtRegister_PhoneNumber
            // 
            txtRegister_PhoneNumber.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegister_PhoneNumber.Location = new Point(76, 386);
            txtRegister_PhoneNumber.Name = "txtRegister_PhoneNumber";
            txtRegister_PhoneNumber.Size = new Size(363, 25);
            txtRegister_PhoneNumber.TabIndex = 27;
            // 
            // txtRegister_Username
            // 
            txtRegister_Username.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegister_Username.Location = new Point(577, 119);
            txtRegister_Username.Name = "txtRegister_Username";
            txtRegister_Username.Size = new Size(423, 25);
            txtRegister_Username.TabIndex = 26;
            // 
            // txtConfrimRegister_Password
            // 
            txtConfrimRegister_Password.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtConfrimRegister_Password.Location = new Point(577, 251);
            txtConfrimRegister_Password.Name = "txtConfrimRegister_Password";
            txtConfrimRegister_Password.Size = new Size(423, 25);
            txtConfrimRegister_Password.TabIndex = 25;
            // 
            // txtRegister_Password
            // 
            txtRegister_Password.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegister_Password.Location = new Point(16, 251);
            txtRegister_Password.Name = "txtRegister_Password";
            txtRegister_Password.Size = new Size(423, 25);
            txtRegister_Password.TabIndex = 24;
            // 
            // txtRegister_Email
            // 
            txtRegister_Email.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRegister_Email.Location = new Point(16, 119);
            txtRegister_Email.Name = "txtRegister_Email";
            txtRegister_Email.Size = new Size(423, 25);
            txtRegister_Email.TabIndex = 23;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1015, 429);
            pictureBox1.TabIndex = 22;
            pictureBox1.TabStop = false;
            // 
            // Register_LogIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1440, 1024);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Register_LogIn";
            Text = "Register_LogIn";
            Load += Register_LogIn_Load_1;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgLogInPreset).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox imgLogInPreset;
        private Button btnSignUp;
        private Button btnLogIn;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Button btnConfirmAccountRegister;
        private Button btnLogInExistingAcc;
        private TextBox txtRegister_PhoneNumber;
        private TextBox txtRegister_Username;
        private TextBox txtConfrimRegister_Password;
        private TextBox txtRegister_Password;
        private TextBox txtRegister_Email;
    }
}
