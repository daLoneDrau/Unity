using RPGBase.Constants;
using RPGBase.Flyweights;

namespace LabLord.Flyweights
{
    public class LabLordScriptable : Scriptable
    {
        public virtual int OnCharWizardStepOne()
        {
            return ScriptConsts.ACCEPT;
        }
    }
}