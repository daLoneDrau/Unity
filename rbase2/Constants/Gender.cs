using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGBase.Constants
{
    public sealed class Gender
    {
        /// <summary>
        /// child relationships by gender.
        /// </summary>
        public static string[] GENDER_CHILD_RELATION = { "son", "daughter" };
        /// <summary>
        /// the female gender.
        /// </summary>
        public static int GENDER_FEMALE = 1;
        /// <summary>
        /// the male gender.
        /// </summary>
        public static int GENDER_MALE = 0;
        /// <summary>
        /// objective nouns by gender.
        /// </summary>
        public static string[] GENDER_OBJECTIVE = { "him", "her" };
        /// <summary>
        /// possessive adjectives by gender.
        /// </summary>
        public static string[] GENDER_POSSESSIVE = { "his", "her" };
        /// <summary>
        /// possessive objective nouns by gender.
        /// </summary>
        public static string[] GENDER_POSSESSIVE_OBJECTIVE = { "his", "hers" };
        /// <summary>
        /// pronouns by gender.
        /// </summary>
        public static string[] GENDER_PRONOUN = { "he", "she" };
        /// <summary>
        /// a list of player gender names.
        /// </summary>
        public static string[] GENDERS = { "Male", "Female" };
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private Gender()
        {
        }
    }
}
