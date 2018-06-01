using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Flyweights
{
    public class BHParty
    {
        /// <summary>
        /// the list of reference ids of all party members.
        /// </summary>
        private int[] members = new int[0];
        public int this[int index]
        {
            get
            {
                return members[index];
            }
            set
            {
                members[index] = value;
            }
        }
        public int Size { get { return members.Length; } }
        /// <summary>
        /// the party configuration.
        /// </summary>
        private BHPartyConfiguration configuration = new BHPartyConfiguration();
        /// <summary>
        /// the party configuration property.
        /// </summary>
        public BHPartyConfiguration Configuration { get { return configuration; } }
        /// <summary>
        /// Adds an IO to the party.
        /// </summary>
        /// <param name="io">the <see cref="BaseInteractiveObject"/></param>
        public void Add(BaseInteractiveObject io)
        {
            if (io == null)
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO is null");
            }
            Add(io.RefId);
        }
        /// <summary>
        /// Adds an IO to the party.
        /// </summary>
        /// <param name="refId">the IO's reference id</param>
        public void Add(int refId)
        {
            if (!Interactive.Instance.HasIO(refId))
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO does not exist");
            }
            if (!Interactive.Instance.GetIO(refId).HasIOFlag(IoGlobals.IO_01_PC)
                && !Interactive.Instance.GetIO(refId).HasIOFlag(IoGlobals.IO_03_NPC))
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO is not a PC or NPC");
            }
            int index = -1;
            if (!IsInParty(refId))
            {
                for (int i = members.Length - 1; i >= 0; i--)
                {
                    if (members[i] == -1)
                    {
                        index = i;
                        break;
                    }
                }
            }
            if (index >= 0)
            {
                members[index] = refId;
            }
            else
            {
                members = ArrayUtilities.Instance.ExtendArray(refId, members);
            }
        }
        /// <summary>
        /// Determines if an IO is in the party.
        /// </summary>
        /// <param name="io">the <see cref="BaseInteractiveObject"/></param>
        /// <returns></returns>
        public bool IsInParty(BaseInteractiveObject io)
        {
            if (io == null)
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO is null");
            }
            return IsInParty(io.RefId);
        }
        /// <summary>
        /// Determines if an IO is in the party.
        /// </summary>
        /// <param name="refId">the IO's reference id</param>
        /// <returns></returns>
        public bool IsInParty(int refId)
        {
            if (!Interactive.Instance.HasIO(refId))
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO does not exist");
            }
            bool f = false;
            for (int i = members.Length - 1; i >= 0; i--)
            {
                if (members[i] == refId)
                {
                    f = true;
                    break;
                }
            }
            return f;
        }
    }
}
