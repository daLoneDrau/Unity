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

        // DRAG START
        eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.BeginDrag
        };
        eventtype.callback.AddListener((eventData) => { OnBeginDrag(); });

        gameObject.AddComponent<EventTrigger>();
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);

        // DRAGGING
        eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Drag
        };
        eventtype.callback.AddListener((eventData) => { OnDrag(); });

        gameObject.AddComponent<EventTrigger>();
        gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);

        // DRAG END
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
        DragAndDropHandler.EnterDraggable(this);
    }
    void OnMouseExit()
    {
        // TODO - notify inventory that user is hovering
        if (io != null)
        {
            Inventory.ExitIo(io);
            DragAndDropHandler.ExitDraggable(this);
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
            DragAndDropHandler.DragStart(this);
        }
    }
    private bool cursorSet = false;
    void OnDrag()
    {
        //print("dragging...............................................");
        // change the cursor to my object
        if (io != null
            && !cursorSet)
        {
            Image img = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            print(img);
            // assume "sprite" is your Sprite object
            var croppedTexture = new Texture2D((int)img.sprite.rect.width, (int)img.sprite.rect.height);
            var pixels = img.sprite.texture.GetPixels((int)img.sprite.textureRect.x,
                                                    (int)img.sprite.textureRect.y,
                                                    (int)img.sprite.textureRect.width,
                                                    (int)img.sprite.textureRect.height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();
            Cursor.SetCursor(croppedTexture, Vector2.zero, CursorMode.Auto);
            //Sprite.Create(texture, rect, pivot);
            cursorSet = true;
        }
    }
    void OnEndDrag()
    {
        cursorSet = false;
        DragAndDropHandler.DragEnd();
    }
    public void HandleDrop()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
