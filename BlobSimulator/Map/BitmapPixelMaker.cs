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
        }

        /// Get a pixel's value.
        public void GetPixel(int p_X, int p_Y, out byte p_Red, out byte p_Green, out byte p_Blue, out byte p_Alpha)
        {
            int l_Index = p_Y * m_Stride + p_X * 4;
            p_Blue = m_Pixels[l_Index++];
            p_Green = m_Pixels[l_Index++];
            p_Red = m_Pixels[l_Index++];
            p_Alpha = m_Pixels[l_Index];
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

                /*
               
                    if ((m_Pixels[l_Index] - p_EvaporateStep) < 0)
                    {
                        m_Pixels[l_Index] = 0;
                    }
                    else
                    {
                        m_Pixels[l_Index] = m_Pixels[l_Index];
                    }
                */

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

                m_Pixels[l_Index] = (byte)Math.Max(0, l_DiffusedValue);*/

                l_Index++;
            }
        }
        
        /// <summary>
        /// Blur and Evaporate each pixel depending on it's neighborhoods.
        /// </summary>
        public void BlurAndEvaporateAllPixel(float p_EvaporateStep, float p_BlurStep)
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
            // Create the WriteableBitmap.
            WriteableBitmap l_WritableBitmap = new WriteableBitmap(
                m_Width, m_Height, p_DpiX, p_DpiY,
                PixelFormats.Bgra32, null);

            /// Load the pixel data.
            Int32Rect l_Rect = new Int32Rect(0, 0, m_Width, m_Height);
            l_WritableBitmap.WritePixels(l_Rect, m_Pixels, m_Stride, 0);

            /// Return the bitmap.
            return l_WritableBitmap;
        }
    }
}