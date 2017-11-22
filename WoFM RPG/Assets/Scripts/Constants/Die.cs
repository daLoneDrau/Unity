using System;

namespace RPGBase.Constants
{
    public static class DieExtensions
    {
        public static int GetFaces(this Die die)
        {
            return (int)die;
        }
    }
    public enum Die
    {
        D2 = 2,
        D3 = 3,
        D4 = 4,
        D6 = 6,
        D8 = 8,
        D10 = 10,
        D12 = 12,
        D20 = 20
    }
}
