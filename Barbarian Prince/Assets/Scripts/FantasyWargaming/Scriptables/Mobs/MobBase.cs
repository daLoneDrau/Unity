using Assets.Scripts.FantasyWargaming.Flyweights;
using RPGBase.Constants;
using System;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Scriptables.Mobs
{
    public class MobBase : FWScriptable
    {
        /// <summary>
        /// On being given a Morale Check.
        /// </summary>
        /// <returns></returns>
        public override int OnMoraleCheck()
        {
            return base.OnMoraleCheck();
        }
        public override int OnInit()
        {
            // Debug.Log("Mob ONINIT");
            return base.OnInit();
        }
    }
}
