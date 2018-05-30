using Assets.Scripts.Blueholme.Flyweights;
using Assets.Scripts.Blueholme.Globals;
using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Scriptables.Items
{
    public class Handaxe : WeaponBase
    {
        public override int OnInit()
        {
            Console.WriteLine("Handaxe ONINIT");
            UnityEngine.Debug.Log("Handaxe ONINIT");
            BHItemData item = (BHItemData)Io.ItemData;
            item.ItemName = "Hand axe";
            item.SetObjectType(EquipmentGlobals.OBJECT_TYPE_1H);
            item.WeaponSpeed = BHGlobals.MEDIUM_WEAPON;
            item.Dice = Dice.ONE_D6;
            item.DmgModifier = 0;
            item.Weight = 7;
            item.Price = 3;
            return base.OnInit();
        }
    }
}
