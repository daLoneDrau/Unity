using Assets.Scripts.Crypts.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Flyweights
{
    /// <summary>
    /// the 
    /// </summary>
    public class Homeland
    {
        /// <summary>
        /// The set of all <see cref="Homeland"/>s.
        /// </summary>
        private static Dictionary<string, Homeland> values;
        /// <summary>
        /// Gets the list of all <see cref="Homeland"/>s.
        /// </summary>
        /// <returns><see cref="Homeland"/>[]</returns>
        public static Homeland[] Values()
        {
            return values.Values.ToArray<Homeland>();
        }
        /// <summary>
        /// The <see cref="Homeland"/>'s name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The <see cref="Homeland"/>'s description.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// the list of element modifiers.
        /// </summary>
        private EquipmentItemModifier[] elements;
        /// <summary>
        /// Creates a new instance of <see cref="Homeland"/>.
        /// </summary>
        /// <param name="name">the <see cref="Homeland"/>'s name</param>
        /// <param name="description">the <see cref="Homeland"/>'s description</param>
        public Homeland(string name, string description)
        {
            if (values == null)
            {
                values = new Dictionary<string, Homeland>();
            }
            if (values.ContainsKey(name))
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Homeland " + name + " already exists!");
            }
            int numElements = ProjectConstants.Instance.GetNumberEquipmentElements();
            elements = new EquipmentItemModifier[numElements];
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                elements[i] = new EquipmentItemModifier();
            }
            Name = name;
            Description = description;
            values.Add(name, this);
        }
        /// <summary>
        /// Gets the element modifier.
        /// </summary>
        /// <param name="element">the element</param>
        /// <returns><see cref="EquipmentItemModifier"/></returns>
        public EquipmentItemModifier GetElementModifier(int element)
        {
            return elements[element];
        }
        /// <summary>
        /// Sets the modifier applied for character's coming from this <see cref="Homeland"/>.
        /// </summary>
        /// <param name="element">the element</param>
        /// <param name="modifier">the <see cref="EquipmentItemModifier"/></param>
        public void SetModifier(int element, EquipmentItemModifier modifier)
        {
            elements[element] = modifier;
        }
    }
}
