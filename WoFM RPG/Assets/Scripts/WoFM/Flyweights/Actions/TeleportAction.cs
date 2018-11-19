using RPGBase.Constants;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;

namespace WoFM.Flyweights.Actions
{
    public class TeleportAction : IGameAction
    {
        /// <summary>
        /// the IO being teleported.
        /// </summary>
        private WoFMInteractiveObject io;
        /// <summary>
        /// the location to teleport to.
        /// </summary>
        private Vector2 location;
        /// <summary>
        /// Creates a new instance of <see cref="TeleportAction"/>
        /// </summary>
        /// <param name="i">the IO being teleported</param>
        /// <param name="v">the location to which the IO is being teleported</param>
        public TeleportAction(WoFMInteractiveObject i, Vector2 v)
        {
            io = i;
            location = v;
        }
        public void Execute()
        {
            if (io.HasIOFlag(IoGlobals.IO_01_PC))
            {
                // set IO's position to tile position
                io.Position = location;
                io.LastPositionHeld = location;
                // set Game Object's transform to correct viewport coordinates
                GameObject ioObj = io.gameObject;
                Vector2 v = ViewportController.Instance.GetWorldCoordinatesForTile(location);
                Debug.Log("teleport to " + v);
                ioObj.transform.position = new Vector3(v.x, v.y, 0);
                TileViewportController.Instance.CenterOnTile(location);
                //TileViewportController.Instance.CenterOnTile(new Vector2(28.9f, 16.9f));
            }
            else if (io.HasIOFlag(IoGlobals.IO_03_NPC))
            {
                // set IO's position to tile position
                io.Position = location;
                io.LastPositionHeld = location;
                // set script variables

                // set Game Object's transform to correct viewport coordinates
                GameObject ioObj = io.gameObject;
                Vector2 v = ViewportController.Instance.GetWorldCoordinatesForTile(location);
                Debug.Log("teleport to " + v);
                ioObj.transform.position = new Vector3(v.x, v.y, 0);
                TileViewportController.Instance.CenterOnTile(location);
            }
        }
        public bool IsResolved()
        {
            return io.Position == location;
        }
    }
}
