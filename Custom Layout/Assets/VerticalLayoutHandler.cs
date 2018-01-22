using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets
{
    /// <summary>
    /// Custom layout handler that positions children vertically.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class VerticalLayoutHandler : UIBehaviour, IRPGLayoutHandler
    {
        /// <summary>
        /// Horizontal alignment settings.
        /// </summary>
        public enum HorizontalAlignment
        {
            /// <summary>
            /// child elements are aligned to the left side of the layout
            /// </summary>
            Left,
            /// <summary>
            /// child elements are aligned to the center of the layout
            /// </summary>
            Center,
            /// <summary>
            /// child elements are aligned to the right side of the layout
            /// </summary>
            Right
        }
        /// <summary>
        /// The padding settings.
        /// </summary>
        [Serializable]
        public class Padding
        {
            /// <summary>
            /// the left-side padding.
            /// </summary>
            public int Left;
            /// <summary>
            /// the top-side padding.
            /// </summary>
            public int Top;
            /// <summary>
            /// the bottom-side padding.
            /// </summary>
            public int Bottom;
            /// <summary>
            /// the right-side padding.
            /// </summary>
            public int Right;
        }
        /// <summary>
        /// the stored sizes of all child elements.
        /// </summary>
        private List<Vector2> childSizes = new List<Vector2>();
        /// <summary>
        /// the layout's height.
        /// </summary>
        private float height;
        /// <summary>
        /// the layout's horizontal alignment.
        /// </summary>
        [SerializeField]
        private HorizontalAlignment HorizontalAlign = HorizontalAlignment.Left;
        /// <summary>
        /// the last time the layout was updated.
        /// </summary>
        private float lastUpdate = 0;
        /// <summary>
        /// the layout's padding.
        /// </summary>
        [SerializeField]
        private Padding padding;
        /// <summary>
        /// the spacing between child elements
        /// </summary>
        [SerializeField]
        private int spacing;
        /// <summary>
        /// the layout's width.
        /// </summary>
        private float width;
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
        public void Configure()
        {
            if ((transform.parent.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler) != null)
            {
                print("parent " + transform.parent.name + " has layout manager. will wait for parent to request processing.");
            }
            else
            {
                print("++++++++++++++++++++++++++++++++configuring " + gameObject.name + "++++++++++++++++++++++++++++++++");
                // 1. get all my children's sizes
                Vector2 size = GetPreferredSize();
                print("size::" + gameObject.name + "::" + size);
                // 2. resize myself
                Resize();
                // 3. resize all my layout children's sizes
                for (int i = 0, length = transform.childCount; i < length; i++)
                {
                    Transform child = transform.GetChild(i);
                    if ((child.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler) != null)
                    {
                        IRPGLayoutHandler childLayout = child.gameObject.GetComponent<IRPGLayoutHandler>();
                        print("********************************resizing child " + child.name);
                        childLayout.Resize();
                        // 3a. place all my layout children's children
                        childLayout.PlaceChildren();
                    }
                }
                // 4. place all my children
                PlaceChildren();
                print("++++++++++++++++++++++++++++++++done " + gameObject.name + "++++++++++++++++++++++++++++++++");
            }
        }
        public Vector2 GetPreferredSize()
        {
            height = 0;
            width = 0;
            childSizes.Clear();
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

                    print("********************************getting size for " + child.name);
                    Vector2 childSize = childLayout.GetPreferredSize();
                    print("*************************child is " + childSize);
                    height += childSize.y;
                    width = Mathf.Max(width, childSize.x);
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
                    height += h;
                    width = Mathf.Max(width, w);
                    childSizes.Add(new Vector2(w, h));
                }
                else
                {
                    RectTransform rect = (RectTransform)child;
                    print("*************************child is " + rect.rect);
                    height += rect.rect.height;
                    width = Mathf.Max(width, rect.rect.width);
                    childSizes.Add(new Vector2(rect.rect.width, rect.rect.height));
                }
                if (i + 1 < length
                    && transform.GetChild(i + 1).gameObject.activeSelf)
                {
                    width += spacing;
                }
            }
            height += padding.Top + padding.Bottom;
            width += padding.Left + padding.Right;
            return new Vector2(width, height);
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
        public bool NotNanOrInfinity(float val)
        {
            return !Double.IsNaN(val) && !Double.IsInfinity(val);
        }
        public void PlaceChildren()
        {
            print("-------------------------PlaceChildren " + gameObject.name);
            RectTransform me = GetComponent<RectTransform>();
            // children get placed starting at position 0,height and then proceed left
            float y = height;
            y -= padding.Top;
            for (int i = 0, length = me.childCount; i < length; i++)
            {
                float x = 0;
                RectTransform child = transform.GetChild(i) as RectTransform;
                // ignore hidden children
                if (!child.gameObject.activeSelf)
                {
                    print("child " + child.name + " is hidden");
                    // add spacing if next child is not hidden
                    if (i + 1 < length
                        && transform.GetChild(i + 1).gameObject.activeSelf)
                    {
                        y -= spacing;
                    }
                    continue;
                }
                Vector2 childSize = childSizes[i];
                y -= childSize.y;
                switch (HorizontalAlign)
                {
                    case HorizontalAlignment.Left:
                        x = 0;
                        x += padding.Left;
                        break;
                    case HorizontalAlignment.Center:
                        x = me.rect.size.x / 2;
                        x -= childSize.x / 2;
                        break;
                    case HorizontalAlignment.Right:
                        x = me.rect.size.x;
                        x -= padding.Right;
                        x -= childSize.x;
                        break;
                }
                if (IsStretching(child)
                    || (child.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler) != null)
                {
                    // treat custom layout handlers as stretchy regardless of anchor positions
                    ResizeAndPositionStretchy(me, child, childSizes[i], new Vector2(x, y));
                }
                else
                {
                    ResizeAndPositionNonStretchy(me, child, childSizes[i], new Vector2(x, y));
                }
                // add spacing if next child is not hidden
                if (i + 1 < length
                    && transform.GetChild(i + 1).gameObject.activeSelf)
                {
                    y -= spacing;
                }
            }
            print("-------------------------PlaceChildren DONE" + gameObject.name);
        }
        public void Resize()
        {
            print("*************************Resize " + gameObject.name);
            RectTransform me = GetComponent<RectTransform>();
            if (IsStretching(me))
            {
                print(gameObject.name + " is stretchy");
                Vector2 parentSize = ((RectTransform)me.parent).rect.size;
                // try changing anchor positions to move element.
                // anchor min x stays the same.
                // anchor max y stays the same.
                // anchor max x changes to (parentsize width - ((min x * parentsize width) + new width)) / parentsize width;
                // anchor min y moves up to (parentsize height - ((max y * parentsize height) + new height) / parentsize height;
                float minX = NotNanOrInfinity(me.anchorMin.x) ? me.anchorMin.x : 0;
                minX = Mathf.Max(0, minX);
                minX = Mathf.Min(1, minX);
                float maxY = NotNanOrInfinity(me.anchorMax.y) ? me.anchorMax.y : 1;
                maxY = Mathf.Max(0, maxY);
                maxY = Mathf.Min(1, maxY);
                float maxX = ((minX * parentSize.x) + width) / parentSize.x;
                float minY = ((maxY * parentSize.y) - height) / parentSize.y;
                me.anchorMin = new Vector2(minX, minY);
                me.anchorMax = new Vector2(maxX, maxY);
                // remove any offsets
                me.offsetMin = new Vector2(0, 0);
                me.offsetMax = new Vector2(0, 0);
            }
            else
            {
                //ResizeAndPositionNonStretchy(me, child, child.rect.size, new Vector2(x, y));
            }
            print("*************************Resize DONE -" + gameObject.name + "::" + me.rect);
        }
        /// <summary>
        /// Resizes and positions a "non-stretching" UI element
        /// </summary>
        /// <param name="parent">the parent <see cref="RectTransform"/></param>
        /// <param name="child">the child <see cref="RectTransform"/></param>
        /// <param name="size">the element's new size</param>
        /// <param name="lowerLeft">the position of the element's lower-left corner</param>
        private void ResizeAndPositionNonStretchy(RectTransform parent, RectTransform child, Vector2 size, Vector2 lowerLeft)
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
            print("stretchy child placed at " + child.anchorMin + "x" + child.anchorMax);
        }
        public void SetLayoutHorizontal()
        {
            //throw new NotImplementedException();
        }
        public void SetLayoutVertical()
        {
            //throw new NotImplementedException();
        }
    }
}
