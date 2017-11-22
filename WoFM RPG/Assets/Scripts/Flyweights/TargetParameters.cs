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
         * @throws RPGException 
         */
        public TargetParameters( String initParams) throws RPGException
        {
            String []
            split = initParams.split(" ");
        for (int i = split.length - 1; i >= 0; i--) {
            if (split[i].startsWith("-")) {
                if (split[i].toUpperCase().contains("S")) {
                    this.addFlag(ScriptConstants.PATHFIND_ONCE);
    }
                if (split[i].toUpperCase().contains("A")) {
                    this.addFlag(ScriptConstants.PATHFIND_ALWAYS);
                }
                if (split[i].toUpperCase().contains("N")) {
                    this.addFlag(ScriptConstants.PATHFIND_NO_UPDATE);
                }
}
            if (split[i].equalsIgnoreCase("PATH")) {
                targetInfo = -2;
            }
            if (split[i].equalsIgnoreCase("PLAYER")) {
                targetInfo = Interactive.getInstance().getTargetByNameTarget(
                        "PLAYER");
            }
            if (split[i].equalsIgnoreCase("NONE")) {
                targetInfo = ScriptConstants.TARGET_NONE;
            }
            if (split[i].startsWith("NODE_")) {
                targetInfo = Interactive.getInstance().getTargetByNameTarget(
                        split[i].replace("NODE_", ""));
            }
            if (split[i].startsWith("OBJECT_")) {
                targetInfo = Interactive.getInstance().getTargetByNameTarget(
                        split[i].replace("OBJECT_", ""));
            }
            if (split[i].startsWith("ID_")) {
                int id = Integer.parseInt(split[i].replace("ID_", ""));
                if (Interactive.getInstance().hasIO(id)) {
                    targetInfo = id;
                }
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
 * @return the flags
 */
public int getFlags()
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
 * @param targetInfo the targetInfo to set
 */
public void setTargetInfo(int targetInfo)
{
    this.targetInfo = targetInfo;
}
    }
}
