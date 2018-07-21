using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Singletons
{
    public interface IProjectConstants
    {
        /// <summary>
        /// Gets the index of the equipment element for damage.
        /// </summary>
        /// <returns></returns>
        int GetDamageElementIndex();
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
       int GetMaxEquipped();
        /// <summary>
        /// Gets the maximum number of spells.
        /// </summary>
        /// <returns></returns>
        int GetMaxSpells();
        int GetNumberEquipmentElements();
        /// <summary>
        /// Gets the reference id of the player.
        /// </summary>
        /// <returns></returns>
        int GetPlayer();
    }
}
