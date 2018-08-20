using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LabLord.Flyweights;

namespace LabLord.Scriptables.Mobs
{
    public class MobBase : LabLordScriptable
    {
        public override int OnInit()
        {
            Console.WriteLine("Mob ONINIT");
            Debug.Log("Mob ONINIT");
            return base.OnInit();
        }
    }
}
