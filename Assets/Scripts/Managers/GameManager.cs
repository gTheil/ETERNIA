using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() {
        instance = this;
    }

    // References
    public Player player;
    public FloatingTextManager floatingTextManager;
    public UIManager uiManager;

    // Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.ShowText(msg, fontSize, color, position, motion, duration);
    }

    public void UpdateDebugUI(string txt) {
        uiManager.UpdateDebugUI(txt);
    }

    public void UpdateHealthBar(float hitPoint, float hitPointMax) {
        uiManager.UpdateHealthBar(hitPoint, hitPointMax);
    }

    public void UpdateBlockBar(float blockPoint, float blockPointMax) {
        uiManager.UpdateBlockBar(blockPoint, blockPointMax);
    }
}
