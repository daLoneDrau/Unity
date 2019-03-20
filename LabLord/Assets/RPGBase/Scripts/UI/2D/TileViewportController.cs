using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGBase.Scripts.UI._2D
{
    public class TileViewportController : MonoBehaviour
    {
        /// <summary>
        /// the instance field.
        /// </summary>
        private static TileViewportController instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static TileViewportController Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject
                    {
                        name = "TileViewportController"
                    };
                    instance = go.AddComponent<TileViewportController>();
                    // persist this instance
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }
        /// <summary>
        /// the height of the camera
        /// </summary>
        private float cameraHeight;
        /// <summary>
        /// the width of the camera
        /// </summary>
        private float cameraWidth;
        /// <summary>
        /// Property to set the viewport's dimensions, based on the number of tiles it covers.  A dimension of 16x12 covers an area 16 tiles wide, 12 tiles high.  If not set, the viewport will cover the entire screen.
        /// </summary>
        public Vector2 ViewportTileDimensions { get; set; }
        /// <summary>
        /// The left-most tile that exists within the world map.
        /// </summary>
        public int MaxX { get; set; }
        /// <summary>
        /// the upper-most tile that exists within the world map.
        /// </summary>
        public int MaxY { get; set; }
        /// <summary>
        /// property for getting the dimensions of the required tile area displayed in the viewport.
        /// </summary>
        public Vector2 RequiredTileDimensions
        {
            get
            {
                int h, w;
                if (ViewportTileDimensions != null)
                {
                    w = (int)ViewportTileDimensions.x;
                    h = (int)ViewportTileDimensions.y;
                }
                else
                {

                    h = (int)cameraHeight + 1;
                    w = Mathf.CeilToInt(cameraWidth) + 1;
                }
                return new Vector2(w, h);
            }
        }
        public Vector2 ViewportPosition { get; set; }
        #region MonoBehaviour messages
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            cameraHeight = 2f * Camera.main.orthographicSize;
            cameraWidth = cameraHeight * Camera.main.aspect;
            print("camera dimensions " + cameraWidth + "x" + cameraHeight);
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
        }
        #endregion
        public void CenterOnPoint(Vector2 vector2)
        {
            vector2 += new Vector2(-cameraWidth * .5f, -cameraHeight * .5f);
            ViewportPosition = vector2;
            // did view go off edge of map?
            if (ViewportPosition.x < 0 || ViewportPosition.y < 0 || (ViewportPosition.x + cameraWidth) > MaxX || (ViewportPosition.y + cameraHeight) > MaxY)
            {
                // going off edge of map. move back
                if (ViewportPosition.x < 0)
                {
                    ViewportPosition = new Vector2(0, ViewportPosition.y);
                }
                else if (MaxX > 0 && (ViewportPosition.x + cameraWidth) > MaxX)
                {
                    ViewportPosition = new Vector2(MaxX - cameraWidth, ViewportPosition.y);
                }
                if (ViewportPosition.y < 0)
                {
                    ViewportPosition = new Vector2(ViewportPosition.x, 0);
                }
                else if (MaxY > 0 && (ViewportPosition.y + cameraHeight) > MaxY)
                {
                    ViewportPosition = new Vector2(ViewportPosition.x, MaxY - cameraHeight);
                }
            }
        }
        /// <summary>
        /// Centers the veiwport on a specific tile.
        /// </summary>
        /// <param name="tileCoords">the tile's coordinates</param>
        public void CenterOnTile(Vector2 tileCoords)
        {
            // print("find center point for " + tileCoords);
            // print("ViewportTileDimensions::" + ViewportTileDimensions);
            // print("maximum boundaries::" + MaxX + "," + MaxY);
            // calculate the position the viewport needs to be at to center the coordinates within the view
            float x = tileCoords.x - ((ViewportTileDimensions.x - 1f) / 2f);
            float y = tileCoords.y - ((ViewportTileDimensions.y - 1f) / 2f);
            // print("perfect center is at " + x + "," + y);
            if (x < 0) { x = 0; }
            if (x + ViewportTileDimensions.x > MaxX)
            {
                // x pushes view past right-side boundary
                x = MaxX - ViewportTileDimensions.x;
                // print("too far out on x-axis, go back");
            }
            if (y < 0) { y = 0; }
            if (y + ViewportTileDimensions.y > MaxY)
            {
                // y pushes view past top-side boundary
                y = MaxY - ViewportTileDimensions.y;
                // print("too far out on y-axis, go back");
            }
            ViewportPosition = new Vector2(x, y);
            // print("center on tile " + tileCoords + " at " + new Vector2(x, y));
            // print("viewport at " + ViewportPosition.x + "," + ViewportPosition.y + "\nrange from " + Mathf.FloorToInt(x) + "," + Mathf.FloorToInt(y) + " to " + (Mathf.FloorToInt(x)+ViewportTileDimensions.x) + "," + (Mathf.FloorToInt(y)+ViewportTileDimensions.y));
        }
        public void DragMap(Vector3 diff)
        {
            ViewportPosition += (Vector2)diff;
            // did view go off edge of map?
            if (ViewportPosition.x < 0 || ViewportPosition.y < 0 || (ViewportPosition.x + cameraWidth) > MaxX || (ViewportPosition.y + cameraHeight) > MaxY)
            {
                // going off edge of map. move back
                if (ViewportPosition.x < 0)
                {
                    ViewportPosition = new Vector2(0, ViewportPosition.y);
                }
                else if (MaxX > 0 && (ViewportPosition.x + cameraWidth) > MaxX)
                {
                    ViewportPosition = new Vector2(MaxX - cameraWidth, ViewportPosition.y);
                }
                if (ViewportPosition.y < 0)
                {
                    ViewportPosition = new Vector2(ViewportPosition.x, 0);
                }
                else if (MaxY > 0 && (ViewportPosition.y + cameraHeight) > MaxY)
                {
                    ViewportPosition = new Vector2(ViewportPosition.x, MaxY - cameraHeight);
                }
            }
        }
        /// <summary>
        /// Gets the screen coordinates for a location in the world.
        /// </summary>
        /// <param name="pt">the world location</param>
        /// <returns><see cref="Vector2"/></returns>
        public Vector2 GetScreenCoordinatesForWorldPosition(Vector2 pt,bool debug=false)
        {
            // get the fractional part of the viewport's position. if viewport is at 0.5,10.3, then dx=-.5, dy=-.3
            float dx = ViewportPosition.x - (float)Math.Truncate(ViewportPosition.x);
            float dy = ViewportPosition.y - (float)Math.Truncate(ViewportPosition.y);
            dx *= -1;
            dy *= -1;
            if (debug)
            {
                print("viewport::" + ViewportPosition.x+","+ViewportPosition.y + "\t||pt:" + pt.x + "," + pt.y+"||unit:" + (pt.y - ViewportPosition.y + Y_OFFSET));
            }
            return new Vector2(pt.x -ViewportPosition.x + X_OFFSET, pt.y -ViewportPosition.y + Y_OFFSET);
        }
        /// <summary>
        /// the viewport's x-offset is 1, since the 1st tile is covered by the map border.
        /// </summary>
        public static int X_OFFSET = 1;
        /// <summary>
        /// the viewport's y-offset is 24; the map is 24 units up and the 1st tile is covered by the map border.
        /// </summary>
        public static int Y_OFFSET = 25;
        /// <summary>
        /// Gets the "on-screen" coordinates for a tile
        /// </summary>
        /// <param name="tile"></param>
        public Vector2 GetWorldCoordinatesForTile(Vector2 tile)
        {
            // print("viewport is at " + ViewportPosition);
            // get the value of the left and bottom edges of the viewport
            int minx = Mathf.FloorToInt(ViewportPosition.x);
            int miny = Mathf.FloorToInt(ViewportPosition.y);
            // get the fractional part of the viewport's position
            float dx = ViewportPosition.x - (float)Math.Truncate(ViewportPosition.x);
            float dy = ViewportPosition.y - (float)Math.Truncate(ViewportPosition.y);
            dx *= -1;
            dy *= -1;
            // world x-position of the tile is at the tile's coordinates - viewport's left position + fractional part of viewport's position + viewport's x-offset
            float newX = tile.x - minx + dx + X_OFFSET;
            float newY = tile.y - miny + dy + Y_OFFSET;
            // print("tile " + tile + " should appear at " + new Vector2(newX, newY));
            return new Vector2(newX, newY);
        }
        public void PositionViewport(Vector2 v)
        {
            ViewportPosition = v;
            // camera's 0,0 is at this position
            Vector2 c = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            // move camera's 0,0 to requested world position
            Vector2 diff = v - c; // get space between world position and camera's 0,0
            Camera.main.transform.Translate(diff); // move camera by difference
        }
    }
}
