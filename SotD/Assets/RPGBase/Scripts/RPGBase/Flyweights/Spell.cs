using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public class Spell
    {
        /** the caster's reference id. */
        public int Caster { get; set; }
        /** the caster's level. */
        public float CasterLevel { get; set; }
        public bool Exists { get; set; }
        /** any flags that have been set. */
        private long flags = 0;
        public int LastTurnUpdated { get; set; }
        public long LastUpdated { get; set; }
        public int Longinfo { get; set; }
        public int Longinfo2 { get; set; }
        public object Misc { get; set; }
        public int Target { get; set; }
        public long TimeCreated { get; set; }
        public long TimeToLive { get; set; }
        public int TurnCreated { get; set; }
        public int TurnsToLive { get; set; }
        public int Type { get; set; }
        /**
         * Adds a flag.
         * @param flag the flag
         */
        public void AddFlag( long flag)
        {
            flags |= flag;
        }
        /** Clears all flags that were set. */
        public void ClearFlags()
        {
            flags = 0;
        }
        /**
         * Determines if the {@link Spell} has a specific flag.
         * @param flag the flag
         * @return true if the {@link Spell} has the flag; false otherwise
         */
        public bool HasFlag( long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public void RemoveFlag( long flag)
        {
            flags &= ~flag;
        }
    }
}
