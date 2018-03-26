using Assets.Scripts.BarbarianPrince.Constants;
using Assets.Scripts.BarbarianPrince.Flyweights;
using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Singletons
{
    public class MasterScript : MonoBehaviour
    {
        private static MasterScript instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static MasterScript Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject
                    {
                        name = "MasterScript"
                    };
                    instance = go.AddComponent<MasterScript>();
                }
                return instance;
            }
        }
        /// <summary>
        /// Event 001 - The Adventure Begins
        /// </summary>
        /// <returns></returns>
        public IEnumerator E001TheAdventureBegins()
        {
            print("E001TheAdventureBegins");
            // create hero
            BPInteractiveObject io = ((BPInteractive)BPInteractive.Instance).NewHero();
            print("before new hero");
            yield return StartCoroutine(BPServiceClient.Instance.GetItemByName("Bonebiter", value => EquipItemOnFreshIo(io, BPGlobals.EQUIP_SLOT_WEAPON, value)));
            print("after");
            // roll for hero's location
            switch (Diceroller.Instance.RolldX(6))
            {
                case 1:
                    io.Position = new Vector2(1, 1);
                    break;
                case 2:
                    io.Position = new Vector2(7, 1);
                    break;
                case 3:
                    io.Position = new Vector2(8, 1);
                    break;
                case 4:
                    io.Position = new Vector2(13, 1);
                    break;
                case 5:
                    io.Position = new Vector2(15, 1);
                    break;
                case 6:
                    io.Position = new Vector2(18, 1);
                    break;
            }
            WorldController.Instance.CenterOnHex(io.Position);
            print("center on " + io.Position);
        }
        private void EquipItemOnFreshIo(BPInteractiveObject src, int slot, BPInteractiveObject item)
        {
            if (src.HasIOFlag(IoGlobals.IO_01_PC))
            {
                src.PcData.SetEquippedItem(slot, item);
            }
            else if (src.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                src.NpcData.SetEquippedItem(slot, item);
            }
            print("item equipped");
        }
    }
}
