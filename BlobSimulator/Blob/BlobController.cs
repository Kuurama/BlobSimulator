using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BlobSimulator.Blob
{
    public class BlobController
    {
        /// <summary>
        ///     Adds BlobCells to the given array considering the different parameters.
        /// </summary>
        /// <param name="p_BlobCells"></param>
        /// <param name="p_PosX"></param>
        /// <param name="p_PosY"></param>
        /// <param name="p_SpawnRadius"></param>
        /// <param name="p_Speed"></param>
        /// <param name="p_TurnSpeed"></param>
        /// <param name="p_SensorAngleSpacing"></param>
        /// <param name="p_SensorSize"></param>
        /// <param name="p_SensorOffsetDst"></param>
        /// <param name="p_Color"></param>
        /// <param name="p_SaveDirection"></param>
        /// <param name="p_Random"></param>
        public static void CreateBlobGroup(BlobCell[] p_BlobCells, int p_PosX, int p_PosY, int p_SpawnRadius, float p_Speed, float p_TurnSpeed, float p_SensorAngleSpacing, int p_SensorSize, int p_SensorOffsetDst, Color p_Color, bool p_SaveDirection, Random p_Random)
        {
            for (int l_I = 0; l_I < p_BlobCells.Length; l_I++)
            {
                int l_BlobPosX;
                int l_BlobPosY;
                do
                {
                    l_BlobPosX = p_Random.Next(p_PosX - p_SpawnRadius, p_PosX + p_SpawnRadius + 1);
                    l_BlobPosY = p_Random.Next(p_PosY - p_SpawnRadius, p_PosY + p_SpawnRadius + 1);
                } while ((l_BlobPosX - p_PosX) * (l_BlobPosX - p_PosX) + (l_BlobPosY - p_PosY) * (l_BlobPosY - p_PosY) >= p_SpawnRadius * p_SpawnRadius);

                p_BlobCells[l_I] = new BlobCell(l_BlobPosX, l_BlobPosY, (float)(p_Random.NextDouble() * 2 * Math.PI), p_Speed, p_TurnSpeed, p_SensorAngleSpacing, p_SensorSize, p_SensorOffsetDst, p_Color, p_SaveDirection);
            }
        }

        /// <summary>
        ///     Return a BlobCell List from the given Array which didn't make to the final destination.
        /// </summary>
        /// <param name="p_BlobCells"></param>
        public static List<BlobCell> SortBlobByDestination(BlobCell[] p_BlobCells, float p_DestinationPosX, float p_DestinationPosY, float p_DestinationMargin)
        {
            List<BlobCell> l_SortedBlobCells = new List<BlobCell>();


            for (int l_I = 0; l_I < p_BlobCells.Length; l_I++)
            {
                if (!p_BlobCells[l_I].m_BlobVectors.Any()) continue;
                bool l_BlobVectorMatch = false;

                foreach (BlobVectorFormat l_BlobVector in p_BlobCells[l_I].m_BlobVectors
                    .Where(p_BlobVector => !(p_BlobVector.m_PosX < p_DestinationPosX - p_DestinationMargin) && !(p_BlobVector.m_PosX > p_DestinationPosX + p_DestinationMargin))
                    .Where(p_BlobVector => p_BlobVector.m_PosY > p_DestinationPosY - p_DestinationMargin && p_BlobVector.m_PosY < p_DestinationPosY + p_DestinationMargin))
                {
                    l_BlobVectorMatch = true; /// One of the blob position match with the Destination.
                }

                if (l_BlobVectorMatch) l_SortedBlobCells.Add(p_BlobCells[l_I]);
            }

            return l_SortedBlobCells;
        }

        public static List<BlobCompletedPath> GiveCompleteBlobPathList(List<BlobVectorFormat> p_BlobVectors, int p_BasePosX, int p_BasePosY, int p_BasePosMargin, int p_DestinationPosX, int p_DestinationPosY, int p_DestinationMargin)
        {
            List<List<BlobVectorFormat>> l_BlobVectorFormats = new List<List<BlobVectorFormat>>();
            if (!p_BlobVectors.Any()) return new List<BlobCompletedPath>();

            List<BlobVectorFormat> l_BlobVectorList = new List<BlobVectorFormat>();

            bool l_IsFromDestination = false;

            foreach (var l_BlobVector in p_BlobVectors)
                if (StaticFunction.IsInArea((int)l_BlobVector.m_PosX, (int)l_BlobVector.m_PosY, p_BasePosX, p_BasePosY, p_BasePosMargin))
                {
                    if (l_BlobVectorList.Any()) /// Mean there is at least 1 vector.
                    {
                        l_BlobVectorList = new List<BlobVectorFormat> { l_BlobVector }; /// Reset the VectorList because the blob didn't leaved the base yet (but obviously changed direction).
                        l_IsFromDestination = false;
                    }
                    else
                    {
                        l_BlobVectorList.Add(l_BlobVector); /// Blob starting from the Base.
                    }
                }
                else /// The blob isn't in the Base Area.
                {
                    if (StaticFunction.IsInArea((int)l_BlobVector.m_PosX, (int)l_BlobVector.m_PosY, p_DestinationPosX, p_DestinationPosY, p_DestinationMargin)) /// If the blob is at the destination.
                    {
                        if (!l_IsFromDestination) /// Prevent next vector which are in the destination to be added.
                        {
                            l_IsFromDestination = true;
                            l_BlobVectorList.Add(l_BlobVector);
                            l_BlobVectorFormats.Add(l_BlobVectorList);
                        }
                    }
                    else /// The blob isn't at the destination (yet).
                    {
                        if (!l_IsFromDestination) /// Prevent next vector which come from the destination to be added.
                            l_BlobVectorList.Add(l_BlobVector);
                    }
                }

            return (from l_BlobVectors in l_BlobVectorFormats
                let l_Distance = l_BlobVectors.Sum(p_Vector => Math.Abs(p_Vector.m_StepX) + Math.Abs(p_Vector.m_StepY))
                select new BlobCompletedPath { m_BlobVectors = l_BlobVectors, m_Distance = l_Distance }).ToList();
        }


        /// <summary>
        /// Merge all Path depending on their shared node and the lower distance. DO NOT USE. BAD, GARBAGE.
        /// </summary>
        /// <param name="p_BlobVectors"></param>
        /// <param name="p_BasePosX"></param>
        /// <param name="p_BasePosY"></param>
        /// <param name="p_BasePosMargin"></param>
        /// <param name="p_DestinationPosX"></param>
        /// <param name="p_DestinationPosY"></param>
        /// <param name="p_DestinationMargin"></param>
        /// <returns></returns>
        private static List<BlobCompletedPath> MergeAndSortAllPathByNode(List<BlobCompletedPath> p_BlobCompletedPaths, int p_SimilarityParameter, int p_Margin, int p_MaxNodeCount)
        {
            List<BlobVectorFormat> l_TotalVectorList = new List<BlobVectorFormat>();
            foreach (var l_BlobPath in p_BlobCompletedPaths)
            {
                if (l_BlobPath.m_BlobVectors != null)
                {
                    l_TotalVectorList.AddRange(l_BlobPath.m_BlobVectors);
                }
            }

            List<BlobVectorFormat> l_SimilarVectorList = (
                from l_BlobVector in l_TotalVectorList
                from l_SearchedVector in l_TotalVectorList
                where !(l_BlobVector.m_PosX - l_SearchedVector.m_PosX >= p_SimilarityParameter)
                where !(l_BlobVector.m_PosY - l_SearchedVector.m_PosY >= p_SimilarityParameter)
                select l_SearchedVector).ToList();

            List<BlobCompletedPath> l_CompletedPaths = new List<BlobCompletedPath>();
            foreach (var l_SimilarVector in l_SimilarVectorList)
            {
                List<BlobCompletedPath> l_AllPathBetweenTwoNode = new List<BlobCompletedPath>();
                foreach (var l_SimilarSecondVector in l_SimilarVectorList)
                {
                    l_AllPathBetweenTwoNode.AddRange(GiveCompleteBlobPathList(l_TotalVectorList, (int)l_SimilarVector.m_PosX, (int)l_SimilarVector.m_PosY, p_Margin, (int)l_SimilarSecondVector.m_PosX, (int)l_SimilarSecondVector.m_PosY, p_Margin));
                }

                l_CompletedPaths.Add(GiveShortestDestinationPath(l_AllPathBetweenTwoNode));
                if (l_CompletedPaths.Count >= p_MaxNodeCount)
                {
                    break;
                }
            }


            return l_CompletedPaths;
        }


        public static BlobCompletedPath GiveShortestDestinationPath(List<BlobCompletedPath> p_BlobPathList)
        {
            BlobCompletedPath l_BestBlobPath = new BlobCompletedPath();

            float l_MinDistance = float.MaxValue;
            foreach (var l_BlobPath in p_BlobPathList)
                if (l_BlobPath.m_Distance < l_MinDistance)
                {
                    l_BestBlobPath = l_BlobPath;
                    l_MinDistance = l_BlobPath.m_Distance;
                }

            l_BestBlobPath = new BlobCompletedPath() { m_BlobVectors = l_BestBlobPath.m_BlobVectors, m_Distance = l_BestBlobPath.m_Distance };
            return l_BestBlobPath;
        }

        public class BlobCompletedPath
        {
            public List<BlobVectorFormat>? m_BlobVectors;
            public float m_Distance;
        }
    }
}