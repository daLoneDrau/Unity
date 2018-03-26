using Assets.Scripts.BarbarianPrince.Constants;
using Assets.Scripts.BarbarianPrince.Flyweights;
using Assets.Scripts.BarbarianPrince.Scriptables.Mobs;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.Singletons
{
    public class BPInteractive : Interactive
    {
        /// <summary>
        /// the next available id.
        /// </summary>
        private int nextId;
        /// <summary>
        /// the list of <see cref="BPInteractiveObject"/>s.
        /// </summary>
        private BPInteractiveObject[] objs = new BPInteractiveObject[0];
        public BPInteractive() {
        }
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "BPInteractive"
                };
                Instance = go.AddComponent<BPInteractive>();
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
            BPInteractiveObject io = new BPInteractiveObject(id);
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
        /// <summary>
        /// Gets a new Player IO.
        /// </summary>
        /// <returns><see cref="BPInteractiveObject"/></returns>
        public BPInteractiveObject NewHero()
        {
            print("NewHero");
            BPInteractiveObject io = (BPInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new BPCharacter();
            io.Script = new CalArath();
            int val = Script.Instance.SendInitScriptEvent(io);
            return io;
        }
        /// <summary>
        /// Gets a new Item IO.
        /// </summary>
        /// <returns><see cref="BPInteractiveObject"/></returns>
        public BPInteractiveObject NewItem()
        {
            BPInteractiveObject io = (BPInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_02_ITEM);
            io.ItemData = new BPItemData();
            return io;
        }
    }
}
