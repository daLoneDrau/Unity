using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.UI.Controllers
{
    public class ViewportController : MonoBehaviour
    {
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
                }
                return instance;
            }
        }
        private float cameraWidth;
        private float cameraHeight;
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public Vector2 ViewportPosition { get; set; }
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
        /*
        [SerializeField]
        private GameObject marker;
        */
        /// <summary>
        /// the position of the last frame mouse click in WORLD space.
        /// </summary>
        private Vector3 lastFramePosition;
        private void Awake()
        {
            cameraHeight = 2f * Camera.main.orthographicSize;
            cameraWidth = cameraHeight * Camera.main.aspect;
            print("camera dimensions " + cameraWidth + "x" + cameraHeight);
        }
        // Use this for initialization
        void Start()
        {
        }
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
        // Update is called once per frame
        void Update()
        {
        }

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
    }
}
