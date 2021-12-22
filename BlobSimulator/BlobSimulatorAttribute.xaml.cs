using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BlobSimulator.Blob;
using BlobSimulator.Map;

namespace BlobSimulator
{
    /// <summary>
    ///     Attributes for BlobSimulatorWindow.xaml
    /// </summary>
    public partial class BlobSimulatorWindow
    {
        /// Set the screen minimum Width and Height, then the Scale Multiplier => By how much you multiply them to set the BlobSimulatorWindow Size.
        private const int MIN_WIDTH = 256, MIN_HEIGHT = 144, SCALE_MULTIPLIER = 5;
        private const float SIM_SCALE_MULTIPLIER = 5f;
        public const int SIM_WIDTH = (int)(MIN_WIDTH*SIM_SCALE_MULTIPLIER), SIM_HEIGHT = (int)(MIN_HEIGHT*SIM_SCALE_MULTIPLIER);
        private const double SCREEN_RATIO = (double)MIN_WIDTH / MIN_HEIGHT;

        /// Basically how fast the program *could*/*will* run, set the Update and Render loops tick/sec.
        private const int UPDATE_TPS = 100000, RENDER_TPS = 100000;

        /// Important Variable.
        private readonly List<List<BlobCell>> m_BlobCellsList;

        private readonly int m_BlobCount, m_BlobListCount;
        private readonly float m_BlobSpeed, m_BlobTurnSpeed;
        private readonly bool m_ProcessMap = true;
        private readonly int m_ProcessMapLoopTimeOut;


        /// TextBox.
        private readonly TextBlock m_FPSTextBlock = new TextBlock
        {
            Text = "FPS: 0",
            Foreground = new SolidColorBrush(Colors.Aqua),
            FontSize = SIM_HEIGHT * 0.015f
        };

        private readonly Image m_Image = new Image /// Initialise the image used by the Canvas.
        {
            Tag = "Main",
            Stretch = Stretch.None,
            Margin = new Thickness(0),
            Source = null
        };

        private readonly TextBlock m_BLobCountTextBlock = new TextBlock
        {
            Text = "BlobCount: 0",
            Foreground = new SolidColorBrush(Colors.Aqua),
            FontSize = SIM_HEIGHT * 0.015f
        };

        private readonly Random m_Random;
        private readonly Stopwatch m_Stopwatch;

        private readonly TextBlock m_TPSTextBlock = new TextBlock
        {
            Text = "TPS: 0",
            Foreground = new SolidColorBrush(Colors.Aqua),
            FontSize = SIM_HEIGHT * 0.015f
        };

        private readonly TrailMap m_TrailMap;


        /// Informative variable.
        private int m_FPS, m_TPS;

        private long m_RenderStartTimeMillisecond, m_UpdateStartTimeMillisecond;
    }
}