using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBaseCS.Constants
{
    public static class DieExtensions
    {
        public static int GetFaces(this Die die)
        {
            return Int32.Parse(die.ToString().Substring(1));
        }
    }
    public enum Die
    {
        D2,
        D3,
        D4,
        D6,
        D8,
        D10,
        D12,
        D20
    }
}
