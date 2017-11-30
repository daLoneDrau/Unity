using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Flyweights;

namespace RPGBase.Systems
{
    public abstract class Script
    {
        private static int ANIM_TALK_ANGRY = 0;
        private static int ANIM_TALK_HAPPY = 0;
        private static int ANIM_TALK_NEUTRAL = 0;
        /** the one and only instance of the <tt>Script</tt> class. */
        private static Script instance;
        /** the maximum number of system parameters. */
        public static int MAX_SYSTEM_PARAMS = 5;
        /** the list of system parameters. */
        private static String[] SYSTEM_PARAMS = new String[MAX_SYSTEM_PARAMS];
        /**
         * Gives access to the singleton instance of {@link Script}.
         * @return {@link Script}
         */
        public static Script GetInstance()
        {
            return Script.instance;
        }
        /**
         * Sets the singleton instance.
         * @param i the instance to set
         */
        protected static void SetInstance(Script i)
        {
            Script.instance = i;
        }
        private bool ARXPausedTime;
        /** the flag indicating whether debug output is turned on. */
        private bool debug;
        private bool EDITMODE;
        private BaseInteractiveObject eventSender;
        private int eventTotalCount;
        private int GLOB;
        /** the list of global script variables. */
        private ScriptVariable[] gvars;
        /** the maximum number of timer scripts. */
        private int maxTimerScript = 0;
        private bool PauseScript;
        private int stackFlow = 8;
        /**
         * Adds an BaseInteractiveObject to a specific group.
         * @param io the BaseInteractiveObject
         * @param group the group name
         */
        public void addToGroup(BaseInteractiveObject io, String group)
        {
            if (io != null
                    && group != null)
            {
                io.AddGroup(group);
            }
        }
        public void allowInterScriptExecution()
        {
            int ppos = 0;

            if (!PauseScript && !EDITMODE && !ARXPausedTime)
            {
                this.eventSender = null;

                int numm = Math.min(Interactive.GetInstance().getMaxIORefId(), 10);

                for (int n = 0; n < numm; n++)
                {
                    int i = ppos;
                    ppos++;

                    if (ppos >= Interactive.GetInstance().getMaxIORefId())
                    {
                        ppos = 0;
                        break;
                    }
                    if (Interactive.GetInstance().hasIO(i))
                    {
                        BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                        if (io.HasGameFlag(IoGlobals.GFLAG_ISINTREATZONE))
                        {
                            if (io.getMainevent() != null)
                            {
                                sendIOScriptEvent(io, 0, null, io.getMainevent());
                            }
                            else
                            {
                                sendIOScriptEvent(
                                        io, ScriptConsts.SM_008_MAIN, null, null);
                            }
                        }
                    }
                }
            }
        }
        protected abstract void clearAdditionalEventStacks();
        protected abstract void clearAdditionalEventStacksForIO(BaseInteractiveObject io);
        /**
         * Clones all local variables from the source {@link BaseInteractiveObject} to the destination
         * {@link BaseInteractiveObject}.
         * @param src the source {@link BaseInteractiveObject}
         * @param dest the destination {@link BaseInteractiveObject}
         * @ if an error occurs
         */
        public void cloneLocalVars(BaseInteractiveObject src, BaseInteractiveObject dest)

        {
            if (dest != null
                    && src != null)
            {
                freeAllLocalVariables(dest);
                if (src.getScript().hasLocalVariables())
                {
                    int i = src.getScript().getLocalVarArrayLength() - 1;
                    for (; i >= 0; i--)
                    {
                        dest.getScript().setLocalVariable(new ScriptVariable(
                                src.getScript().getLocalVariable(i)));
                    }
                }
            }
        }
        /**
         * Count the number of active script timers.
         * @return <code>int</code>
         */
        public int countTimers()
        {
            int activeTimers = 0;
            TIMER[] scriptTimers = getScriptTimers();
            for (int i = scriptTimers.Length - 1; i >= 0; i--)
            {
                if (scriptTimers[i] != null
                        && scriptTimers[i].exists())
                {
                    activeTimers++;
                }
            }
            return activeTimers;
        }
        protected abstract void destroyScriptTimers();
        /**
         * Checks to see if a scripted event is disallowed.
         * @param msg the event message id
         * @param script the {@link Scriptable} script
         * @return <tt>true</tt> if the event is not allowed; <tt>false</tt>
         *         otherwise
         */
        private bool eventIsDisallowed(int msg,
                 Scriptable script)
        {
            bool disallowed = false;
            // check to see if message is for an event that was disabled
            switch (msg)
            {
                case ScriptConsts.SM_055_COLLIDE_NPC:
                    if (script.hasAllowedEvent(ScriptConsts.DISABLE_COLLIDE_NPC))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_010_CHAT:
                    if (script.hasAllowedEvent(ScriptConsts.DISABLE_CHAT))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_016_HIT:
                    if (script.hasAllowedEvent(ScriptConsts.DISABLE_HIT))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_028_INVENTORY2_OPEN:
                    if (script.hasAllowedEvent(
                            ScriptConsts.DISABLE_INVENTORY2_OPEN))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_046_HEAR:
                    if (script.hasAllowedEvent(ScriptConsts.DISABLE_HEAR))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_023_UNDETECTPLAYER:
                case ScriptConsts.SM_022_DETECTPLAYER:
                    if (script.hasAllowedEvent(ScriptConsts.DISABLE_DETECT))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_057_AGGRESSION:
                    if (script.hasAllowedEvent(
                            ScriptConsts.DISABLE_AGGRESSION))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_008_MAIN:
                    if (script.hasAllowedEvent(ScriptConsts.DISABLE_MAIN))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_073_CURSORMODE:
                    if (script.hasAllowedEvent(
                            ScriptConsts.DISABLE_CURSORMODE))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_074_EXPLORATIONMODE:
                    if (script.hasAllowedEvent(
                            ScriptConsts.DISABLE_EXPLORATIONMODE))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_061_KEY_PRESSED:
                    // float dwCurrTime = ARX_TIME_Get();
                    // if ((dwCurrTime - g_TimeStartCinemascope) < 3000) {
                    // return ScriptConsts.REFUSE;
                    // }
                    break;
                default:
                    break;
            }
            return disallowed;
        }
        public void eventStackClear()
        {
            for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
            {
                if (getStackedEvent(i).exists())
                {
                    getStackedEvent(i).setParams(null);
                    getStackedEvent(i).setEventname(null);
                    getStackedEvent(i).setSender(null);
                    getStackedEvent(i).setExist(false);
                    getStackedEvent(i).Io = null);
            getStackedEvent(i).setMsg(0);
        }
    }
    clearAdditionalEventStacks();
}
public void eventStackClearForIo(BaseInteractiveObject io)
{
    for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
    {
        if (getStackedEvent(i).exists()
                && io.Equals(getStackedEvent(i).getIo()))
        {
            getStackedEvent(i).setParams(null);
            getStackedEvent(i).setEventname(null);
            getStackedEvent(i).setSender(null);
            getStackedEvent(i).setExist(false);
            getStackedEvent(i).Io = null);
    getStackedEvent(i).setMsg(0);
}
    }
    clearAdditionalEventStacksForIO(io);
}
public void eventStackExecute()
{
    int count = 0;
    for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
    {
        if (getStackedEvent(i).exists())
        {
            int ioid = getStackedEvent(i).getIo().GetRefId();
            if (Interactive.GetInstance().hasIO(ioid))
            {
                if (getStackedEvent(i).getSender() != null)
                {
                    int senderid =
                            getStackedEvent(i).getSender().GetRefId();
                    if (Interactive.GetInstance().hasIO(senderid))
                    {
                        eventSender = getStackedEvent(i).getSender();
                    }
                    else
                    {
                        eventSender = null;
                    }
                }
                else
                {
                    eventSender = null;
                }
                sendIOScriptEvent(getStackedEvent(i).getIo(),
                        getStackedEvent(i).getMsg(),
                        getStackedEvent(i).getParams(),
                        getStackedEvent(i).getEventname());
            }
            getStackedEvent(i).setParams(null);
            getStackedEvent(i).setEventname(null);
            getStackedEvent(i).setSender(null);
            getStackedEvent(i).setExist(false);
            getStackedEvent(i).Io = null);
    getStackedEvent(i).setMsg(0);
    count++;
    if (count >= stackFlow)
    {
        break;
    }
}
    }
    executeAdditionalStacks();
}
public void eventStackExecuteAll()
{
    stackFlow = 9999999;
    eventStackExecute();
    stackFlow = 20;
}
public abstract void eventStackInit();
protected abstract void executeAdditionalStacks();
public void forceDeath(BaseInteractiveObject io, String target)

