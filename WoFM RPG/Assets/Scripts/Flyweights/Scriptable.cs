using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public class Scriptable
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
            for (int i = 0; i < lvar.Length; i++)
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
                lvar = ArrayUtilities.GetInstance().extendArray(svar, lvar);
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
                eventActions.put(eventID, ArrayUtilities.GetInstance().extendArray(
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
            if (io.HasIOFlag(IoGlobals.IO_03_NPC))
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
            for (int i = lvar.Length - 1; i >= 0; i--)
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
            for (int i = lvar.Length - 1; i >= 0; i--)
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
         * @ if no such variable was assigned
         */
        public  float[] getLocalFloatArrayVariableValue( String name)
             {
        ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_11_FLOAT_ARR) {
            PooledStringBuilder sb =
                    StringBuilderPool.GetInstance().GetStringBuilder();
        sb.append("Local floating-point array type variable ");
            sb.append(name);
            sb.append(" was never set.");
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.toString());
        sb.ReturnToPool();
            throw ex;
        }
        return svar.getFloatArrayVal();
    }
/**
 * Gets the local floating-point value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws PooledException if one occurs
 * @ if no such variable was assigned
 */
public  float getLocalFloatVariableValue( String name)
            
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_10_FLOAT) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        sb.ReturnToPool();
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
 * @ if no such variable was assigned
 */
public  int[] getLocalIntArrayVariableValue( String name)
             {
        ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_13_INT_ARR) {
            PooledStringBuilder sb =
                    StringBuilderPool.GetInstance().GetStringBuilder();
sb.append("Local floating-point variable ");
            sb.append(name);
            sb.append(" was never set.");
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.toString());
sb.ReturnToPool();
            throw ex;
        }
        return svar.getIntArrayVal();
    }
    /**
     * Gets the local integer value assigned to a specific variable.
     * @param name the variable name
     * @return {@link String}
     * @ if no such variable was assigned
     */
    public  int getLocalIntVariableValue( String name)
            
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_12_INT) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        sb.ReturnToPool();
        throw ex;
    }
        return svar.getIntVal();
}
/**
 * Gets the local long integer value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @throws PooledException if one occurs
 * @ if no such variable was assigned
 */
public  long[] getLocalLongArrayVariableValue( String name)
             {
        ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_15_LONG_ARR) {
            PooledStringBuilder sb =
                    StringBuilderPool.GetInstance().GetStringBuilder();
sb.append("Local floating-point variable ");
            sb.append(name);
            sb.append(" was never set.");
            RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                    sb.toString());
sb.ReturnToPool();
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
            
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_14_LONG) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        sb.ReturnToPool();
        throw ex;
    }
        return svar.getLongVal();
}
/**
 * Gets the local text array value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @ if no such variable was assigned
 */
public  String[] getLocalStringArrayVariableValue( String name)
            
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_09_TEXT_ARR) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        sb.ReturnToPool();
        throw ex;
    }
        return svar.getTextArrayVal();
}
/**
 * Gets the local text value assigned to a specific variable.
 * @param name the variable name
 * @return {@link String}
 * @ if no such variable was assigned
 */
public  String getLocalStringVariableValue( String name)
            
