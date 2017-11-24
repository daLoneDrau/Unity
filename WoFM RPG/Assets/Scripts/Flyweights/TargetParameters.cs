using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class TargetParameters
    {
        private int flags;
        private int targetInfo = -1;
        /**
         * @param initParams
         * @ 
         */
        public TargetParameters( String initParams) 
        {
            String []
            split = initParams.Split(" ");
        for (int i = split.Length - 1; i >= 0; i--) {
            if (split[i].startsWith("-")) {
                if (split[i].toUpperCase().contains("S")) {
                    this.AddFlag(ScriptConsts.PATHFIND_ONCE);
    }
                if (split[i].toUpperCase().contains("A")) {
                    this.AddFlag(ScriptConsts.PATHFIND_ALWAYS);
                }
                if (split[i].toUpperCase().contains("N")) {
                    this.AddFlag(ScriptConsts.PATHFIND_NO_UPDATE);
                }
}
            if (split[i].equalsIgnoreCase("PATH")) {
                targetInfo = -2;
            }
            if (split[i].equalsIgnoreCase("PLAYER")) {
                targetInfo = Interactive.GetInstance().getTargetByNameTarget(
                        "PLAYER");
            }
            if (split[i].equalsIgnoreCase("NONE")) {
                targetInfo = ScriptConsts.TARGET_NONE;
            }
            if (split[i].startsWith("NODE_")) {
                targetInfo = Interactive.GetInstance().getTargetByNameTarget(
                        split[i].replace("NODE_", ""));
            }
            if (split[i].startsWith("OBJECT_")) {
                targetInfo = Interactive.GetInstance().getTargetByNameTarget(
                        split[i].replace("OBJECT_", ""));
            }
            if (split[i].startsWith("ID_")) {
                int id = Integer.parseInt(split[i].replace("ID_", ""));
                if (Interactive.GetInstance().hasIO(id)) {
                    targetInfo = id;
                }
            }
        }
    }
    /**
     * Adds a flag.
     * @param flag the flag
     */
    public void AddFlag( long flag)
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
public int GetFlags()
{
    return flags;
}
/**
 * @return the targetInfo
 */
public int getTargetInfo()
{
    return targetInfo;
}
/**
 * Determines if the {@link BaseInteractiveObject} has a specific flag.
 * @param flag the flag
 * @return true if the {@link BaseInteractiveObject} has the flag; false
 *         otherwise
 */
public  bool HasFlag( long flag)
{
    return (flags & flag) == flag;
}
/**
 * Removes a flag.
 * @param flag the flag
 */
public  void RemoveFlag( long flag)
{
    flags &= ~flag;
}
/**
 * @param targetInfo the targetInfo to set
 */
public void setTargetInfo(int targetInfo)
{
    this.targetInfo = targetInfo;
}
    }
}
