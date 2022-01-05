using System;
using System.Collections.Generic;
using System.Drawing;

namespace BlobSimulator.Salt
{
    public class SaltController
    {
        public static List<Salt> CreateSaltGroup(int p_SaltCount, int p_PosX, int p_PosY, int p_SpawnRadius, int p_Size, Color p_Color, Random p_Random)
        {
            List<Salt> l_BlobCells = new List<Salt>();

            /// Instanciate the Salts.
            for (int l_I = 0; l_I < p_SaltCount; l_I++)
            {
                int l_SaltPosX;
                int l_SaltPosY;
                do
                {
                    l_SaltPosX = p_Random.Next(p_PosX - p_SpawnRadius, p_PosX + p_SpawnRadius + 1);
                    l_SaltPosY = p_Random.Next(p_PosY - p_SpawnRadius, p_PosY + p_SpawnRadius + 1);
                } while ((l_SaltPosX-p_PosX)*(l_SaltPosX-p_PosX) + (l_SaltPosY-p_PosY)*(l_SaltPosY-p_PosY) >= p_SpawnRadius*p_SpawnRadius);

                l_BlobCells.Add(new Salt(l_SaltPosX, l_SaltPosY, p_Size, p_Color));
            }

            return l_BlobCells;
        }
    }
}