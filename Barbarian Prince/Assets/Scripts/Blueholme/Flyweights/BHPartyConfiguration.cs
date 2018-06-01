using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Flyweights
{
    /// <summary>
    /// Class defining an encounter party's 
    /// </summary>
    public class BHPartyConfiguration
    {
        /// <summary>
        /// the front left position.
        /// </summary>
        public const int POSITION_FRONT_LEFT = 0;
        /// <summary>
        /// the front center position.
        /// </summary>
        public const int POSITION_FRONT_CENTER = 1;
        /// <summary>
        /// the front right position.
        /// </summary>
        public const int POSITION_FRONT_RIGHT = 2;
        /// <summary>
        /// the middle left position.
        /// </summary>
        public const int POSITION_MIDDLE_LEFT = 3;
        /// <summary>
        /// the middle center position.
        /// </summary>
        public const int POSITION_MIDDLE_CENTER = 4;
        /// <summary>
        /// the middle right position.
        /// </summary>
        public const int POSITION_MIDDLE_RIGHT = 5;
        /// <summary>
        /// the rear left position.
        /// </summary>
        public const int POSITION_REAR_LEFT = 6;
        /// <summary>
        /// the rear center position.
        /// </summary>
        public const int POSITION_REAR_CENTER = 7;
        /// <summary>
        /// the rear right position.
        /// </summary>
        public const int POSITION_REAR_RIGHT = 8;
        /// <summary>
        /// the matrix of positions in the encounter party - 3 rows of 3 positions. Row 1 (positions 0-2) is the row closest to the enemy. Each subsequent row is considered 10' away.
        /// </summary>
        private int[] positions = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        /// <summary>
        /// Assigns an IO to occupy a specific position.
        /// </summary>
        /// <param name="position">the position number</param>
        /// <param name="refId">the IO's reference id</param>
        public void AssignToPosition(int position, int refId)
        {
            if (position < POSITION_FRONT_LEFT
                || position > POSITION_REAR_RIGHT)
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "Invalid position");
            }
            if (positions[position] >= 0
                && Interactive.Instance.HasIO(positions[position]))
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "Another IO is already in that position");
            }
            if (!Interactive.Instance.HasIO(refId))
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO does not exist");
            }
            if (!Interactive.Instance.GetIO(refId).HasIOFlag(IoGlobals.IO_01_PC)
                && !Interactive.Instance.GetIO(refId).HasIOFlag(IoGlobals.IO_03_NPC))
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO is not a PC or NPC");
            }
            positions[position] = refId;
        }
        /// <summary>
        /// Assigns an IO to occupy a specific position.
        /// </summary>
        /// <param name="position">the position number</param>
        /// <param name="io">the <see cref="BHInteractiveObject"/></param>
        public void AssignToPosition(int position, BHInteractiveObject io)
        {
            if (io == null)
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO is null");
            }
            AssignToPosition(position, io.RefId);
        }
        /// <summary>
        /// Gets an IO's position.
        /// </summary>
        /// <param name="io">the <see cref="BHInteractiveObject"/></param>
        /// <returns></returns>
        public int GetIoPosition(BHInteractiveObject io)
        {
            if (io == null)
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO is null");
            }
            return GetIoPosition(io.RefId);
        }
        public int GetIoPosition(int refId)
        {
            if (!Interactive.Instance.HasIO(refId))
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "IO does not exist");
            }
            int position = -1;
            for (int i = positions.Length - 1; i >= 0; i--)
            {
                if (positions[i] == refId)
                {
                    position = i;
                    break;
                }
            }
            return position;
        }
        /// <summary>
        /// Switches two IO's positions.
        /// </summary>
        /// <param name="io0">the first <see cref="BHInteractiveObject"/></param>
        /// <param name="io1">the second <see cref="BHInteractiveObject"/></param>
        public void SwitchPositions(BHInteractiveObject io0, BHInteractiveObject io1)
        {
            int position0 = this.GetIoPosition(io0), position1 = this.GetIoPosition(io1);
            if (position0 == -1)
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "Io0 has no position");
            }
            if (position1 == -1)
            {
                throw new RPGException(ErrorMessage.INVALID_OPERATION, "Io0 has no position");
            }
            positions[position0] = io1.RefId;
            positions[position1] = io0.RefId;
        }

        public int GetIoAt(int i)
        {
            if (i < 0
                || i > POSITION_REAR_RIGHT)
            {

                throw new RPGException(ErrorMessage.INVALID_OPERATION, "Invalid position");
            }
            return positions[i];
        }
    }
}
