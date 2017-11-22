using UnityEngine;

namespace RPGBase.Engine.Systems
{
    public abstract class ProjectConstants : MonoBehaviour
    {
        /// <summary>
        /// the one and only instance of the <see cref="ProjectConstants"/> class.
        /// </summary>
        private static ProjectConstants instance;
        /// <summary>
        /// Gives access to the singleton instance of <see cref="ProjectConstants"/>.
        /// </summary>
        /// <returns><see cref="ProjectConstants"/></returns>
        public static ProjectConstants GetInstance()
        {
            return ProjectConstants.instance;
        }
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
        /// <summary>
        /// Sets the global instance.
        /// </summary>
        /// <param name="value">the instance</param>
        protected void SetInstance(ProjectConstants value)
        {
            ProjectConstants.instance = value;
        }
    }
}
