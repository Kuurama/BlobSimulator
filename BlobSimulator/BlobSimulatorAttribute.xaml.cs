﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BlobSimulator.Blob;
using BlobSimulator.Map;
using Color = System.Drawing.Color;

namespace BlobSimulator
{
    /// <summary>
    ///     Attributes for BlobSimulatorWindow.xaml
    /// </summary>
    public partial class BlobSimulatorWindow
    {
        /// Set the screen minimum Width and Height, then the Scale Multiplier => By how much you multiply them to set the BlobSimulatorWindow Size.
        private const int MIN_WIDTH = 256, MIN_HEIGHT = 144, SCALE_MULTIPLIER = 6;

        private const float SIM_SCALE_MULTIPLIER = 3;
        public const int SIM_WIDTH = (int)(MIN_WIDTH * SIM_SCALE_MULTIPLIER), SIM_HEIGHT = (int)(MIN_HEIGHT * SIM_SCALE_MULTIPLIER);
        private const double SCREEN_RATIO = (double)MIN_WIDTH / MIN_HEIGHT;

        /// Basically how fast the program *could*/*will* run, set the Update and Render loops tick/sec.
        private const int SIM_FPS = 100000;

        private const int SIM_TPS = 10;
        public static Color[]? m_BlockListColor;

        /// Important Variable.
        private readonly BlobCell[] m_BlobCells;

        private readonly int m_BlobCount;

        private readonly TextBlock m_BLobCountTextBlock = new TextBlock
        {
            Text = "BlobCount: 0",
            Foreground = new SolidColorBrush(Colors.Aqua),
            FontSize = SIM_HEIGHT * 0.015f
        };


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

        private readonly bool m_ProcessMap = true;
        private readonly int m_ProcessMapLoopTimeOut;

        private readonly Random m_Random;
        private readonly int m_SaltCount;
        private readonly Salt.Salt[] m_Salts;
        private readonly Stopwatch m_Stopwatch;

        private readonly TextBlock m_TPSTextBlock = new TextBlock
        {
            Text = "TPS: 0",
            Foreground = new SolidColorBrush(Colors.Aqua),
            FontSize = SIM_HEIGHT * 0.015f
        };

        private readonly TrailMap m_TrailMap;
        private readonly bool m_UncappedTPS;


        /// Informative variable.
        private int m_FPS, m_TPS;
    }
}