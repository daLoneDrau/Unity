using Assets.Scripts.BarbarianPrince.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Singletons
{
    public class BPController : ProjectConstants
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "BPController"
                };
                Instance = go.AddComponent<BPController>();
            }
        }
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
        public override int GetMaxEquipped() { return BPGlobals.MAX_EQUIPPED; }
        public override int GetNumberEquipmentElements() { return BPGlobals.NUM_ELEMENTS; }
    }
}
