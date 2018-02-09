using Assets.Scripts.UI;
using RPGBase.Flyweights;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropHandler : MonoBehaviour
{
    /// <summary>
    /// flag indicating a drag has started.
    /// </summary>
    private bool dragging;
    IDropAccessible dragSource;
    public void DragStart(IDropAccessible vessel)
    {
        if (!dragging)
        {
            print("starting to drag from vessel");
            Type t = typeof(InventorySlotController);
            if (t == vessel.GetType())
            {
                print("dragging from slot");
            }
            dragging = true;
            dragSource = vessel;
        }
    }
    public void DragEnd(IDropAccessible dragTarget)
    {
        if (dragging)
        {
            print("ending drag");
            Type t = typeof(InventorySlotController);
            if (t == dragTarget.GetType()
                && t == dragSource.GetType())
            {
                print("slot to slot");
                // SLOT TO SLOT DRAG
                InventorySlotController target = (InventorySlotController)dragTarget;
                InventorySlotController source = (InventorySlotController)dragSource;
                BaseInteractiveObject srcIo = null, trgIo = null;
                if (source.Io != null)
                {
                    srcIo = source.Io;
                }
                if (target.Io != null)
                {
                    trgIo = target.Io;
                }
                print(trgIo);
                if (srcIo != null
                    && trgIo == null)
                {
                    print("drag to empty");
                    // dragging item to empty slot
                    // put source into target slot
                    target.Io = srcIo;
                    source.Io = null;
                }
            }
        }
        dragging = false;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
