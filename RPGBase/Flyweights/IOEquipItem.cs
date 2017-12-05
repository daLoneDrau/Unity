using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Singletons;

namespace RPGBase.Flyweights
{
    /// <summary>
    /// Class definition for an equipment item.
    /// </summary>
    public class IOEquipItem
    {
        /// <summary>
        /// the list of equipment modifiers.
        /// </summary>
        private EquipmentItemModifier[] elements;
        /// <summary>
        /// Creates a new instance of <see cref="IOEquipItem"/>.
        /// </summary>
        public IOEquipItem()
        {
            int numElements = ProjectConstants.GetInstance().GetNumberEquipmentElements();
            elements = new EquipmentItemModifier[numElements];
            for (int i = elements.Length - 1; i >= 0 ; i--)
            {
                elements[i] = new EquipmentItemModifier();
            }
        }
        /// <summary>
        /// Frees all resources.
        /// </summary>
        public void Free()
        {
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                elements[i] = null;
            }
        }
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="element">the element</param>
        /// <returns><see cref="EquipmentItemModifier"/></returns>
        public EquipmentItemModifier GetElementModifier(int element)
        {
            return elements[element];
        }
        /** Resets all modifiers. */
        public void Reset()
        {
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                elements[i].ClearData();
            }
        }
    }
}
