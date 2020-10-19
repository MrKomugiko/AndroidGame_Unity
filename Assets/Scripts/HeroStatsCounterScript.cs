using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroStatsCounterScript : MonoBehaviour
{
    [SerializeField] GameObject playerObject; 
    private Player playerInstance;
    [SerializeField] private float countersBarsWidth;
    [SerializeField] private GameObject counterBar;

    [SerializeField] private int playerLevel;
    [SerializeField] TextMeshProUGUI levelText;


    [SerializeField] private float currentHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float hpPErcentageFill;
    [SerializeField] GameObject HPBar;

    [SerializeField] private float currentMana;
    [SerializeField] private float maxMana;
    [SerializeField] private float manaPercentageFill;
    [SerializeField] GameObject ManaBar;


    [SerializeField] private float currentExp;
    [SerializeField] private float maxExp;
    [SerializeField] private float expPercentageFill;
    [SerializeField] GameObject ExpBar;


    void Start()
    {
        countersBarsWidth = counterBar.GetComponent<RectTransform>().rect.width;
        playerInstance = playerObject.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        playerLevel = playerInstance.Level;
        levelText.SetText(playerLevel.ToString());
    
        // Setting HP Bar
        maxHp = playerInstance.MaxPlayerHealth();
        currentHp = playerInstance.Health;
        hpPErcentageFill = (100 * currentHp) / maxHp;
        HPBar.GetComponent<RectTransform>().sizeDelta= new Vector2(countersBarsWidth*(hpPErcentageFill/100), 100);

        // Setting MANA Bar TODO: Player dont have mana/stamina wchich should be displayed on UI
        manaPercentageFill = 0;
        ManaBar.GetComponent<RectTransform>().sizeDelta = new Vector2((countersBarsWidth * manaPercentageFill/100), 100);

        // Setting EXP Bar
        maxExp = playerInstance.MaxPlayerExperienceAtLevel(playerLevel);
        currentExp = playerInstance.Experience;
        expPercentageFill = (100 * currentExp) / maxExp;
        ExpBar.GetComponent<RectTransform>().sizeDelta = new Vector2((countersBarsWidth * expPercentageFill / 100), 100);
    }

}
