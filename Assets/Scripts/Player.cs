using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{    
    // Player Statistics
    [SerializeField] private int _level;
    [SerializeField] private int _experience;
    [SerializeField] private float _health;
    // basic hero statistic
    [SerializeField] private int _strength;
    [SerializeField] private int _dexterity;
    [SerializeField] private int _inteligence;
    [SerializeField] private int _agility;
    [SerializeField] private int _vitality;
    // attack stuff
    [SerializeField] private int _minAtkDamage;
    [SerializeField] private float _maxAtkDamage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private int _armor;

   public InventoryObject inventory;
    [SerializeField] public List<EquipmentObject> listOfEquipmentInUse;
    [SerializeField] public bool _playerStatsIsChanged = false;
    public int Level { 
        get => _level; 
        set { 
            _level = value; 
            _playerStatsIsChanged = true; } 
    }
    public int Experience { get => _experience; 
        set {
            if(value >= MaxPlayerExperienceAtLevel(Level)) {
                int resztaExpa = value - MaxPlayerExperienceAtLevel(Level);
                _experience = resztaExpa;

                LevelUp();
                Debug.Log("Congratulation => Level UP!");
            } else {
                _experience = value;
            }
        } 
    }
    public float Health { 
        get => _health;
        set { 
            _health = value;
            if(_health > MaxPlayerHealth()) {
                _health = MaxPlayerHealth();
            }else if(_health <= 0f) {
                _health = 0f;
            }
        }
    }
    public int Strength { 
        get => _strength; 
        set { 
            _strength = value;
            _playerStatsIsChanged = true;
        } 
    }
    public int Dexterity {
        get => _dexterity; 
        set {
            _dexterity = value; _playerStatsIsChanged = true;
        }
    }
    public int Inteligence {
        get => _inteligence; 
        set {
            _inteligence = value; _playerStatsIsChanged = true;
        }
    }
    public int Agility {
        get => _agility; 
        set {
            _agility = value; _playerStatsIsChanged = true;
        }
    }

    public int Vitality {
        get => _vitality; 
        set {
            _vitality = value; _playerStatsIsChanged = true;
        }
    }
    public int MinAtkDamage { get => _minAtkDamage; set => _minAtkDamage = value; }
    public float MaxAtkDamage { get => _maxAtkDamage; set => _maxAtkDamage = value; }
    public float AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public int Armor { get => _armor; set => _armor = value; }

    public void OnTriggerEnter2D(Collider2D collision) {
        var item = collision.GetComponent<Item>();
        if (item) {
            inventory.AddItem(item.item, 1);
            Destroy(collision.gameObject);
        }

       // Debug.Log("Collected a "+item.name);

        GameObject.Find("PlayerInventory").GetComponent<InventoryScript>().UpdateInventoryContent();
    }

    private void OnApplicationQuit() {
        inventory.Container.Clear();
    }

    private void Start() {
        Level = 1;
        Experience = 0;
        Health = 50;
        Strength = 1;
        Dexterity = 1;
        Inteligence = 1;
        Agility = 1;
        Vitality = 1;

        MinAtkDamage = 1;
        MaxAtkDamage = 5;
    }

    public float MaxPlayerHealth() {
        return 100f + (Level*10f);
    }

    public bool CheckIfItemWithThisGenreAlreadyIsWeared(ItemObject newEqItem) {
        var eq = (EquipmentObject)newEqItem;
        var typeOfWearableItem = eq.equipmentGenre;
            
        foreach (var equipmentItem in listOfEquipmentInUse) {
            if (equipmentItem.equipmentGenre == typeOfWearableItem) {
                return true;
            }
        }
        return false;
    }

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
    public EquipmentObject GetCurrentEquippedItemByGenre(string equipmentGenre) {
        var equippedItem = listOfEquipmentInUse
            .Where(p => p.equipmentGenre.ToString() == equipmentGenre)
            .FirstOrDefault();
        
        if(equippedItem != null) {
            return equippedItem;
        } else {
            return null;
        }
    }

    public void SwapEquipmentPositionFromEqToInventory(ItemObject eqItemToSwap) {
        var eq = (EquipmentObject)eqItemToSwap;
        var oldItem = listOfEquipmentInUse.Where(i => i.equipmentGenre == eq.equipmentGenre).FirstOrDefault();
        SubtractOldEquipmentStatsFromPlayer(oldItem);
        listOfEquipmentInUse.Remove(oldItem);
        // Add item to player inventory
        inventory.AddItem(oldItem,1);
        GameObject.Find("PlayerInventory").GetComponent<InventoryScript>().UpdateInventoryContent();
    }

    public void WearEquipment(ItemObject eqItem) {
        var eq = (EquipmentObject)eqItem;
        listOfEquipmentInUse.Add(eq);
        AddEquipmentStatsToPlayer(eq);
    }

    private void SubtractOldEquipmentStatsFromPlayer(EquipmentObject oldEqItem) {
        if(oldEqItem.equipmentGenre.ToString() == "Weapon") {
            MinAtkDamage -= oldEqItem.attackDamage;
            MaxAtkDamage -= oldEqItem.attackDamage * 1.25f;
            AttackSpeed = 1;
            //Debug.Log("Attack stuff back to original values");
        } else {
            Armor -= oldEqItem.defenceBonus;
           // Debug.Log("armor back to original state");
        }
        _playerStatsIsChanged = true;
    }

    private void AddEquipmentStatsToPlayer(EquipmentObject eqItem) {
        if (eqItem.equipmentGenre.ToString() == "Weapon") {
            MinAtkDamage += eqItem.attackDamage;
            MaxAtkDamage += Convert.ToInt32(Math.Round(eqItem.attackDamage * 1.25f, 0));
            AttackSpeed = eqItem.attackSpeed;
        } else {
            Armor += eqItem.defenceBonus;
        }
        _playerStatsIsChanged = true;
}

    public void TakeDamage(float damage) {
        Health -= damage;
        Debug.Log($"Player take {damage} damage, hp left: {Health}");   
    }

    public void Add10ToAllStats() {
        Level += 10;
        Strength += 10;
        Dexterity += 10;
        Inteligence += 10;
        Agility += 10;
        Vitality += 10;
    }


    public void GetHealed(int restoreHealthValue) {
        Health += restoreHealthValue;
    }

    public void GetExperience(int expValue) {
        Experience += expValue;

    }

    public int MaxPlayerExperienceAtLevel(int playerLevel) {
        return 10*playerLevel;
    }

    private void LevelUp() {
        Level++;
    }


}
