using Assets.Scripts.BarbarianPrince.Constants;
using Assets.Scripts.FantasyWargaming.Globals;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Singletons
{
    public class FWController : ProjectConstants
    {
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "FWController"
                };
                Instance = go.AddComponent<FWController>();
            }
        }
        /// <summary>
        /// Gets the maximum number of equipment slots.
        /// </summary>
        /// <returns></returns>
        public override int GetMaxEquipped() { return FWGlobals.MAX_EQUIPPED; }
        public override int GetNumberEquipmentElements() { return FWGlobals.NUM_ELEMENTS; }
    }
}
