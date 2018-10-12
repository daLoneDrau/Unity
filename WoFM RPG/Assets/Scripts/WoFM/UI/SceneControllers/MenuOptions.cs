using RPGBase.Graph;
using RPGBase.Pooled;
using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WoFM.Constants;
using WoFM.Flyweights;
using WoFM.Flyweights.Actions;
using WoFM.Singletons;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.SceneControllers
{
    public class MenuOptions : Singleton<MenuOptions>
    {
        public Button AttackButton;
        public Button BashButton;
        public Button LookButton;
        private struct DirectionData
        {
            public Vector2 position;
            public int opposite;
        }
        /// <summary>
        /// the id of the last room the player entered.
        /// </summary>
        public int LastRoomEntered;
        /// <summary>
        /// Checks to see which options should be enabled.
        /// </summary>
        public void CheckOptions()
        {
            if (GameSceneController.Instance.CONTROLS_FROZEN)
            {
                LookButton.interactable = false;
                BashButton.interactable = false;
                AttackButton.interactable = false;
            }
            else
            {
                LookButton.interactable = true;
                BashButton.interactable = CheckBash();
                AttackButton.interactable = false;

            }
        }
        #region BASH UTILITIES
        /// <summary>
        /// Determines if a specific location contains a door that can be bashed.
        /// </summary>
        /// <param name="pos">the position</param>
        /// <returns>true if the location has a bashable door; false otherwise</returns>
        private bool BashableDoorAtPosition(Vector2 pos)
        {
            bool can = false;
            foreach (Transform child in GameController.Instance.doorHolder)
            {
                //child is your child transform
                WoFMInteractiveObject io = child.gameObject.GetComponent<WoFMInteractiveObject>();
                float iox = io.Script.GetLocalFloatVariableValue("x"), ioy = io.Script.GetLocalFloatVariableValue("y");
                if (pos == new Vector2(iox, ioy)
                    && io.Script.GetLocalIntVariableValue("bashable") == 1)
                {
                    can = true;
                    break;
                }
            }
            return can;
        }
        /// <summary>
        /// Checks to see if the Bash button should be enabled.
        /// </summary>
        /// <returns>if true, the bash button can be enabled; false otherwise</returns>
        private bool CheckBash()
        {
            bool can = false;
            // get player's position
            WoFMInteractiveObject io = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            // check all directions
            for (int i = WoFMGlobals.DIRECTIONS.Length - 1; i >= 0; i--)
            {
                if (BashableDoorAtPosition(io.LastPositionHeld + WoFMGlobals.DIRECTIONS[i]))
                {
                    can = true;
                    break;
                }
            }
            return can;
        }
        /// <summary>
        /// Gets the door that is going to be bashed.
        /// </summary>
        /// <returns><see cref="Transform"/></returns>
        private Transform GetDoorToBash()
        {
            Transform door = null;
            // get player's position
            WoFMInteractiveObject io = ((WoFMInteractive)Interactive.Instance).GetPlayerIO();
            // check all directions
            for (int i = WoFMGlobals.DIRECTIONS.Length - 1; i >= 0; i--)
            {
                foreach (Transform child in GameController.Instance.doorHolder)
                {
                    //child is your child transform
                    WoFMInteractiveObject dio = child.gameObject.GetComponent<WoFMInteractiveObject>();
                    float diox = dio.Script.GetLocalFloatVariableValue("x"), dioy = dio.Script.GetLocalFloatVariableValue("y");
                    if (io.LastPositionHeld + WoFMGlobals.DIRECTIONS[i] == new Vector2(diox, dioy)
                        && dio.Script.GetLocalIntVariableValue("bashable") == 1)
                    {
                        door = child;
                        break;
                    }
                }
            }
            return door;
        }
        #endregion
        #region MENU ACTIONS
        public void PressBash()
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(BashButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
        }
        public void PressLook()
        {
            LookButton.animator.SetTrigger(LookButton.animationTriggers.pressedTrigger);
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(LookButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
            LookButton.animator.SetTrigger(LookButton.animationTriggers.normalTrigger);
        }
        /// <summary>
        /// Performs the BASH action.
        /// </summary>
        public void Bash()
        {
            if (BashButton.interactable == true)
            {
                // send event to door being bashed
                Script.Instance.SendIOScriptEvent(
                    GetDoorToBash().GetComponent<WoFMInteractiveObject>(),
                    WoFMGlobals.SM_303_BASHED,
                    null,
                    null
                    );
            }
            GameSceneController.Instance.CONTROLS_FROZEN = false;
        }
        /// <summary>
        /// Performs the LOOK action.
        /// </summary>
        public void Look()
        {
            if (LookButton.interactable == true)
            {
                PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                sb.Append(LastRoomEntered);
                sb.Append("_SECONDARY");
                Messages.Instance.SendMessage(GameController.Instance.GetText(sb.ToString()));
                sb.ReturnToPool();
            }
            GameSceneController.Instance.CONTROLS_FROZEN = false;
        }
        #endregion
    }
}
