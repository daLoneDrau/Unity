using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    /// <summary>
    /// the player object.
    /// </summary>
    public GameObject player;
    /// <summary>
    /// the offset between the camera and the player object.
    /// </summary>
    private Vector3 offset;
    /// <summary>
    /// LateUpdate is called after all Update functions have been called. This is useful to order script execution.
    /// For example a follow camera should always be implemented in LateUpdate because it tracks objects that might
    /// have moved inside Update.
    /// 
    /// Updates to follow cameras, procedural animation, and gathering last known states should be performed in
    /// LateUpdate.
    /// </summary>
    void LateUpdate()
    {
        // with each frame update, position the camera relative to the player
        transform.position = player.transform.position + offset;
    }
	// Use this for initialization
	void Start () {
        // capture the initial offset defined in the unity setup
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
