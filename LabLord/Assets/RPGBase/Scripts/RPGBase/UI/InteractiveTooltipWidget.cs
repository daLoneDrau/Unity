using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGBase.UI
{
    /// <summary>
    /// A 
    /// </summary>
    public class InteractiveTooltipWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// the default cursor mode - hardware.
        /// </summary>
        public CursorMode cursorMode = CursorMode.Auto;
        /// <summary>
        /// flag indicating whether the debug flag is on.
        /// </summary>
        public bool Debug;
        /// <summary>
        /// the highlight sprite used when entering the widget.  not used right now.
        /// </summary>
        public Sprite HighlightSprite;
        /// <summary>
        /// the default cursor hotspot.
        /// </summary>
        public Vector2 hotSpot = Vector2.zero;
        /// <summary>
        /// flag indicating whether the widget is inside a modal window.
        /// </summary>
        public bool IsModal;
        /// <summary>
        /// Flag indicating the tooltip always shows, regardless of the active status.
        /// </summary>
        public bool ShowTTAlways;
        /// <summary>
        /// the widget's normal sprite.
        /// </summary>
        private Sprite normalSprite;
        public MonoBehaviour StringProducerObject;
        public string StringProducerMethod;
        /// <summary>
        /// The cursor texture.
        /// </summary>
        public Texture2D PointerTexture;
        /// <summary>
        /// the name of the tooltip object associated with the widget.
        /// </summary>
        public string TooltipText;
        /// <summary>
        /// the <see cref="Text"/> instance used to display the tooltip.
        /// </summary>
        public Text TooltipArea;
        /// <summary>
        /// Actions taken when the pointer enters the widget.
        /// </summary>
        /// <param name="eventData">the pointer event data</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Debug)
            {
                print("EnterWidget");
            }
            bool ignore = false;
            if (!ignore
                && InputController.Instance != null
                && InputController.Instance.CONTROLS_FROZEN)
            {
                if (Debug)
                {
                    print("controls are frozen");
                }
                // controls are frozen, check to see if we're in a modal window
                if (!IsModal)
                {
                    if (Debug)
                    {
                        print("not in modal");
                    }
                    ignore = true;
                }
            }
            if (!ignore
                && gameObject.GetComponent<Selectable>() != null
                && !gameObject.GetComponent<Selectable>().interactable)
            {
                if (Debug)
                {
                    print("widget is selectable, but not interactable");
                }
                // widget is a selectable, but not interactable right now
                if (!ShowTTAlways)
                {
                    ignore = true;
                }
            }
            if (!ignore)
            {
                if (Debug)
                {
                    print("widget will not be ignored");
                }
                // show tooltip
                if (TooltipText != null
                    && TooltipText.Length > 0)
                {
                    if (TooltipArea != null)
                    {
                        if (StringProducerObject != null)
                        {
                            MethodInfo method = StringProducerObject.GetType().GetMethod(StringProducerMethod, new Type[] { "".GetType() });
                            var returnval = method.Invoke(StringProducerObject, new object[] { TooltipText }) as string;
                            TooltipArea.text = returnval.Replace("<br>", "\n");
                        }
                        else
                        {
                            TooltipArea.text = TooltipText.Replace("<br>", "\n");
                        }
                    }
                    else if (Tooltip.Instance != null)
                    {
                        Tooltip.Instance.Show(GetComponent<RectTransform>(), TooltipText.Replace("<br>", "\n"));
                    }
                }

                // change the cursor
                if (PointerTexture != null)
                {
                    if (gameObject.GetComponent<Selectable>() != null
                        && !gameObject.GetComponent<Selectable>().interactable)
                    {
                        if (Debug)
                        {
                            print("selectable is not interactable - do not change the cursor");
                        }
                    }
                    else
                    {
                        if (Debug)
                        {
                            print("change the cursor");
                        }
                        Cursor.SetCursor(PointerTexture, hotSpot, cursorMode);
                    }
                }

                // change the icon
                if (HighlightSprite != null)
                {
                    normalSprite = gameObject.GetComponent<Image>().sprite;
                    gameObject.GetComponent<Image>().sprite = HighlightSprite;
                }
            }
        }
        public void OnDisable()
        {
            OnPointerExit(null);
        }
        /// <summary>
        /// Actions taken when the pointer exits the widget.
        /// </summary>
        /// <param name="eventData">the pointer event data</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            bool ignore = false;
            if (!ignore
                && InputController.Instance != null
                && InputController.Instance.CONTROLS_FROZEN)
            {
                // controls are frozen, check to see if we're in a modal window
                if (!IsModal)
                {
                    ignore = true;
                }
            }
            if (!ignore
                && gameObject.GetComponent<Selectable>() != null
                && !gameObject.GetComponent<Selectable>().interactable)
            {
                // widget is a selectable, but not interactable right now
                if (!ShowTTAlways)
                {
                    ignore = true;
                }
            }
            if (!ignore)
            {
                // show tooltip
                if (TooltipText != null
                    && TooltipText.Length > 0)
                {
                    if (TooltipArea != null)
                    {
                        TooltipArea.text = "";
                    }
                    else if (Tooltip.Instance != null)
                    {
                        Tooltip.Instance.Hide();
                    }
                }
                // change the cursor
                if (PointerTexture != null)
                {
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                }

                // change the icon
                if (HighlightSprite != null)
                {
                    gameObject.GetComponent<Image>().sprite = normalSprite;
                }
            }
        }
    }
}
