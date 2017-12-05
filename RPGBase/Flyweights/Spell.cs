using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public class Spell
    {
        /** the caster's reference id. */
        private int Caster { get; set; }
        /** the caster's level. */
        private float CasterLevel { get; set; }
        private bool Exists { get; set; }
        /** any flags that have been set. */
        private long flags = 0;
        private int LastTurnUpdated { get; set; }
        private long LastUpdated { get; set; }
        private int Longinfo { get; set; }
        private int Longinfo2 { get; set; }
        private object Misc { get; set; }
        private int Target { get; set; }
        private long TimeCreated { get; set; }
        private long TimeToLive { get; set; }
        private int TurnCreated { get; set; }
        private int TurnsToLive { get; set; }
        private int Type { get; set; }
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
