using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionsWindowScript : MonoBehaviour
{

    [SerializeField] private ItemObject _clickedItem;
    [SerializeField] private int _clickedItemSlotIntex;
    [SerializeField] private GameObject ComparingItemsWindow;
    [SerializeField] private GameObject compareButtonBlocker;
    [SerializeField] private ItemDataHolderScript itemScript;
    [SerializeField] private GameObject requirmentButtonBlocker;
    [SerializeField] private bool _clickedItemRequirmentsState;
    [SerializeField] private bool _isCompareEquipmentPossible;


    public ItemObject ClickedItem { 
        get => _clickedItem; 
        set => _clickedItem = 
            value; }
    public bool ClickedItemRequirmentsState {
        get => _clickedItemRequirmentsState;
        set {
            _clickedItemRequirmentsState = value;
            if (_clickedItemRequirmentsState) {
                requirmentButtonBlocker.SetActive(false);
            } else {
                requirmentButtonBlocker.SetActive(true);
            }
        }
    }
    public bool IsCompareEquipmentPossible {
        get => _isCompareEquipmentPossible;
        set {
            _isCompareEquipmentPossible = value;
            if (_isCompareEquipmentPossible) {
                compareButtonBlocker.SetActive(false);
            } else {
                compareButtonBlocker.SetActive(true);
            }
        }
    }


    public void OpenAndPassItemDataToActionListWindow(ItemObject item, int itemSlotIndex) {
        this.gameObject.SetActive(true);
        itemScript = GameObject.Find("PlayerInventory").transform.GetChild(_clickedItemSlotIntex).GetComponentInChildren<ItemDataHolderScript>();
        ClickedItem = item;
        _clickedItemSlotIntex = itemSlotIndex;
        Debug.Log(item.name);

    }
    public void OnClickOpenItemDetailsWindow() {
        itemScript = GameObject.Find("PlayerInventory").transform.GetChild(_clickedItemSlotIntex).GetComponentInChildren<ItemDataHolderScript>();

        itemScript.DisplayTooltipWithItemInfo(ClickedItem);

        Debug.Log("Open Details");
    }
    public void OnClickUseItem() {
        itemScript = GameObject.Find("PlayerInventory").transform.GetChild(_clickedItemSlotIntex).GetComponentInChildren<ItemDataHolderScript>();
        Debug.Log("Item "+itemScript.itemData.item.name);
        itemScript.UseItem();
        this.gameObject.SetActive(false);
    }
    public void OnClickCompareItemWithAlreadyWearedOne() {
        ComparingItemsWindow.SetActive(true);
        ComparingItemsWindow.GetComponent<ItemComparingScript>().SelectedItem = ClickedItem;
        ComparingItemsWindow.GetComponent<ItemComparingScript>().CurrentEquippedItem =
                GameObject.Find("Player").GetComponent<Player>()
                .GetCurrentEquippedItemByGenre((_clickedItem as EquipmentObject)
                .equipmentGenre.ToString());
        ComparingItemsWindow.GetComponent<ItemComparingScript>().RUN_COMPAIRSON_WINDOW_CONFIGURATION_PIPELINE();
    }
    public void OnClickDestroyItem() {
       
       this.gameObject.SetActive(false);
        Destroy(itemScript.gameObject);
        Debug.Log("Item destroyed");

    }

    public void OnClickCloseWindow() {
        this.gameObject.SetActive(false);
    }
}
