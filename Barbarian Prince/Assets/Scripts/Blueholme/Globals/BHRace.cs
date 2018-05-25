using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Globals
{
    public sealed class BHRace
    {
        public string Title { get; private set; }
        public int Val { get; private set; }
        private BHRace(string t, int v)
        {
            Title = t;
            Val = v;
        }
        /// <summary>
        /// Human
        /// </summary>
        public static BHRace Human = new BHRace("Human", 1);
        /// <summary>
        /// Dwarf
        /// </summary>
        public static BHRace Dwarf = new BHRace("Dwarf", 2);
        /// <summary>
        /// Dwarf
        /// </summary>
        public static BHRace Elf = new BHRace("Elf", 4);
        /// <summary>
        /// Dwarf
        /// </summary>
        public static BHRace Halfling = new BHRace("Halfling", 8);
    }
}
