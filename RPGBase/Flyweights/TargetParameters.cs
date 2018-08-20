using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Constants;
using RPGBase.Singletons;

namespace RPGBase.Flyweights
{
    public class TargetParameters
    {
        private long flags;
        private int TargetInfo { get; set; }
        /**
         * @param initParams
         * @ 
         */
        public TargetParameters(String initParams)
        {
            TargetInfo = -1;
            String[] split = initParams.Split(' ');
            for (int i = split.Length - 1; i >= 0; i--)
            {
                if (split[i].StartsWith("-"))
                {
                    if (split[i].ToUpper().Contains("S"))
                    {
                        this.AddFlag(ScriptConsts.PATHFIND_ONCE);
                    }
                    if (split[i].ToUpper().Contains("A"))
                    {
                        this.AddFlag(ScriptConsts.PATHFIND_ALWAYS);
                    }
                    if (split[i].ToUpper().Contains("N"))
                    {
                        this.AddFlag(ScriptConsts.PATHFIND_NO_UPDATE);
                    }
                }
                if (String.Equals(split[i], "PATH", StringComparison.OrdinalIgnoreCase))
                {
                    TargetInfo = -2;
                }
                if (String.Equals(split[i], "PLAYER", StringComparison.OrdinalIgnoreCase))
                {
                    TargetInfo = Interactive.Instance.GetTargetByNameTarget("PLAYER");
                }
                if (String.Equals(split[i], "NONE", StringComparison.OrdinalIgnoreCase))
                {
                    TargetInfo = ScriptConsts.TARGET_NONE;
                }
                if (split[i].StartsWith("NODE_"))
                {
                    TargetInfo = Interactive.Instance.GetTargetByNameTarget(split[i].Replace("NODE_", ""));
                }
                if (split[i].StartsWith("OBJECT_"))
                {
                    TargetInfo = Interactive.Instance.GetTargetByNameTarget(split[i].Replace("OBJECT_", ""));
                }
                if (split[i].StartsWith("ID_"))
                {
                    int id = int.Parse(split[i].Replace("ID_", ""));
                    if (Interactive.Instance.HasIO(id))
                    {
                        TargetInfo = id;
                    }
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
