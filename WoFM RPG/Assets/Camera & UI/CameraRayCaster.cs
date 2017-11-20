using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayCaster : MonoBehaviour
{

    private Layer[] layerPriorities =
    {
        Layer.IOs,
        Layer.Obstacle,
        Layer.Walkable
    };
    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;
    RaycastHit hit;
    Layer lastLayer;
    public RaycastHit Hit
    {
        get { return hit;  }
    }
    public Layer LayerHit { get; set; }
    // Use this for initialization
    void Start()
    {
        viewCamera = Camera.main;
    }
    public delegate void OnLayerChange(Layer layer); // declare new delegate type
    // use keyword event to prevent resetting the delegates.
    // event keyword means field can only appear to the left of '+=' or '-='
    public event OnLayerChange LayerChangeObservers; // instantiate observer set

    void SomeLayerChangeHandler()
    {
        print("I got it!!");
    }

    // Update is called once per frame
    void Update()
    {
        bool gotHit = false;
        foreach (Layer layer in layerPriorities)
        {
            var hit1 = RayCastForLayer(layer);
            if (hit1 != null)
            {
                hit = (RaycastHit)hit1;
                if (LayerHit != layer)
                {
                    LayerHit = layer;
                    // call delegate
                    LayerChangeObservers(LayerHit); // call the list of delegates
                }
                gotHit = true;
                break;
            }
        }
        if (!gotHit)
        {
            hit.distance = distanceToBackground;
            if (LayerHit != Layer.RaycastEndStop)
            {
                LayerHit = Layer.RaycastEndStop;
                // call delegate
                LayerChangeObservers(LayerHit); // call the list of delegates
            }
        }
    }
    
    private object RayCastForLayer(Layer layer)
    {
        object o = null;
        int layerMask = 1 << (int)layer;
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit raycastHit;
        // pass raycastHit in to method as reference, not value
        bool hasHit = Physics.Raycast(ray, out raycastHit, distanceToBackground, layerMask);
        if (hasHit)
        {
            o = raycastHit;
            Debug.Log("Touched object " + raycastHit.transform.gameObject.name + " layer is " + raycastHit.transform.gameObject.layer + " while checking layer " + layer);
        }
        return o;
    }
}
