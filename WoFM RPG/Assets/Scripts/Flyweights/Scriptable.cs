using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class Scriptable
    {
        /** bit flag storing which events are allowed. */
        private long allowedEvent;
        /** the list of actions for an event. */
        private Map<Integer, ScriptAction[]> eventActions;
        /** the IO associated with this script. */
        private IO io;
        /** the array of local {@link ScriptVariable}s. */
        private ScriptVariable[] lvar;
        /** the master script. */
        private Scriptable<IO> master;
        /** the list of script timers. */
        private  int[] timers;
        /** Creates a new instance of {@link Scriptable}. */
        public Scriptable()
        {
            this(null);
        }
        /**
         * Creates a new instance of {@link Scriptable}.
         * @param ioInstance the IO associated with this script
         */
        public Scriptable( IO ioInstance)
        {
            timers = new int[ScriptConstants.MAX_SCRIPTTIMERS];
            lvar = new ScriptVariable[0];
            eventActions = new HashMap<Integer, ScriptAction[]>();
            io = ioInstance;
        }
        /**
         * Adds a local variable.
         * @param svar the local variable
         */
        public  void addLocalVariable( ScriptVariable svar)
        {
            int index = -1;
            for (int i = 0; i < lvar.length; i++)
            {
                if (lvar[i] == null)
                {
                    lvar[i] = svar;
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                lvar = ArrayUtilities.getInstance().extendArray(svar, lvar);
            }
        }
        /**
         * Adds a {@link ScriptAction} to the list of actions for an event.
         * @param eventID the event ID - usually the script message #
         * @param action the script action
         */
        public  void addScriptAction( int eventID,
                 ScriptAction action)
        {
            if (eventActions.get(eventID) == null)
            {
                eventActions.put(eventID, new ScriptAction[0]);
            }
            if (action != null)
            {
                action.setScript(this);
                eventActions.put(eventID, ArrayUtilities.getInstance().extendArray(
                        action, eventActions.get(eventID)));
            }
        }
        /**
         * Assigns a bit flag for an allowed event.
         * @param event the event flag
         */
        public  void assignDisallowedEvent( int event) {
            allowedEvent |= event;
        }
        /**
         * Changes the IO's behavior.
         * @param params the behavior parameters
         */
        public  void behavior( BehaviorParameters params)
        {
            if (io.hasIOFlag(IoGlobals.IO_03_NPC))
            {
                if ("STACK".equalsIgnoreCase(params.getAction()))
                {
                    io.getNPCData().ARX_NPC_Behaviour_Stack();
                }
                else if ("UNSTACK".equalsIgnoreCase(params.getAction()))
                {
                    io.getNPCData().ARX_NPC_Behaviour_UnStack();
                }
                else if ("UNSTACKALL".equalsIgnoreCase(params.getAction()))
                {
                    io.getNPCData().resetBehavior();
                }
                else
                {
                    io.getNPCData().ARX_NPC_Behaviour_Change(params.getFlags(),
                            (long) params.getBehaviorParam());
                    if (params.getMovemode() > -1) {
                        io.getNPCData().setMovemode(params.getMovemode());
                    }
                    if (params.getTactics() > -1) {
                        io.getNPCData().setTactics(params.getTactics());
                    }
                    if (params.getTargetInfo() != -1) {
                        io.setTargetinfo(params.getTargetInfo());
                    }
                }
            }
        }
        /** Clears the bit flags for allowed events. */
        public  void clearDisallowedEvents()
        {
            allowedEvent = 0;
        }
        /**
         * Clears a local variable assigned to the {@link Scriptable}.
         * @param varName the variable's name
         */
        public  void clearLocalVariable( String varName)
        {
            for (int i = lvar.length - 1; i >= 0; i--)
            {
                if (lvar[i] != null
                        && lvar[i].getName() != null
                        && lvar[i].getName().equalsIgnoreCase(varName))
                {
                    lvar[i].clear();
                }
                lvar[i] = null;
            }
        }
        /** Clears all local variables assigned to the {@link Scriptable}. */
        public  void clearLocalVariables()
        {
            for (int i = lvar.length - 1; i >= 0; i--)
            {
                if (lvar[i] != null)
                {
                    lvar[i].clear();
                }
                lvar[i] = null;
            }
        }
        /**
         * Gets all event actions for a scripted event.
         * @param eventID the event ID - usually the script message #
         * @return {@link ScriptAction}[]
         */
        public  ScriptAction[] getEventActions( int eventID)
        {
            ScriptAction[] actions = new ScriptAction[0];
            if (eventActions.get(eventID) == null)
            {
                eventActions.put(eventID, actions);
            }
            return eventActions.get(eventID);
        }
        /**
         * Gets the IO associated with this script.
         * @return {@link IO}
         */
        public  IO getIO()
        {
            return io;
        }
        /**
         * Gets the local floating-point array value assigned to a specific
         * variable.
         * @param name the variable name
         * @return {@link String}
         * @throws PooledException if one occurs
         * @throws RPGException if no such variable was assigned
         */
        public  float[] getLocalFloatArrayVariableValue( String name)
            throws PooledException, RPGException {
        ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_11_FLOAT_ARR) {
            PooledStringBuilder sb =
                    StringBuilderPool.getInstance().getStringBuilder();
        sb.append("Local floating-point array type variable ");
            sb.append(name);
            sb.append(" was never set.");
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.toString());
        sb.returnToPool();
            throw ex;
        }
        return svar.getFloatArrayVal();
    }
