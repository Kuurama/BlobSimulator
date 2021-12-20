using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace BlobSimulator
{
    /// A class to represent WriteableBitmap pixels in Bgra32 format.
    public class BitmapPixelMaker
    {
        /// The pixel array.
        private readonly byte[] m_Pixels;

        /// The number of bytes per row.
        private readonly int m_Stride;

        /// The bitmap's size.
        private readonly int m_Width;
        private readonly int m_Height;

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
        public void SetPixel(int p_X, int p_Y, byte p_Red, byte p_Green, byte p_Blue, byte p_Alpha)
        {
            int l_Index = p_Y * m_Stride + p_X * 4;
            m_Pixels[l_Index++] = p_Blue;
            m_Pixels[l_Index++] = p_Green;
            m_Pixels[l_Index++] = p_Red;
            // ReSharper disable once RedundantAssignment
            m_Pixels[l_Index++] = p_Alpha;
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