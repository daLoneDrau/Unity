using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGBase.Scripts.UI._2D
{
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// the instance field.
        /// </summary>
        private static CameraController instance;
        /// <summary>
        /// the singleton instance.
        /// </summary>
        public static CameraController Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject
                    {
                        name = "CameraController"
                    };
                    instance = go.AddComponent<CameraController>();
                    // persist this instance
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // set the desired aspect ratio (the values in this example are
            // hard-coded for 16:9, but you could make them into public
            // variables instead so you can set them at design time)
            // i changed to 4:3 for 1024x768 resolution
            // main camera is also set to Orthographic, and size is 24, making
            // camera's viewport 64x48 units. at 16px per tile, that is 1024x768
            float targetaspect = 4.0f / 3.0f;

            // determine the game window's current aspect ratio
            float windowaspect = (float)Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount
            float scaleheight = windowaspect / targetaspect;

            // obtain camera component so we can modify its viewport
            Camera camera = Camera.main;

            // if scaled height is less than current height, add letterbox
            if (scaleheight < 1.0f)
            {
                Rect rect = camera.rect;
                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;
                camera.rect = rect;

            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / scaleheight;

                Rect rect = camera.rect;

                rect.width = scalewidth;

                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;

            }
        }
    }
}
