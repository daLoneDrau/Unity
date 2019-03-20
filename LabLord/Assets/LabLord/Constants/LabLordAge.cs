using RPGBase.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Constants
{
    public class LabLordAge
    {
        private static int[][] DWARF = {
            new int[] {  75, 3, 6 },
            new int[] {   0, 0, 0 },
            new int[] {  40, 4, 6 },
            new int[] { 230, 3, 20 }
        };
        private static int[][] ELF = {
            new int[] { 100, 4, 8 },
            new int[] { 150, 4, 8 },
            new int[] { 125, 5, 8 },
            new int[] { 510, 8, 8 }
        };
        private static int[][] GNOME = {
            new int[] {  80, 3, 6 },
            new int[] {  60, 3, 6 },
            new int[] { 100, 2, 10 },
            new int[] { 300, 3, 10 }
        };
        private static int[][] HALFLING = {
            new int[] {  40, 1, 6 },
            new int[] {   0, 0, 0 },
            new int[] {  20, 2, 6 },
            new int[] {   0, 0, 0 }
        };
        private static int[][] HALF_ELF = {
            new int[] {  20, 5, 4 },
            new int[] {  35, 3, 4 },
            new int[] {  20, 4, 4 },
            new int[] {  30, 3, 4 }
        };
        private static int[][] HALF_ORC = {
            new int[] {  20, 1, 4 },
            new int[] {   0, 0, 0 },
            new int[] {  14, 1, 4 },
            new int[] {  20, 1, 4 }
        };
        private static int[][] HUMAN = {
            new int[] {  18, 1, 4 },
            new int[] {  27, 1, 8 },
            new int[] {  16, 1, 4 },
            new int[] {  18, 1, 6 }
        };
        private static int[][][] STARTING_AGES_BY_CLASS = {
            // ASSASSIN
            new int[][] {
                DWARF[0], ELF[0], GNOME[0], HALFLING[0],
                HALF_ELF[0], HALF_ORC[0], HUMAN[0] },
            // CLERIC
            new int[][] {
                DWARF[3], ELF[3], GNOME[3], HALFLING[3],
                HALF_ELF[3], HALF_ORC[3], HUMAN[3] },
            // DRUID
            new int[][] {
                DWARF[3], ELF[3], GNOME[3], HALFLING[3],
                HALF_ELF[3], HALF_ORC[3], HUMAN[3] },
            // FIGHTER
            new int[][] {
                DWARF[2], ELF[2], GNOME[2], HALFLING[2],
                HALF_ELF[2], HALF_ORC[2], HUMAN[2] },
            // ILLUSIONIST
            new int[][] {
                DWARF[1], ELF[1], GNOME[1], HALFLING[1],
                HALF_ELF[1], HALF_ORC[1], HUMAN[1] },
            // MAGE
            new int[][] {
                DWARF[1], ELF[1], GNOME[1], HALFLING[1],
                HALF_ELF[1], HALF_ORC[1], HUMAN[1] },
            // MONK
            new int[][] {
                DWARF[3], ELF[3], GNOME[3], HALFLING[3],
                HALF_ELF[3], HALF_ORC[3], HUMAN[3] },
            // PALADIN
            new int[][] {
                DWARF[2], ELF[2], GNOME[2], HALFLING[2],
                HALF_ELF[2], HALF_ORC[2], HUMAN[2] },
            // RANGER
            new int[][] {
                DWARF[2], ELF[2], GNOME[2], HALFLING[2],
                HALF_ELF[2], HALF_ORC[2], HUMAN[2] },
            // THIEF
            new int[][] {
                DWARF[0], ELF[0], GNOME[0], HALFLING[0],
                HALF_ELF[0], HALF_ORC[0], HUMAN[0] }
        };
        public const int ADOLESCENT = 0;
        public const int ADULT = 1;
        public const int MIDDLE_AGE = 2;
        public const int ELDERLY = 3;
        public const int VENERABLE = 4;
        public const int NUM_AGES = 5;
        public static string[] AGE_TITLE = { "Adolescent", "Adult", "Middle Age", "Elderly", "Venerable" };
        private static int[][][] AGES_MATRIX = {
            // DWARF
            new int[][]{
                new int[]{ 35,55},
                new int[]{ 56,149},
                new int[]{ 150,249},
                new int[]{ 250,349},
                new int[]{ 350,450}
            },
            // ELF
            new int[][]{
                new int[]{ 100,179},
                new int[]{ 180,574},
                new int[]{ 575,874},
                new int[]{ 875,1199},
                new int[]{ 1200,1700}
            },
            // GNOME
            new int[][]{
                new int[]{ 55,89},
                new int[]{ 90,299},
                new int[]{ 300,449},
                new int[]{ 450,599},
                new int[]{ 600,760}
            },
            // HALFLING
            new int[][]{
                new int[]{ 22,32},
                new int[]{ 33,69},
                new int[]{ 70,99},
                new int[]{ 100,149},
                new int[]{ 150,200}
            },
            // HALF-ELF
            new int[][]{
                new int[]{ 24,44},
                new int[]{ 45,99},
                new int[]{ 100,179},
                new int[]{ 180,249},
                new int[]{ 250,350}
            },
            // HALF-ORC
            new int[][]{
                new int[]{ 12,16},
                new int[]{ 17,31},
                new int[]{ 32,46},
                new int[]{ 47,80},
                new int[]{ 62,200}
            },
            // HUMAN
            new int[][]{
                new int[]{ 14,19},
                new int[]{ 20,40},
                new int[]{ 41,60},
                new int[]{ 61,85},
                new int[]{ 86,100}
            }
        };
        public static int[][] ABILITY_MODIFIERS = {
            // ADOLESCENT CON +1 WIS -1
            new int[]{ 0, 0, 1, 0, -1, 0 },
            // ADULT STR +1 CON +1
            new int[]{ 1, 0, 1, 0, 0, 0 },
            // MIDLE AGE INT +1 WIS +1
            new int[]{ 0, 0, 0, 1, 1, 0 },
            // ELDERLY STR -2 DEX -1 CON -1  INT +1 WIS -1
            new int[]{ -2, -1, -1, 1, 2, 0 },
            // VENERABLE STR -3 DEX -2 CON -2 INT +2 WIS +3
            new int[]{ -3, -2, -2, 2, 3, 0 }
        };
        public static int GetRandomStartingAge(int race, int clazz)
        {
            return STARTING_AGES_BY_CLASS[clazz][race][0] + Diceroller.Instance.RollXdY(STARTING_AGES_BY_CLASS[clazz][race][1], STARTING_AGES_BY_CLASS[clazz][race][2]); ;
        }
        public static int GetAgeRank(int race, int age)
        {
            int rank = ADOLESCENT;
            int[][] ranges = AGES_MATRIX[race];
            for (int i = ranges.Length - 1; i >= 0; i--)
            {
                if (age >= ranges[i][0]
                    && age <= ranges[i][1])
                {
                    rank = i;
                    break;
                }
            }
            return rank;
        }
    }
}
