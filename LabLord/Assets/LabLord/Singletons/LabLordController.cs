using RPGBase.Singletons;
using UnityEngine;
using LabLord.Constants;

namespace LabLord.Singletons
{
    public class LabLordController : ProjectConstants
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "LabLordController"
                };
                Instance = go.AddComponent<LabLordController>();
                DontDestroyOnLoad(go);
            }
        }
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
        public override int GetMaxEquipped() { return LabLordGlobals.MAX_EQUIPPED; }
        /// <summary>
        /// Gets the maximum number of spells.
        /// </summary>
        /// <returns></returns>
        public override int GetMaxSpells() { return -1; }
        /// <summary>
        /// Gets the number of equipment element modifiers there are.
        /// </summary>
        /// <returns></returns>
        public override int GetNumberEquipmentElements() { return LabLordGlobals.NUM_ELEMENTS; }
    }
}