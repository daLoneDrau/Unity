using Assets.Scripts.UI;
using RPGBase.Flyweights;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
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
        /// <summary>
        /// flag indicating the cursor has been set after a drag action
        /// </summary>
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
                var pixels = img.sprite.texture.GetPixels((int)img.sprite.textureRect.x, (int)img.sprite.textureRect.y, (int)img.sprite.textureRect.width, (int)img.sprite.textureRect.height);
                croppedTexture.SetPixels(pixels);
                // draw "arrow" pointing to hotspot
                DrawLine(croppedTexture,
                    0,
                    (int)img.sprite.textureRect.height - 1,
                    0,
                    (int)img.sprite.textureRect.height - 5,
                    Color.white,
                    2); // vertical line
                DrawLine(croppedTexture,
                    0,
                    (int)img.sprite.textureRect.height - 1,
                    4,
                    (int)img.sprite.textureRect.height - 1,
                    Color.white,
                    2); // horizontal line
                croppedTexture.Apply();
                Cursor.SetCursor(croppedTexture, Vector2.zero, CursorMode.Auto);
                cursorSet = true;
            }
        }
        private void DrawLine(Texture2D a_Texture, int x1, int y1, int x2, int y2, Color a_Color, int lineWidth = 1)
        {
            float xPix = x1;
            float yPix = y1;

            float width = x2 - x1;
            float height = y2 - y1;
            float length = Mathf.Abs(width);
            if (Mathf.Abs(height) > length) length = Mathf.Abs(height);
            int intLength = (int)length;
            float dx = width / (float)length;
            float dy = height / (float)length;
            for (int i = 0; i <= intLength; i++)
            {
                if (lineWidth == 1)
                {
                    a_Texture.SetPixel((int)xPix, (int)yPix, a_Color);
                }
                else
                {
                    if (Mathf.Abs(x1 - x2) > Mathf.Abs(y1 - y2))
                    {
                        // horizontal line
                        int minY = (int)yPix - (lineWidth / 2);
                        if (minY + lineWidth - 1 >= a_Texture.height)
                        {
                            minY = a_Texture.height - lineWidth;
                        }
                        else if (minY < 0)
                        {
                            minY = 0;
                        }
                        for (int j = minY; j < minY + lineWidth; j++)
                        {
                            a_Texture.SetPixel((int)xPix, j, a_Color);
                        }
                    }
                    else
                    {
                        // vertical line
                        int minX = (int)xPix - (lineWidth / 2);
                        if (minX + lineWidth - 1 >= a_Texture.width)
                        {
                            minX = a_Texture.width - lineWidth;
                        }
                        else if (minX < 0)
                        {
                            minX = 0;
                        }
                        for (int j = minX; j < minX + lineWidth; j++)
                        {
                            a_Texture.SetPixel(j, (int)yPix, a_Color);
                        }
                    }
                }
                xPix += dx;
                yPix += dy;
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
}
