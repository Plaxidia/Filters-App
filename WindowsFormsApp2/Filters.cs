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
        protected abstract Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y);

        public Bitmap ProcessImage(Bitmap sourceImage, BackgroundWorker worker)
        {   //at the moment the function creates a blank image 
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            //get all pixels of the image
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, CalculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }
        //calcuculate Pixel color and should be unique to eacg real class

        public int Clamp(int value, int min, int max)
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
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R, 255 - sourceColor.G, 255 - sourceColor.B);
            return resultColor;
        }
    }
    //Matrix filters
    class MatrixFilters : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilters() { }
        public MatrixFilters(float[,] kernel)
        {
            this.kernel = kernel;
        }
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
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
               Clamp((int)resultG, 0, 255),
               Clamp((int)resultB, 0, 255)
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
            CreateGaussianKernel(3, 2);
        }

        public void CreateGaussianKernel(int radius, float sigma)
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
    class GrayScaleFilter : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color oc = sourceImage.GetPixel(x, y);
            int grayscale = (int)((oc.R * 0.36) + (oc.G * 0.53) + oc.B * 0.11);
            Color nc = Color.FromArgb(oc.A, grayscale, grayscale, grayscale);

            return nc;
        }
    }
    //create a filter that will convert image into brown color 
    class Sepia : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color oc = sourceImage.GetPixel(x, y);
            int grayscale = (int)((oc.R * 0.36) + (oc.G * 0.53) + oc.B * 0.11);

            float k = 20;
            float R = grayscale + (2 * k);
            float G = grayscale + 0.5f * k;
            float B = grayscale - 1 * k;

            // return Color.FromArgb(A, R, G, B);
            return Color.FromArgb(
             Clamp((int)oc.A, 0, 255),
             Clamp((int)R, 0, 255),
             Clamp((int)G, 0, 255),
              Clamp((int)B, 0, 255)
             );
        }
    }
    class Threshold : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int red = sourceColor.R;
            int green = sourceColor.G;
            int blue = sourceColor.B;
            int gray = (int)((.36 * red) + (.53 * green) + (.11 * blue));
            if (gray < 128)
            {
                red = 0;
                green = 0;
                blue = 0;
            }
            else
            {
                red = 255;
                green = 255;
                blue = 255;
            }
            Color resultColor = Color.FromArgb(red, green, blue);
            return resultColor;

        }
    }

    class Sobel : MatrixFilters
    {
        public Sobel()
        {
            
            kernel = new float[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        }
    }
    class Imagesharpener : MatrixFilters
    {
        public Imagesharpener()
        {
            kernel = new float[3, 3] { { 0,-2, 0 }, { -2, 11, -2 }, { 0, -2, 0 } };
            
        }
    }
    class Embosing: MatrixFilters
    {
      

        public Embosing()
        {
            float brightness = 1.0f;

            kernel = new float[3, 3] { { 0, 1, 0 }, { 1, 0, -1 }, { 0, -1, 0 } };
            //+brightness;
           


        }
    }
    class Motion:MatrixFilters
    {
        public Motion()
        {

            kernel = new float[9, 9] { { 1, 0, 0, 0, 0, 0, 0, 0, 1 },
                                        { 0, 1, 0, 0, 0, 0, 0, 1, 0 },
                                        {  0, 0, 1, 0, 0, 0, 1, 0, 0},
                                        { 0, 0, 0, 1, 0, 1, 0, 0, 0 },
                                        { 0, 0, 0, 0, 1 ,0, 0, 0, 0 },
                                        { 0, 0, 0, 1, 0, 1, 0, 0, 0 },
                                        {  0, 0, 1, 0, 0, 0, 1, 0, 0},
                                        { 0, 1, 0, 0, 0, 0, 0, 1, 0 },
                                        { 1, 0, 0, 0, 0, 0, 0, 0, 1 } };
            /*int n = 9;//amount of ones
            for (int i = 0; i < n; i++)
            {
                double factor = 1 / n;
                kernel =factor(new float[9,9] );
            }
            */
        }

    }       
}
