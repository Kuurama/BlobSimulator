using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Threading;

namespace BlobSimulator
{
    public class BlobController
    {
        public static List<BlobCell> CreateBlobGroup(int p_BlobCellCount, int p_PosX, int p_PosY, float p_Speed, Random p_Random)
        {
            List<BlobCell> l_BlobCells = new List<BlobCell>();

            /// Instanciate the Blobs.
            for (int l_I = 0; l_I < p_BlobCellCount; l_I++)
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

        /// Create the DispatcherTimers used to Make the Update and Render Loop.
        private readonly DispatcherTimer m_RenderTimer = new DispatcherTimer();
        private readonly DispatcherTimer m_UpdateTimer = new DispatcherTimer();
        
        private readonly List<BlobCell> m_BlobCells;
        public BlobController(List<BlobCell> p_BlobCells)
        {
            m_BlobCells = p_BlobCells;

            //Thread l_Thread = new Thread(StartTimersThread);
            //l_Thread.Start();
            Thread l_UpdateThread = new Thread(new ThreadStart(Update));
            Thread l_RenderThread = new Thread(new ThreadStart(Render));
            l_UpdateThread.Start();
            l_RenderThread.Start();
        }

        private void StartTimersThread()
        {
            m_UpdateTimer.Interval = TimeSpan.FromMilliseconds(600D / MainWindow.UPDATE_DELAY); /// Sets m_UpdateTimer.Tick to loop at UPDATE_DELAY.
            //m_UpdateTimer.Tick += Update;
            
            m_RenderTimer.Interval = TimeSpan.FromMilliseconds(600D / MainWindow.RENDER_DELAY); /// Sets m_RenderTimer.Tick to loop at RENDER_DELAY.
            //m_RenderTimer.Tick += Render;
            m_UpdateTimer.Start();
            m_RenderTimer.Start();
        }

        private void Update()
        {
            int l_TPS = 0, l_Call = 0;
            long l_UpdateStartTimeMillisecond = MainWindow.m_Stopwatch.ElapsedMilliseconds;
            while (true)
            {
                l_TPS++;
                l_Call++;
                if (MainWindow.m_Stopwatch.ElapsedMilliseconds - l_UpdateStartTimeMillisecond > 1000)
                {
                    l_UpdateStartTimeMillisecond += 1000;
                    MainWindow.m_TPSText = $"TPS: {l_TPS} | Total Call: {l_Call}";
                    l_TPS = 0;
                }
                foreach (BlobCell l_BlobCell in m_BlobCells)
                {
                    l_BlobCell.Move(MainWindow.Random);
                }
                Thread.Sleep(MainWindow.UPDATE_DELAY);
            }
        }

        private void Render()
        {
            int l_FPS = 0;
            long l_RenderStartTimeMillisecond = MainWindow.m_Stopwatch.ElapsedMilliseconds;
            while (true)
            {
                l_FPS++;
                if (MainWindow.m_Stopwatch.ElapsedMilliseconds - l_RenderStartTimeMillisecond > 1000)
                {
                    l_RenderStartTimeMillisecond += 1000;
                    MainWindow.m_FPSText = $"FPS: {l_FPS}";
                    l_FPS = 0;
                }
                foreach (var l_BlobCell in m_BlobCells)
                {
                    l_BlobCell.Draw();
                }
                
                Thread.Sleep(MainWindow.RENDER_DELAY);
            }
        }
    }
}