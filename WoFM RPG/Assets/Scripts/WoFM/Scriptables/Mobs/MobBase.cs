using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI.SceneControllers;

namespace WoFM.Scriptables.Mobs
{
    public class MobBase : WoFMScriptable
    {
        public override int OnInit()
        {
            // Debug.Log("Mob ONINIT");
            return base.OnInit();
        }
        public override int OnInView()
        {
            Io.Show = 1;
            return base.OnInView();
        }
        public override int OnOutOfView()
        {
            if (Particles.Instance.SnoreTarget == Io.RefId)
            {
                Particles.Instance.StopSnoring();
            }
            Io.transform.position = new Vector3(-1, 0, 0);
            Io.Show = 0;
            return base.OnOutOfView();
        }
    }
}