{
    int tioid = -1;
    if (target.equalsIgnoreCase("me")
            || target.equalsIgnoreCase("self"))
    {
        tioid = Interactive.GetInstance().GetInterNum(io);
    }
    else
    {
        tioid = Interactive.GetInstance().getTargetByNameTarget(target);
        if (tioid == -2)
        {
            tioid = Interactive.GetInstance().GetInterNum(io);
        }
    }
    if (tioid >= 0)
    {
        BaseInteractiveObject tio = (BaseInteractiveObject)Interactive.GetInstance().getIO(tioid);
        if (tio.HasIOFlag(IoGlobals.IO_03_NPC))
        {
            tio.getNPCData().forceDeath(io);
        }
    }
}
public void freeAllGlobalVariables()
{
    if (gvars != null)
    {
        for (int i = gvars.Length - 1; i >= 0; i--)
        {
            if (gvars[i] != null
                    && (gvars[i].getType() == ScriptConsts.TYPE_G_00_TEXT
                            || gvars[i]
                                    .getType() == ScriptConsts.TYPE_L_08_TEXT)
                    && gvars[i].getText() != null)
            {
                gvars[i].set(null);
            }
            gvars[i] = null;
        }
    }
}
/**
 * Removes all local variables from an {@link BaseInteractiveObject} and frees up the memory.
 * @param io the {@link BaseInteractiveObject}
 * @ if an error occurs
 */
public void freeAllLocalVariables(BaseInteractiveObject io)
{
    if (io != null
            && io.getScript() != null
            && io.getScript().hasLocalVariables())
    {
        int i = io.getScript().getLocalVarArrayLength() - 1;
        for (; i >= 0; i--)
        {
            if (io.getScript().getLocalVariable(i) != null
                    && (io.getScript().getLocalVariable(i)
                            .getType() == ScriptConsts.TYPE_G_00_TEXT
                            || io.getScript().getLocalVariable(i)
                                    .getType() == ScriptConsts.TYPE_L_08_TEXT)
                    && io.getScript().getLocalVariable(i)
                            .getText() != null)
            {
                io.getScript().getLocalVariable(i).set(null);
            }
            io.getScript().setLocalVariable(i, null);
        }
    }
}
/**
 * Gets the EVENT_SENDER global.
 * @return {@link BaseInteractiveObject}
 */
public BaseInteractiveObject getEventSender()
{
    return eventSender;
}
/**
 * Gets the value of a global floating-point array variable.
 * @param name the name of the variable
 * @return <code>float</code>[]
 * @ if the variable value was never set
 */
public float[] getGlobalFloatArrayVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_03_FLOAT_ARR)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global Float Array variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getFloatArrayVal();
}
/**
 * Gets the global floating-point value assigned to a specific variable.
 * @param name the variable name
 * @return <code>float</code>
 * @ if no such variable was assigned
 */
public float getGlobalFloatVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_02_FLOAT)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global Float variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getFloatVal();
}
/**
 * Gets the value of a global integer array variable.
 * @param name the name of the variable
 * @return <code>int</code>[]
 * @ if the variable value was never set
 */
public int[] getGlobalIntArrayVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_05_INT_ARR)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global Integer Array variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getIntArrayVal();
}
/**
 * Gets the value of a global integer variable.
 * @param name the name of the variable
 * @return <code>int</code>
 * @ if the variable value was never set
 */
public int getGlobalIntVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_04_INT)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global Integer variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getIntVal();
}
/**
 * Gets the value of a global long integer array variable.
 * @param name the name of the variable
 * @return <code>long</code>[]
 * @ if the variable value was never set
 */
public long[] getGlobalLongArrayVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_07_LONG_ARR)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global Long Integer Array variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getLongArrayVal();
}
/**
 * Gets the value of a global long integer variable.
 * @param name the name of the variable
 * @return <code>long</code>
 * @ if the variable value was never set
 */
public long getGlobalLongVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_06_LONG)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global Long Integer variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getLongVal();
}
/**
 * Gets the local text array value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @ if no such variable was assigned
 */
public String[] getGlobalStringArrayVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_01_TEXT_ARR)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global Text Array variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getTextArrayVal();
}
/**
 * Gets the global text value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @ if no such variable was assigned
 */
public String getGlobalStringVariableValue(String name)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    int index = -1;
    for (int i = 0; i < gvars.Length; i++)
    {
        if (gvars[i] != null
                && gvars[i].getName().Equals(name)
                && gvars[i].getType() == ScriptConsts.TYPE_G_00_TEXT)
        {
            index = i;
            break;
        }
    }
    if (index == -1)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("Global String variable ");
            sb.Append(name);
            sb.Append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.ToString());
        sb.ReturnToPool();
        sb = null;
        throw ex;
    }
    return gvars[index].getText();
}
public int getGlobalTargetParam(BaseInteractiveObject io)
{
    return io.getTargetinfo();
}
/**
 * Gets a global {@link Scriptable} variable.
 * @param name the name of the variable
 * @return {@link ScriptVariable}
 */
public ScriptVariable getGlobalVariable(String name)
{
    ScriptVariable var = null;
    for (int i = gvars.Length - 1; i >= 0; i--)
    {
        if (gvars[i] != null
                && gvars[i].getName() != null
                && gvars[i].getName().equalsIgnoreCase(name))
        {
            var = gvars[i];
            break;
        }
    }
    return var;
}
public BaseInteractiveObject getIOMaxEvents()
{
    int max = -1;
    int ionum = -1;
    BaseInteractiveObject io = null;
    int i = Interactive.GetInstance().getMaxIORefId();
    for (; i >= 0; i--)
    {
        if (Interactive.GetInstance().hasIO(i))
        {
            BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
            if (hio.getStatCount() > max)
            {
                ionum = i;
                max = hio.getStatCount();
            }
            hio = null;
        }
    }
    if (max > 0
            && ionum > -1)
    {
        io = (BaseInteractiveObject)Interactive.GetInstance().getIO(ionum);
    }
    return io;
}
public BaseInteractiveObject getIOMaxEventsSent()
{
    int max = -1;
    int ionum = -1;
    BaseInteractiveObject io = null;
    int i = Interactive.GetInstance().getMaxIORefId();
    for (; i >= 0; i--)
    {
        if (Interactive.GetInstance().hasIO(i))
        {
            BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
            if (hio.getStatSent() > max)
            {
                ionum = i;
                max = hio.getStatSent();
            }
        }
    }
    if (max > 0
            && ionum > -1)
    {
        io = (BaseInteractiveObject)Interactive.GetInstance().getIO(ionum);
    }
    return io;
}
/**
 * Gets the maximum number of timer scripts.
 * @return <code>int</code>
 */
public int getMaxTimerScript()
{
    return maxTimerScript;
}
/**
 * Gets a script timer.
 * @param id the timer's id
 * @return {@link TIMER}
 */
public abstract TIMER getScriptTimer(int id);
/**
 * Gets the script timers.
 * @return {@link TIMER}[]
 */
protected abstract TIMER[] getScriptTimers();
/**
 * Gets the stacked event at a specific index.
 * @param index the index
 * @return {@link STACKED}
 */
protected abstract STACKED getStackedEvent(int index);
/**
 * Gets the id of a named script assigned to a specific BaseInteractiveObject.
 * @param io the BaseInteractiveObject
 * @param name the script's name
 * @return the script's id, if found. If no script exists, -1 is returned
 */
public int getSystemIOScript(BaseInteractiveObject io, String name)
{
    int index = -1;
    if (countTimers() > 0)
    {
        for (int i = 0; i < maxTimerScript; i++)
        {
            TIMER[] scriptTimers = getScriptTimers();
            if (scriptTimers[i].exists())
            {
                if (scriptTimers[i].getIo().Equals(io)
                        && scriptTimers[i].getName().equalsIgnoreCase(
                                name))
                {
                    index = i;
                    break;
                }
            }
        }
    }
    return index;
}
/**
 * Determines if a {@link Script} has local variable with a specific name.
 * @param name the variable name
 * @return <tt>true</tt> if the {@link Script} has the local variable;
 *         <tt>false</tt> otherwise
 */
public bool hasGlobalVariable(String name)
{
    return getGlobalVariable(name) != null;
}
public void initEventStats()
{
    eventTotalCount = 0;
    int i = Interactive.GetInstance().getMaxIORefId();
    for (; i >= 0; i--)
    {
        if (Interactive.GetInstance().hasIO(i))
        {
            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
            io.setStatCount(0);
            io.setStatSent(0);
        }
    }
}
protected abstract void initScriptTimers();
/**
 * Gets the flag indicating whether debug output is turned on.
 * @return <tt>true</tt> if the debug output is turned on; <tt>false</tt>
 *         otherwise
 */
public bool isDebug()
{
    return debug;
}
/**
 * Determines if an BaseInteractiveObject is in a specific group.
 * @param io the BaseInteractiveObject
 * @param group the group name
 * @return true if the BaseInteractiveObject is in the group; false otherwise
 */
