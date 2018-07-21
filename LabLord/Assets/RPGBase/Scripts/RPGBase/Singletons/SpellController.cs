using RPGBase.Flyweights;

namespace RPGBase.Singletons
{
    public abstract class SpellController
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static SpellController Instance { get; protected set; }
        public abstract Spell GetSpell(int index);
    }
}