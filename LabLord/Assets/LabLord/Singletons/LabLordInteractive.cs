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
        /// <summary>
        /// the stored Id of the player object.
        /// </summary>
        public int PlayerId { get; private set; }
        /// <summary>
        /// the list of <see cref="LabLordInteractiveObject"/>s.
        /// </summary>
        private LabLordInteractiveObject[] objs = new LabLordInteractiveObject[0];
        public LabLordInteractive ()
        {
            PlayerId = -1;
        }
        public static void Init()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "LabLordInteractive"
                };
                Instance = go.AddComponent<LabLordInteractive>();
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
                objs = ArrayUtilities.Instance.ExtendArray((LabLordInteractiveObject)io, objs);
            }
            else
            {
                objs[index] = (LabLordInteractiveObject)io;
            }
        }
        public LabLordInteractiveObject GetPlayerIO()
        {
            return (LabLordInteractiveObject)GetIO(PlayerId);
        }
        /// <summary>
        /// Gets a new Player IO.
        /// </summary>
        /// <returns><see cref="LabLordInteractiveObject"/></returns>
        public void NewHero(LabLordInteractiveObject io)
        {
            // register the IO
            NewIO(io);
            // add player flag and component
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new LabLordCharacter
            {
                Level = 1
            };
            io.PcData.SetBaseAttributeScore("AC", 10f);
            // add script
            Hero script = new Hero();
            io.Script = script;
            //script.Io = io;
            int val = Script.Instance.SendInitScriptEvent(io);
            // initialize inventory
            io.Inventory = new LabLordInventoryData();
            // register the IO as the player
            PlayerId = io.RefId;
        }
        /// <summary>
        /// Gets a new Item IO.
        /// </summary>
        /// <returns><see cref="LabLordInteractiveObject"/></returns>
        public void NewItem(LabLordInteractiveObject io, Scriptable script)
        {
            // register the IO
            NewIO(io);
            // add item flag and component
            io.AddIOFlag(IoGlobals.IO_02_ITEM);
            io.ItemData = new LabLordItemData();
            // add script
            io.Script = script;
            //script.Io = io;
            int val = Script.Instance.SendInitScriptEvent(io);
        }
        /// <summary>
        /// Initializes a new Mob IO.
        /// </summary>
        public void NewMob(LabLordInteractiveObject io, Scriptable script)
        {
            /*
            // register the IO
            NewIO(io);
            // add player flag and component
            io.AddIOFlag(IoGlobals.IO_03_NPC);
            io.NpcData = new LabLordNPC();
            // add script
            io.Script = script;
            //script.Io = io;
            int val = Script.Instance.SendInitScriptEvent(io);
            */
        }
    }
}