public bool isIOInGroup(BaseInteractiveObject io, String group)
{
    bool val = false;
    if (io != null
            && group != null)
    {
        for (int i = 0; i < io.GetNumIOGroups(); i++)
        {
            if (group.equalsIgnoreCase(io.GetIOGroup(i)))
            {
                val = true;
                break;
            }
        }
    }
    return val;
}
public bool isPlayerInvisible(BaseInteractiveObject io)
{
    bool invisible = false;
    // if (inter.iobj[0]->invisibility > 0.3f) {
    // invisible = true;
    // }
    return invisible;
}
private void MakeSSEPARAMS(String params)
{
    for (int i = MAX_SYSTEM_PARAMS - 1; i >= 0; i--)
    {
        SYSTEM_PARAMS[i] = null;
    }
    if (params != null) {
        String[] split = params.Split(" ");
        for (int i = 0, len = split.Length - 1; i < len; i++)
        {
            if (i / 2 < MAX_SYSTEM_PARAMS)
            {
                SYSTEM_PARAMS[i] = split[i];
            }
            else
            {
                break;
            }
        }
    }
}
/**
 * Sends an event message to the BaseInteractiveObject.
 * @param io the BaseInteractiveObject
 * @param msg the message
 * @param params the script parameters
 * @return {@link int}
 * @ if an error occurs
 */
public int notifyIOEvent(BaseInteractiveObject io, int msg,
         String params)
{
    int acceptance = ScriptConsts.REFUSE;
    if (sendIOScriptEvent(io, msg, null, null) != acceptance)
    {
        switch (msg)
        {
            case ScriptConsts.SM_017_DIE:
                if (io != null && Interactive.GetInstance().hasIO(io))
                {
                    // TODO - set death color
                    // io->infracolor.b = 1.f;
                    // io->infracolor.g = 0.f;
                    // io->infracolor.r = 0.f;
                }
                break;
            default:
                break;
        }
        acceptance = ScriptConsts.ACCEPT;
    }
    return acceptance;
}
/**
 * Hides a target BaseInteractiveObject.
 * @param io the BaseInteractiveObject sending the event.
 * @param megahide if true, the target BaseInteractiveObject is "mega-hidden"
 * @param targetName the target's name
 * @param hideOn if true, the hidden flags are set; otherwise all hidden
 *            flags are removed
 * @ if an error occurs
 */
public void objectHide(BaseInteractiveObject io, bool megahide,
         String targetName, bool hideOn)
{
    int targetId =
            Interactive.GetInstance().getTargetByNameTarget(targetName);
    if (targetId == -2)
    {
        targetId = io.GetRefId();
    }
    if (Interactive.GetInstance().hasIO(targetId))
    {
        BaseInteractiveObject tio = (BaseInteractiveObject)Interactive.GetInstance().getIO(targetId);
        tio.RemoveGameFlag(IoGlobals.GFLAG_MEGAHIDE);
        if (hideOn)
        {
            if (megahide)
            {
                tio.AddGameFlag(IoGlobals.GFLAG_MEGAHIDE);
                tio.show = IoGlobals.SHOW_FLAG_MEGAHIDE);
            }
            else
            {
                tio.show = IoGlobals.SHOW_FLAG_HIDDEN);
            }
        }
        else if (tio.getShow() == IoGlobals.SHOW_FLAG_MEGAHIDE
              || tio.getShow() == IoGlobals.SHOW_FLAG_HIDDEN)
        {
            tio.show = IoGlobals.SHOW_FLAG_IN_SCENE);
            if (tio.HasIOFlag(IoGlobals.IO_03_NPC)
                    && tio.getNPCData().getBaseLife() <= 0f)
            {
                // tio.animlayer[0].cur_anim =
                // inter.iobj[t]->anims[ANIM_DIE];
                // tio.animlayer[1].cur_anim = NULL;
                // tio.animlayer[2].cur_anim = NULL;
                // tio.animlayer[0].ctime = 9999999;
            }
        }
    }
}
/**
 * Removes an BaseInteractiveObject from all groups to which it was assigned.
 * @param io the BaseInteractiveObject
 */
public void releaseAllGroups(BaseInteractiveObject io)
{
    while (io != null
            && io.GetNumIOGroups() > 0)
    {
        io.RemoveGroup(io.GetIOGroup(0));
    }
}
/**
 * Releases an event, clearing all variables.
 * @param event the scriptable event
 */
public void releaseScript(SCRIPTABLE event)
{
    if (event != null) {
            event.ClearLocalVariables();
    }
}
/**
 * Removes an BaseInteractiveObject from a group.
 * @param io the BaseInteractiveObject
 * @param group the group
 */
public void RemoveGroup(BaseInteractiveObject io, String group)
{
    if (io != null
            && group != null)
    {
        io.RemoveGroup(group);
    }
}
/**
 * Resets the object's script.
 * @param io the object
 * @param initialize if <tt>true</tt> and the object is script-loaded, it
 *            will be initialized again
 * @ if an error occurs
 */
public void reset(BaseInteractiveObject io, bool initialize)

{
    // Release Script Local Variables
    if (io.getScript().getLocalVarArrayLength() > 0)
    {
        int i = io.getScript().getLocalVarArrayLength() - 1;
        for (; i >= 0; i--)
        {
            if (io.getScript().getLocalVariable(i) != null)
            {
                io.getScript().getLocalVariable(i).set(null);
                io.getScript().setLocalVariable(i, null);
            }
        }
    }

    // Release Script Over-Script Local Variables
    if (io.getOverscript().getLocalVarArrayLength() > 0)
    {
        int i = io.getOverscript().getLocalVarArrayLength() - 1;
        for (; i >= 0; i--)
        {
            if (io.getOverscript().getLocalVariable(i) != null)
            {
                io.getOverscript().getLocalVariable(i).set(null);
                io.getOverscript().setLocalVariable(i, null);
            }
        }
    }
    if (!io.ScriptLoaded)
    {
        resetObject(io, initialize);
    }
}
/**
 * Resets all interactive objects.
 * @param initialize if <tt>true</tt> and an object is script-loaded, it
 *            will be initialized again
 * @ if an error occurs
 */
public void resetAll(bool initialize)
{
    int i = Interactive.GetInstance().getMaxIORefId();
    for (; i >= 0; i--)
    {
        if (Interactive.GetInstance().hasIO(i))
        {
            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
            if (!io.ScriptLoaded)
            {
                resetObject(io, initialize);
            }
        }
    }
}
/**
 * Resets the BaseInteractiveObject.
 * @param io the BaseInteractiveObject
 * @param initialize if <tt>true</tt>, the object needs to be initialized as
 *            well
 * @ if an error occurs
 */
public void resetObject(BaseInteractiveObject io, bool initialize)

{
    // Now go for Script INIT/RESET depending on Mode
    int num = Interactive.GetInstance().GetInterNum(io);
    if (Interactive.GetInstance().hasIO(num))
    {
        BaseInteractiveObject objIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
        if (objIO != null
                && objIO.getScript() != null)
        {
            objIO.getScript().ClearDisallowedEvents();

            if (initialize)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getScript(),
                        ScriptConsts.SM_001_INIT,
                        new Object[0],
                        objIO,
                        null);
            }
            objIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
            if (objIO != null)
            {
                setMainEvent(objIO, "MAIN");
            }
        }

        // Do the same for Local Script
        objIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
        if (objIO != null
                && objIO.getOverscript() != null)
        {
            objIO.getOverscript().ClearDisallowedEvents();

            if (initialize)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getOverscript(),
                        ScriptConsts.SM_001_INIT,
                        new Object[0],
                        objIO,
                        null);
            }
        }

        // Sends InitEnd Event
        if (initialize)
        {
            objIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
            if (objIO != null
                    && objIO.getScript() != null)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getScript(),
                        ScriptConsts.SM_033_INITEND,
                        new Object[0],
                        objIO,
                        null);
            }
            objIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
            if (objIO != null
                    && objIO.getOverscript() != null)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getOverscript(),
                        ScriptConsts.SM_033_INITEND,
                        new Object[0],
                        objIO,
                        null);
            }
        }

        objIO = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
        if (objIO != null)
        {
            objIO.RemoveGameFlag(IoGlobals.GFLAG_NEEDINIT);
        }
    }
}
protected void runEvent(SCRIPTABLE script, String eventName, BaseInteractiveObject io)

