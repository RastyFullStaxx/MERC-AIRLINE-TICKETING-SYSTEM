namespace MERC
{
    partial class Ticket : BaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ticket));
            lblPassengerName = new Label();
            lblOrigin = new Label();
            lblDestination = new Label();
            lblAge = new Label();
            lblPassengers = new Label();
            lblTravelInsurance = new Label();
            lblSenior = new Label();
            lblTripType = new Label();
            lblBoardingDate = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            lblBoardingTime = new Label();
            lblArrivalTime = new Label();
            lblArrivalDate = new Label();
            lblBookingTime = new Label();
            lblBookingDate = new Label();
            lblClassType = new Label();
            lblDestinationFromOrigin = new Label();
            lblTicketID = new Label();
            btnReturn = new Button();
            btnPrint = new Button();
            btnDownload = new Button();
            SuspendLayout();
            // 
            // lblPassengerName
            // 
            lblPassengerName.BackColor = Color.Transparent;
            lblPassengerName.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPassengerName.ForeColor = Color.Transparent;
            lblPassengerName.Location = new Point(919, 142);
            lblPassengerName.Name = "lblPassengerName";
            lblPassengerName.Size = new Size(298, 70);
            lblPassengerName.TabIndex = 0;
            lblPassengerName.Text = "ESPARTERO,\r\nRASTY\r\nC.";
            lblPassengerName.TextAlign = ContentAlignment.MiddleRight;
            lblPassengerName.Click += lblPassengerName_Click;
            // 
            // lblOrigin
            // 
            lblOrigin.BackColor = Color.Transparent;
            lblOrigin.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOrigin.ForeColor = Color.Transparent;
            lblOrigin.Location = new Point(564, 260);
            lblOrigin.Name = "lblOrigin";
            lblOrigin.Size = new Size(242, 53);
            lblOrigin.TabIndex = 1;
            lblOrigin.Text = "PASAY, \r\nPHILIPPINES";
            lblOrigin.TextAlign = ContentAlignment.MiddleLeft;
            lblOrigin.Click += lblOrigin_Click;
            // 
            // lblDestination
            // 
            lblDestination.BackColor = Color.Transparent;
            lblDestination.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDestination.ForeColor = Color.Transparent;
            lblDestination.Location = new Point(812, 260);
            lblDestination.Name = "lblDestination";
            lblDestination.Size = new Size(208, 53);
            lblDestination.TabIndex = 2;
            lblDestination.Text = "TOKYO, \r\nJAPAN";
            lblDestination.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblAge
            // 
            lblAge.BackColor = Color.Transparent;
            lblAge.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAge.ForeColor = Color.Transparent;
            lblAge.Location = new Point(1041, 285);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(61, 28);
            lblAge.TabIndex = 3;
            lblAge.Text = "16";
            lblAge.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPassengers
            // 
            lblPassengers.BackColor = Color.Transparent;
            lblPassengers.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPassengers.ForeColor = Color.Transparent;
            lblPassengers.Location = new Point(1130, 285);
            lblPassengers.Name = "lblPassengers";
            lblPassengers.Size = new Size(61, 28);
            lblPassengers.TabIndex = 4;
            lblPassengers.Text = "1";
            lblPassengers.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTravelInsurance
            // 
            lblTravelInsurance.BackColor = Color.Transparent;
            lblTravelInsurance.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTravelInsurance.ForeColor = Color.Transparent;
            lblTravelInsurance.Location = new Point(564, 383);
            lblTravelInsurance.Name = "lblTravelInsurance";
            lblTravelInsurance.Size = new Size(186, 28);
            lblTravelInsurance.TabIndex = 5;
            lblTravelInsurance.Text = "YES";
            lblTravelInsurance.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSenior
            // 
            lblSenior.BackColor = Color.Transparent;
            lblSenior.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSenior.ForeColor = Color.Transparent;
            lblSenior.Location = new Point(812, 383);
            lblSenior.Name = "lblSenior";
            lblSenior.Size = new Size(186, 28);
            lblSenior.TabIndex = 6;
            lblSenior.Text = "NO";
            lblSenior.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTripType
            // 
            lblTripType.BackColor = Color.Transparent;
            lblTripType.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTripType.ForeColor = Color.Transparent;
            lblTripType.Location = new Point(1041, 383);
            lblTripType.Name = "lblTripType";
            lblTripType.Size = new Size(186, 28);
            lblTripType.TabIndex = 7;
            lblTripType.Text = "ONE WAY";
            lblTripType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBoardingDate
            // 
            lblBoardingDate.BackColor = Color.Transparent;
            lblBoardingDate.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBoardingDate.ForeColor = Color.Transparent;
            lblBoardingDate.Location = new Point(564, 478);
            lblBoardingDate.Name = "lblBoardingDate";
            lblBoardingDate.Size = new Size(213, 28);
            lblBoardingDate.TabIndex = 8;
            lblBoardingDate.Text = "02/18/2025";
            lblBoardingDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBoardingTime
            // 
            lblBoardingTime.BackColor = Color.Transparent;
            lblBoardingTime.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBoardingTime.ForeColor = Color.Transparent;
            lblBoardingTime.Location = new Point(564, 506);
            lblBoardingTime.Name = "lblBoardingTime";
            lblBoardingTime.Size = new Size(213, 28);
            lblBoardingTime.TabIndex = 9;
            lblBoardingTime.Text = "6:00 AM";
            lblBoardingTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblArrivalTime
            // 
            lblArrivalTime.BackColor = Color.Transparent;
            lblArrivalTime.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblArrivalTime.ForeColor = Color.Transparent;
            lblArrivalTime.Location = new Point(812, 506);
            lblArrivalTime.Name = "lblArrivalTime";
            lblArrivalTime.Size = new Size(213, 28);
            lblArrivalTime.TabIndex = 11;
            lblArrivalTime.Text = "1:05 PM";
            lblArrivalTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblArrivalDate
            // 
            lblArrivalDate.BackColor = Color.Transparent;
            lblArrivalDate.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblArrivalDate.ForeColor = Color.Transparent;
            lblArrivalDate.Location = new Point(812, 478);
            lblArrivalDate.Name = "lblArrivalDate";
            lblArrivalDate.Size = new Size(213, 28);
            lblArrivalDate.TabIndex = 10;
            lblArrivalDate.Text = "02/19/2025";
            lblArrivalDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBookingTime
            // 
            lblBookingTime.BackColor = Color.Transparent;
            lblBookingTime.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBookingTime.ForeColor = Color.Transparent;
            lblBookingTime.Location = new Point(1041, 506);
            lblBookingTime.Name = "lblBookingTime";
            lblBookingTime.Size = new Size(213, 28);
            lblBookingTime.TabIndex = 13;
            lblBookingTime.Text = "10:00 AM";
            lblBookingTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBookingDate
            // 
            lblBookingDate.BackColor = Color.Transparent;
            lblBookingDate.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBookingDate.ForeColor = Color.Transparent;
            lblBookingDate.Location = new Point(1041, 478);
            lblBookingDate.Name = "lblBookingDate";
            lblBookingDate.Size = new Size(213, 28);
            lblBookingDate.TabIndex = 12;
            lblBookingDate.Text = "02/15/2025";
            lblBookingDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClassType
            // 
            lblClassType.BackColor = Color.Transparent;
            lblClassType.Font = new Font("Arial", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblClassType.ForeColor = Color.Transparent;
            lblClassType.Location = new Point(564, 167);
            lblClassType.Name = "lblClassType";
            lblClassType.Size = new Size(186, 28);
            lblClassType.TabIndex = 14;
            lblClassType.Text = "PRIVATE";
            lblClassType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDestinationFromOrigin
            // 
            lblDestinationFromOrigin.BackColor = Color.Transparent;
            lblDestinationFromOrigin.Font = new Font("Arial", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDestinationFromOrigin.ForeColor = Color.Transparent;
            lblDestinationFromOrigin.Location = new Point(672, 20);
            lblDestinationFromOrigin.Name = "lblDestinationFromOrigin";
            lblDestinationFromOrigin.Size = new Size(261, 50);
            lblDestinationFromOrigin.TabIndex = 15;
            lblDestinationFromOrigin.Text = "JPN-MNL";
            lblDestinationFromOrigin.TextAlign = ContentAlignment.MiddleCenter;
            lblDestinationFromOrigin.Click += label1_Click;
            // 
            // lblTicketID
            // 
            lblTicketID.BackColor = Color.Transparent;
            lblTicketID.Font = new Font("Arial Narrow", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTicketID.ForeColor = Color.Transparent;
            lblTicketID.Location = new Point(1059, 42);
            lblTicketID.Name = "lblTicketID";
            lblTicketID.Size = new Size(186, 28);
            lblTicketID.TabIndex = 16;
            lblTicketID.Text = "2135-144-2025";
            lblTicketID.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnReturn
            // 
            btnReturn.BackColor = Color.Transparent;
            btnReturn.Cursor = Cursors.Hand;
            btnReturn.FlatAppearance.BorderSize = 0;
            btnReturn.FlatAppearance.MouseDownBackColor = Color.White;
            btnReturn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.ForeColor = Color.Transparent;
            btnReturn.Image = (Image)resources.GetObject("btnReturn.Image");
            btnReturn.Location = new Point(965, 557);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(55, 19);
            btnReturn.TabIndex = 17;
            btnReturn.UseVisualStyleBackColor = false;
            btnReturn.Click += btnReturn_Click;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.Transparent;
            btnPrint.Cursor = Cursors.Hand;
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatAppearance.MouseDownBackColor = Color.White;
            btnPrint.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.ForeColor = Color.Transparent;
            btnPrint.Image = (Image)resources.GetObject("btnPrint.Image");
            btnPrint.Location = new Point(1059, 557);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(55, 19);
            btnPrint.TabIndex = 18;
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnDownload
            // 
            btnDownload.BackColor = Color.Transparent;
            btnDownload.Cursor = Cursors.Hand;
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.FlatAppearance.MouseDownBackColor = Color.White;
            btnDownload.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnDownload.FlatStyle = FlatStyle.Flat;
            btnDownload.ForeColor = Color.Transparent;
            btnDownload.Image = (Image)resources.GetObject("btnDownload.Image");
            btnDownload.Location = new Point(1142, 557);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(103, 19);
            btnDownload.TabIndex = 19;
            btnDownload.UseVisualStyleBackColor = false;
            btnDownload.Click += btnDownload_Click;
            // 
            // Ticket
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1281, 586);
            Controls.Add(btnDownload);
            Controls.Add(btnPrint);
            Controls.Add(btnReturn);
            Controls.Add(lblTicketID);
            Controls.Add(lblDestinationFromOrigin);
            Controls.Add(lblClassType);
            Controls.Add(lblBookingTime);
            Controls.Add(lblBookingDate);
            Controls.Add(lblArrivalTime);
            Controls.Add(lblArrivalDate);
            Controls.Add(lblBoardingTime);
            Controls.Add(lblBoardingDate);
            Controls.Add(lblTripType);
            Controls.Add(lblSenior);
            Controls.Add(lblTravelInsurance);
            Controls.Add(lblPassengers);
            Controls.Add(lblAge);
            Controls.Add(lblDestination);
            Controls.Add(lblOrigin);
            Controls.Add(lblPassengerName);
            Name = "Ticket";
            Text = "Ticket";
            ResumeLayout(false);
        }

        #endregion

        private Label lblPassengerName;
        private Label lblOrigin;
        private Label lblDestination;
        private Label lblAge;
        private Label lblPassengers;
        private Label lblTravelInsurance;
        private Label lblSenior;
        private Label lblTripType;
        private Label lblBoardingDate;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label lblBoardingTime;
        private Label lblArrivalTime;
        private Label lblArrivalDate;
        private Label lblBookingTime;
        private Label lblBookingDate;
        private Label lblClassType;
        private Label lblDestinationFromOrigin;
        private Label lblTicketID;
        private Button btnReturn;
        private Button btnPrint;
        private Button btnDownload;
    }
}