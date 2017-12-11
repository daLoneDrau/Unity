using System;
using RPGBase.Flyweights;
using RPGBase.Constants;
using RPGBase.Pooled;
using System.Reflection;

namespace RPGBase.Singletons
{
    public abstract class Script
    {
        private static int ANIM_TALK_ANGRY = 0;
        private static int ANIM_TALK_HAPPY = 0;
        private static int ANIM_TALK_NEUTRAL = 0;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static Script Instance { get; protected set; }
        /// <summary>
        /// the maximum number of system parameters.
        /// </summary>
        public static int MAX_SYSTEM_PARAMS = 5;
        /// <summary>
        /// the list of system parameters.
        /// </summary>
        private static string[] SYSTEM_PARAMS = new string[MAX_SYSTEM_PARAMS];
        private bool ARXPausedTime;
        /// <summary>
        /// the flag indicating whether debug output is turned on.
        /// </summary>
        public bool Debug { get; set; }
        private bool EDITMODE;
        public BaseInteractiveObject EventSender { get; set; }
        private int eventTotalCount;
        private int GLOB;
        /** the list of global script variables. */
        private ScriptVariable[] gvars;
        /** the maximum number of timer scripts. */
        public int MaxTimerScript { get; protected set; } = 0;
        private bool PauseScript;
        private int stackFlow = 8;
        /// <summary>
        /// Adds an BaseInteractiveObject to a specific group.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="group">the group name</param>
        public void AddToGroup(BaseInteractiveObject io, string group)
        {
            if (io != null
                    && group != null)
            {
                io.AddGroup(group);
            }
        }
        public void AllowInterScriptExecution()
        {
            int ppos = 0;

            if (!PauseScript && !EDITMODE && !ARXPausedTime)
            {
                EventSender = null;

                int numm = Math.Min(Interactive.Instance.GetMaxIORefId(), 10);

                for (int n = 0; n < numm; n++)
                {
                    int i = ppos;
                    ppos++;

                    if (ppos >= Interactive.Instance.GetMaxIORefId())
                    {
                        ppos = 0;
                        break;
                    }
                    if (Interactive.Instance.hasIO(i))
                    {
                        BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                        if (io.HasGameFlag(IoGlobals.GFLAG_ISINTREATZONE))
                        {
                            if (io.Mainevent != null)
                            {
                                SendIOScriptEvent(io, 0, null, io.Mainevent);
                            }
                            else
                            {
                                SendIOScriptEvent(io, ScriptConsts.SM_008_MAIN, null, null);
                            }
                        }
                    }
                }
            }
        }
        protected abstract void ClearAdditionalEventStacks();
        protected abstract void ClearAdditionalEventStacksForIO(BaseInteractiveObject io);
        /// <summary>
        /// Clones all local variables from the source <see cref="BaseInteractiveObject"/> to the destination <see cref="BaseInteractiveObject"/>.
        /// </summary>
        /// <param name="src">the source <see cref="BaseInteractiveObject"/></param>
        /// <param name="dest">the destination <see cref="BaseInteractiveObject"/></param>
        public void CloneLocalVars(BaseInteractiveObject src, BaseInteractiveObject dest)