{
    int msg = 0;
    if (eventName.equalsIgnoreCase("INIT"))
    {
        msg = ScriptConsts.SM_001_INIT;
    }
    else if (eventName.equalsIgnoreCase("HIT"))
    {
        msg = ScriptConsts.SM_016_HIT;
    }
    else if (eventName.equalsIgnoreCase("INIT_END"))
    {
        msg = ScriptConsts.SM_033_INITEND;
    }
    if (msg > 0)
    {
        runMessage(script, msg, io);
    }
    else
    {
        try
        {
            Method method;
            if (!eventName.StartsWith("on"))
            {
                PooledStringBuilder sb =
                        StringBuilderPool.GetInstance().GetStringBuilder();
                sb.Append("on");
                sb.Append(eventName.ToUpper().charAt(0));
                sb.Append(eventName.substring(1));
                method = script.getClass().getMethod(sb.ToString());
                sb.ReturnToPool();
                sb = null;
            }
            else
            {
                method = script.getClass().getMethod(eventName);
            }
            method.invoke(script, (Object[])null);
        }
        catch (NoSuchMethodException | SecurityException
              | IllegalAccessException | IllegalArgumentException
              | InvocationTargetException | PooledException e) {
            e.printStackTrace();
            throw new RPGException(ErrorMessage.INVALID_PARAM, e);
        }
        }
    }
    protected void runMessage(SCRIPTABLE script, int msg, BaseInteractiveObject io)

    {
        switch (msg)
        {
            case ScriptConsts.SM_001_INIT:
                script.onInit();
                break;
            case ScriptConsts.SM_002_INVENTORYIN:
                script.onInventoryIn();
                break;
            case ScriptConsts.SM_004_INVENTORYUSE:
                script.onInventoryUse();
                break;
            case ScriptConsts.SM_007_EQUIPOUT:
                script.onUnequip();
                break;
            case ScriptConsts.SM_016_HIT:
                script.onHit();
                break;
            case ScriptConsts.SM_017_DIE:
                script.onDie();
                break;
            case ScriptConsts.SM_024_COMBINE:
                script.onCombine();
                break;
            case ScriptConsts.SM_033_INITEND:
                script.onInitEnd();
                break;
            case ScriptConsts.SM_041_LOAD:
                script.onLoad();
                break;
            case ScriptConsts.SM_043_RELOAD:
                script.onReload();
                break;
            case ScriptConsts.SM_045_OUCH:
                script.onOuch();
                break;
            case ScriptConsts.SM_046_HEAR:
                script.onHear();
                break;
            case ScriptConsts.SM_057_AGGRESSION:
                script.onAggression();
                break;
            case ScriptConsts.SM_069_IDENTIFY:
                script.onIdentify();
                break;
            default:
                throw new RPGException(ErrorMessage.INVALID_PARAM,
                        "No action defined for message " + msg);
        }
    }
    public void sendEvent(BaseInteractiveObject io, SendParameters params)

    {
        BaseInteractiveObject oes = eventSender;
        eventSender = io;
        if (params.HasFlag(SendParameters.RADIUS)) {
        // SEND EVENT TO ALL OBJECTS IN A RADIUS

        // LOOP THROUGH ALL IOs.
        int i = Interactive.GetInstance().getMaxIORefId();
        for (; i >= 0; i--)
        {
            if (Interactive.GetInstance().hasIO(i))
            {
                BaseInteractiveObject iio = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                // skip cameras and markers
                // if (iio.HasIOFlag(IoGlobals.io_camera)
                // || iio.HasIOFlag(IoGlobals.io_marker)) {
                // continue;
                // }
                // skip IOs not in required group
                if (params.HasFlag(SendParameters.GROUP)) {
            if (!this.isIOInGroup(iio, params.getGroupName()))
            {
                continue;
            }
        }
        // if send event is for NPCs, send to NPCs,
        // if for Items, send to Items, etc...
        if ((params.HasFlag(SendParameters.IONpcData)

                && iio.HasIOFlag(IoGlobals.IO_03_NPC))
                            // || (params.HasFlag(SendParameters.FIX)
                            // && iio.HasIOFlag(IoGlobals.IO_FIX))
                            || (params.HasFlag(SendParameters.IOItemData)
                                    && iio.HasIOFlag(IoGlobals.IO_02_ITEM))) {
            Vector2 senderPos = new Vector2(),
                    ioPos = new Vector2();
            Interactive.GetInstance().GetItemWorldPosition(io,
                    senderPos);
            Interactive.GetInstance().GetItemWorldPosition(iio,
                    ioPos);
            // IF BaseInteractiveObject IS IN SENDER RADIUS, SEND EVENT
            io.setStatSent(io.getStatSent() + 1);
            this.stackSendIOScriptEvent(
                    iio,
                    0,
                                params.getEventParameters(),
                                params.getEventName());
        }
    }
}
        }
        if (params.HasFlag(SendParameters.ZONE)) {
            // SEND EVENT TO ALL OBJECTS IN A ZONE
            // ARX_PATH * ap = ARX_PATH_GetAddressByName(zonename);

            // if (ap != NULL) {
            // LOOP THROUGH ALL IOs.
            int i = Interactive.GetInstance().getMaxIORefId();
            for (; i >= 0; i--) {
                if (Interactive.GetInstance().hasIO(i)) {
                    BaseInteractiveObject iio = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                    // skip cameras and markers
                    // if (iio.HasIOFlag(IoGlobals.io_camera)
                    // || iio.HasIOFlag(IoGlobals.io_marker)) {
                    // continue;
                    // }
                    // skip IOs not in required group
                    if (params.HasFlag(SendParameters.GROUP)) {
                        if (!this.isIOInGroup(iio, params.getGroupName())) {
    continue;
}
}
                    // if send event is for NPCs, send to NPCs,
                    // if for Items, send to Items, etc...
                    if ((params.HasFlag(SendParameters.IONpcData)
                            && iio.HasIOFlag(IoGlobals.IO_03_NPC))
                            // || (params.HasFlag(SendParameters.FIX)
                            // && iio.HasIOFlag(IoGlobals.IO_FIX))
                            || (params.HasFlag(SendParameters.IOItemData)
                                    && iio.HasIOFlag(IoGlobals.IO_02_ITEM))) {
                        Vector2 ioPos = new Vector2();
Interactive.GetInstance().GetItemWorldPosition(iio,
        ioPos);
// IF BaseInteractiveObject IS IN ZONE, SEND EVENT
// if (ARX_PATH_IsPosInZone(ap, _pos.x, _pos.y, _pos.z))
// {
io.setStatSent(io.getStatSent() + 1);
                        this.stackSendIOScriptEvent(
                                iio,
                                0,
                                params.getEventParameters(),
                                params.getEventName());
// }
}
                }
            }
        }
        if (params.HasFlag(SendParameters.GROUP)) {
            // sends an event to all members of a group
            // LOOP THROUGH ALL IOs.
            int i = Interactive.GetInstance().getMaxIORefId();
            for (; i >= 0; i--) {
                if (Interactive.GetInstance().hasIO(i)) {
                    BaseInteractiveObject iio = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
                    // skip IOs not in required group
                    if (!this.isIOInGroup(iio, params.getGroupName())) {
    continue;
}
iio.setStatSent(io.getStatSent() + 1);
                    this.stackSendIOScriptEvent(
                            iio,
                            0,
                            params.getEventParameters(),
                            params.getEventName());
}
            }
        } else {
            // SINGLE OBJECT EVENT
            int tioid = Interactive.GetInstance().getTargetByNameTarget(
                    params.getTargetName());

            if (tioid == -2) {
                tioid = Interactive.GetInstance().GetInterNum(io);
            }
            if (Interactive.GetInstance().hasIO(tioid)) {
                io.setStatSent(io.getStatSent() + 1);
                this.stackSendIOScriptEvent(
                        (BaseInteractiveObject) Interactive.GetInstance().getIO(tioid),
                        0,
                        params.getEventParameters(),
                        params.getEventName());
}
        }
        this.eventSender = oes;
    }
/**
 * Sends an initialization event to an BaseInteractiveObject. The initialization event runs the
 * local script first, followed by the over script.
 * @param io the BaseInteractiveObject
 * @return {@link int}
 * @ if an error occurs
 */
public int sendInitScriptEvent(BaseInteractiveObject io)
{
    if (io == null)
    {
        return -1;
    }
    int num = io.GetRefId();
    if (!Interactive.GetInstance().hasIO(num))
    {
        return -1;
    }
    BaseInteractiveObject oldEventSender = eventSender;
    eventSender = null;
    // send script the init message
    BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
    if (hio.getScript() != null)
    {
        GLOB = 0;
        sendScriptEvent((SCRIPTABLE)hio.getScript(),
                ScriptConsts.SM_001_INIT,
                null,
                hio,
                null);
    }
    hio = null;
    // send overscript the init message
    if (Interactive.GetInstance().getIO(num) != null)
    {
        hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
        if (hio.getOverscript() != null)
        {
            GLOB = 0;
            sendScriptEvent((SCRIPTABLE)hio.getOverscript(),
                    ScriptConsts.SM_001_INIT,
                    null,
                    hio,
                    null);
        }
        hio = null;
    }
    // send script the init end message
    if (Interactive.GetInstance().getIO(num) != null)
    {
        hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
        if (hio.getScript() != null)
        {
            GLOB = 0;
            sendScriptEvent((SCRIPTABLE)hio.getScript(),
                    ScriptConsts.SM_033_INITEND,
                    null,
                    hio,
                    null);
        }
        hio = null;
    }
    // send overscript the init end message
    if (Interactive.GetInstance().getIO(num) != null)
    {
        hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
        if (hio.getOverscript() != null)
        {
            GLOB = 0;
            sendScriptEvent((SCRIPTABLE)hio.getOverscript(),
                    ScriptConsts.SM_033_INITEND,
                    null,
                    hio,
                    null);
        }
        hio = null;
    }
    eventSender = oldEventSender;
    return ScriptConsts.ACCEPT;
}
/**
 * Sends a script event to an interactive object. The returned value is a
 * flag indicating whether the event was successful or refused.
 * @param target the reference id of the targeted io
 * @param msg the message being sent
 * @param params the list of parameters applied, grouped in key-value pairs
 * @param eventname the name of the event, for example, new Object[]
 *            {"key0", value, "key1", new int[] {0, 0}}
 * @return <code>int</code>
 * @ if an error occurs
 */
