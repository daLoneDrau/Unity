using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Graph;
using RPGBase.Singletons;
using UnityEngine;
using WoFM.Constants;
using WoFM.Flyweights.Actions;
using WoFM.Singletons;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace WoFM.Flyweights
{
    public class WoFMScriptable : Scriptable
    {
        public virtual int OnBashed()
        {
            WoFMInteractiveObject playerIo = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            Transform doorToBash = Io.transform;

            int dir;
            Vector2 backUpPos;
            // get the direction of the BASH
            if (playerIo.gameObject.transform.position.x < doorToBash.position.x)
            {
                Debug.Log("bash is to east");
                dir = WoFMGlobals.EAST;
                backUpPos = playerIo.LastPositionHeld + WoFMGlobals.DIRECTIONS[WoFMGlobals.WEST];
            }
            else if (playerIo.gameObject.transform.position.x > doorToBash.position.x)
            {
                Debug.Log("bash is to west");
                dir = WoFMGlobals.WEST;
                backUpPos = playerIo.LastPositionHeld + WoFMGlobals.DIRECTIONS[WoFMGlobals.EAST];
            }
            else if (playerIo.gameObject.transform.position.y < doorToBash.position.y)
            {
                Debug.Log("bash is to north");
                dir = WoFMGlobals.NORTH;
                backUpPos = playerIo.LastPositionHeld + WoFMGlobals.DIRECTIONS[WoFMGlobals.SOUTH];
            }
            else
            {
                Debug.Log("bash is to south");
                dir = WoFMGlobals.SOUTH;
                backUpPos = playerIo.LastPositionHeld + WoFMGlobals.DIRECTIONS[WoFMGlobals.NORTH];
            }
            // add walk action for backing up
            WoFMTile tile = WorldController.Instance.GetTileAt(backUpPos);
            if (tile != null)
            {

                WeightedGraphEdge[] path = WorldController.Instance.GetLandPath(playerIo.LastPositionHeld, backUpPos);
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    // get tile destination
                    Vector2 node = WorldController.Instance.GetNodeCoordinatesFromId(path[i].To);
                    GameSceneController.Instance.AddMustCompleteAction(new MoveIoUninterruptedAction(((WoFMInteractive)Interactive.Instance).GetPlayerIO(), node, 0f));
                }
            }
            else
            {
                backUpPos = playerIo.LastPositionHeld;
            }
            SetLocalVariable("backUpPos_x", backUpPos.x);
            SetLocalVariable("backUpPos_y", backUpPos.y);
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnEnterTrigger()
        {
            return ScriptConsts.ACCEPT;
        }
        public virtual int OnRollStats()
        {
            return ScriptConsts.ACCEPT;
        }
    }
}