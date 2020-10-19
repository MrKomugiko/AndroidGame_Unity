using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationTabsScript : MonoBehaviour
{

    [SerializeField] GameObject heroEquipmentWindow;
    [SerializeField] GameObject gamePauseWindow;

    [SerializeField] private bool _isWindowOpen;
    [SerializeField] private float closePosition;
    [SerializeField] private float openPosition;

    public bool IsWindowOpen {
        get => _isWindowOpen;
        set {
            _isWindowOpen = value;
            if (_isWindowOpen) {
                Debug.Log("Navbar is [ OPEN ]");
                gamePauseWindow.SetActive(true);
                this.GetComponent<RectTransform>().localPosition = new Vector3(0, openPosition, 0);
                GameObject.Find("TouchingManager").GetComponent<TouchingManagerScript>().UpdateTouchingBehavior();

            } else {
                Debug.Log("Navbar is [ CLOSED ]");
               gamePauseWindow.SetActive(false);
                this.GetComponent<RectTransform>().localPosition = new Vector3 (0,closePosition,0);
                GameObject.Find("TouchingManager").GetComponent<TouchingManagerScript>().UpdateTouchingBehavior();
            }
        }
    }

    public Canvas CanvasUIElement;//Set in editor
    private float CanvasWidth;
    private float CanvasHeight;
    // Start is called before the first frame update
    void Start()
    {
        _isWindowOpen = true;
        openPosition = this.GetComponent<RectTransform>().localPosition.y;

           CanvasWidth = CanvasUIElement.GetComponent<RectTransform>().rect.width;
        CanvasHeight = CanvasUIElement.GetComponent<RectTransform>().rect.height;
        Debug.LogFormat("Canvas width:{0} canvas height:{1} ", CanvasWidth, CanvasHeight);
        closePosition = -(CanvasHeight / 2) + (this.GetComponent<RectTransform>().rect.height / 2);
        Debug.Log(closePosition);
}

    // Update is called once per frame
    void Update()
    {
    }

    public void MinimalizeOrMaximalizeWindow() {
        _isWindowOpen = !_isWindowOpen; 
    }
    public void OnClick_OpenHeroEquipmentWindow() {
        heroEquipmentWindow.SetActive(true);
        heroEquipmentWindow.GetComponent<HeroDetailPanelScript>().NeedUpdate = true;
        GameObject.Find("TouchingManager").GetComponent<TouchingManagerScript>().UpdateTouchingBehavior();

    }
}
