using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class VerticalLayoutHandler : UIBehaviour, IRPGLayoutHandler
    {
        private float lastUpdate = -1f;
        public enum HorizontalAlignment
        {
            Left,
            Center,
            Right
        }
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
        [SerializeField] HorizontalAlignment HorizontalAlign = HorizontalAlignment.Left;
        [SerializeField] VerticalAlignment VerticalAlign = VerticalAlignment.Upper;
        public virtual void SetLayoutHorizontal()
        {
            if (gameObject.activeSelf)
            {
                float now = Time.realtimeSinceStartup;
                if (lastUpdate < 0)
                {
                    lastUpdate = now;
                    print("SetLayoutHorizontal::" + now);
                    UpdateRect();
                }
                else if (now - lastUpdate > 1f)
                {
                    lastUpdate = now;
                    print("SetLayoutHorizontal::" + now);
                    UpdateRect();
                }
            }
        }
        public virtual void SetLayoutVertical() { }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (gameObject.activeSelf)
            {
                float now = Time.realtimeSinceStartup;
                if (lastUpdate < 0)
                {
                    lastUpdate = now;
                    print("OnValidate::" + now);
                    UpdateRect();
                }
                else if (now - lastUpdate > 1f)
                {
                    lastUpdate = now;
                    print("OnValidate::" + now);
                    UpdateRect();
                }
            }
        }
