using RPGBase.Constants;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.UI._2D;

namespace WoFM.Flyweights.Actions
{
    public class MovePathAction:IGameAction
    {
        private WoFMInteractiveObject io;
        private Vector2[] path;
        public MovePathAction(WoFMInteractiveObject i, Vector2[] v)
        {
            io = i;
            path = v;
        }
        public void AddToPath(Vector2 node)
        {
            if (path == null)
            {
                path = new Vector2[0];
            }
            path = ArrayUtilities.Instance.ExtendArray(ViewportController.Instance.GetWorldCoordinatesForTile(node), path);
        }
        public void Execute()
        {
            // executes the action.
            // in this implementation, the object's movement is not paused, and movement occurs until completed.
            GameObject ioObj = io.gameObject;
            if (io.HasIOFlag(IoGlobals.IO_01_PC))
            {
                for (int i = 0; i < path.Length; i++)
                {
                    Debug.Log("move to " + path[i]);
                    ioObj.GetComponent<HeroMove>().MoveUninterrupted(path[i]);
                    path = ArrayUtilities.Instance.RemoveIndex(i, path);
                    i--;
                }
            }
        }
        public bool IsResolved()
        {
            return path.Length == 0;
        }
    }
}
