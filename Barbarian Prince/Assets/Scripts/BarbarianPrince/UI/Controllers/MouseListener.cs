using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarbarianPrince.UI.Controllers
{
    public class MouseListener : MonoBehaviour
    {
        private static MouseListener instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static MouseListener Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject
                    {
                        name = "MouseListener"
                    };
                    instance = go.AddComponent<MouseListener>();
                }
                return instance;
            }
        }
        private MouseListener() { print("new MouseListener"); }
        /*
        [SerializeField]
        private GameObject marker;
        [SerializeField]
        private WorldController world;
        */
        /// <summary>
        /// the position of the last frame mouse click in WORLD space.
        /// </summary>
        private Vector3 lastFramePosition;
        private void Awake()
        {
            cameraHeight = 2f * Camera.main.orthographicSize;
            cameraWidth = cameraHeight * Camera.main.aspect;
        }
        // Use this for initialization
        void Start()
        {
        }
        private float cameraWidth;
        private float cameraHeight;
        // Update is called once per frame
        void Update()
        {
            Vector3 currMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currMousePos.z = 0; // fix z value so camera doesn't move along z-axis
                                /*
                                // update marker position
                                if (world.RequiresMarker(currMousePos))
                                {
                                    marker.SetActive(true);
                                    marker.transform.position = world.GetTileCoordinatesForWorldCoordinates(currMousePos);
                                }
                                else
                                {
                                    marker.SetActive(false);
                                }
                                */
                                // handle left-mouse clicks
            if (Input.GetMouseButtonDown(0))
            {
                // possible start of a drag
            }
            // handle left-mouse clicks
            if (Input.GetMouseButtonUp(0))
            {
                // possible end of a drag or just a click
            }
            // handle screen dragging
            if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
            {
                // middle button
                Vector3 diff = lastFramePosition - currMousePos; // get space between last position and current
                ViewportController.Instance.DragMap(diff);
            }
            else if (Input.GetMouseButton(1))
            {
                // right button down 
            }
            else if (Input.GetMouseButton(0))
            {
                // left button down 
            }
            lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastFramePosition.z = 0;
        }
    }
}
