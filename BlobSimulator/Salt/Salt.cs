using System.Collections.Generic;
using System.Drawing;
using BlobSimulator.Map;

namespace BlobSimulator.Salt
{
    public class Salt /// Salt is a known obstacle for Blobs
    {
        private readonly Color m_Color;
        private readonly List<Position> m_SaltPixels;
        private int m_PosX;
        private int m_PosY;

        public Salt(int p_SaltPosX, int p_SaltPosY, int p_Size, Color p_Color)
        {
            m_PosX = p_SaltPosX;
            m_PosY = p_SaltPosY;
            m_Color = p_Color;

            m_SaltPixels = new List<Position>();

            for (int l_X = 0; l_X < BlobSimulatorWindow.SIM_WIDTH; l_X++)
            for (int l_Y = 0; l_Y < BlobSimulatorWindow.SIM_HEIGHT; l_Y++)
            {
                double l_Dx = l_X - p_SaltPosX;
                double l_Dy = l_Y - p_SaltPosY;
                double l_DistanceSquared = l_Dx * l_Dx + l_Dy * l_Dy;

                if (l_DistanceSquared <= p_Size * p_Size) m_SaltPixels.Add(new Position { m_X = l_X, m_Y = l_Y});
            }
        }

        /// <summary>
        ///     Draws the Salt on the BitMap.
        /// </summary>
        public void Draw(TrailMap p_TrailMap)
        {
            foreach (var l_Pixel in m_SaltPixels) p_TrailMap.m_BitMap.SetPixel((int)l_Pixel.m_X, (int)l_Pixel.m_Y, m_Color);
        }

        private class Position
        {
            public float m_X;
            public float m_Y;
        }
    }
}