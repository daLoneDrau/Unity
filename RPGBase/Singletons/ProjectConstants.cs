using System;
using UnityEngine;

namespace RPGBase.Singletons
{
    public class ProjectConstants: MonoBehaviour, IProjectConstants
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static ProjectConstants Instance { get; protected set; }
        /// <summary>
        /// Gets the index of the equipment element for damage.
        /// </summary>
        /// <returns></returns>
        public virtual int GetDamageElementIndex() { throw new NotImplementedException(); }
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
        public virtual int GetMaxEquipped() { throw new NotImplementedException(); }
        /// <summary>
        /// Gets the maximum number of spells.
        /// </summary>
        /// <returns></returns>
        public virtual int GetMaxSpells() { throw new NotImplementedException(); }
        /// <summary>
        /// Gets the number of equipment element modifiers there are.
        /// </summary>
        /// <returns></returns>
        public virtual int GetNumberEquipmentElements() { throw new NotImplementedException(); }
        /// <summary>
        /// Gets the reference id of the player.
        /// </summary>
        /// <returns></returns>
        public virtual int GetPlayer() { throw new NotImplementedException(); }
    }
}
