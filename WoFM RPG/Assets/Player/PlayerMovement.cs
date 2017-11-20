using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// stop radius for when figure stops moving.
    /// </summary>
    [SerializeField] float walkMoveStopRadius = .3f;
    [SerializeField] float attackkMoveStopRadius = 1f;
    ThirdPersonCharacter thirdPersonCharacter;
    CameraRayCaster cameraRayCaster;
    Vector3 currentDestination, clickPoint;
    // Use this for initialization
    private void Start()
    {
        cameraRayCaster = Camera.main.GetComponent<CameraRayCaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        ProcessMouseActions();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        // draw black line to click target
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        // draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackkMoveStopRadius);

    }
    private void ProcessMouseActions()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRayCaster.Hit.point;
            // handle where mouse clicked on screen
            switch (cameraRayCaster.LayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.IOs:
                    currentDestination = ShortDestination(clickPoint, attackkMoveStopRadius);
                    break;
            }
        }
        WalkToDestination();
    }

    private void WalkToDestination()
    {
        // get the current distance to spot clicked
        Vector3 distanceToClick = currentDestination - transform.position;
        // stop movement when close enough to target
        if (distanceToClick.magnitude >= 0)
        {
            thirdPersonCharacter.Move(distanceToClick, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 v, float scale)
    {
        Vector3 reduced = (v - transform.position).normalized * scale;
        return v - reduced;
    }
}
