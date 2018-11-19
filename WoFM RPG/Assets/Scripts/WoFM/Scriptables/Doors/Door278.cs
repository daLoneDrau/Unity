using RPGBase.Constants;
using RPGBase.Graph;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.Flyweights.Actions;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;
using WoFM.UI.Widgets;

namespace WoFM.Scriptables.Doors
{
    public class Door278 : WoFMScriptable
    {
        public override int OnBashed()
        {
            // call base for backing up
            base.OnBashed();
            Vector2 backUpPos = new Vector2(GetLocalFloatVariableValue("backUpPos_x"), GetLocalFloatVariableValue("backUpPos_y"));
            float x = GetLocalFloatVariableValue("x");
            float y = GetLocalFloatVariableValue("y");
            WeightedGraphEdge[] path = WorldController.Instance.GetLandPath(backUpPos, new Vector2(x, y));
            // test player's skill
            WoFMInteractiveObject playerIo = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            if (((WoFMCharacter)playerIo.PcData).TestSkill())
                {
                // reset door's values
                // disable attached boxcollider so when casting rays you dont hit your own unit's collider
                Io.GetComponent<BoxCollider2D>().enabled = false;
                // change object's sorting layer
                Io.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
                Io.gameObject.layer = LayerMask.NameToLayer("Floor");
                // run through door
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    Vector2 node = WorldController.Instance.GetNodeCoordinatesFromId(path[i].To);
                    GameSceneController.Instance.AddMustCompleteAction(new MoveIoSpeedyAction(playerIo, node));
                }
                // add action to remove door from game
                GameSceneController.Instance.AddMustCompleteAction(new DestroyIOAction(Io.RefId));
                // add action to run into room
                GameSceneController.Instance.AddMustCompleteAction(new MoveIoSpeedyAction(playerIo, new Vector2(x + 1, y)));
                // add action to display message
                GameSceneController.Instance.AddMustCompleteAction(new MessageAction(GameController.Instance.GetText("278_BASH_SUCCESS"), Messages.WARN));
                // show modal
                GameSceneController.Instance.AddMustCompleteAction(new ModalAction(new ModalPanelDetails()
                {
                    content = GameController.Instance.GetText("278_BASH_SUCCESS"),
                    iconImage = SpriteMap.Instance.GetSprite("icon_falling"),
                    button1Details = new EventButtonDetails()
                    {
                        buttonTitle = "Aaaah!"
                    }
                }));
                // add damage action
                GameSceneController.Instance.AddMustCompleteAction(new DamageIOAction(playerIo.RefId, 1, -1, -1));
                // change room 'Look' text
                GameController.Instance.SetText("278_SECONDARY", GameController.Instance.GetText("278_TERTIARY"));
            }
            else
            {
                Debug.Log("unsuccessful bash");
                // add run to just before door
                for (int i = path.Length - 1; i > 0; i--)
                {
                    Vector2 node = WorldController.Instance.GetNodeCoordinatesFromId(path[i].To);
                    GameSceneController.Instance.AddMustCompleteAction(new MoveIoSpeedyAction(playerIo, node));
                }
                // add action to play bonk
                GameSceneController.Instance.AddMustCompleteAction(new ParticleAction(Particles.Instance.GetType().GetMethod("PlayBonkAboveIo"), playerIo.RefId));
                // wait for a second for bonk to finish
                GameSceneController.Instance.AddMustCompleteAction(new WaitAction(1f));
                // add action to display message
                GameSceneController.Instance.AddMustCompleteAction(new MessageAction(GameController.Instance.GetText("278_BASH_FAILURE"), Messages.INFO));
                // show modal
                GameSceneController.Instance.AddMustCompleteAction(new ModalAction(new ModalPanelDetails()
                {
                    content = GameController.Instance.GetText("278_BASH_FAILURE"),
                    iconImage = SpriteMap.Instance.GetSprite("icon_door_in"),
                    button1Details = new EventButtonDetails()
                    {
                        buttonTitle = "...Stupid Door"
                    }
                }));
            }

            return ScriptConsts.ACCEPT;
        }
    }
}
