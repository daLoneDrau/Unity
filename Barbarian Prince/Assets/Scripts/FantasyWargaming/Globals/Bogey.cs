using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.FantasyWargaming.Flyweights;

namespace Assets.Scripts.FantasyWargaming.Globals
{
    public sealed class Bogey
    {
        public string Title { get; private set; }
        public int Val { get; private set; }
        public int Modifier { get; private set; }
        public string Attribute { get; private set; }
        private Bogey(string t, int v, string a, int m = 0)
        {
            Title = t;
            Val = v;
            Attribute = a;
            Modifier = m;
        }
        public static Bogey Homosexual = new Bogey("Homosexual", 1, null);
        public static Bogey Ugly = new Bogey("Ugly", 2, "CHA", -1);
        public static Bogey Nearsighted = new Bogey("Nearsighted", 4, "AGI", -1);
        public static Bogey Stammer = new Bogey("Stammer", 8, "CHA", -1);
        public static Bogey Limp = new Bogey("Limp", 16, "AGI", -1);
        public static Bogey Weak = new Bogey("Weak Constitution", 32, "END", -1);
        public static Bogey Shy = new Bogey("Shy", 64, "CHA", -1);
        public static Bogey Smelly = new Bogey("Body Odor", 128, "CHA", -1);
        public static Bogey Insomniac = new Bogey("Insomniac", 256, "END", -1);
        public static Bogey Alcoholic = new Bogey("Alcoholic", 512, "LUS", 1);
        public static Bogey Beauty = new Bogey("Beautiful", 1024, "CHA", 1);
        public static Bogey Amiable = new Bogey("Likable", 2048, "CHA", 1);
        public static Bogey Extrovert = new Bogey("Extrovert", 4096, "CHA", 1);
        public static Bogey LevelHeaded = new Bogey("Born Leader", 8192, "LEA", 1);
        public static Bogey Orator = new Bogey("Orator", 16384, "LEA", 1);
        public static Bogey Charismatic = new Bogey("Charismatic", 32768, "CHA", 1);
        public static Bogey Robust = new Bogey("Robust", 65536, "END", 1);
        public static Bogey Dolorous = new Bogey("Dolorous", 131072, "CHA", -1);
        public static Bogey Nimble = new Bogey("Nimble", 262144, "AGI", 1);
        public static Bogey Lucky = new Bogey("Lucky", 524288, null);

        public void Apply(FWCharacter character)
        {
            if (Attribute != null)
            {
                character.AdjustAttributeModifier(Attribute, Modifier);
            }
        }
    }
}
