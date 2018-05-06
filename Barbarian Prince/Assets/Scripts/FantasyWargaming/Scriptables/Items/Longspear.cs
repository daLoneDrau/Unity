using Assets.Scripts.FantasyWargaming.Flyweights;
using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Scriptables.Items
{
    public class Longspear:WeaponBase
    {
        public override int OnInit()
        {
            Console.WriteLine("Longspear ONINIT");
            FWItemData item = (FWItemData)Io.ItemData;
            item.ItemName = "Long spear";
            item.SetObjectType(EquipmentGlobals.OBJECT_TYPE_2H);
            item.MinMeleePhysique = 13;
            item.MinMeleeAgility = 13;
            item.Range = 6;
            item.Dice = Dice.ONE_D4;
            item.DmgModifier = 4;
            item.Parry = false;
            item.Weight = 6;
            item.BreakCode = 'B';
            item.Price = 20;
            return base.OnInit();
        }
    }
}
