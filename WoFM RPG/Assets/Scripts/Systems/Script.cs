using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Systems
{
    public abstract class Script
    {
        private static  int ANIM_TALK_ANGRY = 0;
        private static  int ANIM_TALK_HAPPY = 0;
        private static  int ANIM_TALK_NEUTRAL = 0;
        /** the one and only instance of the <tt>Script</tt> class. */
        private static Script instance;
        /** the maximum number of system parameters. */
        public static  int MAX_SYSTEM_PARAMS = 5;
        /** the list of system parameters. */
        private static  String[] SYSTEM_PARAMS = new String[MAX_SYSTEM_PARAMS];
    /**
     * Gives access to the singleton instance of {@link Script}.
     * @return {@link Script}
     */
    public static Script getInstance()
        {
            return Script.instance;
        }
        /**
         * Sets the singleton instance.
         * @param i the instance to set
         */
        protected static void setInstance( Script i)
        {
            Script.instance = i;
        }
        private bool ARXPausedTime;
        /** the flag indicating whether debug output is turned on. */
        private bool debug;
        private bool EDITMODE;
        private IO eventSender;
        private int eventTotalCount;
        private int GLOB;
        /** the list of global script variables. */
        private ScriptVariable[] gvars;
        /** the maximum number of timer scripts. */
        private int maxTimerScript = 0;
        private bool PauseScript;
        private int stackFlow = 8;
        /**
         * Adds an IO to a specific group.
         * @param io the IO
         * @param group the group name
         */
        public  void addToGroup( IO io,  String group)
        {
            if (io != null
                    && group != null)
            {
                io.addGroup(group);
            }
        }
        public  void allowInterScriptExecution() throws RPGException
        {
        int ppos = 0;

        if (!PauseScript && !EDITMODE && !ARXPausedTime) {
            this.eventSender = null;

            int numm = Math.min(Interactive.getInstance().getMaxIORefId(), 10);

            for (int n = 0; n<numm; n++) {
                int i = ppos;
        ppos++;

                if (ppos >= Interactive.getInstance().getMaxIORefId()) {
                    ppos = 0;
                    break;
                }
                if (Interactive.getInstance().hasIO(i)) {
                    IO io = (IO)Interactive.getInstance().getIO(i);
                    if (io.hasGameFlag(IoGlobals.GFLAG_ISINTREATZONE)) {
                        if (io.getMainevent() != null) {
                            sendIOScriptEvent(io, 0, null, io.getMainevent());
} else {
                            sendIOScriptEvent(
                                    io, ScriptConstants.SM_008_MAIN, null, null);
                        }
                    }
                }
            }
        }
    }
    protected abstract void clearAdditionalEventStacks();
protected abstract void clearAdditionalEventStacksForIO(IO io);
/**
 * Clones all local variables from the source {@link IO} to the destination
 * {@link IO}.
 * @param src the source {@link IO}
 * @param dest the destination {@link IO}
 * @throws RPGException if an error occurs
 */
public  void cloneLocalVars( IO src,  IO dest)
            throws RPGException
{
        if (dest != null
                && src != null) {
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
public  int countTimers()
{
    int activeTimers = 0;
    TIMER[] scriptTimers = getScriptTimers();
    for (int i = scriptTimers.length - 1; i >= 0; i--)
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
private bool eventIsDisallowed( int msg,
         Scriptable script)
{
    bool disallowed = false;
    // check to see if message is for an event that was disabled
    switch (msg)
    {
        case ScriptConstants.SM_055_COLLIDE_NPC:
            if (script.hasAllowedEvent(ScriptConstants.DISABLE_COLLIDE_NPC))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_010_CHAT:
            if (script.hasAllowedEvent(ScriptConstants.DISABLE_CHAT))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_016_HIT:
            if (script.hasAllowedEvent(ScriptConstants.DISABLE_HIT))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_028_INVENTORY2_OPEN:
            if (script.hasAllowedEvent(
                    ScriptConstants.DISABLE_INVENTORY2_OPEN))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_046_HEAR:
            if (script.hasAllowedEvent(ScriptConstants.DISABLE_HEAR))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_023_UNDETECTPLAYER:
        case ScriptConstants.SM_022_DETECTPLAYER:
            if (script.hasAllowedEvent(ScriptConstants.DISABLE_DETECT))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_057_AGGRESSION:
            if (script.hasAllowedEvent(
                    ScriptConstants.DISABLE_AGGRESSION))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_008_MAIN:
            if (script.hasAllowedEvent(ScriptConstants.DISABLE_MAIN))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_073_CURSORMODE:
            if (script.hasAllowedEvent(
                    ScriptConstants.DISABLE_CURSORMODE))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_074_EXPLORATIONMODE:
            if (script.hasAllowedEvent(
                    ScriptConstants.DISABLE_EXPLORATIONMODE))
            {
                disallowed = true;
            }
            break;
        case ScriptConstants.SM_061_KEY_PRESSED:
            // float dwCurrTime = ARX_TIME_Get();
            // if ((dwCurrTime - g_TimeStartCinemascope) < 3000) {
            // return ScriptConstants.REFUSE;
            // }
            break;
        default:
            break;
    }
    return disallowed;
}
public  void eventStackClear()
{
    for (int i = 0; i < ScriptConstants.MAX_EVENT_STACK; i++)
    {
        if (getStackedEvent(i).exists())
        {
            getStackedEvent(i).setParams(null);
            getStackedEvent(i).setEventname(null);
            getStackedEvent(i).setSender(null);
            getStackedEvent(i).setExist(false);
            getStackedEvent(i).setIo(null);
            getStackedEvent(i).setMsg(0);
        }
    }
    clearAdditionalEventStacks();
}
public  void eventStackClearForIo( IO io)
{
    for (int i = 0; i < ScriptConstants.MAX_EVENT_STACK; i++)
    {
        if (getStackedEvent(i).exists()
                && io.equals(getStackedEvent(i).getIo()))
        {
            getStackedEvent(i).setParams(null);
            getStackedEvent(i).setEventname(null);
            getStackedEvent(i).setSender(null);
            getStackedEvent(i).setExist(false);
            getStackedEvent(i).setIo(null);
            getStackedEvent(i).setMsg(0);
        }
    }
    clearAdditionalEventStacksForIO(io);
}
public  void eventStackExecute() throws RPGException
{
        int count = 0;
        for (int i = 0; i < ScriptConstants.MAX_EVENT_STACK; i++) {
        if (getStackedEvent(i).exists())
        {
            int ioid = getStackedEvent(i).getIo().getRefId();
            if (Interactive.getInstance().hasIO(ioid))
            {
                if (getStackedEvent(i).getSender() != null)
                {
                    int senderid =
                            getStackedEvent(i).getSender().getRefId();
                    if (Interactive.getInstance().hasIO(senderid))
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
            getStackedEvent(i).setIo(null);
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
public  void eventStackExecuteAll() throws RPGException
{
    stackFlow = 9999999;
    eventStackExecute();
    stackFlow = 20;
}
public abstract void eventStackInit();
protected abstract void executeAdditionalStacks();
public  void forceDeath( IO io,  String target)
            throws RPGException
{
        int tioid = -1;
        if (target.equalsIgnoreCase("me")
                || target.equalsIgnoreCase("self")) {
        tioid = Interactive.getInstance().GetInterNum(io);
    } else {
        tioid = Interactive.getInstance().getTargetByNameTarget(target);
        if (tioid == -2)
        {
            tioid = Interactive.getInstance().GetInterNum(io);
        }
    }
        if (tioid >= 0) {
        IO tio = (IO)Interactive.getInstance().getIO(tioid);
        if (tio.hasIOFlag(IoGlobals.IO_03_NPC))
        {
            tio.getNPCData().forceDeath(io);
        }
    }
}
public  void freeAllGlobalVariables() throws RPGException
{
        if (gvars != null) {
        for (int i = gvars.length - 1; i >= 0; i--)
        {
            if (gvars[i] != null
                    && (gvars[i].getType() == ScriptConstants.TYPE_G_00_TEXT
                            || gvars[i]
                                    .getType() == ScriptConstants.TYPE_L_08_TEXT)
                    && gvars[i].getText() != null)
            {
                gvars[i].set(null);
            }
            gvars[i] = null;
        }
    }
}
/**
 * Removes all local variables from an {@link IO} and frees up the memory.
 * @param io the {@link IO}
 * @throws RPGException if an error occurs
 */
public  void freeAllLocalVariables( IO io) throws RPGException
{
        if (io != null
                && io.getScript() != null
                && io.getScript().hasLocalVariables()) {
        int i = io.getScript().getLocalVarArrayLength() - 1;
        for (; i >= 0; i--)
        {
            if (io.getScript().getLocalVariable(i) != null
                    && (io.getScript().getLocalVariable(i)
                            .getType() == ScriptConstants.TYPE_G_00_TEXT
                            || io.getScript().getLocalVariable(i)
                                    .getType() == ScriptConstants.TYPE_L_08_TEXT)
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
public  IO getEventSender()
{
    return eventSender;
}
/**
 * Gets the value of a global floating-point array variable.
 * @param name the name of the variable
 * @return <code>float</code>[]
 * @throws RPGException if the variable value was never set
 */
public  float[] getGlobalFloatArrayVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_03_FLOAT_ARR)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global Float Array variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getFloatArrayVal();
}
/**
 * Gets the global floating-point value assigned to a specific variable.
 * @param name the variable name
 * @return <code>float</code>
 * @throws RPGException if no such variable was assigned
 */
public  float getGlobalFloatVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_02_FLOAT)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global Float variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getFloatVal();
}
/**
 * Gets the value of a global integer array variable.
 * @param name the name of the variable
 * @return <code>int</code>[]
 * @throws RPGException if the variable value was never set
 */
public  int[] getGlobalIntArrayVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_05_INT_ARR)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global Integer Array variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getIntArrayVal();
}
/**
 * Gets the value of a global integer variable.
 * @param name the name of the variable
 * @return <code>int</code>
 * @throws RPGException if the variable value was never set
 */
public  int getGlobalIntVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_04_INT)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global Integer variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getIntVal();
}
/**
 * Gets the value of a global long integer array variable.
 * @param name the name of the variable
 * @return <code>long</code>[]
 * @throws RPGException if the variable value was never set
 */
public  long[] getGlobalLongArrayVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_07_LONG_ARR)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global Long Integer Array variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getLongArrayVal();
}
/**
 * Gets the value of a global long integer variable.
 * @param name the name of the variable
 * @return <code>long</code>
 * @throws RPGException if the variable value was never set
 */
public  long getGlobalLongVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_06_LONG)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global Long Integer variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getLongVal();
}
/**
 * Gets the local text array value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws RPGException if no such variable was assigned
 */
public  String[] getGlobalStringArrayVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_01_TEXT_ARR)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global Text Array variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getTextArrayVal();
}
/**
 * Gets the global text value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws RPGException if no such variable was assigned
 */
