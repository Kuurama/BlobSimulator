using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BlobSimulator
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// Sets the screen Width and Height.
        public const int WIDTH = 800;

        public const int HEIGHT = 450;

        /// Basically FPS.
        private const int UPDATE_TICK_PER_SECOND = 50;

        private const int RENDER_TICK_PER_SECOND = 50;
        private readonly Random m_Random = new Random();
        public int m_FPS;

        /// Create the Stopwatch used for the Thread's Tick Loop.
        private readonly Stopwatch m_Stopwatch = new Stopwatch();

        private long m_StartTimeMillisecond;

        /// Make the BitmapPixelMaker.
        public static readonly BitmapPixelMaker BitmapPixelMaker = new BitmapPixelMaker(WIDTH, HEIGHT);


        private readonly Image m_Image = new Image
        {
            Tag = "Main",
            Stretch = Stretch.None,
            Margin = new Thickness(0)
        };

        /// Create an Image to display the bitmap.
        private readonly List<BlobCell> m_BlobCells;

        private readonly DispatcherTimer m_RenderTimer = new DispatcherTimer();

        private readonly DispatcherTimer m_UpdateTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            m_RenderTimer.Interval = TimeSpan.FromMilliseconds(60D / RENDER_TICK_PER_SECOND); /// Sets m_RenderTimer.Tick to loop at RENDER_TICK_PER_SECOND.
            m_RenderTimer.Tick += Render;

            m_UpdateTimer.Interval = TimeSpan.FromMilliseconds(60D / UPDATE_TICK_PER_SECOND); /// Sets m_UpdateTimer.Tick to loop at UPDATE_TICK_PER_SECOND.
            m_UpdateTimer.Tick += Update;

            /// Instanciate the Blobs.
            m_BlobCells = BlobController.CreateBlobGroup(150, m_Random.Next(0, WIDTH), m_Random.Next(0, HEIGHT), 1.0f, m_Random);

            m_Stopwatch.Start();
            m_StartTimeMillisecond = m_Stopwatch.ElapsedMilliseconds;

            m_RenderTimer.Start();
            m_UpdateTimer.Start();
        }

        private void Update(object? p_Sender, EventArgs p_EventArgs) /// This will be used for code processing.
        {
            foreach (var l_BlobCell in m_BlobCells)
            {
                l_BlobCell.Move(m_Random);
            }
        }

        private void Render(object? p_Sender, EventArgs p_EventArgs) /// This will be used for render only.
        {
            m_FPS++;

            if (m_Stopwatch.ElapsedMilliseconds - m_StartTimeMillisecond > 1000)
            {
                m_StartTimeMillisecond += 1000;
                Debug.WriteLine(m_FPS);
                m_FPS = 0;
            }


            // Clear the bitmap before updating the blobs.
            BitmapPixelMaker.SetColor(0, 0, 0, 255);

            foreach (var l_BlobCell in m_BlobCells)
            {
                l_BlobCell.Draw();
            }

            // Update the Image source (Which is being displayed).
            m_Image.Source = BitmapPixelMaker.MakeBitmap(96, 96);
        }

        private void Window_Loaded(object p_Sender, RoutedEventArgs p_EventArgs)
        {
            /// Clear to black.
            BitmapPixelMaker.SetColor(0, 0, 0, 255);

            /// Add the Image to the Canvas.
            CanvasMain.Children.Add(m_Image);

            /// Set the Image source (Also edit the Canvas stored Image).
            m_Image.Source = BitmapPixelMaker.MakeBitmap(96, 96); /// Convert the pixel data.
        }
    }
}