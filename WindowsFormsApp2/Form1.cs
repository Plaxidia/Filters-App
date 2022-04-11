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

        private void invertFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             InvertFilter filter = new InvertFilter();
             Bitmap resultImage = filter.processImage(image);
             pictureBox1.Image = resultImage;
             pictureBox1.Refresh();
            
           */
            Filters filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            image = ((Filters)e.Argument).processImage(image,backgroundWorker1);

        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Image = image;
            pictureBox1.Refresh();
            progressBar1.Value = 0;
        }
    }
}
