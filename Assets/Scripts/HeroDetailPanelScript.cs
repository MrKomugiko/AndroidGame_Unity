using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDetailPanelScript : MonoBehaviour
{
    [SerializeField] public bool NeedUpdate;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject ItemDetailsWindow;
    private Player playerInstance;

    private Dictionary<string, string> ItemSlotsByType = new Dictionary<string, string>()
    {
        {"Armor","ChestSlot"},
        {"Helmet","HelmetSlot"},
        {"Boots","BootsSlot"},
        {"Pants","TrausersSlot"},
        {"Shield","ShieldSlot"},
        {"Weapon","SwordSlot"},
        {"Necklase","NecklaseSlot"},
        {"Ring","Ring"}
    };


    private Dictionary<string, Color32> rarityColors = new Dictionary<string, Color32>() {
            {   "Common",      new Color32(0,0,0,0)          }, // none/transparent
            {   "Uncommon",    new Color32(135,108,108,128)  }, // grey
            {   "Rare",        new Color32(85,255,8,128)     }, // green
            {   "Epic",        new Color32(0,143,255,128)    }, // blue
            {   "Mistic",      new Color32(177,0,255,128)    }, // pink
            {   "Legendary",   new Color32(255,246,0,201)    }  // orange
    };
    enum  RingsSlotsName  
    {
        Ring1Slot,
        Ring2Slot,
        Ring3Slot
    };

    void Start()
    {
        NeedUpdate = true;
        playerInstance = PlayerObject.GetComponent<Player>();
    }

    void Update()
    {
        if (NeedUpdate) {
            PutCurentlyUsingEquipmentIntoSlot();
            DisplayPlayerStatistics();
            NeedUpdate = false;
        }
    }
    private void PutCurentlyUsingEquipmentIntoSlot() {
        string slotName;
        int ringSlotCounter = 0;
        foreach (var equipmentItem in playerInstance.listOfEquipmentInUse) {
            ItemSlotsByType.TryGetValue(equipmentItem.equipmentGenre.ToString(), out slotName);
            if(slotName == "Ring") {
                ringSlotCounter++;
                    Debug.LogError("Image updates is required"); 
                    UpdateItemSlotOverlayData(slotName, equipmentItem, ring:true, ringIndex:ringSlotCounter);                   
                }
            else {
                UpdateItemSlotOverlayData(slotName, equipmentItem);        
                }
            }
        }

    private void UpdateItemSlotOverlayData(string slotName, EquipmentObject equipmentItem, bool ring = false, int ringIndex=0)
    {
        GameObject slot = null;

        if(!ring){
            slot = this.transform.Find("itemsSlots").Find(slotName).gameObject;
        }
        
        if(ring){
            switch(ringIndex){
                case 1:
                    slot = this.transform.Find("itemsSlots").Find(RingsSlotsName.Ring1Slot.ToString()).gameObject;
                    break;
                case 2:
                    slot = this.transform.Find("itemsSlots").Find(RingsSlotsName.Ring2Slot.ToString()).gameObject;
                    break;
                case 3:
                    slot = this.transform.Find("itemsSlots").Find(RingsSlotsName.Ring3Slot.ToString()).gameObject;
                    break;
            }
        }
        
        ColorBackgroundDeppendOfItemRarity(slot, equipmentItem);           
        AddItemImageToSlot(slot, equipmentItem);
    }
    private void DisplayPlayerStatistics() {
        string statisticText =
            $"Level: \t\t{playerInstance.Level}\n" +
            $"Experience: \t{playerInstance.Experience}\n" +
            $"Health: \t\t{playerInstance.Health}\n" +
            $"Atk. Damage: \t{playerInstance.MinAtkDamage}/{playerInstance.MaxAtkDamage}\n" +
            $"Armor: \t\t{playerInstance.Armor}\n\n" +
            $"\tStr: \t{playerInstance.Strength}\tDex: \t{playerInstance.Dexterity}\n" +
            $"\tInt: \t{playerInstance.Inteligence}\tVit: \t{playerInstance.Vitality}";

        this.transform.Find("PlayerStatistic").GetComponent<TextMeshProUGUI>().SetText(statisticText);
    }

    private void ColorBackgroundDeppendOfItemRarity(GameObject slot, EquipmentObject item) {
        slot.transform.Find("BackgroundRarityColor").gameObject.SetActive(true);
        Color32 bgColor;
        rarityColors.TryGetValue(item.rarity.ToString(), out bgColor);
        slot.transform.Find("BackgroundRarityColor").GetComponent<Image>().color = bgColor;
    }

    private void AddItemImageToSlot(GameObject slot, EquipmentObject item) {
        slot.transform.Find("Image").gameObject.SetActive(true);
        slot.transform.Find("Image").GetComponent<Image>().sprite = item.ItemImage;
    }

    public void DisplayTooltipWithItemInfo(string itemType) {
        ItemDetailsWindow.SetActive(true);
        GameObject.Find("TouchingManager").GetComponent<TouchingManagerScript>().UpdateTouchingBehavior();

        // pass data based on item type from slot "itemType"
        EquipmentObject eqItem;
        if (itemType == "Ring1" || itemType == "Ring2" || itemType == "Ring3"  ) { // becouse we have 3 ring slots
            int ringSlotIndex = 0;
            switch (itemType) {
                case "Ring1":
                    ringSlotIndex = 0;
                    break;
                case "Ring2":
                    ringSlotIndex = 1;
                    break;
                case "Ring3":
                    ringSlotIndex = 2;
                    break;
                default:
                    ringSlotIndex = 0;
                    break;
            }
            eqItem = playerInstance.listOfEquipmentInUse
                .Where(i => i.equipmentGenre.ToString() == "Ring").ElementAt(ringSlotIndex);
        } else {
             eqItem = playerInstance.listOfEquipmentInUse
                .Where(i => i.equipmentGenre.ToString() == itemType).First();
        }

        //SETING NAMES
        ItemDetailsWindow.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>().SetText(eqItem.name.ToString());

        // SETTING RARITY COLOR TEXT WITH TEXT
        Color32 bgColor;
        rarityColors.TryGetValue(eqItem.rarity.ToString(), out bgColor);
        ItemDetailsWindow.transform.Find("ItemRarityName").GetComponent<TextMeshProUGUI>().color = bgColor;

        ItemDetailsWindow.transform.Find("ItemRarityName").GetComponent<TextMeshProUGUI>().SetText("( "+eqItem.rarity.ToString()+" )");

        // SETTING IMAGE
        ItemDetailsWindow.transform.Find("ItemImage").GetComponent<Image>().sprite = eqItem.ItemImage;

        // SETING DESCRIPTION AND REQUIRMENTS
        // ITS CHANGE DEPPEND OF ITEM TYPE
        string statisticText = "";
        if (itemType == "Weapon") {
            statisticText = $"<b>Statistic:</b>\n" +
                $"Level: \t{eqItem.level}\n" +
                $"Attack: \t{eqItem.attackDamage}-{eqItem.attackDamage * 1.25f}\n" +
                $"Speed: \t{eqItem.attackSpeed}\n";
        } else if (itemType != "Necklase" && itemType != "Ring"){
            statisticText = $"<b>Statistic:</b>\n" +
                $"Level: \t{eqItem.level}\n" +
                $"Armor: \t{eqItem.defenceBonus}\n";
        }else {
            statisticText = $"<b>Statistic:</b>\n" +
               $"Level: \t{eqItem.level}\n" +
               $"Attack: \t{eqItem.attackDamage}-{eqItem.attackDamage * 1.25f}\n" +
               $"Speed: \t{eqItem.attackSpeed}\n" +
               $"Armor: \t{eqItem.defenceBonus}\n";
        }

        string requirmentsText = MakeItemRequirmentsTextToDisplay(eqItem);

        ItemDetailsWindow.transform.Find("DescriptionAndOtherInfo").GetComponent<TextMeshProUGUI>().SetText(eqItem.description.ToString());
        ItemDetailsWindow.transform.Find("Stat Info").GetComponent<TextMeshProUGUI>().SetText(statisticText);
        ItemDetailsWindow.transform.Find("Requirments Info").GetComponent<TextMeshProUGUI>().SetText(requirmentsText);
    }

    private string MakeItemRequirmentsTextToDisplay(EquipmentObject eqItem) {

        string reqText = $"<b>Requirments:</b>\n";
        
        if(playerInstance.Level < eqItem.reqLevel) {
            reqText += $"<color=red>Lvl: \t\t{eqItem.reqLevel}</color>\n";
        } else {
            reqText += $"Lvl: \t\t{eqItem.reqLevel}\n";
        }
        
        if (playerInstance.Strength < eqItem.reqStr) {
            reqText += $"<color=red>Str: \t\t{eqItem.reqStr}</color>\n";
        } else {
            reqText += $"Str: \t\t{eqItem.reqStr}\n";
        }

        if (playerInstance.Dexterity < eqItem.reqDex) {
            reqText += $"<color=red>Dex: \t\t{eqItem.reqDex}</color>\n";
        } else {
            reqText += $"Dex: \t\t{eqItem.reqDex}\n";
        }

        if (playerInstance.Inteligence < eqItem.reqInt) {
            reqText += $"<color=red>Int: \t\t{eqItem.reqInt}</color>\n";
        } else {
            reqText += $"Int: \t\t{eqItem.reqInt}\n";
        }

        return reqText;
    }
    public void OnClick_CloseItemTooltipWindow() {

        ItemDetailsWindow.SetActive(false);
        GameObject.Find("TouchingManager").GetComponent<TouchingManagerScript>().UpdateTouchingBehavior();

    }
    public void OnClick_CloseWindow() {
        GameObject.Find("HeroDetailsPanel").SetActive(false);
        GameObject.Find("TouchingManager").GetComponent<TouchingManagerScript>().UpdateTouchingBehavior();
    }
}