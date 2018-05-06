using RPGBase.Constants;
using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Flyweights
{
    public class FWScriptable : Scriptable
    {
        /// <summary>
        /// On being given a Morale Check.
        /// </summary>
        /// <returns></returns>
        public virtual int OnMoraleCheck()
        {
            /*
             * already verified base class methods called.
            Console.WriteLine("FWScriptable OnMoraleCheck");
            Debug.Log("FWScriptable OnMoraleCheck");
            */
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On being given a Berserk Check.
        /// </summary>
        /// <returns></returns>
        public virtual int OnBerserkCheck()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnTimeChange()
        {
            return ScriptConsts.ACCEPT;
        }
    }
}
