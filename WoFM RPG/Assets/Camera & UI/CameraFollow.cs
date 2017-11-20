using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    GameObject player;
    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // change camera to isometric view
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.Rotate(30, 45, 0);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        // position camera arm at player's position
        transform.position = player.transform.position;
	}
}
