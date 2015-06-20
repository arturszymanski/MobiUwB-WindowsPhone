#region

using System;

#endregion

namespace MobiUwB.Utilities
{
    public class UnitConventer
    {
        public static Double PixelsToFontSize(Double pixels)
        {
            return 3d / 4d * pixels;
        }

        public static Double FontSizeToPixels(Double points)
        {
            return 4d / 3d * points;
        }
    }
}
