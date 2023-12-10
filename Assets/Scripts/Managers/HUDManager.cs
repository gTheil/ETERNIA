using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public TMP_Text hitsText;
    public TMP_Text damageText;
    public TMP_Text dodgeText;
    public TMP_Text blockText;

    public TMP_Text healthPointText;
    public TMP_Text blockPointText;
    public TMP_Text swordAtkText;
    public TMP_Text bowAtkText;
    public TMP_Text shieldDefText;

    public Image healthBar;
    public Image blockBar;

    public TMP_Text goldText;

    private int hits;
    private int hitsTaken;
    private int hitsDodged;
    private int hitsBlocked;

    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("HUD");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if (goldText != null)
            goldText.text = "Gold: " + GameManager.instance.GetPlayer().GetComponent<Player>().gold;
    }

    public void UpdateDebugUI(string txt) {
        switch (txt) {
            case "hit":
                hits++;
                hitsText.text = "Golpes Acertados: " + hits;
                break;
            case "damage":
                hitsTaken++;
                damageText.text = "Golpes Sofridos: " + hitsTaken;
                break;
            case "dodge":
                hitsDodged++;
                dodgeText.text = "Golpes Desviados: " + hitsDodged;
                break;
            case "block":
                hitsBlocked++;
                blockText.text = "Golpes Bloqueados: " + hitsBlocked;
                break;
            default:
                break;
        } 
    }

    public void UpdateHealthBar(float hitPoint, float hitPointMax) {
        healthBar.fillAmount = hitPoint / hitPointMax;
    }

    public void UpdateBlockBar(float blockPoint, float blockPointMax) {
        blockBar.fillAmount = blockPoint / blockPointMax;
    }

    public void UpdateStatsUI(float hitPoint, float hitPointMax, float blockPoint, float blockPointMax, int swordAtk, int bowAtk, int shieldDef) {
        healthPointText.text = hitPoint + "/" + hitPointMax;
        blockPointText.text = Mathf.Round(blockPoint) + "/" + blockPointMax;
        swordAtkText.text = swordAtk.ToString();
        bowAtkText.text = bowAtk.ToString();
        shieldDefText.text = shieldDef.ToString();
    }

    public void UpdateSingleStat(string statName, float statValue, float statMax) {
        switch (statName) {
            case "block":
                blockPointText.text = Mathf.Round(statValue) + "/" + statMax;
                break;
            default:
                break;
        }
    }
}
