using Assets.Scripts.BarbarianPrince.Constants;
using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.BarbarianPrince.Turn;
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
            GameController.Instance.StartLoad(GameController.STATE_GAME);
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
            GameController.Instance.StopLoad();
            // display beginning message
            GameController.Instance.ShowMessage("Evil events have overtaken your Northlands Kingdom. Your father, the old king, is dead - assassinated by rivals to the throne. These usurpers now hold the palace with their mercenary royal guard. You have escaped, and must collect 500 gold pieces to raise a force to smash them and retake your heritage. Furthermore, the usurpers have powerful friends overseas. If you can't return to take them out in ten weeks, their allies will arm and you will lose your kingdom forever.\n\nTo escape the mercenary and royal guard, your loyal body servant Ogab smuggled you into a merchant caravan to the southern border. Now, at dawn you roll out of the merchant wagons into a ditch, dust off your clothes, loosen your swordbelt, and get ready to start the first day of your adventure.");
            TimeTrack.Instance.NextPhase();
        }
        private void EquipItemOnFreshIo(BPInteractiveObject src, int slot, BPInteractiveObject item)
        {
            if (src.HasIOFlag(IoGlobals.IO_01_PC))
            {
                print(item.RefId);
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
