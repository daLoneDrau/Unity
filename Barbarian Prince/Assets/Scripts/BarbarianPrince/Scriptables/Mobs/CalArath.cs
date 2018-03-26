using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.BarbarianPrince.Singletons;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Scriptables.Mobs
{
    public class CalArath: MobBase
    {
        public override int OnInit()
        {
            Console.WriteLine("CalArath oninit");
            Debug.Log("CalArath ONINIT");
            BPCharacter pc = (BPCharacter)Io.PcData;
            pc.SetBaseAttributeScore("CS", 0);
            pc.SetBaseAttributeScore("EN", 9);
            pc.AdjustGold(TreasureTable.Instance.AwardTreasure(Constants.WealthCode.WC2));
            pc.SetBaseAttributeScore("WI", Diceroller.Instance.RolldXPlusY(5, 1));
            //BPServiceClient.Instance.GetItemByName("Bonebiter");
            return base.OnInit();
        }
    }
}
