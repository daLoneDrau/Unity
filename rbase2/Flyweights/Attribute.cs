using System;

namespace RPGBase.Flyweights
{
    public sealed class Attribute
    {
        /// <summary>
        /// the <see cref="Attribute"/>'s name abbreviation.
        /// </summary>
        public string Abbr { get; set; }
        /// <summary>
        /// the <see cref="Attribute"/> 's base value.
        /// </summary>
        public float BaseVal { get; set; }
        /// <summary>
	    /// the <see cref="Attribute"/> 's description.
        /// </summary>
	    public string Description { get; set; }
        /// <summary>
        /// the <see cref="Attribute"/> 's display name.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// the value of any modifiers to the attribute.
        /// </summary>
        public float Modifier { get; private set; }
        /// <summary>
        /// Creates a new instance of <see cref="Attribute"/>.
        /// </summary>
        /// <param name="a">the <see cref="Attribute"/>'s name abbreviation</param>
        /// <param name="n">the <see cref="Attribute"/>'s display name</param>
        public Attribute(char[] a, char[] n) : this(a, n, null)
        {
        }
        /// <summary>
        /// Creates a new instance of <see cref="Attribute"/>.
        /// </summary>
        /// <param name="a">the <see cref="Attribute"/>'s name abbreviation</param>
        /// <param name="n">the <see cref="Attribute"/>'s display name</param>
        /// <param name="desc">the <see cref="Attribute"/>'s description</param>
        public Attribute(char[] a, char[] n, char[] desc)
        {
            Abbr = new string(a);
            DisplayName = new string(n);
            Description = new string(desc);
        }
        /// <summary>
        /// Creates a new instance of <see cref="Attribute"/>.
        /// </summary>
        /// <param name="a">the <see cref="Attribute"/>'s name abbreviation</param>
        /// <param name="n">the <see cref="Attribute"/>'s display name</param>
        public Attribute(String a, String n)
        {
            if (a == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                        "Name abbreviation cannot be null");
            }
            if (n == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                        "Display name cannot be null");
            }
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
            if (a == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                        "Name abbreviation cannot be null");
            }
            if (n == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                        "Display name cannot be null");
            }
            if (desc == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                        "Description cannot be null");
            }
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
