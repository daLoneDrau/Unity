using Assets.Scripts.FantasyWargaming.Flyweights;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.FantasyWargaming.Singletons
{
    public class FWInteractive : Interactive
    {
        /// <summary>
        /// the next available id.
        /// </summary>
        private int nextId;
        private int playerId;
        /// <summary>
        /// the list of <see cref="BPInteractiveObject"/>s.
        /// </summary>
        private FWInteractiveObject[] objs = new FWInteractiveObject[0];
        public FWInteractive()
        {
        }
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "FWInteractive"
                };
                Instance = go.AddComponent<FWInteractive>();
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
            FWInteractiveObject io = new FWInteractiveObject(id);
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
        public FWInteractiveObject GetPlayerIO()
        {
            return (FWInteractiveObject)GetIO(playerId);
        }
        /// <summary>
        /// Gets a new Player IO.
        /// </summary>
        /// <returns><see cref="BPInteractiveObject"/></returns>
        public FWInteractiveObject NewHero()
        {
            print("NewHero");
            FWInteractiveObject io = (FWInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new FWCharacter();
            //io.Script = new CalArath();
            //int val = Script.Instance.SendInitScriptEvent(io);
            playerId = io.RefId;
            return io;
        }
        /// <summary>
        /// Gets a new Item IO.
        /// </summary>
        /// <returns><see cref="BPInteractiveObject"/></returns>
        public FWInteractiveObject NewItem()
        {
            FWInteractiveObject io = (FWInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_02_ITEM);
            io.ItemData = new FWItemData();
            return io;
        }
    }
}
