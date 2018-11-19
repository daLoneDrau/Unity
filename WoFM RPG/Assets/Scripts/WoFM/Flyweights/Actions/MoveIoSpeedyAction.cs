using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace WoFM.Flyweights.Actions
{
    public class MoveIoSpeedyAction : IGameAction
    {
        /// <summary>
        /// the IO's destination.
        /// </summary>
        private Vector2 destination;
        /// <summary>
        /// flag indicating the execution has started.
        /// </summary>
        private bool executionStarted = false;
        /// <summary>
        /// the IO being moved.
        /// </summary>
        private WoFMInteractiveObject io;
        private float speedTime;
        /// <summary>
        /// Creates a new instance of <see cref="MoveIoSpeedyAction"/>.
        /// </summary>
        /// <param name="i">the IO being moved</param>
        /// <param name="v">the IO's destination</param>
        public MoveIoSpeedyAction(WoFMInteractiveObject i, Vector2 v, float speed = .1f)
        {
            io = i;
            destination = v;
            speedTime = speed;
        }
        public void Execute()
        {
            if (!executionStarted)
            {
                Debug.Log("exceute move to " + destination);
                if (io.HasIOFlag(IoGlobals.IO_01_PC))
                {
                    io.gameObject.GetComponent<HeroMove>().MoveFast(destination, speedTime);
                }
                else if (io.HasIOFlag(IoGlobals.IO_03_NPC))
                {
                    io.gameObject.GetComponent<MobMove>().MoveFast(destination, speedTime);
                }
                executionStarted = true;
            }
        }
        public bool IsResolved()
        {
            bool resolved = false;
            if (executionStarted)
            {
                float sqrRemainingDistance = (io.Position - destination).sqrMagnitude;
                // check if movement has ended
                if (sqrRemainingDistance <= float.Epsilon)
                {
                    Debug.Log("resolved move to " + destination);
                    resolved = true;
                    io.LastPositionHeld = destination;
                    if (io.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        // if player's move resolved, unfreeze controls
                        GameSceneController.Instance.CONTROLS_FROZEN = false;
                    }
                    // notify watchers that IO has reached destination
                    GameSceneController.Instance.CheckIOMoveIntoTile(io, destination);
                }
            }
            return resolved;
        }
    }
}
