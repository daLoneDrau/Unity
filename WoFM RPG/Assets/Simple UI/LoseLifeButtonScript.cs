using UnityEngine;

public class LoseLifeButtonScript : MonoBehaviour {

    [SerializeField]
    private StatusBarScript otherScript;
    void Start()
    {
    }

    public void TaskOnClick()
    {
        print("You have clicked the button!");
        otherScript.Adjust(-33);
    }
    public int[] ConvertIntToPoint(int val)
    {
        int sixteen = 16, shift = 0xffff;
        return new int[] { val >> sixteen, val & shift };
    }
    private int ConvertValuesToInt(int x, int y)
    {
        int sixteen = 16;
        int val = (int)x << sixteen;
        val += (int)y;
        return val;
    }
}
