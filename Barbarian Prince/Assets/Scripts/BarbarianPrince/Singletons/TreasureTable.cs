using Assets.Scripts.BarbarianPrince.Constants;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Singletons
{
    public class TreasureTable
    {
        /// <summary>
        /// the singleton instance.
        /// </summary>
        private static TreasureTable instance;
        /// <summary>
        /// the instance property
        /// </summary>
        public static TreasureTable Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TreasureTable();
                }
                return instance;
            }
        }
        private TreasureTable() { }
        public int AwardTreasure(WealthCode wc)
        {
            int val = 0;
            switch (wc)
            {
                case WealthCode.WC0:
                    break;
                case WealthCode.WC1:
                    switch (Diceroller.Instance.RolldX(6))
                    {
                        case 1:
                        case 2:
                            break;
                        case 3:
                        case 4:
                            val = 1;
                            break;
                        case 5:
                        case 6:
                            val = 2;
                            break;
                    }
                    break;
                case WealthCode.WC2:
                    switch (Diceroller.Instance.RolldX(6))
                    {
                        case 1:
                            break;
                        case 2:
                            val = 1;
                            break;
                        case 3:
                        case 4:
                            val = 2;
                            break;
                        case 5:
                            val = 3;
                            break;
                        case 6:
                            val = 4;
                            break;
                    }
                    break;
            }
            return val;
        }
    }
}
