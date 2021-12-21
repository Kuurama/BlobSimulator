﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

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
            m_TrailMap = new TrailMap(MIN_WIDTH * SCALE_MULTIPLIER, MIN_HEIGHT * SCALE_MULTIPLIER);
            m_Stopwatch = new Stopwatch();
            m_Stopwatch.Start();
            ///////////////////////////

            DispatcherTimer l_RenderTimer = new DispatcherTimer();
            DispatcherTimer l_UpdateTimer = new DispatcherTimer();

            l_RenderTimer.Interval = TimeSpan.FromTicks(RENDER_TPS); /// Sets m_RenderTimer.Tick to loop at RENDER_TPS.
            l_RenderTimer.Tick += Render;

            l_UpdateTimer.Interval = TimeSpan.FromTicks(UPDATE_TPS); /// Sets m_RenderTimer.Tick to loop at UPDATE_TPS.
            l_UpdateTimer.Tick += Update;

            m_BlobCount = 500000;
            m_BlobListCount = 8; /// By extension => the number of Thread.
            m_BlobSpeed = 10.0f;
            m_BlobCellsList = new List<List<BlobCell>>();

            /// Sets randomly the blob's position.
            int l_PosX = m_Random.Next(0, SIM_WIDTH);
            int l_PosY = m_Random.Next(0, SIM_HEIGHT);

            List<int> l_ListBlobNeededPerList = StaticFunction.NumberSplit(m_BlobCount, m_BlobListCount); /// Returns a list of Integers representing the number of Blob per BlobList needed.

            /// Instanciating the blobs and adding each BlobCellLists into ListBlobCellsList.
            foreach (int l_BlobNeededPerList in l_ListBlobNeededPerList) m_BlobCellsList.Add(BlobController.CreateBlobGroup(l_BlobNeededPerList, l_PosX, l_PosY, m_BlobSpeed, m_Random));

            /// Starts the Update and Render Loop.
            m_RenderStartTimeMillisecond = m_Stopwatch.ElapsedMilliseconds;
            m_UpdateStartTimeMillisecond = m_Stopwatch.ElapsedMilliseconds;
            l_RenderTimer.Start();
            l_UpdateTimer.Start();

            m_ProcessMap = true;
            m_ProcessMapLoopTimeOut = 10;
            Thread l_ProcessTrailMapThread = new Thread(ProcessTrailMap);
            l_ProcessTrailMapThread.Start();
        }

        private void Update(object? sender, EventArgs eventArgs)
        {
            m_TPS++;
            if (m_Stopwatch.ElapsedMilliseconds - m_UpdateStartTimeMillisecond > 1000)
            {
                m_UpdateStartTimeMillisecond += 1000;
                m_TPSTextBlock.Text = $"TPS: {m_TPS}";
                m_BLobCountTextBlock.Text = $"BlobCount: {m_BlobCount}";
                m_TPS = 0;
            }

            List<Task> l_TaskArray = m_BlobCellsList.Select(blobCells => Task.Factory.StartNew(() =>
            {
                foreach (BlobCell l_BlobCell in blobCells)
                {
                    l_BlobCell.Move(m_Random);
                }
            })).ToList();
            
            Task.WaitAll(l_TaskArray.ToArray());
        }

        private void Render(object? sender, EventArgs eventArgs)
        {
            m_FPS++;
            if (m_Stopwatch.ElapsedMilliseconds - m_RenderStartTimeMillisecond > 1000)
            {
                m_RenderStartTimeMillisecond += 1000;
                m_FPSTextBlock.Text = $"FPS: {m_FPS}";
                m_FPS = 0;
            }

            List<Task> l_TaskArray = m_BlobCellsList.Select(blobCells => Task.Factory.StartNew(() =>
                {
                    foreach (BlobCell l_BlobCell in blobCells)
                    {
                        l_BlobCell.Draw(m_TrailMap);
                    }
                })).ToList();

            Task.WaitAll(l_TaskArray.ToArray());

            // Update the Image source (Which is being displayed).
            m_Image.Source = m_TrailMap.m_BitMap.MakeBitmap(96, 96);
        }

        private void ProcessTrailMap()
        {
            while (m_ProcessMap)
            {
                m_TrailMap.m_BitMap.EvaporateAllPixel(5);
                Thread.Sleep(m_ProcessMapLoopTimeOut);
            }
        }
    }
}