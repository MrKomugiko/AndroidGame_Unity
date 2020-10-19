using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemComparingScript : MonoBehaviour
{
    [SerializeField] private EquipmentObject _selectedItem;
    [SerializeField] private EquipmentObject _currentEquippedItem;
    [SerializeField] private GameObject selectedItemSection;
    [SerializeField] private GameObject currentEquippedItemSection;
    [SerializeField] private GameObject comparisonSection;

    public ItemObject SelectedItem { 
        get => _selectedItem; 
        set => _selectedItem = value as EquipmentObject; 
    }
    public ItemObject CurrentEquippedItem { 
        get => _currentEquippedItem; 
        set => _currentEquippedItem = value as EquipmentObject; 
    }
    void Start()
    {

    }
    void Update()
    {
        
    }
    public void OnClick_CloseWindow() {
        this.gameObject.SetActive(false);
    }
    public void RUN_COMPAIRSON_WINDOW_CONFIGURATION_PIPELINE() {
        Debug.Log("Configuration is starting");
        ConfigureItemDetailSection(_selectedItem as EquipmentObject, selectedItemSection /*current section*/);
        ConfigureItemDetailSection(_currentEquippedItem as EquipmentObject, currentEquippedItemSection /*current section*/);
        ConfigureComparisomSection(_selectedItem as EquipmentObject, _currentEquippedItem as EquipmentObject, comparisonSection);
    }
    void ConfigureItemDetailSection(EquipmentObject item, GameObject section /* curent or equipped section*/) {
        // Set image
        section.transform.Find("ItemImage").GetComponent<Image>().sprite = item.ItemImage;

        // Set name
        section.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>()
            .SetText(item.name);

        // Extract BaseStatistic and assign to proper section
        section.transform.Find("ItemStats").GetComponent<TextMeshProUGUI>()
            .SetText(GenerateCoreInfoTextForAChoosenItem(item));

        // Extract Special Ability of item and assign to proper section
        section.transform.Find("ItemSpecialAbility").GetComponent<TextMeshProUGUI>()
            .SetText(GenerateSpecialAbilityInfoTextForAChoosenItem(item));
    }
    string GenerateCoreInfoTextForAChoosenItem(EquipmentObject item) {
        Debug.Log("Extractingbase item stats of item:" + item.name);
        // TODO 1(IMPORTANT): changing returning core info string values depend of item genre
        if(item.equipmentGenre.ToString() == "Weapon") {
            string itemCoreInfoString =
                $"Level \t\t\t {item.level}\n" +
                // TODO 2: Add max damage in item model, currently everywhere max damage is damage multiply by 1.25, 
                //         and sometimes weapons sould be can use more or less maybe 1-10 or 4-5 attack damages
                $"Attack \t\t {item.attackDamage}-{item.attackDamage*1.25}\n" +
                $"AttackSpeed \t {item.attackSpeed}\n" +
                $"{GenerateItemDPSAndAvgDPGString(item.attackDamage, item.attackDamage*1.25, item.attackSpeed)}\n" +
                $"\n" +
                // TODO 5: Add gem slots system in items, optional
                $"<align=center>[Empty Gem Slot] W.I.P</align>";

            return itemCoreInfoString;
        }
        return "ERROR - not a weapon";
    }
    string GenerateItemDPSAndAvgDPGString(double minDMG, double maxDMG, double attackSpeed) {
        double minDPS, maxDPS, avgDPS;
        GetDPSandAvgDpsValues(minDMG, maxDMG, attackSpeed).TryGetValue("minDPS", out minDPS);
        GetDPSandAvgDpsValues(minDMG, maxDMG, attackSpeed).TryGetValue("avgDPS", out avgDPS);
        GetDPSandAvgDpsValues(minDMG, maxDMG, attackSpeed).TryGetValue("maxDPS", out maxDPS);

        return $"<align=center>DPS: {minDPS} - {maxDPS} (<b>{avgDPS}</b>)</align>";
    }
    static Dictionary<string,double> GetDPSandAvgDpsValues(double minDMG, double maxDMG, double attackSpeed) {
        double minDPS, maxDPS, avgDPS;

        minDPS = Math.Round(minDMG * attackSpeed, 2);
        maxDPS = Math.Round(maxDMG * attackSpeed, 2);
        avgDPS = Math.Round((minDPS + maxDPS) / 2, 2);

        Dictionary<string, double> WeaponDPSandAvgDPSDict = new Dictionary<string, double>() {
            {"minDPS",minDPS },
            {"maxDPS",maxDPS },
            {"avgDPS",avgDPS }
        };

        return WeaponDPSandAvgDPSDict;
    }
    string GenerateSpecialAbilityInfoTextForAChoosenItem(EquipmentObject item) {
        Debug.Log("Extractingbase item unique ablility of item:" + item.name);
        //TODO 4: Add special abilities list in item object like resistances, special attacks, passives
        string itemSpecialAbilityString = "";
        foreach (var bonus in item.listOfItemBonuses){           
            itemSpecialAbilityString += $"*{bonus.NameOfBonus} <b>[{bonus.Value}]</b>\n";
        }
        return itemSpecialAbilityString;
    }
    void ConfigureComparisomSection(EquipmentObject selectedItem, EquipmentObject equippedItem, GameObject compareSection) {
        // Make Comparison and calculation, extract differences about items
        compareSection.transform.Find("StandardDifferencesText").GetComponent<TextMeshProUGUI>()
            .SetText(GenerateStandardComparisonBeetwenTwoItems(selectedItem, equippedItem));
        compareSection.transform.Find("UniqueDifferencesText").GetComponent<TextMeshProUGUI>()
            .SetText(GenerateUniqueAbilityComparisonBeetwenTwoItems(selectedItem, equippedItem));
    }
    string GenerateStandardComparisonBeetwenTwoItems(EquipmentObject selectedItem, EquipmentObject equippedItem) {
        // TODO 1(IMPORTANT): changing returning compairson result string value
       if(selectedItem.equipmentGenre.ToString() == "Weapon") {
            string green = "green", red = "red", extraBonusesCompareString="";

            #region Min/Max Damage
                // compare min damage
                double minSelected = Math.Round((double)selectedItem.attackDamage, 2),
                    minEquipped = Math.Round((double)equippedItem.attackDamage, 2),
                    maxSelected = Math.Round(selectedItem.attackDamage * 1.25, 2), 
                    maxEquipped = Math.Round(equippedItem.attackDamage*1.25, 2),
                // compare max damage
                    minDifference = minSelected - minEquipped,
                    maxDifference = maxSelected - maxEquipped;
                // fuse and coloring damage text
                string attackDifferencesString = $"<color={(minDifference > 0 ? green : red)}> {minDifference} </color>/<color={(maxDifference > 0 ? green:red)}> {maxDifference} </color>";
            #endregion
            #region Attack Speed
            //compare attack speed
            double attackSpeedSelected = Math.Round(selectedItem.attackSpeed, 2),
                attackSpeedEquipped = Math.Round(equippedItem.attackSpeed, 2),
                attackSpeedDifference = Math.Round(attackSpeedSelected - attackSpeedEquipped,2);

                string attackSpeedDifferenceString = $"<color={(attackSpeedDifference>0?green:red)}> {attackSpeedDifference} </color>";
            #endregion
            #region Average DPS
                // create dpses getting from static dictionary method
                double currentDPS, equippedDPS;
                GetDPSandAvgDpsValues(selectedItem.attackDamage, selectedItem.attackDamage * 1.25, selectedItem.attackSpeed).TryGetValue("avgDPS", out currentDPS);
                GetDPSandAvgDpsValues(equippedItem.attackDamage, equippedItem.attackDamage * 1.25, equippedItem.attackSpeed).TryGetValue("avgDPS", out equippedDPS);
                double weaponDPSDifference = Math.Round(currentDPS-equippedDPS, 2);

                string weaponDPSDifferenceString = $"<color={(weaponDPSDifference > 0 ? green : red)}> {weaponDPSDifference} </color>";
            #endregion

            #region Fetching Bonuses from items
                foreach (var selectedItemBonus in selectedItem.listOfItemBonuses){
                    if(!equippedItem.listOfItemBonuses.Where(p=>p.NameOfBonus == selectedItemBonus.NameOfBonus).Any()){
                        // when selected item have value that dont exist in current equipped item
                        extraBonusesCompareString +=  $"* <color=green>{selectedItemBonus.NameOfBonus} <b>[{selectedItemBonus.Value}]</b></color>\n";
                    }
                }

                foreach (var equippedItemBonus in equippedItem.listOfItemBonuses){
                    if(selectedItem.listOfItemBonuses.Where(p=>p.NameOfBonus == equippedItemBonus.NameOfBonus).Any()){
                        var valueExactBonusInSelectedITem = selectedItem.listOfItemBonuses.Where(p=>p.NameOfBonus == equippedItemBonus.NameOfBonus).First().Value;
                        double statDifference = Math.Round(valueExactBonusInSelectedITem - equippedItemBonus.Value,2);
                        extraBonusesCompareString +=  $"* {equippedItemBonus.NameOfBonus} <color={(statDifference >0? green:red)}><b>[{statDifference}]</b></color>\n";
                    }else{
                        extraBonusesCompareString +=  $"* <color=red>{equippedItemBonus.NameOfBonus} <b>[{equippedItemBonus.Value}]</b></color>\n";
                    }
                }
            #endregion

            string differencesAboutItemsStats =
                $"<b><u> Base Statistics </u></b>\n\n" +
                $"* Attack:	\t<b>{attackDifferencesString}</b>\n" +
                $"* Attack Speed: \t<b>{attackSpeedDifferenceString}</b>\n" +
                $"* Average DPS: \t<b>{weaponDPSDifferenceString}</b>\n" +
                // TODO 1(CURRENTLY WORKING ON): if weapon have extra stats like additional str, dex, crit there should be foreach loop
                $"{extraBonusesCompareString}";
//                $"* Critical Strike:<color=black>\t <b>W.I.P.</b></color>\n" +
  //              $"* Strength:<color=black>\t\t <b>W.I.P.</b></color>";

            return differencesAboutItemsStats;
       }
        return "ERROR - not a Weapon";
}
    string GenerateUniqueAbilityComparisonBeetwenTwoItems(EquipmentObject selectedItem, EquipmentObject equippedItem) {
        string differencesAboutSpecialAbilities = "<b><u>Unique Equipment abilites:</u></b> \n\n SPECIAL ABILITIES NOT IMPLEMENTED <u>YET</u> \n unable to make compairson ";
        return differencesAboutSpecialAbilities;
    }
}

