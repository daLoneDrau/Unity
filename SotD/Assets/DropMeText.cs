using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyUI
{
    public class DropMeText : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Text receivingText;
        private Color normalColor;
        public Color highlightColor = Color.yellow;

        public void OnEnable()
        {
            /*
                  if (containerImage != null)
                      normalColor = containerImage.color;
                */
        }

        public void OnDrop(PointerEventData data)
        {
            //containerImage.color = normalColor;
            // if no Text instance assigned, then do nothing.
            if (receivingText != null)
            {
                string dropText = GetDropText(data);
                if (dropText != null)
                {
                    receivingText.text = dropText;
                }
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            /*
                  if (containerImage == null)
                      return;
                  */
            string dropText = GetDropText(data);
            /*
                if (dropSprite != null)
                    containerImage.color = highlightColor;
              */
        }

        public void OnPointerExit(PointerEventData data)
        {
            /*
                  if (containerImage == null)
                      return;
                  */
            /*
                containerImage.color = normalColor;
            */
        }

        private string GetDropText(PointerEventData data)
        {
            string srcString = null;
            var originalObj = data.pointerDrag;
            if (originalObj != null)
            {
                var dragMe = originalObj.GetComponent<DragMeText>();
                if (dragMe != null)
                {
                    var srcText = originalObj.GetComponent<Text>();
                    if (srcText != null)
                    {
                        srcString = srcText.text;
                    }
                    srcText = null;
                }
            }
            return srcString;
        }
    }
}
