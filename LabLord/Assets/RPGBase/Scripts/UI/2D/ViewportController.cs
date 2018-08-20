using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGBase.Scripts.UI._2D
{
    public class ViewportController : MonoBehaviour
    {
        /// <summary>
        /// the instance field.
        /// </summary>
        private static ViewportController instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static ViewportController Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject
                    {
                        name = "ViewportController"
                    };
                    instance = go.AddComponent<ViewportController>();
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
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        /// <summary>
        /// property for getting the dimensions of the required tile area displayed in the viewport.
        /// </summary>
        public Vector2 RequiredTileDimensions
        {
            get
            {
                int h = (int)cameraHeight + 1;
                int w = Mathf.CeilToInt(cameraWidth) + 1;
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
