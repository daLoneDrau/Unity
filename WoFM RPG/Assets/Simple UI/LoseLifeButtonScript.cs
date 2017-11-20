using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseLifeButtonScript : MonoBehaviour {

    [SerializeField]
    private StatusBarScript otherScript;
    void Start()
    {
    }

    public void TaskOnClick()
    {
        print("You have clicked the button!");
        otherScript.Adjust(-33);
    }
}
