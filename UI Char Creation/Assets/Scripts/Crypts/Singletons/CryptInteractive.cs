using Assets.Scripts.Crypts.Flyweights;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Singletons
{
    public class CryptInteractive : Interactive
    {
        /** the next available id. */
        private int nextId;
        /** the list of {@link FFInteractiveObject}s. */
        private CryptInteractiveObject[] objs = new CryptInteractiveObject[0];
        public CryptInteractive()
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
            throw new NotImplementedException();
        }

        protected override BaseInteractiveObject GetNewIO()
        {
            // step 1 - find the next id
            int id = nextId++;
            CryptInteractiveObject io = new CryptInteractiveObject(id);
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
        public CryptInteractiveObject NewHero()
        {
            CryptInteractiveObject io = (CryptInteractiveObject)GetNewIO();
            io.AddIOFlag(IoGlobals.IO_01_PC);
            io.PcData = new CryptCharacter();
            //io.PcData.NewHero();
            //((FFController)ProjectConstants.getInstance()).setPlayer(io.getRefId());
            //io.setScript(new Hero(io));
            //Script.getInstance().sendInitScriptEvent(io);
            return io;
        }
    }
}
