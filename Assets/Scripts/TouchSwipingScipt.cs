using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;

public class TouchSwipingScipt : MonoBehaviour
{
    // Script checking if player touch sceen  and swipe in direction
    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private float timeThreshold = 0.5f;

    // public UnityEvent OnSwipeLeft;
    // public UnityEvent OnSwipeRight;
    // public UnityEvent OnSwipeUp;
    // public UnityEvent OnSwipeDown;

    private Vector2 fingerDown;
    private DateTime fingerDownTime;
    private Vector2 fingerUp;
    private DateTime fingerUpTime;

    private Camera myMainCamera;
    private Vector3[] navBarWorldCorners;
    [SerializeField] public Vector3[] navBarScreenAreaCorners;
    [SerializeField] public bool isTouchingDetectEnabled;

    private void Start() {
        isTouchingDetectEnabled = true;
        navBarScreenAreaCorners = GetUiWindowFourCornersPositionInScreenSpace();
    }
    private void Update() {
        if (isTouchingDetectEnabled == true) {
            // MOUSE SWIPING IN COMPUTER 
            if (Input.GetMouseButtonDown(0)) {
                this.fingerDown = Input.mousePosition;
                this.fingerUp = Input.mousePosition;
                this.fingerDownTime = DateTime.Now;
            }
            if (Input.GetMouseButtonUp(0)) {
                this.fingerDown = Input.mousePosition;
                this.fingerUpTime = DateTime.Now;
                navBarScreenAreaCorners = GetUiWindowFourCornersPositionInScreenSpace();
                this.CheckSwipe();

            }
            // FINGER TOUCHES IN MOBILE
            foreach (Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    this.fingerDown = touch.position;
                    this.fingerUp = touch.position;
                    this.fingerDownTime = DateTime.Now;
                }
                if (touch.phase == TouchPhase.Ended) {
                    this.fingerDown = touch.position;
                    this.fingerUpTime = DateTime.Now;
                    navBarScreenAreaCorners = GetUiWindowFourCornersPositionInScreenSpace();
                    this.CheckSwipe();
                }
            }
        }
    }

    private void CheckSwipe() {
        var console = GameObject.Find("ConsoleLogger").GetComponent<ConsoleLoggerScript>();
        string directionOfSwipe = "";
        float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
        if (duration > this.timeThreshold) return;

        float deltaX = this.fingerDown.x - this.fingerUp.x;
        if (Mathf.Abs(deltaX) > this.swipeThreshold) {
            if (deltaX > 0) {
                //TODO: Unityevent SwipeRight this.OnSwipeRight.Invoke();
                directionOfSwipe = "Right";
                console.DisplayConsoleText(directionOfSwipe, this.fingerUp.x, this.fingerDown.x, duration, deltaX, this.fingerUp.y);
                
            } else if (deltaX < 0) {
                //TODO: Unityevent SwipeLeft this.OnSwipeLeft.Invoke();
                directionOfSwipe = "Left";
                console.DisplayConsoleText(directionOfSwipe, this.fingerUp.x, this.fingerDown.x, duration, deltaX, this.fingerUp.y);
                
            }
        }

        float deltaY = fingerDown.y - fingerUp.y;
        if (Mathf.Abs(deltaY) > this.swipeThreshold) {
            if (deltaY > 0) {
                //TODO: Unityevent SwipeUp this.OnSwipeUp.Invoke();
                directionOfSwipe = "Up";
                console.DisplayConsoleText(directionOfSwipe, this.fingerUp.y, this.fingerDown.y, duration, deltaY);
                
            } else if (deltaY < 0) {
                ////TODO: Unityevent SwipeDown this.OnSwipeDown.Invoke();
                directionOfSwipe = "Down";
                console.DisplayConsoleText(directionOfSwipe, this.fingerUp.y, this.fingerDown.y, duration, deltaY);
            }
        }

        if(directionOfSwipe == "Up" || directionOfSwipe == "Down") {
            if (CheckIfSwipeBeginInNavbarArea(directionOfSwipe, this.fingerUp.y, 0, navBarScreenAreaCorners)) {
                Debug.LogWarning(GetUserActionBasedOnSwipeDirectionInValidArea(directionOfSwipe));
            }
        }  
        if(directionOfSwipe == "Right" || directionOfSwipe == "Left"){
            if (CheckIfSwipeBeginInNavbarArea(directionOfSwipe, this.fingerUp.x, this.fingerUp.y, navBarScreenAreaCorners)) {
                Debug.LogWarning(GetUserActionBasedOnSwipeDirectionInValidArea(directionOfSwipe));
            }
        }

        this.fingerUp = this.fingerDown;
    }
    private Vector3[] GetUiWindowFourCornersPositionInScreenSpace() {
        //TODO: is possible to make this a universal function and using it to getting ui positions
        navBarScreenAreaCorners = new Vector3[4];
        myMainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        var rectTransform = this.GetComponent<RectTransform>();
        navBarWorldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(navBarWorldCorners); // pass out data to navBarAreaCorners 4 coners of this UI

        for (var i = 0; i < 4; i++) {
            navBarScreenAreaCorners[i] = myMainCamera.WorldToScreenPoint(navBarWorldCorners[i]);
        }
        return navBarScreenAreaCorners;
    }
    public bool CheckIfSwipeBeginInNavbarArea(string direction, float startPosition, float fingerUpY, Vector3[] windowCorners) {
        if (direction == "Down" || direction == "Up") { // szerokosc od ~17 do 1062px

            if (startPosition >= windowCorners[0].y && startPosition <= windowCorners[1].y) {
                return true;
            }
        }
        if (direction == "Right" || direction == "Left") {
            if (startPosition >= windowCorners[0].x && startPosition <= windowCorners[2].x) { // szerokosc od ~17 do 1062px
                if (fingerUpY >= windowCorners[0].y && fingerUpY <= windowCorners[2].y) { // wysokosc od ~940 do 1100px
                    return true;
                }
            }
        }
        return false;
    }
    private string GetUserActionBasedOnSwipeDirectionInValidArea(string direction) {
        var navbar = this.GetComponent<NavigationTabsScript>();
        bool isWindowOpen = navbar.IsWindowOpen; 

        if (isWindowOpen) {
            if (direction == "Right" || direction == "Left") {
                return "Swipe " + direction;
            } 
            else if (direction == "Down") {
                navbar.IsWindowOpen = false;
                return "Hide window";
            }
        } 
        else {
            if (direction == "Right" || direction == "Left") {
                return "Swipe " + direction;
            } 
            else if (direction == "Up") {
                navbar.IsWindowOpen = true;
                return "Open window";
            }
        }
        return "";
    }
}