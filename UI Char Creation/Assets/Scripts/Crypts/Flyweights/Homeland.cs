using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Crypts.Flyweights
{
    public class Homeland
    {
        public int Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Homeland(int code, string name, string description)
        {
            Code = code;
            Name = name;
            Description = description;
        }
    }
}
