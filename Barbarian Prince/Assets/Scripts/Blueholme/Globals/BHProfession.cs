using RPGBase.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blueholme.Globals
{
    public sealed class BHProfession
    {
        public Dice HitDice { get; private set; }
        public string Title { get; private set; }
        public int Val { get; private set; }
        public int AllowedRaces { get; private set; }
        private BHProfession(string t, int v, Dice d, int a = 1)
        {
            Title = t;
            Val = v;
            HitDice = d;
            AllowedRaces = a;
        }
        /// <summary>
        /// Determines if a specific race is allowed in the profession.
        /// </summary>
        /// <param name="race">the <see cref="BHRace"/></param>
        /// <returns></returns>
        public bool IsRaceAllowed(BHRace race)
        {
            return (AllowedRaces & race.Val) == race.Val;
        }
        /// <summary>
        /// Cleric
        /// </summary>
        public static BHProfession Cleric = new BHProfession("Cleric", 0, Dice.ONE_D6);
        /// <summary>
        /// Cleric
        /// </summary>
        public static BHProfession Fighter = new BHProfession("Fighter-user", 1, Dice.ONE_D6, 15);
        /// <summary>
        /// Cleric
        /// </summary>
        public static BHProfession MagicUser = new BHProfession("Magic-user", 2, Dice.ONE_D4, 5);
        /// <summary>
        /// Cleric
        /// </summary>
        public static BHProfession Thief = new BHProfession("Thief", 3, Dice.ONE_D4);
    }
}
