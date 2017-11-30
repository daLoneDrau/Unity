using UnityEngine;
using UnityEngine.UI;

public class StatusBarScript : MonoBehaviour
{
    /// <summary>
    /// the image backing the status bar.
    /// </summary>
    private Image image;
    /// <summary>
    /// the maximum value
    /// </summary>
    public int maxValue { get; set; }
    /// <summary>
    /// the current value.
    /// </summary>
    private int currentValue { get; set; }
    private float imgWidth = -999f;
    /// <summary>
    /// Adjusts the status bar by a set value.
    /// </summary>
    /// <param name="value">the value being adjusted by</param>
    /// <returns></returns>
    public bool Adjust(int value)
    {
        print("Adjust " + value);
        bool done = false;
        if (currentValue > 0)
        {
            done = true;
            if (value + currentValue > maxValue)
            {
                value = maxValue - currentValue;
            }
            else if (value + currentValue < 0)
            {
                value = -currentValue;
            }
            currentValue += value;
        }
        SetBar(value);
        return done;
    }
    // Use this for initialization
    void Start()
    {
        image = this.GetComponent<Image>();
        imgWidth = image.rectTransform.rect.width;
        currentValue = maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void SetBar(float val)
    {
        print("SetBar " + val);
        print("% " + (val / (float)maxValue));
        float len = imgWidth * (val / (float)maxValue);
        print("len " + len);
        image.rectTransform.offsetMax = new Vector2(image.rectTransform.offsetMax.x + len, image.rectTransform.offsetMax.y);
        print(image.rectTransform.rect);
    }
}
