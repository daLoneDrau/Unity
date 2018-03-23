using Assets.Scripts.BarbarianPrince.Flyweights;
using RPGBase.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Scriptables.Mobs
{
    public class MobBase : BPScriptable
    {
        public override int OnInit()
        {
            Console.WriteLine("Mob ONINIT");
            return base.OnInit();
        }
    }
}
