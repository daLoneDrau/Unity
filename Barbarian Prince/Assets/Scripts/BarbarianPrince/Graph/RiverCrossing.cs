using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.BarbarianPrince.Graph
{
    public class RiverCrossing
    {
        public int From { get; set; }
        public int To { get; set; }
        public string RiverName { get; set; }
        public override string ToString()
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("[from = ");
            sb.Append(From);
            sb.Append(", to = ");
            sb.Append(To);
            sb.Append(", name = \"");
            sb.Append(RiverName);
            sb.Append("\"]");
            string s = sb.ToString();
            sb.ReturnToPool();
            sb = null;
            return s;
        }
    }
}
