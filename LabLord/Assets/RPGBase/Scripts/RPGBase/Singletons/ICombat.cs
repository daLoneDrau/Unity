using RPGBase.Flyweights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Singletons
{
    public interface ICombat
    {
        float ComputeDamages(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo, BaseInteractiveObject targetIo, long flags);
        void DamageFix(BaseInteractiveObject io, float dmg, long source, long flags);
        bool StrikeCheck(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo, long flags, int targ);
    }
}
