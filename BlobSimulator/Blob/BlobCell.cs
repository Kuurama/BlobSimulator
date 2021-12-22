using System;
using System.Drawing;
using BlobSimulator.Map;

namespace BlobSimulator.Blob
{
    public class BlobCell
    {
        //private readonly List<BlobVectorFormat> m_BlobVectors = new List<BlobVectorFormat>();
        private readonly Color m_Color = Color.DeepSkyBlue;
        private readonly float m_Speed, m_TurnSpeed;
        private float m_Angle;
        private float m_SensorAngleSpacing = 45f;
        private int m_SensorSize = 2, m_SensorOffsetDst = 3;

        private float m_PosX;
        private float m_PosY;

        public BlobCell(float p_PosX, float p_PosY, float p_Angle, float p_Speed, float p_TurnSpeed)
        {
            m_PosX = p_PosX;
            m_PosY = p_PosY;
            m_Angle = p_Angle;
            m_Speed = p_Speed;
            m_TurnSpeed = p_TurnSpeed;
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

            /*if (m_BlobVectors.Count <= 0)
            {
                /// Adding the first BlobCell starting position.
                m_BlobVectors.Add(new BlobVectorFormat { m_PosX = m_PosX, m_PosY = m_PosY, m_Angle = m_Angle, m_DirectionX = l_DirectionX, m_DirectionY = l_DirectionY, m_StepX = l_NewPosX - m_PosX, m_StepY = l_NewPosY - m_PosY });
            }
            else
            {
                if (Math.Abs(m_BlobVectors[^1].m_Angle - m_Angle) > 0.001)
                    /// Adding the new coordonate if the BlobCell shift direction.
                    m_BlobVectors.Add(new BlobVectorFormat { m_PosX = m_PosX, m_PosY = m_PosY, m_Angle = m_Angle, m_DirectionX = l_DirectionX, m_DirectionY = l_DirectionY, m_StepX = l_NewPosX - m_PosX, m_StepY = l_NewPosY - m_PosY });
            }*/

            /// Replacing the old stored position with the new one.
            m_PosX = l_NewPosX;
            m_PosY = l_NewPosY;
        }

        public void FollowTrail(TrailMap p_TrailMap, Random p_Random)
        {
            float l_WeightForward = Sense(p_TrailMap, 0);
            float l_WeightLeft = Sense(p_TrailMap, m_SensorAngleSpacing);
            float l_WeightRight = Sense(p_TrailMap, -m_SensorAngleSpacing);

            float l_RandomSteerStrength = (float)p_Random.NextDouble();

            if (l_WeightForward > l_WeightLeft && l_WeightForward > l_WeightRight)
            {
                m_Angle += 0;
            }
            else if (l_WeightForward < l_WeightLeft && l_WeightForward < l_WeightRight)
            {
                m_Angle += (l_RandomSteerStrength - 0.5f) * 2 * m_TurnSpeed;
            }
            else if (l_WeightRight > l_WeightLeft)
            {
                m_Angle -= l_RandomSteerStrength * m_TurnSpeed;
            }
            else if (l_WeightLeft > l_WeightRight)
            {
                m_Angle += l_RandomSteerStrength * m_TurnSpeed;
            }
        }

        private float Sense(TrailMap p_TrailMap, float p_SensorAngleOffset)
        {
            float l_SensorAngle = m_Angle + p_SensorAngleOffset;
            float l_SensorDirX = (float)Math.Cos(l_SensorAngle);
            float l_SensorDirY = (float)Math.Sin(l_SensorAngle);
            int l_SensorCentreX = (int)(m_PosX + l_SensorDirX * m_SensorOffsetDst);
            int l_SensorCentreY = (int)(m_PosY + l_SensorDirY * m_SensorOffsetDst);
            float l_Sum = 0;

            for (int l_OffsetX = -m_SensorSize; l_OffsetX <= m_SensorSize; l_OffsetX++)
            {
                for (int l_OffsetY = -m_SensorSize; l_OffsetY <= m_SensorSize; l_OffsetY++)
                {
                    int l_PosX = l_SensorCentreX + l_OffsetX;
                    int l_PosY = l_SensorCentreY + l_OffsetY;
                    if (l_PosX is >= 0 and < BlobSimulatorWindow.SIM_WIDTH && l_PosY is >= 0 and < BlobSimulatorWindow.SIM_HEIGHT)
                    {
                        Color l_Color = p_TrailMap.m_BitMap.GetPixel(l_PosX, l_PosY);
                        l_Sum += (l_Color.B + l_Color.G + l_Color.R) / 3f;
                    }
                }
            }

            return l_Sum;
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