using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MERC
{
    public partial class Ticket : BaseForm
    {
        public Ticket()
        {
            InitializeComponent();
        }

        private void lblOrigin_Click(object sender, EventArgs e)
        {

        }

        private void lblPassengerName_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
            this.Hide();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();

            // Attach the PrintPage event to define the printing logic
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            // Display the print preview dialog to the user
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 800,
                Height = 600
            };

            try
            {
                // Show the print preview dialog
                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while printing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Capture the current frame (Ticket.cs) as an image or use controls
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));

            // Draw the image on the print page
            e.Graphics.DrawImage(bitmap, 0, 0, e.PageBounds.Width, e.PageBounds.Height);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a bitmap to store the current frame (Ticket form)
                Bitmap bmp = new Bitmap(this.Width, this.Height);

                // Draw the form onto the bitmap
                this.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                // Show a SaveFileDialog to let the user choose where to save the image
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JPEG Image|*.jpg";
                    saveFileDialog.Title = "Save Ticket as Image";
                    saveFileDialog.FileName = "Ticket.jpg";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the bitmap as a JPG file
                        bmp.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        MessageBox.Show("Ticket successfully saved as an image!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Dispose of the bitmap
                bmp.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the ticket: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
