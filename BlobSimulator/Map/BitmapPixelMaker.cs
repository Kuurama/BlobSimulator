using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace BlobSimulator.Map
{
    /// A class to represent WriteableBitmap pixels in Bgra32 format.
    public class BitmapPixelMaker
    {
        private readonly int m_Height;

        /// The pixel array.
        private readonly byte[] m_Pixels;

        /// The number of bytes per row.
        private readonly int m_Stride;

        /// The bitmap's size.
        private readonly int m_Width;

        private readonly WriteableBitmap m_WritableBitmap;

        /// Constructor. Width and height required.
        public BitmapPixelMaker(int p_Width, int p_Height)
        {
            // Save the width and height.
            m_Width = p_Width;
            m_Height = p_Height;

            // Create the pixel array.
            m_Pixels = new byte[p_Width * p_Height * 4];

            // Calculate the stride.
            m_Stride = p_Width * 4;

            // Create the WriteableBitmap.
            m_WritableBitmap = new WriteableBitmap(
                m_Width, m_Height, 96, 96,
                PixelFormats.Bgra32, null);
        }


        /// Set all pixels to a specific color.
        public void SetColor(Color p_Color)
        {
            int l_NumBytes = m_Width * m_Height * 4;
            int l_Index = 0;
            while (l_Index < l_NumBytes)
            {
                m_Pixels[l_Index++] = p_Color.B;
                m_Pixels[l_Index++] = p_Color.G;
                m_Pixels[l_Index++] = p_Color.R;
                m_Pixels[l_Index++] = p_Color.A;
            }
        }

        /// Set a pixel's value.
        public void SetPixel(int p_X, int p_Y, Color p_Color)
        {
            int l_Index = p_Y * m_Stride + p_X * 4;
            m_Pixels[l_Index++] = p_Color.B;
            m_Pixels[l_Index++] = p_Color.G;
            m_Pixels[l_Index++] = p_Color.R;
            // ReSharper disable once RedundantAssignment
            m_Pixels[l_Index++] = p_Color.A;
        }

        /// DrawLine.
        public void DrawLine(float p_X1, float p_Y1, float p_X2, float p_Y2, bool p_BigLine, float p_Density, Color p_Color)
        {
            //float l_Slope = p_Y2 - p_Y1 /p_X2 - p_X1;
            //float l_YIntercept = p_Y2 - l_Slope*p_X2;

            float l_Distance = (float)Math.Sqrt(Math.Pow(p_X1 - p_X2, 2) + Math.Pow(p_Y1 - p_Y2, 2));
            int l_NumberOfPoint = (int)(l_Distance * p_Density);
            double l_D = Math.Sqrt((p_X1 - p_X2) * (p_X1 - p_X2) + (p_Y1 - p_Y2) * (p_Y1 - p_Y2)) / l_NumberOfPoint;
            double l_Fi = Math.Atan2(p_Y2 - p_Y1, p_X2 - p_X1);
            double l_CosFi = Math.Cos(l_Fi);
            double l_SinFi = Math.Sin(l_Fi);

            //float l_StepX = (p_X2 - p_X1) / l_NumberOfPoint;

            for (int l_I = 0; l_I < l_NumberOfPoint; l_I++)
                if ((int)(p_X1 - 1 + l_I * l_D * l_CosFi) >= 0 && (int)(p_X1 + 1 + l_I * l_D * l_CosFi) < m_Width && (int)(p_Y1 - 1 + l_I * l_D * l_SinFi) >= 0 && (int)(p_Y1 + 1 + l_I * l_D * l_SinFi) < m_Height)
                {
                    if (p_BigLine)
                    {
                        SetPixel((int)(p_X1 - 1 + l_I * l_D * l_CosFi), (int)(p_Y1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 + l_I * l_D * l_CosFi), (int)(p_Y1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 + 1 + l_I * l_D * l_CosFi), (int)(p_Y1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 + l_I * l_D * l_CosFi), (int)(p_Y1 - 1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 + l_I * l_D * l_CosFi), (int)(p_Y1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 + l_I * l_D * l_CosFi), (int)(p_Y1 + 1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 - 1 + l_I * l_D * l_CosFi), (int)(p_Y1 - 1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 - 1 + l_I * l_D * l_CosFi), (int)(p_Y1 + 1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 + 1 + l_I * l_D * l_CosFi), (int)(p_Y1 - 1 + l_I * l_D * l_SinFi), p_Color);
                        SetPixel((int)(p_X1 + 1 + l_I * l_D * l_CosFi), (int)(p_Y1 + 1 + l_I * l_D * l_SinFi), p_Color);
                    }
                    else
                    {
                        SetPixel((int)(p_X1 + l_I * l_D * l_CosFi), (int)(p_Y1 + l_I * l_D * l_SinFi), p_Color);
                    }
                }
        }

        /// Draw a square
        public void DrawSquare(int p_X1, int p_Y1, int p_X2, int p_Y2, bool p_BigLine, float p_Density, Color p_Color)
        {
            DrawLine(p_X1, p_Y1, p_X2, p_Y1, p_BigLine, 1, p_Color); /// Up/Down

            DrawLine(p_X1, p_Y2, p_X2, p_Y2, p_BigLine, 1, p_Color); /// Up/Down

            DrawLine(p_X2, p_Y1, p_X2, p_Y2, p_BigLine, 1, p_Color); /// Left/Right

            DrawLine(p_X1, p_Y1, p_X1, p_Y2, p_BigLine, 1, p_Color); /// Left/Right
        }


        /// Set a pixel's value.
        public Color GetPixel(int p_X, int p_Y)
        {
            int l_Index = p_Y * m_Stride + p_X * 4;
            return Color.FromArgb(m_Pixels[l_Index + 3], m_Pixels[l_Index + 2], m_Pixels[l_Index + 1], m_Pixels[l_Index + 0]);
        }


        /// <summary>
        ///     Blur and Evaporate each pixel depending on it's neighborhoods.
        /// </summary>
        public void BlurAndEvaporateAllPixel(float p_EvaporateStep, float p_BlurStep)
        {
            int l_NumBytes = m_Width * m_Height * 4;
            int l_Index = 0;

            while (l_Index < l_NumBytes)
            {
                if ((BitConverter.ToUInt32(m_Pixels, l_Index) & 0xFFFFFF00) == 0)
                {
                    l_Index += 4;
                    continue;
                }


                int l_PreviousHeightPixel = l_Index - 4 * m_Width;
                int l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                int l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                int l_PreviousPixel = l_Index - 4;
                int l_CurrentPixelValue = m_Pixels[l_Index];
                int l_NextPixel = l_Index + 4;
                int l_NextHeightPixel = l_Index + 4 * m_Width;
                int l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                int l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                int l_PixelSum = l_CurrentPixelValue;


                if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];

                if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixel];

                if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];

                if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousPixel];

                if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextPixel];

                if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixelLeft];

                if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixel];

                if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixelRight];

                float l_BlurValue = (float)l_PixelSum / 9;

                float l_DiffusedValue = StaticFunction.Lerp(l_CurrentPixelValue, l_BlurValue, p_BlurStep);

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue - p_EvaporateStep); /// Difused and Evaporated Value.

                l_Index++;

                l_PreviousHeightPixel = l_Index - 4 * m_Width;
                l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                l_PreviousPixel = l_Index - 4;
                l_CurrentPixelValue = m_Pixels[l_Index];
                l_NextPixel = l_Index + 4;
                l_NextHeightPixel = l_Index + 4 * m_Width;
                l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                l_PixelSum = l_CurrentPixelValue;


                if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];

                if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixel];

                if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];

                if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousPixel];

                if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextPixel];

                if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixelLeft];

                if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixel];

                if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixelRight];

                l_BlurValue = (float)l_PixelSum / 9;

                l_DiffusedValue = StaticFunction.Lerp(l_CurrentPixelValue, l_BlurValue, p_BlurStep);

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue - p_EvaporateStep); /// Difused and Evaporated Value.


                l_Index++;

                l_PreviousHeightPixel = l_Index - 4 * m_Width;
                l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                l_PreviousPixel = l_Index - 4;
                l_CurrentPixelValue = m_Pixels[l_Index];
                l_NextPixel = l_Index + 4;
                l_NextHeightPixel = l_Index + 4 * m_Width;
                l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                l_PixelSum = l_CurrentPixelValue;


                if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];

                if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixel];

                if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];

                if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_PreviousPixel];

                if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextPixel];

                if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixelLeft];

                if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixel];

                if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4) l_PixelSum += m_Pixels[l_NextHeightPixelRight];

                l_BlurValue = (float)l_PixelSum / 9;

                l_DiffusedValue = StaticFunction.Lerp(l_CurrentPixelValue, l_BlurValue, p_BlurStep);

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue - p_EvaporateStep); /// Difused and Evaporated Value.

                l_Index++;


                /*l_PreviousHeightPixel = l_Index - 4 * m_Width;
                l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                l_PreviousPixel = l_Index - 4;
                l_CurrentPixelValue = m_Pixels[l_Index];
                l_NextPixel = l_Index + 4;
                l_NextHeightPixel = l_Index + 4 * m_Width;
                l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                l_PixelSum = l_CurrentPixelValue;


                if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];
                }

                if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixel];
                }
                
                if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];
                }
                
                if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousPixel];
                }

                if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextPixel];
                }
                
                if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelLeft];
                }
                
                if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixel];
                }
                
                if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelRight];
                }

                l_BlurValue = (float)l_PixelSum / 9;

                l_DiffusedValue = StaticFunction.Lerp( l_CurrentPixelValue, l_BlurValue, p_BlurStep);

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue - p_EvaporateStep); /// Difused and Evaporated Value.*/

                l_Index++;
            }
        }


        /// Use the pixel data to create a WriteableBitmap.
        public WriteableBitmap MakeBitmap(double p_DpiX, double p_DpiY)
        {
            /// Load the pixel data.
            Int32Rect l_Rect = new Int32Rect(0, 0, m_Width, m_Height);
            m_WritableBitmap.WritePixels(l_Rect, m_Pixels, m_Stride, 0);

            /// Return the bitmap.
            return m_WritableBitmap;
        }


        /*
        /// Get a pixel's value.
        public Color GetPixel(int p_X, int p_Y)
        {
            int l_Index = p_Y * m_Stride + p_X * 4;
            byte l_B = m_Pixels[l_Index++];
            byte l_G = m_Pixels[l_Index++];
            byte l_R = m_Pixels[l_Index++];
            // ReSharper disable once RedundantAssignment
            byte l_A = m_Pixels[l_Index++];
            return Color.FromArgb(blue: l_B, green: l_G, red: l_R, alpha: l_A);
        }
        public byte GetBlue(int p_X, int p_Y)
        {
            return m_Pixels[p_Y * m_Stride + p_X * 4];
        }

        public byte GetGreen(int p_X, int p_Y)
        {
            return m_Pixels[p_Y * m_Stride + p_X * 4 + 1];
        }

        public byte GetRed(int p_X, int p_Y)
        {
            return m_Pixels[p_Y * m_Stride + p_X * 4 + 2];
        }

        public byte GetAlpha(int p_X, int p_Y)
        {
            return m_Pixels[p_Y * m_Stride + p_X * 4 + 3];
        }

        /// Draw a specific sized dot on the beatmap.
        public void DrawDot(int p_X, int p_Y, Color p_Color)
        {
            int l_Index = p_Y * m_Stride + p_X * 4;
            m_Pixels[l_Index++] = p_Color.B;
            m_Pixels[l_Index++] = p_Color.G;
            m_Pixels[l_Index++] = p_Color.R;
            // ReSharper disable once RedundantAssignment
            m_Pixels[l_Index++] = p_Color.A;
        }
        public void SetBlue(int p_X, int p_Y, byte p_Blue)
        {
            m_Pixels[p_Y * m_Stride + p_X * 4] = p_Blue;
        }
        public void SetGreen(int p_X, int p_Y, byte p_Green)
        {
            m_Pixels[p_Y * m_Stride + p_X * 4 + 1] = p_Green;
        }
        public void SetRed(int p_X, int p_Y, byte p_Red)
        {
            m_Pixels[p_Y * m_Stride + p_X * 4 + 2] = p_Red;
        }
        public void SetAlpha(int p_X, int p_Y, byte p_Alpha)
        {
            m_Pixels[p_Y * m_Stride + p_X * 4 + 3] = p_Alpha;
        }
        /// Lower all pixels color except the Alpha value.
        public void EvaporateAllPixel(int p_EvaporateStep)
        {
            int l_NumBytes = m_Width * m_Height * 4;
            int l_Index = 0;

            while (l_Index < l_NumBytes)
            {
                if (m_Pixels[l_Index] - p_EvaporateStep < 0)
                    m_Pixels[l_Index] = 0;
                else
                    m_Pixels[l_Index] = (byte)(m_Pixels[l_Index] - p_EvaporateStep);


                l_Index++;

                if (m_Pixels[l_Index] - p_EvaporateStep < 0)
                    m_Pixels[l_Index] = 0;
                else
                    m_Pixels[l_Index] = (byte)(m_Pixels[l_Index] - p_EvaporateStep);

                l_Index++;

                if (m_Pixels[l_Index] - p_EvaporateStep < 0)
                    m_Pixels[l_Index] = 0;
                else
                    m_Pixels[l_Index] = (byte)(m_Pixels[l_Index] - p_EvaporateStep);

                l_Index++;

               ///
               ///
               /// if ((m_Pixels[l_Index] - p_EvaporateStep) < 0)
               /// {
               ///     m_Pixels[l_Index] = 0;
               /// }
               /// else
               /// {
               ///     m_Pixels[l_Index] = m_Pixels[l_Index];
               /// }
               ///

                l_Index++;
            }
        }

        /// <summary>
        /// Blur each pixel depending on it's neighborhoods.
        /// </summary>
        public void BlurAllPixel(float p_BlurStep)
        {
            int l_NumBytes = m_Width * m_Height * 4;
            int l_Index = 0;

            while (l_Index < l_NumBytes)
            {
                
                int l_PreviousHeightPixel = l_Index - 4 * m_Width;
                int l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                int l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                int l_PreviousPixel = l_Index - 4;
                int l_CurrentPixelValue = m_Pixels[l_Index];
                int l_NextPixel = l_Index + 4;
                int l_NextHeightPixel = l_Index + 4 * m_Width;
                int l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                int l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                int l_PixelSum = l_CurrentPixelValue;


                if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];
                }

                if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixel];
                }
                
                if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];
                }
                
                if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousPixel];
                }

                if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextPixel];
                }
                
                if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelLeft];
                }
                
                if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixel];
                }
                
                if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelRight];
                }

                float l_BlurValue = (float)l_PixelSum / 9;

                float l_DiffusedValue = StaticFunction.Lerp( l_CurrentPixelValue, l_BlurValue, p_BlurStep);

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue);

                l_Index++;

                l_PreviousHeightPixel = l_Index - 4 * m_Width;
                l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                l_PreviousPixel = l_Index - 4;
                l_CurrentPixelValue = m_Pixels[l_Index];
                l_NextPixel = l_Index + 4;
                l_NextHeightPixel = l_Index + 4 * m_Width;
                l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                l_PixelSum = l_CurrentPixelValue;


                if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];
                }

                if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixel];
                }
                
                if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];
                }
                
                if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousPixel];
                }

                if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextPixel];
                }
                
                if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelLeft];
                }
                
                if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixel];
                }
                
                if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelRight];
                }

                l_BlurValue = (float)l_PixelSum / 9;

                l_DiffusedValue = StaticFunction.Lerp( l_CurrentPixelValue, l_BlurValue, p_BlurStep);

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue);


                l_Index++;

                l_PreviousHeightPixel = l_Index - 4 * m_Width;
                l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                l_PreviousPixel = l_Index - 4;
                l_CurrentPixelValue = m_Pixels[l_Index];
                l_NextPixel = l_Index + 4;
                l_NextHeightPixel = l_Index + 4 * m_Width;
                l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                l_PixelSum = l_CurrentPixelValue;


                if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];
                }

                if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixel];
                }
                
                if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];
                }
                
                if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_PreviousPixel];
                }

                if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextPixel];
                }
                
                if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelLeft];
                }
                
                if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixel];
                }
                
                if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4)
                {
                    l_PixelSum += m_Pixels[l_NextHeightPixelRight];
                }

                l_BlurValue = (float)l_PixelSum / 9;

                l_DiffusedValue = StaticFunction.Lerp( l_CurrentPixelValue, l_BlurValue, p_BlurStep);

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue);

                l_Index++;
                

                /// l_PreviousHeightPixel = l_Index - 4 * m_Width;
                /// l_PreviousHeightPixelLeft = l_Index - 4 * (m_Width - 1);
                /// l_PreviousHeightPixelRight = l_Index - 4 * (m_Width + 1);
                /// l_PreviousPixel = l_Index - 4;
                /// l_CurrentPixelValue = m_Pixels[l_Index];
                /// l_NextPixel = l_Index + 4;
                /// l_NextHeightPixel = l_Index + 4 * m_Width;
                /// l_NextHeightPixelRight = l_Index + 4 * (m_Width - 1);
                /// l_NextHeightPixelLeft = l_Index + 4 * (m_Width + 1);
                /// l_PixelSum = l_CurrentPixelValue;
                /// 
                /// 
                /// if (l_PreviousHeightPixelLeft >= 0 && l_PreviousHeightPixelLeft < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_PreviousHeightPixelLeft];
                /// }
                /// 
                /// if (l_PreviousHeightPixel >= 0 && l_PreviousHeightPixel < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_PreviousHeightPixel];
                /// }
                /// 
                /// if (l_PreviousHeightPixelRight >= 0 && l_PreviousHeightPixelRight < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_PreviousHeightPixelRight];
                /// }
                /// 
                /// if (l_PreviousPixel >= 0 && l_PreviousPixel < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_PreviousPixel];
                /// }
                /// 
                /// if (l_NextPixel >= 0 && l_NextPixel < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_NextPixel];
                /// }
                /// 
                /// if (l_NextHeightPixelLeft >= 0 && l_NextHeightPixelLeft < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_NextHeightPixelLeft];
                /// }
                /// 
                /// if (l_NextHeightPixel >= 0 && l_NextHeightPixel < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_NextHeightPixel];
                /// }
                /// 
                /// if (l_NextHeightPixelRight >= 0 && l_NextHeightPixelRight < m_Width * m_Height * 4)
                /// {
                ///     l_PixelSum += m_Pixels[l_NextHeightPixelRight];
                /// }
                /// 
                /// l_BlurValue = (float)l_PixelSum / 9;
                /// 
                /// l_DiffusedValue = StaticFunction.Lerp( l_CurrentPixelValue, l_BlurValue, p_BlurStep);
                /// 
                /// m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue);

                l_Index++;
            }
        }

        */
    }
}