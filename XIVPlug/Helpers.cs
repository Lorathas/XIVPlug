using System;

namespace XIVPlug
{
    public static class Helpers
    {
        public static double Clamp(this double value, double min, double max)
        {
            return Math.Min(Math.Max(value, 0.0), 1.0);
        }
    }
}