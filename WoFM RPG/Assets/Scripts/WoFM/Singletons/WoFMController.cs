using RPGBase.Singletons;
using UnityEngine;
using WoFM.Constants;

namespace WoFM.Singletons
{
    public class WoFMController : ProjectConstants
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "WoFMController"
                };
                Instance = go.AddComponent<WoFMController>();
                DontDestroyOnLoad(go);
            }
        }
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
        public override int GetMaxEquipped() { return WoFMGlobals.MAX_EQUIPPED; }
        /// <summary>
        /// Gets the number of equipment element modifiers there are.
        /// </summary>
        /// <returns></returns>
        public override int GetNumberEquipmentElements() { return WoFMGlobals.NUM_ELEMENTS; }
    }
}