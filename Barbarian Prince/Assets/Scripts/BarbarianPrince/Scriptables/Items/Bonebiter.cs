using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Scriptables.Items
{
    public class Bonebiter : WeaponBase
    {
        public override int OnInit()
        {
            Console.WriteLine("Bonebiter oninit");
            // set local variables
            SetLocalVariable("reagent", "none");
            SetLocalVariable("poisonable", 1);
            return base.OnInit();
        }
    }
}
