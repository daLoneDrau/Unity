using Assets.Scripts.UI;
using RPGBase.Flyweights;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour, IDropAccessible
{
    /// <summary>
    /// The master inventory controller.
    /// </summary>
    private InventoryController Inventory;
    /// <summary>
    /// The master inventory controller.
    /// </summary>
    private DragAndDropHandler DragAndDropHandler;
    /// <summary>
    /// The slot sprite.
    /// </summary>
    private Image icon;
    /// <summary>
    /// The IO in the slot.
    /// </summary>
    private BaseInteractiveObject io;
    /// <summary>
    /// The IO property.
    /// </summary>
    public BaseInteractiveObject Io
    {
        get { return io; }
        set
        {
            print("setting io");
            io = value;
            // TODO - set sprite
            if (io != null)
            {
                icon.sprite = io.Sprite;
                icon.color = Color.white;
            }
            else
            {
                icon.color = Color.clear;
            }
        }
    }
    void Awake()
    {
        // set event listeners
        EventTrigger.Entry eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        eventtype.callback.AddListener((eventData) => { OnMouseEnter(); });

        gameObject.AddComponent<EventTrigger>();
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);

        eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        eventtype.callback.AddListener((eventData) => { OnMouseExit(); });
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
        // set image icon
        icon = transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>();
        icon.color = Color.clear;
        // set inventory controller
        Inventory = transform.parent.parent.GetComponent<InventoryController>();
        // set drag and drop controller
        DragAndDropHandler = transform.parent.parent.GetComponent<DragAndDropHandler>();

        eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.BeginDrag
        };
        eventtype.callback.AddListener((eventData) => { OnBeginDrag(); });

        gameObject.AddComponent<EventTrigger>();
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);


        eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Drag
        };
        eventtype.callback.AddListener((eventData) => { OnDrag(); });

        gameObject.AddComponent<EventTrigger>();
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
        
        eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.EndDrag
        };
        eventtype.callback.AddListener((eventData) => { OnEndDrag(); });

        gameObject.AddComponent<EventTrigger>();
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
    }
    void OnMouseEnter()
    {
        // TODO - notify inventory that user is hovering
        if (io != null)
        {
            Inventory.EnterIo(io);
        }
    }
    void OnMouseExit()
    {
        // TODO - notify inventory that user is hovering
        if (io != null)
        {
            Inventory.ExitIo(io);
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnBeginDrag()
    {
        if (io != null)
        {
            print("start drag from "+gameObject.name);
            DragAndDropHandler.DragStart(this);
        }
    }
    void OnDrag()
    {
        //print("dragging...............................................");
    }
    void OnEndDrag()
    {
        Debug.Log("Current detected event: " + Event.current);
        print("pointer up at "+gameObject.name);
        DragAndDropHandler.DragEnd(this);
    }
    public void HandleDrop()
    {
        throw new System.NotImplementedException();
    }
}
