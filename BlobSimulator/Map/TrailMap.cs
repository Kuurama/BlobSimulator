using System.Drawing;

namespace BlobSimulator.Map
{
    public class TrailMap
    {
        public readonly BitmapPixelMaker m_BitMap;

        public TrailMap(int p_Width, int p_Height)
        {
            m_BitMap = new BitmapPixelMaker(p_Width, p_Height);
            m_BitMap.SetColor(Color.Black);
        }
    }
}