/**
 * Gets the local floating-point value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws PooledException if one occurs
 * @throws RPGException if no such variable was assigned
 */
public  float getLocalFloatVariableValue( String name)
            throws RPGException
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_10_FLOAT) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Local floating-point variable ");
            sb.append(name);
            sb.append(" was never set.");
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
        return svar.getFloatVal();
}
/**
 * Gets the local integer array value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws PooledException if one occurs
 * @throws RPGException if no such variable was assigned
 */
public  int[] getLocalIntArrayVariableValue( String name)
            throws PooledException, RPGException {
        ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_13_INT_ARR) {
            PooledStringBuilder sb =
                    StringBuilderPool.getInstance().getStringBuilder();
sb.append("Local floating-point variable ");
            sb.append(name);
            sb.append(" was never set.");
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.toString());
sb.returnToPool();
            throw ex;
        }
        return svar.getIntArrayVal();
    }
    /**
     * Gets the local integer value assigned to a specific variable.
     * @param name the variable name
     * @return {@link String}
     * @throws RPGException if no such variable was assigned
     */
    public  int getLocalIntVariableValue( String name)
            throws RPGException
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_12_INT) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Local integer variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                sb.toString());
        sb.returnToPool();
        throw ex;
    }
        return svar.getIntVal();
}
/**
 * Gets the local long integer value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws PooledException if one occurs
 * @throws RPGException if no such variable was assigned
 */
public  long[] getLocalLongArrayVariableValue( String name)
            throws PooledException, RPGException {
        ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_15_LONG_ARR) {
            PooledStringBuilder sb =
                    StringBuilderPool.getInstance().getStringBuilder();
sb.append("Local floating-point variable ");
            sb.append(name);
            sb.append(" was never set.");
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.toString());
sb.returnToPool();
            throw ex;
        }
        return svar.getLongArrayVal();
    }
    /**
     * Gets the local long integer value assigned to a specific variable.
     * @param name the variable name
     * @return {@link String}
     * @throws PooledException if one occurs
     */
    public  long getLocalLongVariableValue( String name)
            throws RPGException
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_14_LONG) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Local long integer variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                sb.toString());
        sb.returnToPool();
        throw ex;
    }
        return svar.getLongVal();
}
/**
 * Gets the local text array value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws RPGException if no such variable was assigned
 */
public  String[] getLocalStringArrayVariableValue( String name)
            throws RPGException
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_09_TEXT_ARR) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Local string array variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                sb.toString());
        sb.returnToPool();
        throw ex;
    }
        return svar.getTextArrayVal();
}
/**
 * Gets the local text value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws RPGException if no such variable was assigned
 */
public  String getLocalStringVariableValue( String name)
            throws RPGException
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_08_TEXT) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Local string variable ");
            sb.append(name);
            sb.append(" was never set.");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                sb.toString());
        sb.returnToPool();
        throw ex;
    }
        return svar.getText();
}
/**
 * Gets the length of the local variable array.
 * @return int
 */
