using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public int level;
    public int attackDamage;
    public float attackSpeed;
    public int defenceBonus;
    public equipmentType equipmentGenre;

    // REQUIRMENT SECTION
    public int reqLevel;
    public int reqStr;
    public int reqInt;
    public int reqDex;

    // ITEM BUILD IN STATS POWERUPS
    public int numberOfRandomBonuses;
    public List<ItemExtraBonus> listOfItemBonuses;
  
    public enum equipmentType
    {
        Armor,
        Helmet,
        Boots,
        Pants,
        Shield,
        Weapon,
        Necklase,
        Ring
    }

    private void Awake() {
        type = itemType.Equipment;
        stackableSize = 1;
    }

    [Serializable] public class ItemExtraBonus
    {
        public BonusCategory BonusCategory;
        public BonusType BonusType;
        public string NameOfBonus;
        public float Value;

        // Storing a Types of bonus to easiest finding or creating
    }
    public enum BonusType{
        Multyplyfer,
        Addition,
        HealingSpeed,
        DamageOverTime,
        CriticalStrikeChance,
        CriticalDamage,
        HP,
        Mana,

    }
    public enum BonusCategory
    {
        // Everything like extra damage, crit, attack speed bonuses
        Attack,

        // Bonuses like %def, armor, resistance, dodge etc. 
        Deffense,

        // Hero statistic additions Health, Mana,intelligence, strength, ...
        HeroStats,

        // More complicated stuff like, Aoe damage, extra hits every x hits, 
        //  faster regeneration, random effects?
        SpecialAbility,

        // Resistance x % to <earth, water, fire> elements attack ?
        Resistance
    }
}
