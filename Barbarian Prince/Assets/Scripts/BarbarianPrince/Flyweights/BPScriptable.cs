using RPGBase.Constants;
using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Flyweights
{
    public class BPScriptable : Scriptable
    {
        public virtual int OnTimeChange()
        {
            return ScriptConsts.ACCEPT;
        }
    }
}
