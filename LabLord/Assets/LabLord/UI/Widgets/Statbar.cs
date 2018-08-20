using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WoFM.UI.Widgets
{
    public class Statbar : MonoBehaviour
    {
        private Image image;
        private float maxWidth;
        private void Awake()
        {
            image = GetComponent<Image>();
            maxWidth = image.rectTransform.rect.width;
        }
        public void Set(float current, float max)
        {
            if (current > max)
            {
                current = max;
            }
            float size = maxWidth * (current / max);
            image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMin.x + size, image.rectTransform.offsetMax.y);
        }
    }
}
