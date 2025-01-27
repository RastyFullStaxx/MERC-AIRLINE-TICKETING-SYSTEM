using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MERC
{
    public class BaseForm : Form // Changed to 'public'
    {
        public BaseForm()
        {
            // Set default form properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen; // Centers the form on the screen
            this.Size = new System.Drawing.Size(1440, 1024); // Default window size
        }
    }
}
