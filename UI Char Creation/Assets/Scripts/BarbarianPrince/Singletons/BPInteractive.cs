using Assets.Scripts.BarbarianPrince.Flyweights;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public BPInteractive()
        {
            if (Interactive.Instance == null)
            {
                Interactive.Instance = this;
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
        /**
         * Gets a new Player IO.
         * @return {@link FFInteractiveObject}
         * @throws RPGException
         */
        public BPInteractiveObject NewHero()
        {
            BPInteractiveObject io = (BPInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new BPCharacter();
            //io.PcData.NewHero();
            //((FFController)ProjectConstants.getInstance()).setPlayer(io.getRefId());
            //io.setScript(new Hero(io));
            //Script.getInstance().sendInitScriptEvent(io);
            return io;
        }
        /**
         * Gets a new Item IO.
         * @return {@link FFInteractiveObject}
         * @throws RPGException
         */
        public BPInteractiveObject NewItem()
        {
            BPInteractiveObject io = (BPInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_02_ITEM);
            io.ItemData = new BPItemData();
            return io;
        }
    }
}
