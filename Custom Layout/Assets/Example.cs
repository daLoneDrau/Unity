using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    /// <summary>
    /// Calculates the size of a child from its parent's dimensions
    /// </summary>
    /// <param name="parent">the parent transform</param>
    /// <param name="child">the child transform</param>
    /// <returns><see cref="Vector2"/></returns>
    Vector2 CalculateSizeFromAnchors(RectTransform parent, RectTransform child)
    {
        float widthPercent = child.anchorMax.x - child.anchorMin.x; // right-left
        float heightPercent = child.anchorMax.y - child.anchorMin.y; //top - bottom
        return new Vector2(widthPercent * parent.rect.size.x, heightPercent * parent.rect.size.y);
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
    /// <summary>
    /// Resizes and positions a "stretching" UI element
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <param name="size"></param>
    /// <param name="lowerLeft"></param>
    void ResizeAndPositionStretchy(RectTransform parent, RectTransform child, Vector2 size, Vector2 lowerLeft)
    {
        Vector2 parentSize = parent.rect.size;
        float minX = lowerLeft.x;
        float maxX = -(parentSize.x - size.x);
        float minY = lowerLeft.y;
        float maxY = -(parentSize.y - size.y);
        child.offsetMin = new Vector2(minX, minY);
        child.offsetMax = new Vector2(minX + maxX, minY + maxY);
    }
    void ResizeAndPositionNonStretchy(RectTransform parent, RectTransform child, Vector2 size, Vector2 lowerLeft)
    {
        print("ResizeAndPositionNonStretchy");
        Vector2 parentSize = parent.rect.size;
        // size delta is the difference in size between an element's actual size and the size
        // of the rectangle made up by its anchors.  if an element is 500x300, while its anchors cover an area
        // 600x400, then the size delta is -100,-100.  if the element was 600x400 and its anchors covered 400x300,
        // then the size delta would be 200,100;
        print("sizeDelta::" + child.sizeDelta);
        print("rect::" + child.rect);
        print(child.anchoredPosition);
        // size delta is going to be the element's actual size.  reset it to the desired size
        child.sizeDelta = size;
        // new position should be adjusted to consider element size, child size and child pivot position
        float x = -parentSize.x / 2f;
        x += lowerLeft.x + (child.sizeDelta.x * child.pivot.x);
        float y = -parentSize.y / 2f;
        y += lowerLeft.y + (child.sizeDelta.y * child.pivot.y);
        child.anchoredPosition = new Vector2(x, y);
    }
    void Awake()
    {
        RectTransform me = GetComponent<RectTransform>();
        print("Awake");
        print("parent::" + me.rect.size);
        // position elements based on assumption that local space of parent, lower-left corner is 0,0
        foreach (Transform child in transform)
        {
            var m_RectTransform = child.GetComponent<RectTransform>();
            Vector2 currentSize = CalculateSizeFromAnchors(me, m_RectTransform);
            print("child stretchy? " + IsStretching(m_RectTransform));
            print("calculated child size::" + currentSize);
            if (IsStretching(m_RectTransform))
            {
                print("stretchy child");
                ResizeAndPositionStretchy(me, m_RectTransform, new Vector2(50, 50), new Vector2(50,50));
                print("tried to resize to 200x200::"+m_RectTransform.rect);
            } else
            {
                ResizeAndPositionNonStretchy(me, m_RectTransform, new Vector2(50, 50), new Vector2(50, 50));
            }
        }
    }
}
