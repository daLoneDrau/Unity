using UnityEngine;
using System.Collections;
using System;

public class PickupController : MonoBehaviour
{
    /// <summary>
    /// The hours to degrees.
    /// </summary>
    private const float hoursToDegrees = 360f / 12f;
    /// <summary>
    /// The minutes to degrees.
    /// </summary>
    private const float minutesToDegrees = 360f / 60f;
    /// <summary>
    /// The seconds to degrees.
    /// </summary>
    private const float secondsToDegrees = 360f / 60f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        TimeSpan timespan = DateTime.Now.TimeOfDay;
        // rotate 6 degrees per second - as fast as a second hand on a clock
        //transform.localRotation = Quaternion.Euler((float)timespan.TotalSeconds * -secondsToDegrees,
        //    (float)timespan.TotalSeconds * -secondsToDegrees,
        //    0);
        // rotate at a rate of 15 degrees x, 30 degrees y, 45 degrees z per second.
        // see documentation for transform.Rotate at https://docs.unity3d.com/ScriptReference/Transform.Rotate.html
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

    }
}
