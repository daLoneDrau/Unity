using RPGBase.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.Flyweights;
using WoFM.UI._2D;
using WoFM.UI.GlobalControllers;
using WoFM.UI.SceneControllers;

namespace RPGBase.Scripts.UI._2D
{
    public abstract class MovingObject : MonoBehaviour
    {
        /// <summary>
        /// the time it will take the object to perform a move, in seconds.
        /// </summary>
        public float moveTime = .1f;
        /// <summary>
        /// the layer on which we will check for collisions to see if a space is open to move into.
        /// </summary>
        public LayerMask blockingLayer;
        /// <summary>
        /// stores a reference to the <see cref="BoxCollider2D"/> component of the unit being moved.
        /// </summary>
        private BoxCollider2D boxCollider;
        /// <summary>
        /// stores a reference to the <see cref="Rigidbody2D"/> component of the unit being moved.
        /// </summary>
        private Rigidbody2D rigidBody;
        /// <summary>
        /// used to make movement calculations more efficient.
        /// </summary>
        private float inverseMoveTime;
        #region MonoBehaviour messages
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        protected virtual void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            rigidBody = GetComponent<Rigidbody2D>();
            // store reciprical of moveTime to use multiplaction instead of division
            inverseMoveTime = 1f / moveTime;
        }
        #endregion
        public virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
        {
            // hit is an out parameter - we store the hit result in this instance for use later
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);
            if (hit.transform == null)
            {
                return;
            }
            // get component that was hit.  could be enemies, players, triggers, etc...
            T hitComponent = hit.transform.GetComponent<T>();
            // if unit cannot move and hit something
            if (!canMove && hitComponent != null)
            {
                OnCantMove(hitComponent);
            }
        }
        public IEnumerator MoveToTile(Vector2 tileCoords, float pauseTime = 0f)
        {
            // pause before moving, if needed
            if (pauseTime > 0f)
            {
                yield return new WaitForSeconds(pauseTime);
            }
            WoFMInteractiveObject io = GetComponent<WoFMInteractiveObject>();
            if (io.HasIOFlag(IoGlobals.IO_01_PC))
            {
                // calculate remaining distance to move based on the square magnitude of the difference between the current position and the end parameter
                float sqrRemainingDistance = (io.Position - tileCoords).sqrMagnitude;// while remaining distance still not 0
                while (sqrRemainingDistance > float.Epsilon)
                {
                    // find a position proportionally closer to the end based on the move time.
                    // Vector2 MoveTowards moves a point in a straight line towards a target point
                    Vector2 newPosition = Vector2.MoveTowards(io.Position, tileCoords, inverseMoveTime * Time.deltaTime);

                    // re-position the viewport
                    TileViewportController.Instance.CenterOnTile(newPosition);
                    // position the IO at the new position
                    io.Position = newPosition;
                    Vector2 rbp = TileViewportController.Instance.GetScreenCoordinatesForWorldPosition(newPosition);
                    // move unit
                    transform.position = rbp;
                    //rigidBody.MovePosition(rbp);

                    // re-calculate remaining distance
                    sqrRemainingDistance = (io.Position - tileCoords).sqrMagnitude;
                    // print("moving unit to " + tileCoords + "\nposition " + newPosition + "\t||unit " + rbp);
                    // wait one frame before re-evaluating loop condition
                    yield return null;
                }
            }
            else
            {
                // move a unit that is not the PC
            }
        }
        /// <summary>
        /// Co-routine to move units from one space to the next.
        /// </summary>
        /// <param name="end">the move destination</param>
        /// <returns></returns>
        protected IEnumerator SmoothMovement(Vector3 end, float waitTime = 0f)
        {
            if (waitTime > 0f)
            {
                yield return new WaitForSeconds(waitTime);
            }
            // calculate remaining distance to move based on the square magnitude of the difference between the current position and the end parameter
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            // while remaining distance still not 0
            while (sqrRemainingDistance > float.Epsilon)
            {
                Debug.Log("still moving to");
                // find a position proportionally closer to the end based on the move time.
                // Vector3 MoveTowards moves a point in a straight line towards a target point
                Vector3 newPosition = Vector3.MoveTowards(rigidBody.position, end, inverseMoveTime * Time.deltaTime);
                // move unit
                rigidBody.MovePosition(newPosition);
                // re-calculate remaining distance
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                // wait one frame before re-evaluating loop condition
                yield return null;
            }
            print("arrive at " + end);
        }
        protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            bool moveResult = false;
            // store the current position
            Vector2 start = transform.position;
            // calculate end position based on directions passed in when calling move
            Vector2 end = start + new Vector2(xDir, yDir);
            print(end);

            // disable attached boxcollider so when casting rays you dont hit your own unit's collider
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, blockingLayer);
            //re-enable box collider
            boxCollider.enabled = true;
            // check to see if anything was hit
            if (hit.transform == null)
            {
                print("hit nothing");
                // space is open and available to move into
                // StartCoroutine(SmoothMovement(end));
                moveResult = true;
            }
            return moveResult;
        }
        protected abstract void OnCantMove<T>(T component) where T : Component;
    }
}
