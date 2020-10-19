using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataHolderScript : MonoBehaviour
{
    [SerializeField] public string Name;
    [SerializeField] public int Amount;
    [SerializeField] private int _itemSlotIndex;
    [SerializeField] public bool needUpdateItemRequirments;
    [SerializeField] private bool _itemMeedAllRequirments;
    [SerializeField] public InventorySlot itemData;
    private Player _playerInstance;

    public int ItemSlotIndex { get => _itemSlotIndex; set => _itemSlotIndex = value; }
    public bool ItemMeedAllRequirments { get => _itemMeedAllRequirments; private set => _itemMeedAllRequirments = value; }

    private void Start() {
       _playerInstance = GameObject.Find("Player").GetComponent<Player>();

        MakeShureIfItemDataLoadedCorrectly();
        if (itemData.item.type.ToString() == "Equipment") {
            TryMakeItemAbleToUse();
            // do not need to show items counter in equipment items, its nonstacable objects
            // BUTTON > child0 (background), child1 (textcounter)
            this.transform.GetChild(1).GetChild(0).gameObject.SetActive(false); 
            this.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        ColorBackgroundDeppendOfItemRarity();
    }

    private void FixedUpdate() {
        MakeShureIfItemDataLoadedCorrectly();
        if (needUpdateItemRequirments) {
            TryMakeItemAbleToUse();
        }
    }

    public void UseItem() {
        if (itemData.item.type.ToString() == "Default") {
            Debug.Log("You cannot use this item, (its a trash :D)");
        }
        else if (!IsContainerWithItemGonnaBeEmptyAfterUse()) {
            if (itemData.item.type.ToString() == "Food") {
                UsingFoodItem();
            }

            Amount -= 1;
            _playerInstance.inventory.Container[ItemSlotIndex].amount = Amount;
            _playerInstance.inventory.Container[ItemSlotIndex].item.name = Name;
            ChangeItemLabelWithAmountInStack();
            Debug.Log("Item used, now its count:" + Amount);
        } else {
            // USING LAST ITEM FROM STACK
            if (itemData.item.type.ToString() == "Equipment") {
                UsingEquipmentItem();
            }
            if (itemData.item.type.ToString() == "Food") {
                UsingFoodItem();
            }

            Amount -= 1;
            //  Debug.Log("all is used, empty slot gonna disapear now");
            _playerInstance.inventory.RemoveEmptyContainer(ItemSlotIndex);
            Destroy(this.gameObject);
        }
    }
    private void TryMakeItemAbleToUse() {
        if (itemData.item.type.ToString() == "Equipment") {
            ItemMeedAllRequirments = GameObject.Find("PlayerInventory").GetComponent<InventoryScript>().CheckItemRequirmentsIfIsAbleToWear((EquipmentObject)itemData.item);
            if (ItemMeedAllRequirments == false) {
                // TODO: display warring info or dim image
                this.transform.Find("RequirmentBackground").gameObject.SetActive(true);
                //Debug.Log("This item do not meet requirment to use it");
            } else {
                this.transform.Find("RequirmentBackground").gameObject.SetActive(false);
               //Debug.Log("You can wear this item, meet all requirments.");
            }
        }
            needUpdateItemRequirments = false;
    }
    private void MakeShureIfItemDataLoadedCorrectly() {
        if (itemData.item == null) {
            if (_itemSlotIndex >= GameObject.Find("Player").GetComponent<Player>().inventory.Container.Count) {
                _itemSlotIndex = GameObject.Find("Player").GetComponent<Player>().inventory.Container.Count - 1;
            }
            itemData = GameObject.Find("Player").GetComponent<Player>().inventory.Container[_itemSlotIndex];
            this.transform.GetChild(1).GetComponent<Image>().sprite = itemData.item.ItemImage;
        }
    }
    private void ColorBackgroundDeppendOfItemRarity() {
        Dictionary<string, Color32> rarityColors = new Dictionary<string, Color32>() {
            {   "Common",      new Color32(0,0,0,0)          }, // none/transparent
            {   "Uncommon",    new Color32(135,108,108,128)  }, // grey
            {   "Rare",        new Color32(85,255,8,128)     }, // green
            {   "Epic",        new Color32(0,143,255,128)    }, // blue
            {   "Mistic",      new Color32(177,0,255,128)    }, // pink
            {   "Legendary",   new Color32(255,246,0,201)    }  // orange
        };

        Color32 bgColor;
        rarityColors.TryGetValue(itemData.item.rarity.ToString(),out bgColor);
        this.transform.Find("ItemRarityBackgroundColor").GetComponent<Image>().color = bgColor;
    }
    private void UsingFoodItem() {
        var foodItem = (FoodObject)itemData.item;
        // Debug.Log($"You eat <{foodItem.name}>.");

        // USE FOOD TO HEAL PLAYER
        _playerInstance.GetHealed(foodItem.restoreHealthValue);

        // DISPLAY UI INFO WHOS GONA DISSAPEAR ABOUT X SECONDS
         GameObject.Find("PlayerInventory").GetComponent<InventoryScript>().DisplayFloatingInfoAboutFoosUsing(foodItem);
    }
    private void UsingEquipmentItem() {
        var wearableITem = (EquipmentObject)itemData.item;
        if (_playerInstance.CheckIfItemWithThisGenreAlreadyIsWeared(itemData.item)) {
            if (wearableITem.equipmentGenre.ToString() == "Ring") {
                if(_playerInstance.listOfEquipmentInUse.Where(p=>p.equipmentGenre.ToString() == "Ring").Count() == 3) {
                    Debug.Log("You already have 3 rings, now one of them swap with new ring");
                    _playerInstance.SwapEquipmentPositionFromEqToInventory(itemData.item);
                }
            } else {
                _playerInstance.SwapEquipmentPositionFromEqToInventory(itemData.item);
            }
        }
        _playerInstance.WearEquipment(itemData.item);
    }
    private void ChangeItemLabelWithAmountInStack() {
            this.transform
                .GetChild(1) // Button
                .GetChild(1) // "ItemCounterText"
                .GetComponent<Text>()
                .text = $"{Amount}";
    }
    private bool IsContainerWithItemGonnaBeEmptyAfterUse() {
        if (Amount - 1 <= 0) {
            return true;
        }
        return false;
    }





    float buttonHoldingStartTime;
    public void onPress() {
        Debug.Log("pressed and timer start counting");
        // time start counting
        buttonHoldingStartTime = Time.time;
    }

    public void onRelease() {
        // time end counting and check how long button was pressed 
        // then determine if just open like clasic click or make another action because button were hold
        float buttonHoldingTimeElapsed = Time.time - buttonHoldingStartTime;
        if (buttonHoldingTimeElapsed <= 0.35) {
            if (!this.transform.Find("RequirmentBackground").gameObject.activeSelf) {
                UseItem();
            }
        } else if(buttonHoldingStartTime <1){
            DisplayItemTooltip();
            DisplayTooltipWithItemInfo(itemData.item);
        }
        // reset time counter;
        buttonHoldingStartTime = 0;
    }


    public void DisplayItemTooltip() {
        Debug.Log($"displaying tooltip item {itemData.item.name}");
    }


    // Teoretycznie ponizszy kod powinien działąc dla itemków ypu equipment
   [SerializeField] private GameObject ItemDetailsWindow;
   [SerializeField] private GameObject ItemActionsListWindow;


    private Dictionary<string, Color32> rarityColors = new Dictionary<string, Color32>() {
            {   "Common",      new Color32(0,0,0,0)          }, // none/transparent
            {   "Uncommon",    new Color32(135,108,108,128)  }, // grey
            {   "Rare",        new Color32(85,255,8,128)     }, // green
            {   "Epic",        new Color32(0,143,255,128)    }, // blue
            {   "Mistic",      new Color32(177,0,255,128)    }, // pink
            {   "Legendary",   new Color32(255,246,0,201)    }  // orange
    };

    public void DisplayTooltipWithItemInfo(ItemObject item) {
        ItemDetailsWindow = gameObject.GetComponentInParent<InventoryScript>().itemDetailsWindow;

        ItemDetailsWindow.SetActive(true);
        GameObject.Find("TouchingManager").GetComponent<TouchingManagerScript>().UpdateTouchingBehavior();

        // check if this item is equipment or food or default one
        if(item is EquipmentObject){
            Debug.Log("this item is equipment object");
        
            #region EquipmentObject setting up window
        // SETING DESCRIPTION AND REQUIRMENTS
        string statisticText = "";
        var itemEQ = (EquipmentObject) item;
        if (itemEQ.equipmentGenre.ToString() == "Weapon") {
            statisticText = $"<b>Statistic:</b>\n" +
                $"Level: \t{itemEQ.level}\n" +
                $"Attack: \t{itemEQ.attackDamage}-{itemEQ.attackDamage * 1.25f}\n" +
                $"Speed: \t{itemEQ.attackSpeed}\n";
        } 
        else if (itemEQ.equipmentGenre.ToString() != "Necklase" && itemEQ.equipmentGenre.ToString() != "Ring") {
            statisticText = $"<b>Statistic:</b>\n" +
                $"Level: \t{itemEQ.level}\n" +
                $"Armor: \t{itemEQ.defenceBonus}\n";
        } 
        else {
            statisticText = $"<b>Statistic:</b>\n" +
               $"Level: \t{itemEQ.level}\n" +
               $"Attack: \t{itemEQ.attackDamage}-{itemEQ.attackDamage * 1.25f}\n" +
               $"Speed: \t{itemEQ.attackSpeed}\n" +
               $"Armor: \t{itemEQ.defenceBonus}\n";
        }

        string requirmentsText = MakeItemRequirmentsTextToDisplay(itemEQ);

        ItemDetailsWindow.transform.Find("Stat Info").GetComponent<TextMeshProUGUI>().SetText(statisticText);
        ItemDetailsWindow.transform.Find("Requirments Info").GetComponent<TextMeshProUGUI>().SetText(requirmentsText);
        #endregion

        }
        if (item is FoodObject) {
            Debug.Log("this item is food object");
            ItemDetailsWindow.transform.Find("Stat Info").GetComponent<TextMeshProUGUI>().SetText("food");
            ItemDetailsWindow.transform.Find("Requirments Info").GetComponent<TextMeshProUGUI>().SetText("food");

        }
        if (item is DefaultObject) {
            Debug.Log("this item is default object");
            ItemDetailsWindow.transform.Find("Stat Info").GetComponent<TextMeshProUGUI>().SetText("default");
            ItemDetailsWindow.transform.Find("Requirments Info").GetComponent<TextMeshProUGUI>().SetText("default");

        }
        //SETING NAMES
        ItemDetailsWindow.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>().SetText(item.name.ToString());

        // SETTING RARITY COLOR TEXT WITH TEXT it item is a equipment
        Color32 bgColor;
        rarityColors.TryGetValue(item.rarity.ToString(), out bgColor);
        ItemDetailsWindow.transform.Find("ItemRarityName").GetComponent<TextMeshProUGUI>().color = bgColor;

        ItemDetailsWindow.transform.Find("ItemRarityName").GetComponent<TextMeshProUGUI>().SetText("( " + item.rarity.ToString() + " )");

        // SETTING IMAGE
        ItemDetailsWindow.transform.Find("ItemImage").GetComponent<Image>().sprite = item.ItemImage;



        ItemDetailsWindow.transform.Find("DescriptionAndOtherInfo").GetComponent<TextMeshProUGUI>().SetText(item.description.ToString());
    }

    public void DisplayItemActionListWindow(ItemObject item) {
        ItemActionsWindowScript itemActionScript = gameObject.GetComponentInParent<InventoryScript>().itemActionListWindow.GetComponent<ItemActionsWindowScript>();
        if(item as EquipmentObject) {
            itemActionScript.IsCompareEquipmentPossible = _playerInstance.GetCurrentEquippedItemByGenre((item as EquipmentObject).equipmentGenre.ToString());
        } else {
            itemActionScript.IsCompareEquipmentPossible = false;
        }

        itemActionScript.ClickedItemRequirmentsState = _itemMeedAllRequirments;
        itemActionScript.OpenAndPassItemDataToActionListWindow(item,ItemSlotIndex);

    }
    private string MakeItemRequirmentsTextToDisplay(EquipmentObject eqItem) {
        string reqText = $"<b>Requirments:</b>\n";

        if (_playerInstance.Level < eqItem.reqLevel) {
            reqText += $"<color=red>Lvl: \t\t{eqItem.reqLevel}</color>\n";
        } else {
            reqText += $"Lvl: \t\t{eqItem.reqLevel}\n";
        }

        if (_playerInstance.Strength < eqItem.reqStr) {
            reqText += $"<color=red>Str: \t\t{eqItem.reqStr}</color>\n";
        } else {
            reqText += $"Str: \t\t{eqItem.reqStr}\n";
        }

        if (_playerInstance.Dexterity < eqItem.reqDex) {
            reqText += $"<color=red>Dex: \t\t{eqItem.reqDex}</color>\n";
        } else {
            reqText += $"Dex: \t\t{eqItem.reqDex}\n";
        }

        if (_playerInstance.Inteligence < eqItem.reqInt) {
            reqText += $"<color=red>Int: \t\t{eqItem.reqInt}</color>\n";
        } else {
            reqText += $"Int: \t\t{eqItem.reqInt}\n";
        }

        return reqText;
    }


}
