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
    public partial class Homepage : Form
    {
        private int currentImageIndex = 0;
        private string[] imagePaths;

        public Homepage()
        {
            InitializeComponent();
        }

        private void Homepage_Load(object sender, EventArgs e)
        {
            // Get all image files from the specified folder
            string folderPath = @"C:\Users\MSI\source\repos\MERC\MERC\assets\";

            try
            {
                // Load all image files from the folder
                imagePaths = Directory.GetFiles(folderPath, "*.jpg")
                    .Concat(Directory.GetFiles(folderPath, "*.png"))
                    .Concat(Directory.GetFiles(folderPath, "*.bmp"))
                    .ToArray();

                // Check if images were found
                if (imagePaths.Length == 0)
                {
                    MessageBox.Show("No images found in the specified folder.");
                    return;
                }

                // Display the first image
                imgbSlideshow.Image = Image.FromFile(imagePaths[currentImageIndex]);

                // Start the Timer
                timer1.Interval = 3000; // 3 seconds
                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
            }
        }

        private void imgbSlideshow_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (imagePaths == null || imagePaths.Length == 0) return;

            // Move to the next image
            currentImageIndex = (currentImageIndex + 1) % imagePaths.Length;

            try
            {
                // Display the next image
                imgbSlideshow.Image = Image.FromFile(imagePaths[currentImageIndex]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying image: {ex.Message}");
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {

        }
    }
}