#endif
        protected override void OnRectTransformDimensionsChange()
        {
            if (gameObject.activeSelf)
            {
                float now = Time.realtimeSinceStartup;
                if (lastUpdate < 0)
                {
                    lastUpdate = now;
                    print("OnRectTransformDimensionsChange::" + now);
                    UpdateRect();
                }
                else if (now - lastUpdate > 1f)
                {
                    lastUpdate = now;
                    print("OnRectTransformDimensionsChange::" + now);
                    UpdateRect();
                }
            }
        }
        Vector2 GetParentSize()
        {
            RectTransform parent = transform.parent as RectTransform;

            return parent == null ? Vector2.zero : parent.rect.size;
        }
        private void MoveToTopOfParent()
        {
            var rectTransform = GetComponent<RectTransform>();
            RectTransform parentTransform = rectTransform.parent as RectTransform;
            Vector2 myPivot = rectTransform.pivot;
            switch (HorizontalAlign)
            {
                case HorizontalAlignment.Right:
                    // try to move to right of parent
                    // right is parent width / 2
                    float pWidth = parentTransform.rect.width / 2;
                    // move to top should be offset by element width times pivot point
                    float myWidth = rectTransform.rect.width * (1 - rectTransform.pivot.x);
                    float myNewX = pWidth - myWidth;
                    if (myNewX != rectTransform.anchoredPosition.x)
                    {
                        float diff = 0;
                        if (myNewX != rectTransform.anchoredPosition.x)
                        {
                            if (myNewX < rectTransform.anchoredPosition.x)
                            {
                                // new location is left of current location
                                diff = rectTransform.anchoredPosition.x - myNewX;
                                rectTransform.anchoredPosition += Vector2.left * diff;
                            }
                            else
                            {
                                // new location is right of current location
                                diff = myNewX - rectTransform.anchoredPosition.x;
                                rectTransform.anchoredPosition += Vector2.right * diff;
                            }
                        }
                    }
                    break;
                case HorizontalAlignment.Center:
                    // try to move to center of parent
                    // middle is 0
                    pWidth = 0;
                    // move to middle should be offset by 1/2 element width times pivot point
                    myWidth = rectTransform.rect.width * (1 - rectTransform.pivot.x);
                    myWidth -= rectTransform.rect.width * 0.5F;
                    myNewX = pWidth - myWidth;
                    if (myNewX != rectTransform.anchoredPosition.x)
                    {
                        float diff = 0;
                        if (myNewX < rectTransform.anchoredPosition.x)
                        {
                            // new location is left of current location
                            diff = rectTransform.anchoredPosition.x - myNewX;
                            rectTransform.anchoredPosition += Vector2.left * diff;
                        }
                        else
                        {
                            // new location is right of current location
                            diff = myNewX - rectTransform.anchoredPosition.x;
                            rectTransform.anchoredPosition += Vector2.right * diff;
                        }
                    }
                    break;
                case HorizontalAlignment.Left:
                    // try to move to left of parent
                    // left is -parent width / 2
                    pWidth = -parentTransform.rect.width / 2;
                    // move to bottom should be offset by element width times pivot point
                    myWidth = rectTransform.rect.width * (1 - rectTransform.pivot.x);
                    myNewX = pWidth + myWidth;
                    if (myNewX != rectTransform.anchoredPosition.x)
                    {
                        float diff = 0;
                        if (myNewX < rectTransform.anchoredPosition.x)
                        {
                            // new location is left of current location
                            diff = rectTransform.anchoredPosition.x - myNewX;
                            rectTransform.anchoredPosition += Vector2.left * diff;
                        }
                        else
                        {
                            // new location is right of current location
                            diff = myNewX - rectTransform.anchoredPosition.x;
                            rectTransform.anchoredPosition += Vector2.right * diff;
                        }
                    }
                    break;
            }
            switch (VerticalAlign)
            {
                case VerticalAlignment.Upper:
                    // try to move to top of parent
                    // top is parent height / 2
                    float pHeight = parentTransform.rect.height / 2;
                    // move to top should be offset by element height times pivot point
                    float myHeight = rectTransform.rect.height * (1 - rectTransform.pivot.y);
                    float myNewY = pHeight - myHeight;
                    if (myNewY != rectTransform.anchoredPosition.y)
                    {
                        float diff = 0;
                        if (myNewY != rectTransform.anchoredPosition.y)
                        {
                            if (myNewY < rectTransform.anchoredPosition.y)
                            {
                                // new location is below current location
                                diff = rectTransform.anchoredPosition.y - myNewY;
                                rectTransform.anchoredPosition += Vector2.down * diff;
                            }
                            else
                            {
                                // new location is above current location
                                diff = myNewY - rectTransform.anchoredPosition.y;
                                rectTransform.anchoredPosition += Vector2.up * diff;
                            }
                        }
                    }
                    break;
                case VerticalAlignment.Middle:
                    // try to move to middle of parent
                    // middle is 0
                    pHeight = 0;
                    // move to middle should be offset by element height times pivot point
                    myHeight = rectTransform.rect.height * (1 - rectTransform.pivot.y);
                    myHeight -= rectTransform.rect.height * 0.5F;
                    myNewY = pHeight + myHeight;
                    if (myNewY != rectTransform.anchoredPosition.y)
                    {
                        float diff = 0;
                        if (myNewY < rectTransform.anchoredPosition.y)
                        {
                            // new location is below current location
                            diff = rectTransform.anchoredPosition.y - myNewY;
                            rectTransform.anchoredPosition += Vector2.down * diff;
                        }
                        else
                        {
                            // new location is above current location
                            diff = myNewY - rectTransform.anchoredPosition.y;
                            rectTransform.anchoredPosition += Vector2.up * diff;
                        }
                    }
                    break;
                case VerticalAlignment.Lower:
                    // try to move to bottom of parent
                    // bottom is -parent height / 2
                    pHeight = -parentTransform.rect.height / 2;
                    // move to bottom should be offset by element height times pivot point
                    myHeight = rectTransform.rect.height * (1 - rectTransform.pivot.y);
                    myNewY = pHeight + myHeight;
                    if (myNewY != rectTransform.anchoredPosition.y)
                    {
                        float diff = 0;
                        if (myNewY < rectTransform.anchoredPosition.y)
                        {
                            // new location is below current location
                            diff = rectTransform.anchoredPosition.y - myNewY;
                            rectTransform.anchoredPosition += Vector2.down * diff;
                        }
                        else
                        {
                            // new location is above current location
                            diff = myNewY - rectTransform.anchoredPosition.y;
                            rectTransform.anchoredPosition += Vector2.up * diff;
                        }
                    }
                    break;
            }
        }
        private void Resize(Vector2 size)
        {
            print("Resize(" + size);
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        }
        private float maxWidth;
        private float minHeight;
        private void IterateThroughChildren()
        {
            maxWidth = 0;
            minHeight = 0;
            var rectTransform = GetComponent<RectTransform>();
            foreach (Transform child in rectTransform)
            {
                if (!child.gameObject.activeSelf)
                {
                    continue;
                }
                if ((child.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler) != null)
                {
                    print("child has layout manager");
                    var lm = child.gameObject.GetComponent("IRPGLayoutHandler") as IRPGLayoutHandler;
                    print(lm);
                    lm.UpdateRect();
                }
                RectTransform rect = (RectTransform)child;
                print("*************************child");
                print(rect.rect);
                maxWidth = Mathf.Max(maxWidth, rect.rect.width);
                minHeight += rect.rect.height;
            }
            maxWidth += padding.Left + padding.Right;
            minHeight += padding.Top + padding.Bottom;
            minHeight += (rectTransform.childCount - 1) * spacing;
            print("spacing");
            print(rectTransform.childCount - 1);
            print(maxWidth + "," + minHeight);
        }
        private void PlaceChildHorizontally(RectTransform parent, RectTransform child)
        {
            switch (HorizontalAlign)
            {
                case HorizontalAlignment.Right:
                    // try to move to right of parent
                    // right is parent width / 2
                    float pWidth = parent.rect.width / 2;
                    // move to top should be offset by element width times pivot point
                    float myWidth = child.rect.width * (1 - child.pivot.x);
                    float myNewX = pWidth - myWidth - padding.Right;
                    if (myNewX != child.anchoredPosition.x)
                    {
                        float diff = 0;
                        if (myNewX != child.anchoredPosition.x)
                        {
                            if (myNewX < child.anchoredPosition.x)
                            {
                                // new location is left of current location
                                diff = child.anchoredPosition.x - myNewX;
                                child.anchoredPosition += Vector2.left * diff;
                            }
                            else
                            {
                                // new location is right of current location
                                diff = myNewX - child.anchoredPosition.x;
                                child.anchoredPosition += Vector2.right * diff;
                            }
                        }
                    }
                    break;
                case HorizontalAlignment.Center:
                    // try to move to center of parent
                    // middle is 0
                    pWidth = 0;
                    // move to middle should be offset by 1/2 element width times pivot point
                    myWidth = child.rect.width * (1 - child.pivot.x);
                    myWidth -= child.rect.width * 0.5F;
                    myNewX = pWidth - myWidth;
                    if (myNewX != child.anchoredPosition.x)
                    {
                        float diff = 0;
                        if (myNewX < child.anchoredPosition.x)
                        {
                            // new location is left of current location
                            diff = child.anchoredPosition.x - myNewX;
                            child.anchoredPosition += Vector2.left * diff;
                        }
                        else
                        {
                            // new location is right of current location
                            diff = myNewX - child.anchoredPosition.x;
                            child.anchoredPosition += Vector2.right * diff;
                        }
                    }
                    break;
                case HorizontalAlignment.Left:
                    // try to move to left of parent
                    // left is -parent width / 2
                    pWidth = -parent.rect.width / 2;
                    // move to bottom should be offset by element width times pivot point
                    myWidth = child.rect.width * (1 - child.pivot.x);
                    myNewX = pWidth + myWidth + padding.Left;
                    if (myNewX != child.anchoredPosition.x)
                    {
                        float diff = 0;
                        if (myNewX < child.anchoredPosition.x)
                        {
                            // new location is left of current location
                            diff = child.anchoredPosition.x - myNewX;
                            child.anchoredPosition += Vector2.left * diff;
                        }
                        else
                        {
                            // new location is right of current location
                            diff = myNewX - child.anchoredPosition.x;
                            child.anchoredPosition += Vector2.right * diff;
                        }
                    }
                    break;
            }
        }
        private void PlaceChildVertically(float top, RectTransform child)
        {
            Vector2 childPivot = child.pivot;
            // try to move to top of parent
            // top is parent height / 2
            // move to top should be offset by child height times pivot point
            float childHeight = child.rect.height * (1 - child.pivot.y);
            float myNewY = top - childHeight;
            if (myNewY != child.anchoredPosition.y)
            {
                float diff = 0;
                if (myNewY != child.anchoredPosition.y)
                {
                    if (myNewY < child.anchoredPosition.y)
                    {
                        // new location is below current location
                        diff = child.anchoredPosition.y - myNewY;
                        child.anchoredPosition += Vector2.down * diff;
                    }
                    else
                    {
                        // new location is above current location
                        diff = myNewY - child.anchoredPosition.y;
                        child.anchoredPosition += Vector2.up * diff;
                    }
                }
            }
        }
        private void PlaceChildren()
        {
            float top = minHeight / 2f;
            top -= padding.Top;
            var rectTransform = GetComponent<RectTransform>();
            foreach (Transform child in rectTransform)
            {
                var childTransform = child as RectTransform;
                PlaceChildHorizontally(rectTransform, childTransform);
                PlaceChildVertically(top, childTransform);
                print("child placed at " + childTransform.anchoredPosition);
                // move top position down for next child
                top -= childTransform.rect.height;
                top -= spacing;
            }
        }
        protected override void OnEnable()
        {
            print("+++++++++++++++++++script was enabled");
            // if enabled and parent is enabled, tell parent to update
        }
        protected override void OnDisable()
        {
            print("----------------------script was disabled");
            // if disabled and parent is enabled, tell parent to update
        }
        public void TaskOnClick()
        {
            Debug.Log("You have clicked the button!");
            print(gameObject.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
            print(gameObject.activeSelf);
        }
        public void UpdateRect()
        {
            print("UpdateRect");
            IterateThroughChildren();
            Resize(new Vector2(maxWidth, minHeight));
            PlaceChildren();
            // Resize(new Vector2(500, 500));
            /*
            RectTransform rect = (RectTransform)transform;
            Vector2 parentSize = GetParentSize();

            rect.SetInsetAndSizeFromParentEdge(IndentEdgeToRectEdge(m_Edge), parentSize.y + border, parentSize.x - parentSize.y);
            */
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
