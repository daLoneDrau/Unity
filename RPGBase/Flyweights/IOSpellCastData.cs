using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public class IOSpellCastData
    {
        /// <summary>
        /// the reference id of the spell being cast.
        /// </summary>
        public int Castingspell { get; set; }
        /// <summary>
        /// the spell's duration.
        /// </summary>
        public long Duration { get; set; }
        /// <summary>
        /// flags applied to the spell.
        /// </summary>
        private long spellFlags;
        /// <summary>
        /// the spell's level.
        /// </summary>
        public int SpellLevel { get; set; }
        /// <summary>
        /// the reference id of the target.
        /// </summary>
        public int Target { get; set; }
        /// <summary>
        /// Adds a spell flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void AddSpellFlag(long flag)
        {
            spellFlags |= flag;
        }
        /// <summary>
        /// Clears all spell flags that were set.
        /// </summary>
        public void ClearSpellFlags()
        {
            spellFlags = 0;
        }
        /// <summary>
        /// Determines if the <see cref="IOSpellCastData"/> has a specific flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        /// <returns>true if the <see cref="IOSpellCastData"/> has the flag; false otherwise</returns>
        public bool HasSpellFlag(long flag)
        {
            return (spellFlags & flag) == flag;
        }
        /// <summary>
        /// Removes a spell flag.
        /// </summary>
        /// <param name="flag">the flag</param>
        public void RemoveSpellFlag(long flag)
        {
            spellFlags &= ~flag;
        }
    }
}
