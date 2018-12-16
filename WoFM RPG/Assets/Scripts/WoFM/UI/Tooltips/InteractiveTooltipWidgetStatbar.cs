using RPGBase.Constants;
using RPGBase.Pooled;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WoFM.Flyweights;
using WoFM.Singletons;
using WoFM.UI.GlobalControllers;

namespace WoFM.UI.Tooltips
{
    /// <summary>
    /// A 
    /// </summary>
    public class InteractiveTooltipWidgetStatbar : MonoBehaviour
    {
        public Component Statbar;
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
        public string[] SkillSet;
        public int IoId;
        /// <summary>
        /// Actions taken when the pointer enters the widget.
        /// </summary>
        /// <param name="eventData">the pointer event data</param>
        public void EnterWidget(BaseEventData eventData)
        {
            print("enter widget" + gameObject.name);
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
                    if (Statbar != null)
                    {
                        TooltipStatbar.Instance.Show(Statbar.GetComponent<RectTransform>(), TooltipText);
                    }
                    else
                    {
                        TooltipStatbar.Instance.Show(GetComponent<RectTransform>(), TooltipText);
                    }
                }
                else
                {
                    print("print stat "+ SkillSet[0]+" for "+IoId);
                    PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
                    WoFMInteractiveObject io = (WoFMInteractiveObject)Interactive.Instance.GetIO(IoId);
                    if (io.HasIOFlag(IoGlobals.IO_01_PC))
                    {
                        io.PcData.ComputeFullStats();
                        if (string.Equals(SkillSet[0], "mstm", StringComparison.OrdinalIgnoreCase))
                        {
                            sb.Append((int)io.PcData.Life);
                            sb.Append("/");
                            sb.Append((int)io.PcData.GetFullAttributeScore(SkillSet[0]));
                            print("stat:"+sb.ToString());
                        }
                        else
                        {
                            print(sb.ToString());
                            sb.Append((int)io.PcData.GetFullAttributeScore(SkillSet[0]));
                            sb.Append("/");
                            sb.Append((int)io.PcData.GetFullAttributeScore(SkillSet[1]));
                            print("stat:" + sb.ToString());
                        }
                    }
                    else if (io.HasIOFlag(IoGlobals.IO_03_NPC))
                    {
                        io.NpcData.ComputeFullStats();
                        if (string.Equals(SkillSet[0], "mstm", StringComparison.OrdinalIgnoreCase))
                        {
                            sb.Append((int)io.NpcData.Life);
                            sb.Append("/");
                            sb.Append((int)io.NpcData.GetFullAttributeScore(SkillSet[0]));
                        }
                        else
                        {
                            sb.Append((int)io.NpcData.GetFullAttributeScore(SkillSet[0]));
                            sb.Append("/");
                            sb.Append((int)io.NpcData.GetFullAttributeScore(SkillSet[1]));
                        }
                    }
                    if (Statbar != null)
                    {
                        TooltipStatbar.Instance.Show(Statbar.GetComponent<RectTransform>(), sb.ToString());
                    }
                    else
                    {
                        TooltipStatbar.Instance.Show(GetComponent<RectTransform>(), sb.ToString());
                    }
                    sb.ReturnToPool();
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
        /// <summary>
        /// Actions taken when the pointer exits the widget.
        /// </summary>
        /// <param name="eventData">the pointer event data</param>
        public void ExitWidget(BaseEventData eventData)
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
                // hide tooltip
                if (TooltipText != null
                    && TooltipText.Length > 0)
                {
                    TooltipStatbar.Instance.Hide();
                }
                else if (SkillSet[0] != null
                    && SkillSet[0].Length > 0)
                {
                    TooltipStatbar.Instance.Hide();
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
