using System;

namespace RPGBase.Constants
{
    public static class BehaviourExtensions
    {
        public static int GetFlag(this Behaviour behaviour)
        {
            return (int)Math.Pow(2, (int)behaviour);
        }
    }
    public enum Behaviour
    {
        /// <summary>
        /// 
        /// </summary>
        BEHAVIOUR_NONE,
        BEHAVIOUR_FRIENDLY,
        BEHAVIOUR_MOVE_TO,
        BEHAVIOUR_WANDER_AROUND,
        BEHAVIOUR_FLEE,
        BEHAVIOUR_HIDE,
        BEHAVIOUR_LOOK_FOR,
        BEHAVIOUR_SNEAK,
        BEHAVIOUR_FIGHT,
        BEHAVIOUR_DISTANT,
        BEHAVIOUR_MAGIC,
        BEHAVIOUR_GUARD,
        BEHAVIOUR_GO_HOME,
        BEHAVIOUR_LOOK_AROUND,
        BEHAVIOUR_STARE_AT
    }
}