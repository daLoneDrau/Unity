namespace RPGBase.Singletons
{
    public abstract class ProjectConstants
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static ProjectConstants Instance { get; protected set; }
        /// <summary>
        /// Creates a new instance of <see cref="ProjectConstants"/>.
        /// </summary>
        protected ProjectConstants() { }
        /// <summary>
        /// Gets the index of the equipment element for damage.
        /// </summary>
        /// <returns></returns>
        public abstract int GetDamageElementIndex();
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
        public abstract int GetMaxEquipped();
        /// <summary>
        /// Gets the maximum number of spells.
        /// </summary>
        /// <returns></returns>
        public abstract int GetMaxSpells();
        public abstract int GetNumberEquipmentElements();
        /// <summary>
        /// Gets the reference id of the player.
        /// </summary>
        /// <returns></returns>
        public abstract int GetPlayer();
    }
}
