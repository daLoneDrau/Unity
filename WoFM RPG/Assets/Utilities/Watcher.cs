using UnityEngine;
using System.Collections;

public abstract class Watcher : MonoBehaviour
{
    /// <summary>
    /// Updates 
    /// </summary>
    /// <param name="data">the data instance</param>
    public abstract void WatchUpdated(Watchable data);
}
