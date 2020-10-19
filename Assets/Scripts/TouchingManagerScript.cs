using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingManagerScript : MonoBehaviour
{
    /*
     
     detecting what window is currently open, and enable or disable player touch input
     its for case to do not moving player while is in hero detail window open
     or dont detect sliding navbar while window is open on it

         */

    #region Hold windows to check is open or not

    [SerializeField] private GameObject HeroDetailWindow;
    [SerializeField] private bool HeroWindowIsOpen;
    [SerializeField] private GameObject ItemDetailsWindow;
    [SerializeField] private bool ItemDetailWindowIsOpen;
    [SerializeField] private GameObject NavBarWindow;
    [SerializeField] private bool NavBarWindowIsOpen;
    //-------------------------------------------------------------

    private MoveByTouchScript GeneralPlayerMoving;
    private TouchSwipingScipt SwipingNavbar;


    #endregion
    void Start() {

        GeneralPlayerMoving = GameObject.Find("Player").GetComponent<MoveByTouchScript>();
        SwipingNavbar = GameObject.Find("GameOptionTabs").GetComponent<TouchSwipingScipt>();

        GeneralPlayerMoving.isTouchingDetectEnabled = false;

        HeroWindowIsOpen = false;
        ItemDetailWindowIsOpen = false;
        NavBarWindowIsOpen = true;


    }

    public void UpdateTouchingBehavior() {
        // execute while any window is open 
        DetectWhatWindowIsCurrentlyOpened();
        EnableOrDisableTouchingScripts();
    }

    private void DetectWhatWindowIsCurrentlyOpened() {
        // if hero details is on turn off all touch scripts
        HeroWindowIsOpen = HeroDetailWindow.activeSelf;
        if (HeroWindowIsOpen) {
            Debug.Log("Hero window is currently open");
        }

        ItemDetailWindowIsOpen = ItemDetailsWindow.activeSelf;
        if (ItemDetailWindowIsOpen) {
            Debug.Log("ItemDetailPage is currently open");
        }

        NavBarWindowIsOpen = NavBarWindow.GetComponent<NavigationTabsScript>().IsWindowOpen;
        if (NavBarWindowIsOpen) {
            Debug.Log("NavigationBar is currently open");
        }
    }

    private void EnableOrDisableTouchingScripts() {
        
        if (NavBarWindowIsOpen) {
            // if navbar is open then disable player movement
            GeneralPlayerMoving.isTouchingDetectEnabled = false;
            SwipingNavbar.isTouchingDetectEnabled = true;
        } else {
            GeneralPlayerMoving.isTouchingDetectEnabled = true;
        }

        if (HeroWindowIsOpen) {
            GeneralPlayerMoving.isTouchingDetectEnabled = false;
            SwipingNavbar.isTouchingDetectEnabled = false;
        } else {
            SwipingNavbar.isTouchingDetectEnabled = true;
        }

        if (ItemDetailWindowIsOpen) {
            // hero or item window is open , then disable player movment and sliding script
            SwipingNavbar.isTouchingDetectEnabled = false;
        }

    }
}

