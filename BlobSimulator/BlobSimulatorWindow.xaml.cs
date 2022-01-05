using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using BlobSimulator.Blob;
using BlobSimulator.Map;

namespace BlobSimulator
{
    /// <summary>
    ///     Interaction logic for BlobSimulatorWindow.xaml
    /// </summary>
    public partial class BlobSimulatorWindow
    {
        public BlobSimulatorWindow()
        {
            InitializeComponent();

            /// Needed Setup Method ///
            m_Random = new Random();
            m_TrailMap = new TrailMap(SIM_WIDTH, SIM_HEIGHT);
            m_Stopwatch = new Stopwatch();
            m_Stopwatch.Start();
            ///////////////////////////

            DispatcherTimer l_RenderTimer = new DispatcherTimer();

            l_RenderTimer.Interval = TimeSpan.FromTicks(RENDER_TPS); /// Sets m_RenderTimer.Tick to loop at RENDER_TPS.
            l_RenderTimer.Tick += Render;

            /// Sets randomly the blob's position.
            const int POS_X = SIM_WIDTH / 2;
            const int POS_Y = SIM_HEIGHT / 2;

            m_BlobCount = 1000000/2;
            const float BLOB_SPEED = 1.00f;
            const float BLOB_TURN_SPEED = 0.4f;
            const float SENSOR_ANGLE_SPACING = 45f;
            const int SENSOR_SIZE = 1, SENSOR_OFFSET_DST = 20;
            const int SPAWN_RADIUS = 12;
            const bool SAVE_DIRECTION = false;
            Color l_Color = Color.DeepSkyBlue;
            
            m_BlobCellsList = new BlobCell[m_BlobCount];
            BlobController.CreateBlobGroup(m_BlobCellsList, POS_X, POS_Y, SPAWN_RADIUS, BLOB_SPEED, BLOB_TURN_SPEED, SENSOR_ANGLE_SPACING, SENSOR_SIZE, SENSOR_OFFSET_DST, l_Color, SAVE_DIRECTION, m_Random);

            /// Starts the Render Loop.
            l_RenderTimer.Start();

            m_ProcessMap = true;
            m_ProcessMapLoopTimeOut = 2;

            Thread l_ProcessTrailMapThread = new Thread(ProcessTrailMap);
            l_ProcessTrailMapThread.Start();

            m_BLobCountTextBlock.Text = $"BlobCount: {m_BlobCount}";

            Task.Run(() => Update(null, null!));
        }

        private readonly ThreadLocal<Random> m_TLSRandom = new ThreadLocal<Random>(() => new Random());


        private void Update(object? p_Sender, EventArgs p_EventArgs)
        {
            while (true)
            {
                m_TPS++;

                Parallel.ForEach(m_BlobCellsList, new ParallelOptions() { MaxDegreeOfParallelism = 24 }, l_BlobCell =>
                {
                      var l_Random = m_TLSRandom.Value;
                      l_BlobCell.FollowTrail(m_TrailMap, l_Random);
                      l_BlobCell.Move(l_Random);
                      l_BlobCell.Draw(m_TrailMap);
                });

                System.Threading.Thread.Yield();
            }
        }

        private void Render(object? p_Sender, EventArgs p_EventArgs)
        {
            m_FPS++;
            if (m_Stopwatch.ElapsedMilliseconds  > 1000)
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