{
    ScriptVariable svar = getLocalVariable(name);
        if (svar == null
                || svar.getType() != ScriptConstants.TYPE_L_08_TEXT) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
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
        sb.ReturnToPool();
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
    return lvar.Length;
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
            && index < lvar.Length)
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
    for (int i = lvar.Length - 1; i >= 0; i--)
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
public  void getTargetPos(IO io, long smoothing) 
{
        if (io == null) {
        return;
    }

        if (io.HasIOFlag(IoGlobals.IO_03_NPC)) {
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
        if (io.HasIOFlag(IoGlobals.IO_03_NPC)
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
            else if (Interactive.GetInstance().hasIO(
                  io.getNPCData().getPathfinding().getTruetarget()))
            {
                IO ioo = (IO)Interactive.GetInstance().getIO(
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
        IO player = (IO)Interactive.GetInstance().getIO(
                ProjectConstants.GetInstance().getPlayer());
        io.getTarget().setX(player.getPosition().getX());
        io.getTarget().setY(player.getPosition().getY());
        io.getTarget().setZ(0);
        player = null;
        return;
    } else {
        if (Interactive.GetInstance().hasIO(io.getTargetinfo()))
        {
            IO tio = (IO)Interactive.GetInstance()
                    .getIO(io.getTargetinfo());
            Vector2 pos = new Vector2();
            if (Interactive.GetInstance().GetItemWorldPosition(tio, pos))
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
 * @ if an error occurs
 */
protected  String getType() 
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
    for (int i = lvar.Length - 1; i >= 0; i--)
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
 * @ if an error occurs
 */
protected  bool isType( String type) 
{
        return getLocalStringVariableValue("type").equalsIgnoreCase(type);
}
/**
 * Script run when the {@link Scriptable} is added to a party.
 * @return {@link int}
 * @ when an error occurs
 */
public int onAddToParty() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * Script run when the {@link Scriptable} is a target of aggression.
 * @return {@link int}
 * @ when an error occurs
 */
public int onAggression() 
{
        return ScriptConstants.ACCEPT;
}
public int onAttackPlayer() 
{
        return ScriptConstants.ACCEPT;
}
public int onCallHelp() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO chat start.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onChat() 
{
        return ScriptConstants.ACCEPT;
}
public int onCheatDie() 
{
        return ScriptConstants.ACCEPT;
}
public int onCollideDoor() 
{
        return ScriptConstants.ACCEPT;
}
public int onCollideNPC() 
{
        return ScriptConstants.ACCEPT;
}
public int onCollisionError() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO combine.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onCombine() 
{
        return ScriptConstants.ACCEPT;
}
public int onControlsOff() 
{
        return ScriptConstants.ACCEPT;
}
public int onControlsOn() 
{
        return ScriptConstants.ACCEPT;
}
public int onDelation() 
{
        return ScriptConstants.ACCEPT;
}
public int onDetectPlayer() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO dies.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onDie() 
{
        return ScriptConstants.ACCEPT;
}
public int onDoorLocked() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO equipped.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onEquip() 
{
        return ScriptConstants.ACCEPT;
}
public int onFleeEnd() 
{
        return ScriptConstants.ACCEPT;
}
public int onGameReady() 
{
        return ScriptConstants.ACCEPT;
}
public int onHear() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO hit.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onHit() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO attempt to identify.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onIdentify() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO initialization.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onInit() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO initialization end.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onInitEnd() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO closes inventory.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onInventoryClose() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO goes into inventory.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onInventoryIn() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO opens inventory.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onInventoryOpen() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO comes out of inventory.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onInventoryOut() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO is used inside inventory.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onInventoryUse() 
{
        return ScriptConstants.ACCEPT;
}
public int onLoad() 
{
        return ScriptConstants.ACCEPT;
}
public int onLookFor() 
{
        return ScriptConstants.ACCEPT;
}
public int onLookMe() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO traveling on the game map.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onMovement() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO ouch.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onOuch() 
{
        return ScriptConstants.ACCEPT;
}
public int onPlayerEnemy() 
{
        return ScriptConstants.ACCEPT;
}
public int onReachedTarget() 
{
        return ScriptConstants.ACCEPT;
}
public int onReload() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * Causes an IONpcData to
 * @return
 * @
 */
public int onSpeakNoRepeat() 
{
        return ScriptConstants.ACCEPT;
}
public int onSpellcast() 
{
        return ScriptConstants.ACCEPT;
}
public int onSteal() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO successfully strikes a target.
 * @return {@link int}
 * @ if an error occurs
 */
public int onStrike() 
{
        return ScriptConstants.ACCEPT;
}
public int onTargetDeath() 
{
        return ScriptConstants.ACCEPT;
}
public int onUndetectPlayer() 
{
        return ScriptConstants.ACCEPT;
}
/**
 * On IO unequipped.
 * @return <code>int</code>
 * @ if an error occurs
 */
public int onUnequip() 
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
 * @ if no such variable was assigned
 */
public  void setLocalVariable( int index,
         ScriptVariable svar) 
{
        // if the index number is valid
        if (index >= 0) {
        // if the local variables array needs to be extended, do so
        if (index >= lvar.Length)
        {
            ScriptVariable[] dest = new ScriptVariable[index + 1];
            System.arraycopy(lvar, 0, dest, 0, lvar.Length);
            lvar = dest;
            dest = null;
        }
        lvar[index] = svar;
    } else {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.append("Invalid array index ");
            sb.append(index);
            sb.append(".");
        }
        catch (PooledException e)
        {
            sb.ReturnToPool();
            sb = null;
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                sb.toString());
        sb.ReturnToPool();
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
        for (int i = lvar.Length - 1; i >= 0; i--)
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
            int i = lvar.Length - 1;
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
                lvar = ArrayUtilities.GetInstance().extendArray(svar, lvar);
            }
        }
    }
}
/**
 * Sets a global variable.
 * @param name the name of the global variable
 * @param value the variable's value
 * @ if an error occurs
 */
public  void setLocalVariable( String name,  Object value)
            
{
    bool found = false;
        for (int i = 0, len = lvar.Length; i < len; i++) {
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
        if (value is String
                    || value is char[]) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_08_TEXT,
                    value);
        } else if (value is String[]
                    || value is char[][]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_09_TEXT_ARR, value);
        } else if (value is Float) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_10_FLOAT,
                    value);
        } else if (value is Double) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_10_FLOAT,
                    value);
        } else if (value is float[]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_11_FLOAT_ARR, value);
        } else if (value is Integer) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_12_INT,
                    value);
        } else if (value is int[]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_13_INT_ARR, value);
        } else if (value is Long) {
            svar = new ScriptVariable(name, ScriptConstants.TYPE_L_14_LONG,
                    value);
        } else if (value is long[]) {
            svar = new ScriptVariable(name,
                    ScriptConstants.TYPE_L_15_LONG_ARR, value);
        } else {
            PooledStringBuilder sb =
                    StringBuilderPool.GetInstance().GetStringBuilder();
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
            sb.ReturnToPool();
            sb = null;
            throw ex;
        }
        lvar = ArrayUtilities.GetInstance().extendArray(svar, lvar);
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
            
{
        if (io.HasIOFlag(IoGlobals.IO_03_NPC)) {
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
            t = Interactive.GetInstance().GetInterNum(io);
        }
        // if (io.HasIOFlag(ioglobals.io_camera)) {
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
            if (Interactive.GetInstance().hasIO(t))
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
public int onOtherReflection() 
{
        return ScriptConstants.ACCEPT;
}
public int onMiscReflection() 
{
        return ScriptConstants.ACCEPT;
}
public int onPathfinderFailure() 
{
        return ScriptConstants.ACCEPT;
}
public int onSpellEnd() 
{
        return ScriptConstants.ACCEPT;
}
    }
}
