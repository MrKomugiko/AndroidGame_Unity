using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public float buttonHoldingStartTime;
    [SerializeField] public float buttonHoldingTimeElapsed;
    [SerializeField] GameObject RequirmentBackground;
    [SerializeField] GameObject FloatingTextWindow;
    ItemObject item;

    private void Start() {
        item = this.transform.GetComponentInParent<ItemDataHolderScript>().itemData.item;
    }
    public void OnPointerDown(PointerEventData eventData) {
    
        Debug.Log("pressed and timer start counting");
        // time start counting
        buttonHoldingStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData) {
        // time end counting and check how long button was pressed 
        // then determine if just open like clasic click or make another action because button   were hold
        buttonHoldingTimeElapsed = Time.time - buttonHoldingStartTime;
        Debug.Log("Button released after "+buttonHoldingTimeElapsed);
        if (buttonHoldingTimeElapsed <= 0.2 && buttonHoldingTimeElapsed >=0.05) {
            if (!RequirmentBackground.activeSelf) {
               this.transform.GetComponentInParent<ItemDataHolderScript>().UseItem();
            } else {
                GameObject.Find("PlayerInventory").GetComponent<InventoryScript>().DisplayFloatingInfoWithText("You cant wear this item, you do not meet requirments.");
            }
        } else if (buttonHoldingTimeElapsed >0.2 && buttonHoldingTimeElapsed < 2) {
            this.transform.GetComponentInParent<ItemDataHolderScript>().DisplayItemActionListWindow(item);
        }
        // reset time counter;
        buttonHoldingStartTime = 0;
    }
}