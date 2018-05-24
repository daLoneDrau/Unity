using RPGBase.Flyweights;
using System;
using UnityEngine;

namespace RPGBase.Singletons
{
    public class Combat: MonoBehaviour, ICombat
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static Combat Instance { get; protected set; }

        public virtual float ComputeDamages(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo, BaseInteractiveObject targetIo, int result)
        {
            throw new NotImplementedException();
        }

        public virtual bool StrikeCheck(BaseInteractiveObject srcIo, BaseInteractiveObject wpnIo, long flags, int targ)
        {
            throw new NotImplementedException();
        }
    }
}
