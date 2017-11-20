using UnityEngine;
using System;

/// <summary>
/// Tutorial project.
/// Create an object hierarchy.
/// Write a script and attach it to an object.
/// Access namespaces.
/// Update objects through methods.
/// Rotate things based on time.
/// Found at http://catlikecoding.com/unity/tutorials/clock/
/// </summary>
public class ClockAnimator : MonoBehaviour {
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
    /// <summary>
    /// The hours.
    /// </summary>
    public Transform hours;
	/// <summary>
	/// The minutes.
	/// </summary>
    public Transform minutes;
	/// <summary>
	/// The seconds.
	/// </summary>
	public Transform seconds;
	/// <summary>
	/// The analog flag.  If true, the clock is analog, and the hands move constantly;
	/// otherwise they show the exact time only.
	/// </summary>
	public bool analog;
	/// <summary>
	/// Update this instance.  This method is invoked by the Unity engine, public or private.
	/// During an update, the clockś hands are rotated to match the current time.
	/// </summary>
	private void Update () {
		if (analog) {
			// time object that allows us to access fractional hours, minutes, seconds
			TimeSpan timespan = DateTime.Now.TimeOfDay;
			hours.localRotation = Quaternion.Euler(
				0f, 0f, (float)timespan.TotalHours * -hoursToDegrees);
			minutes.localRotation = Quaternion.Euler(
				0f, 0f, (float)timespan.TotalMinutes * -minutesToDegrees);
			seconds.localRotation = Quaternion.Euler(
				0f, 0f, (float)timespan.TotalSeconds * -secondsToDegrees);
		} else {
			DateTime time = DateTime.Now;
			// Quaternions are used to represent rotations.
			// They are compact, don't suffer from gimbal lock and can easily be interpolated.
			// Unity internally uses Quaternions to represent all rotations.
			hours.localRotation =
			Quaternion.Euler (0f, 0f, time.Hour * -hoursToDegrees);
			minutes.localRotation =
			Quaternion.Euler (0f, 0f, time.Minute * -minutesToDegrees);
			seconds.localRotation =
			Quaternion.Euler (0f, 0f, time.Second * -secondsToDegrees);
		}
	}
}