public int sendIOScriptEvent(BaseInteractiveObject target, int msg,
         Object[] params, String eventname)
{
    // checks invalid BaseInteractiveObject
    if (target == null)
    {
        return -1;
    }
    int num = target.GetRefId();

    if (Interactive.GetInstance().hasIO(num))
    {
        BaseInteractiveObject originalEventSender = eventSender;
        if (msg == ScriptConsts.SM_001_INIT
                || msg == ScriptConsts.SM_033_INITEND)
        {
            BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
            sendIOScriptEventReverse(hio, msg, params, eventname);
            eventSender = originalEventSender;
            hio = null;
        }

        if (Interactive.GetInstance().hasIO(num))
        {
            // if this BaseInteractiveObject only has a Local script, send event to it
            BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
            if (hio.getOverscript() == null)
            {
                GLOB = 0;
                int ret = sendScriptEvent(
                        (SCRIPTABLE)hio.getScript(),
                        msg,
                            params,
                        hio,
                        eventname);
                eventSender = originalEventSender;
                return ret;
            }

            // If this BaseInteractiveObject has a Global script send to Local (if exists)
            // then to Global if not overridden by Local
            int s = sendScriptEvent(
                    (SCRIPTABLE)hio.getOverscript(),
                    msg,
                        params,
                    hio,
                    eventname);
            if (s != ScriptConsts.REFUSE)
            {
                eventSender = originalEventSender;
                GLOB = 0;

                if (Interactive.GetInstance().hasIO(num))
                {
                    hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
                    int ret = sendScriptEvent(
                            (SCRIPTABLE)hio.getScript(),
                            msg,
                                params,
                            hio,
                            eventname);
                    eventSender = originalEventSender;
                    return ret;
                }
                else
                {
                    return ScriptConsts.REFUSE;
                }
            }
            hio = null;
        }
        GLOB = 0;
    }

    // Refused further processing.
    return ScriptConsts.REFUSE;
}
private int sendIOScriptEventReverse(BaseInteractiveObject io, int msg,
         Object[] params, String eventname)
{
    // checks invalid BaseInteractiveObject
    if (io == null)
    {
        return -1;
    }
    // checks for no script assigned
    if (io.getOverscript() == null
            && io.getScript() == null)
    {
        return -1;
    }
    int num = io.GetRefId();
    if (Interactive.GetInstance().hasIO(num))
    {
        BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
        // if this BaseInteractiveObject only has a Local script, send event to it
        if (hio.getOverscript() == null
                && hio.getScript() != null)
        {
            GLOB = 0;
            return sendScriptEvent(
                    (SCRIPTABLE)hio.getScript(),
                    msg,
                        params,
                    hio,
                    eventname);
        }

        // If this BaseInteractiveObject has a Global script send to Local (if exists)
        // then to global if no overriden by Local
        if (Interactive.GetInstance().hasIO(num))
        {
            hio = (BaseInteractiveObject)Interactive.GetInstance().getIO(num);
            int s = sendScriptEvent(
                    (SCRIPTABLE)hio.getScript(),
                    msg,
                        params,
                    hio,
                    eventname);
            if (s != ScriptConsts.REFUSE)
            {
                GLOB = 0;
                if (Interactive.GetInstance().hasIO(io.GetRefId()))
                {
                    return sendScriptEvent(
                            (SCRIPTABLE)io.getOverscript(),
                            msg,
                                params,
                            io,
                            eventname);
                }
                else
                {
                    return ScriptConsts.REFUSE;
                }
            }
        }
        hio = null;
        GLOB = 0;
    }
    // Refused further processing.
    return ScriptConsts.REFUSE;
}
/**
 * Sends a scripted event to all IOs.
 * @param msg the message
 * @param dat any script variables
 * @return <code>int</code>
 * @ if an error occurs
 */
public int sendMsgToAllIO(int msg, Object[] dat)

{
    int ret = ScriptConsts.ACCEPT;
    int i = Interactive.GetInstance().getMaxIORefId();
    for (; i >= 0; i--)
    {
        if (Interactive.GetInstance().hasIO(i))
        {
            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
            if (sendIOScriptEvent(io, msg, dat,
                    null) == ScriptConsts.REFUSE)
            {
                ret = ScriptConsts.REFUSE;
            }
        }
    }
    return ret;
}
/**
 * Sends a scripted event to an BaseInteractiveObject.
 * @param localScript the local script the BaseInteractiveObject shoulod follow
 * @param msg the event message
 * @param params any parameters to be applied
 * @param io
 * @param eventName
 * @param info
 * @return
 * @
 */
public int sendScriptEvent(SCRIPTABLE localScript,
         int msg, Object[] params, BaseInteractiveObject io,
         String eventName)
{
    int retVal = ScriptConsts.ACCEPT;
    bool keepGoing = true;
    if (localScript == null)
    {
        throw new RPGException(
                ErrorMessage.INVALID_PARAM, "script cannot be null");
    }
    if (io != null)
    {
        if (io.HasGameFlag(IoGlobals.GFLAG_MEGAHIDE)
                && msg != ScriptConsts.SM_043_RELOAD)
        {
            return ScriptConsts.ACCEPT;
        }

        if (io.getShow() == IoGlobals.SHOW_FLAG_DESTROYED)
        {
            // destroyed
            return ScriptConsts.ACCEPT;
        }
        eventTotalCount++;
        io.setStatCount(io.getStatCount() + 1);

        if (io.HasIOFlag(IoGlobals.IO_06_FREEZESCRIPT))
        {
            if (msg == ScriptConsts.SM_041_LOAD)
            {
                return ScriptConsts.ACCEPT;
            }
            return ScriptConsts.REFUSE;
        }

        if (io.HasIOFlag(IoGlobals.IO_03_NPC)
                && !io.HasIOFlag(IoGlobals.IO_09_DWELLING))
        {
            if (io.getNPCData().getBaseLife() <= 0.f
                    && msg != ScriptConsts.SM_001_INIT
                    && msg != ScriptConsts.SM_012_DEAD
                    && msg != ScriptConsts.SM_017_DIE
                    && msg != ScriptConsts.SM_255_EXECUTELINE
                    && msg != ScriptConsts.SM_043_RELOAD
                    && msg != ScriptConsts.SM_255_EXECUTELINE
                    && msg != ScriptConsts.SM_028_INVENTORY2_OPEN
                    && msg != ScriptConsts.SM_029_INVENTORY2_CLOSE)
            {
                return ScriptConsts.ACCEPT;
            }
        }
        // change weapon if one breaks
        /*
         * if (((io->ioflags & IO_FIX) || (io->ioflags & IO_ITEM)) && (msg
         * == ScriptConsts.SM_BREAK)) { ManageCasseDArme(io); }
         */
    }
    // use master script if available
    SCRIPTABLE script = (SCRIPTABLE)localScript.getMaster();
    if (script == null)
    { // no master - use local script
        script = localScript;
    }
    // set parameters on script that will be used
    if (params != null
            && params.Length > 0) {
        for (int i = 0; i < params.Length; i += 2) {
            script.setLocalVariable((String) params[i], params[i + 1]);
}
        }
        // run the event
        if (eventName != null
                && eventName.Length() > 0) {
            runEvent(script, eventName, io);
        } else {
            if (eventIsDisallowed(msg, script)) {
                return ScriptConsts.REFUSE;
            }
            runMessage(script, msg, io);
        }
        int ret = ScriptConsts.ACCEPT;
        return ret;
    }
    /**
     * Sets the value for the flag indicating whether debug output is turned on.
     * @param val the value to set
     */
    public void setDebug(bool val)
{
    this.debug = val;
}
public void setEvent(BaseInteractiveObject io, String event,
         bool isOn)
{
    if (event.equalsIgnoreCase("COLLIDE_NPC")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(
                    ScriptConsts.DISABLE_COLLIDE_NPC);
        }
        else
        {
            io.getScript().AssignDisallowedEvent(
                    ScriptConsts.DISABLE_COLLIDE_NPC);
        }
    } else if (event.equalsIgnoreCase("CHAT")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConsts.DISABLE_CHAT);
        }
        else
        {
            io.getScript().AssignDisallowedEvent(ScriptConsts.DISABLE_CHAT);
        }
    } else if (event.equalsIgnoreCase("HIT")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConsts.DISABLE_HIT);
        }
        else
        {
            io.getScript().AssignDisallowedEvent(ScriptConsts.DISABLE_HIT);
        }
    } else if (event.equalsIgnoreCase("INVENTORY2_OPEN")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(
                    ScriptConsts.DISABLE_INVENTORY2_OPEN);
        }
        else
        {
            io.getScript().AssignDisallowedEvent(
                    ScriptConsts.DISABLE_INVENTORY2_OPEN);
        }
    } else if (event.equalsIgnoreCase("DETECTPLAYER")) {
        if (isOn)
        {
            io.getScript()
                    .removeDisallowedEvent(ScriptConsts.DISABLE_DETECT);
        }
        else
        {
            io.getScript()
                    .AssignDisallowedEvent(ScriptConsts.DISABLE_DETECT);
        }
    } else if (event.equalsIgnoreCase("HEAR")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConsts.DISABLE_HEAR);
        }
        else
        {
            io.getScript().AssignDisallowedEvent(ScriptConsts.DISABLE_HEAR);
        }
    } else if (event.equalsIgnoreCase("AGGRESSION")) {
        if (isOn)
        {
            io.getScript()
                    .removeDisallowedEvent(ScriptConsts.DISABLE_AGGRESSION);
        }
        else
        {
            io.getScript()
                    .AssignDisallowedEvent(ScriptConsts.DISABLE_AGGRESSION);
        }
    } else if (event.equalsIgnoreCase("MAIN")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConsts.DISABLE_MAIN);
        }
        else
        {
            io.getScript().AssignDisallowedEvent(ScriptConsts.DISABLE_MAIN);
        }
    } else if (event.equalsIgnoreCase("CURSORMODE")) {
        if (isOn)
        {
            io.getScript()
                    .removeDisallowedEvent(ScriptConsts.DISABLE_CURSORMODE);
        }
        else
        {
            io.getScript()
                    .AssignDisallowedEvent(ScriptConsts.DISABLE_CURSORMODE);
        }
    } else if (event.equalsIgnoreCase("EXPLORATIONMODE")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(
                    ScriptConsts.DISABLE_EXPLORATIONMODE);
        }
        else
        {
            io.getScript().AssignDisallowedEvent(
                    ScriptConsts.DISABLE_EXPLORATIONMODE);
        }
    }
}
/**
 * Sets the value of the eventSender.
 * @param val the new value to set
 */
