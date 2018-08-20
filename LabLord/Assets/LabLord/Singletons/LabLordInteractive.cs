using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using UnityEngine;
using LabLord.Flyweights;
using LabLord.Scriptables.Mobs;

namespace LabLord.Singletons
{
    public class LabLordInteractive : Interactive
    {
        /// <summary>
        /// the next available id.
        /// </summary>
        private int nextId;
        private int playerId;
        /// <summary>
        /// the list of <see cref="LabLordInteractiveObject"/>s.
        /// </summary>
        private LabLordInteractiveObject[] objs = new LabLordInteractiveObject[0];
        public LabLordInteractive ()
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
                Instance = go.AddComponent<LabLordInteractive >();
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
            LabLordInteractiveObject io = new LabLordInteractiveObject(id);
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
        public LabLordInteractiveObject GetPlayerIO()
        {
            return (LabLordInteractiveObject)GetIO(playerId);
        }
        /// <summary>
        /// Gets a new Player IO.
        /// </summary>
        /// <returns><see cref="LabLordInteractiveObject"/></returns>
        public LabLordInteractiveObject NewHero()
        {
            print("NewHero");
            LabLordInteractiveObject io = (LabLordInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new LabLordCharacter();
            io.Script = new Hero();
            int val = Script.Instance.SendInitScriptEvent(io);
            playerId = io.RefId;
            return io;
        }
        /// <summary>
        /// Gets a new Item IO.
        /// </summary>
        /// <returns><see cref="LabLordInteractiveObject"/></returns>
        public LabLordInteractiveObject NewItem()
        {
            LabLordInteractiveObject io = (LabLordInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_02_ITEM);
            io.ItemData = new LabLordItemData();
            return io;
        }
    }
}