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

namespace WoFM.UI._2D
{
    public class HeroMove: MovingObject
    {
        public ParticleSystem bonker;
        protected override void Start()
        {
            base.moveTime = .5f;
            base.Start();
        }
        public override void AttemptMove<T>(int xDir, int yDir)
        {
            print("attempt move");
            // hit is an out parameter - we store the hit result in this instance for use later
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);
            if (hit.transform == null)
            {
                // move hero to the spot
                Vector2 end = GetComponent<WoFMInteractiveObject>().LastPositionHeld + new Vector2(xDir, yDir);
                print("hit nothing - move to " + end);
                // calculate end position based on directions passed in when calling move
                GameSceneController.Instance.AddMustCompleteAction(new MoveIoUninterruptedAction(GetComponent<WoFMInteractiveObject>(), end, 0));
            }
            else
            {
                // get component that was hit.  could be enemies, players, triggers, etc...
                T hitComponent = hit.transform.GetComponent<T>();
                // if unit cannot move and hit something
                if (!canMove && hitComponent != null)
                {
                    OnCantMove(hitComponent);
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
        public void MoveUninterrupted(Vector2 dest, float wait=0f)
        {
            StartCoroutine(MoveToTile(dest, wait));
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
                        bonker.transform.position = transform.position + new Vector3(0, .625f, 0);
                        bonker.Play();
                        StartCoroutine(FinishBonk());
                        // un-freeze player controls
                        GameSceneController.Instance.CONTROLS_FROZEN = false;
                        break;
                }
            }
        }
        /// <summary>
        /// Co-routine to move the *BONK* particle animator off-screen once it is finished.
        /// </summary>
        /// <returns></returns>
        public IEnumerator FinishBonk()
        {
            // loop until particle is gone
            while (bonker.IsAlive(true))
            {
                yield return null;
            }
            // move back off-screen
            bonker.transform.position = new Vector3(-1, 0, 0);
        }
    }
}
