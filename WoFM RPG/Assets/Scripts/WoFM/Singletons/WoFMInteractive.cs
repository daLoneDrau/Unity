using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.Scriptables.Mobs;

namespace WoFM.Singletons
{
    public class WoFMInteractive : Interactive
    {
        /// <summary>
        /// the next available id.
        /// </summary>
        private int nextId;
        /// <summary>
        /// the stored Id of the player object.
        /// </summary>
        private int playerId = -1;
        /// <summary>
        /// the list of <see cref="WoFMInteractiveObject"/>s.
        /// </summary>
        private WoFMInteractiveObject[] objs = new WoFMInteractiveObject[0];
        public WoFMInteractive()
        {
        }
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "WoFMInteractive"
                };
                Instance = go.AddComponent<WoFMInteractive>();
                DontDestroyOnLoad(go);
            }
        }
        public override void ForceIOLeaveZone(BaseInteractiveObject io, long flags)
        {
            throw new NotImplementedException();
        }

        public override int GetMaxIORefId()
        {
            return nextId;
        }

        protected override BaseInteractiveObject[] GetIOs()
        {
            return objs;
        }

        protected override void NewIO(BaseInteractiveObject io)
        {
            // step 1 - find the next id
            io.RefId = nextId++;
            // step 2 - find the next available index in the objs array
            int index = -1;
            for (int i = objs.Length - 1; i >= 0; i--)
            {
                if (objs[i] == null)
                {
                    index = i;
                    break;
                }
            }
            // step 3 - put the new object into the arrays
            if (index < 0)
            {
                objs = ArrayUtilities.Instance.ExtendArray((WoFMInteractiveObject)io, objs);
            }
            else
            {
                objs[index] = (WoFMInteractiveObject)io;
            }
        }
        public WoFMInteractiveObject GetPlayerIO()
        {
            return (WoFMInteractiveObject)GetIO(playerId);
        }
        /// <summary>
        /// Initializes a new Player IO.
        /// </summary>
        public void NewHero(WoFMInteractiveObject io)
        {
            // register the IO
            NewIO(io);
            // add player flag and component
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new WoFMCharacter();
            // add script
            io.Script = new Hero();
            int val = Script.Instance.SendInitScriptEvent(io);
            // register the IO as the player
            playerId = io.RefId;
        }
        /// <summary>
        /// Initializes a new Trigger IO.
        /// </summary>
        public void NewTrigger(WoFMInteractiveObject io, string script)
        {
            // register the IO
            NewIO(io);
            // add trigger flag
            io.AddIOFlag(IoGlobals.IO_16_TRIGGER);
            // add script
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("WoFM.Scriptables.Triggers.");
            sb.Append(script);
            Type type = Type.GetType(sb.ToString());
            sb.ReturnToPool();
            io.Script = (Scriptable)Activator.CreateInstance(type);
            int val = Script.Instance.SendInitScriptEvent(io);
        }
        /// <summary>
        /// Initializes a new Item IO.
        /// </summary>
        public void NewItem(BaseInteractiveObject io)
        {
            // register the IO
            NewIO(io);
            // add item flag and component
            io.AddIOFlag(IoGlobals.IO_02_ITEM);
            io.ItemData = new WoFMItemData();
        }
    }
}