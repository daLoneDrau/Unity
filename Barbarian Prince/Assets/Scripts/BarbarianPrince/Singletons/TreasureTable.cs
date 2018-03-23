using Assets.Scripts.BarbarianPrince.Constants;
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
        public void AwardTreasure(WealthCode wc)
        {
            switch (wc)
            {
                case WealthCode.WC0:
                    break;
            }
        }
    }
}
