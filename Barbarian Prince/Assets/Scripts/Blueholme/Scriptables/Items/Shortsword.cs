using Assets.Scripts.Blueholme.Flyweights;
using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Scriptables.Items
{
    public class Shortsword : WeaponBase
    {
        public override int OnInit()
        {
            Console.WriteLine("Shortsword ONINIT");
            BHItemData item = (BHItemData)Io.ItemData;
            item.ItemName = "Short sword";
            item.SetObjectType(EquipmentGlobals.OBJECT_TYPE_1H);
            item.Dice = Dice.ONE_D4;
            item.DmgModifier = 0;
            item.Weight = 7;
            item.Price = 7;
            return base.OnInit();
        }
    }
}
