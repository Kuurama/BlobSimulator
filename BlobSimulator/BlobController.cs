using System;
using System.Collections.Generic;

namespace BlobSimulator
{
    public static class BlobController
    {
        public static List<BlobCell> CreateBlobGroup(int p_BlobCellCount, int p_PosX, int p_PosY, float p_Speed, Random p_Random)
        {
            List<BlobCell> l_BlobCells = new List<BlobCell>();

            /// Instanciate the Blobs.
            for (int l_I = 0; l_I <= p_BlobCellCount; l_I++)
            {
                int l_Vx;
                int l_Vy;
                do
                {
                    l_Vx = p_Random.Next(0, 2);
                    l_Vy = p_Random.Next(0, 2);

                } while (l_Vx == 0 && l_Vy == 0);
                l_BlobCells.Add( new BlobCell(p_PosX, p_PosY, (float)(p_Random.NextDouble() * 2 * Math.PI), p_Speed) );
            }
            
            return l_BlobCells;
        }
    }
}