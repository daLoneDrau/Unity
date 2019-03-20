using LabLord.Flyweights;
using RPGBase.Flyweights;
using RPGBase.Pooled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabLord.Constants
{
    public class LabLordAbility : IOClassification
    {
        /// <summary>
        /// the list of in-game abilities.
        /// </summary>
        public static List<LabLordAbility> Abilities;
        #region ABILITY MODIFIERS
        private static Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> STR_MODS = new Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>>
        {
            {
                new List<int> { 3 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THM,
                        new EquipmentItemModifier(-3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DMG,
                        new EquipmentItemModifier(-3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS,
                        new EquipmentItemModifier(-3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    }
                }
            },
            {
                new List<int> { 4, 5 },
                new Dictionary<int, EquipmentItemModifier> {
                {
                        LabLordGlobals.EQUIP_ELEMENT_THM,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DMG,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    }
                }
            },
            {
                new List<int> { 6, 7, 8 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THM,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DMG,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    }
                }
            },
            {
                new List<int> { 13, 14, 15 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THM,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DMG,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    }
                }
            },
            {
                new List<int> { 16, 17 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THM,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DMG,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    }
                }
            },
            {
                new List<int> { 18 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THM,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DMG,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    }
                }
            },
            {
                new List<int> { 19 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THM,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DMG,
                        new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_FORCE_DOORS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Strength")
                    }
                }
            }
        };
        private static Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> DEX_MODS = new Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>>
        {
            {
                new List<int> { 3 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_AC,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM,
                        new EquipmentItemModifier(-3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_INITIATIVE,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS,
                        new EquipmentItemModifier(-60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS,
                        new EquipmentItemModifier(-60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS,
                        new EquipmentItemModifier(-60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY,
                        new EquipmentItemModifier(-60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS,
                        new EquipmentItemModifier(-60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS,
                        new EquipmentItemModifier(-60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    }
                }
            },
            {
                new List<int> { 4, 5 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_AC,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_INITIATIVE,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS,
                        new EquipmentItemModifier(-30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS,
                        new EquipmentItemModifier(-30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS,
                        new EquipmentItemModifier(-30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY,
                        new EquipmentItemModifier(-30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS,
                        new EquipmentItemModifier(-30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS,
                        new EquipmentItemModifier(-30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    }
                }
            },
            {
                new List<int> { 6, 7, 8 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_AC,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_INITIATIVE,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS,
                        new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS,
                        new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS,
                        new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY,
                        new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS,
                        new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS,
                        new EquipmentItemModifier(-15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    }
                }
            },
            {
                new List<int> { 13, 14, 15 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_AC,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_INITIATIVE,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    }
                }
            },
            {
                new List<int> { 16, 17 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_AC,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_INITIATIVE,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    }
                }
            },
            {
                new List<int> { 18 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_AC,
                        new EquipmentItemModifier(-3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_INITIATIVE,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    }
                }
            },
            {
                new List<int> { 19 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_AC,
                        new EquipmentItemModifier(-4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MISSILE_THM,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_INITIATIVE,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS,
                        new EquipmentItemModifier(15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_FIND_REMOVE_TRAPS,
                        new EquipmentItemModifier(15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_POCKETS,
                        new EquipmentItemModifier(15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_MOVE_SILENTLY,
                        new EquipmentItemModifier(15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_CLIMB_WALLS,
                        new EquipmentItemModifier(15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_THIEF_HIDE_SHADOWS,
                        new EquipmentItemModifier(15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Dexterity")
                    }
                }
            }
        };
        private static Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> CON_MODS = new Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>>
        {
            {
                new List<int> { 3 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(-3, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(40, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(35, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 4 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(-2, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(45, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(40, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 5 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(-2, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(50, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(45, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 6 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(-1, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(55, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(50, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 7 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(-1, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(55, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 8 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(-1, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(65, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(60, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 9 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(70, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(65, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 10 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(75, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(70, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 11 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(80, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(75, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 12 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(85, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(80, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 13 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(1, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(90, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(85, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 14 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(1, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(92, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(90, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 15 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(1, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(94, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(93, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 16 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(2, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(96, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(95, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 17 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(2, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(98, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(97, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 18 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(3, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(100, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(99, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }, {
                new List<int> { 19 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MHP,
                        new EquipmentItemModifier(3, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON,
                        new EquipmentItemModifier(1, false, true, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_RESURRECTION,
                        new EquipmentItemModifier(100, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    },
                    {
                        LabLordGlobals.EQUIP_SURVIVE_POLYMORPH,
                        new EquipmentItemModifier(99, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Constitution")
                    }
                }
            }
        };
        private static Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> INT_MODS = new Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>>
        {
            {
                new List<int> { 3 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(20, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 4, 5 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 6, 7 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(35, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 8 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(40, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(6, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 9 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(40, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(6, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 10, 11, 12 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(50, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(7, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 13, 14 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_ADDL_LANGUAGES,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(70, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(9, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 15 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_ADDL_LANGUAGES,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(75, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(6, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(11, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 16 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_ADDL_LANGUAGES,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(75, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(6, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(11, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 17 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_ADDL_LANGUAGES,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(85, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(7, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(99, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            },
            {
                new List<int> { 18, 19 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_ADDL_LANGUAGES,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL,
                        new EquipmentItemModifier(90, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MIN_SPELLS,
                        new EquipmentItemModifier(8, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS,
                        new EquipmentItemModifier(99, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Intelligence")
                    }
                }
            }
        };
        private static Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> WIS_MODS = new Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>>
        {
            {
                new List<int> { 3 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(-3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(-3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(50, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 4 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(45, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 5 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(40, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 6 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(35, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 7 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(30, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 8 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(25, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 9 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(20, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 10 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(15, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 11 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 12 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 13 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 14 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 15 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_2_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 16 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_2_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 17 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_2_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_3_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 18 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_2_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_3_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_4_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            },
            {
                new List<int> { 19 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_WANDS,
                        new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS,
                        new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_2_SPELLS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_3_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_4_SPELLS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Wisdom")
                    }
                }
            }
        };
        private static Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> CHA_MODS = new Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>>
        {
            {
                new List<int> { 3 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE,
                        new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    }
                }
            },
            {
                new List<int> { 4, 5 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS,
                        new EquipmentItemModifier(2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    }
                }
            },
            {
                new List<int> { 6, 7, 8 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT,
                        new EquipmentItemModifier(1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS,
                        new EquipmentItemModifier(3, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE,
                        new EquipmentItemModifier(6, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    }
                }
            },
            {
                new List<int> { 9, 10, 11, 12 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS,
                        new EquipmentItemModifier(4, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE,
                        new EquipmentItemModifier(7, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    }
                }
            },
            {
                new List<int> { 13, 14, 15 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS,
                        new EquipmentItemModifier(5, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE,
                        new EquipmentItemModifier(8, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    }
                }
            },
            {
                new List<int> { 16, 17 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT,
                        new EquipmentItemModifier(-1, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS,
                        new EquipmentItemModifier(6, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE,
                        new EquipmentItemModifier(9, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    }
                }
            },
            {
                new List<int> { 18, 19 },
                new Dictionary<int, EquipmentItemModifier> {
                    {
                        LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT,
                        new EquipmentItemModifier(-2, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_NUM_HIRELIGNS,
                        new EquipmentItemModifier(7, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    },
                    {
                        LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE,
                        new EquipmentItemModifier(10, false, false, 0, LabLordGlobals.MODIFIER_SRC_ABILITY, "Charisma")
                    }
                }
            }
        };
        #endregion
        /// <summary>
        /// Static initializer.
        /// </summary>
        static LabLordAbility()
        {
            Abilities = new List<LabLordAbility>
            {
                new LabLordAbility(ABILITY_STR, "Strength", "", STR_MODS),
                new LabLordAbility(ABILITY_DEX, "Dexterity", "", DEX_MODS),
                new LabLordAbility(ABILITY_CON, "Constitution", "", CON_MODS),
                new LabLordAbility(ABILITY_INT, "Intelligence", "", INT_MODS),
                new LabLordAbility(ABILITY_WIS, "Wisdom", "", WIS_MODS),
                new LabLordAbility(ABILITY_CHA, "Charisma", "", CHA_MODS)
            };
            Abilities[ABILITY_STR].Abbr = "STR";
            Abilities[ABILITY_DEX].Abbr = "DEX";
            Abilities[ABILITY_CON].Abbr = "CON";
            Abilities[ABILITY_INT].Abbr = "INT";
            Abilities[ABILITY_WIS].Abbr = "WIS";
            Abilities[ABILITY_CHA].Abbr = "CHA";
        }
        public string Abbr { get; set; }
        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private LabLordAbility(int c, string t, string d, Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> dict) : base(c, t, d)
        {
            modifiers = dict;
        }
        /// <summary>
        /// The field defining the list of ability modifiers.  First lookup is the value range, then the list of modifiers.
        /// </summary>
        private Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> modifiers;
        /// <summary>
        /// The property defining the list of ability modifiers.
        /// </summary>
        public Dictionary<List<int>, Dictionary<int, EquipmentItemModifier>> Modifiers { get { return modifiers; } }
        /// <summary>
        /// Gets the map of modifiers that should be applied for an ability's score.
        /// </summary>
        /// <param name="score">the ability score</param>
        /// <returns></returns>
        public Dictionary<int, EquipmentItemModifier> GetModifiersForScore(int score)
        {
            Dictionary<int, EquipmentItemModifier> mods = null;
            foreach (KeyValuePair<List<int>, Dictionary<int, EquipmentItemModifier>> entry in modifiers)
            {
                // do something with entry.Value or entry.Key
                if (entry.Key.Contains(score))
                {
                    mods = entry.Value;
                    break;
                }
            }
            return mods;
        }
        public const int ABILITY_STR = 0;
        public const int ABILITY_DEX = 1;
        public const int ABILITY_CON = 2;
        public const int ABILITY_INT = 3;
        public const int ABILITY_WIS = 4;
        public const int ABILITY_CHA = 5;
        private string GetStrengthModifierText(LabLordCharacter pc, int score, Dictionary<int, EquipmentItemModifier> dict)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            foreach (KeyValuePair<int, EquipmentItemModifier> entry in dict)
            {
                if (entry.Value != null
                    && entry.Value.Value != 0)
                {
                    if (score < 9)
                    {
                        sb.Append("<color=red>");
                    }
                    if (entry.Value.Value > 0)
                    {
                        sb.Append("+");
                    }
                    sb.Append((int)entry.Value.Value);
                    if (score < 9)
                    {
                        sb.Append("</color>");
                    }
                    sb.Append(" <color=#008ECC>");
                    sb.Append(pc.GetAttributeName(entry.Key));
                    sb.Append("</color>");
                    if (!dict[entry.Key].Equals(dict.Last().Value))
                    {
                        sb.Append(", ");
                    }
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        private string GetDexterityModifierText(LabLordCharacter pc, int score, Dictionary<int, EquipmentItemModifier> dict)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            bool needscomma = false;
            for (int i = LabLordGlobals.EQUIP_ELEMENT_AC, li = LabLordGlobals.EQUIP_ELEMENT_INITIATIVE; i <= li; i++)
            {
                if (dict.ContainsKey(i)
                    && dict[i].Value != 0f)
                {
                    if (needscomma)
                    {
                        sb.Append(", ");
                    }
                    if (score < 9)
                    {
                        sb.Append("<color=red>");
                    }
                    if (dict[i].Value > 0)
                    {
                        sb.Append("+");
                    }
                    sb.Append((int)dict[i].Value);
                    if (score < 9)
                    {
                        sb.Append("</color>");
                    }
                    sb.Append(" <color=#008ECC>");
                    sb.Append(pc.GetAttributeName(i));
                    sb.Append("</color>");
                    needscomma = true;
                }
            }
            if (pc.Clazz == LabLordClass.CLASS_THIEF)
            {
                if (dict.ContainsKey(LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS)
                    && dict[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS].Value != 0f)
                {
                    if (needscomma)
                    {
                        sb.Append(", ");
                    }
                    if (score < 9)
                    {
                        sb.Append("<color=red>");
                    }
                    if (dict[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS].Value > 0)
                    {
                        sb.Append("+");
                    }
                    sb.Append((int)dict[LabLordGlobals.EQUIP_ELEMENT_THIEF_PICK_LOCKS].Value);
                    sb.Append("%");
                    if (score < 9)
                    {
                        sb.Append("</color>");
                    }
                    sb.Append(" <color=#008ECC>Thief Skills</color>");
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        private string GetConstitutionModifierText(LabLordCharacter pc, int score, Dictionary<int, EquipmentItemModifier> dict)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            foreach (KeyValuePair<int, EquipmentItemModifier> entry in dict)
            {
                if (entry.Value != null
                    && entry.Value.Value != 0)
                {
                    string name = pc.GetAttributeName(entry.Key);
                    if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_MHP
                        || entry.Key == LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON)
                    {
                        if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_MHP)
                        {
                            name = "HP/Lvl";
                        }
                        else
                        {
                            name = "vs Poison";
                        }
                        if (score < 9)
                        {
                            sb.Append("<color=red>");
                        }
                    }
                    if (entry.Value.Value > 0
                        && entry.Key == LabLordGlobals.EQUIP_ELEMENT_MHP)
                    {
                        sb.Append("+");
                    }
                    sb.Append((int)entry.Value.Value);
                    if ((entry.Key == LabLordGlobals.EQUIP_ELEMENT_MHP
                        || entry.Key == LabLordGlobals.EQUIP_ELEMENT_SAVE_V_POISON)
                            && score < 9)
                    {
                        sb.Append("</color>");
                    }
                    if (entry.Key == LabLordGlobals.EQUIP_SURVIVE_RESURRECTION
                        || entry.Key == LabLordGlobals.EQUIP_SURVIVE_POLYMORPH)
                    {
                        sb.Append("%");
                    }
                    sb.Append(" <color=#008ECC>");
                    sb.Append(name);
                    sb.Append("</color>");
                    if (!dict[entry.Key].Equals(dict.Last().Value))
                    {
                        sb.Append(", ");
                    }
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        private string GetIntelligenceModifierText(LabLordCharacter pc, int score, Dictionary<int, EquipmentItemModifier> dict)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            if (pc.Clazz == LabLordClass.CLASS_MAGIC_USER
                || pc.Clazz == LabLordClass.CLASS_ILLUSIONIST)
            {
                foreach (KeyValuePair<int, EquipmentItemModifier> entry in dict)
                {
                    if (entry.Value != null
                        && entry.Value.Value != 0)
                    {
                        if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_LANGUAGE_PROFICIENCY
                            || entry.Key == LabLordGlobals.EQUIP_ELEMENT_ADDL_LANGUAGES)
                        {
                            continue;
                        }
                        string name = pc.GetAttributeName(entry.Key);
                        if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_MAX_SPELLS
                            && entry.Value.Value == 99f)
                        {
                            sb.Append("Unlimited");
                            name = "Max Spells";
                        }
                        else
                        {
                            sb.Append((int)entry.Value.Value);
                        }
                        if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_LEARN_SPELL)
                        {
                            sb.Append("%");
                        }
                        sb.Append(" <color=#008ECC>");
                        sb.Append(name);
                        sb.Append("</color>");
                        if (!dict[entry.Key].Equals(dict.Last().Value))
                        {
                            sb.Append(", ");
                        }
                    }
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        private static string GetWisdomModifierText(LabLordCharacter pc, int score, Dictionary<int, EquipmentItemModifier> dict)
        {
            bool needscomma = false;
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            List<int> list = new List<int>();
            if (dict.ContainsKey(LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS)
                && dict[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS].Value != 0f)
            {
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (dict[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS].Value > 0)
                {
                    sb.Append("+");
                }
                sb.Append((int)dict[LabLordGlobals.EQUIP_ELEMENT_SAVE_V_SPELLS].Value);
                if (score < 9)
                {
                    sb.Append("</color>");
                }
                sb.Append(" <color=#008ECC>vs Spells</color>");
                needscomma = true;
            }
            if (pc.Clazz == LabLordClass.CLASS_CLERIC
                || pc.Clazz == LabLordClass.CLASS_DRUID)
            {
                if (dict.ContainsKey(LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE)
                    && dict[LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE].Value != 0f)
                {
                    if (needscomma)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("<color=red>");
                    sb.Append((int)dict[LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE].Value);
                    sb.Append("%</color> <color=#008ECC>");
                    sb.Append(pc.GetAttributeName(LabLordGlobals.EQUIP_ELEMENT_DIVINE_SPELL_FAILURE));
                    sb.Append("</color>");
                    needscomma = true;
                }
                for (int i = LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_1_SPELLS, li = LabLordGlobals.EQUIP_ELEMENT_BONUS_LVL_4_SPELLS; i <= li; i++)
                {
                    if (dict.ContainsKey(i)
                        && dict[i].Value != 0f)
                    {
                        list.Add((int)dict[i].Value);
                    }
                }
                if (list.Count > 0)
                {
                    if (needscomma)
                    {
                        sb.Append(", ");
                    }
                    sb.Append("<color=#008ECC>Bonus Spells/Lvl</color>: ");
                    for (int i = 0, li = list.Count; i < li; i++)
                    {
                        sb.Append(list[i]);
                        if (i + 1 < li)
                        {
                            sb.Append("/");
                        }
                    }
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        private static string GetCharismaModifierText(LabLordCharacter pc, int score, Dictionary<int, EquipmentItemModifier> dict)
        {
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            foreach (KeyValuePair<int, EquipmentItemModifier> entry in dict)
            {
                if (entry.Value != null
                    && entry.Value.Value != 0)
                {
                    string name = pc.GetAttributeName(entry.Key);
                    if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT
                        && score < 9)
                    {
                        sb.Append("<color=red>");
                    }
                    if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT
                        && entry.Value.Value > 0)
                    {
                        sb.Append("+");
                    }
                    if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_RETAINER_MORALE)
                    {
                        sb.Append("<color=#008ECC>");
                        sb.Append(name);
                        sb.Append("</color>");
                        sb.Append(": ");
                        sb.Append((int)entry.Value.Value);
                    }
                    else
                    {
                        sb.Append((int)entry.Value.Value);
                        if (entry.Key == LabLordGlobals.EQUIP_ELEMENT_REACTION_ADJUSTMENT
                        && score < 9)
                        {
                            sb.Append("</color>");
                        }
                        sb.Append(" <color=#008ECC>");
                        sb.Append(name);
                        sb.Append("</color>");
                    }
                    if (!dict[entry.Key].Equals(dict.Last().Value))
                    {
                        sb.Append(", ");
                    }
                }
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public static string GetStrengthDescription(int score)
        {
            int mod = 0;
            switch (score)
            {
                case 3:
                    mod = -3;
                    break;
                case 4:
                case 5:
                    mod = -2;
                    break;
                case 6:
                case 7:
                case 8:
                    mod = -1;
                    break;
                case 13:
                case 14:
                case 15:
                    mod = 1;
                    break;
                case 16:
                case 17:
                    mod = 2;
                    break;
                case 18:
                    mod = 3;
                    break;
                case 19:
                    mod = 3;
                    break;
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("<b>Strength</b> (STR) measures a character's muscle and physical power. This ability is especially important for fighters, dwarves, elves, and halflings because it helps them prevail in combat.");
            if (mod != 0)
            {
                sb.Append("\n\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mod > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mod);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(" Penalty");
                }
                else
                {
                    sb.Append(" Bonus");
                }
                sb.Append(" to hit, damage, and forcing doors</b>");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public static string GetDexterityDescription(int score)
        {
            int[] mods = new int[4];
            string[][] arr = new string[][] {
                new string[] { " Armor Class Penalty", " Armor Class Bonus" },
                new string[] { " Missile Attack Penalty", " Missile Attack Bonus" },
                new string[] { " Initiative Penalty", " Initiative Bonus" },
                new string[] { " Thief Skill Penalty", "% Thief Skill Bonus" } };
            switch (score)
            {
                case 3:
                    mods = new int[] { 3, -3, -2, -60 };
                    break;
                case 4:
                case 5:
                    mods = new int[] { 2, -2, -1, -30 };
                    break;
                case 6:
                case 7:
                case 8:
                    mods = new int[] { 1, -1, -1, -15 };
                    break;
                case 13:
                case 14:
                case 15:
                    mods = new int[] { -1, 1, 1, 0 };
                    break;
                case 16:
                case 17:
                    mods = new int[] { -2, 2, 1, 5 };
                    break;
                case 18:
                    mods = new int[] { -4, 3, 2, 15 };
                    break;
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("<b>Dexterity</b> (DEX) measures hand-eye coordination, agility, reflexes, and balance. This ability is the most important one for thieves.");
            int i = 0;
            if (mods[i] != 0)
            {
                sb.Append("\n\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            i++;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            i++;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            i++;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                sb.Append("%");
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public static string GetConstitutionDescription(int score)
        {
            int[] mods = new int[4];
            string[][] arr = new string[][] {
                new string[] { " Hit Point Penalty", " Hit Point Bonus" },
                new string[] { " Poison Resistance Penalty", " Poison Resistance Bonus" },
                new string[] { "% to Survive Resurrection" },
                new string[] { "% to Survive Transformative Shock" } };
            switch (score)
            {
                case 3:
                    mods = new int[] { -3, -2, 40, 35 };
                    break;
                case 4:
                    mods = new int[] { -2, -1, 45, 40 };
                    break;
                case 5:
                    mods = new int[] { -2, -2, 50, 45 };
                    break;
                case 6:
                    mods = new int[] { -1, 0, 55, 50 };
                    break;
                case 7:
                    mods = new int[] { -1, 0, 60, 55 };
                    break;
                case 8:
                    mods = new int[] { -1, 0, 65, 60 };
                    break;
                case 9:
                    mods = new int[] { 0, 0, 70, 65 };
                    break;
                case 10:
                    mods = new int[] { 0, 0, 75, 70 };
                    break;
                case 11:
                    mods = new int[] { 0, 0, 80, 75 };
                    break;
                case 12:
                    mods = new int[] { 0, 0, 85, 80 };
                    break;
                case 13:
                    mods = new int[] { 1, 0, 90, 85 };
                    break;
                case 14:
                    mods = new int[] { 1, 0, 92, 90 };
                    break;
                case 15:
                    mods = new int[] { 1, 0, 94, 93 };
                    break;
                case 16:
                    mods = new int[] { 2, 0, 96, 95 };
                    break;
                case 17:
                    mods = new int[] { 2, 0, 98, 97 };
                    break;
                case 18:
                    mods = new int[] { 3, 0, 100, 99 };
                    break;
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("<b>Constitution</b> (CON) represents a character's health and stamina.\n");
            int i = 0;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            i++;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            i++;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                sb.Append(mods[i]);
                sb.Append(arr[i][0]);
                sb.Append("</b>");
            }
            i++;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                sb.Append(mods[i]);
                sb.Append(arr[i][0]);
                sb.Append("</b>");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public static string GetIntelligenceDescription(int score)
        {
            int[] mods = new int[5];
            string[][] arr = new string[][] {
                new string[] { "Knows No Additional Languages", "Knows One Additional Languages", "Knows Two Additional Languages", "Knows Three Additional Languages" },
                new string[] { "Unable to read or write, communicates in broken speech", "Partial ability to write", "Able to read and write" },
                new string[] { "% Chance to Learn New Spell" },
                new string[] { " Minimum Spells per Level" },
                new string[] { " Maximum Spells per Level" } };
            switch (score)
            {
                case 3:
                    mods = new int[] { 0, 0, 20, 2, 3 };
                    break;
                case 4:
                case 5:
                    mods = new int[] { 0, 0, 30, 2, 4 };
                    break;
                case 6:
                case 7:
                    mods = new int[] { 0, 1, 35, 2, 5 };
                    break;
                case 8:
                    mods = new int[] { 0, 1, 40, 3, 6 };
                    break;
                case 9:
                    mods = new int[] { 0, 2, 40, 3, 6 };
                    break;
                case 10:
                case 11:
                case 12:
                    mods = new int[] { 0, 2, 50, 4, 7 };
                    break;
                case 13:
                case 14:
                    mods = new int[] { 1, 2, 70, 5, 9 };
                    break;
                case 15:
                    mods = new int[] { 1, 2, 75, 6, 11 };
                    break;
                case 16:
                    mods = new int[] { 2, 2, 75, 6, 11 };
                    break;
                case 17:
                    mods = new int[] { 2, 2, 85, 7, 9999 };
                    break;
                case 18:
                    mods = new int[] { 3, 2, 90, 8, 9999 };
                    break;
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("<b>Intelligence</b> (INT) determines how well a character learns, remembers, and reasons. In addition, INT may be used to influence the minimum and maximum spell level potential of mages and illusionists.");
            // 0 - LANGUAGES
            int i = 0;
            sb.Append("\n\n<b>");
            sb.Append(arr[i][mods[i]]);
            sb.Append("</b>");
            i++;
            // 1 - READ/WRITE
            sb.Append("\n<b>");
            sb.Append(arr[i][mods[i]]);
            sb.Append("</b>");
            i++;
            // 2 - % CHANCE LEARN SPELL
            sb.Append("\n<b>");
            sb.Append(mods[i]);
            sb.Append(arr[i][0]);
            sb.Append("</b>");
            i++;
            // 3 - MIN SPELLS
            sb.Append("\n<b>");
            sb.Append(mods[i]);
            sb.Append(arr[i][0]);
            sb.Append("</b>");
            i++;
            // 4 - MAX SPELLS
            sb.Append("\n<b>");
            UnityEngine.Debug.Log(i);
            UnityEngine.Debug.Log(mods.Length);
            if (mods[i] == 9999)
            {
                sb.Append("Unlimited");
            }
            else
            {
                sb.Append(mods[i]);
            }
            sb.Append(arr[i][0]);
            sb.Append("</b>");
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public static string GetWisdomDescription(int score)
        {
            int[] mods = new int[2];
            string[][] arr = new string[][] {
                new string[] { " Saving Throw vs. Magic Penalty", " Saving Throw vs. Magic Bonus" },
                new string[] { "% Chance of Divine Spell Failure" } };
            switch (score)
            {
                case 3:
                    mods = new int[] { -3, 100 };
                    break;
                case 4:
                case 5:
                    mods = new int[] { -2, 100 };
                    break;
                case 6:
                case 7:
                case 8:
                    mods = new int[] { -1, 100 };
                    break;
                case 9:
                    mods = new int[] { 0, 20 };
                    break;
                case 10:
                    mods = new int[] { 0, 15 };
                    break;
                case 11:
                    mods = new int[] { 0, 10 };
                    break;
                case 12:
                    mods = new int[] { 0, 5 };
                    break;
                case 13:
                case 14:
                case 15:
                    mods = new int[] { 1, 0 };
                    break;
                case 16:
                case 17:
                    mods = new int[] { 2, 0 };
                    break;
                case 18:
                    mods = new int[] { 3, 0 };
                    break;
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("<b>Wisdom</b> (WIS) describes a character's willpower, common sense, perception, and intuition. Wisdom is the most important ability for clerics and druids.\n");
            int i = 0;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            i++;
            if (mods[i] != 100)
            {
                sb.Append("\n<b>");
                sb.Append(mods[i]);
                sb.Append(arr[i][0]);
                sb.Append("</b>");
            }
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public static string GetCharismaDescription(int score)
        {
            int[] mods = new int[2];
            string[][] arr = new string[][] {
                new string[] { " Reaction Penalty", " Reaction Bonus" },
                new string[] { " Maximum Retainers" },
                new string[] { "Retainer Morale Score: " } };
            switch (score)
            {
                case 3:
                    mods = new int[] { 2, 1, 4 };
                    break;
                case 4:
                case 5:
                    mods = new int[] { 1, 2, 5 };
                    break;
                case 6:
                case 7:
                case 8:
                    mods = new int[] { 1, 3, 6 };
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    mods = new int[] { 0, 4, 7 };
                    break;
                case 13:
                case 14:
                case 15:
                    mods = new int[] { -1, 5, 8 };
                    break;
                case 16:
                case 17:
                    mods = new int[] { -1, 6, 9 };
                    break;
                case 18:
                    mods = new int[] { -2, 7, 10 };
                    break;
            }
            PooledStringBuilder sb = StringBuilderPool.Instance.GetStringBuilder();
            sb.Append("<b>Charisma</b> (CHA) measures a characterÊs force of personality, persuasiveness, personal magnetism, ability to lead, and physical attractiveness. This ability is important for how other characters or monsters will respond to a character in an encounter, and affects the morale of hirelings and the number of retainers a character may have.\n");
            int i = 0;
            if (mods[i] != 0)
            {
                sb.Append("\n<b>");
                if (score < 9)
                {
                    sb.Append("<color=red>");
                }
                if (mods[i] > 0)
                {
                    sb.Append("+");
                }
                sb.Append(mods[i]);
                if (score < 9)
                {
                    sb.Append("</color>");
                    sb.Append(arr[i][0]);
                }
                else
                {
                    sb.Append(arr[i][1]);
                }
                sb.Append("</b>");
            }
            i++;
            sb.Append("\n<b>");
            sb.Append(mods[i]);
            sb.Append(arr[i][0]);
            sb.Append("</b>");
            i++;
            sb.Append("\n<b>");
            sb.Append(arr[i][0]);
            sb.Append(mods[i]);
            sb.Append("</b>");
            string s = sb.ToString();
            sb.ReturnToPool();
            return s;
        }
        public string GetAbilityModifierTextForCharacterSheet(LabLordCharacter pc)
        {
            string s = "";
            int score = (int)pc.GetFullAttributeScore(Abbr);
            // strength
            Dictionary<int, EquipmentItemModifier> dict = GetModifiersForScore(score);
            if (dict != null)
            {
                switch (Code)
                {
                    case ABILITY_STR:
                        s = GetStrengthModifierText(pc, score, dict);
                        break;
                    case ABILITY_DEX:
                        s = GetDexterityModifierText(pc, score, dict);
                        break;
                    case ABILITY_CON:
                        s = GetConstitutionModifierText(pc, score, dict);
                        break;
                    case ABILITY_INT:
                        s = GetIntelligenceModifierText(pc, score, dict);
                        break;
                    case ABILITY_WIS:
                        s = GetWisdomModifierText(pc, score, dict);
                        break;
                    case ABILITY_CHA:
                        s = GetCharismaModifierText(pc, score, dict);
                        break;
                }

            }
            return s;
        }
    }
}
