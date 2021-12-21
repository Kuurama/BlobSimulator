using System;
using System.Collections.Generic;
using System.Drawing;
using BlobSimulator.Map;

namespace BlobSimulator.Blob
{
    public class BlobCell
    {
        private readonly List<BlobVectorFormat> m_BlobVectors = new List<BlobVectorFormat>();
        private readonly Color m_Color = Color.GreenYellow;
        private readonly float m_Speed;
        private float m_Angle;
        private float m_PosX;
        private float m_PosY;

        public BlobCell(float p_PosX, float p_PosY, float p_Angle, float p_Speed)
        {
            m_PosX = p_PosX;
            m_PosY = p_PosY;
            m_Speed = p_Speed;
            m_Angle = p_Angle;
        }

        /// <summary>
        ///     Moves the blob.
        /// </summary>
        public void Move(Random p_Random)
        {
            const int WIDTH = BlobSimulatorWindow.SIM_WIDTH;
            const int HEIGHT = BlobSimulatorWindow.SIM_HEIGHT;
            
            /// Calculate the Direction depending on the BlobCell's Angle.
            float l_DirectionX = (float)Math.Cos(m_Angle);
            float l_DirectionY = (float)Math.Sin(m_Angle);

            /// Set the new BlobCell's position using the calculated Direction.
            float l_NewPosX = m_PosX + l_DirectionX * m_Speed;
            float l_NewPosY = m_PosY + l_DirectionY * m_Speed;

            /// change the BlobCell's position and Angle if it touches the Simulation's window border.
            if (l_NewPosX is < 0 or >= WIDTH || l_NewPosY is < 0 or >= HEIGHT)
            {
                l_NewPosX = (float)Math.Min(WIDTH - 0.01, Math.Max(0, l_NewPosX));
                l_NewPosY = (float)Math.Min(HEIGHT - 0.01, Math.Max(0, l_NewPosY));
                m_Angle = (float)(p_Random.NextDouble() * 2 * Math.PI);
            }
            
            if (m_BlobVectors.Count <= 0)
            {
                /// Adding the first BlobCell starting position.
                m_BlobVectors.Add(new BlobVectorFormat { m_PosX = m_PosX, m_PosY = m_PosY, m_Angle = m_Angle, m_DirectionX = l_DirectionX, m_DirectionY = l_DirectionY, m_StepX = l_NewPosX - m_PosX, m_StepY = l_NewPosY - m_PosY });
            }
            else
            {
                if (Math.Abs(m_BlobVectors[^1].m_Angle - m_Angle) > 0.001)
                    /// Adding the new coordonate if the BlobCell shift direction.
                    m_BlobVectors.Add(new BlobVectorFormat { m_PosX = m_PosX, m_PosY = m_PosY, m_Angle = m_Angle, m_DirectionX = l_DirectionX, m_DirectionY = l_DirectionY, m_StepX = l_NewPosX - m_PosX, m_StepY = l_NewPosY - m_PosY });
            }

            /// Replacing the old stored position with the new one.
            m_PosX = l_NewPosX;
            m_PosY = l_NewPosY;
        }

        /// <summary>
        ///     Draws the BlobCell on the BitMap.
        /// </summary>
        public void Draw(TrailMap p_TrailMap)
        {
            p_TrailMap.m_BitMap.SetPixel((int)m_PosX, (int)m_PosY, m_Color);
        }
    }

    public class BlobVectorFormat
    {
        public float m_Angle;
        public float m_DirectionX;
        public float m_DirectionY;
        public float m_PosX;
        public float m_PosY;

        /// <summary>
        ///     the number of traveled pixel before m_PosX.
        /// </summary>
        public float m_StepX;

        /// <summary>
        ///     the number of traveled pixel before m_PosY.
        /// </summary>
        public float m_StepY;
    }
}