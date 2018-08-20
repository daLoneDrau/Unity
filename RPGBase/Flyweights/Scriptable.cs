using RPGBase.Constants;
using RPGBase.Pooled;
using RPGBase.Singletons;
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
        private Dictionary<int, IScriptAction[]> eventActions;
        /// <summary>
        /// the <see cref="BaseInteractiveObject"/> associated with this <see cref="InventoryData"/>.
        /// </summary>
        public BaseInteractiveObject Io { get; set; }
        /** the array of local <see cref="ScriptVariable"/>s. */
        private ScriptVariable[] lvar;
        /** the master script. */
        /// <summary>
        /// the master script.
        /// </summary>
        public Scriptable Master { get; set; }
        /// <summary>
        /// the list of script timers.
        /// </summary>
        private int[] timers;
        /// <summary>
        /// Shorthand method to get the type variable.
        /// </summary>
        protected string Type { get { return GetLocalStringVariableValue("type"); } }
        /// <summary>
        /// Creates a new instance of <see cref="Scriptable"/>.
        /// </summary>
        public Scriptable() : this(null) { }
        /// <summary>
        /// Creates a new instance of <see cref="Scriptable"/>.
        /// </summary>
        /// <param name="ioInstance">the BaseInteractiveObject associated with this script</param>
        public Scriptable(BaseInteractiveObject ioInstance)
        {
            timers = new int[ScriptConsts.MAX_SCRIPTTIMERS];
            lvar = new ScriptVariable[0];
            eventActions = new Dictionary<int, IScriptAction[]>();
            Io = ioInstance;
        }
        /// <summary>
        /// Adds a local variable.
        /// </summary>
        /// <param name="svar">the local variable</param>
        public void AddLocalVariable(ScriptVariable svar)
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
                lvar = ArrayUtilities.Instance.ExtendArray(svar, lvar);
            }
        }
        /// <summary>
        /// Adds a {@link IScriptAction} to the list of actions for an event.
        /// </summary>
        /// <param name="eventID">the event ID - usually the script message #</param>
        /// <param name="action">the script action</param>
        public void AddScriptAction(int eventID, IScriptAction action)
        {
            if (eventActions[eventID] == null)
            {
                eventActions[eventID] = new IScriptAction[0];
            }
            if (action != null)
            {
                action.SetScript(this);
                eventActions.Add(eventID, ArrayUtilities.Instance.ExtendArray(action, eventActions[eventID]));
            }
        }
        /// <summary>
        /// Assigns a bit flag for an allowed event.
        /// </summary>
        /// <param name="e">the event flag</param>
        public void AssignDisallowedEvent(int e)
        {
            allowedEvent |= e;
        }
        /// <summary>
        /// Changes the BaseInteractiveObject's behavior.
        /// </summary>
        /// <param name="p">the behavior parameters</param>
        public void Behavior(BehaviorParameters p)
        {
            if (Io.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                if (string.Equals("STACK", p.Action, StringComparison.OrdinalIgnoreCase))
                {
                    Io.NpcData.StackBehavior();
                }
                else if (string.Equals("UNSTACK", p.Action, StringComparison.OrdinalIgnoreCase))
                {
                    Io.NpcData.UnstackBehavior();
                }
                else if (string.Equals("UNSTACKALL", p.Action, StringComparison.OrdinalIgnoreCase))
                {
                    Io.NpcData.ResetBehavior();
                }
                else
                {
                    Io.NpcData.ChangeBehavior(p.GetFlags(), (long)p.BehaviorParam);
                    if (p.Movemode > -1)
                    {
                        Io.NpcData.Movemode = p.Movemode;
                    }
                    if (p.Tactics > -1)
                    {
                        Io.NpcData.Tactics = p.Tactics;
                    }
                    if (p.TargetInfo != -1)
                    {
                        Io.Targetinfo = p.TargetInfo;
                    }
                }
            }
        }
        /// <summary>
        /// Clears the bit flags for allowed events.
        /// </summary>
        public void ClearDisallowedEvents()
        {
            allowedEvent = 0;
        }
        /// <summary>
        /// Clears a local variable assigned to the <see cref="Scriptable"/>.
        /// </summary>
        /// <param name="varName">the variable's name</param>
        public void ClearLocalVariable(string varName)
        {
            for (int i = lvar.Length - 1; i >= 0; i--)
            {
                if (lvar[i] != null
                        && lvar[i].Name != null
                        && string.Equals(lvar[i].Name, varName, StringComparison.OrdinalIgnoreCase))
                {
                    lvar[i].Clear();
                }
                lvar[i] = null;
            }
        }
        /// <summary>
        /// Clears all local variables assigned to the <see cref="Scriptable"/>.
        /// </summary>
        public void ClearLocalVariables()
        {
            for (int i = lvar.Length - 1; i >= 0; i--)
            {
                if (lvar[i] != null)
                {
                    lvar[i].Clear();
                }
                lvar[i] = null;
            }
        }
        /**
         * Gets all event actions for a scripted event.
         * @param eventID the event ID - usually the script message #
         * @return {@link IScriptAction}[]
         */
        /// <summary>
        /// Gets all event actions for a scripted event.
        /// </summary>
        /// <param name="eventID">the event ID - usually the script message #</param>
        /// <returns><see cref="IScriptAction"/>[]</returns>
        public IScriptAction[] GetEventActions(int eventID)
        {
            IScriptAction[] actions = new IScriptAction[0];
            if (eventActions[eventID] == null)
            {
                eventActions.Add(eventID, actions);
            }
            return eventActions[eventID];
        }
        /// <summary>
        /// Gets the local floating-point array value assigned to a specific variable.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns><see cref="float"/>[]</returns>
        public float[] GetLocalFloatArrayVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_11_FLOAT_ARR)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                sb.Append("Local floating-point array type variable ");
                sb.Append(name);
                sb.Append(" was never set.");
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            return svar.Faval;
        }
        /// <summary>
        /// Gets the local floating-point value assigned to a specific variable.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns></returns>
        public float GetLocalFloatVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_10_FLOAT)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Local floating-point variable ");
                    sb.Append(name);
                    sb.Append(" was never set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return svar.Fval;
        }
        /// <summary>
        /// Gets the local integer array value assigned to a specific variable.
        /// </summary>
        /// <param name="name"> the variable name</param>
        /// <returns></returns>
        public int[] GetLocalIntArrayVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_13_INT_ARR)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                sb.Append("Local floating-point variable ");
                sb.Append(name);
                sb.Append(" was never set.");
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            return svar.Iaval;
        }
        /// <summary>
        /// Gets the local integer value assigned to a specific variable.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns></returns>
        public int GetLocalIntVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_12_INT)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Local integer variable ");
                    sb.Append(name);
                    sb.Append(" was never set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            return svar.Ival;
        }
        /// Gets the local long integer array value assigned to a specific variable.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns></returns>
        public long[] GetLocalLongArrayVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_15_LONG_ARR)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                sb.Append("Local floating-point variable ");
                sb.Append(name);
                sb.Append(" was never set.");
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                        sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            return svar.Laval;
        }
        /// Gets the local long integer value assigned to a specific variable.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns></returns>
        public long GetLocalLongVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_14_LONG)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Local long integer variable ");
                    sb.Append(name);
                    sb.Append(" was never set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            return svar.Lval;
        }
        /// Gets the local text array value assigned to a specific variable.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns></returns>
        public string[] GetLocalStringArrayVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_09_TEXT_ARR)
            {
                PooledStringBuilder sb =
                        StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Local string array variable ");
                    sb.Append(name);
                    sb.Append(" was never set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM,
                        sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            return svar.Textaval;
        }
        /// Gets the local text value assigned to a specific variable.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns></returns>
        public string GetLocalStringVariableValue(string name)
        {
            ScriptVariable svar = GetLocalVariable(name);
            if (svar == null
                    || svar.Type != ScriptConsts.TYPE_L_08_TEXT)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Local string variable ");
                    sb.Append(name);
                    sb.Append(" was never set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
            return svar.Text;
        }
        /// <summary>
        /// Gets the length of the local variable array.
        /// </summary>
        /// <returns></returns>
        public int GetLocalVarArrayLength()
        {
            return lvar.Length;
        }
        /// <summary>
        /// ets a local <see cref="Scriptable"/> variable.
        /// </summary>
        /// <param name="index">the index of the variable</param>
        /// <returns></returns>
        public ScriptVariable GetLocalVariable(int index)
        {
            ScriptVariable svar = null;
            if (index >= 0
                    && index < lvar.Length)
            {
                svar = lvar[index];
            }
            return svar;
        }
        /// <summary>
        /// Gets a local <see cref="Scriptable"/> variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public ScriptVariable GetLocalVariable(string name)
        {
            ScriptVariable svar = null;
            for (int i = lvar.Length - 1; i >= 0; i--)
            {
                if (lvar[i] != null
                        && lvar[i].Name != null
                        && string.Equals(lvar[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    svar = lvar[i];
                    break;
                }
            }
            return svar;
        }
        public void GetTargetPos(BaseInteractiveObject io, long smoothing)
        {
            if (io == null)
            {
                return;
            }

            if (io.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                if (io.NpcData.HasBehavior(Behaviour.BEHAVIOUR_NONE))
                {
                    /*
                    io.Target.setX(io.getPosition().getX());
                    io.Target.setY(io.getPosition().getY());
                    io.Target.setZ(0);
                    */
                    return;
                }
                if (io.NpcData.HasBehavior(Behaviour.BEHAVIOUR_GO_HOME))
                {
                    /*
                    if (io.NpcData.getPathfinding().getListPosition() < io
                            .NpcData.getPathfinding().getListnb())
                    {
                        long pos = io.NpcData.getPathfinding().getListItem(
                                io.NpcData.getPathfinding().getListPosition());
                        // io.Target.setX(ACTIVEBKG->anchors[pos].pos.x;
                        // io.Target.setY(ACTIVEBKG->anchors[pos].pos.y;
                        // io.Target.setZ(ACTIVEBKG->anchors[pos].pos.z;
                        return;
                    }
                    io.Target.setX(io.getInitPosition().getX());
                    io.Target.setY(io.getInitPosition().getY());
                    io.Target.setZ(0);
                    */
                    return;
                }
                /*
                if (io.HasIOFlag(IoGlobals.IO_03_NPC)
                        && io.NpcData.getPathfinding().getListnb() != -1
                        && io.NpcData.getPathfinding().hasList()
                        && !io.NpcData.HasBehavior(Behaviour.BEHAVIOUR_FRIENDLY))
                {
                    // Targeting Anchors !
                    if (io.NpcData.getPathfinding().getListPosition() < io
                            .NpcData.getPathfinding().getListnb())
                    {
                        long pos = io.NpcData.getPathfinding().getListItem(
                                io.NpcData.getPathfinding().getListPosition());
                        // io.Target.setX(ACTIVEBKG->anchors[pos].pos.x;
                        // io.Target.setY(ACTIVEBKG->anchors[pos].pos.y;
                        // io.Target.setZ(ACTIVEBKG->anchors[pos].pos.z;
                    }
                    else if (Interactive.Instance.HasIO(
                          io.NpcData.getPathfinding().getTruetarget()))
                    {
                        BaseInteractiveObject ioo = (BaseInteractiveObject)Interactive.Instance.GetIO(
                                io.NpcData.getPathfinding()
                                        .getTruetarget());
                        io.Target.setX(ioo.getPosition().getX());
                        io.Target.setY(ioo.getPosition().getY());
                        io.Target.setZ(0);
                    }
                    return;
                }
                */
            }
            if (io.Targetinfo == ScriptConsts.TARGET_PATH)
            {
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

            if (io.Targetinfo == ScriptConsts.TARGET_NONE)
            {
                /*
                io.Target.setX(io.getPosition().getX());
                io.Target.setY(io.getPosition().getY());
                io.Target.setZ(0);
                */
                return;
            }
            if (io.Targetinfo == ScriptConsts.TARGET_PLAYER
                    || io.Targetinfo == -1)
            {
                /*
                BaseInteractiveObject player = (BaseInteractiveObject)Interactive.Instance.GetIO(ProjectConstants.Instance.GetPlayer());
                io.Target.setX(player.getPosition().getX());
                io.Target.setY(player.getPosition().getY());
                io.Target.setZ(0);
                player = null;
                */
                return;
            }
            else
            {
                if (Interactive.Instance.HasIO(io.Targetinfo))
                {
                    /*
                    BaseInteractiveObject tio = (BaseInteractiveObject)Interactive.Instance.GetIO(io.Targetinfo);
                    Vector2 pos = new Vector2();
                    if (Interactive.Instance.GetItemWorldPosition(tio, pos))
                    {
                        io.Target.setX(pos.getX());
                        io.Target.setY(pos.getY());
                        io.Target.setZ(0);
                        return;
                    }
                    io.Target.setX(tio.getPosition().getX());
                    io.Target.setY(tio.getPosition().getY());
                    io.Target.setZ(0);
                    */
                    return;
                }
            }
            /*
            io.Target.setX(io.getPosition().getX());
            io.Target.setY(io.getPosition().getY());
            io.Target.setZ(0);
            */
        }
        /// <summary>
        /// Gets a specific script timer's reference id.
        /// </summary>
        /// <param name="index">the timer's index</param>
        /// <returns></returns>
        public int GetTimer(int index)
        {
            return timers[index];
        }
        /// <summary>
        /// Determines if the <see cref="Scriptable"/> allows a specific event.
        /// </summary>
        /// <param name="e">the event flag</param>
        /// <returns>true if the object has the event set; false otherwise</returns>
        public bool HasAllowedEvent(int e)
        {
            return (allowedEvent & e) == e;
        }
        /// <summary>
        /// Determines if a <see cref="Scriptable"/> has local variable with a specific name.
        /// </summary>
        /// <param name="name">he variable name</param>
        /// <returns><tt>true</tt> if the {@link ScriptObject} has the local variable; <tt>false</tt> otherwise</returns>
        public bool HasLocalVariable(string name)
        {
            return GetLocalVariable(name) != null;
        }
        /**
         * Determines if a {@link ScriptObject} has local variables assigned to it.
         * @return true if the {@link ScriptObject} has local variables; false
         *         otherwise
         */
        /// <summary>
        /// Determines if a <see cref="Scriptable"/> has local variables assigned to it.
        /// </summary>
        /// <returns>true if the <see cref="Scriptable"/> has local variables; false otherwise</returns>
        public bool HasLocalVariables()
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
        /// <summary>
        /// Shorthand method to determine if the type variable matches a specific type.
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns></returns>
        protected bool IsType(string type)
        {
            return string.Equals(GetLocalStringVariableValue("type"), type, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// Script run when the <see cref="Scriptable"/> is added to a party.
        /// </summary>
        /// <returns></returns>
        public virtual int OnAddToParty()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// Script run when the <see cref="Scriptable"/> is a target of aggression.
        /// </summary>
        /// <returns></returns>
        public virtual int OnAggression()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnAttackPlayer()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnCallHelp()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject chat start.
        /// </summary>
        /// <returns></returns>
        public virtual int OnChat()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnCheatDie()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnCollideDoor()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnCollideNPC()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnCollisionError()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject combine.
        /// </summary>
        /// <returns></returns>
        public virtual int OnCombine()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnControlsOff()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnControlsOn()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnDelation()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnDetectPlayer()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject dies.
        /// </summary>
        /// <returns></returns>
        public virtual int OnDie()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnDoorLocked()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject equipped.
        /// </summary>
        /// <returns></returns>
        public virtual int OnEquip()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnFleeEnd()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnGameReady()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnHear()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject hit.
        /// </summary>
        /// <returns></returns>
        public virtual int OnHit()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject attempt to identify.
        /// </summary>
        /// <returns></returns>
        public virtual int OnIdentify()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject initialization.
        /// </summary>
        /// <returns></returns>
        public virtual int OnInit()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject initialization end.
        /// </summary>
        /// <returns></returns>
        public virtual int OnInitEnd()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject closes inventory.
        /// </summary>
        /// <returns></returns>
        public virtual int OnInventoryClose()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject goes into inventory.
        /// </summary>
        /// <returns></returns>
        public virtual int OnInventoryIn()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject opens inventory.
        /// </summary>
        /// <returns></returns>
        public virtual int OnInventoryOpen()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject comes out of inventory.
        /// </summary>
        /// <returns></returns>
        public virtual int OnInventoryOut()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject is used inside inventory.
        /// </summary>
        /// <returns></returns>
        public virtual int OnInventoryUse()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnLoad()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnLookFor()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnLookMe()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnMiscReflection()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject traveling on the game map.
        /// </summary>
        /// <returns></returns>
        public virtual int OnMovement()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject ouch.
        /// </summary>
        /// <returns></returns>
        public virtual int OnOuch()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnOtherReflection()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnPathfinderFailure()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnSpellEnd()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnPlayerEnemy()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnReachedTarget()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnReload()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnSpeakNoRepeat()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnSpellcast()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnSteal()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject successfully strikes a target.
        /// </summary>
        /// <returns></returns>
        public virtual int OnStrike()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnTargetDeath()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnUndetectPlayer()
        {
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// On BaseInteractiveObject unequipped.
        /// </summary>
        /// <returns></returns>
        public virtual int OnUnequip()
        {
            return ScriptConsts.ACCEPT;
        }
        /**
         * Removed an event from the list of allowed events.
         * @param event the event flag
         */
        /// <summary>
        /// Removes an event from the list of allowed events.
        /// </summary>
        /// <param name="">the event flag</param>
        public void RemoveDisallowedEvent(int e)
        {
            allowedEvent = allowedEvent & ~e;
        }
        /// <summary>
        /// Sets a local <see cref="ScriptVariable"/>.
        /// </summary>
        /// <param name="index">the index of the variable</param>
        /// <param name="svar">the local <see cref="ScriptVariable"/></param>
        public void SetLocalVariable(int index, ScriptVariable svar)
        {
            // if the index number is valid
            if (index >= 0)
            {
                // if the local variables array needs to be extended, do so
                if (index >= lvar.Length)
                {
                    lvar = ArrayUtilities.Instance.ExtendArray(svar, lvar);
                }
                else
                {
                    lvar[index] = svar;
                }
            }
            else
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Invalid array index ");
                    sb.Append(index);
                    sb.Append(".");
                }
                catch (PooledException e)
                {
                    sb.ReturnToPool();
                    sb = null;
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
        }
        /// <summary>
        /// Sets a local <see cref="ScriptVariable"/>.
        /// </summary>
        /// <param name="svar">the local <see cref="ScriptVariable"/></param>
        public void SetLocalVariable(ScriptVariable svar)
        {
            if (svar != null)
            {
                bool found = false;
                for (int i = lvar.Length - 1; i >= 0; i--)
                {
                    if (lvar[i] != null
                            && lvar[i].Name != null
                            && string.Equals(lvar[i].Name, svar.Name, StringComparison.OrdinalIgnoreCase))
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
                        lvar = ArrayUtilities.Instance.ExtendArray(svar, lvar);
                    }
                }
            }
        }
        /// <summary>
        /// Sets a local variable.
        /// </summary>
        /// <param name="name">he name of the global variable</param>
        /// <param name="value">the variable's value</param>
        public void SetLocalVariable(string name, Object value)
        {
            bool found = false;
            for (int i = 0, len = lvar.Length; i < len; i++)
            {
                ScriptVariable svar = lvar[i];
                if (svar != null
                        && svar.Name != null
                        && string.Equals(svar.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    svar.Set(value);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // create a new variable and add to the global array
                ScriptVariable svar = null;
                if (value is string
                            || value is char[])
                {
                    svar = new ScriptVariable(name, ScriptConsts.TYPE_L_08_TEXT,
                            value);
                }
                else if (value is string[]
                          || value is char[][])
                {
                    svar = new ScriptVariable(name,
                            ScriptConsts.TYPE_L_09_TEXT_ARR, value);
                }
                else if (value is float)
                {
                    svar = new ScriptVariable(name, ScriptConsts.TYPE_L_10_FLOAT,
                            value);
                }
                else if (value is Double)
                {
                    svar = new ScriptVariable(name, ScriptConsts.TYPE_L_10_FLOAT,
                            value);
                }
                else if (value is float[])
                {
                    svar = new ScriptVariable(name,
                            ScriptConsts.TYPE_L_11_FLOAT_ARR, value);
                }
                else if (value is int)
                {
                    svar = new ScriptVariable(name, ScriptConsts.TYPE_L_12_INT,
                            value);
                }
                else if (value is int[])
                {
                    svar = new ScriptVariable(name,
                            ScriptConsts.TYPE_L_13_INT_ARR, value);
                }
                else if (value is long)
                {
                    svar = new ScriptVariable(name, ScriptConsts.TYPE_L_14_LONG,
                            value);
                }
                else if (value is long[])
                {
                    svar = new ScriptVariable(name,
                            ScriptConsts.TYPE_L_15_LONG_ARR, value);
                }
                else
                {
                    PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                    try
                    {
                        sb.Append("Local variable ");
                        sb.Append(name);
                        sb.Append(" was passed new value of type ");
                        sb.Append(value.GetType().Name);
                        sb.Append(". Only string, Float, float[], Integer, int[],");
                        sb.Append(" Long, or long[] allowed.");
                    }
                    catch (PooledException e)
                    {
                        throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                    }
                    RPGException ex = new RPGException(ErrorMessage.INVALID_PARAM, sb.ToString());
                    sb.ReturnToPool();
                    sb = null;
                    throw ex;
                }
                lvar = ArrayUtilities.Instance.ExtendArray(svar, lvar);
            }
        }
        public void SetTarget(TargetParameters p)
        {
            if (Io.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                /*
                Io.NpcData.getPathfinding().RemoveFlag(ScriptConsts.PATHFIND_ALWAYS);
                Io.NpcData.getPathfinding().RemoveFlag(ScriptConsts.PATHFIND_ONCE);
                Io.NpcData.getPathfinding().RemoveFlag(ScriptConsts.PATHFIND_NO_UPDATE);
                if (p.HasFlag(ScriptConsts.PATHFIND_ALWAYS))
                {
                    Io.NpcData.getPathfinding().AddFlag(ScriptConsts.PATHFIND_ALWAYS);
                }
                if (p.HasFlag(ScriptConsts.PATHFIND_ONCE))
                {
                    Io.NpcData.getPathfinding().AddFlag(ScriptConsts.PATHFIND_ONCE);
                }
                if (p.HasFlag(ScriptConsts.PATHFIND_NO_UPDATE))
                {
                    Io.NpcData.getPathfinding().AddFlag(ScriptConsts.PATHFIND_NO_UPDATE);
                }
                int old_target = -12;
                if (Io.NpcData.hasReachedtarget())
                {
                    old_target = Io.Targetinfo;
                }
                if (Io.NpcData.HasBehavior(Behaviour.BEHAVIOUR_FLEE)
                        || Io.NpcData.HasBehavior(Behaviour.BEHAVIOUR_WANDER_AROUND))
                {
                    old_target = -12;
                }
                int t = p.getTargetInfo();

                if (t == -2)
                {
                    t = Interactive.Instance.GetInterNum(io);
                }
                // if (io.HasIOFlag(ioglobals.io_camera)) {
                // EERIE_CAMERA * cam = (EERIE_CAMERA *)io->_camdata;
                // cam->translatetarget.x = 0.f;
                // cam->translatetarget.y = 0.f;
                // cam->translatetarget.z = 0.f;
                // }
                if (t == ScriptConsts.TARGET_PATH)
                {
                    Io.setTargetinfo(t); // TARGET_PATH;
                    getTargetPos(io, 0);
                }
                else if (t == ScriptConsts.TARGET_NONE)
                {
                    Io.setTargetinfo(ScriptConsts.TARGET_NONE);
                }
                else
                {
                    if (Interactive.Instance.HasIO(t))
                    {
                        Io.setTargetinfo(t); // TARGET_PATH;
                        getTargetPos(io, 0);
                    }
                }

                if (old_target != t)
                {
                    Io.NpcData.setReachedtarget(false);

                    // ARX_NPC_LaunchPathfind(io, t);
                }
                */
            }
        }
        /// <summary>
        /// Sets the reference id of the <see cref="ScriptTimer"/> found at a specific index.
        /// </summary>
        /// <param name="index">the index</param>
        /// <param name="refId">the reference id</param>
        public void SetTimer(int index, int refId)
        {
            timers[index] = refId;
        }
    }
}
