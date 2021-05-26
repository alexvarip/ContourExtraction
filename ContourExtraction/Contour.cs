using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContourExtraction
{
    public class Contour
    {
        private readonly YuvModel _yuv;
        private readonly Helpers _helpers;
        private int[] histogram = new int[256];
        private int threshold = 0;
        private int _mask;

        public Contour(YuvModel yuv, Helpers helpers)
        {
            _yuv = yuv;
            _helpers = helpers;
            _mask = (_yuv.Mask - 1) / 2;
        }


    public Task ContourTracing()
        {

            Parallel.Invoke(
                () => _yuv.Yplane = _helpers.ConvertTo2DArray(_yuv.Ybytes, _yuv.YHeight, _yuv.YWidth),
                () => _yuv.Uplane = _helpers.ConvertTo2DArray(_yuv.Ubytes, _yuv.UHeight, _yuv.UWidth),
                () => _yuv.Vplane = _helpers.ConvertTo2DArray(_yuv.Vbytes, _yuv.VHeight, _yuv.VWidth));


            _yuv.YBinary = new byte[_yuv.YHeight, _yuv.YWidth];
            _yuv.YContour = new byte[_yuv.YHeight, _yuv.YWidth];


            _ = CreateHistogram()
               .FindThreshold()
               .Binarize()
               .ApplyErosion();
            

            return Task.CompletedTask;
        }


        private Contour CreateHistogram()
        {
            for (int i = 0; i < _yuv.Yplane.GetLength(0); i++)
            {
                for (int j = 0; j < _yuv.Yplane.GetLength(1); j++)
                {
                    byte byteColor = _yuv.Yplane[i, j];

                    histogram[byteColor]++;
                }
            }

            return this;
        }


        private Contour FindThreshold()
        {
            float weight_B = 0;
            float sum = 0;
            float sumB = 0;
            float varMax = 0;


            for (int t = 0; t < 256; t++)
            {
                sum += t * histogram[t];
            }

            for (int t = 0; t < 256; t++)
            {
                // Weight Background 
                weight_B += histogram[t];
                if (weight_B is 0) continue;


                // Weight Foreground
                float weight_F = _yuv.YResolution - weight_B;
                if (weight_F is 0) continue;


                sumB += t * histogram[t];

                // Mean Background
                float mean_B = sumB / weight_B;

                // Mean Foreground
                float mean_F = (sum - sumB) / weight_F;

                // Calculate Between Class Variance
                float varBetween = (float)weight_B * (float)weight_F * (mean_B - mean_F) * (mean_B - mean_F);

                // Check if new maximum found
                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = t;
                }

            }

            return this;
        }


        private Contour Binarize()
        {
            for (int i = 0; i < _yuv.Yplane.GetLength(0); i++)
                for (int j = 0; j < _yuv.Yplane.GetLength(1); j++)
                    _yuv.YBinary[i, j] = _yuv.Yplane[i, j] < threshold ? (byte)0 : (byte)255;

            return this;
        }


        private Contour ApplyErosion()
        {

            // Erosion is being applied to the non extended byte array version, thus it does not take in the image's border values.
            // The mask value for the erosion function is fixed to 3, because the B array size to erode with is 3x3.
            // β(A) = A - (A (-) B)

            for (int i = _mask; i < _yuv.YBinary.GetLength(0) - _mask; i++)
            {
                for (int j = _mask; j < _yuv.YBinary.GetLength(1) - _mask; j++) 
                {
                    Erosion(i, j);
                }
            }

            
            for (int i = _mask; i < _yuv.YBinary.GetLength(0) - _mask; i++)
            {
                for (int j = _mask; j < _yuv.YBinary.GetLength(1) - _mask; j++)
                {
                    if (_yuv.YBinary[i, j] == _yuv.YContour[i, j])
                        _yuv.YBinary[i, j] = 0;
                }
            }


            return this;
        }


        private void Erosion(int i_index, int j_index)
        {
            int count = 0;
            int x = 0, y = 0;
            byte[,] B = new byte[,] { { 255,255,255 },
                                      { 255,255,255 },
                                      { 255,255,255 }
            };


            for (int w = i_index - _mask; w <= i_index + _mask; w++)
            {
                for (int z = j_index - _mask; z <= j_index + _mask; z++)
                {
                    if (_yuv.YBinary[w, z] == B[x, y])
                        count++;

                    y++;
                }
                x++;
                y = 0;
            }

            _yuv.YContour[i_index, j_index] = count is 9 ? (byte)255 : (byte)0;

        }


    }
}
