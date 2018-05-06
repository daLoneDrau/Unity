using Assets.Scripts.FantasyWargaming.Flyweights;
using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Scriptables.Items
{
    public class Shortspear : WeaponBase
    {
        public override int OnInit()
        {
            Console.WriteLine("Shortspear ONINIT");
            FWItemData item = (FWItemData)Io.ItemData;
            item.ItemName = "Short spear";
            item.SetObjectType(EquipmentGlobals.OBJECT_TYPE_2H);
            item.MinMeleePhysique = 12;
            item.MinMeleeAgility = 12;
            item.Dice = Dice.ONE_D4;
            item.DmgModifier = 3;
            item.Parry = false;
            item.Range = 5;
            item.Weight = 5;
            item.BreakCode = 'B';
            item.Price = 20;
            return base.OnInit();
        }
    }
}
