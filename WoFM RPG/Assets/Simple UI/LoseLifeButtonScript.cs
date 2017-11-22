using UnityEngine;
using RPGBaseCS.Constants;

public class LoseLifeButtonScript : MonoBehaviour {

    [SerializeField]
    private StatusBarScript otherScript;
    void Start()
    {
    }

    public void TaskOnClick()
    {
        print("You have clicked the button!");
        print(Dice.ONE_D10.Roll());
        print(Dice.ONE_D10.Roll());
        print(Dice.ONE_D10.Roll());
        print(Dice.ONE_D10.Roll());
        print(Dice.ONE_D10.Roll());
        print(Dice.ONE_D10.Roll());
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
