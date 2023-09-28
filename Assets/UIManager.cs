using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text hitsText;
    public TMP_Text damageText;
    public TMP_Text dodgeText;
    public TMP_Text blockText;

    public Image healthBar;
    public Image blockBar;

    private int hits;
    private int hitsTaken;
    private int hitsDodged;
    private int hitsBlocked;

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
}
