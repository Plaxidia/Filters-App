using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Bitmap image;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
           

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.bmp|All Files(*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK) 
            {

                image = new Bitmap(dialog.FileName);
                //visualise the image
                pictureBox1.Image = image;
                pictureBox1.Refresh();

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