        {
            if (dest != null
                    && src != null)
            {
                FreeAllLocalVariables(dest);
                if (src.Script.HasLocalVariables())
                {
                    int i = src.Script.GetLocalVarArrayLength() - 1;
                    for (; i >= 0; i--)
                    {
                        dest.Script.SetLocalVariable(new ScriptVariable(src.Script.GetLocalVariable(i)));
                    }
                }
            }
        }
        /// <summary>
        /// Count the number of active script timers.
        /// </summary>
        /// <returns></returns>
        public int CountTimers()
        {
            int activeTimers = 0;
            ScriptTimer[] scriptTimers = GetScriptTimers();
            for (int i = scriptTimers.Length - 1; i >= 0; i--)
            {
                ScriptTimer timer = scriptTimers[i];
                if (scriptTimers[i].Exists)
                {
                    activeTimers++;
                }
            }
            return activeTimers;
        }
        protected abstract void DestroyScriptTimers();
        /// <summary>
        /// Checks to see if a scripted event is disallowed.
        /// </summary>
        /// <param name="msg">the event message id</param>
        /// <param name="script">the <see cref="Scriptable"/> script</param>
        /// <returns><tt>true</tt> if the event is not allowed; <tt>false</tt> otherwise</returns>
        private bool EventIsDisallowed(int msg, Scriptable script)
        {
            bool disallowed = false;
            // check to see if message is for an event that was disabled
            switch (msg)
            {
                case ScriptConsts.SM_055_COLLIDE_NPC:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_COLLIDE_NPC))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_010_CHAT:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_CHAT))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_016_HIT:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_HIT))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_028_INVENTORY2_OPEN:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_INVENTORY2_OPEN))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_046_HEAR:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_HEAR))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_023_UNDETECTPLAYER:
                case ScriptConsts.SM_022_DETECTPLAYER:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_DETECT))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_057_AGGRESSION:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_AGGRESSION))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_008_MAIN:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_MAIN))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_073_CURSORMODE:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_CURSORMODE))
                    {
                        disallowed = true;
                    }
                    break;
                case ScriptConsts.SM_074_EXPLORATIONMODE:
                    if (script.HasAllowedEvent(ScriptConsts.DISABLE_EXPLORATIONMODE))
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
        public void EventStackClear()
        {
            for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
            {
                StackedEvent e = GetStackedEvent(i);
                if (e.Exists)
                {
                    e.Parameters = null;
                    e.Eventname = null;
                    e.Sender = null;
                    e.Exists = false;
                    e.Io = null;
                    e.Msg = 0;
                }
            }
            ClearAdditionalEventStacks();
        }
        public void EventStackClearForIo(BaseInteractiveObject io)
        {
            for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
            {
                StackedEvent e = GetStackedEvent(i);
                if (e.Exists
                        && io.Equals(e.Io))
                {
                    e.Parameters = null;
                    e.Eventname = null;
                    e.Sender = null;
                    e.Exists = false;
                    e.Io = null;
                    e.Msg = 0;
                }
            }
            ClearAdditionalEventStacksForIO(io);
        }
        public void EventStackExecute()
        {
            int count = 0;
            for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
            {
                StackedEvent e = GetStackedEvent(i);
                if (e.Exists)
                {
                    int ioid = e.Io.GetRefId();
                    if (Interactive.Instance.hasIO(ioid))
                    {
                        if (e.Sender != null)
                        {
                            int senderid = e.Sender.GetRefId();
                            if (Interactive.Instance.hasIO(senderid))
                            {
                                EventSender = e.Sender;
                            }
                            else
                            {
                                EventSender = null;
                            }
                        }
                        else
                        {
                            EventSender = null;
                        }
                        SendIOScriptEvent(e.Io,
                                e.Msg,
                                e.Parameters,
                                e.Eventname);
                    }
                    e.Parameters = null;
                    e.Eventname = null;
                    e.Sender = null;
                    e.Exists = false;
                    e.Io = null;
                    e.Msg = 0;
                    count++;
                    if (count >= stackFlow)
                    {
                        break;
                    }
                }
            }
            ExecuteAdditionalStacks();
        }
        public void EventStackExecuteAll()
        {
            stackFlow = 9999999;
            EventStackExecute();
            stackFlow = 20;
        }
        public abstract void EventStackInit();
        protected abstract void ExecuteAdditionalStacks();
        public void ForceDeath(BaseInteractiveObject io, string target)

        {
            int tioid = -1;
            if (string.Equals(target, "me", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(target, "self", StringComparison.OrdinalIgnoreCase))
            {
                tioid = Interactive.Instance.GetInterNum(io);
            }
            else
            {
                tioid = Interactive.Instance.getTargetByNameTarget(target);
                if (tioid == -2)
                {
                    tioid = Interactive.Instance.GetInterNum(io);
                }
            }
            if (tioid >= 0)
            {
                BaseInteractiveObject tio = (BaseInteractiveObject)Interactive.Instance.getIO(tioid);
                if (tio.HasIOFlag(IoGlobals.IO_03_NPC))
                {
                    tio.NpcData.ForceDeath(io);
                }
            }
        }
        public void FreeAllGlobalVariables()
        {
            if (gvars != null)
            {
                for (int i = gvars.Length - 1; i >= 0; i--)
                {
                    if (gvars[i] != null
                            && (gvars[i].Type == ScriptConsts.TYPE_G_00_TEXT
                                    || gvars[i].Type == ScriptConsts.TYPE_L_08_TEXT)
                            && gvars[i].Text != null)
                    {
                        gvars[i].Set(null);
                    }
                    gvars[i] = null;
                }
            }
        }
        /// <summary>
        /// Removes all local variables from an <see cref="BaseInteractiveObject"/> and frees up the memory.
        /// </summary>
        /// <param name="io">the <see cref="BaseInteractiveObject"/></param>
        public void FreeAllLocalVariables(BaseInteractiveObject io)
        {
            if (io != null
                    && io.Script != null
                    && io.Script.HasLocalVariables())
            {
                int i = io.Script.GetLocalVarArrayLength() - 1;
                for (; i >= 0; i--)
                {
                    if (io.Script.GetLocalVariable(i) != null
                            && (io.Script.GetLocalVariable(i).Type == ScriptConsts.TYPE_G_00_TEXT
                                    || io.Script.GetLocalVariable(i).Type == ScriptConsts.TYPE_L_08_TEXT)
                            && io.Script.GetLocalVariable(i).Text != null)
                    {
                        io.Script.GetLocalVariable(i).Set(null);
                    }
                    io.Script.SetLocalVariable(i, null);
                }
            }
        }
        /// <summary>
        /// Gets the value of a global floating-point array variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public float[] GetGlobalFloatArrayVariableValue(string name)

        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_03_FLOAT_ARR)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global Float Array variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Faval;
        }
        /// <summary>
        /// Gets the value of a global floating-point variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public float GetGlobalFloatVariableValue(string name)
        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_02_FLOAT)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global Float variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Fval;
        }
        /// <summary>
        /// Gets the value of a global integer array variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public int[] GetGlobalIntArrayVariableValue(string name)
        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_05_INT_ARR)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global Integer Array variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Iaval;
        }
        /// <summary>
        /// Gets the value of a global integer variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public int GetGlobalIntVariableValue(string name)
        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_04_INT)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global Integer variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Ival;
        }
        /// <summary>
        /// Gets the value of a global long integer array variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public long[] GetGlobalLongArrayVariableValue(string name)
        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_07_LONG_ARR)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global Long Integer Array variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Laval;
        }
        /// <summary>
        /// Gets the value of a global long integer variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public long GetGlobalLongVariableValue(string name)
        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_06_LONG)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global Long Integer variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Lval;
        }
        /// <summary>
        /// Gets the value of a global text array variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public string[] GetGlobalStringArrayVariableValue(string name)
        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_01_TEXT_ARR)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global Text Array variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Textaval;
        }
        /// <summary>
        /// Gets the value of a global text variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public string getGlobalStringVariableValue(string name)
        {
            if (gvars == null)
            {
                gvars = new ScriptVariable[0];
            }
            int index = -1;
            for (int i = 0; i < gvars.Length; i++)
            {
                if (gvars[i] != null
                        && gvars[i].Name.Equals(name)
                        && gvars[i].Type == ScriptConsts.TYPE_G_00_TEXT)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                try
                {
                    sb.Append("Global string variable ");
                    sb.Append(name);
                    sb.Append(" was never Set.");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.INVALID_OPERATION, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
            return gvars[index].Text;
        }
        public int GetGlobalTargetParam(BaseInteractiveObject io)
        {
            return io.Targetinfo;
        }
        /// <summary>
        /// ets a global <see cref="Scriptable"/> variable.
        /// </summary>
        /// <param name="name">the name of the variable</param>
        /// <returns></returns>
        public ScriptVariable GetGlobalVariable(string name)
        {
            ScriptVariable var = null;
            for (int i = gvars.Length - 1; i >= 0; i--)
            {
                if (gvars[i] != null
                        && gvars[i].Name != null
                        && string.Equals(gvars[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    var = gvars[i];
                    break;
                }
            }
            return var;
        }
        public BaseInteractiveObject GetIOMaxEvents()
        {
            int max = -1;
            int ionum = -1;
            BaseInteractiveObject io = null;
            int i = Interactive.Instance.GetMaxIORefId();
            for (; i >= 0; i--)
            {
                if (Interactive.Instance.hasIO(i))
                {
                    BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                    if (hio.StatCount > max)
                    {
                        ionum = i;
                        max = hio.StatCount;
                    }
                    hio = null;
                }
            }
            if (max > 0
                    && ionum > -1)
            {
                io = (BaseInteractiveObject)Interactive.Instance.getIO(ionum);
            }
            return io;
        }
        public BaseInteractiveObject GetIOMaxEventsSent()
        {
            int max = -1;
            int ionum = -1;
            BaseInteractiveObject io = null;
            int i = Interactive.Instance.GetMaxIORefId();
            for (; i >= 0; i--)
            {
                if (Interactive.Instance.hasIO(i))
                {
                    BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                    if (hio.StatSent > max)
                    {
                        ionum = i;
                        max = hio.StatSent;
                    }
                }
            }
            if (max > 0
                    && ionum > -1)
            {
                io = (BaseInteractiveObject)Interactive.Instance.getIO(ionum);
            }
            return io;
        }
        /// <summary>
        /// Gets the maximum number of timer scripts.
        /// </summary>
        /// <returns></returns>
        public int GetMaxTimerScript()
        {
            return MaxTimerScript;
        }
        /// <summary>
        /// Gets a script timer.
        /// </summary>
        /// <param name="id">the timer's id</param>
        /// <returns></returns>
        public abstract ScriptTimer getScriptTimer(int id);
        /// <summary>
        /// Gets the script timers.
        /// </summary>
        /// <returns></returns>
        protected abstract ScriptTimer[] GetScriptTimers();
        /// <summary>
        /// Gets the stacked event at a specific index.
        /// </summary>
        /// <param name="index">the index</param>
        /// <returns></returns>
        protected abstract StackedEvent GetStackedEvent(int index);
        /// <summary>
        /// Gets the id of a named script assigned to a specific BaseInteractiveObject.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="name">the script's name</param>
        /// <returns></returns>
        public int GetSystemIOScript(BaseInteractiveObject io, string name)
        {
            int index = -1;
            if (CountTimers() > 0)
            {
                for (int i = 0; i < MaxTimerScript; i++)
                {
                    ScriptTimer[] scriptTimers = GetScriptTimers();
                    if (scriptTimers[i].Exists)
                    {
                        if (scriptTimers[i].Io.Equals(io)
                                && string.Equals(scriptTimers[i].Name, name, StringComparison.OrdinalIgnoreCase))
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// Determines if a {@link Script} has local variable with a specific name.
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <returns></returns>
        public bool GasGlobalVariable(string name)
        {
            return GetGlobalVariable(name) != null;
        }
        public void InitEventStats()
        {
            eventTotalCount = 0;
            int i = Interactive.Instance.GetMaxIORefId();
            for (; i >= 0; i--)
            {
                if (Interactive.Instance.hasIO(i))
                {
                    BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                    io.StatCount = 0;
                    io.StatSent = 0;
                }
            }
        }
        protected abstract void InitScriptTimers();
        /**
         * Determines if an BaseInteractiveObject is in a specific group.
         * @param io the BaseInteractiveObject
         * @param group the group name
         * @return true if the BaseInteractiveObject is in the group; false otherwise
         */
        public bool IsIOInGroup(BaseInteractiveObject io, string group)
        {
            bool val = false;
            if (io != null
                    && group != null)
            {
                for (int i = 0; i < io.GetNumIOGroups(); i++)
                {
                    if (string.Equals(group, io.GetIOGroup(i), StringComparison.OrdinalIgnoreCase))
                    {
                        val = true;
                        break;
                    }
                }
            }
            return val;
        }
        public bool IsPlayerInvisible(BaseInteractiveObject io)
        {
            bool invisible = false;
            // if (inter.iobj[0]->invisibility > 0.3f) {
            // invisible = true;
            // }
            return invisible;
        }
        private void MakeSSEPARAMS(string parameters)
        {
            for (int i = MAX_SYSTEM_PARAMS - 1; i >= 0; i--)
            {
                SYSTEM_PARAMS[i] = null;
            }
            if (parameters != null)
            {
                string[] split = parameters.Split(' ');
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
        /// <summary>
        /// Sends an event message to the BaseInteractiveObject.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="msg">the message</param>
        /// <param name="parameters">the script parameters</param>
        /// <returns></returns>
        public int NotifyIOEvent(BaseInteractiveObject io, int msg, string parameters)
        {
            int acceptance = ScriptConsts.REFUSE;
            if (SendIOScriptEvent(io, msg, null, null) != acceptance)
            {
                switch (msg)
                {
                    case ScriptConsts.SM_017_DIE:
                        if (io != null && Interactive.Instance.hasIO(io))
                        {
                            // TODO - Set death color
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
        /// <summary>
        /// Hides a target BaseInteractiveObject.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject sending the event.</param>
        /// <param name="megahide">if true, the target BaseInteractiveObject is "mega-hidden"</param>
        /// <param name="targetName">the target's name</param>
        /// <param name="hideOn">if true, the hidden flags are Set; otherwise all hidden flags are removed</param>
        public void ObjectHide(BaseInteractiveObject io, bool megahide, string targetName, bool hideOn)
        {
            int targetId = Interactive.Instance.getTargetByNameTarget(targetName);
            if (targetId == -2)
            {
                targetId = io.GetRefId();
            }
            if (Interactive.Instance.hasIO(targetId))
            {
                BaseInteractiveObject tio = (BaseInteractiveObject)Interactive.Instance.getIO(targetId);
                tio.RemoveGameFlag(IoGlobals.GFLAG_MEGAHIDE);
                if (hideOn)
                {
                    if (megahide)
                    {
                        tio.AddGameFlag(IoGlobals.GFLAG_MEGAHIDE);
                        tio.Show = IoGlobals.SHOW_FLAG_MEGAHIDE;
                    }
                    else
                    {
                        tio.Show = IoGlobals.SHOW_FLAG_HIDDEN;
                    }
                }
                else if (tio.Show == IoGlobals.SHOW_FLAG_MEGAHIDE
                      || tio.Show == IoGlobals.SHOW_FLAG_HIDDEN)
                {
                    tio.Show = IoGlobals.SHOW_FLAG_IN_SCENE;
                    if (tio.HasIOFlag(IoGlobals.IO_03_NPC)
                            && tio.NpcData.GetBaseLife() <= 0f)
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
        /// <summary>
        /// Removes an BaseInteractiveObject from all groups to which it was assigned.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        public void ReleaseAllGroups(BaseInteractiveObject io)
        {
            while (io != null
                    && io.GetNumIOGroups() > 0)
            {
                io.RemoveGroup(io.GetIOGroup(0));
            }
        }
        /// <summary>
        /// Releases a script, clearing all variables.
        /// </summary>
        /// <param name="script">the scriptable event</param>
        public void ReleaseScript(Scriptable script)
        {
            if (script != null)
            {
                script.ClearLocalVariables();
            }
        }
        /// <summary>
        /// Removes a BaseInteractiveObject from a group.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="group">the group</param>
        public void RemoveGroup(BaseInteractiveObject io, string group)
        {
            if (io != null
                    && group != null)
            {
                io.RemoveGroup(group);
            }
        }
        /// <summary>
        /// Resets the object's script.
        /// </summary>
        /// <param name="io">the object</param>
        /// <param name="initialize">if <tt>true</tt> and the object is script-loaded, it will be initialized again</param>
        public void Reset(BaseInteractiveObject io, bool initialize)

        {
            // Release Script Local Variables
            if (io.Script.GetLocalVarArrayLength() > 0)
            {
                int i = io.Script.GetLocalVarArrayLength() - 1;
                for (; i >= 0; i--)
                {
                    if (io.Script.GetLocalVariable(i) != null)
                    {
                        io.Script.GetLocalVariable(i).Set(null);
                        io.Script.SetLocalVariable(i, null);
                    }
                }
            }

            // Release Script Over-Script Local Variables
            if (io.Overscript.GetLocalVarArrayLength() > 0)
            {
                int i = io.Overscript.GetLocalVarArrayLength() - 1;
                for (; i >= 0; i--)
                {
                    if (io.Overscript.GetLocalVariable(i) != null)
                    {
                        io.Overscript.GetLocalVariable(i).Set(null);
                        io.Overscript.SetLocalVariable(i, null);
                    }
                }
            }
            if (!io.ScriptLoaded)
            {
                ResetObject(io, initialize);
            }
        }
        /// <summary>
        /// Resets all interactive objects.
        /// </summary>
        /// <param name="initialize">if <tt>true</tt> and an object is script-loaded, it will be initialized again</param>
        public void ResetAll(bool initialize)
        {
            int i = Interactive.Instance.GetMaxIORefId();
            for (; i >= 0; i--)
            {
                if (Interactive.Instance.hasIO(i))
                {
                    BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                    if (!io.ScriptLoaded)
                    {
                        ResetObject(io, initialize);
                    }
                }
            }
        }
        /// <summary>
        /// Resets the BaseInteractiveObject.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="initialize">if <tt>true</tt>, the object needs to be initialized as well</param>
        public void ResetObject(BaseInteractiveObject io, bool initialize)
        {
            // Now go for Script INIT/RESET depending on Mode
            int num = Interactive.Instance.GetInterNum(io);
            if (Interactive.Instance.hasIO(num))
            {
                BaseInteractiveObject objIO = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                if (objIO != null
                        && objIO.Script != null)
                {
                    objIO.Script.ClearDisallowedEvents();

                    if (initialize)
                    {
                        SendScriptEvent((Scriptable)objIO.Script,
                                ScriptConsts.SM_001_INIT,
                                new Object[0],
                                objIO,
                                null);
                    }
                    objIO = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                    if (objIO != null)
                    {
                        SetMainEvent(objIO, "MAIN");
                    }
                }

                // Do the same for Local Script
                objIO = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                if (objIO != null
                        && objIO.Overscript != null)
                {
                    objIO.Overscript.ClearDisallowedEvents();

                    if (initialize)
                    {
                        SendScriptEvent((Scriptable)objIO.Overscript,
                                ScriptConsts.SM_001_INIT,
                                new Object[0],
                                objIO,
                                null);
                    }
                }

                // Sends InitEnd Event
                if (initialize)
                {
                    objIO = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                    if (objIO != null
                            && objIO.Script != null)
                    {
                        SendScriptEvent((Scriptable)objIO.Script,
                                ScriptConsts.SM_033_INITEND,
                                new Object[0],
                                objIO,
                                null);
                    }
                    objIO = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                    if (objIO != null
                            && objIO.Overscript != null)
                    {
                        SendScriptEvent((Scriptable)objIO.Overscript,
                                ScriptConsts.SM_033_INITEND,
                                new Object[0],
                                objIO,
                                null);
                    }
                }

                objIO = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                if (objIO != null)
                {
                    objIO.RemoveGameFlag(IoGlobals.GFLAG_NEEDINIT);
                }
            }
        }
        protected void RunEvent(Scriptable script, string eventName, BaseInteractiveObject io)
        {
            int msg = 0;
            if (string.Equals(eventName, "INIT", StringComparison.OrdinalIgnoreCase))
            {
                msg = ScriptConsts.SM_001_INIT;
            }
            else if (string.Equals(eventName, "HIT", StringComparison.OrdinalIgnoreCase))
            {
                msg = ScriptConsts.SM_016_HIT;
            }
            else if (string.Equals(eventName, "INIT_END", StringComparison.OrdinalIgnoreCase))
            {
                msg = ScriptConsts.SM_033_INITEND;
            }
            if (msg > 0)
            {
                RunMessage(script, msg, io);
            }
            else
            {
                try
                {
                    MethodInfo method;
                    if (!eventName.StartsWith("on"))
                    {
                        PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                        sb.Append("on");
                        sb.Append(eventName.ToUpper()[0]);
                        sb.Append(eventName.Substring(1));
                        Type type = script.GetType();
                        method = script.GetType().GetRuntimeMethod(sb.ToString(), null);
                        sb.ReturnToPool();
                        sb = null;
                    }
                    else
                    {
                        method = script.GetType().GetRuntimeMethod(eventName, null);
                    }
                    method.Invoke(script, (object[])null);
                }
                catch (Exception e)
                {
                    throw new RPGException(ErrorMessage.INVALID_PARAM, e);
                }
            }
        }
        protected void RunMessage(Scriptable script, int msg, BaseInteractiveObject io)
        {
            switch (msg)
            {
                case ScriptConsts.SM_001_INIT:
                    script.OnInit();
                    break;
                case ScriptConsts.SM_002_INVENTORYIN:
                    script.OnInventoryIn();
                    break;
                case ScriptConsts.SM_004_INVENTORYUSE:
                    script.OnInventoryUse();
                    break;
                case ScriptConsts.SM_007_EQUIPOUT:
                    script.OnUnequip();
                    break;
                case ScriptConsts.SM_016_HIT:
                    script.OnHit();
                    break;
                case ScriptConsts.SM_017_DIE:
                    script.OnDie();
                    break;
                case ScriptConsts.SM_024_COMBINE:
                    script.OnCombine();
                    break;
                case ScriptConsts.SM_033_INITEND:
                    script.OnInitEnd();
                    break;
                case ScriptConsts.SM_041_LOAD:
                    script.OnLoad();
                    break;
                case ScriptConsts.SM_043_RELOAD:
                    script.OnReload();
                    break;
                case ScriptConsts.SM_045_OUCH:
                    script.OnOuch();
                    break;
                case ScriptConsts.SM_046_HEAR:
                    script.OnHear();
                    break;
                case ScriptConsts.SM_057_AGGRESSION:
                    script.OnAggression();
                    break;
                case ScriptConsts.SM_069_IDENTIFY:
                    script.OnIdentify();
                    break;
                default:
                    throw new RPGException(ErrorMessage.INVALID_PARAM, "No action defined for message " + msg);
            }
        }
        public void SendEvent(BaseInteractiveObject io, SendParameters parameters)
        {
            BaseInteractiveObject oes = EventSender;
            EventSender = io;
            if (parameters.HasFlag(SendParameters.RADIUS))
            {
                // SEND EVENT TO ALL OBJECTS IN A RADIUS

                // LOOP THROUGH ALL IOs.
                int i = Interactive.Instance.GetMaxIORefId();
                for (; i >= 0; i--)
                {
                    if (Interactive.Instance.hasIO(i))
                    {
                        BaseInteractiveObject iio = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                        // skip cameras and markers
                        // if (iio.HasIOFlag(IoGlobals.io_camera)
                        // || iio.HasIOFlag(IoGlobals.io_marker)) {
                        // continue;
                        // }
                        // skip IOs not in required group
                        if (parameters.HasFlag(SendParameters.GROUP))
                        {
                            if (!IsIOInGroup(iio, parameters.GroupName))
                            {
                                continue;
                            }
                        }
                        // if send event is for NPCs, send to NPCs,
                        // if for Items, send to Items, etc...
                        if ((parameters.HasFlag(SendParameters.IONpcData)

                                && iio.HasIOFlag(IoGlobals.IO_03_NPC))
                                            // || (parameters.HasFlag(SendParameters.FIX)
                                            // && iio.HasIOFlag(IoGlobals.IO_FIX))
                                            || (parameters.HasFlag(SendParameters.IOItemData)
                                                    && iio.HasIOFlag(IoGlobals.IO_02_ITEM)))
                        {
                            /*
                            Vector2 senderPos = new Vector2(),
                                    ioPos = new Vector2();
                            Interactive.Instance.GetItemWorldPosition(io, senderPos);
                            Interactive.Instance.GetItemWorldPosition(iio, ioPos);
                            */
                            // IF BaseInteractiveObject IS IN SENDER RADIUS, SEND EVENT
                            io.StatSent++;
                            StackSendIOScriptEvent(
                                    iio,
                                    0,
                                                parameters.EventParameters,
                                                parameters.EventName);
                        }
                    }
                }
            }
            if (parameters.HasFlag(SendParameters.ZONE))
            {
                // SEND EVENT TO ALL OBJECTS IN A ZONE
                // ARX_PATH * ap = ARX_PATH_GetAddressByName(zonename);

                // if (ap != NULL) {
                // LOOP THROUGH ALL IOs.
                int i = Interactive.Instance.GetMaxIORefId();
                for (; i >= 0; i--)
                {
                    if (Interactive.Instance.hasIO(i))
                    {
                        BaseInteractiveObject iio = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                        // skip cameras and markers
                        // if (iio.HasIOFlag(IoGlobals.io_camera)
                        // || iio.HasIOFlag(IoGlobals.io_marker)) {
                        // continue;
                        // }
                        // skip IOs not in required group
                        if (parameters.HasFlag(SendParameters.GROUP))
                        {
                            if (!IsIOInGroup(iio, parameters.GroupName))
                            {
                                continue;
                            }
                        }
                        // if send event is for NPCs, send to NPCs,
                        // if for Items, send to Items, etc...
                        if ((parameters.HasFlag(SendParameters.IONpcData)
                                && iio.HasIOFlag(IoGlobals.IO_03_NPC))
                                // || (parameters.HasFlag(SendParameters.FIX)
                                // && iio.HasIOFlag(IoGlobals.IO_FIX))
                                || (parameters.HasFlag(SendParameters.IOItemData)
                                        && iio.HasIOFlag(IoGlobals.IO_02_ITEM)))
                        {
                            /*
                            Vector2 ioPos = new Vector2();
                            Interactive.Instance.GetItemWorldPosition(iio,
                                    ioPos);
                            // IF BaseInteractiveObject IS IN ZONE, SEND EVENT
                            // if (ARX_PATH_IsPosInZone(ap, _pos.x, _pos.y, _pos.z))
                            // {
                            */
                            io.StatSent++;
                            StackSendIOScriptEvent(
                                    iio,
                                    0,
                                    parameters.EventParameters,
                                    parameters.EventName);
                            // }
                        }
                    }
                }
            }
            if (parameters.HasFlag(SendParameters.GROUP))
            {
                // sends an event to all members of a group
                // LOOP THROUGH ALL IOs.
                int i = Interactive.Instance.GetMaxIORefId();
                for (; i >= 0; i--)
                {
                    if (Interactive.Instance.hasIO(i))
                    {
                        BaseInteractiveObject iio = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                        // skip IOs not in required group
                        if (!this.IsIOInGroup(iio, parameters.GroupName))
                        {
                            continue;
                        }
                        iio.StatSent++;
                        StackSendIOScriptEvent(
                                iio,
                                0,
                                parameters.EventParameters,
                                parameters.EventName);
                    }
                }
            }
            else
            {
                // SINGLE OBJECT EVENT
                int tioid = Interactive.Instance.getTargetByNameTarget(parameters.TargetName);

                if (tioid == -2)
                {
                    tioid = Interactive.Instance.GetInterNum(io);
                }
                if (Interactive.Instance.hasIO(tioid))
                {
                    io.StatSent++;
                    StackSendIOScriptEvent(
                            (BaseInteractiveObject)Interactive.Instance.getIO(tioid),
                            0,
                            parameters.EventParameters,
                            parameters.EventName);
                }
            }
            EventSender = oes;
        }
        /// <summary>
        /// Sends an initialization event to an BaseInteractiveObject. The initialization event runs the local script first, followed by the over script.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <returns></returns>
        public int SendInitScriptEvent(BaseInteractiveObject io)
        {
            if (io == null)
            {
                return -1;
            }
            int num = io.GetRefId();
            if (!Interactive.Instance.hasIO(num))
            {
                return -1;
            }
            BaseInteractiveObject oldEventSender = EventSender;
            EventSender = null;
            // send script the init message
            BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
            if (hio.Script != null)
            {
                GLOB = 0;
                SendScriptEvent((Scriptable)hio.Script,
                        ScriptConsts.SM_001_INIT,
                        null,
                        hio,
                        null);
            }
            hio = null;
            // send overscript the init message
            if (Interactive.Instance.getIO(num) != null)
            {
                hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                if (hio.Overscript != null)
                {
                    GLOB = 0;
                    SendScriptEvent((Scriptable)hio.Overscript,
                            ScriptConsts.SM_001_INIT,
                            null,
                            hio,
                            null);
                }
                hio = null;
            }
            // send script the init end message
            if (Interactive.Instance.getIO(num) != null)
            {
                hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                if (hio.Script != null)
                {
                    GLOB = 0;
                    SendScriptEvent((Scriptable)hio.Script,
                            ScriptConsts.SM_033_INITEND,
                            null,
                            hio,
                            null);
                }
                hio = null;
            }
            // send overscript the init end message
            if (Interactive.Instance.getIO(num) != null)
            {
                hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                if (hio.Overscript != null)
                {
                    GLOB = 0;
                    SendScriptEvent((Scriptable)hio.Overscript,
                            ScriptConsts.SM_033_INITEND,
                            null,
                            hio,
                            null);
                }
                hio = null;
            }
            EventSender = oldEventSender;
            return ScriptConsts.ACCEPT;
        }
        /// <summary>
        /// Sends a script event to an interactive object. The returned value is a flag indicating whether the event was successful or refused.
        /// </summary>
        /// <param name="target">the reference id of the targeted io</param>
        /// <param name="msg">the message being sent</param>
        /// <param name="parameters">the list of parameters applied, grouped in key-value pairs, for example, new Object[] {"key0", value, "key1", new int[] {0, 0}}</param>
        /// <param name="eventname">the name of the event</param>
        /// <returns></returns>
        public int SendIOScriptEvent(BaseInteractiveObject target, int msg, Object[] parameters, string eventname)
        {
            // checks invalid BaseInteractiveObject
            if (target == null)
            {
                return -1;
            }
            int num = target.GetRefId();

            if (Interactive.Instance.hasIO(num))
            {
                BaseInteractiveObject originalEventSender = EventSender;
                if (msg == ScriptConsts.SM_001_INIT
                        || msg == ScriptConsts.SM_033_INITEND)
                {
                    BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                    SendIOScriptEventReverse(hio, msg, parameters, eventname);
                    EventSender = originalEventSender;
                    hio = null;
                }

                if (Interactive.Instance.hasIO(num))
                {
                    // if this BaseInteractiveObject only has a Local script, send event to it
                    BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                    if (hio.Overscript == null)
                    {
                        GLOB = 0;
                        int ret = SendScriptEvent(
                                (Scriptable)hio.Script,
                                msg,
                                    parameters,
                                hio,
                                eventname);
                        EventSender = originalEventSender;
                        return ret;
                    }

                    // If this BaseInteractiveObject has a Global script send to Local (if exists)
                    // then to Global if not overridden by Local
                    int s = SendScriptEvent(
                            (Scriptable)hio.Overscript,
                            msg,
                                parameters,
                            hio,
                            eventname);
                    if (s != ScriptConsts.REFUSE)
                    {
                        EventSender = originalEventSender;
                        GLOB = 0;

                        if (Interactive.Instance.hasIO(num))
                        {
                            hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                            int ret = SendScriptEvent(
                                    (Scriptable)hio.Script,
                                    msg,
                                        parameters,
                                    hio,
                                    eventname);
                            EventSender = originalEventSender;
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
        private int SendIOScriptEventReverse(BaseInteractiveObject io, int msg, Object[] parameters, string eventname)
        {
            // checks invalid BaseInteractiveObject
            if (io == null)
            {
                return -1;
            }
            // checks for no script assigned
            if (io.Overscript == null
                    && io.Script == null)
            {
                return -1;
            }
            int num = io.GetRefId();
            if (Interactive.Instance.hasIO(num))
            {
                BaseInteractiveObject hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                // if this BaseInteractiveObject only has a Local script, send event to it
                if (hio.Overscript == null
                        && hio.Script != null)
                {
                    GLOB = 0;
                    return SendScriptEvent(
                            (Scriptable)hio.Script,
                            msg,
                                parameters,
                            hio,
                            eventname);
                }

                // If this BaseInteractiveObject has a Global script send to Local (if exists)
                // then to global if no overriden by Local
                if (Interactive.Instance.hasIO(num))
                {
                    hio = (BaseInteractiveObject)Interactive.Instance.getIO(num);
                    int s = SendScriptEvent(
                            (Scriptable)hio.Script,
                            msg,
                                parameters,
                            hio,
                            eventname);
                    if (s != ScriptConsts.REFUSE)
                    {
                        GLOB = 0;
                        if (Interactive.Instance.hasIO(io.GetRefId()))
                        {
                            return SendScriptEvent(
                                    (Scriptable)io.Overscript,
                                    msg,
                                        parameters,
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
        /// <summary>
        /// Sends a scripted event to all IOs.
        /// </summary>
        /// <param name="msg">the message</param>
        /// <param name="dat">any script variables</param>
        /// <returns></returns>
        public int SendMsgToAllIO(int msg, Object[] dat)
        {
            int ret = ScriptConsts.ACCEPT;
            int i = Interactive.Instance.GetMaxIORefId();
            for (; i >= 0; i--)
            {
                if (Interactive.Instance.hasIO(i))
                {
                    BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                    if (SendIOScriptEvent(io, msg, dat, null) == ScriptConsts.REFUSE)
                    {
                        ret = ScriptConsts.REFUSE;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// Sends a scripted event to an BaseInteractiveObject.
        /// </summary>
        /// <param name="localScript">the local script the BaseInteractiveObject should follow</param>
        /// <param name="msg">the event message</param>
        /// <param name="p">any parameters to be applied</param>
        /// <param name="io"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public int SendScriptEvent(Scriptable localScript, int msg, Object[] p, BaseInteractiveObject io, string eventName)
        {
            int retVal = ScriptConsts.ACCEPT;
            bool keepGoing = true;
            if (localScript == null)
            {
                throw new RPGException(ErrorMessage.INVALID_PARAM, "script cannot be null");
            }
            if (io != null)
            {
                if (io.HasGameFlag(IoGlobals.GFLAG_MEGAHIDE)
                        && msg != ScriptConsts.SM_043_RELOAD)
                {
                    return ScriptConsts.ACCEPT;
                }

                if (io.Show == IoGlobals.SHOW_FLAG_DESTROYED)
                {
                    // destroyed
                    return ScriptConsts.ACCEPT;
                }
                eventTotalCount++;
                io.StatCount++;

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
                    if (io.NpcData.GetBaseLife() <= 0f
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
            Scriptable script = (Scriptable)localScript.Master;
            if (script == null)
            { // no master - use local script
                script = localScript;
            }
            // Set parameters on script that will be used
            if (p != null
                    && p.Length > 0)
            {
                for (int i = 0; i < p.Length; i += 2)
                {
                    script.SetLocalVariable((string)p[i], p[i + 1]);
                }
            }
            // run the event
            if (eventName != null
                    && eventName.Length > 0)
            {
                RunEvent(script, eventName, io);
            }
            else
            {
                if (EventIsDisallowed(msg, script))
                {
                    return ScriptConsts.REFUSE;
                }
                RunMessage(script, msg, io);
            }
            int ret = ScriptConsts.ACCEPT;
            return ret;
        }
        public void SetEvent(BaseInteractiveObject io, string e, bool isOn)
        {
            if (string.Equals(e, "COLLIDE_NPC", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_COLLIDE_NPC);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_COLLIDE_NPC);
                }
            }
            else if (string.Equals(e, "CHAT", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_CHAT);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_CHAT);
                }
            }
            else if (string.Equals(e, "HIT", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_HIT);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_HIT);
                }
            }
            else if (string.Equals(e, "INVENTORY2_OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_INVENTORY2_OPEN);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_INVENTORY2_OPEN);
                }
            }
            else if (string.Equals(e, "DETECTPLAYER", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_DETECT);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_DETECT);
                }
            }
            else if (string.Equals(e, "HEAR", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_HEAR);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_HEAR);
                }
            }
            else if (string.Equals(e, "AGGRESSION", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_AGGRESSION);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_AGGRESSION);
                }
            }
            else if (string.Equals(e, "MAIN", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_MAIN);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_MAIN);
                }
            }
            else if (string.Equals(e, "CURSORMODE", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_CURSORMODE);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_CURSORMODE);
                }
            }
            else if (string.Equals(e, "EXPLORATIONMODE", StringComparison.OrdinalIgnoreCase))
            {
                if (isOn)
                {
                    io.Script.RemoveDisallowedEvent(ScriptConsts.DISABLE_EXPLORATIONMODE);
                }
                else
                {
                    io.Script.AssignDisallowedEvent(ScriptConsts.DISABLE_EXPLORATIONMODE);
                }
            }
        }
        /// <summary>
        /// Sets a global variable.
        /// </summary>
        /// <param name="name">the name of the global variable</param>
        /// <param name="value">the variable's value</param>
        public void SetGlobalVariable(string name, Object value)
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
                        && var.Name != null
                        && string.Equals(var.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    // found the correct script variable
                    var.Set(value);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // create a new variable and add to the global array
                ScriptVariable var = null;
                if (value is string
                            || value is char[])
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_00_TEXT, value);
                }
                else if (value is string[]
                          || value is char[][])
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_01_TEXT_ARR, value);
                }
                else if (value is float)
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_02_FLOAT, value);
                }
                else if (value is Double)
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_02_FLOAT, value);
                }
                else if (value is float[])
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_03_FLOAT_ARR, value);
                }
                else if (value is int)
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_04_INT, value);
                }
                else if (value is int[])
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_05_INT_ARR, value);
                }
                else if (value is long)
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_06_LONG, value);
                }
                else if (value is long[])
                {
                    var = new ScriptVariable(name, ScriptConsts.TYPE_G_07_LONG_ARR, value);
                }
                else
                {
                    PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                    try
                    {
                        sb.Append("Global variable ");
                        sb.Append(name);
                        sb.Append(" was passed new value of type ");
                        sb.Append(value.GetType());
                        sb.Append(". Only string, string[], char[][], Float, ");
                        sb.Append("float[], Integer, int[], Long, or long[] ");
                        sb.Append("allowed.");
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
                gvars = ArrayUtilities.Instance.ExtendArray(var, gvars);
            }
        }
        /// <summary>
        /// Sets the main event for an BaseInteractiveObject.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="newevent">the event's name</param>
        public void SetMainEvent(BaseInteractiveObject io, string newevent)
        {
            if (io != null)
            {
                if (!string.Equals("MAIN", newevent, StringComparison.OrdinalIgnoreCase))
                {
                    io.Mainevent = null;
                }
                else
                {
                    io.Mainevent = newevent;
                }
            }
        }
        protected abstract void SetScriptTimer(int index, ScriptTimer timer);
        /// <summary>
        /// Processes and BaseInteractiveObject's speech.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="parameters">the {@link SpeechParameters}</param>
        public void Speak(BaseInteractiveObject io, SpeechParameters parameters)
        {
            // speech variables
            // ARX_CINEMATIC_SPEECH acs;
            // acs.type = ARX_CINE_SPEECH_NONE;
            long voixoff = 0;
            int mood = ANIM_TALK_NEUTRAL;
            if (parameters.KillAllSpeech)
            {
                // ARX_SPEECH_Reset();
            }
            else
            {
                if (parameters.HasFlag(SpeechParameters.HAPPY))
                {
                    mood = ANIM_TALK_HAPPY;
                }
                if (parameters.HasFlag(SpeechParameters.ANGRY))
                {
                    mood = ANIM_TALK_ANGRY;
                }
                if (parameters.HasFlag(SpeechParameters.OFF_VOICE))
                {
                    voixoff = 2;
                }
                if (parameters.HasFlag(SpeechParameters.KEEP_SPEECH)
                        || parameters.HasFlag(SpeechParameters.ZOOM_SPEECH)
                        || parameters.HasFlag(SpeechParameters.SPEECH_CCCTALKER_L)
                        || parameters.HasFlag(SpeechParameters.SPEECH_CCCTALKER_R)
                        || parameters.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)
                        || parameters.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_R)
                        || parameters.HasFlag(SpeechParameters.SIDE_L)
                        || parameters.HasFlag(SpeechParameters.SIDE_R))
                {
                    // FRAME_COUNT = 0;
                    if (parameters.HasFlag(SpeechParameters.KEEP_SPEECH))
                    {
                        // acs.type = ARX_CINE_SPEECH_KEEP;
                        // acs.pos1.x = LASTCAMPOS.x;
                        // acs.pos1.y = LASTCAMPOS.y;
                        // acs.pos1.z = LASTCAMPOS.z;
                        // acs.pos2.a = LASTCAMANGLE.a;
                        // acs.pos2.b = LASTCAMANGLE.b;
                        // acs.pos2.g = LASTCAMANGLE.g;
                    }
                    if (parameters.HasFlag(SpeechParameters.ZOOM_SPEECH))
                    {
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

                        if (parameters.HasFlag(SpeechParameters.PLAYER))
                        {
                            // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                        }
                        else
                        {
                            // ComputeACSPos(&acs, io, -1);
                        }
                    }
                    if (parameters.HasFlag(SpeechParameters.SPEECH_CCCTALKER_L)
                            || parameters
                                        .HasFlag(SpeechParameters.SPEECH_CCCTALKER_R))
                    {
                        if (parameters.HasFlag(SpeechParameters.SPEECH_CCCTALKER_L))
                        {
                            // acs.type = ARX_CINE_SPEECH_CCCTALKER_R;
                        }
                        else
                        {
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

                        if (parameters.HasFlag(SpeechParameters.PLAYER))
                        {
                            // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                        }
                        else
                        {
                            // ComputeACSPos(&acs, io, acs.ionum);
                        }
                    }
                    if (parameters.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_L)
                            || parameters.HasFlag(
                                    SpeechParameters.SPEECH_CCCLISTENER_R))
                    {
                        if (parameters.HasFlag(SpeechParameters.SPEECH_CCCLISTENER_L))
                        {
                            // acs.type = ARX_CINE_SPEECH_CCCLISTENER_L;
                        }
                        else
                        {
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

                        if (parameters.HasFlag(SpeechParameters.PLAYER))
                        {
                            // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                        }
                        else
                        {
                            // ComputeACSPos(&acs, io, acs.ionum);
                        }
                    }
                    if (parameters.HasFlag(SpeechParameters.SIDE_L)
                            || parameters.HasFlag(SpeechParameters.SIDE_R))
                    {
                        if (parameters.HasFlag(SpeechParameters.SIDE_L))
                        {
                            // acs.type = ARX_CINE_SPEECH_SIDE_LEFT;
                        }
                        else
                        {
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

                        if (parameters.HasFlag(SpeechParameters.PLAYER))
                        {
                            // ComputeACSPos(&acs, inter.iobj[0], acs.ionum);
                        }
                        else
                        {
                            // ComputeACSPos(&acs, io, acs.ionum);
                        }
                    }
                }

                long speechnum = 0;

                if (parameters.SpeechName == null
                        || parameters.SpeechName.Length == 0)
                {
                    // ARX_SPEECH_ClearIOSpeech(io);
                }
                else
                {
                    if (parameters.HasFlag(SpeechParameters.NO_TEXT))
                    {
                        // voixoff |= ARX_SPEECH_FLAG_NOTEXT;
                    }

                    // if (!CINEMASCOPE) voixoff |= ARX_SPEECH_FLAG_NOTEXT;
                    if (parameters.HasFlag(SpeechParameters.PLAYER))
                    {
                        // speechnum = ARX_SPEECH_AddSpeech(inter.iobj[0], temp1,
                        // PARAM_LOCALISED, mood, voixoff);
                    }
                    else
                    {
                        // speechnum = ARX_SPEECH_AddSpeech(io, temp1,
                        // PARAM_LOCALISED, mood, voixoff);
                        // TODO - handle speech
                        // speechnum = Speech.Instance.ARX_SPEECH_AddSpeech(io, mood, parameters.SpeechName, voixoff);
                    }

                    if (speechnum >= 0)
                    {
                        // ARX_SCRIPT_Timer_GetDefaultName(timername2);
                        // sprintf(timername, "SPEAK_%s", timername2);
                        // aspeech[speechnum].scrpos = pos;
                        // aspeech[speechnum].es = es;
                        // aspeech[speechnum].ioscript = io;
                        if (parameters.HasFlag(SpeechParameters.UNBREAKABLE))
                        {
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
        /// <summary>
        /// Sends a scripted event to the event stack for all members of a group, to be fired during the game cycle.
        /// </summary>
        /// <param name="group">the name of the BaseInteractiveObject group</param>
        /// <param name="msg">the script message</param>
        /// <param name="parameters">the parameters assigned to the script</param>
        /// <param name="eventname">the event name</param>
        public void stackSendGroupScriptEvent(string group, int msg, Object[] parameters, string eventname)

        {
            int i = Interactive.Instance.GetMaxIORefId();
            for (; i >= 0; i--)
            {
                if (Interactive.Instance.hasIO(i))
                {
                    BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                    if (IsIOInGroup(io, group))
                    {
                        StackSendIOScriptEvent(io, msg, parameters, eventname);
                    }
                    io = null;
                }
            }
        }
        /// <summary>
        /// ends an BaseInteractiveObject scripted event to the event stack, to be fired during the game cycle.
        /// </summary>
        /// <param name="io">the BaseInteractiveObject</param>
        /// <param name="msg">the script message</param>
        /// <param name="parameters">the parameters assigned to the script</param>
        /// <param name="eventname">the event name</param>
        public void StackSendIOScriptEvent(BaseInteractiveObject io, int msg, Object[] parameters, string eventname)
        {
            for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
            {
                StackedEvent e = GetStackedEvent(i);
                if (!e.Exists)
                {
                    if (parameters != null
                            && parameters.Length > 0)
                    {
                        e.Parameters = parameters;
                    }
                    else
                    {
                        e.Parameters = null;
                    }
                    if (eventname != null
                            && eventname.Length > 0)
                    {
                        e.Eventname = eventname;
                    }
                    else
                    {
                        e.Eventname = null;
                    }

                    e.Sender = EventSender;
                    e.Io = io;
                    e.Msg = msg;
                    e.Exists = true;
                    break;
                }
            }
        }
        /// <summary>
        /// Adds messages for all NPCs to the event stack.
        /// </summary>
        /// <param name="msg">the message</param>
        /// <param name="dat">the message parameters</param>
        public void StackSendMsgToAllNPCIO(int msg, Object[] dat)
        {
            int i = Interactive.Instance.GetMaxIORefId();
            for (; i >= 0; i--)
            {
                if (Interactive.Instance.hasIO(i))
                {
                    BaseInteractiveObject io = (BaseInteractiveObject)Interactive.Instance.getIO(i);
                    if (io.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        StackSendIOScriptEvent(io, msg, dat, null);
                    }
                }
            }
        }
        /// <summary>
        /// Starts a timer using a defined Set of parameters.
        /// </summary>
        /// <param name="parameters">the parameters</param>
        public void StartTimer(ScriptTimerInitializationParameters parameters)
        {
            int timerNum = timerGetFree();
            ScriptTimer timer = getScriptTimer(timerNum);
            timer.Script = parameters.Script;
            timer.Exists = true;
            timer.Io = parameters.Io;
            timer.Msecs = parameters.Milliseconds;
            if (parameters.Name == null
                    || (parameters.Name != null
                            && parameters.Name.Length == 0))
            {
                timer.Name = TimerGetDefaultName();
            }
            else
            {
                timer.Name = parameters.Name;
            }
            timer.Action = new ScriptTimerAction(
                        parameters.Obj,
                        parameters.Method,
                        parameters.Args);
            timer.Tim = parameters.StartTime;
            timer.Times = parameters.RepeatTimes;
            timer.ClearFlags();
            timer.AddFlag(parameters.FlagValues);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="io"></param>
        /// <param name="behind"></param>
        /// <param name="isPlayer"></param>
        /// <param name="initPosition"></param>
        /// <param name="target"></param>
        public void teleport(BaseInteractiveObject io, bool behind, bool isPlayer, bool initPosition, string target)
        {
            if (behind)
            {
                Interactive.Instance.ARX_INTERACTIVE_TeleportBehindTarget(io);
            }
            else
            {
                if (!initPosition)
                {
                    int ioid =
                            Interactive.Instance.getTargetByNameTarget(target);

                    if (ioid == -2)
                    {
                        ioid = Interactive.Instance.GetInterNum(io);
                    }
                    if (ioid != -1
                            && ioid != -2)
                    {
                        if (ioid == -3)
                        {
                            if (io.Show != IoGlobals.SHOW_FLAG_LINKED
                                    && io.Show != IoGlobals.SHOW_FLAG_HIDDEN
                                    && io.Show != IoGlobals.SHOW_FLAG_MEGAHIDE
                                    && io.Show != IoGlobals.SHOW_FLAG_DESTROYED
                                    && io.Show != IoGlobals.SHOW_FLAG_KILLED)
                            {
                                io.Show = IoGlobals.SHOW_FLAG_IN_SCENE);
                            }
                            BaseInteractiveObject pio = (BaseInteractiveObject)Interactive.Instance.getIO(
                                    ProjectConstants.Instance.getPlayer());
                            Interactive.Instance.ARX_INTERACTIVE_Teleport(
                                    io, pio.getPosition());
                            pio = null;
                        }
                        else
                        {
                            if (Interactive.Instance.hasIO(ioid))
                            {
                                BaseInteractiveObject tio = (BaseInteractiveObject)Interactive.Instance.getIO(ioid);
                                Vector2 pos = new Vector2();

                                if (Interactive.Instance
                                        .GetItemWorldPosition(tio, pos))
                                {
                                    if (isPlayer)
                                    {
                                        BaseInteractiveObject pio = (BaseInteractiveObject)Interactive.Instance
                                                .getIO(
                                                        ProjectConstants
                                                                .Instance
                                                                .getPlayer());
                                        Interactive.Instance
                                                .ARX_INTERACTIVE_Teleport(
                                                        pio, pos);
                                        pio = null;
                                    }
                                    else
                                    {
                                        if (io.HasIOFlag(IoGlobals.IO_03_NPC)
                                                && io.NpcData
                                                        .GetBaseLife() <= 0)
                                        {
                                            // do nothing
                                        }
                                        else
                                        {
                                            if (io.Show != IoGlobals.SHOW_FLAG_HIDDEN
                                                    && io.Show != IoGlobals.SHOW_FLAG_MEGAHIDE)
                                            {
                                                io.show =
                                                        IoGlobals.SHOW_FLAG_IN_SCENE);
                                            }
                                            Interactive.Instance
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
                            if (Interactive.Instance.GetItemWorldPosition(io,
                                    pos))
                            {
                                BaseInteractiveObject pio = (BaseInteractiveObject)Interactive.Instance.getIO(
                                        ProjectConstants.Instance.getPlayer());
                                Interactive.Instance.ARX_INTERACTIVE_Teleport(
                                        pio, pos);
                                pio = null;

                            }
                        }
                        else
                        {
                            if (io.HasIOFlag(IoGlobals.IO_03_NPC)
                                    && io.NpcData.GetBaseLife() <= 0)
                            {
                                // do nothing
                            }
                            else
                            {
                                if (io.Show != IoGlobals.SHOW_FLAG_HIDDEN
                                        && io.Show != IoGlobals.SHOW_FLAG_MEGAHIDE)
                                {
                                    io.show = IoGlobals.SHOW_FLAG_IN_SCENE);
                                }
                                Interactive.Instance.ARX_INTERACTIVE_Teleport(
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
                    ScriptTimer timer = getScriptTimers()[i];
                    if (timer.Exists)
                    {
                        long currentTime = Time.Instance.getGameTime();
                        if (timer.isTurnBased())
                        {
                            currentTime = Time.Instance.getGameRound();
                        }
                        if (timer.HasFlag(1))
                        {
                            if (!timer.Io.HasGameFlag(
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
                            Scriptable script = (Scriptable)timer.Script;
                            BaseInteractiveObject io = timer.Io;
                            if (script != null)
                            {
                                if (string.Equals("_R_A_T_", timer.Name, StringComparison.OrdinalIgnoreCase))
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
                                    && Interactive.Instance.hasIO(io))
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
            ScriptTimer[] scriptTimers = getScriptTimers();
            for (int i = 0; i < maxTimerScript; i++)
            {
                if (scriptTimers[i].Exists)
                {
                    if (scriptTimers[i].Io.Equals(io)
                            && scriptTimers[i].Script
                                    .Equals(io.Overscript))
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
                ScriptTimer[] scriptTimers = getScriptTimers();
                for (int i = 0; i < maxTimerScript; i++)
                {
                    if (scriptTimers[i] != null
                            && scriptTimers[i].Exists)
                    {
                        if (scriptTimers[i].Io.GetRefId() == io.GetRefId())
                        {
                            timerClearByNum(i);
                        }
                    }
                }
            }
        }
        public void timerClearByNameAndIO(string timername,
                 BaseInteractiveObject io)
        {
            if (io != null)
            {
                ScriptTimer[] scriptTimers = getScriptTimers();
                for (int i = 0; i < maxTimerScript; i++)
                {
                    if (scriptTimers[i] != null
                            && scriptTimers[i].Exists)
                    {
                        if (scriptTimers[i].Io.GetRefId() == io.GetRefId()
                                && string.Equals(timername, scriptTimers[i].Name, StringComparison.OrdinalIgnoreCase))
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
            ScriptTimer[] scriptTimers = getScriptTimers();
            if (scriptTimers[timeridx].Exists)
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
        private int timerExist(string texx)
        {
            int index = -1;
            ScriptTimer[] scriptTimers = getScriptTimers();
            for (int i = 0; i < maxTimerScript; i++)
            {
                if (scriptTimers[i].Exists)
                {
                    if (scriptTimers[i].Name != null
                            && string.Equals(scriptTimers[i].Name, texx, StringComparison.OrdinalIgnoreCase))
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
         * @return {@link string}
         */
        private string timerGetDefaultName()
        {
            int i = 1;
            string texx;

            while (true)
            {
                PooledStringBuilder sb =
                        StringBuilderPool.Instance.GetStringBuilder();
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
            ScriptTimer[] scriptTimers = getScriptTimers();
            for (int i = 0; i < maxTimerScript; i++)
            {
                if (!scriptTimers[i].Exists)
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
            return Time.Instance.getGameTime(false);
        }
    }
}
