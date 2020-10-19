using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public GameObject floatingWindow;
    [SerializeField] public GameObject itemHolderPrefab;
    [SerializeField] public GameObject itemDetailsWindow;
    [SerializeField] public GameObject itemActionListWindow;


    public int ItemsInInventory {
        get {

            CheckIfIsEnoughtSlotsToEnableScrolling();

            return this.transform.childCount;
        }
    }

    private void FixedUpdate() {

        try {
            var cointainers = GameObject.Find("Player").GetComponent<Player>().inventory.Container;
            int counter = 0;
            foreach (var item in cointainers) {
                this.transform.GetChild(counter).GetComponent<ItemDataHolderScript>().ItemSlotIndex = counter;
                counter++;
            }
        } catch (System.Exception e) {
            Debug.Log(e);
        }

        if (GameObject.Find("Player").GetComponent<Player>()._playerStatsIsChanged) {
            var playerInventory = GameObject.Find("Player").GetComponent<Player>().inventory;
            int counter = 0;
            foreach (InventorySlot item in playerInventory.Container) {
                if (item.item.type.ToString() == "Equipment") {
                    if (CheckItemRequirmentsIfIsAbleToWear(item.item as EquipmentObject)) {
                        this.transform.GetChild(counter).GetComponent<ItemDataHolderScript>().needUpdateItemRequirments = true;
                    } else {
                        this.transform.GetChild(counter).GetComponent<ItemDataHolderScript>().needUpdateItemRequirments = false;
                    }
                }
                counter++;
            }
            GameObject.Find("Player").GetComponent<Player>()._playerStatsIsChanged = false;
        }
    }

    public bool CheckItemRequirmentsIfIsAbleToWear(EquipmentObject eqItem) {
        var playerInstance = GameObject.Find("Player").GetComponent<Player>();
        bool wynik = true;

        if (playerInstance.Level < eqItem.reqLevel) {
            wynik = false;
        }

        if (playerInstance.Strength < eqItem.reqStr) {
            wynik = false;
        }

        if (playerInstance.Dexterity < eqItem.reqDex) {
            wynik = false;

        }
        if (playerInstance.Inteligence < eqItem.reqInt) {
            wynik = false;
        }

        return wynik;
    }

    public void UpdateInventoryContent() {
        var playerInventory = GameObject.Find("Player").GetComponent<Player>().inventory;
        int counter = 0;
        foreach (InventorySlot item in playerInventory.Container) {
            if (counter >= ItemsInInventory) {
                var instantiatedItem = Instantiate(itemHolderPrefab, this.transform);
                instantiatedItem.GetComponent<ItemDataHolderScript>().Name = item.item.name;
                instantiatedItem.GetComponent<ItemDataHolderScript>().Amount = item.amount;
                instantiatedItem.GetComponent<ItemDataHolderScript>().ItemSlotIndex = counter;
            }
            UpdateItemsCounters(item, counter);
            counter++;
        }
    }

    public void DisplayFloatingInfoAboutFoosUsing(FoodObject food) {
        var playerInstance = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(ActivationRoutine(floatingWindow, 1));
        if (playerInstance.Health < playerInstance.MaxPlayerHealth()) {
            floatingWindow.GetComponentInChildren<TextMeshProUGUI>().SetText($"You eat a <{food.name}> and restored {food.restoreHealthValue}hp.");
        } else {
            floatingWindow.GetComponentInChildren<TextMeshProUGUI>().SetText($"You eat a <{food.name}>, <b>Your HP is full.</b>");
        }
    }
    public void DisplayFloatingInfoWithText(string text) {
        StartCoroutine(ActivationRoutine(floatingWindow, 1.75f));
        floatingWindow.GetComponentInChildren<TextMeshProUGUI>().SetText($"{text}");
    }
    private IEnumerator ActivationRoutine(GameObject window, float timeInSeconds) {
        window.SetActive(true);
        yield return new WaitForSeconds(timeInSeconds);
        window.SetActive(false);
    }

    void UpdateItemsCounters(InventorySlot item, int itemIndex) {
        this.transform.GetChild(itemIndex).GetComponent<ItemDataHolderScript>().Amount = item.amount;
        this.transform.GetChild(itemIndex).GetComponent<ItemDataHolderScript>().ItemSlotIndex = itemIndex;

        this.transform.GetChild(itemIndex)
        .GetChild(1).GetChild(1)
        .GetComponent<Text>()
        .text = $"{GameObject.Find("Player").GetComponent<Player>().inventory.Container[itemIndex].amount}";
    }

    void CheckIfIsEnoughtSlotsToEnableScrolling() {
        if (this.transform.childCount > 12) {
            GetComponentInParent<ScrollRect>().vertical = true;
        }
    }

}