public  int getLocalVarArrayLength()
{
    return lvar.length;
}
/**
 * Gets a local {@link Scriptable} variable.
 * @param index the index of the variable
 * @return {@link ScriptVariable}
 */
public  ScriptVariable getLocalVariable( int index)
{
    ScriptVariable svar = null;
    if (index >= 0
            && index < lvar.length)
    {
        svar = lvar[index];
    }
    return svar;
}
/**
 * Gets a local {@link Scriptable} variable.
 * @param name the name of the variable
 * @return {@link ScriptVariable}
 */
public  ScriptVariable getLocalVariable( String name)
{
    ScriptVariable svar = null;
    for (int i = lvar.length - 1; i >= 0; i--)
    {
        if (lvar[i] != null
                && lvar[i].getName() != null
                && lvar[i].getName().equalsIgnoreCase(name))
        {
            svar = lvar[i];
            break;
        }
    }
    return svar;
}
/**
 * Gets the master script.
 * @return {@link Scriptable<IO>}
 */
public  Scriptable<IO> getMaster() {
    return master;
}
public  void getTargetPos(IO io, long smoothing) throws RPGException
{
        if (io == null) {
        return;
    }

        if (io.hasIOFlag(IoGlobals.IO_03_NPC)) {
        if (io.getNPCData().hasBehavior(Behaviour.BEHAVIOUR_NONE))
        {
            io.getTarget().setX(io.getPosition().getX());
            io.getTarget().setY(io.getPosition().getY());
            io.getTarget().setZ(0);
            return;
        }
        if (io.getNPCData().hasBehavior(Behaviour.BEHAVIOUR_GO_HOME))
        {
            if (io.getNPCData().getPathfinding().getListPosition() < io
                    .getNPCData().getPathfinding().getListnb())
            {
                long pos = io.getNPCData().getPathfinding().getListItem(
                        io.getNPCData().getPathfinding().getListPosition());
                // io.getTarget().setX(ACTIVEBKG->anchors[pos].pos.x;
                // io.getTarget().setY(ACTIVEBKG->anchors[pos].pos.y;
                // io.getTarget().setZ(ACTIVEBKG->anchors[pos].pos.z;
                return;
            }
            io.getTarget().setX(io.getInitPosition().getX());
            io.getTarget().setY(io.getInitPosition().getY());
            io.getTarget().setZ(0);
            return;
        }
        if (io.hasIOFlag(IoGlobals.IO_03_NPC)
                && io.getNPCData().getPathfinding().getListnb() != -1
                && io.getNPCData().getPathfinding().hasList()
                && !io.getNPCData()
                        .hasBehavior(Behaviour.BEHAVIOUR_FRIENDLY))
        {
            // Targeting Anchors !
            if (io.getNPCData().getPathfinding().getListPosition() < io
                    .getNPCData().getPathfinding().getListnb())
            {
                long pos = io.getNPCData().getPathfinding().getListItem(
                        io.getNPCData().getPathfinding().getListPosition());
                // io.getTarget().setX(ACTIVEBKG->anchors[pos].pos.x;
                // io.getTarget().setY(ACTIVEBKG->anchors[pos].pos.y;
                // io.getTarget().setZ(ACTIVEBKG->anchors[pos].pos.z;
            }
            else if (Interactive.getInstance().hasIO(
                  io.getNPCData().getPathfinding().getTruetarget()))
            {
                IO ioo = (IO)Interactive.getInstance().getIO(
                        io.getNPCData().getPathfinding()
                                .getTruetarget());
                io.getTarget().setX(ioo.getPosition().getX());
                io.getTarget().setY(ioo.getPosition().getY());
                io.getTarget().setZ(0);
            }
            return;
        }
    }
        if (io.getTargetinfo() == ScriptConstants.TARGET_PATH) {
        // if (io->usepath == NULL)
        // {
        // io->target.x = io->pos.x;
        // io->target.y = io->pos.y;
        // io->target.z = io->pos.z;
        // return;
        // }

        // ARX_USE_PATH * aup = (ARX_USE_PATH *)io->usepath;
        // aup->_curtime += smoothing + 100;
        // EERIE_3D tp;
        // long wp = ARX_PATHS_Interpolate(aup, &tp);

        // if (wp < 0)
        // {
        // if (io->ioflags & IO_CAMERA)
        // io->_camdata->cam.lastinfovalid = FALSE;
        // }
        // else
        // {

        // io->target.x = tp.x;
        // io->target.y = tp.y;
        // io->target.z = tp.z;

        // }

        // return;
    }

        if (io.getTargetinfo() == ScriptConstants.TARGET_NONE) {
        io.getTarget().setX(io.getPosition().getX());
        io.getTarget().setY(io.getPosition().getY());
        io.getTarget().setZ(0);
        return;
    }
        if (io.getTargetinfo() == ScriptConstants.TARGET_PLAYER
                || io.getTargetinfo() == -1) {
        IO player = (IO)Interactive.getInstance().getIO(
                ProjectConstants.getInstance().getPlayer());
        io.getTarget().setX(player.getPosition().getX());
        io.getTarget().setY(player.getPosition().getY());
        io.getTarget().setZ(0);
        player = null;
        return;
    } else {
        if (Interactive.getInstance().hasIO(io.getTargetinfo()))
        {
            IO tio = (IO)Interactive.getInstance()
                    .getIO(io.getTargetinfo());
            Vector2 pos = new Vector2();
            if (Interactive.getInstance().GetItemWorldPosition(tio, pos))
            {
                io.getTarget().setX(pos.getX());
                io.getTarget().setY(pos.getY());
                io.getTarget().setZ(0);
                return;
            }
            io.getTarget().setX(tio.getPosition().getX());
            io.getTarget().setY(tio.getPosition().getY());
            io.getTarget().setZ(0);
            return;
        }
    }
    io.getTarget().setX(io.getPosition().getX());
    io.getTarget().setY(io.getPosition().getY());
    io.getTarget().setZ(0);
}
/**
 * Gets a specific script timer's reference id.
 * @param index the timer's index
 * @return {@link int}
 */
