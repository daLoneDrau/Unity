using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;

namespace WoFM.Scriptables.Mobs
{
    public class Hero : MobBase
    {
        public override int OnInit()
        {
            // Debug.Log("Hero ONINIT");
            WoFMCharacter pc = (WoFMCharacter)Io.PcData;
            // roll stats
            OnRollStats();
			// equip sword
            //BPServiceClient.Instance.GetItemByName("Bonebiter");
            return base.OnInit();
        }
		public override int OnRollStats()
        {
            WoFMCharacter pc = (WoFMCharacter)Io.PcData;
            // SKILL is 6 + 1D6
            // Debug.Log("ROLL SKILL");
            pc.SetBaseAttributeScore("SKL", 6 + Diceroller.Instance.RolldX(6));
            // Debug.Log("ROLL MAXSKILL");
            pc.SetBaseAttributeScore("MSK", pc.GetBaseAttributeScore("SKL"));

            // STAMINA is 12 + 2D6
            pc.SetBaseAttributeScore("STM", 12 + Diceroller.Instance.RollXdY(2, 6));
            pc.SetBaseAttributeScore("MSTM", pc.GetBaseAttributeScore("STM"));

            // LUCK is 6 + 1D6
            pc.SetBaseAttributeScore("LUK", 6 + Diceroller.Instance.RolldX(6));
            pc.SetBaseAttributeScore("MLK", pc.GetBaseAttributeScore("LUK"));

            pc.ComputeFullStats();
            // Debug.Log("HEAL");
            pc.HealPlayer(999, true);

            return base.OnRollStats();
        }
    }
}
