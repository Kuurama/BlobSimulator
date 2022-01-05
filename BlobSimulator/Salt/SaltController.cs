using System;
using System.Drawing;

namespace BlobSimulator.Salt
{
    public class SaltController
    {
        public static void CreateSaltGroup(Salt[] p_Salts, int p_MinPosX ,int p_MaxPosX, int p_MinPosY, int p_MaxPosY, int p_MaxSize, Color p_Color, Random p_Random)
        {
            /// Instanciate the Salts.
            for (int l_I = 0; l_I < p_Salts.Length; l_I++)
            {
                int l_SaltPosX = p_Random.Next(p_MinPosX, p_MaxPosX + 1);
                int l_SaltPosY = p_Random.Next(p_MinPosY, p_MaxPosY + 1);
                
                p_Salts[l_I] = new Salt(l_SaltPosX, l_SaltPosY, (int)(p_MaxSize * p_Random.NextDouble()), p_Color);
            }
        }
    }
}