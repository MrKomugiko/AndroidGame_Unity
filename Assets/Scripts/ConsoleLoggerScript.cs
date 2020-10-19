using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleLoggerScript : MonoBehaviour
{
    [SerializeField] public GameObject navigationBar;
    private TouchSwipingScipt navigationBarSwipingScript;
    private NavigationTabsScript navigationBarBaseScript;
    private void Start() {
        navigationBarSwipingScript = navigationBar.GetComponent<TouchSwipingScipt>();
        navigationBarBaseScript = navigationBar.GetComponent<NavigationTabsScript>();
    }
    public void DisplayConsoleText(string direction, float startPosition, float endPosition, float swipingTime, float distance, float fingerUpY = 0) {
        bool isInNavbarArea = navigationBarSwipingScript.CheckIfSwipeBeginInNavbarArea(direction, startPosition, fingerUpY, navigationBarSwipingScript.navBarScreenAreaCorners);
        bool isWindowOpen = navigationBarBaseScript.IsWindowOpen;

        string outputConsoleString = $"- Swipe direction:\t<color=green>{direction}</color>\n" +
                $"- Start position:\t<color=green>({startPosition})</color>\n" +
                $"- End position:\t<color=green>({endPosition})</color>\n" +
                $"- Swiping time:\t<color=green>{swipingTime}s</color>\n" +
                $"- Swipe distance:\t<color=green>{distance}</color>\n" +
                $"- Swipe inside:\t<color=green>{isInNavbarArea}</color>\n" +
                $"\n";
        // TODO: rewrite swiping right/left system , insteed use scrolling panel like in the inventory but horizontally ON
        if (isInNavbarArea) {
            if (isWindowOpen) {
                if (direction == "Right" || direction == "Left") {
                    outputConsoleString += GeneratePlayerBehaviorString(wantSwipe: true);
                } else if (direction == "Down") {
                    // window is already opened, swipe up shouldnt do nothink only swipe down to close is avaiable move
                    outputConsoleString += GeneratePlayerBehaviorString(wantHide: true);
                } else {
                    outputConsoleString += GeneratePlayerBehaviorString(notAllowedActionCode: 1);
                }
            } else {
                if (direction == "Right" || direction == "Left") {
                    outputConsoleString += GeneratePlayerBehaviorString(wantSwipe: true);
                } else if (direction == "Up") {
                    // window is already closed, swipe udown shouldnt do nothink only swipe up to open is available
                    outputConsoleString += GeneratePlayerBehaviorString(wantOpen: true);
                } else {
                    outputConsoleString += GeneratePlayerBehaviorString(notAllowedActionCode: 2);
                }
            }
        } else {
            // player dont grab navbar or dont slide in navbar area ? wait for a new attempt ;d
            outputConsoleString += GeneratePlayerBehaviorString(notAllowedActionCode: 3);
        }

        this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(outputConsoleString);
    }
    private string GeneratePlayerBehaviorString(bool wantHide = false, bool wantOpen = false, bool wantSwipe = false, int notAllowedActionCode = 0) {
        string outputConsoleString;
        /*
         "notAllowedActionCodes":
            0 - everythink goes as should do 
            1 - trying to open window if its already opened
            2 - trying to hide hidden window
            3 - user dont touch navbar, swipe somewhere else
         */
        switch (notAllowedActionCode) {
            case 1:
                outputConsoleString = $"<color=yellow>Player want to open already opened window?</color> ";
                return outputConsoleString;
            case 2:
                outputConsoleString = $"<color=yellow>Player want to hide already hidden window?</color>";
                return outputConsoleString;
            case 3:
                outputConsoleString = $"<color=yellow>Player dont make valid action inside navbar area</color> ";
                return outputConsoleString;
        }

        outputConsoleString = $"- Player want to ";
        outputConsoleString += wantHide ? $"<color=green>hide</color>/" : $"<color=red><s>hide</s></color>/";
        outputConsoleString += wantOpen ? $"<color=green>open</color>/" : $"<color=red><s>open</s></color>/";
        outputConsoleString += wantSwipe ? $"<color=green>swipe</color>/" : $"<color=red><s>swipe</s></color>";

        return outputConsoleString;

    }
}
