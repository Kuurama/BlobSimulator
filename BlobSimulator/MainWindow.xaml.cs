using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Color = System.Drawing.Color;

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
        public const int UPDATE_DELAY = 2;

        public const int RENDER_DELAY = 2;

        public static readonly Random Random = new Random();

        private readonly int m_BlobCount;
        
        public static int m_FPS;
        public static int m_TPS;
        public static string m_FPSText;
        public static string m_TPSText;

        private readonly int m_WorkerThreads;
        private readonly int m_CompletionPortThreads;

        /// Create the FPS TextBox Globally.
        private TextBlock m_FPSTextBlock = new TextBlock
        {
            Text = "FPS:",
            Foreground = new SolidColorBrush(Colors.Aqua)
        };

        /// Create the TPS (Tick Per Second) TextBox Globally.
        private readonly TextBlock m_TPSTextBlock = new TextBlock
        {
            Text = "TPS:",
            Foreground = new SolidColorBrush(Colors.Aqua)
        };
        
        /// Create the BLobCount TextBox Globally.
        private readonly TextBlock m_BLobCountTextBlock = new TextBlock
        {
            Text = "BlobCount:",
            Foreground = new SolidColorBrush(Colors.Aqua)
        };

        /// Create the Stopwatch used for the Thread's Tick Loop.
        public static readonly Stopwatch m_Stopwatch = new Stopwatch();

        public static long m_RenderStartTimeMillisecond;
        public static long m_UpdateStartTimeMillisecond;

        /// Make the BitmapPixelMaker.
        public static readonly BitmapPixelMaker BitmapPixelMaker = new BitmapPixelMaker(WIDTH, HEIGHT);


        private readonly Image m_Image = new Image
        {
            Tag = "Main",
            Stretch = Stretch.None,
            Margin = new Thickness(0)
        };


        /// Create the DispatcherTimers used to Make the Update and Render Loop.
        private readonly DispatcherTimer m_RenderTimer = new DispatcherTimer();
        private readonly DispatcherTimer m_UpdateTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            int l_NumberOfThread = 15;

            /// Instanciate the Blobs.
            m_BlobCount = 16;

            if (m_BlobCount < l_NumberOfThread)
            {
                l_NumberOfThread = m_BlobCount;
            }

            int l_PosX = Random.Next(0, WIDTH);
            int l_PosY = Random.Next(0, HEIGHT);
            
            m_Stopwatch.Start();

            m_RenderTimer.Interval = TimeSpan.FromMilliseconds(20); /// Sets m_RenderTimer.Tick to loop at RENDER_DELAY.
            m_RenderTimer.Tick += Render;
            
            m_UpdateTimer.Start();
            m_RenderTimer.Start();
            
            
            for (int l_Index = 0; l_Index < l_NumberOfThread; l_Index++)
            {
                int l_BlobCount = m_BlobCount / l_NumberOfThread;
                List<BlobCell> l_BlobCells = BlobController.CreateBlobGroup(l_BlobCount, l_PosX, l_PosY, 1.0f, Random);
                BlobController l_BlobController = new BlobController(l_BlobCells);
            }

            ThreadPool.GetMaxThreads(out m_WorkerThreads, out m_CompletionPortThreads);
        }

        private void Render(object? p_Sender, EventArgs p_EventArgs) /// This will be used for render only.
        {
            if (m_Stopwatch.ElapsedMilliseconds - m_RenderStartTimeMillisecond > 1000)
            {
                m_FPSTextBlock.Text = m_FPSText;
                m_TPSTextBlock.Text = m_TPSText;
            }


            // Clear the bitmap before updating the blobs.
            //BitmapPixelMaker.SetColor(Color.Black);

            // Update the Image source (Which is being displayed).
            m_Image.Source = BitmapPixelMaker.MakeBitmap(96, 96);
        }

        private void Window_Loaded(object p_Sender, RoutedEventArgs p_EventArgs)
        {
            /// Clear to black.
            BitmapPixelMaker.SetColor(Color.Black);

            /// Add the Image to the Canvas.
            CanvasMain.Children.Add(m_Image);

            /// Set the Image source (Also edit the Canvas stored Image).
            m_Image.Source = BitmapPixelMaker.MakeBitmap(96, 96); /// Convert the pixel data.

            /// Set the FPF TextBox into the Main Canvas.
            Canvas.SetLeft(m_FPSTextBlock, 0);
            Canvas.SetTop(m_FPSTextBlock, 0);
            CanvasMain.Children.Add(m_FPSTextBlock);

            /// Set the TPS (Tick Per Second) TextBox into the Main Canvas.
            Canvas.SetLeft(m_TPSTextBlock, 0);
            Canvas.SetTop(m_TPSTextBlock, 12);
            CanvasMain.Children.Add(m_TPSTextBlock);
            
            /// Set the BlobCount TextBox into the Main Canvas.
            Canvas.SetRight(m_BLobCountTextBlock, 0);
            Canvas.SetTop(m_BLobCountTextBlock, 0);
            m_BLobCountTextBlock.Text = $"BlobCount: {m_BlobCount.ToString()}".ToString();
            CanvasMain.Children.Add(m_BLobCountTextBlock);
        }
    }
}