using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public int restoreHealthValue;
    private void Awake() {
        type = itemType.Food;
        stackableSize = 5;
    }
}
