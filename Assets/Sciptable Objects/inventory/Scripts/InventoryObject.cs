using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObject _item, int _amount) {
        bool hasItem = false;
        //  bool isThisSlotFull = false;
        for (int i = 0; i < Container.Count; i++) {
            if (Container[i].item == _item) {
                // find a match to add, now check if its fit to slot size
                if (Container[i].amount == Container[i].item.stackableSize) {
                    // now need to create new slot ans start adding this item into
                    // check if is already exist added slot
                    AddWhereItFitNext(i, _item, _amount);
                    
                } else {
                    Container[i].AddAmount(_amount);
                    hasItem = true;
                    break;
                }
            }
        }
        if (!hasItem) {
            CreateNewSlot(_item, _amount);
        }
    }

    internal void RemoveEmptyContainer(int itemSlotIndex) {
        Container.RemoveAt(itemSlotIndex);
    }

    void CreateNewSlot(ItemObject item, int amount) {
        Container.Add(new InventorySlot(item, amount));
    }

    void AddWhereItFitNext(int startInventorySlotIndex, ItemObject item, int amount) {
        for (int i = startInventorySlotIndex + 1; i < Container.Count; i++) {
            if (Container[i].item.name == item.name) {
                if (Container[i].amount < item.stackableSize) {
                    Container[i].AddAmount(amount);
                }
            }
        }
    }
}
[Serializable] public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount) {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }

}
