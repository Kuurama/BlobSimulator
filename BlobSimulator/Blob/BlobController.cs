using System;
using System.Collections.Generic;
using System.Drawing;

namespace BlobSimulator.Blob
{
    public class BlobController
    {
        public static List<BlobCell> CreateBlobGroup(int p_BlobCellCount, int p_PosX, int p_PosY, int p_SpawnRadius, float p_Speed, float p_TurnSpeed, float p_SensorAngleSpacing, int p_SensorSize, int p_SensorOffsetDst, Color p_Color, Random p_Random)
        {
            List<BlobCell> l_BlobCells = new List<BlobCell>();

            /// Instanciate the Blobs.
            for (int l_I = 0; l_I < p_BlobCellCount; l_I++)
            {
                int l_BlobPosX;
                int l_BlobPosY;
                do
                {
                    l_BlobPosX = p_Random.Next(p_PosX - p_SpawnRadius, p_PosX + p_SpawnRadius + 1);
                    l_BlobPosY = p_Random.Next(p_PosY - p_SpawnRadius, p_PosY + p_SpawnRadius + 1);
                } while ((l_BlobPosX-p_PosX)*(l_BlobPosX-p_PosX) + (l_BlobPosY-p_PosY)*(l_BlobPosY-p_PosY) >= p_SpawnRadius*p_SpawnRadius);

                l_BlobCells.Add(new BlobCell(l_BlobPosX, l_BlobPosY, (float)(p_Random.NextDouble() * 2 * Math.PI), p_Speed, p_TurnSpeed, p_SensorAngleSpacing, p_SensorSize, p_SensorOffsetDst, p_Color));
            }

            return l_BlobCells;
        }
    }
}