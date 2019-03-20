using LabLord.Flyweights;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Constants
{
    public class LabLordCurrency : IOClassification
    {
        /// <summary>
        /// Gets 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToString(int val)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            bool needsSpace = false;
            int remainder = 0;
            if (val / 100 > 0)
            {
                sb.Append(val / 100);
                sb.Append("gp");
                needsSpace = true;
            }
            if (val % 100 > 0)
            {
                val %= 100;
                // EP
                if (val / 50 > 0)
                {
                    if (needsSpace)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(val / 50);
                    sb.Append("ep");
                    needsSpace = true;
                }
                if (val % 50 > 0)
                {
                    val %= 50;
                    // SP
                    if (val / 10 > 0)
                    {
                        if (needsSpace)
                        {
                            sb.Append(" ");
                        }
                        sb.Append(val / 10);
                        sb.Append("sp");
                        needsSpace = true;
                    }
                    if (val % 10 > 0)
                    {
                        val %= 10;
                        // CP
                        if (needsSpace)
                        {
                            sb.Append(" ");
                        }
                        sb.Append(val % 10);
                        sb.Append("cp");
                    }
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private LabLordCurrency(int c, string t, string d) : base(c, t, d)
        {
        }
    }
}
