using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;

namespace WoFM.Scriptables.Mobs
{
    public class MobBase : WoFMScriptable
    {
        public override int OnInit()
        {
            // Debug.Log("Mob ONINIT");
            return base.OnInit();
        }
    }
}
