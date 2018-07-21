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
        /// <summary>
        /// Gets the modifier applied to the damage amount for a successful critical hit.
        /// </summary>
        /// <returns><see cref="float"/></returns>
        protected virtual float ApplyCriticalModifier()
        {
            return 1;
        }
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