public void setEventSender(BaseInteractiveObject val)
{
    eventSender = val;
}
/**
 * Sets a global variable.
 * @param name the name of the global variable
 * @param value the variable's value
 * @ if an error occurs
 */
public void setGlobalVariable(String name, Object value)

{
    if (gvars == null)
    {
        gvars = new ScriptVariable[0];
    }
    bool found = false;
    for (int i = gvars.Length - 1; i >= 0; i--)
    {
        ScriptVariable var = gvars[i];
        if (var != null
                && var.getName() != null
                && var.getName().equalsIgnoreCase(name))
        {
            // found the correct script variable
            var.set(value);
            found = true;
            break;
        }
    }
    if (!found)
    {
        // create a new variable and add to the global array
        ScriptVariable var = null;
        if (value is String
                    || value is char[])
        {
            var = new ScriptVariable(name, ScriptConsts.TYPE_G_00_TEXT,
                    value);
        }
        else if (value is String[]
                  || value is char[][])
        {
            var = new ScriptVariable(name,
                    ScriptConsts.TYPE_G_01_TEXT_ARR, value);
        }
        else if (value is Float)
        {
            var = new ScriptVariable(name, ScriptConsts.TYPE_G_02_FLOAT,
                    value);
        }
        else if (value is Double)
        {
            var = new ScriptVariable(name, ScriptConsts.TYPE_G_02_FLOAT,
                    value);
        }
        else if (value is float[])
        {
            var = new ScriptVariable(name,
                    ScriptConsts.TYPE_G_03_FLOAT_ARR, value);
        }
        else if (value is Integer)
        {
            var = new ScriptVariable(name, ScriptConsts.TYPE_G_04_INT,
                    value);
        }
        else if (value is int[])
        {
            var = new ScriptVariable(name,
                    ScriptConsts.TYPE_G_05_INT_ARR, value);
        }
        else if (value is Long)
        {
            var = new ScriptVariable(name, ScriptConsts.TYPE_G_06_LONG,
                    value);
        }
        else if (value is long[])
        {
            var = new ScriptVariable(name,
                    ScriptConsts.TYPE_G_07_LONG_ARR, value);
        }
        else
        {
            PooledStringBuilder sb =
                    StringBuilderPool.GetInstance().GetStringBuilder();
            try
            {
                sb.Append("Global variable ");
                sb.Append(name);
                sb.Append(" was passed new value of type ");
                sb.Append(value.getClass().getCanonicalName());
                sb.Append(". Only String, String[], char[][], Float, ");
                sb.Append("float[], Integer, int[], Long, or long[] ");
                sb.Append("allowed.");
            }
            catch (PooledException e)
            {
                throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
            }
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.ToString());
            sb.ReturnToPool();
            sb = null;
            throw ex;
        }
        gvars = ArrayUtilities.GetInstance().ExtendArray(var, gvars);
    }
}
/**
 * Sets the main event for an BaseInteractiveObject.
 * @param io the BaseInteractiveObject
 * @param newevent the event's name
 */
public void setMainEvent(BaseInteractiveObject io, String newevent)
{
    if (io != null)
    {
        if (!"MAIN".equalsIgnoreCase(newevent))
        {
            io.setMainevent(null);
        }
        else
        {
            io.setMainevent(newevent);
        }
    }
}
/**
 * Sets the maximum number of timer scripts.
 * @param val the maximum number of timer scripts to set
 */
protected void setMaxTimerScript(int val)
{
    maxTimerScript = val;
}
protected abstract void setScriptTimer(int index, TIMER timer);
/**
 * Processes and BaseInteractiveObject's speech.
 * @param io the BaseInteractiveObject
 * @param params the {@link SpeechParameters}
 * @ 
 */
