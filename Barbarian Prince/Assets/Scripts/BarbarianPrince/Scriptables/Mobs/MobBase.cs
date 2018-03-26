using Assets.Scripts.BarbarianPrince.Flyweights;
using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Scriptables.Mobs
{
    public class MobBase : BPScriptable
    {
        public override int OnInit()
        {
            Console.WriteLine("Mob ONINIT");
            Debug.Log("Mob ONINIT");
            return base.OnInit();
        }
    }
}
