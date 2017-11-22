using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Engine.Systems;

namespace RPGBase.Flyweights
{
    /// <summary>
    /// Class definition for an equipment item.
    /// </summary>
    public class IOEquipItem
    {
        /** the list of equipment modifiers. */
        private EquipmentItemModifier[] elements;
        /**
         * Creates a new instance of {@link IOEquipItem}. 
         * @throws RPGException
         */
        public IOEquipItem()
        {
            int numElements = ProjectConstants.GetInstance().GetNumberEquipmentElements();
            elements = new EquipmentItemModifier[numElements];
            for (int i = elements.Length - 1; i >= 0 ; i--)
            {
                elements[i] = new EquipmentItemModifier();
            }
        }
        /** Frees all resources. */
        public void Free()
        {
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                elements[i] = null;
            }
        }
        /**
         * Gets the element.
         * @param element the element
         * @return {@link EquipmentItemModifier}
         */
        public EquipmentItemModifier GetElement(int element)
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
