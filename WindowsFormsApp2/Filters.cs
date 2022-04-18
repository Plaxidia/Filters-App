using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public abstract class Filters
    {
        protected abstract Color  calculateNewPixelColor(Bitmap sourceImage, int x, int y);
         
        public Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {   //at the moment the function creates a blank image 
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            //get all pixels of the image
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }
        //calcuculate Pixel color and should be unique to eacg real class

        public int Clamp(int value,int min,int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

    }


    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R, 255 - sourceColor.G, 255 - sourceColor.B);
            return resultColor;
        }


    }
    //Matrix filters
    class MatrixFilters :Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilters() { }
        public MatrixFilters(float[,] kernel)
            {
            this.kernel= kernel;
            }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            //store  the color componets of the resulting color
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;

            //create the loops that will sort out the pixel neigborbood 
            for (int I = -radiusY; I <= radiusY; I++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + I, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, I + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, I + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, I + radiusY];
                    //as a result work  return the color, made of your variables


                }

            }
            return Color.FromArgb(
               Clamp((int)resultR, 0, 255),
               Clamp((int)resultB, 0, 255),
               Clamp((int)resultG, 0, 255)
               );
        }
    }
    //create a blurfilter thta overrides the default constaractor 
    class Blurfilter : MatrixFilters
    {
        public Blurfilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);

                }

            }
        }
    }
    class GaussianFilter : MatrixFilters
    {
        public GaussianFilter()
        {
            createGaussianKernel(3, 2);
           
<<<<<<< HEAD
        }
       
            public void createGaussianKernel(int radius, float sigma)
            {
                //calculate kernel size
                int size = 2 * radius + 1;
                //create kernel
                kernel = new float[size, size];
                //coefficient of norma
                float norm = 0;
                //calculate coefficients
                for (int i = -radius; i <= radius; i++)
                {
                    for (int j = -radius; j <= radius; j++)
                    {
                        kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / sigma * sigma));
                        norm += kernel[i + radius, j + radius];

                    }
                }
                //normalize coefficients
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        kernel[i, j] /= norm;

                    }
                }

            }
        
    }
    class GrayScaleFilter:Filters
    {  
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            //store  the color componets of the resulting color
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;

            //create the loops that will sort out the pixel neigborbood 
            
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {

                    Color oc = sourceImage.GetPixel(i, j);
                    int grayscale = (int)((oc.R * 0.36) + (oc.G * 0.53) + oc.B * 0.11);
                    Color nc = Color.FromArgb(oc.A, grayscale, grayscale, grayscale);
                    resultImage.SetPixel(i, j, nc);


                }

            }
            return Color.FromArgb(
               Clamp((int)resultR, 0, 255),
               Clamp((int)resultB, 0, 255),
               Clamp((int)resultG, 0, 255)
               );
=======
>>>>>>> ab147995782bd8346310ded6cc6da108c3b9c1fd
        }
       
            public void createGaussianKernel(int radius, float sigma)
            {
                //calculate kernel size
                int size = 2 * radius + 1;
                //create kernel
                kernel = new float[size, size];
                //coefficient of norma
                float norm = 0;
                //calculate coefficients
                for (int i = -radius; i <= radius; i++)
                {
                    for (int j = -radius; j <= radius; j++)
                    {
                        kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / sigma * sigma));
                        norm += kernel[i + radius, j + radius];

                    }
                }
                //normalize coefficients
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        kernel[i, j] /= norm;

                    }
                }

            }
        
    }
    
}