public void speak(BaseInteractiveObject io, SpeechParameters params)
{
    // speech variables
    // ARX_CINEMATIC_SPEECH acs;
    // acs.type = ARX_CINE_SPEECH_NONE;
    long voixoff = 0;
    int mood = ANIM_TALK_NEUTRAL;
    if (params.isKillAllSpeech()) {
        // ARX_SPEECH_Reset();
    } else {
        if (params.HasFlag(SpeechParameters.HAPPY)) {
            mood = ANIM_TALK_HAPPY;
        }
        if (params.HasFlag(SpeechParameters.ANGRY)) {
            mood = ANIM_TALK_ANGRY;
        }
        if (params.HasFlag(SpeechParameters.OFF_VOICE)) {
            voixoff = 2;
        }
        if (params.HasFlag(SpeechParameters.KEEP_SPEECH)
                || params.HasFlag(SpeechParameters.ZOOM_SPEECH)
                || params.HasFlag(SpeechParameters.SPEECH_CCCTALKER_L)
                || params.HasFlag(SpeechParameters.SPEECH_CCCTALKER_R)
                || params.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)
                || params.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_R)
                || params.HasFlag(SpeechParameters.SIDE_L)
                || params.HasFlag(SpeechParameters.SIDE_R)) {
            // FRAME_COUNT = 0;
            if (params.HasFlag(SpeechParameters.KEEP_SPEECH)) {
                // acs.type = ARX_CINE_SPEECH_KEEP;
                // acs.pos1.x = LASTCAMPOS.x;
                // acs.pos1.y = LASTCAMPOS.y;
                // acs.pos1.z = LASTCAMPOS.z;
                // acs.pos2.a = LASTCAMANGLE.a;
                // acs.pos2.b = LASTCAMANGLE.b;
                // acs.pos2.g = LASTCAMANGLE.g;
            }
            if (params.HasFlag(SpeechParameters.ZOOM_SPEECH)) {
                // acs.type = ARX_CINE_SPEECH_ZOOM;
                // pos = GetNextWord(es, pos, temp2);
                // acs.startangle.a = GetVarValueInterpretedAsFloat(temp2,
                // esss, io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.startangle.b = GetVarValueInterpretedAsFloat(temp2,
                // esss, io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.endangle.a = GetVarValueInterpretedAsFloat(temp2,
                // esss, io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.endangle.b = GetVarValueInterpretedAsFloat(temp2,
                // esss, io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.startpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.endpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);

                // ARX_CHECK_NO_ENTRY(); //ARX: xrichter (2010-07-20) -
                // temp2 is often (always?) a string number and
                // GetTargetByNameTarget return -1. To be careful if temp2
                // is not a string number, we choose to test
                // GetTargetByNameTarget return value.
                // acs.ionum = GetTargetByNameTarget(temp2);

                // if (acs.ionum == -2) //means temp2 is "me" or "self"
                // acs.ionum = GetInterNum(io);

                if (params.HasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, -1);
                }
            }
            if (params.HasFlag(SpeechParameters.SPEECH_CCCTALKER_L)
                    || params
                                .HasFlag(SpeechParameters.SPEECH_CCCTALKER_R)) {
                if (params.HasFlag(SpeechParameters.SPEECH_CCCTALKER_L)) {
                    // acs.type = ARX_CINE_SPEECH_CCCTALKER_R;
                } else {
                    // acs.type = ARX_CINE_SPEECH_CCCTALKER_L;
                }
                // pos = GetNextWord(es, pos, temp2);
                // acs.ionum = GetTargetByNameTarget(temp2);

                // if (acs.ionum == -2) acs.ionum = GetInterNum(io);

                // pos = GetNextWord(es, pos, temp2);
                // acs.startpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.endpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);

                if (params.HasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, acs.ionum);
                }
            }
            if (params.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)
                    || params.HasFlag(
                            SpeechParameters.SPEECH_CCCLISTENER_R)) {
                if (params.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)) {
                    // acs.type = ARX_CINE_SPEECH_CCCLISTENER_L;
                } else {
                    // acs.type = ARX_CINE_SPEECH_CCCLISTENER_R;
                }
                // pos = GetNextWord(es, pos, temp2);
                // acs.ionum = GetTargetByNameTarget(temp2);

                // if (acs.ionum == -2) acs.ionum = GetInterNum(io);

                // pos = GetNextWord(es, pos, temp2);
                // acs.startpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.endpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);

                if (params.HasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, acs.ionum);
                }
            }
            if (params.HasFlag(SpeechParameters.SIDE_L)
                    || params.HasFlag(SpeechParameters.SIDE_R)) {
                if (params.HasFlag(SpeechParameters.SIDE_L)) {
                    // acs.type = ARX_CINE_SPEECH_SIDE_LEFT;
                } else {
                    // acs.type = ARX_CINE_SPEECH_SIDE;
                }
                // pos = GetNextWord(es, pos, temp2);
                // acs.ionum = GetTargetByNameTarget(temp2);

                // if (acs.ionum == -2) acs.ionum = GetInterNum(io);

                // pos = GetNextWord(es, pos, temp2);
                // acs.startpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);
                // pos = GetNextWord(es, pos, temp2);
                // acs.endpos = GetVarValueInterpretedAsFloat(temp2, esss,
                // io);
                // startdist
                // pos = GetNextWord(es, pos, temp2);
                // acs.f0 = GetVarValueInterpretedAsFloat(temp2, esss, io);
                // enddist
                // pos = GetNextWord(es, pos, temp2);
                // acs.f1 = GetVarValueInterpretedAsFloat(temp2, esss, io);
                // height modifier
                // pos = GetNextWord(es, pos, temp2);
                // acs.f2 = GetVarValueInterpretedAsFloat(temp2, esss, io);

                if (params.HasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, acs.ionum);
                }
            }
        }

        long speechnum = 0;

        if (params.getSpeechName() == null
                || params.getSpeechName().Length() == 0) {
            // ARX_SPEECH_ClearIOSpeech(io);
        } else {
            if (params.HasFlag(SpeechParameters.NO_TEXT)) {
                // voixoff |= ARX_SPEECH_FLAG_NOTEXT;
            }

            // if (!CINEMASCOPE) voixoff |= ARX_SPEECH_FLAG_NOTEXT;
            if (params.HasFlag(SpeechParameters.PLAYER)) {
                // speechnum = ARX_SPEECH_AddSpeech(inter.iobj[0], temp1,
                // PARAM_LOCALISED, mood, voixoff);
            } else {
                // speechnum = ARX_SPEECH_AddSpeech(io, temp1,
                // PARAM_LOCALISED, mood, voixoff);
                speechnum = Speech.GetInstance().ARX_SPEECH_AddSpeech(io,
                        mood, params.getSpeechName(), voixoff);
            }

            if (speechnum >= 0)
            {
                // ARX_SCRIPT_Timer_GetDefaultName(timername2);
                // sprintf(timername, "SPEAK_%s", timername2);
                // aspeech[speechnum].scrpos = pos;
                // aspeech[speechnum].es = es;
                // aspeech[speechnum].ioscript = io;
                if (params.HasFlag(SpeechParameters.UNBREAKABLE)) {
                    // aspeech[speechnum].flags |=
                    // ARX_SPEECH_FLAG_UNBREAKABLE;
                }

                // memcpy(&aspeech[speechnum].cine, &acs,
                // sizeof(ARX_CINEMATIC_SPEECH));
                // pos = GotoNextLine(es, pos);
            }
        }
    }
}
/**
 * Sends a scripted event to the event stack for all members of a group, to
 * be fired during the game cycle.
 * @param group the name of the BaseInteractiveObject group
 * @param msg the script message
 * @param params the parameters assigned to the script
 * @param eventname the event name
 * @ if an error occurs
 */
public void stackSendGroupScriptEvent(String group,
         int msg, Object[] params, String eventname)

{
    int i = Interactive.GetInstance().getMaxIORefId();
    for (; i >= 0; i--)
    {
        if (Interactive.GetInstance().hasIO(i))
        {
            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
            if (isIOInGroup(io, group))
            {
                stackSendIOScriptEvent(io, msg, params, eventname);
            }
            io = null;
        }
    }
}
/**
 * Sends an BaseInteractiveObject scripted event to the event stack, to be fired during the
 * game cycle.
 * @param io the BaseInteractiveObject
 * @param msg the script message
 * @param params the parameters assigned to the script
 * @param eventname the event name
 */
public void stackSendIOScriptEvent(BaseInteractiveObject io,
         int msg, Object[] params, String eventname)
{
    for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
    {
        if (!getStackedEvent(i).exists())
        {
            if (params != null
                    && params.Length > 0) {
        getStackedEvent(i).setParams(params);
    } else {
        getStackedEvent(i).setParams(null);
    }
    if (eventname != null
            && eventname.Length() > 0)
    {
        getStackedEvent(i).setEventname(eventname);
    }
    else
    {
        getStackedEvent(i).setEventname(null);
    }

    getStackedEvent(i).setSender(eventSender);
    getStackedEvent(i).Io = io);
    getStackedEvent(i).setMsg(msg);
    getStackedEvent(i).setExist(true);
    break;
}
        }
    }
    /**
     * Adds messages for all NPCs to the event stack.
     * @param msg the message
     * @param dat the message parameters
     * @ if an error occurs
     */
    public void stackSendMsgToAllNPCIO(int msg, Object[] dat)

{
    int i = Interactive.GetInstance().getMaxIORefId();
    for (; i >= 0; i--)
    {
        if (Interactive.GetInstance().hasIO(i))
        {
            BaseInteractiveObject io = (BaseInteractiveObject)Interactive.GetInstance().getIO(i);
            if (io.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                stackSendIOScriptEvent(io, msg, dat, null);
            }
        }
    }
}
/**
 * Starts a timer using a defined set of parameters.
 * @param params the parameters
 */
public void startTimer(
         ScriptTimerInitializationParameters<BaseInteractiveObject, SCRIPTABLE> params)
{
    int timerNum = timerGetFree();
    ScriptTimer timer = getScriptTimer(timerNum);
    timer.SetScript(params.getScript());
    timer.setExists(true);
    timer.Io =params.getIo());
    timer.setCycleLength(params.getMilliseconds());
    if (params.getName() == null
            || (params.getName() != null
                    && params.getName().Length() == 0)) {
        timer.setName(timerGetDefaultName());
    } else {
        timer.setName(params.getName());
    }
    timer.setAction(new ScriptTimerAction(
                params.getObj(),
                params.getMethod(),
                params.getArgs()));
    timer.setLastTimeCheck(params.getStartTime());
    timer.setRepeatTimes(params.getRepeatTimes());
    timer.ClearFlags();
    timer.AddFlag(params.getFlagValues());
}
/**
 * Teleports an BaseInteractiveObject to a target location.
 * @param io the io calling for the teleport event
 * @param behind flag indicating the target teleports behind
 * @param isPlayer flag indicating object being teleported is the player
 * @param initPosition flag indicating the object being teleported goes to
 *            its initial position
 * @param target the name of teleport destination
 * @ if an error occurs
 */
