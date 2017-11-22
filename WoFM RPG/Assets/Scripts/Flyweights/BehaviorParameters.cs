using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class BehaviorParameters
    {
        private char[] action;
        private float behaviorParam;
        private int flags;
        private int movemode = -1;
        private int tactics = -1;
        private int targetInfo = -1;
        private char[] targetName;
        /**
         * Creates a new instance of {@link BehaviorParameters}.
         * @param initParams a list of string parameters,
         * such as STACK, L, FRIENDLY, etc...
         * @param bParam the parameter applied to the behavior
         */
        public BehaviorParameters( String initParams,  float bParam)
        {
            behaviorParam = bParam;
            String[] split = initParams.split(" ");
            for (int i = split.length - 1; i >= 0; i--)
            {
                if (split[i].equalsIgnoreCase("STACK"))
                {
                    setAction("STACK");
                    break;
                }
                if (split[i].equalsIgnoreCase("UNSTACK"))
                {
                    setAction("UNSTACK");
                    break;
                }
                if (split[i].equalsIgnoreCase("UNSTACKALL"))
                {
                    setAction("UNSTACKALL");
                    break;
                }
                if (split[i].equalsIgnoreCase("L"))
                {
                    addFlag(Behaviour.BEHAVIOUR_LOOK_AROUND.getFlag());
                }
                if (split[i].equalsIgnoreCase("S"))
                {
                    addFlag(Behaviour.BEHAVIOUR_SNEAK.getFlag());
                }
                if (split[i].equalsIgnoreCase("D"))
                {
                    addFlag(Behaviour.BEHAVIOUR_DISTANT.getFlag());
                }
                if (split[i].equalsIgnoreCase("M"))
                {
                    addFlag(Behaviour.BEHAVIOUR_MAGIC.getFlag());
                }
                if (split[i].equalsIgnoreCase("F"))
                {
                    addFlag(Behaviour.BEHAVIOUR_FIGHT.getFlag());
                }
                if (split[i].equalsIgnoreCase("A"))
                {
                    addFlag(Behaviour.BEHAVIOUR_STARE_AT.getFlag());
                }
                if (split[i].equalsIgnoreCase("0")
                        || split[i].equalsIgnoreCase("1")
                        || split[i].equalsIgnoreCase("2"))
                {
                    tactics = 0;
                }
                if (split[i].equalsIgnoreCase("GO_HOME"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_GO_HOME.getFlag());
                }
                if (split[i].equalsIgnoreCase("FRIENDLY"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_FRIENDLY.getFlag());
                    movemode = IoGlobals.NOMOVEMODE;
                }
                if (split[i].equalsIgnoreCase("MOVE_TO"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_MOVE_TO.getFlag());
                    movemode = IoGlobals.WALKMODE;
                }
                if (split[i].equalsIgnoreCase("FLEE"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_FLEE.getFlag());
                    movemode = IoGlobals.RUNMODE;
                }
                if (split[i].equalsIgnoreCase("LOOK_FOR"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_LOOK_FOR.getFlag());
                    movemode = IoGlobals.WALKMODE;
                }
                if (split[i].equalsIgnoreCase("HIDE"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_HIDE.getFlag());
                    movemode = IoGlobals.WALKMODE;
                }
                if (split[i].equalsIgnoreCase("WANDER_AROUND"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_WANDER_AROUND.getFlag());
                    movemode = IoGlobals.WALKMODE;
                }
                if (split[i].equalsIgnoreCase("GUARD"))
                {
                    clearFlags();
                    addFlag(Behaviour.BEHAVIOUR_GUARD.getFlag());
                    movemode = IoGlobals.NOMOVEMODE;
                    targetInfo = -2;
                }
            }
        }
        /**
         * Adds a flag.
         * @param flag the flag
         */
        public void addFlag( long flag)
        {
            flags |= flag;
        }
        private void clearFlags()
        {
            flags = 0;
        }
        /**
         * @return the speechName
         */
        public String getAction()
        {
            String s = null;
            if (action != null)
            {
                s = new String(action);
            }
            return s;
        }
        /**
         * @return the behaviorParam
         */
        public float getBehaviorParam()
        {
            return behaviorParam;
        }
        /**
         * @return the flags
         */
        public int getFlags()
        {
            return flags;
        }
        /**
         * @return the movemode
         */
        public int getMovemode()
        {
            return movemode;
        }
        /**
         * @return the tactics
         */
        public int getTactics()
        {
            return tactics;
        }
        /**
         * @return the targetInfo
         */
        public int getTargetInfo()
        {
            return targetInfo;
        }
        /**
         * @return the speechName
         */
        public String getTargetName()
        {
            String s = null;
            if (targetName != null)
            {
                s = new String(targetName);
            }
            return s;
        }
        /**
         * Determines if the {@link BaseInteractiveObject} has a specific flag.
         * @param flag the flag
         * @return true if the {@link BaseInteractiveObject} has the flag; false
         *         otherwise
         */
        public  bool hasFlag( long flag)
        {
            return (flags & flag) == flag;
        }
        /**
         * Removes a flag.
         * @param flag the flag
         */
        public  void removeFlag( long flag)
        {
            flags &= ~flag;
        }
        /**
         * @param val the value to set
         */
        public void setAction(String val)
        {
            if (val != null)
            {
                action = val;
            }
        }
        /**
         * @param movemode the movemode to set
         */
        public void setMovemode(int movemode)
        {
            this.movemode = movemode;
        }
        /**
         * @param tactics the tactics to set
         */
        public void setTactics(int tactics)
        {
            this.tactics = tactics;
        }
        /**
         * @param targetInfo the targetInfo to set
         */
        public void setTargetInfo(int targetInfo)
        {
            this.targetInfo = targetInfo;
        }
        /**
         * @param val the value to set
         */
        public void setTargetName(String val)
        {
            if (val != null)
            {
                targetName = val;
            }
        }
    }
}
