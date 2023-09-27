using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update() {
        foreach(FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;
        
        floatingText.gameObject.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }

    private FloatingText GetFloatingText() {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null) {
            txt = new FloatingText();
            txt.gameObject = Instantiate(textPrefab);
            txt.gameObject.transform.SetParent(textContainer.transform);
            txt.txt = txt.gameObject.GetComponent<TMP_Text>();

            floatingTexts.Add(txt);
        }

        return txt;
    }
}