public  String getGlobalStringVariableValue( String name)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
        int index = -1;
        for (int i = 0; i < gvars.length; i++) {
        if (gvars[i] != null
                && gvars[i].getName().equals(name)
                && gvars[i].getType() == ScriptConstants.TYPE_G_00_TEXT)
        {
            index = i;
            break;
        }
    }
        if (index == -1) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Global String variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
        return gvars [index].getText();
}
public int getGlobalTargetParam( IO io)
{
    return io.getTargetinfo();
}
/**
 * Gets a global {@link Scriptable} variable.
 * @param name the name of the variable
 * @return {@link ScriptVariable}
 */
public  ScriptVariable getGlobalVariable( String name)
{
    ScriptVariable var = null;
    for (int i = gvars.length - 1; i >= 0; i--)
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
public  IO getIOMaxEvents() throws RPGException
{
        int max = -1;
        int ionum = -1;
    IO io = null;
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--) {
        if (Interactive.getInstance().hasIO(i))
        {
            IO hio = (IO)Interactive.getInstance().getIO(i);
            if (hio.getStatCount() > max)
            {
                ionum = i;
                max = hio.getStatCount();
            }
            hio = null;
        }
    }
        if (max > 0
                && ionum > -1) {
        io = (IO)Interactive.getInstance().getIO(ionum);
    }
        return io;
}
public  IO getIOMaxEventsSent() throws RPGException
{
        int max = -1;
        int ionum = -1;
    IO io = null;
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--) {
        if (Interactive.getInstance().hasIO(i))
        {
            IO hio = (IO)Interactive.getInstance().getIO(i);
            if (hio.getStatSent() > max)
            {
                ionum = i;
                max = hio.getStatSent();
            }
        }
    }
        if (max > 0
                && ionum > -1) {
        io = (IO)Interactive.getInstance().getIO(ionum);
    }
        return io;
}
/**
 * Gets the maximum number of timer scripts.
 * @return <code>int</code>
 */
public  int getMaxTimerScript()
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
 * Gets the id of a named script assigned to a specific IO.
 * @param io the IO
 * @param name the script's name
 * @return the script's id, if found. If no script exists, -1 is returned
 */
