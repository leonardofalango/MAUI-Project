using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MaiuProject
{
    public static class Blur
    {
        public static byte[] ParallelBlur((Bitmap bmp, byte[] img) t, int radius = 5) // Mais rápido
        {
            byte[] integralImage = new byte[t.bmp.Width * t.bmp.Height];
            byte[] returnImg = new byte[t.bmp.Width * t.bmp.Height];

            int N = radius * 2 + 1;
            int A = N * N;
            int index;

            int width = t.bmp.Width;

            integralImage[0] = t.img[0];

            for (int i = 1; i < width; i++)
            {
                integralImage[i] = (byte)(t.img[i] + integralImage[i - 1]);
            }
            for (int j = 1; j < t.bmp.Height; j++)
            {
                index = j * width;
                integralImage[index] = (byte)(t.img[index] + integralImage[index - 1]);
            }

            for (int j = 1; j < t.bmp.Height; j++)
            {
                for (int i = 1; i < width; i++)
                {
                    integralImage[i + j * width] = (byte)(t.img[i + j * width]
                        + integralImage[i + (j - 1) * width]
                        + integralImage[i - 1 + j * width]
                        - integralImage[i - 1 + (j - 1) * width]);
                }
            }

            index = 0;

            Parallel.For(radius + 1, t.bmp.Height - radius, j =>
            {
                for (int i = radius + 1; i < width - radius; i++)
                {
                    float pixelSoma = integralImage[i + radius + ((j + radius) * width)];
                    float pixelSub1 = integralImage[i + radius + ((j - radius - 1) * width)];
                    float pixelSub2 = integralImage[i - radius - 1 + ((j + radius) * width)];
                    float pixelSoma2 = integralImage[i - radius - 1 + ((j - radius - 1) * width)];

                    float r = (byte)((pixelSoma2 + pixelSoma - pixelSub1 - pixelSub2) / A);

                    returnImg[index] = (byte)r;
                    index++;
                }
            });

            return returnImg;
        }
    }
}
