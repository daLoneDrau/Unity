using Assets.Scripts.Blueholme.Globals;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Blueholme.Singletons
{
    public class BHController : ProjectConstants
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "BHController"
                };
                Instance = go.AddComponent<BHController>();
            }
        }
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
        public override int GetMaxEquipped() { return BHGlobals.MAX_EQUIPPED; }
        public override int GetMaxSpells() { return 0; }
        public override int GetNumberEquipmentElements() { return BHGlobals.NUM_ELEMENTS; }
    }
}
