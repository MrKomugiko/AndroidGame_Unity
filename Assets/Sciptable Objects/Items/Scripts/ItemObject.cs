using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    Food,
    Equipment,
    Default
}
public enum ratityType
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Mistic,
    Legendary
}

public class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public itemType type;
    [TextArea(15,20)]
    public string description;
    public int stackableSize;
    public Sprite ItemImage;
    public ratityType rarity;
}
