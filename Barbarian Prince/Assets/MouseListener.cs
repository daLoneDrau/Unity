using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseListener : MonoBehaviour
{
    [SerializeField]
    private GameObject marker;
    [SerializeField]
    private WorldController world;
    private Vector3 lastFramePosition;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currMousePos.z = 0; // fix z value so camera doesn't move along z-axis
        // update marker position
        if (world.RequiresMarker(currMousePos))
        {
            marker.SetActive(true);
            marker.transform.position = world.GetTileCoordinatesForWorldCoordinates(currMousePos);
        }
        else
        {
            marker.SetActive(false);
        }

        // handle screen dragging
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            // middle button
            Vector3 diff = lastFramePosition - currMousePos; // get space between last position and current
            Camera.main.transform.Translate(diff); // move camera by difference
            // if diff.x is >0, move right, <0 move left
            // if diff.y is >0 move up, <0 move down
        }
        else if (Input.GetMouseButton(1))
        {
            // right button down 
        }
        else if (Input.GetMouseButton(0))
        {
            // left button down 
        }
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }
}
