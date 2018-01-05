using RPGBase.Constants;
using System;

namespace RPGBase.Flyweights
{
    public class BehaviorParameters
    {
        public string Action { get; set; }
        public float BehaviorParam { get; }
        private long flags;
        public int Movemode { get; set; } = -1;
        public int Tactics { get; set; } = -1;
        public int TargetInfo { get; set; } = -1;
        public string TargetName { get; set; }
        /**
         * Creates a new instance of {@link BehaviorParameters}.
         * @param initParams a list of string parameters,
         * such as STACK, L, FRIENDLY, etc...
         * @param bParam the parameter applied to the behavior
         */
        /// <summary>
        /// Creates a new instance of {@link BehaviorParameters}.
        /// </summary>
        /// <param name="initParams"></param>
        /// <param name="bParam"></param>
        public BehaviorParameters(string initParams, float bParam)
        {
            BehaviorParam = bParam;
            string[] split = initParams.Split(' ');
            for (int i = split.Length - 1; i >= 0; i--)
            {
                if (string.Equals(split[i], "STACK", StringComparison.OrdinalIgnoreCase))
                {
                    Action = "STACK";
                    break;
                }
                if (string.Equals(split[i], "UNSTACK", StringComparison.OrdinalIgnoreCase))
                {
                    Action = "UNSTACK";
                    break;
                }
                if (string.Equals(split[i], "UNSTACKALL", StringComparison.OrdinalIgnoreCase))
                {
                    Action = "UNSTACKALL";
                    break;
                }
                if (string.Equals(split[i], "L", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_LOOK_AROUND.GetFlag());
                }
                if (string.Equals(split[i], "S", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_SNEAK.GetFlag());
                }
                if (string.Equals(split[i], "D", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_DISTANT.GetFlag());
                }
                if (string.Equals(split[i], "M", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_MAGIC.GetFlag());
                }
                if (string.Equals(split[i], "F", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_FIGHT.GetFlag());
                }
                if (string.Equals(split[i], "A", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_STARE_AT.GetFlag());
                }
                if (string.Equals(split[i], "0", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(split[i], "1", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(split[i], "2", StringComparison.OrdinalIgnoreCase))
                {
                    Tactics = 0;
                }
                if (string.Equals(split[i], "GO_HOME", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_GO_HOME.GetFlag());
                }
                if (string.Equals(split[i], "FRIENDLY", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_FRIENDLY.GetFlag());
                    Movemode = IoGlobals.NOMOVEMODE;
                }
                if (string.Equals(split[i], "MOVE_TO", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_MOVE_TO.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (string.Equals(split[i], "FLEE", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_FLEE.GetFlag());
                    Movemode = IoGlobals.RUNMODE;
                }
                if (string.Equals(split[i], "LOOK_FOR", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_LOOK_FOR.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (string.Equals(split[i], "HIDE", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_HIDE.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (string.Equals(split[i], "WANDER_AROUND", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_WANDER_AROUND.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (string.Equals(split[i], "GUARD", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_GUARD.GetFlag());
                    Movemode = IoGlobals.NOMOVEMODE;
                    TargetInfo = -2;
                }
            }
        }
        /**
         * Adds a flag.
         * @param flag the flag
         */
        public void AddFlag(long flag)
        {
            flags |= flag;
        }
        private void ClearFlags()
        {
            flags = 0;
        }
        /**
         * @return the flags
         */
        public long GetFlags()
        {
            return flags;
        }
        /**
         * Determines if the <see cref="BaseInteractiveObject"/> has a specific flag.
         * @param flag the flag
         * @return true if the <see cref="BaseInteractiveObject"/> has the flag; false
         *         otherwise
         */
        public bool HasFlag(long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public void RemoveFlag(long flag)
        {
            flags &= ~flag;
        }
    }
}
