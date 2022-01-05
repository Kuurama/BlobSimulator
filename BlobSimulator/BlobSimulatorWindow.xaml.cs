using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using BlobSimulator.Blob;
using BlobSimulator.Map;
using BlobSimulator.Salt;

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
            const int POS_X = SIM_WIDTH / 2;
            const int POS_Y = SIM_HEIGHT / 2;

            //m_BlobCount = 1000000 / 6;
            m_BlobCount = 10000;
            const float BLOB_SPEED = 1.00f;
            const float BLOB_TURN_SPEED = 0.4f;
            const float SENSOR_ANGLE_SPACING = 45f;
            const int SENSOR_SIZE = 1, SENSOR_OFFSET_DST = 20;
            const int SPAWN_RADIUS = 12;
            const bool SAVE_DIRECTION = false;
            Color l_BlobColor = Color.DeepSkyBlue;
            //Color l_BlobColor = Color.Gray;

            m_BlobCells = new BlobCell[m_BlobCount];
            BlobController.CreateBlobGroup(m_BlobCells, POS_X, POS_Y, SPAWN_RADIUS, BLOB_SPEED, BLOB_TURN_SPEED, SENSOR_ANGLE_SPACING, SENSOR_SIZE, SENSOR_OFFSET_DST, l_BlobColor, SAVE_DIRECTION, m_Random);
            
            m_SaltCount = 200;
            m_Salts = new Salt.Salt[m_SaltCount];
            Color l_SaltColor = Color.White;
            BlockListColor = new Color[1];g
            BlockListColor[0] = l_SaltColor;

            int l_MaxSaltSize = 20;
            SaltController.CreateSaltGroup(m_Salts, 0,  SIM_WIDTH, 0, SIM_HEIGHT, l_MaxSaltSize, Color.White, m_Random);
            
            /// Starts the Render Loop.
            l_RenderTimer.Start();

            m_ProcessMap = true;
            m_ProcessMapLoopTimeOut = 2;

            Thread l_ProcessTrailMapThread = new Thread(ProcessTrailMap);
            l_ProcessTrailMapThread.Start();

            m_BLobCountTextBlock.Text = $"BlobCount: {m_BlobCount}";

            Task.Run(Update);
        }


        private void Update()
        {
            while (true)
            {
                m_TPS++;

                Parallel.ForEach(m_Salts, new ParallelOptions { MaxDegreeOfParallelism = 16 }, p_Salt =>
                {
                    p_Salt.Draw(m_TrailMap);
                });
                
                Parallel.ForEach(m_BlobCells, new ParallelOptions { MaxDegreeOfParallelism = 24 }, p_BlobCell =>
                {
                    Random? l_Random = m_TLSRandom.Value;
                    p_BlobCell.FollowTrail(m_TrailMap, l_Random);
                    p_BlobCell.Move(l_Random);
                    p_BlobCell.Draw(m_TrailMap);
                });

                if (!m_UncappedTPS)
                {
                    Thread.Sleep(SIM_TPS);
                }
                else
                {
                    Thread.Yield(); /// Doing this would uncap fps and give better performances.
                }
            }
            // ReSharper disable once FunctionNeverReturns
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

            // Update the Image source (Which is being displayed).

            m_Image.Source = m_TrailMap.m_BitMap.MakeBitmap(96, 96);
        }

        private void ProcessTrailMap()
        {
            while (m_ProcessMap)
            {
                m_TrailMap.m_BitMap.BlurAndEvaporateAllPixel(5f, 0.2f);
                Thread.Sleep(m_ProcessMapLoopTimeOut);
            }
        }
    }
}