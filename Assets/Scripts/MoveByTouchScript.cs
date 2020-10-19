using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTouchScript : MonoBehaviour
{
    private Touch touch;
    private float speedModifier;
    [SerializeField] public bool isTouchingDetectEnabled;

    void Start()
    {
        isTouchingDetectEnabled = false;
        speedModifier = 0.005f;
    }
    private Vector2 fingerDown;

    void Update() {
        if (isTouchingDetectEnabled == true) {
            if (Input.touchCount > 0) {
                touch = Input.GetTouch(0);
                    
                if (touch.phase == TouchPhase.Began) {
                    fingerDown = touch.position;
                    }

                if (!CheckIfStartTouchingIsInsideNavbar(fingerDown)) {
                    if (touch.phase == TouchPhase.Moved) {
                        transform.position = new Vector3(
                            transform.position.x + touch.deltaPosition.x * speedModifier,
                            transform.position.y + touch.deltaPosition.y * speedModifier,
                            transform.position.z);

                        this.transform.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    } else {
                        this.transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    }
                }
            }
        }
    }

    private bool CheckIfStartTouchingIsInsideNavbar(Vector2 startPosition) {
        // TODO: with this function , its blocking player movement if start touching inside navbar but swipe outside still make player move?
        Vector3[] navBarScreenAreaCorners = GameObject.Find("GameOptionTabs").GetComponent<TouchSwipingScipt>().navBarScreenAreaCorners;
        float x1111 = navBarScreenAreaCorners[0].x;
        float x2222 = navBarScreenAreaCorners[2].x;
        float y1111 = navBarScreenAreaCorners[0].y;
        float y2222 = navBarScreenAreaCorners[1].y;
        if (startPosition.x > x1111 && startPosition.x < x2222) {
            if (startPosition.y > y1111 && startPosition.y < y2222) {
                Debug.Log("true");
                return true;
            }
        }
        Debug.Log("false");
        return false;
    }
}