public  int getTimer( int index)
{
    return timers[index];
}
/**
 * Shorthand method to get the type variable.
 * @return {@link String}
 * @throws RPGException if an error occurs
 */
protected  String getType() throws RPGException
{
        return getLocalStringVariableValue("type");
}
/**
 * Determines if the {@link InteractiveObject} allows a specific event.
 * @param event the event flag
 * @return true if the object has the event set; false otherwise
 */
public  bool hasAllowedEvent( int event)
{
    return (allowedEvent & event) == event;
}
/**
 * Determines if a {@link ScriptObject} has local variable with a specific
 * name.
 * @param name the variable name
 * @return <tt>true</tt> if the {@link ScriptObject} has the local variable;
 *         <tt>false</tt> otherwise
 */
public  bool hasLocalVariable( String name)
{
    return getLocalVariable(name) != null;
}
/**
 * Determines if a {@link ScriptObject} has local variables assigned to it.
 * @return true if the {@link ScriptObject} has local variables; false
 *         otherwise
 */
public  bool hasLocalVariables()
{
    bool has = false;
    for (int i = lvar.length - 1; i >= 0; i--)
    {
        if (lvar[i] != null)
        {
            has = true;
            break;
        }
    }
    return has;
}
/**
 * Shorthand method to determine if the type variable matches a specific
 * type.
 * @param type the type
 * @return {@link bool}
 * @throws RPGException if an error occurs
 */
protected  bool isType( String type) throws RPGException
{
        return getLocalStringVariableValue("type").equalsIgnoreCase(type);
}
/**
 * Script run when the {@link Scriptable} is added to a party.
 * @return {@link int}
 * @throws RPGException when an error occurs
 */
public int onAddToParty() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * Script run when the {@link Scriptable} is a target of aggression.
 * @return {@link int}
 * @throws RPGException when an error occurs
 */
