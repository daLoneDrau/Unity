using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChildScript : UIBehaviour {
    
	
	// Update is called once per frame
	void Update () {

    }
    protected override void OnEnable()
    {
        print("+++++++++++++++++++child was enabled");
    }
}
