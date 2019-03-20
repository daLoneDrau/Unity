using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WoFM.UI._2D
{
    public class WoFMAnimationUtility : MonoBehaviour
    {
        public IEnumerator MoveObjectInStraightLine(Transform objectTransform, Vector3 endPoint, float moveTime, bool useLocal = false)
        {
            // get inverse of movement time
            float inverseSpeed = 1f / moveTime;
            // calculate remaining distance to move based on the square magnitude of the difference between the current position and the end parameter
            float sqrRemainingDistance = (objectTransform.position - endPoint).sqrMagnitude;
            if (useLocal)
            {
                sqrRemainingDistance = (objectTransform.localPosition - endPoint).sqrMagnitude;
            }
            // while remaining distance still not 0
            while (sqrRemainingDistance > float.Epsilon)
            {
                // find a position proportionally closer to the end based on the move time.
                // Vector3 MoveTowards moves a point in a straight line towards a target point
                Vector3 newPosition = Vector3.MoveTowards(objectTransform.position, endPoint, inverseSpeed * Time.deltaTime);
                if (useLocal)
                {
                    newPosition = Vector3.MoveTowards(objectTransform.localPosition, endPoint, inverseSpeed * Time.deltaTime);
                }

                // position the animation object at the new position
                if (useLocal)
                {
                    objectTransform.localPosition = newPosition;
                }
                else
                {
                    objectTransform.position = newPosition;
                }

                // re-calculate remaining distance
                sqrRemainingDistance = (objectTransform.position - endPoint).sqrMagnitude;
                if (useLocal)
                {
                    sqrRemainingDistance = (objectTransform.localPosition - endPoint).sqrMagnitude;
                }
                // wait one frame before re-evaluating loop condition
                yield return null;
            }
        }
    }
}
