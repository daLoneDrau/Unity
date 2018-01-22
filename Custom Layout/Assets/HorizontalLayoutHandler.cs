using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets
{
    [RequireComponent(typeof(RectTransform))]
    public class HorizontalLayoutHandler : UIBehaviour, IRPGLayoutHandler
    {
        private float lastUpdate = 0;
        private float width;
        private float height;
        public enum VerticalAlignment
        {
            Upper,
            Middle,
            Lower
        }
        [SerializeField] int spacing;
        [Serializable]
        public class Padding
        {
            public int Left;
            public int Top;
            public int Bottom;
            public int Right;
        }
        [SerializeField] Padding padding;
        [SerializeField] VerticalAlignment VerticalAlign = VerticalAlignment.Upper;
        private List<Vector2> childSizes = new List<Vector2>();
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (gameObject.activeSelf)
            {
                float now = Time.realtimeSinceStartup;
                if (lastUpdate < 0)
                {
                    lastUpdate = now;
                    print("OnValidate::" + gameObject.name + "::" + now);
                    Configure();
                }
                else if (now - lastUpdate > 1f)
                {
                    lastUpdate = now;
                    print("OnValidate::" + gameObject.name + "::" + now);
                    Configure();
                }
            }
        }
#endif
        public Vector2 GetPreferredSize()
        {
            width = 0;
            height = 0;
            for (int i = 0, length = transform.childCount; i < length; i++)
            {
                Transform child = transform.GetChild(i);
                // ignore hidden children
                if (!child.gameObject.activeSelf)
                {
                    print("child " + child.name + " is hidden");
                    childSizes.Add(new Vector2(0, 0));
                    continue;
                }
                // handle layout handler children specially
                if ((child.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler) != null)
                {
                    print("child " + child.name + " has layout manager");
                    IRPGLayoutHandler childLayout = child.gameObject.GetComponent<IRPGLayoutHandler>();

                    print("********************************configuring " + child.name);
                    Vector2 childSize = childLayout.GetPreferredSize();
                    childLayout.Resize();
                    childLayout.PlaceChildren();
                    print("*************************child is " + childSize);
                    height = Mathf.Max(height, childSize.y);
                    width += childSize.x;
                    childSizes.Add(childSize);
                }
                else if (child.gameObject.GetComponent<LayoutElement>() != null)
                {
                    print("child " + child.name + " has layout element");
                    LayoutElement le = child.gameObject.GetComponent<LayoutElement>();
                    RectTransform rect = (RectTransform)child;
                    print("*************************child is " + le.minWidth + "," + le.minHeight);
                    float h = Mathf.Max(le.minHeight, le.preferredHeight);
                    float w = Mathf.Max(le.minWidth, le.preferredWidth);
                    height = Mathf.Max(height, h);
                    width += w;
                    if (i + 1 < length)
                    {
                        width += spacing;
                    }
                    childSizes.Add(new Vector2(w, h));
                }
                else
                {
                    RectTransform rect = (RectTransform)child;
                    print("*************************child is " + rect.rect);
                    height = Mathf.Max(height, rect.rect.height);
                    width += rect.rect.width;
                    if (i + 1 < length)
                    {
                        width += spacing;
                    }
                    childSizes.Add(new Vector2(rect.rect.width, rect.rect.height));
                }
            }
            width += padding.Left + padding.Right;
            height += padding.Top + padding.Bottom;
            return new Vector2(width, height);
        }
        public void Resize()
        {
            RectTransform me = GetComponent<RectTransform>();
            print(me.rect);
            if (IsStretching(me))
            {
                Vector2 parentSize = ((RectTransform)me.parent).rect.size;

                // try changing anchor positions to move element.
                // anchor min x stays the same.
                // anchor max y stays the same.
                // anchor max x changes to (parentsize width - ((min x * parentsize width) + new width)) / parentsize width;
                // anchor min y moves up to (parentsize height - ((max y * parentsize height) + new height) / parentsize height;
                float minX = me.anchorMin.x;
                float maxY = me.anchorMax.y;
                float maxX = ((minX * parentSize.x) + width) / parentSize.x;
                float minY = ((maxY * parentSize.y) - height) / parentSize.y;
                me.anchorMin = new Vector2(minX, minY);
                me.anchorMax = new Vector2(maxX, maxY);
            }
            else
            {
                //ResizeAndPositionNonStretchy(me, child, child.rect.size, new Vector2(x, y));
            }
        }
        public void PlaceChildren()
        {
            RectTransform me = GetComponent<RectTransform>();
            // children get place starting at position 0,0 and then proceed left
            float x = 0;
            x += padding.Left;
            for (int i = 0, length = me.childCount; i < length; i++)
            {
                float y = 0;
                RectTransform child = transform.GetChild(i) as RectTransform;
                Vector2 childSize = childSizes[i];
                switch (VerticalAlign)
                {
                    case VerticalAlignment.Upper:
                        y = me.rect.size.y;
                        y -= padding.Top;
                        y -= childSize.y;
                        break;
                    case VerticalAlignment.Middle:
                        y = me.rect.size.y / 2;
                        y -= childSize.y / 2;
                        break;
                    case VerticalAlignment.Lower:
                        y += padding.Bottom;
                        break;
                }
                if (IsStretching(child))
                {
                    ResizeAndPositionStretchy(me, child, childSizes[i], new Vector2(x, y));
                }
                else
                {
                    ResizeAndPositionNonStretchy(me, child, childSizes[i], new Vector2(x, y));
                }
                x += childSize.x;
                if (i + 1 < length)
                {
                    x += spacing;
                }
            }
        }
        void ResizeAndPositionNonStretchy(RectTransform parent, RectTransform child, Vector2 size, Vector2 lowerLeft)
        {
            print("ResizeAndPositionNonStretchy(" + size + "," + lowerLeft);
            Vector2 parentSize = parent.rect.size;
            // size delta is the difference in size between an element's actual size and the size
            // of the rectangle made up by its anchors.  if an element is 500x300, while its anchors cover an area
            // 600x400, then the size delta is -100,-100.  if the element was 600x400 and its anchors covered 400x300,
            // then the size delta would be 200,100;
            // size delta is going to be the element's actual size.  reset it to the desired size
            child.sizeDelta = size;
            // new position should be adjusted to consider element size, child size and child pivot position
            float x = -parentSize.x / 2f;
            x += lowerLeft.x + (child.sizeDelta.x * child.pivot.x);
            float y = -parentSize.y / 2f;
            y += lowerLeft.y + (child.sizeDelta.y * child.pivot.y);
            child.anchoredPosition = new Vector2(x, y);
        }
        /// <summary>
        /// Resizes and positions a "stretching" UI element
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="size"></param>
        /// <param name="lowerLeft"></param>
        void ResizeAndPositionStretchy(RectTransform parent, RectTransform child, Vector2 size, Vector2 lowerLeft)
        {
            print("ResizeAndPositionStretchy(" + size + "," + lowerLeft);
            Vector2 parentSize = parent.rect.size;
            /*
            float minX = lowerLeft.x;
            float maxX = -(parentSize.x - size.x);
            float minY = lowerLeft.y;
            float maxY = -(parentSize.y - size.y);
            child.offsetMin = new Vector2(minX, minY);
            child.offsetMax = new Vector2(minX + maxX, minY + maxY);
            */


            // try changing anchor positions to move element.
            // anchor min x is left position / parent width.
            // anchor min y is bottom position / parent height
            float minX = lowerLeft.x / parentSize.x;
            float minY = lowerLeft.y / parentSize.y;
            float maxX = (lowerLeft.x + size.x) / parentSize.x;
            float maxY = (lowerLeft.y + size.y) / parentSize.y;
            child.anchorMin = new Vector2(minX, minY);
            child.anchorMax = new Vector2(maxX, maxY);
            child.offsetMin = new Vector2(0, 0);
            child.offsetMax = new Vector2(0, 0);
        }
        /// <summary>
        /// Determines if a Rect Transform has any stretching behavior.  Stretching behavior happens when the anchorMin and anchorMax properties are not identical.
        /// </summary>
        /// <param name="transform">the Rect Transform</param>
        /// <returns>true if the Rect Transform has stretching behavior; false otherwise</returns>
        bool IsStretching(RectTransform transform)
        {
            return transform.anchorMin != transform.anchorMax;
        }
        public void SetLayoutHorizontal()
        {
            //throw new NotImplementedException();
        }
        public void SetLayoutVertical()
        {
            //throw new NotImplementedException();
        }
        public void Configure()
        {
            if ((transform.parent.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler) != null)
            {
                print("parent " + transform.parent.name + " has layout manager. will wait for parent to request processing.");
            }
            else
            {
                print("********************************configuring " + gameObject.name);
                Vector2 size = GetPreferredSize();
                print("size::" + size);
                Resize();
                PlaceChildren();
            }
        }
    }
}