public int onAggression() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onAttackPlayer() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onCallHelp() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO chat start.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onChat() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onCheatDie() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onCollideDoor() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onCollideNPC() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onCollisionError() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO combine.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onCombine() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onControlsOff() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onControlsOn() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onDelation() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onDetectPlayer() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO dies.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onDie() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onDoorLocked() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO equipped.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onEquip() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onFleeEnd() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onGameReady() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onHear() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO hit.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onHit() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO attempt to identify.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onIdentify() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO initialization.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onInit() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO initialization end.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onInitEnd() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO closes inventory.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onInventoryClose() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO goes into inventory.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onInventoryIn() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO opens inventory.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onInventoryOpen() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO comes out of inventory.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onInventoryOut() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO is used inside inventory.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onInventoryUse() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onLoad() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onLookFor() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onLookMe() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO traveling on the game map.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onMovement() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO ouch.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onOuch() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onPlayerEnemy() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onReachedTarget() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onReload() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * Causes an IONpcData to
 * @return
 * @throws RPGException
 */
public int onSpeakNoRepeat() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onSpellcast() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onSteal() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO successfully strikes a target.
 * @return {@link int}
 * @throws RPGException if an error occurs
 */
public int onStrike() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onTargetDeath() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onUndetectPlayer() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO unequipped.
 * @return <code>int</code>
 * @throws RPGException if an error occurs
 */
public int onUnequip() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
/**
 * Removed an event from the list of allowed events.
 * @param event the event flag
 */
public  void removeDisallowedEvent( int event)
{
    allowedEvent = allowedEvent & ~event;
}
/**
 * Sets the IO associated with this script.
 * @param val the IO to set
 */
public  void setIO( IO val)
{
    io = val;
}
/**
 * Sets a local {@link ScriptVariable}.
 * @param index the index of the variable
 * @param svar the local {@link ScriptVariable}
 * @throws PooledException if one occurs
 * @throws RPGException if no such variable was assigned
 */
public  void setLocalVariable( int index,
         ScriptVariable svar) throws RPGException
{
        // if the index number is valid
        if (index >= 0) {
        // if the local variables array needs to be extended, do so
        if (index >= lvar.length)
        {
            ScriptVariable[] dest = new ScriptVariable[index + 1];
            System.arraycopy(lvar, 0, dest, 0, lvar.length);
            lvar = dest;
            dest = null;
        }
        lvar[index] = svar;
    } else {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Invalid array index ");
            sb.append(index);
            sb.append(".");
        }
        catch (PooledException e)
        {
            sb.returnToPool();
            sb = null;
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
}
/**
 * Sets a local {@link ScriptVariable}.
 * @param svar the local {@link ScriptVariable}
 */
public  void setLocalVariable( ScriptVariable svar)
{
    if (svar != null)
    {
        bool found = false;
        for (int i = lvar.length - 1; i >= 0; i--)
        {
            if (lvar[i] != null
                    && lvar[i].getName() != null
                    && lvar[i].getName().equalsIgnoreCase(svar.getName()))
            {
                lvar[i] = svar;
                found = true;
                break;
            }
        }
        // if the local variable was not found
        if (!found)
        {
            // find an empty index
            int i = lvar.length - 1;
            for (; i >= 0; i--)
            {
                if (lvar[i] == null)
                {
                    break;
                }
            }
            if (i >= 0)
            {
                lvar[i] = svar;
            }
            else
            {
                lvar = ArrayUtilities.getInstance().extendArray(svar, lvar);
            }
        }
    }
}
/**
 * Sets a global variable.
 * @param name the name of the global variable
 * @param value the variable's value
 * @throws RPGException if an error occurs
 */
public  void setLocalVariable( String name,  Object value)
            throws RPGException
{
    bool found = false;
        for (int i = 0, len = lvar.length; i < len; i++) {
        ScriptVariable svar = lvar[i];
        if (svar != null
                && svar.getName() != null
                && svar.getName().equalsIgnoreCase(name))
        {
            svar.set(value);
            found = true;
            break;
        }
    }
        if (!found) {
        // create a new variable and add to the global array
        ScriptVariable svar = null;
        if (value instanceof String
                    || value instanceof char[]) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_08_TEXT,
                    value);
        } else if (value instanceof String[]
                    || value instanceof char[][]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_09_TEXT_ARR, value);
        } else if (value instanceof Float) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_10_FLOAT,
                    value);
        } else if (value instanceof Double) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_10_FLOAT,
                    value);
        } else if (value instanceof float[]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_11_FLOAT_ARR, value);
        } else if (value instanceof Integer) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_12_INT,
                    value);
        } else if (value instanceof int[]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_13_INT_ARR, value);
        } else if (value instanceof Long) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_14_LONG,
                    value);
        } else if (value instanceof long[]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_15_LONG_ARR, value);
        } else {
            PooledStringBuilder sb =
                    StringBuilderPool.getInstance().getStringBuilder();
            try
            {
                sb.append("Local variable ");
                sb.append(name);
                sb.append(" was passed new value of type ");
                sb.append(value.getClass().getCanonicalName());
                sb.append(". Only String, Float, float[], Integer, int[],");
                sb.append(" Long, or long[] allowed.");
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
        lvar = ArrayUtilities.getInstance().extendArray(svar, lvar);
    }
}
/**
 * Sets the master script.
 * @param script the script to set
 */
