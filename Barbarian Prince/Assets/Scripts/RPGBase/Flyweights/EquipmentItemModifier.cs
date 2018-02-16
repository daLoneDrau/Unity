namespace RPGBase.Flyweights
{
    /// <summary>
    /// Equipped item element modifiers. An equipped item can have 0 or more of these modifiers applied; an element that modifies the wielder's strength, an element that modifies the wielder's to-hit score, etc.
    /// </summary>
    public class EquipmentItemModifier
    {
        /// <summary>
        /// the flag indicating whether the <see cref="EquipmentItemModifier"/> is a percentage modifier.
        /// </summary>
        public bool Percent { get; set; }
        /// <summary>
        /// not used. yet.
        /// </summary>
        public int Special { get; set; }
        /// <summary>
        /// the value of modifier to be applied.
        /// </summary>
        public float Value { get; set; }
        /** Clears all data. */
        public void ClearData()
        {
            Percent = false;
            Special = 0;
            Value = 0f;
        }
        /**
         * Sets the modifier values.
         * @param other the values being cloned
         */
        public void Set(EquipmentItemModifier other)
        {
            this.Percent = other.Percent;
            this.Special = other.Special;
            this.Value = other.Value;
        }
    }
}
