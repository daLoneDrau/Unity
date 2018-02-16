using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Flyweights;
using RPGBase.Constants;

namespace Assets.Scripts.BarbarianPrince.Singletons
{
    public class BPScript : Script
    {
        /// <summary>
        /// the event stack
        /// </summary>
        private StackedEvent[] eventstack;
        /// <summary>
        /// the list of script timers.
        /// </summary>
        private ScriptTimer[] scriptTimers;
        public BPScript()
        {
            // master = ((FFInteractive)Interactive.getInstance()).getMasterScript();
            EventStackInit();
            MaxTimerScript = 100;
            scriptTimers = new ScriptTimer[MaxTimerScript];
            for (int i = MaxTimerScript - 1; i >= 0; i--)
            {
                scriptTimers[i] = new ScriptTimer();
            }
            // init global params
            SetGlobalVariable("PLAYERCASTING", 0);
            SetGlobalVariable("COMBATROUND", 0);
            SetGlobalVariable("SHUT_UP", 0);
            // set singleton
            Script.Instance = this;
        }
        public override void EventStackInit()
        {
            eventstack = new StackedEvent[ScriptConsts.MAX_EVENT_STACK];
            for (int i = 0; i < ScriptConsts.MAX_EVENT_STACK; i++)
            {
                eventstack[i] = new StackedEvent();
            }
        }

        public override ScriptTimer GetScriptTimer(int id)
        {
            return scriptTimers[id];
        }

        protected override void ClearAdditionalEventStacks()
        {
            throw new NotImplementedException();
        }

        protected override void ClearAdditionalEventStacksForIO(BaseInteractiveObject io)
        {
            throw new NotImplementedException();
        }

        protected override void DestroyScriptTimers()
        {
            if (scriptTimers != null)
            {
                for (int i = scriptTimers.Length - 1; i >= 0; i--)
                {
                    scriptTimers[i] = null;
                }
                scriptTimers = null;
            }
        }

        protected override void ExecuteAdditionalStacks()
        {
            //throw new NotImplementedException();
        }

        protected override ScriptTimer[] GetScriptTimers()
        {
            return scriptTimers;
        }

        protected override StackedEvent GetStackedEvent(int index)
        {
            return eventstack[index];
        }

        protected override void InitScriptTimers()
        {
            scriptTimers = new ScriptTimer[MaxTimerScript];
            for (int i = 0; i < scriptTimers.Length; i++)
            {
                scriptTimers[i] = new ScriptTimer();
            }
        }

        protected override void SetScriptTimer(int index, ScriptTimer timer)
        {
            scriptTimers[index] = timer;
        }
    }
}
