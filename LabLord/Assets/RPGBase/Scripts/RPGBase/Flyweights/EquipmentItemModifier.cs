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
        /// the flag indicating whether the <see cref="EquipmentItemModifier"/> is applied per Level.
        /// </summary>
        public bool PerLevel { get; set; }
        /// <summary>
        /// not used. yet.
        /// </summary>
        public int Special { get; set; }
        /// <summary>
        /// the value of modifier to be applied.
        /// </summary>
        public float Value { get; set; }
        /// <summary>
        /// The source of the modifier.
        /// </summary>
        public int Src { get; set; }
        /// <summary>
        /// A description of the source.
        /// </summary>
        public string SrcDescription { get; set; }
        /// <summary>
        /// Creates an empty instance of <see cref="EquipmentItemModifier"/>.
        /// </summary>
        public EquipmentItemModifier() {
            Src = -1;
        }
        /// <summary>
        /// Creates an parameteriezed instance of <see cref="EquipmentItemModifier"/>.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="pCent"></param>
        /// <param name="pLevel"></param>
        /// <param name="spec"></param>
        /// <param name="source"></param>
        /// <param name="desc"></param>
        public EquipmentItemModifier(float val, bool pCent, bool pLevel, int spec, int source, string desc)
        {
            Percent = pCent;
            PerLevel = pLevel;
            Special = spec;
            Value = val;
            SrcDescription = desc;
            Src = source;
        }
        /** Clears all data. */
        public void ClearData()
        {
            Percent = false;
            PerLevel = false;
            Special = 0;
            Value = 0f;
            SrcDescription = "";
            Src = -1;
        }
        /**
         * Sets the modifier values.
         * @param other the values being cloned
         */
        public void Set(EquipmentItemModifier other)
        {
            this.Percent = other.Percent;
            this.PerLevel = other.PerLevel;
            this.Special = other.Special;
            this.Value = other.Value;
            this.Src = other.Src;
            this.SrcDescription = other.SrcDescription;
        }
    }
}
