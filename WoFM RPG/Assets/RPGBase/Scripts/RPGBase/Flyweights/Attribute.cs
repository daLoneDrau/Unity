using System;

namespace RPGBase.Flyweights
{
    public sealed class Attribute
    {
        private string abbr;
        /// <summary>
        /// the <see cref="Attribute"/>'s name abbreviation.
        /// </summary>
        public string Abbr
        {
            get { return abbr; }
            set
            {
                abbr = value;
                if (abbr == null) { throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Abbreviation cannot be null"); }
            }
        }
        /// <summary>
        /// the <see cref="Attribute"/> 's base value.
        /// </summary>
        public float BaseVal { get; set; }
        private string description;
        /// <summary>
	    /// the <see cref="Attribute"/>'s description.
        /// </summary>
	    public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (description == null) { throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Description cannot be null"); }
            }
        }
        private string displayName;
        /// <summary>
        /// the <see cref="Attribute"/> 's display name.
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;
                if (displayName == null) { throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Display name cannot be null"); }
            }
        }
        /// <summary>
        /// the <see cref="Attribute"/> 's display name.
        /// </summary>
        public float Full { get { return BaseVal + Modifier; } }
        /// <summary>
        /// the value of any modifiers to the attribute.
        /// </summary>
        public float Modifier { get; private set; }
        /// <summary>
        /// Creates a new instance of <see cref="Attribute"/>.
        /// </summary>
        /// <param name="a">the <see cref="Attribute"/>'s name abbreviation</param>
        /// <param name="n">the <see cref="Attribute"/>'s display name</param>
        public Attribute(String a, String n)
        {
            Abbr = a;
            DisplayName = n;
        }
        /// <summary>
        /// Creates a new instance of <see cref="Attribute"/>.
        /// </summary>
        /// <param name="a">the <see cref="Attribute"/>'s name abbreviation</param>
        /// <param name="n">the <see cref="Attribute"/>'s display name</param>
        /// <param name="desc">the <see cref="Attribute"/>'s description</param>
        public Attribute(String a, String n, String desc)
        {
            Abbr = a;
            DisplayName = n;
            Description = desc;
        }
        /// <summary>
        /// Adjusts the value for the modifier.
        /// </summary>
        /// <param name="val">the value to adjust by</param>
        public void AdjustModifier(float val)
        {
            Modifier += val;
        }
        /// <summary>
        /// Resets the <see cref="Attribute"/>'s modifier value to 0.
        /// </summary>
        public void ClearModifier()
        {
            Modifier = 0;
        }
    }
}