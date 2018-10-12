using RPGBase.Scripts.UI._2D;
using RPGBase.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.Flyweights.Actions;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace WoFM.UI._2D
{
    public class HeroMove: MovingObject
    {
        /// <summary>
        /// flag indicating a move that hit an object should actually be allowed.
        /// </summary>
        private bool allowMove;
        protected override void Start()
        {
            base.moveTime = .5f;
            base.Start();
        }
        public override void AttemptMove<T>(int xDir, int yDir)
        {
            allowMove = false;
            print("attempt move");
            // hit is an out parameter - we store the hit result in this instance for use later
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);
            if (hit.transform == null)
            {
                // calculate end position based on directions passed in when calling move
                GameSceneController.Instance.AddMustCompleteAction(new MoveIoUninterruptedAction(
                    GetComponent<WoFMInteractiveObject>(),
                    GetComponent<WoFMInteractiveObject>().LastPositionHeld + new Vector2(xDir, yDir),
                    0));
            }
            else
            {
                // get component that was hit.  could be enemies, players, triggers, etc...
                T hitComponent = hit.transform.GetComponent<T>();
                // if unit cannot move and hit something
                if (!canMove && hitComponent != null)
                {
                    OnCantMove(hitComponent);
                    if (allowMove)
                    {
                        // move is actually allowed
                        // calculate end position based on directions passed in when calling move
                        GameSceneController.Instance.AddMustCompleteAction(new MoveIoUninterruptedAction(
                            GetComponent<WoFMInteractiveObject>(),
                            GetComponent<WoFMInteractiveObject>().LastPositionHeld + new Vector2(xDir, yDir),
                            0));
                    }
                }
                else
                {
                    // didn't hit something, but can't move.  what do we do?
                }
            }
        }
        /// <summary>
        /// Moves the player to a destination source without checking for collisions.
        /// </summary>
        /// <param name="dest">the destination coordinates</param>
        public void MoveUninterrupted(Vector2 dest, float wait = 0f)
        {
            StartCoroutine(MoveToTile(dest, wait));
        }
        /// <summary>
        /// Moves the player to a destination source without checking for collisions.
        /// </summary>
        /// <param name="dest">the destination coordinates</param>
        public void MoveFast(Vector2 dest, float speed)
        {
            StartCoroutine(MoveFastToTile(dest, speed));
        }
        protected override void OnCantMove<T>(T component)
        {
            if (component is Blocker)
            {
                var bl = component as Blocker;
                switch (bl.Type)
                {
                    case Blocker.WALL:
                        print("ran into a wall");
                        // turn on *BONK*
                        Particles.Instance.PlayBonk(transform.position);
                        // un-freeze player controls
                        GameSceneController.Instance.CONTROLS_FROZEN = false;
                        break;
                    case Blocker.DOOR:
                        print("ran into a door");
                        WoFMInteractiveObject dio = component.GetComponent<WoFMInteractiveObject>();
                        if (dio.Script.GetLocalIntVariableValue("locked") == 1)
                        {
                            // ran into a locked door
                            // TODO - check for key
                            // turn on *LOCKED*
                            Particles.Instance.PlayLocked(bl.transform.position);
                            // un-freeze player controls
                            GameSceneController.Instance.CONTROLS_FROZEN = false;
                        }
                        else
                        {
                            // ran into unlocked door - change door's sprite and blocking layer
                            SpriteRenderer sr = component.GetComponent<SpriteRenderer>();
                            sr.sprite = SpriteMap.Instance.GetSprite("door_1");
                            sr.sortingLayerName = "Items";
                            component.gameObject.layer = LayerMask.NameToLayer("Floor");
                            dio.Script.SetLocalVariable("open", 1);
                            allowMove = true;
                        }
                        break;
                    default:
                        print("ran into something unknown");
                        break;
                }
            }
        }
    }
}
