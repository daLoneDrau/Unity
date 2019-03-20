using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WoFM.UI.GlobalControllers;

namespace RPGBase.UI
{
    /// <summary>
    /// A 
    /// </summary>
    public class InteractiveTooltipWidget : MonoBehaviour
    {
        /// <summary>
        /// the default cursor mode - hardware.
        /// </summary>
        public CursorMode cursorMode = CursorMode.Auto;
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
        /// the widget's normal sprite.
        /// </summary>
        private Sprite normalSprite;
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
        public void EnterWidget(BaseEventData eventData)
        {
            bool ignore = false;
            if (!ignore
                && GameSceneController.Instance.CONTROLS_FROZEN)
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
                ignore = true;
            }
            if (!ignore)
            {
                // show tooltip
                if (TooltipText != null
                    && TooltipText.Length > 0)
                {
                    if (TooltipArea != null)
                    {
                        TooltipArea.text = TooltipText.Replace("<br>", "\n");
                    }
                    else
                    {
                        Tooltip.Instance.Show(GetComponent<RectTransform>(), TooltipText.Replace("<br>", "\n"));
                    }
                }

                // change the cursor
                if (PointerTexture != null)
                {
                    Cursor.SetCursor(PointerTexture, hotSpot, cursorMode);
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
            ExitWidget(null);
        }
        /// <summary>
        /// Actions taken when the pointer exits the widget.
        /// </summary>
        /// <param name="eventData">the pointer event data</param>
        public void ExitWidget(BaseEventData eventData)
        {
            bool ignore = false;
            if (!ignore
                && GameSceneController.Instance != null
                && GameSceneController.Instance.CONTROLS_FROZEN)
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
                ignore = true;
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
                    else
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