public  void setMaster( Scriptable<IO> script)
{
    master = script;
}
public  void setTarget( TargetParameters params)
            throws RPGException
{
        if (io.hasIOFlag(IoGlobals.IO_03_NPC)) {
        io.getNPCData().getPathfinding()
                .removeFlag(ScriptConstants.PATHFIND_ALWAYS);
        io.getNPCData().getPathfinding()
                .removeFlag(ScriptConstants.PATHFIND_ONCE);
        io.getNPCData().getPathfinding()
                .removeFlag(ScriptConstants.PATHFIND_NO_UPDATE);
        if (params.hasFlag(ScriptConstants.PATHFIND_ALWAYS)) {
            io.getNPCData().getPathfinding()
                    .addFlag(ScriptConstants.PATHFIND_ALWAYS);
        }
        if (params.hasFlag(ScriptConstants.PATHFIND_ONCE)) {
            io.getNPCData().getPathfinding()
                    .addFlag(ScriptConstants.PATHFIND_ONCE);
        }
        if (params.hasFlag(ScriptConstants.PATHFIND_NO_UPDATE)) {
            io.getNPCData().getPathfinding()
                    .addFlag(ScriptConstants.PATHFIND_NO_UPDATE);
        }
        int old_target = -12;
        if (io.getNPCData().hasReachedtarget())
        {
            old_target = io.getTargetinfo();
        }
        if (io.getNPCData().hasBehavior(Behaviour.BEHAVIOUR_FLEE)
                || io.getNPCData()
                        .hasBehavior(Behaviour.BEHAVIOUR_WANDER_AROUND))
        {
            old_target = -12;
        }
        int t = params.getTargetInfo();

        if (t == -2)
        {
            t = Interactive.getInstance().GetInterNum(io);
        }
        // if (io.hasIOFlag(ioglobals.io_camera)) {
        // EERIE_CAMERA * cam = (EERIE_CAMERA *)io->_camdata;
        // cam->translatetarget.x = 0.f;
        // cam->translatetarget.y = 0.f;
        // cam->translatetarget.z = 0.f;
        // }
        if (t == ScriptConstants.TARGET_PATH)
        {
            io.setTargetinfo(t); // TARGET_PATH;
            getTargetPos(io, 0);
        }
        else if (t == ScriptConstants.TARGET_NONE)
        {
            io.setTargetinfo(ScriptConstants.TARGET_NONE);
        }
        else
        {
            if (Interactive.getInstance().hasIO(t))
            {
                io.setTargetinfo(t); // TARGET_PATH;
                getTargetPos(io, 0);
            }
        }

        if (old_target != t)
        {
            io.getNPCData().setReachedtarget(false);

            // ARX_NPC_LaunchPathfind(io, t);
        }
    }
}
/**
 * Sets the reference id of the {@link ScriptTimer} found at a specific
 * index.
 * @param index the index
 * @param refId the reference id
 */
public  void setTimer( int index,  int refId)
{
    timers[index] = refId;
}
public int onOtherReflection() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onMiscReflection() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onPathfinderFailure() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
public int onSpellEnd() throws RPGException
{
        return ScriptConstants.ACCEPT;
}
    }
}
