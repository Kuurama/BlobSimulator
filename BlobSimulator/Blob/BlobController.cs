using System;
using System.Drawing;

namespace BlobSimulator.Blob
{
    public class BlobController
    {
        public static void CreateBlobGroup(BlobCell[] p_Dest, int p_PosX, int p_PosY, int p_SpawnRadius, float p_Speed, float p_TurnSpeed, float p_SensorAngleSpacing, int p_SensorSize, int p_SensorOffsetDst, Color p_Color, bool p_SaveDirection, Random p_Random)
        {
            for (int l_I = 0; l_I < p_Dest.Length; l_I++)
            {
                int l_BlobPosX;
                int l_BlobPosY;
                do
                {
                    l_BlobPosX = p_Random.Next(p_PosX - p_SpawnRadius, p_PosX + p_SpawnRadius + 1);
                    l_BlobPosY = p_Random.Next(p_PosY - p_SpawnRadius, p_PosY + p_SpawnRadius + 1);
                } while ((l_BlobPosX - p_PosX) * (l_BlobPosX - p_PosX) + (l_BlobPosY - p_PosY) * (l_BlobPosY - p_PosY) >= p_SpawnRadius * p_SpawnRadius);

                p_Dest[l_I] = new BlobCell(l_BlobPosX, l_BlobPosY, (float)(p_Random.NextDouble() * 2 * Math.PI), p_Speed, p_TurnSpeed, p_SensorAngleSpacing, p_SensorSize, p_SensorOffsetDst, p_Color, p_SaveDirection);
            }
        }
    }
}