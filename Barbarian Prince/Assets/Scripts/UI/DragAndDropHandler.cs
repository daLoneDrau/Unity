using Assets.Scripts.UI;
using RPGBase.Flyweights;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DragAndDropHandler : MonoBehaviour
    {
        /// <summary>
        /// flag indicating a drag has started.
        /// </summary>
        public bool Dragging { get; private set; }
        IDropAccessible dragSource;
        IDropAccessible dragTarget;
        public void EnterDraggable(IDropAccessible dragArea)
        {
            if (Dragging)
            {
                dragTarget = dragArea;
                print("entered " + dragArea.GetGameObject().name);
            }
        }
        public void ExitDraggable(IDropAccessible dragArea)
        {
            if (Dragging && dragTarget != null)
            {
                if (GameObject.ReferenceEquals(dragTarget.GetGameObject(), dragArea.GetGameObject()))
                {
                    dragTarget = null;
                    print("exited " + dragArea.GetGameObject().name);
                }
            }
        }
        public void DragStart(IDropAccessible vessel)
        {
            if (!Dragging)
            {
                print("starting to drag from " + vessel.GetGameObject().name);
                Type t = typeof(InventorySlotController);
                if (t == vessel.GetType())
                {
                    print("dragging from slot");
                }
                Dragging = true;
                dragSource = vessel;
            }
        }
        public void DragEnd()
        {
            if (Dragging && dragTarget != null)
            {
                print("ending drag  - last over " + dragTarget.GetGameObject().name);
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
            Dragging = false;
            // go back to default cursor
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
}
