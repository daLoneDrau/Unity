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
    public class MoveIoUninterruptedAction : IGameAction
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
        private float waitTime;
        /// <summary>
        /// Creates a new instance of <see cref="MoveIoUninterruptedAction"/>.
        /// </summary>
        /// <param name="i">the IO being moved</param>
        /// <param name="v">the IO's destination</param>
        public MoveIoUninterruptedAction(WoFMInteractiveObject i, Vector2 v, float wait=.25f)
        {
            io = i;
            destination = v;
            waitTime = wait;
        }
        public void Execute()
        {
            if (!executionStarted)
            {
                Debug.Log("exceute move to " + destination);
                io.gameObject.GetComponent<HeroMove>().MoveUninterrupted(destination, waitTime);
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
