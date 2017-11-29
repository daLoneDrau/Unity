using System;
using RPGBase.Singletons;

namespace RPGBase.Constants
{
    public static class DiceExtensions
    {
        public static int Roll(this Dice dice)
        {
            int sixteen = 16, shift = 0xffff;
            int num = (int)dice >> sixteen, faces = (int)dice & shift;
            return Diceroller.GetInstance().RollXdY(num, faces);
        }
    }
    public enum Dice
    {
        ONE_D10 = 65546,
        ONE_D2 = 65538,
        ONE_D3 = 65539,
        ONE_D4 = 65540,
        ONE_D6 = 65542,
        ONE_D8 = 65544,
        ONE_D12 = 65548,
        ONE_D20 = 65556
    }
}