using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using BlobSimulator.Blob;
using BlobSimulator.Map;
using BlobSimulator.Salt;
using Color = System.Drawing.Color;

namespace BlobSimulator
{
    /// <summary>
    ///     Interaction logic for BlobSimulatorWindow.xaml
    /// </summary>
    public partial class BlobSimulatorWindow
    {
        private readonly ThreadLocal<Random> m_TLSRandom = new ThreadLocal<Random>(() => new Random());

        public BlobSimulatorWindow()
        {
            InitializeComponent();

            /// Needed Setup Method ///
            m_Random = new Random();
            m_TrailMap = new TrailMap(SIM_WIDTH, SIM_HEIGHT);
            m_Stopwatch = new Stopwatch();
            m_Stopwatch.Start();
            ///////////////////////////

            DispatcherTimer l_RenderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromTicks(SIM_FPS) /// Sets m_RenderTimer.Tick to loop at SIM_FPS.
            };

            l_RenderTimer.Tick += Render;

            m_UncappedTPS = false; /// Set the uncapped TPS to false.

            /// Sets randomly the blob's position.
            m_PosX = SIM_WIDTH - SIM_WIDTH / 7;
            m_PosY = SIM_HEIGHT - SIM_HEIGHT / 6;

            //m_BlobCount = 1000000 / 6;
            m_BlobCount = 25000;
            const float BLOB_SPEED = 1.00f;
            const float BLOB_TURN_SPEED = 0.4f;
            const float SENSOR_ANGLE_SPACING = 45f;
            const int SENSOR_SIZE = 1, SENSOR_OFFSET_DST = 20;
            m_SpawnRadius = 25;


            const bool SAVE_DIRECTION = true; /// Make the BlobCells able to save their Path.

            /// PathFindingUI Colors
            m_PathColor = Color.Crimson;
            m_DestinationColor = Colors.Crimson;
            m_SpawnColor = Colors.Lime;


            Color l_BlobColor = Color.DeepSkyBlue;
            //Color l_BlobColor = Color.Gray;

            /// Destination ///
            m_DestinationPosX = 60;
            m_DestinationPosY = 60;
            m_DestinationMargin = 30;
            /// ///////////////

            m_BlobCells = new BlobCell[m_BlobCount];
            BlobController.CreateBlobGroup(m_BlobCells, m_PosX, m_PosY, m_SpawnRadius, BLOB_SPEED, BLOB_TURN_SPEED, SENSOR_ANGLE_SPACING, SENSOR_SIZE, SENSOR_OFFSET_DST, l_BlobColor, SAVE_DIRECTION, m_Random);

            m_SaltCount = 50;
            m_Salts = new Salt.Salt[m_SaltCount];
            Color l_SaltColor = Color.White;
            m_BlockListColor = new Color[1];

            m_BlockListColor[0] = l_SaltColor;

            const int MAX_SALT_SIZE = 25;
            SaltController.CreateSaltGroup(m_Salts, 0, SIM_WIDTH, 0, SIM_HEIGHT, MAX_SALT_SIZE, l_SaltColor, m_PosX, m_PosY, m_SpawnRadius, m_Random);


            /// Starts the Render Loop.
            l_RenderTimer.Start();

            m_ProcessMap = true;
            m_ProcessMapLoopTimeOut = 5;

            Thread l_ProcessTrailMapThread = new Thread(ProcessTrailMap);
            l_ProcessTrailMapThread.Start();

            m_BLobCountTextBlock.Text = $"BlobCount: {m_BlobCount}";

            Task.Run(Update);
        }


        private void Update()
        {
            while (m_BoolUpdateLoop)
            {
                m_TPS++;

                Parallel.ForEach(m_Salts, new ParallelOptions { MaxDegreeOfParallelism = 12 }, p_Salt => { p_Salt.Draw(m_TrailMap); });

                Parallel.ForEach(m_BlobCells, new ParallelOptions { MaxDegreeOfParallelism = 24 }, p_BlobCell =>
                {
                    Random? l_Random = m_TLSRandom.Value;
                    p_BlobCell.FollowTrail(m_TrailMap, l_Random);
                    p_BlobCell.Move(l_Random);
                    p_BlobCell.Draw(m_TrailMap);
                });

                if (!m_UncappedTPS)
                    Thread.Sleep(SIM_TPS);
                else
                    Thread.Yield(); /// Doing this would uncap fps and give better performances.
            }
        }

        private void Render(object? p_Sender, EventArgs p_EventArgs)
        {
            m_FPS++;
            if (m_Stopwatch.ElapsedMilliseconds > 1000)
            {
                m_FPSTextBlock.Text = $"FPS: {m_FPS}";
                m_TPSTextBlock.Text = $"TPS: {m_TPS}";

                m_TPS = 0;
                m_FPS = 0;

                m_Stopwatch.Restart();
            }

            //m_TrailMap.m_BitMap.DrawSquare(m_DestinationPosX - m_DestinationMargin, m_DestinationPosY - m_DestinationMargin, m_DestinationPosX + m_DestinationMargin, m_DestinationPosY + m_DestinationMargin, false, 1, m_DestinationColor);

            // Update the Image source (Which is being displayed).
            m_Image.Source = m_TrailMap.m_BitMap.MakeBitmap(96, 96);
        }

        private void ProcessTrailMap()
        {
            while (m_ProcessMap)
            {
                m_TrailMap.m_BitMap.BlurAndEvaporateAllPixel(5f, 0.3f);

                Thread.Sleep(m_ProcessMapLoopTimeOut);
            }
        }

        private void ProcessResult()
        {
            List<BlobCell> l_ProcessedBlobCells = BlobController.SortBlobByDestination(m_BlobCells, m_DestinationPosX, m_DestinationPosY, m_DestinationMargin);

            //m_TrailMap.m_BitMap.BlurAndEvaporateAllPixel(255f, 0f); /// Clear the map.
            List<BlobController.BlobCompletedPath> l_CompletedPaths = new List<BlobController.BlobCompletedPath>();
            foreach (var l_ProcessedBlobCell in l_ProcessedBlobCells)
            {
                List<BlobController.BlobCompletedPath> l_CurrentBlobCompletedPaths = BlobController.GiveCompleteBlobPathList(l_ProcessedBlobCell.m_BlobVectors, m_PosX, m_PosY, m_SpawnRadius, m_DestinationPosX, m_DestinationPosY, m_DestinationMargin);
                foreach (var l_Path in l_CurrentBlobCompletedPaths)
                    l_CompletedPaths.Add(l_Path);
            }

            BlobCell.DrawVectorStatic(BlobController.GiveShortestDestinationPath(l_CompletedPaths).m_BlobVectors, m_PathColor, m_TrailMap);
        }
    }
}