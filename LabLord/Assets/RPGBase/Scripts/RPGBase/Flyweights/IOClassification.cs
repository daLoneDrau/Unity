using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Flyweights
{
    public class IOClassification
    {
        /// <summary>
        /// the <see cref="IOClassification"/>'s code field.
        /// </summary>
        private int code;
        /// <summary>
        /// the <see cref="IOClassification"/>'s code property.
        /// </summary>
        public int Code { get { return code; } set { code = value; } }
        /// <summary>
        /// the <see cref="IOClassification"/>'s description field.
        /// </summary>
        private string description;
        /// <summary>
        /// the <see cref="IOClassification"/>'s description property.
        /// </summary>
        public string Description { get { return description; } set { description = value; } }
        /// <summary>
        /// the <see cref="IOClassification"/>'s title field.
        /// </summary>
        private string title;
        /// <summary>
        /// the <see cref="IOClassification"/>'s title property.
        /// </summary>
        public string Title { get { return title; } set { title = value; } }
        /// <summary>
        /// Creates a new instance of <see cref="IOClassification"/>.
        /// </summary>
        /// <param name="c">the code or id</param>
        /// <param name="t">the title</param>
        /// <param name="d">the description</param>
        public IOClassification(int c, string t, string d)
        {
            code = c;
            title = t;
            description = d;
        }
    }
}
