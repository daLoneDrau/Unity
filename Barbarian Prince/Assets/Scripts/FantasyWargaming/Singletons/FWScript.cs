﻿using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Flyweights;
using RPGBase.Constants;
using UnityEngine;
using Assets.Scripts.FantasyWargaming.Globals;
using Assets.Scripts.FantasyWargaming.Flyweights;
using Assets.Scripts.FantasyWargaming.Scriptables.Mobs;

namespace Assets.Scripts.FantasyWargaming.Singletons
{
    public class FWScript : Script
    {
        /// <summary>
        /// the event stack
        /// </summary>
        private StackedEvent[] eventstack;
        /// <summary>
        /// the list of script timers.
        /// </summary>
        private ScriptTimer[] scriptTimers;
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "FWScript"
                };
                Instance = go.AddComponent<FWScript>();
            }
        }
        void Awake()
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
            // set singleton - now s
            // Script.Instance = this;
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

        public override void ClearAdditionalEventStacks()
        {
            throw new NotImplementedException();
        }

        public override void ClearAdditionalEventStacksForIO(BaseInteractiveObject io)
        {
            throw new NotImplementedException();
        }

        public override void DestroyScriptTimers()
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

        public override void ExecuteAdditionalStacks()
        {
            //throw new NotImplementedException();
        }

        public override ScriptTimer[] GetScriptTimers()
        {
            return scriptTimers;
        }

        public override StackedEvent GetStackedEvent(int index)
        {
            return eventstack[index];
        }

        public override void InitScriptTimers()
        {
            scriptTimers = new ScriptTimer[MaxTimerScript];
            for (int i = 0; i < scriptTimers.Length; i++)
            {
                scriptTimers[i] = new ScriptTimer();
            }
        }
        protected override void RunMessage(Scriptable script, int msg, BaseInteractiveObject io)
        {
            if (Debug)
            {
                print("RunMessage(" + script + "," + msg + "," + io.RefId);
            }
            switch (msg)
            {
                case FWGlobals.SM_300_MORALE_CHECK:
                    ((FWScriptable)script).OnMoraleCheck();
                    break;
                case FWGlobals.SM_301_BERSERK_CHECK:
                    ((FWScriptable)script).OnBerserkCheck();
                    break;
                case FWGlobals.SM_302_COMBAT_FLURRY:
                    ((FWScriptable)script).OnCombatFlurry();
                    break;
                default:
                    base.RunMessage(script, msg, io);
                    break;
            }
        }
        public override void SetScriptTimer(int index, ScriptTimer timer)
        {
            scriptTimers[index] = timer;
        }
    }
}