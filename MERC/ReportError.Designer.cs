namespace MERC
{
    partial class ReportError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportError));
            lblErrorText = new Label();
            lblErrorCode = new Label();
            lblErrorDescription = new Label();
            btnRetry = new Button();
            SuspendLayout();
            // 
            // lblErrorText
            // 
            lblErrorText.AutoSize = true;
            lblErrorText.BackColor = Color.Transparent;
            lblErrorText.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblErrorText.ForeColor = Color.White;
            lblErrorText.Location = new Point(344, 167);
            lblErrorText.Name = "lblErrorText";
            lblErrorText.Size = new Size(250, 50);
            lblErrorText.TabIndex = 0;
            lblErrorText.Text = "ERROR CODE";
            // 
            // lblErrorCode
            // 
            lblErrorCode.BackColor = Color.Transparent;
            lblErrorCode.Font = new Font("Segoe UI", 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblErrorCode.ForeColor = Color.White;
            lblErrorCode.Location = new Point(417, 196);
            lblErrorCode.Name = "lblErrorCode";
            lblErrorCode.Size = new Size(109, 128);
            lblErrorCode.TabIndex = 1;
            lblErrorCode.Text = "1";
            lblErrorCode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblErrorDescription
            // 
            lblErrorDescription.BackColor = Color.Transparent;
            lblErrorDescription.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblErrorDescription.ForeColor = Color.White;
            lblErrorDescription.Location = new Point(191, 341);
            lblErrorDescription.Name = "lblErrorDescription";
            lblErrorDescription.Size = new Size(546, 62);
            lblErrorDescription.TabIndex = 2;
            lblErrorDescription.Text = "Exceed the number of passenger reservation";
            lblErrorDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRetry
            // 
            btnRetry.BackColor = Color.Transparent;
            btnRetry.Cursor = Cursors.Hand;
            btnRetry.FlatAppearance.BorderSize = 0;
            btnRetry.FlatAppearance.MouseDownBackColor = Color.White;
            btnRetry.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnRetry.FlatStyle = FlatStyle.Flat;
            btnRetry.ForeColor = Color.Transparent;
            btnRetry.Image = (Image)resources.GetObject("btnRetry.Image");
            btnRetry.Location = new Point(326, 427);
            btnRetry.Name = "btnRetry";
            btnRetry.Size = new Size(280, 72);
            btnRetry.TabIndex = 9;
            btnRetry.UseVisualStyleBackColor = false;
            btnRetry.Click += btnRetry_Click;
            // 
            // ReportError
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(929, 586);
            Controls.Add(btnRetry);
            Controls.Add(lblErrorDescription);
            Controls.Add(lblErrorText);
            Controls.Add(lblErrorCode);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ReportError";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ReportError";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblErrorText;
        private Label lblErrorCode;
        private Label lblErrorDescription;
        private Button btnRetry;
    }
}