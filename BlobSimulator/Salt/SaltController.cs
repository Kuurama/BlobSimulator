using System;
using System.Drawing;

namespace BlobSimulator.Salt
{
    public class SaltController
    {
        public static void CreateSaltGroup(Salt[] p_Salts, int p_MinPosX, int p_MaxPosX, int p_MinPosY, int p_MaxPosY, int p_MaxSize, Color p_Color, int p_BlobPosX, int p_BlobPosY, int p_BlobSpawnRadius, Random p_Random)
        {
            /// Instanciate the Salts.
            for (int l_I = 0; l_I < p_Salts.Length; l_I++)
            {
                int l_SaltPosX;
                int l_SaltPosY;
                do
                {
                    l_SaltPosX = p_Random.Next(p_MinPosX, p_MaxPosX + 1);
                    l_SaltPosY = p_Random.Next(p_MinPosY, p_MaxPosY + 1);
                } while (StaticFunction.IsInArea(l_SaltPosX, l_SaltPosY, p_BlobPosX, p_BlobPosY, p_BlobSpawnRadius) || StaticFunction.IsInArea(p_BlobPosX, p_BlobPosY, l_SaltPosX, l_SaltPosY, p_MaxSize));

                p_Salts[l_I] = new Salt(l_SaltPosX, l_SaltPosY, (int)(p_MaxSize * p_Random.NextDouble()), p_Color);
            }
        }
    }
}