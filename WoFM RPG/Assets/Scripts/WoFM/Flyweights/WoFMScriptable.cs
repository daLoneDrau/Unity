using RPGBase.Constants;
using RPGBase.Flyweights;

namespace WoFM.Flyweights
{
    public class WoFMScriptable : Scriptable
    {
        public virtual int OnRollStats()
        {
            return ScriptConsts.ACCEPT;
        }
    }
}