public  int getSystemIOScript( IO io,  String name)
{
    int index = -1;
    if (countTimers() > 0)
    {
        for (int i = 0; i < maxTimerScript; i++)
        {
            TIMER[] scriptTimers = getScriptTimers();
            if (scriptTimers[i].exists())
            {
                if (scriptTimers[i].getIo().equals(io)
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
public  bool hasGlobalVariable( String name)
{
    return getGlobalVariable(name) != null;
}
public  void initEventStats() throws RPGException
{
    eventTotalCount = 0;
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--) {
        if (Interactive.getInstance().hasIO(i))
        {
            IO io = (IO)Interactive.getInstance().getIO(i);
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
public  bool isDebug()
{
    return debug;
}
/**
 * Determines if an IO is in a specific group.
 * @param io the IO
 * @param group the group name
 * @return true if the IO is in the group; false otherwise
 */
public  bool isIOInGroup( IO io,  String group)
{
    bool val = false;
    if (io != null
            && group != null)
    {
        for (int i = 0; i < io.getNumIOGroups(); i++)
        {
            if (group.equalsIgnoreCase(io.getIOGroup(i)))
            {
                val = true;
                break;
            }
        }
    }
    return val;
}
public bool isPlayerInvisible(IO io)
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
        String[] split = params.split(" ");
        for (int i = 0, len = split.length - 1; i < len; i++)
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
 * Sends an event message to the IO.
 * @param io the IO
 * @param msg the message
 * @param params the script parameters
 * @return {@link int}
 * @throws RPGException if an error occurs
 */
public  int notifyIOEvent( IO io,  int msg,
         String params) throws RPGException
{
        int acceptance = ScriptConstants.REFUSE;
        if (sendIOScriptEvent(io, msg, null, null) != acceptance) {
        switch (msg)
        {
            case ScriptConstants.SM_017_DIE:
                if (io != null && Interactive.getInstance().hasIO(io))
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
        acceptance = ScriptConstants.ACCEPT;
    }
        return acceptance;
}
/**
 * Hides a target IO.
 * @param io the IO sending the event.
 * @param megahide if true, the target IO is "mega-hidden"
 * @param targetName the target's name
 * @param hideOn if true, the hidden flags are set; otherwise all hidden
 *            flags are removed
 * @throws RPGException if an error occurs
 */
public  void objectHide( IO io,  bool megahide,
         String targetName,  bool hideOn) throws RPGException
{
        int targetId =
                Interactive.getInstance().getTargetByNameTarget(targetName);
        if (targetId == -2) {
        targetId = io.getRefId();
    }
        if (Interactive.getInstance().hasIO(targetId)) {
        IO tio = (IO)Interactive.getInstance().getIO(targetId);
        tio.removeGameFlag(IoGlobals.GFLAG_MEGAHIDE);
        if (hideOn)
        {
            if (megahide)
            {
                tio.addGameFlag(IoGlobals.GFLAG_MEGAHIDE);
                tio.setShow(IoGlobals.SHOW_FLAG_MEGAHIDE);
            }
            else
            {
                tio.setShow(IoGlobals.SHOW_FLAG_HIDDEN);
            }
        }
        else if (tio.getShow() == IoGlobals.SHOW_FLAG_MEGAHIDE
              || tio.getShow() == IoGlobals.SHOW_FLAG_HIDDEN)
        {
            tio.setShow(IoGlobals.SHOW_FLAG_IN_SCENE);
            if (tio.hasIOFlag(IoGlobals.IO_03_NPC)
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
 * Removes an IO from all groups to which it was assigned.
 * @param io the IO
 */
public  void releaseAllGroups( IO io)
{
    while (io != null
            && io.getNumIOGroups() > 0)
    {
        io.removeGroup(io.getIOGroup(0));
    }
}
/**
 * Releases an event, clearing all variables.
 * @param event the scriptable event
 */
public  void releaseScript( SCRIPTABLE event)
{
    if (event != null) {
            event.clearLocalVariables();
    }
}
/**
 * Removes an IO from a group.
 * @param io the IO
 * @param group the group
 */
public  void removeGroup( IO io,  String group)
{
    if (io != null
            && group != null)
    {
        io.removeGroup(group);
    }
}
/**
 * Resets the object's script.
 * @param io the object
 * @param initialize if <tt>true</tt> and the object is script-loaded, it
 *            will be initialized again
 * @throws RPGException if an error occurs
 */
public  void reset( IO io,  bool initialize)
            throws RPGException
{
        // Release Script Local Variables
        if (io.getScript().getLocalVarArrayLength() > 0) {
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
        if (io.getOverscript().getLocalVarArrayLength() > 0) {
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
        if (!io.isScriptLoaded()) {
        resetObject(io, initialize);
    }
}
/**
 * Resets all interactive objects.
 * @param initialize if <tt>true</tt> and an object is script-loaded, it
 *            will be initialized again
 * @throws RPGException if an error occurs
 */
public  void resetAll( bool initialize) throws RPGException
{
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--) {
        if (Interactive.getInstance().hasIO(i))
        {
            IO io = (IO)Interactive.getInstance().getIO(i);
            if (!io.isScriptLoaded())
            {
                resetObject(io, initialize);
            }
        }
    }
}
/**
 * Resets the IO.
 * @param io the IO
 * @param initialize if <tt>true</tt>, the object needs to be initialized as
 *            well
 * @throws RPGException if an error occurs
 */
public  void resetObject( IO io,  bool initialize)
            throws RPGException
{
        // Now go for Script INIT/RESET depending on Mode
        int num = Interactive.getInstance().GetInterNum(io);
        if (Interactive.getInstance().hasIO(num)) {
        IO objIO = (IO)Interactive.getInstance().getIO(num);
        if (objIO != null
                && objIO.getScript() != null)
        {
            objIO.getScript().clearDisallowedEvents();

            if (initialize)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getScript(),
                        ScriptConstants.SM_001_INIT,
                        new Object[0],
                        objIO,
                        null);
            }
            objIO = (IO)Interactive.getInstance().getIO(num);
            if (objIO != null)
            {
                setMainEvent(objIO, "MAIN");
            }
        }

        // Do the same for Local Script
        objIO = (IO)Interactive.getInstance().getIO(num);
        if (objIO != null
                && objIO.getOverscript() != null)
        {
            objIO.getOverscript().clearDisallowedEvents();

            if (initialize)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getOverscript(),
                        ScriptConstants.SM_001_INIT,
                        new Object[0],
                        objIO,
                        null);
            }
        }

        // Sends InitEnd Event
        if (initialize)
        {
            objIO = (IO)Interactive.getInstance().getIO(num);
            if (objIO != null
                    && objIO.getScript() != null)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getScript(),
                        ScriptConstants.SM_033_INITEND,
                        new Object[0],
                        objIO,
                        null);
            }
            objIO = (IO)Interactive.getInstance().getIO(num);
            if (objIO != null
                    && objIO.getOverscript() != null)
            {
                sendScriptEvent((SCRIPTABLE)objIO.getOverscript(),
                        ScriptConstants.SM_033_INITEND,
                        new Object[0],
                        objIO,
                        null);
            }
        }

        objIO = (IO)Interactive.getInstance().getIO(num);
        if (objIO != null)
        {
            objIO.removeGameFlag(IoGlobals.GFLAG_NEEDINIT);
        }
    }
}
protected void runEvent(SCRIPTABLE script, String eventName, IO io)
            throws RPGException
{
        int msg = 0;
        if (eventName.equalsIgnoreCase("INIT")) {
        msg = ScriptConstants.SM_001_INIT;
    } else if (eventName.equalsIgnoreCase("HIT")) {
        msg = ScriptConstants.SM_016_HIT;
    } else if (eventName.equalsIgnoreCase("INIT_END")) {
        msg = ScriptConstants.SM_033_INITEND;
    }
        if (msg > 0) {
        runMessage(script, msg, io);
    } else {
        try
        {
            Method method;
            if (!eventName.startsWith("on"))
            {
                PooledStringBuilder sb =
                        StringBuilderPool.getInstance().getStringBuilder();
                sb.append("on");
                sb.append(eventName.toUpperCase().charAt(0));
                sb.append(eventName.substring(1));
                method = script.getClass().getMethod(sb.toString());
                sb.returnToPool();
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
    protected void runMessage(SCRIPTABLE script, int msg, IO io)
            throws RPGException
{
        switch (msg) {
        case ScriptConstants.SM_001_INIT:
            script.onInit();
        break;
        case ScriptConstants.SM_002_INVENTORYIN:
            script.onInventoryIn();
        break;
        case ScriptConstants.SM_004_INVENTORYUSE:
            script.onInventoryUse();
        break;
        case ScriptConstants.SM_007_EQUIPOUT:
            script.onUnequip();
        break;
        case ScriptConstants.SM_016_HIT:
            script.onHit();
        break;
        case ScriptConstants.SM_017_DIE:
            script.onDie();
        break;
        case ScriptConstants.SM_024_COMBINE:
            script.onCombine();
        break;
        case ScriptConstants.SM_033_INITEND:
            script.onInitEnd();
        break;
        case ScriptConstants.SM_041_LOAD:
            script.onLoad();
        break;
        case ScriptConstants.SM_043_RELOAD:
            script.onReload();
        break;
        case ScriptConstants.SM_045_OUCH:
            script.onOuch();
        break;
        case ScriptConstants.SM_046_HEAR:
            script.onHear();
        break;
        case ScriptConstants.SM_057_AGGRESSION:
            script.onAggression();
        break;
        case ScriptConstants.SM_069_IDENTIFY:
            script.onIdentify();
        break;
        default:
            throw new RPGException(ErrorMessage.INVALID_PARAM,
                    "No action defined for message " + msg);
    }
}
public  void sendEvent( IO io,  SendParameters params)
            throws RPGException
{
    IO oes = eventSender;
    eventSender = io;
        if (params.hasFlag(SendParameters.RADIUS)) {
        // SEND EVENT TO ALL OBJECTS IN A RADIUS

        // LOOP THROUGH ALL IOs.
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--)
        {
            if (Interactive.getInstance().hasIO(i))
            {
                IO iio = (IO)Interactive.getInstance().getIO(i);
                // skip cameras and markers
                // if (iio.hasIOFlag(IoGlobals.io_camera)
                // || iio.hasIOFlag(IoGlobals.io_marker)) {
                // continue;
                // }
                // skip IOs not in required group
                if (params.hasFlag(SendParameters.GROUP)) {
            if (!this.isIOInGroup(iio, params.getGroupName()))
            {
                continue;
            }
        }
        // if send event is for NPCs, send to NPCs,
        // if for Items, send to Items, etc...
        if ((params.hasFlag(SendParameters.IONpcData)

                && iio.hasIOFlag(IoGlobals.IO_03_NPC))
                            // || (params.hasFlag(SendParameters.FIX)
                            // && iio.hasIOFlag(IoGlobals.IO_FIX))
                            || (params.hasFlag(SendParameters.IOItemData)
                                    && iio.hasIOFlag(IoGlobals.IO_02_ITEM))) {
            Vector2 senderPos = new Vector2(),
                    ioPos = new Vector2();
            Interactive.getInstance().GetItemWorldPosition(io,
                    senderPos);
            Interactive.getInstance().GetItemWorldPosition(iio,
                    ioPos);
            // IF IO IS IN SENDER RADIUS, SEND EVENT
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
        if (params.hasFlag(SendParameters.ZONE)) {
            // SEND EVENT TO ALL OBJECTS IN A ZONE
            // ARX_PATH * ap = ARX_PATH_GetAddressByName(zonename);

            // if (ap != NULL) {
            // LOOP THROUGH ALL IOs.
            int i = Interactive.getInstance().getMaxIORefId();
            for (; i >= 0; i--) {
                if (Interactive.getInstance().hasIO(i)) {
                    IO iio = (IO)Interactive.getInstance().getIO(i);
                    // skip cameras and markers
                    // if (iio.hasIOFlag(IoGlobals.io_camera)
                    // || iio.hasIOFlag(IoGlobals.io_marker)) {
                    // continue;
                    // }
                    // skip IOs not in required group
                    if (params.hasFlag(SendParameters.GROUP)) {
                        if (!this.isIOInGroup(iio, params.getGroupName())) {
    continue;
}
}
                    // if send event is for NPCs, send to NPCs,
                    // if for Items, send to Items, etc...
                    if ((params.hasFlag(SendParameters.IONpcData)
                            && iio.hasIOFlag(IoGlobals.IO_03_NPC))
                            // || (params.hasFlag(SendParameters.FIX)
                            // && iio.hasIOFlag(IoGlobals.IO_FIX))
                            || (params.hasFlag(SendParameters.IOItemData)
                                    && iio.hasIOFlag(IoGlobals.IO_02_ITEM))) {
                        Vector2 ioPos = new Vector2();
Interactive.getInstance().GetItemWorldPosition(iio,
        ioPos);
// IF IO IS IN ZONE, SEND EVENT
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
        if (params.hasFlag(SendParameters.GROUP)) {
            // sends an event to all members of a group
            // LOOP THROUGH ALL IOs.
            int i = Interactive.getInstance().getMaxIORefId();
            for (; i >= 0; i--) {
                if (Interactive.getInstance().hasIO(i)) {
                    IO iio = (IO)Interactive.getInstance().getIO(i);
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
            int tioid = Interactive.getInstance().getTargetByNameTarget(
                    params.getTargetName());

            if (tioid == -2) {
                tioid = Interactive.getInstance().GetInterNum(io);
            }
            if (Interactive.getInstance().hasIO(tioid)) {
                io.setStatSent(io.getStatSent() + 1);
                this.stackSendIOScriptEvent(
                        (IO) Interactive.getInstance().getIO(tioid),
                        0,
                        params.getEventParameters(),
                        params.getEventName());
}
        }
        this.eventSender = oes;
    }
/**
 * Sends an initialization event to an IO. The initialization event runs the
 * local script first, followed by the over script.
 * @param io the IO
 * @return {@link int}
 * @throws RPGException if an error occurs
 */
public  int sendInitScriptEvent( IO io) throws RPGException
{
        if (io == null) {
        return -1;
    }
        int num = io.getRefId();
        if (!Interactive.getInstance().hasIO(num)) {
        return -1;
    }
    IO oldEventSender = eventSender;
    eventSender = null;
    // send script the init message
    IO hio = (IO) Interactive.getInstance().getIO(num);
        if (hio.getScript() != null) {
        GLOB = 0;
        sendScriptEvent((SCRIPTABLE)hio.getScript(),
                ScriptConstants.SM_001_INIT,
                null,
                hio,
                null);
    }
    hio = null;
        // send overscript the init message
        if (Interactive.getInstance().getIO(num) != null) {
        hio = (IO)Interactive.getInstance().getIO(num);
        if (hio.getOverscript() != null)
        {
            GLOB = 0;
            sendScriptEvent((SCRIPTABLE)hio.getOverscript(),
                    ScriptConstants.SM_001_INIT,
                    null,
                    hio,
                    null);
        }
        hio = null;
    }
        // send script the init end message
        if (Interactive.getInstance().getIO(num) != null) {
        hio = (IO)Interactive.getInstance().getIO(num);
        if (hio.getScript() != null)
        {
            GLOB = 0;
            sendScriptEvent((SCRIPTABLE)hio.getScript(),
                    ScriptConstants.SM_033_INITEND,
                    null,
                    hio,
                    null);
        }
        hio = null;
    }
        // send overscript the init end message
        if (Interactive.getInstance().getIO(num) != null) {
        hio = (IO)Interactive.getInstance().getIO(num);
        if (hio.getOverscript() != null)
        {
            GLOB = 0;
            sendScriptEvent((SCRIPTABLE)hio.getOverscript(),
                    ScriptConstants.SM_033_INITEND,
                    null,
                    hio,
                    null);
        }
        hio = null;
    }
    eventSender = oldEventSender;
        return ScriptConstants.ACCEPT;
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
 * @throws RPGException if an error occurs
 */
public  int sendIOScriptEvent( IO target,  int msg,
         Object[] params,  String eventname) throws RPGException
{
        // checks invalid IO
        if (target == null) {
        return -1;
    }
        int num = target.getRefId();

        if (Interactive.getInstance().hasIO(num)) {
        IO originalEventSender = eventSender;
        if (msg == ScriptConstants.SM_001_INIT
                || msg == ScriptConstants.SM_033_INITEND)
        {
            IO hio = (IO)Interactive.getInstance().getIO(num);
            sendIOScriptEventReverse(hio, msg, params, eventname);
            eventSender = originalEventSender;
            hio = null;
        }

        if (Interactive.getInstance().hasIO(num))
        {
            // if this IO only has a Local script, send event to it
            IO hio = (IO)Interactive.getInstance().getIO(num);
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

            // If this IO has a Global script send to Local (if exists)
            // then to Global if not overridden by Local
            int s = sendScriptEvent(
                    (SCRIPTABLE)hio.getOverscript(),
                    msg,
                        params,
                    hio,
                    eventname);
            if (s != ScriptConstants.REFUSE)
            {
                eventSender = originalEventSender;
                GLOB = 0;

                if (Interactive.getInstance().hasIO(num))
                {
                    hio = (IO)Interactive.getInstance().getIO(num);
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
                    return ScriptConstants.REFUSE;
                }
            }
            hio = null;
        }
        GLOB = 0;
    }

        // Refused further processing.
        return ScriptConstants.REFUSE;
}
private int sendIOScriptEventReverse( IO io,  int msg,
         Object[] params,  String eventname) throws RPGException
{
        // checks invalid IO
        if (io == null) {
        return -1;
    }
        // checks for no script assigned
        if (io.getOverscript() == null
                && io.getScript() == null) {
        return -1;
    }
        int num = io.getRefId();
        if (Interactive.getInstance().hasIO(num)) {
        IO hio = (IO)Interactive.getInstance().getIO(num);
        // if this IO only has a Local script, send event to it
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

        // If this IO has a Global script send to Local (if exists)
        // then to global if no overriden by Local
        if (Interactive.getInstance().hasIO(num))
        {
            hio = (IO)Interactive.getInstance().getIO(num);
            int s = sendScriptEvent(
                    (SCRIPTABLE)hio.getScript(),
                    msg,
                        params,
                    hio,
                    eventname);
            if (s != ScriptConstants.REFUSE)
            {
                GLOB = 0;
                if (Interactive.getInstance().hasIO(io.getRefId()))
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
                    return ScriptConstants.REFUSE;
                }
            }
        }
        hio = null;
        GLOB = 0;
    }
        // Refused further processing.
        return ScriptConstants.REFUSE;
}
/**
 * Sends a scripted event to all IOs.
 * @param msg the message
 * @param dat any script variables
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public  int sendMsgToAllIO( int msg,  Object[] dat)
            throws RPGException
{
        int ret = ScriptConstants.ACCEPT;
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--) {
        if (Interactive.getInstance().hasIO(i))
        {
            IO io = (IO)Interactive.getInstance().getIO(i);
            if (sendIOScriptEvent(io, msg, dat,
                    null) == ScriptConstants.REFUSE)
            {
                ret = ScriptConstants.REFUSE;
            }
        }
    }
        return ret;
}
/**
 * Sends a scripted event to an IO.
 * @param localScript the local script the IO shoulod follow
 * @param msg the event message
 * @param params any parameters to be applied
 * @param io
 * @param eventName
 * @param info
 * @return
 * @throws RPGException
 */
public  int sendScriptEvent( SCRIPTABLE localScript,
         int msg,  Object[] params,  IO io,
         String eventName) throws RPGException
{
        int retVal = ScriptConstants.ACCEPT;
    bool keepGoing = true;
        if (localScript == null) {
        throw new RPGException(
                ErrorMessage.INVALID_PARAM, "script cannot be null");
    }
        if (io != null) {
        if (io.hasGameFlag(IoGlobals.GFLAG_MEGAHIDE)
                && msg != ScriptConstants.SM_043_RELOAD)
        {
            return ScriptConstants.ACCEPT;
        }

        if (io.getShow() == IoGlobals.SHOW_FLAG_DESTROYED)
        {
            // destroyed
            return ScriptConstants.ACCEPT;
        }
        eventTotalCount++;
        io.setStatCount(io.getStatCount() + 1);

        if (io.hasIOFlag(IoGlobals.IO_06_FREEZESCRIPT))
        {
            if (msg == ScriptConstants.SM_041_LOAD)
            {
                return ScriptConstants.ACCEPT;
            }
            return ScriptConstants.REFUSE;
        }

        if (io.hasIOFlag(IoGlobals.IO_03_NPC)
                && !io.hasIOFlag(IoGlobals.IO_09_DWELLING))
        {
            if (io.getNPCData().getBaseLife() <= 0.f
                    && msg != ScriptConstants.SM_001_INIT
                    && msg != ScriptConstants.SM_012_DEAD
                    && msg != ScriptConstants.SM_017_DIE
                    && msg != ScriptConstants.SM_255_EXECUTELINE
                    && msg != ScriptConstants.SM_043_RELOAD
                    && msg != ScriptConstants.SM_255_EXECUTELINE
                    && msg != ScriptConstants.SM_028_INVENTORY2_OPEN
                    && msg != ScriptConstants.SM_029_INVENTORY2_CLOSE)
            {
                return ScriptConstants.ACCEPT;
            }
        }
        // change weapon if one breaks
        /*
         * if (((io->ioflags & IO_FIX) || (io->ioflags & IO_ITEM)) && (msg
         * == ScriptConstants.SM_BREAK)) { ManageCasseDArme(io); }
         */
    }
    // use master script if available
    SCRIPTABLE script = (SCRIPTABLE) localScript.getMaster();
        if (script == null) { // no master - use local script
        script = localScript;
    }
        // set parameters on script that will be used
        if (params != null
                && params.length > 0) {
        for (int i = 0; i < params.length; i += 2) {
            script.setLocalVariable((String) params[i], params[i + 1]);
}
        }
        // run the event
        if (eventName != null
                && eventName.length() > 0) {
            runEvent(script, eventName, io);
        } else {
            if (eventIsDisallowed(msg, script)) {
                return ScriptConstants.REFUSE;
            }
            runMessage(script, msg, io);
        }
        int ret = ScriptConstants.ACCEPT;
        return ret;
    }
    /**
     * Sets the value for the flag indicating whether debug output is turned on.
     * @param val the value to set
     */
    public  void setDebug( bool val)
{
    this.debug = val;
}
public  void setEvent( IO io,  String event,
         bool isOn)
{
    if (event.equalsIgnoreCase("COLLIDE_NPC")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(
                    ScriptConstants.DISABLE_COLLIDE_NPC);
        }
        else
        {
            io.getScript().assignDisallowedEvent(
                    ScriptConstants.DISABLE_COLLIDE_NPC);
        }
    } else if (event.equalsIgnoreCase("CHAT")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConstants.DISABLE_CHAT);
        }
        else
        {
            io.getScript().assignDisallowedEvent(ScriptConstants.DISABLE_CHAT);
        }
    } else if (event.equalsIgnoreCase("HIT")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConstants.DISABLE_HIT);
        }
        else
        {
            io.getScript().assignDisallowedEvent(ScriptConstants.DISABLE_HIT);
        }
    } else if (event.equalsIgnoreCase("INVENTORY2_OPEN")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(
                    ScriptConstants.DISABLE_INVENTORY2_OPEN);
        }
        else
        {
            io.getScript().assignDisallowedEvent(
                    ScriptConstants.DISABLE_INVENTORY2_OPEN);
        }
    } else if (event.equalsIgnoreCase("DETECTPLAYER")) {
        if (isOn)
        {
            io.getScript()
                    .removeDisallowedEvent(ScriptConstants.DISABLE_DETECT);
        }
        else
        {
            io.getScript()
                    .assignDisallowedEvent(ScriptConstants.DISABLE_DETECT);
        }
    } else if (event.equalsIgnoreCase("HEAR")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConstants.DISABLE_HEAR);
        }
        else
        {
            io.getScript().assignDisallowedEvent(ScriptConstants.DISABLE_HEAR);
        }
    } else if (event.equalsIgnoreCase("AGGRESSION")) {
        if (isOn)
        {
            io.getScript()
                    .removeDisallowedEvent(ScriptConstants.DISABLE_AGGRESSION);
        }
        else
        {
            io.getScript()
                    .assignDisallowedEvent(ScriptConstants.DISABLE_AGGRESSION);
        }
    } else if (event.equalsIgnoreCase("MAIN")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(ScriptConstants.DISABLE_MAIN);
        }
        else
        {
            io.getScript().assignDisallowedEvent(ScriptConstants.DISABLE_MAIN);
        }
    } else if (event.equalsIgnoreCase("CURSORMODE")) {
        if (isOn)
        {
            io.getScript()
                    .removeDisallowedEvent(ScriptConstants.DISABLE_CURSORMODE);
        }
        else
        {
            io.getScript()
                    .assignDisallowedEvent(ScriptConstants.DISABLE_CURSORMODE);
        }
    } else if (event.equalsIgnoreCase("EXPLORATIONMODE")) {
        if (isOn)
        {
            io.getScript().removeDisallowedEvent(
                    ScriptConstants.DISABLE_EXPLORATIONMODE);
        }
        else
        {
            io.getScript().assignDisallowedEvent(
                    ScriptConstants.DISABLE_EXPLORATIONMODE);
        }
    }
}
/**
 * Sets the value of the eventSender.
 * @param val the new value to set
 */
public  void setEventSender( IO val)
{
    eventSender = val;
}
/**
 * Sets a global variable.
 * @param name the name of the global variable
 * @param value the variable's value
 * @throws RPGException if an error occurs
 */
public  void setGlobalVariable( String name,  Object value)
            throws RPGException
{
        if (gvars == null) {
        gvars = new ScriptVariable[0];
    }
    bool found = false;
        for (int i = gvars.length - 1; i >= 0; i--) {
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
        if (!found) {
        // create a new variable and add to the global array
        ScriptVariable var = null;
        if (value instanceof String
                    || value instanceof char[]) {
            var = new ScriptVariable(name, ScriptConstants.TYPE_G_00_TEXT,
                    value);
        } else if (value instanceof String[]
                    || value instanceof char[][]) {
            var = new ScriptVariable(name,
                    ScriptConstants.TYPE_G_01_TEXT_ARR, value);
        } else if (value instanceof Float) {
            var = new ScriptVariable(name, ScriptConstants.TYPE_G_02_FLOAT,
                    value);
        } else if (value instanceof Double) {
            var = new ScriptVariable(name, ScriptConstants.TYPE_G_02_FLOAT,
                    value);
        } else if (value instanceof float[]) {
            var = new ScriptVariable(name,
                    ScriptConstants.TYPE_G_03_FLOAT_ARR, value);
        } else if (value instanceof Integer) {
            var = new ScriptVariable(name, ScriptConstants.TYPE_G_04_INT,
                    value);
        } else if (value instanceof int[]) {
            var = new ScriptVariable(name,
                    ScriptConstants.TYPE_G_05_INT_ARR, value);
        } else if (value instanceof Long) {
            var = new ScriptVariable(name, ScriptConstants.TYPE_G_06_LONG,
                    value);
        } else if (value instanceof long[]) {
            var = new ScriptVariable(name,
                    ScriptConstants.TYPE_G_07_LONG_ARR, value);
        } else {
            PooledStringBuilder sb =
                    StringBuilderPool.getInstance().getStringBuilder();
            try
            {
                sb.append("Global variable ");
                sb.append(name);
                sb.append(" was passed new value of type ");
                sb.append(value.getClass().getCanonicalName());
                sb.append(". Only String, String[], char[][], Float, ");
                sb.append("float[], Integer, int[], Long, or long[] ");
                sb.append("allowed.");
            }
            catch (PooledException e)
            {
                throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
            }
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.toString());
            sb.returnToPool();
            sb = null;
            throw ex;
        }
        gvars = ArrayUtilities.getInstance().extendArray(var, gvars);
    }
}
/**
 * Sets the main event for an IO.
 * @param io the IO
 * @param newevent the event's name
 */
public  void setMainEvent( IO io,  String newevent)
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
protected  void setMaxTimerScript( int val)
{
    maxTimerScript = val;
}
protected abstract void setScriptTimer(int index, TIMER timer);
/**
 * Processes and IO's speech.
 * @param io the IO
 * @param params the {@link SpeechParameters}
 * @throws RPGException 
 */
public  void speak( IO io,  SpeechParameters params) throws RPGException
{
        // speech variables
        // ARX_CINEMATIC_SPEECH acs;
        // acs.type = ARX_CINE_SPEECH_NONE;
        long voixoff = 0;
        int mood = ANIM_TALK_NEUTRAL;
        if (params.isKillAllSpeech()) {
        // ARX_SPEECH_Reset();
    } else {
        if (params.hasFlag(SpeechParameters.HAPPY)) {
            mood = ANIM_TALK_HAPPY;
        }
        if (params.hasFlag(SpeechParameters.ANGRY)) {
            mood = ANIM_TALK_ANGRY;
        }
        if (params.hasFlag(SpeechParameters.OFF_VOICE)) {
            voixoff = 2;
        }
        if (params.hasFlag(SpeechParameters.KEEP_SPEECH)
                || params.hasFlag(SpeechParameters.ZOOM_SPEECH)
                || params.hasFlag(SpeechParameters.SPEECH_CCCTALKER_L)
                || params.hasFlag(SpeechParameters.SPEECH_CCCTALKER_R)
                || params.hasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)
                || params.hasFlag(SpeechParameters.SPEECH_CCCLISTENER_R)
                || params.hasFlag(SpeechParameters.SIDE_L)
                || params.hasFlag(SpeechParameters.SIDE_R)) {
            // FRAME_COUNT = 0;
            if (params.hasFlag(SpeechParameters.KEEP_SPEECH)) {
                // acs.type = ARX_CINE_SPEECH_KEEP;
                // acs.pos1.x = LASTCAMPOS.x;
                // acs.pos1.y = LASTCAMPOS.y;
                // acs.pos1.z = LASTCAMPOS.z;
                // acs.pos2.a = LASTCAMANGLE.a;
                // acs.pos2.b = LASTCAMANGLE.b;
                // acs.pos2.g = LASTCAMANGLE.g;
            }
            if (params.hasFlag(SpeechParameters.ZOOM_SPEECH)) {
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

                if (params.hasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, -1);
                }
            }
            if (params.hasFlag(SpeechParameters.SPEECH_CCCTALKER_L)
                    || params
                                .hasFlag(SpeechParameters.SPEECH_CCCTALKER_R)) {
                if (params.hasFlag(SpeechParameters.SPEECH_CCCTALKER_L)) {
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

                if (params.hasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, acs.ionum);
                }
            }
            if (params.hasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)
                    || params.hasFlag(
                            SpeechParameters.SPEECH_CCCLISTENER_R)) {
                if (params.hasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)) {
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

                if (params.hasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, acs.ionum);
                }
            }
            if (params.hasFlag(SpeechParameters.SIDE_L)
                    || params.hasFlag(SpeechParameters.SIDE_R)) {
                if (params.hasFlag(SpeechParameters.SIDE_L)) {
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

                if (params.hasFlag(SpeechParameters.PLAYER)) {
                    // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                } else {
                    // ComputeACSPos(&acs, io, acs.ionum);
                }
            }
        }

        long speechnum = 0;

        if (params.getSpeechName() == null
                || params.getSpeechName().length() == 0) {
            // ARX_SPEECH_ClearIOSpeech(io);
        } else {
            if (params.hasFlag(SpeechParameters.NO_TEXT)) {
                // voixoff |= ARX_SPEECH_FLAG_NOTEXT;
            }

            // if (!CINEMASCOPE) voixoff |= ARX_SPEECH_FLAG_NOTEXT;
            if (params.hasFlag(SpeechParameters.PLAYER)) {
                // speechnum = ARX_SPEECH_AddSpeech(inter.iobj[0], temp1,
                // PARAM_LOCALISED, mood, voixoff);
            } else {
                // speechnum = ARX_SPEECH_AddSpeech(io, temp1,
                // PARAM_LOCALISED, mood, voixoff);
                speechnum = Speech.getInstance().ARX_SPEECH_AddSpeech(io,
                        mood, params.getSpeechName(), voixoff);
            }

            if (speechnum >= 0)
            {
                // ARX_SCRIPT_Timer_GetDefaultName(timername2);
                // sprintf(timername, "SPEAK_%s", timername2);
                // aspeech[speechnum].scrpos = pos;
                // aspeech[speechnum].es = es;
                // aspeech[speechnum].ioscript = io;
                if (params.hasFlag(SpeechParameters.UNBREAKABLE)) {
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
 * @param group the name of the IO group
 * @param msg the script message
 * @param params the parameters assigned to the script
 * @param eventname the event name
 * @throws RPGException if an error occurs
 */
public  void stackSendGroupScriptEvent( String group,
         int msg,  Object[] params,  String eventname)
            throws RPGException
{
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--) {
        if (Interactive.getInstance().hasIO(i))
        {
            IO io = (IO)Interactive.getInstance().getIO(i);
            if (isIOInGroup(io, group))
            {
                stackSendIOScriptEvent(io, msg, params, eventname);
            }
            io = null;
        }
    }
}
/**
 * Sends an IO scripted event to the event stack, to be fired during the
 * game cycle.
 * @param io the IO
 * @param msg the script message
 * @param params the parameters assigned to the script
 * @param eventname the event name
 */
public  void stackSendIOScriptEvent( IO io,
         int msg,  Object[] params,  String eventname)
{
    for (int i = 0; i < ScriptConstants.MAX_EVENT_STACK; i++)
    {
        if (!getStackedEvent(i).exists())
        {
            if (params != null
                    && params.length > 0) {
        getStackedEvent(i).setParams(params);
    } else {
        getStackedEvent(i).setParams(null);
    }
    if (eventname != null
            && eventname.length() > 0)
    {
        getStackedEvent(i).setEventname(eventname);
    }
    else
    {
        getStackedEvent(i).setEventname(null);
    }

    getStackedEvent(i).setSender(eventSender);
    getStackedEvent(i).setIo(io);
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
     * @throws RPGException if an error occurs
     */
    public  void stackSendMsgToAllNPCIO( int msg,  Object[] dat)
            throws RPGException
{
        int i = Interactive.getInstance().getMaxIORefId();
        for (; i >= 0; i--) {
        if (Interactive.getInstance().hasIO(i))
        {
            IO io = (IO)Interactive.getInstance().getIO(i);
            if (io.hasIOFlag(IoGlobals.IO_03_NPC))
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
public  void startTimer(
         ScriptTimerInitializationParameters<IO, SCRIPTABLE> params)
{
    int timerNum = timerGetFree();
    ScriptTimer timer = getScriptTimer(timerNum);
    timer.setScript(params.getScript());
    timer.setExists(true);
    timer.setIo(params.getIo());
    timer.setCycleLength(params.getMilliseconds());
    if (params.getName() == null
            || (params.getName() != null
                    && params.getName().length() == 0)) {
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
    timer.clearFlags();
    timer.addFlag(params.getFlagValues());
}
/**
 * Teleports an IO to a target location.
 * @param io the io calling for the teleport event
 * @param behind flag indicating the target teleports behind
 * @param isPlayer flag indicating object being teleported is the player
 * @param initPosition flag indicating the object being teleported goes to
 *            its initial position
 * @param target the name of teleport destination
 * @throws RPGException if an error occurs
 */
public  void teleport( IO io,  bool behind,
         bool isPlayer,  bool initPosition,
         String target) throws RPGException
{
        if (behind) {
        Interactive.getInstance().ARX_INTERACTIVE_TeleportBehindTarget(io);
    } else {
        if (!initPosition)
        {
            int ioid =
                    Interactive.getInstance().getTargetByNameTarget(target);

            if (ioid == -2)
            {
                ioid = Interactive.getInstance().GetInterNum(io);
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
                        io.setShow(IoGlobals.SHOW_FLAG_IN_SCENE);
                    }
                    IO pio = (IO)Interactive.getInstance().getIO(
                            ProjectConstants.getInstance().getPlayer());
                    Interactive.getInstance().ARX_INTERACTIVE_Teleport(
                            io, pio.getPosition());
                    pio = null;
                }
                else
                {
                    if (Interactive.getInstance().hasIO(ioid))
                    {
                        IO tio = (IO)Interactive.getInstance().getIO(ioid);
                        Vector2 pos = new Vector2();

                        if (Interactive.getInstance()
                                .GetItemWorldPosition(tio, pos))
                        {
                            if (isPlayer)
                            {
                                IO pio = (IO)Interactive.getInstance()
                                        .getIO(
                                                ProjectConstants
                                                        .getInstance()
                                                        .getPlayer());
                                Interactive.getInstance()
                                        .ARX_INTERACTIVE_Teleport(
                                                pio, pos);
                                pio = null;
                            }
                            else
                            {
                                if (io.hasIOFlag(IoGlobals.IO_03_NPC)
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
                                        io.setShow(
                                                IoGlobals.SHOW_FLAG_IN_SCENE);
                                    }
                                    Interactive.getInstance()
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
                    if (Interactive.getInstance().GetItemWorldPosition(io,
                            pos))
                    {
                        IO pio = (IO)Interactive.getInstance().getIO(
                                ProjectConstants.getInstance().getPlayer());
                        Interactive.getInstance().ARX_INTERACTIVE_Teleport(
                                pio, pos);
                        pio = null;

                    }
                }
                else
                {
                    if (io.hasIOFlag(IoGlobals.IO_03_NPC)
                            && io.getNPCData().getBaseLife() <= 0)
                    {
                        // do nothing
                    }
                    else
                    {
                        if (io.getShow() != IoGlobals.SHOW_FLAG_HIDDEN
                                && io.getShow() != IoGlobals.SHOW_FLAG_MEGAHIDE)
                        {
                            io.setShow(IoGlobals.SHOW_FLAG_IN_SCENE);
                        }
                        Interactive.getInstance().ARX_INTERACTIVE_Teleport(
                                io, io.getInitPosition());
                    }
                }
            }
        }
    }
}
public  void timerCheck() throws RPGException
{
        if (countTimers() > 0) {
        for (int i = 0, len = this.maxTimerScript; i < len; i++)
        {
            TIMER timer = getScriptTimers()[i];
            if (timer.exists())
            {
                long currentTime = Time.getInstance().getGameTime();
                if (timer.isTurnBased())
                {
                    currentTime = Time.getInstance().getGameRound();
                }
                if (timer.hasFlag(1))
                {
                    if (!timer.getIo().hasGameFlag(
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
                    IO io = timer.getIo();
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
                            && Interactive.getInstance().hasIO(io))
                    {
                        timer.getAction().process();
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
public  void timerClearAll()
{
    for (int i = 0; i < maxTimerScript; i++)
    {
        timerClearByNum(i);
    }
}
public  void timerClearAllLocalsForIO( IO io)
{
    TIMER[] scriptTimers = getScriptTimers();
    for (int i = 0; i < maxTimerScript; i++)
    {
        if (scriptTimers[i].exists())
        {
            if (scriptTimers[i].getIo().equals(io)
                    && scriptTimers[i].getScript()
                            .equals(io.getOverscript()))
            {
                timerClearByNum(i);
            }
        }
    }
}
/**
 * Clears a timer by the IO assigned to it.
 * @param io the IO
 */
public  void timerClearByIO( IO io)
{
    if (io != null)
    {
        TIMER[] scriptTimers = getScriptTimers();
        for (int i = 0; i < maxTimerScript; i++)
        {
            if (scriptTimers[i] != null
                    && scriptTimers[i].exists())
            {
                if (scriptTimers[i].getIo().getRefId() == io.getRefId())
                {
                    timerClearByNum(i);
                }
            }
        }
    }
}
public  void timerClearByNameAndIO( String timername,
         IO io)
{
    if (io != null)
    {
        TIMER[] scriptTimers = getScriptTimers();
        for (int i = 0; i < maxTimerScript; i++)
        {
            if (scriptTimers[i] != null
                    && scriptTimers[i].exists())
            {
                if (scriptTimers[i].getIo().getRefId() == io.getRefId()
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
public void timerClearByNum( int timeridx)
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
private int timerExist( String texx)
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
public  void timerFirstInit( int number)
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
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("TIMER_");
            sb.append(i);
        }
        catch (PooledException e)
        {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        i++;

        if (timerExist(sb.toString()) == -1)
        {
            texx = sb.toString();
            sb.returnToPool();
            sb = null;
            break;
        }
        sb.returnToPool();
        sb = null;
    }
    return texx;
}
/**
 * Gets the index of a free script timer.
 * @return <code>int</code>
 */
public  int timerGetFree()
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
 * Determines if an IO is speaking.
 * @param io the IO
 * @return <tt>true</tt> if the IO is speaking; <tt>false</tt> otherwise
 */
public bool amISpeaking( IO io)
{
    // TODO Auto-generated method stub
    // GO THROUGH ALL SPEECH INSTANCES.  IF THE IO IS SPEAKING
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
    return Time.getInstance().getGameTime(false);
}
    }
}
