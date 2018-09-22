using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WoFM.UI.SceneControllers;

namespace WoFM.UI._2D
{
    public class ShadowcastFOV
    {
        /// <summary>
        /// flag indicating a tile hasn't been scanned.
        /// </summary>
        public const int UNSCANNED = 0;
        public const int LIT = 1;
        public const int BLOCKED = 2;
        /// <summary>
        /// As part of the lighting algorithm, checks all tiles surrounding the starting point for visibility.
        /// </summary>
        /// <param name="start">the starting point</param>
        /// <param name="radius">the radius</param>
        /// <param name="slope">slope is change in y over change in x</param>
        public void CheckQuadrant1(Vector2 start, int radius, int startingRow = 1, float startSlope = -1f, float endSlope = float.PositiveInfinity, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 1");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingRow:" + startingRow + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // py is the row that is being checked
            float py = start.y + startingRow;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for x: (y-b)/m=x
            float x = (py - b) / startSlope;
            float y = startSlope * start.x + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.FloorToInt(x); j <= start.x; j++)
            {
                float endingSlope = (py - start.y) / ((float)j - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope > endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + j + "," + (int)py);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt(j, (int)py);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last row
                    if (startingRow < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the row
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.FloorToInt(x))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check to left of blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by to the left of blocking cell
                                float newEndSlope = GetSlopeQ1(start, new Vector2(j, py), true);
                                // make recursive call to check next row
                                CheckQuadrant1(start,   // same start point
                                    radius,             // same radius
                                    startingRow + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last row
                        if (startingRow < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check to right of blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by to the right of blocking cell
                            float newStartSlope = GetSlopeQ1(start, new Vector2(j, py), false);
                            // make recursive call to check next row
                            CheckQuadrant1(start,   // same start point
                            radius,             // same radius
                            startingRow + 1,    // next row
                            newStartSlope,
                            endSlope);          // same end slope
                        }
                    }
                }
                if (j == start.x
                    || ((py - start.y) / ((float)j + 1 - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in row");
                    }
                    // reached the last cell in the row, and not on last row
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingRow < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant1(start,   // same start point
                        radius,             // same radius
                        startingRow + 1,    // next row
                        startSlope,         // same start slope
                        endSlope);          // same end slope
                    }
                }
            }
        }
        public void CheckQuadrant2(Vector2 start, int radius, int startingRow = 1, float startSlope = 1f, float endSlope = float.PositiveInfinity, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 2");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingRow:" + startingRow + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // py is the row that is being checked
            float py = start.y + startingRow;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for x: (y-b)/m=x
            float x = (py - b) / startSlope;
            if (debug)
            {
                Debug.Log("b::" + b + "||x:" + x);
            }
            float y = startSlope * start.x + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.CeilToInt(x); j >= start.x; j--)
            {
                float endingSlope = (py - start.y) / ((float)j - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope > endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + j + "," + (int)py);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt(j, (int)py);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last row
                    if (startingRow < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the row
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.CeilToInt(x))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check to right of blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by to the right of blocking cell
                                float newEndSlope = GetSlopeQ2(start, new Vector2(j, py), false);
                                // make recursive call to check next row
                                CheckQuadrant2(start,   // same start point
                                    radius,             // same radius
                                    startingRow + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last row
                        if (startingRow < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check to left of blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by to the left of blocking cell
                            float newStartSlope = GetSlopeQ2(start, new Vector2(j, py), true);
                            // make recursive call to check next row
                            CheckQuadrant2(start,   // same start point
                            radius,             // same radius
                            startingRow + 1,    // next row
                            newStartSlope,
                            endSlope);          // same end slope
                        }
                    }
                }
                if (j == start.x
                    || ((py - start.y) / ((float)j - 1 - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in row");
                    }
                    // reached the last cell in the row
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingRow < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant2(start,   // same start point
                        radius,             // same radius
                        startingRow + 1,    // next row
                        startSlope,         // same start slope
                        endSlope);          // same end slope
                    }
                }
            }
        }
        public void CheckQuadrant3(Vector2 start, int radius, int startingCol = 1, float startSlope = 1f, float endSlope = 0, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 3");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingCol:" + startingCol + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // px is the column that is being checked
            float px = start.x + startingCol;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for y: y=mx+b
            float y = startSlope * px + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.CeilToInt(y); j >= start.y; j--)
            {
                float endingSlope = (j - start.y) / (px - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope < endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + (int)px + "," + j);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt((int)px, j);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last column
                    if (startingCol < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the column
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.CeilToInt(y))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check to top of blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by to the top of blocking cell
                                float newEndSlope = GetSlopeQ3(start, new Vector2(px, j), true);
                                // make recursive call to check next row
                                CheckQuadrant3(start,   // same start point
                                    radius,             // same radius
                                    startingCol + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope,
                                    debug);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last column
                        if (startingCol < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check to bottom of blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by to the bottom of blocking cell
                            float newStartSlope = GetSlopeQ3(start, new Vector2(px, j), false);
                            // make recursive call to check next row
                            CheckQuadrant3(start,   // same start point
                            radius,             // same radius
                            startingCol + 1,    // next row
                            newStartSlope,
                            endSlope,           // same end slope
                            debug);
                        }
                    }
                }
                if (j == start.y
                    || ((j - 1 - start.y) / (px - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in column");
                    }
                    // reached the last cell in the column
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingCol < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant3(start,   // same start point
                        radius,             // same radius
                        startingCol + 1,    // next column
                        startSlope,         // same start slope
                        endSlope,           // same end slope
                        debug);
                    }
                }
            }
        }
        public void CheckQuadrant4(Vector2 start, int radius, int startingCol = 1, float startSlope = -1f, float endSlope = 0, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 4");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingCol:" + startingCol + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // px is the column that is being checked
            float px = start.x + startingCol;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for y: y=mx+b
            float y = startSlope * px + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.FloorToInt(y); j <= start.y; j++)
            {
                float endingSlope = (j - start.y) / (px - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope > endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + (int)px + "," + j);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt((int)px, j);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last column
                    if (startingCol < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the column
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.FloorToInt(y))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check before blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by to the bottom of blocking cell
                                float newEndSlope = GetSlopeQ4(start, new Vector2(px, j), true);
                                // make recursive call to check next row
                                CheckQuadrant4(start,   // same start point
                                    radius,             // same radius
                                    startingCol + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope,
                                    debug);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last column
                        if (startingCol < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check after blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by to the bottom of blocking cell
                            float newStartSlope = GetSlopeQ4(start, new Vector2(px, j), false);
                            // make recursive call to check next row
                            CheckQuadrant4(start,   // same start point
                            radius,             // same radius
                            startingCol + 1,    // next row
                            newStartSlope,
                            endSlope,           // same end slope
                            debug);
                        }
                    }
                }
                if (j == start.y
                    || ((j - 1 - start.y) / (px - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in column");
                    }
                    // reached the last cell in the column
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingCol < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant4(start,   // same start point
                        radius,             // same radius
                        startingCol + 1,    // next column
                        startSlope,         // same start slope
                        endSlope,           // same end slope
                        debug);
                    }
                }
            }
        }
        public void CheckQuadrant5(Vector2 start, int radius, int startingRow = 1, float startSlope = -1f, float endSlope = float.PositiveInfinity, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 5");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingRow:" + startingRow + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // py is the row that is being checked
            float py = start.y - startingRow;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for x: (y-b)/m=x
            float x = (py - b) / startSlope;
            if (debug)
            {
                Debug.Log("b::" + b + "||x:" + x);
            }
            float y = startSlope * start.x + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.CeilToInt(x); j >= start.x; j--)
            {
                float endingSlope = (py - start.y) / ((float)j - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope > endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + j + "," + (int)py);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt(j, (int)py);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last row
                    if (startingRow < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the row
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.CeilToInt(x))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check before blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by to the right of blocking cell
                                float newEndSlope = GetSlopeQ5(start, new Vector2(j, py), true);
                                // make recursive call to check next row
                                CheckQuadrant5(start,   // same start point
                                    radius,             // same radius
                                    startingRow + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last row
                        if (startingRow < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check after blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by after blocking cell
                            float newStartSlope = GetSlopeQ5(start, new Vector2(j, py), false);
                            // make recursive call to check next row
                            CheckQuadrant5(start,   // same start point
                            radius,             // same radius
                            startingRow + 1,    // next row
                            newStartSlope,
                            endSlope);          // same end slope
                        }
                    }
                }
                if (j == start.x
                    || ((py - start.y) / ((float)j - 1 - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in row");
                    }
                    // reached the last cell in the row
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingRow < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant5(start,   // same start point
                        radius,             // same radius
                        startingRow + 1,    // next row
                        startSlope,         // same start slope
                        endSlope);          // same end slope
                    }
                }
            }
        }
        public void CheckQuadrant6(Vector2 start, int radius, int startingRow = 1, float startSlope = 1f, float endSlope = float.PositiveInfinity, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 6");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingRow:" + startingRow + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // py is the row that is being checked
            float py = start.y - startingRow;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for x: (y-b)/m=x
            float x = (py - b) / startSlope;
            float y = startSlope * start.x + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.FloorToInt(x); j <= start.x; j++)
            {
                float endingSlope = (py - start.y) / ((float)j - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope > endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + j + "," + (int)py);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt(j, (int)py);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last row
                    if (startingRow < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the row
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.FloorToInt(x))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check before blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by before blocking cell
                                float newEndSlope = GetSlopeQ6(start, new Vector2(j, py), true);
                                // make recursive call to check next row
                                CheckQuadrant6(start,   // same start point
                                    radius,             // same radius
                                    startingRow + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last row
                        if (startingRow < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check after blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by after blocking cell
                            float newStartSlope = GetSlopeQ6(start, new Vector2(j, py), false);
                            // make recursive call to check next row
                            CheckQuadrant6(start,   // same start point
                            radius,             // same radius
                            startingRow + 1,    // next row
                            newStartSlope,
                            endSlope);          // same end slope
                        }
                    }
                }
                if (j == start.x
                    || ((py - start.y) / ((float)j + 1 - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in row");
                    }
                    // reached the last cell in the row, and not on last row
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingRow < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant6(start,   // same start point
                        radius,             // same radius
                        startingRow + 1,    // next row
                        startSlope,         // same start slope
                        endSlope);          // same end slope
                    }
                }
            }
        }
        public void CheckQuadrant7(Vector2 start, int radius, int startingCol = 1, float startSlope = 1f, float endSlope = 0, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 7");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingCol:" + startingCol + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // px is the column that is being checked
            float px = start.x - startingCol;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for y: y=mx+b
            float y = startSlope * px + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.FloorToInt(y); j <= start.y; j++)
            {
                float endingSlope = (j - start.y) / (px - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope < endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + (int)px + "," + j);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt((int)px, j);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last column
                    if (startingCol < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the column
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.FloorToInt(y))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check before blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by to the bottom of blocking cell
                                float newEndSlope = GetSlopeQ7(start, new Vector2(px, j), true);
                                // make recursive call to check next row
                                CheckQuadrant7(start,   // same start point
                                    radius,             // same radius
                                    startingCol + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope,
                                    debug);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last column
                        if (startingCol < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check after blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by to the bottom of blocking cell
                            float newStartSlope = GetSlopeQ7(start, new Vector2(px, j), false);
                            // make recursive call to check next row
                            CheckQuadrant7(start,   // same start point
                            radius,             // same radius
                            startingCol + 1,    // next row
                            newStartSlope,
                            endSlope,           // same end slope
                            debug);
                        }
                    }
                }
                if (j == start.y
                    || ((j - 1 - start.y) / (px - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in column");
                    }
                    // reached the last cell in the column
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingCol < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant7(start,   // same start point
                        radius,             // same radius
                        startingCol + 1,    // next column
                        startSlope,         // same start slope
                        endSlope,           // same end slope
                        debug);
                    }
                }
            }
        }
        public void CheckQuadrant8(Vector2 start, int radius, int startingCol = 1, float startSlope = -1f, float endSlope = 0, bool debug = false)
        {
            if (debug)
            {
                Debug.Log("*********************************\nQuadrant 8");
                Debug.Log("start:" + start + "||radius:" + radius + "||startingCol:" + startingCol + "||startSlope:" + startSlope + "||endSlope" + endSlope);
            }
            // px is the column that is being checked
            float px = start.x - startingCol;
            // find point that will be first cell checked
            // equation of a line is y=mx+b
            // solve for b: b = y-mx
            float b = start.y - (startSlope * start.x);
            // solve for y: y=mx+b
            float y = startSlope * px + b;
            bool blockingSection = false, hadBlocker = false;
            for (int j = Mathf.CeilToInt(y); j >= start.y; j--)
            {
                float endingSlope = (j - start.y) / (px - start.x);
                // when we go past the end slope point, break out of the loop
                if (endingSlope > endSlope)
                {
                    break;
                }
                if (debug)
                {
                    Debug.Log("checking tile " + (int)px + "," + j);
                }
                WoFMTile tile = WorldController.Instance.GetTileAt((int)px, j);
                // skip missing tiles
                if (tile == null) { continue; }
                // light the tile up
                tile.ShadeLevel = LIT;
                if (tile.IsLightBlocker())
                {
                    hadBlocker = true;
                    if (debug)
                    {
                        Debug.Log("BLOCKER!");
                    }
                    // FOUND A BLOCKER!
                    // if this is not the last column
                    if (startingCol < radius)
                    {
                        // are we on a blocking section? 
                        if (!blockingSection)
                        {
                            // just starting a blocking section
                            blockingSection = true;
                            // check to see if this was the first tile in the column
                            // first tile blockers are treated like existing
                            // blocking sections and the next row isn't checked
                            if (j != Mathf.CeilToInt(y))
                            {
                                if (debug)
                                {
                                    Debug.Log("recursive check before blocker");
                                }
                                // end slope is slope of line that ends at the 
                                // point that brushes by before blocking cell
                                float newEndSlope = GetSlopeQ8(start, new Vector2(px, j), true);
                                // make recursive call to check next row
                                CheckQuadrant8(start,   // same start point
                                    radius,             // same radius
                                    startingCol + 1,    // next row
                                    startSlope,         // same start slope
                                    newEndSlope,
                                    debug);
                            }
                        }
                    }
                }
                else
                {
                    // are we on a blocking section?
                    if (blockingSection)
                    {
                        // just finished a blocking section! remove the flag
                        blockingSection = false;
                        // if this is not the last column
                        if (startingCol < radius)
                        {
                            if (debug)
                            {
                                Debug.Log("recursive check to bottom of blocker");
                            }
                            // start slope is slope of line that ends at the 
                            // point that brushes by after blocking cell
                            float newStartSlope = GetSlopeQ8(start, new Vector2(px, j), false);
                            // make recursive call to check next row
                            CheckQuadrant8(start,   // same start point
                            radius,             // same radius
                            startingCol + 1,    // next row
                            newStartSlope,
                            endSlope,           // same end slope
                            debug);
                        }
                    }
                }
                if (j == start.y
                    || ((j - 1 - start.y) / (px - start.x) > endSlope))
                {
                    if (debug)
                    {
                        Debug.Log("reached last cell in column");
                    }
                    // reached the last cell in the column
                    if (!tile.IsLightBlocker()
                        && !hadBlocker
                        && startingCol < radius)
                    {
                        if (debug)
                        {
                            Debug.Log("recursive check for clear row");
                        }
                        // last cell was not a light blocker and this row had no blockers
                        // make recursive call to check next row
                        CheckQuadrant8(start,   // same start point
                        radius,             // same radius
                        startingCol + 1,    // next column
                        startSlope,         // same start slope
                        endSlope,           // same end slope
                        debug);
                    }
                }
            }
        }
        public float GetSlopeQ1(Vector2 startPoint, Vector2 endPoint, bool isLeft)
        {
            float x1 = 0, y1 = 0, x2, y2;

            y2 = endPoint.y - startPoint.y;
            y2--;
            y2 *= 16f;
            y2 += 7f;
            if (isLeft)
            {
                y2++;
            }
            else
            {
                y2 += 16f;
            }
            x2 = endPoint.x - startPoint.x;
            x2 *= 16f;
            x2 -= 8f;
            if (isLeft)
            {
                x2--;
            }
            return (y2 - y1) / (x2 - x1);
        }
        public float GetSlopeQ2(Vector2 startPoint, Vector2 endPoint, bool isLeft)
        {
            float x1 = 0, y1 = 0, x2, y2;

            y2 = endPoint.y - startPoint.y;
            y2--;
            y2 *= 16f;
            y2 += 7f;
            if (isLeft)
            {
                y2 += 16f;
            }
            else
            {
                y2++;
            }

            x2 = endPoint.x - startPoint.x;
            x2 *= 16f;
            x2 += 9f;
            if (isLeft)
            {
                x2--;
            }
            return (y2 - y1) / (x2 - x1);
        }
        public float GetSlopeQ3(Vector2 startPoint, Vector2 endPoint, bool isTop)
        {
            float x1 = 0, y1 = 0, x2, y2;

            x2 = endPoint.x - startPoint.x;
            x2--;
            x2 *= 16f;
            x2 += 7f;
            if (isTop)
            {
                x2++;
            }
            else
            {
                x2 += 16f;
            }

            y2 = endPoint.y - startPoint.y;
            y2 *= 16f;
            y2 += 9f;
            if (!isTop)
            {
                y2--;
            }
            return (y2 - y1) / (x2 - x1);
        }
        public float GetSlopeQ4(Vector2 startPoint, Vector2 endPoint, bool beforeBlocker)
        {
            float x1 = 0, y1 = 0, x2, y2;

            x2 = endPoint.x - startPoint.x;
            x2--;
            x2 *= 16f;
            x2 += 7f;
            if (beforeBlocker)
            {
                x2++;
            }
            else
            {
                x2 += 16f;
            }

            y2 = endPoint.y - startPoint.y;
            y2 *= 16f;
            y2 -= 9f;
            if (!beforeBlocker)
            {
                y2++;
            }
            return (y2 - y1) / (x2 - x1);
        }
        public float GetSlopeQ5(Vector2 startPoint, Vector2 endPoint, bool beforeBlocker)
        {
            float x1 = 0, y1 = 0, x2, y2;

            y2 = endPoint.y - startPoint.y;
            y2++;
            y2 *= 16f;
            y2 -= 7f;
            if (beforeBlocker)
            {
                y2--;   
            }
            else
            {
                y2 -= 16f;
            }

            x2 = endPoint.x - startPoint.x;
            x2 *= 16f;
            x2 += 9f;
            if (!beforeBlocker)
            {
                x2--;
            }
            return (y2 - y1) / (x2 - x1);
        }
        public float GetSlopeQ6(Vector2 startPoint, Vector2 endPoint, bool beforeBlocker)
        {
            float x1 = 0, y1 = 0, x2, y2;

            y2 = endPoint.y - startPoint.y;
            y2++;
            y2 *= 16f;
            y2 -= 7f;
            if (beforeBlocker)
            {
                y2--;
            }
            else
            {
                y2 -= 16f;
            }

            x2 = endPoint.x - startPoint.x;
            x2 *= 16f;
            x2 -= 8f;
            if (!beforeBlocker)
            {
                x2--;
            }
            return (y2 - y1) / (x2 - x1);
        }
        public float GetSlopeQ7(Vector2 startPoint, Vector2 endPoint, bool beforeBlocker)
        {
            float x1 = 0, y1 = 0, x2, y2;

            x2 = endPoint.x - startPoint.x;
            x2++;
            x2 *= 16f;
            x2 -= 7f;
            if (beforeBlocker)
            {
                x2--;
            }
            else
            {
                x2 -= 16f;
            }

            y2 = endPoint.y - startPoint.y;
            y2 *= 16f;
            y2 -= 9f;
            if (!beforeBlocker)
            {
                y2++;
            }
            return (y2 - y1) / (x2 - x1);
        }
        public float GetSlopeQ8(Vector2 startPoint, Vector2 endPoint, bool beforeBlocker)
        {
            float x1 = 0, y1 = 0, x2, y2;

            x2 = endPoint.x - startPoint.x;
            x2++;
            x2 *= 16f;
            x2 -= 7f;
            if (beforeBlocker)
            {
                x2--;
            }
            else
            {
                x2 -= 16f;
            }

            y2 = endPoint.y - startPoint.y;
            y2 *= 16f;
            y2 += 9f;
            if (!beforeBlocker)
            {
                y2--;
            }
            return (y2 - y1) / (x2 - x1);
        }
    }
}
