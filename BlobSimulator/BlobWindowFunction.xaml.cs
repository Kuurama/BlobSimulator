using System.Windows;
using System.Windows.Controls;

namespace BlobSimulator
{
    /// <summary>
    ///     Window function and logic for BlobSimulatorWindow.xaml
    /// </summary>
    public partial class BlobSimulatorWindow
    {
        private void Window_Loaded(object p_Sender, RoutedEventArgs p_EventArgs)
        {
            CanvasMain.Children.Add(m_Image); /// Add the Image to the Canvas.

            /// Adds the FPS TextBox to the MainCanvas.
            Canvas.SetLeft(m_FPSTextBlock, 0);
            Canvas.SetTop(m_FPSTextBlock, 0);
            CanvasMain.Children.Add(m_FPSTextBlock);

            /// Adds the TPS (Tick Per Second) TextBox to the MainCanvas.
            Canvas.SetLeft(m_TPSTextBlock, 0);
            Canvas.SetTop(m_TPSTextBlock, SIM_HEIGHT * 0.015f);
            CanvasMain.Children.Add(m_TPSTextBlock);

            /// Adds the BlobCount TextBox to the MainCanvas.
            Canvas.SetRight(m_BLobCountTextBlock, 0);
            Canvas.SetTop(m_BLobCountTextBlock, 0);
            CanvasMain.Width = SIM_WIDTH;
            CanvasMain.Height = SIM_HEIGHT;
            BlobSimulatorMainWindow.Width = MIN_WIDTH * SCALE_MULTIPLIER;
            BlobSimulatorMainWindow.Height = MIN_HEIGHT * SCALE_MULTIPLIER;
            m_BLobCountTextBlock.Text = $"BlobCount: {m_BlobCount.ToString()}".ToString();
            CanvasMain.Children.Add(m_BLobCountTextBlock);
        }

        /// <summary>
        ///     Makes a window (Here BlobSimulatorWindow) keeps the 16/9 ratio on resize.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sizeChangedEventArgs"></param>
        private void OnRenderSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (sizeChangedEventArgs.WidthChanged) Width = sizeChangedEventArgs.NewSize.Height * SCREEN_RATIO;
            else Height = sizeChangedEventArgs.NewSize.Width / SCREEN_RATIO;
        }
    }
}