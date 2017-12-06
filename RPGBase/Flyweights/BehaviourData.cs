using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public sealed class BehaviourData
    {
        /** the parameter applied to a behavior. */
        public float BehaviorParam { get; set; }
        /** the behavior flag that has been set. */
        public long Behaviour { get; set; }
        /** flag indicating whether the behavior exists. */
        public bool Exists { get; set; }
        /** the movement mode. */
        public int MoveMode { get; set; }
        /** tactics for the behavior; e.g., 0=none, 1=side, 2=side + back, etc... */
        public int Tactics { get; set; }
        /** the behavior target. */
        public int Target { get; set; }
        // ANIM_USE animlayer[MAX_ANIM_LAYERS];
    }
}
