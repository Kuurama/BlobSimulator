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
                return p_A+(p_B-p_A)*p_T;
            //else
            //    return (float)(p_B-(p_B-p_A)*(1.0-p_T));
        }
    }
}