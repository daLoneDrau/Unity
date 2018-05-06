using Assets.Scripts.FantasyWargaming.Flyweights;
using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FantasyWargaming.Scriptables.Items
{
    public class Shortsword : WeaponBase
    {
        public override int OnInit()
        {
            Console.WriteLine("Shortsword ONINIT");
            FWItemData item = (FWItemData)Io.ItemData;
            item.ItemName = "Short sword";
            item.SetObjectType(EquipmentGlobals.OBJECT_TYPE_1H);
            item.MinMeleePhysique = 11;
            item.MinMeleeAgility = 12;
            item.Dice = Dice.ONE_D4;
            item.DmgModifier = 2;
            item.Parry = true;
            item.ParryModifier = 2;
            item.Range = 4;
            item.Weight = 7;
            item.BreakCode = 'B';
            item.Price = 30;
            return base.OnInit();
        }
    }
}
