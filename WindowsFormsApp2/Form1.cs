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

        private void ToolStripMenuItem1_Click(object sender, EventArgs e )
        {


            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Image files|*.png;*.bmp|All Files(*.*)|*.*"
            };

            if (dialog.ShowDialog() == DialogResult.OK) 
            {

                image = new Bitmap(dialog.FileName);
                //visualise the image
                pictureBox1.Image = image;
                pictureBox1.Refresh();

            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void InvertFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            image = ((Filters)e.Argument).ProcessImage(image,backgroundWorker1);

        }

       

        private void Button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Image = image;
            pictureBox1.Refresh();
            progressBar1.Value = 0;
        }

        private void MatrixFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //on the form in the matrix filters add an element "Blur" and do double click functio to crate a function
        private void BlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Blurfilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void GaussianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void GrayScaleFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayScaleFilter();

            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void SepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Sepia();

            backgroundWorker1.RunWorkerAsync(filter);
        }


        private void ThresholdToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Filters filter = new Threshold();

            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void FiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
