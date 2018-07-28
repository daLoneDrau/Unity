using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.Scriptables.Mobs;

namespace WoFM.Singletons
{
    public class WoFMInteractive: Interactive
    {
        /// <summary>
        /// the next available id.
        /// </summary>
        private int nextId;
        private int playerId;
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

        protected override BaseInteractiveObject GetNewIO()
        {
            // step 1 - find the next id
            int id = nextId++;
            WoFMInteractiveObject io = new WoFMInteractiveObject(id);
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
                objs = ArrayUtilities.Instance.ExtendArray(io, objs);
            }
            else
            {
                objs[index] = io;
            }
            return io;
        }
        public WoFMInteractiveObject GetPlayerIO()
        {
            return (WoFMInteractiveObject)GetIO(playerId);
        }
        /// <summary>
        /// Gets a new Player IO.
        /// </summary>
        /// <returns><see cref="WoFMInteractiveObject"/></returns>
        public WoFMInteractiveObject NewHero()
        {
            print("NewHero");
            WoFMInteractiveObject io = (WoFMInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new WoFMCharacter();
            io.Script = new Hero();
            int val = Script.Instance.SendInitScriptEvent(io);
            playerId = io.RefId;
            return io;
        }
        /// <summary>
        /// Gets a new Item IO.
        /// </summary>
        /// <returns><see cref="WoFMInteractiveObject"/></returns>
        public WoFMInteractiveObject NewItem()
        {
            WoFMInteractiveObject io = (WoFMInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_02_ITEM);
            io.ItemData = new WoFMItemData();
            return io;
        }
    }
}