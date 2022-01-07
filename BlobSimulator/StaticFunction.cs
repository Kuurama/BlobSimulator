using System;
using System.Collections.Generic;

namespace BlobSimulator
{
    public static class StaticFunction
    {
        public static List<int> NumberSplit(int p_X, int p_N)
        {
            List<int> l_Ints = new List<int>();
            /// If we cannot split the number into exactly 'N' parts
            if (p_X < p_N)
            {
                l_Ints.Add(p_X); /// Return the value as we cannot cut it.
            }

            // If x % n == 0 then the minimum difference is 0 and all numbers are x / n
            else if (p_X % p_N == 0)
            {
                for (int l_I = 0; l_I < p_N; l_I++)
                    l_Ints.Add(p_X / p_N);
            }
            else
            {
                int l_Z = p_N - p_X % p_N;
                int l_P = p_X / p_N;
                for (int l_I = 0; l_I < p_N; l_I++)
                    if (l_I >= l_Z)
                        l_Ints.Add(l_P + 1);
                    else
                        l_Ints.Add(l_P);
            }

            return l_Ints;
        }

        public static float Lerp(float p_A, float p_B, float p_T)
        {
            //if (p_T <= 0.5)
            return p_A + (p_B - p_A) * p_T;
            //else
            //    return (float)(p_B-(p_B-p_A)*(1.0-p_T));
        }

        public static float Scale(float p_Value, float p_Min, float p_Max, float p_MinScale, float p_MaxScale)
        {
            float l_Scaled = p_MinScale + (p_Value - p_Min) / (p_Max - p_Min) * (p_MaxScale - p_MinScale);
            return l_Scaled;
        }

        public static float GetMaxStep(float p_ValueA, float p_ValueB, float p_MinValue, float p_MaxValue)
        {
            float l_MaxStep = Math.Abs(p_ValueB - p_ValueA);

            if (l_MaxStep < p_MinValue)
                return p_MinValue;

            if (l_MaxStep > p_MaxValue)
                return p_MaxValue;

            return l_MaxStep;
        }
        
        public static bool IsInArea(int p_PosX, int p_PosY, int p_AreaX, int p_AreaY, int p_AreaMargin)
        {
            return p_PosX >= p_AreaX - p_AreaMargin && p_PosX <= p_AreaX + p_AreaMargin && p_PosY >= p_AreaY - p_AreaMargin && p_PosY <= p_AreaY + p_AreaMargin;
        }
    }
}