public void teleport(BaseInteractiveObject io, bool behind,
         bool isPlayer, bool initPosition,
         String target)
{
    if (behind)
    {
        Interactive.GetInstance().ARX_INTERACTIVE_TeleportBehindTarget(io);
    }
    else
    {
        if (!initPosition)
        {
            int ioid =
                    Interactive.GetInstance().getTargetByNameTarget(target);

            if (ioid == -2)
            {
                ioid = Interactive.GetInstance().GetInterNum(io);
            }
            if (ioid != -1
                    && ioid != -2)
            {
                if (ioid == -3)
                {
                    if (io.getShow() != IoGlobals.SHOW_FLAG_LINKED
                            && io.getShow() != IoGlobals.SHOW_FLAG_HIDDEN
                            && io.getShow() != IoGlobals.SHOW_FLAG_MEGAHIDE
                            && io.getShow() != IoGlobals.SHOW_FLAG_DESTROYED
                            && io.getShow() != IoGlobals.SHOW_FLAG_KILLED)
                    {
                        io.show = IoGlobals.SHOW_FLAG_IN_SCENE);
                    }
                    BaseInteractiveObject pio = (BaseInteractiveObject)Interactive.GetInstance().getIO(
                            ProjectConstants.GetInstance().getPlayer());
                    Interactive.GetInstance().ARX_INTERACTIVE_Teleport(
                            io, pio.getPosition());
                    pio = null;
                }
                else
                {
                    if (Interactive.GetInstance().hasIO(ioid))
                    {
                        BaseInteractiveObject tio = (BaseInteractiveObject)Interactive.GetInstance().getIO(ioid);
                        Vector2 pos = new Vector2();

                        if (Interactive.GetInstance()
                                .GetItemWorldPosition(tio, pos))
                        {
                            if (isPlayer)
                            {
                                BaseInteractiveObject pio = (BaseInteractiveObject)Interactive.GetInstance()
                                        .getIO(
                                                ProjectConstants
                                                        .GetInstance()
                                                        .getPlayer());
                                Interactive.GetInstance()
                                        .ARX_INTERACTIVE_Teleport(
                                                pio, pos);
                                pio = null;
                            }
                            else
                            {
                                if (io.HasIOFlag(IoGlobals.IO_03_NPC)
                                        && io.getNPCData()
                                                .getBaseLife() <= 0)
                                {
                                    // do nothing
                                }
                                else
                                {
                                    if (io.getShow() != IoGlobals.SHOW_FLAG_HIDDEN
                                            && io.getShow() != IoGlobals.SHOW_FLAG_MEGAHIDE)
                                    {
                                        io.show =
                                                IoGlobals.SHOW_FLAG_IN_SCENE);
                                    }
                                    Interactive.GetInstance()
                                            .ARX_INTERACTIVE_Teleport(
                                                    io, pos);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (io != null)
            {
                if (isPlayer)
                {
                    Vector2 pos = new Vector2();
                    if (Interactive.GetInstance().GetItemWorldPosition(io,
                            pos))
                    {
                        BaseInteractiveObject pio = (BaseInteractiveObject)Interactive.GetInstance().getIO(
                                ProjectConstants.GetInstance().getPlayer());
                        Interactive.GetInstance().ARX_INTERACTIVE_Teleport(
                                pio, pos);
                        pio = null;

                    }
                }
                else
                {
                    if (io.HasIOFlag(IoGlobals.IO_03_NPC)
                            && io.getNPCData().getBaseLife() <= 0)
                    {
                        // do nothing
                    }
                    else
                    {
                        if (io.getShow() != IoGlobals.SHOW_FLAG_HIDDEN
                                && io.getShow() != IoGlobals.SHOW_FLAG_MEGAHIDE)
                        {
                            io.show = IoGlobals.SHOW_FLAG_IN_SCENE);
                        }
                        Interactive.GetInstance().ARX_INTERACTIVE_Teleport(
                                io, io.getInitPosition());
                    }
                }
            }
        }
    }
}
public void timerCheck()
{
    if (countTimers() > 0)
    {
        for (int i = 0, len = this.maxTimerScript; i < len; i++)
        {
            TIMER timer = getScriptTimers()[i];
            if (timer.exists())
            {
                long currentTime = Time.GetInstance().getGameTime();
                if (timer.isTurnBased())
                {
                    currentTime = Time.GetInstance().getGameRound();
                }
                if (timer.HasFlag(1))
                {
                    if (!timer.getIo().HasGameFlag(
                            IoGlobals.GFLAG_ISINTREATZONE))
                    {
                        while (timer.getLastTimeCheck()
                                + timer.getCycleLength() < currentTime)
                        {
                            timer.setLastTimeCheck(timer.getLastTimeCheck()
                                    + timer.getCycleLength());
                        }
                        continue;
                    }
                }
                if (timer.getLastTimeCheck()
                        + timer.getCycleLength() < currentTime)
                {
                    SCRIPTABLE script = (SCRIPTABLE)timer.getScript();
                    BaseInteractiveObject io = timer.getIo();
                    if (script != null)
                    {
                        if (timer.getName().equalsIgnoreCase("_R_A_T_"))
                        {
                            // if (Manage_Specific_RAT_Timer(st))
                            continue;
                        }
                    }
                    if (timer.getRepeatTimes() == 1)
                    {
                        timerClearByNum(i);
                    }
                    else
                    {
                        if (timer.getRepeatTimes() != 0)
                        {
                            timer.setRepeatTimes(
                                    timer.getRepeatTimes() - 1);
                        }
                        timer.setLastTimeCheck(timer.getLastTimeCheck()
                                + timer.getCycleLength());
                    }
                    if (script != null
                            && Interactive.GetInstance().hasIO(io))
                    {
                        timer.GetAction().process();
                    }
                    script = null;
                    io = null;
                }
            }
            timer = null;
        }
    }
}
/** Clears all timers in play. */
public void timerClearAll()
{
    for (int i = 0; i < maxTimerScript; i++)
    {
        timerClearByNum(i);
    }
}
public void timerClearAllLocalsForIO(BaseInteractiveObject io)
{
    TIMER[] scriptTimers = getScriptTimers();
    for (int i = 0; i < maxTimerScript; i++)
    {
        if (scriptTimers[i].exists())
        {
            if (scriptTimers[i].getIo().Equals(io)
                    && scriptTimers[i].getScript()
                            .Equals(io.getOverscript()))
            {
                timerClearByNum(i);
            }
        }
    }
}
/**
 * Clears a timer by the BaseInteractiveObject assigned to it.
 * @param io the BaseInteractiveObject
 */
public void timerClearByIO(BaseInteractiveObject io)
{
    if (io != null)
    {
        TIMER[] scriptTimers = getScriptTimers();
        for (int i = 0; i < maxTimerScript; i++)
        {
            if (scriptTimers[i] != null
                    && scriptTimers[i].exists())
            {
                if (scriptTimers[i].getIo().GetRefId() == io.GetRefId())
                {
                    timerClearByNum(i);
                }
            }
        }
    }
}
public void timerClearByNameAndIO(String timername,
         BaseInteractiveObject io)
{
    if (io != null)
    {
        TIMER[] scriptTimers = getScriptTimers();
        for (int i = 0; i < maxTimerScript; i++)
        {
            if (scriptTimers[i] != null
                    && scriptTimers[i].exists())
            {
                if (scriptTimers[i].getIo().GetRefId() == io.GetRefId()
                        && timername.equalsIgnoreCase(
                                scriptTimers[i].getName()))
                {
                    timerClearByNum(i);
                }
            }
        }
    }
}
/**
 * Clears a timer by its index on the timers list.
 * @param timeridx the index
 */
public void timerClearByNum(int timeridx)
{
    TIMER[] scriptTimers = getScriptTimers();
    if (scriptTimers[timeridx].exists())
    {
        scriptTimers[timeridx].setName(null);
        scriptTimers[timeridx].setExists(false);
    }
}
/**
 * Determines whether a specific named timer exists.
 * @param texx the timer's name
 * @return the timer's index if it exists, otherwise returns -1
 */
private int timerExist(String texx)
{
    int index = -1;
    TIMER[] scriptTimers = getScriptTimers();
    for (int i = 0; i < maxTimerScript; i++)
    {
        if (scriptTimers[i].exists())
        {
            if (scriptTimers[i].getName() != null
                    && scriptTimers[i].getName().equalsIgnoreCase(texx))
            {
                index = i;
                break;
            }
        }
    }
    return index;
}
/**
 * Initializes all game timers.
 * @param number the maximum number of timers used. Must be at least 100.
 */
public void timerFirstInit(int number)
{
    if (number < 100)
    {
        setMaxTimerScript(100);
    }
    else
    {
        setMaxTimerScript(number);
    }
    destroyScriptTimers();
    initScriptTimers();
}
/**
 * Generates a random name for an unnamed timer.
 * @return {@link String}
 */
private String timerGetDefaultName()
{
    int i = 1;
    String texx;

    while (true)
    {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.Append("TIMER_");
            sb.Append(i);
        }
        catch (PooledException e)
        {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        i++;

        if (timerExist(sb.ToString()) == -1)
        {
            texx = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            break;
        }
        sb.ReturnToPool();
        sb = null;
    }
    return texx;
}
/**
 * Gets the index of a free script timer.
 * @return <code>int</code>
 */
public int timerGetFree()
{
    int index = -1;
    TIMER[] scriptTimers = getScriptTimers();
    for (int i = 0; i < maxTimerScript; i++)
    {
        if (!scriptTimers[i].exists())
        {
            index = i;
            break;
        }
    }
    return index;
}
/**
 * Determines if an BaseInteractiveObject is speaking.
 * @param io the BaseInteractiveObject
 * @return <tt>true</tt> if the BaseInteractiveObject is speaking; <tt>false</tt> otherwise
 */
public bool amISpeaking(BaseInteractiveObject io)
{
    // TODO Auto-generated method stub
    // GO THROUGH ALL SPEECH INSTANCES.  IF THE BaseInteractiveObject IS SPEAKING
    // RETURN FALSE.  OTHERWISE TRUE
    //for (long i = 0; i < MAX_ASPEECH; i++) {
    //if (aspeech[i].exist) {
    //if (io == aspeech[i].io) {
    //*lcontent = 1;
    //return TYPE_LONG;
    //}
    //}
    //}
    return false;
}
public long getGameSeconds()
{
    return Time.GetInstance().getGameTime(false);
}
    }
}
