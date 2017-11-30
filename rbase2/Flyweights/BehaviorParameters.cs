using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Constants;

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
        public BehaviorParameters(String initParams, float bParam)
        {
            BehaviorParam = bParam;
            String[] split = initParams.Split(' ');
            for (int i = split.Length - 1; i >= 0; i--)
            {
                if (String.Equals(split[i], "STACK", StringComparison.OrdinalIgnoreCase))
                {
                    Action = "STACK";
                    break;
                }
                if (String.Equals(split[i], "UNSTACK", StringComparison.OrdinalIgnoreCase))
                {
                    Action = "UNSTACK";
                    break;
                }
                if (String.Equals(split[i], "UNSTACKALL", StringComparison.OrdinalIgnoreCase))
                {
                    Action = "UNSTACKALL";
                    break;
                }
                if (String.Equals(split[i], "L", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_LOOK_AROUND.GetFlag());
                }
                if (String.Equals(split[i], "S", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_SNEAK.GetFlag());
                }
                if (String.Equals(split[i], "D", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_DISTANT.GetFlag());
                }
                if (String.Equals(split[i], "M", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_MAGIC.GetFlag());
                }
                if (String.Equals(split[i], "F", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_FIGHT.GetFlag());
                }
                if (String.Equals(split[i], "A", StringComparison.OrdinalIgnoreCase))
                {
                    AddFlag(Behaviour.BEHAVIOUR_STARE_AT.GetFlag());
                }
                if (String.Equals(split[i], "0", StringComparison.OrdinalIgnoreCase)
                    || String.Equals(split[i], "1", StringComparison.OrdinalIgnoreCase)
                    || String.Equals(split[i], "2", StringComparison.OrdinalIgnoreCase))
                {
                    Tactics = 0;
                }
                if (String.Equals(split[i], "GO_HOME", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_GO_HOME.GetFlag());
                }
                if (String.Equals(split[i], "FRIENDLY", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_FRIENDLY.GetFlag());
                    Movemode = IoGlobals.NOMOVEMODE;
                }
                if (String.Equals(split[i], "MOVE_TO", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_MOVE_TO.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (String.Equals(split[i], "FLEE", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_FLEE.GetFlag());
                    Movemode = IoGlobals.RUNMODE;
                }
                if (String.Equals(split[i], "LOOK_FOR", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_LOOK_FOR.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (String.Equals(split[i], "HIDE", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_HIDE.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (String.Equals(split[i], "WANDER_AROUND", StringComparison.OrdinalIgnoreCase))
                {
                    ClearFlags();
                    AddFlag(Behaviour.BEHAVIOUR_WANDER_AROUND.GetFlag());
                    Movemode = IoGlobals.WALKMODE;
                }
                if (String.Equals(split[i], "GUARD", StringComparison.OrdinalIgnoreCase))
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
         * Determines if the {@link BaseInteractiveObject} has a specific flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
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
