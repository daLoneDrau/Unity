using LabLord.Constants;
using RPGBase.Constants;
using RPGBase.Flyweights;
using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.UI.SceneControllers.LabLord
{
    public class CharacterValidator
    {
        /// <summary>
        /// flag indicating debugging is on.
        /// </summary>
        public bool debug = false;
        /// <summary>
        /// the matrix storing possible race/class values for the newly-rolled character.
        /// </summary>
        private bool[][] matrix = new bool[LabLordRace.NUM_RACES][];
        /// <summary>
        /// Checks whether a specific race is valid for the character.
        /// </summary>
        /// <param name="race">the race</param>
        /// <returns></returns>
        public bool CheckRace(int race)
        {
            bool atLeastOne = false;
            for (int clazz = matrix[race].Length - 1; clazz >= 0; clazz--)
            {
                if (matrix[race][clazz])
                {
                    atLeastOne = true;
                    break;
                }
            }
            return atLeastOne;
        }
        public bool CheckClass(int race, int clazz)
        {
            return matrix[race][clazz];
        }
        /// <summary>
        /// Initializes the validation matrix.
        /// </summary>
        public void InitMatrix()
        {
            for (int i = matrix.Length - 1; i >= 0; i--)
            {
                matrix[i] = new bool[LabLordClass.NUM_CLASSES];
                for (int j = matrix[i].Length - 1; j >= 0; j--)
                {
                    matrix[i][j] = true;
                }
            }
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_ASSASSIN] = true;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_CLERIC] = true;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_DRUID] = false;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_FIGHTER] = true;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_ILLUSIONIST] = false;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_MAGIC_USER] = false;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_MONK] = false;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_PALADIN] = false;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_RANGER] = false;
            matrix[LabLordRace.RACE_DWARF][LabLordClass.CLASS_THIEF] = true;

            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_ASSASSIN] = true;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_CLERIC] = true;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_DRUID] = false;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_FIGHTER] = true;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_ILLUSIONIST] = false;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_MAGIC_USER] = true;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_MONK] = false;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_PALADIN] = false;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_RANGER] = false;
            matrix[LabLordRace.RACE_ELF][LabLordClass.CLASS_THIEF] = true;

            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_ASSASSIN] = true;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_CLERIC] = true;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_DRUID] = false;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_FIGHTER] = true;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_ILLUSIONIST] = true;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_MAGIC_USER] = false;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_MONK] = false;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_PALADIN] = false;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_RANGER] = false;
            matrix[LabLordRace.RACE_GNOME][LabLordClass.CLASS_THIEF] = true;

            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_ASSASSIN] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_CLERIC] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_DRUID] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_FIGHTER] = true;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_ILLUSIONIST] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_MAGIC_USER] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_MONK] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_PALADIN] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_RANGER] = false;
            matrix[LabLordRace.RACE_HALFLING][LabLordClass.CLASS_THIEF] = true;

            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_ASSASSIN] = true;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_CLERIC] = true;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_DRUID] = false;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_FIGHTER] = true;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_ILLUSIONIST] = false;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_MAGIC_USER] = true;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_MONK] = false;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_PALADIN] = false;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_RANGER] = false;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_RANGER] = true;
            matrix[LabLordRace.RACE_HALF_ELF][LabLordClass.CLASS_THIEF] = true;

            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_ASSASSIN] = true;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_CLERIC] = true;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_DRUID] = false;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_FIGHTER] = true;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_ILLUSIONIST] = false;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_MAGIC_USER] = false;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_MONK] = false;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_PALADIN] = false;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_RANGER] = false;
            matrix[LabLordRace.RACE_HALF_ORC][LabLordClass.CLASS_THIEF] = true;
        }
        public void CompleteMatrix(int gender, int[] abilities)
        {
            for (int race = matrix.Length - 1; race >= 0; race--)
            {
                if (debug)
                {
                    UnityEngine.Debug.Log("******CHECK RACE " + LabLordRace.Races[race].Title);
                }
                // check abilities for each race
                if (!ValidateRace(race, gender, abilities))
                {
                    // race not valid.  invalidate all race/class combo in matrix
                    for (int clazz = matrix[race].Length - 1; clazz >= 0; clazz--)
                    {
                        matrix[race][clazz] = false;
                    }
                }
                else
                {
                    if (debug)
                    {
                        UnityEngine.Debug.Log("race " + LabLordRace.Races[race].Title + " is valid");
                    }
                    // abilities qualify for race.
                    for (int clazz = matrix[race].Length - 1; clazz >= 0; clazz--)
                    {
                        // test to see if class is possible
                        if (matrix[race][clazz])
                        {
                            if (debug)
                            {
                                UnityEngine.Debug.Log("+++CHECK CLASS " + LabLordClass.Classes[clazz].Title);
                            }
                            matrix[race][clazz] = ValidateClass(clazz, race, abilities);
                        }
                    }
                }
                if (debug)
                {
                    UnityEngine.Debug.Log("******FINISHED RACE " + LabLordRace.Races[race].Title);
                }
            }
        }
        public int[] NewAbilities(int gender)
        {
            int[] abilities = new int[6];
            bool valid = false;
            do
            {
                // initialize validation matrix
                InitMatrix();
                // roll new abilities
                for (int i = abilities.Length - 1; i >= 0; i--)
                {
                    abilities[i] = Diceroller.Instance.RollXdY(3, 6);
                }
                CompleteMatrix(gender, abilities);
                // after completing the matrix, check for
                // at least one valid race/class combination
                for (int race = matrix.Length - 1; race >= 0; race--)
                {
                    valid = CheckRace(race);
                    if (valid)
                    {
                        break;
                    }
                }
            } while (!valid);
            return abilities;
        }
        private bool ValidateClass(int clazz, int race, int[] abilities)
        {
            bool valid = true;
            for (int ability = abilities.Length - 1; ability >= 0; ability--)
            {
                // check to see if Class has Min Requirement for Ability
                if (!LabLordClass.Classes[clazz].Requirements.ContainsKey(ability))
                {
                    continue;
                }
                int minScore = LabLordClass.Classes[clazz].Requirements[ability];
                int abilityScore = abilities[ability];
                EquipmentItemModifier mod = LabLordRace.Races[race].Modifiers[ability];
                if (mod != null
                    && mod.Value != 0f)
                {
                    abilityScore += (int)mod.Value;
                }
                if (abilityScore < minScore)
                {
                    if (debug)
                    {
                        UnityEngine.Debug.Log("invalid " + LabLordAbility.Abilities[ability].Abbr + " score");
                        UnityEngine.Debug.Log(abilityScore + " < " + minScore);
                    }
                    valid = false;
                    break;
                }
            }
            return valid;
        }
        /// <summary>
        /// Validates whether a character's abilities meet a race's required Min/Max ability scores.
        /// </summary>
        /// <param name="race">the race</param>
        /// <param name="gender">the character's gender</param>
        /// <param name="abilities">the character's ability scores</param>
        /// <returns></returns>
        private bool ValidateRace(int race, int gender, int[] abilities)
        {
            bool valid = true;
            // get min/max ability scores for race
            LabLordRace raceObj = LabLordRace.Races[race];
            // loop through abilities to verify all within range
            for (int ability = abilities.Length - 1; ability >= 0; ability--)
            {
                int abilityScore = abilities[ability];
                int min = raceObj.Requirements[ability][gender, 0];
                int max = raceObj.Requirements[ability][gender, 1];
                if (abilityScore < min
                    || abilityScore > max)
                {
                    if (debug)
                    {
                        UnityEngine.Debug.Log("invalid " + LabLordAbility.Abilities[ability].Abbr + " score");
                        UnityEngine.Debug.Log(abilityScore + " outside range " + min + " - " + max);
                    }
                    // found an ability outside of range. stop
                    valid = false;
                    break;
                }
            }
            return valid;
        }
        /// <summary>
        /// Rolls a new age for the character.
        /// </summary>
        /// <param name="race">the character's race</param>
        /// <param name="clazz">the character's class</param>
        /// <returns></returns>
        public int NewAge(int race, int clazz)
        {
            return LabLordAge.GetRandomStartingAge(race, clazz);
        }
    }
}
