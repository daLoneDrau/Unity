using Assets.Scripts.Crypts.Constants;
using RPGBase.Flyweights;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Crypts.Flyweights
{
    public class CharacterOrigin
    {
        private static Dictionary<int, CharacterOrigin> values;
        public static CharacterOrigin[] Values()
        {
            return values.Values.ToArray<CharacterOrigin>();
        }
        public int Homeland { get; private set; }
        public int Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int[] Modifier { get; private set; }
        public CharacterOrigin(int code, int homelandCode, string name, string description, int[] modifier)
        {
            if (values == null)
            {
                values = new Dictionary<int, CharacterOrigin>();
            }
            if (values.ContainsKey(code))
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Homeland code already exists!");
            }
            if (modifier == null
                || modifier.Length != 2)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Must have a modifier!");
            }
            if (modifier[0] < 0
                || modifier[0] >= CryptEquipGlobals.NUM_EQUIP_ELEMENTS)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Must modify a valid element!");
            }
            Code = code;
            Homeland = homelandCode;
            Name = name;
            Description = description;
            Modifier = modifier;
            values.Add(code, this);
        }
    }
}