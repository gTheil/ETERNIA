using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject mapMenu;
    public GameObject tilemap;
    public Image mapImage;
    public Image playerImage;

    private Vector3 tilemapSize;
    private float coefficientX, coefficientY;

    // Start is called before the first frame update
    void Start()
    {
        if (tilemap != null && mapImage != null) {
            tilemapSize = tilemap.GetComponent<Renderer>().bounds.size;
            coefficientX = tilemapSize.x / mapImage.rectTransform.rect.width;
            coefficientY = tilemapSize.y / mapImage.rectTransform.rect.height;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Map") && !GameManager.instance.IsUIOpen() && !GameManager.instance.IsDialogueActive()) {
            if (!mapMenu.activeSelf) {
                mapMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else {
                mapMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

        if (Input.GetButtonDown("Cancel")) {
            if (mapMenu.activeSelf) {
                mapMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

        if (GameManager.instance.GetPlayer() != null)
            playerImage.rectTransform.anchoredPosition = new Vector3(GameManager.instance.GetPlayer().transform.position.x / coefficientX, GameManager.instance.GetPlayer().transform.position.y / coefficientY, 0);
    }
}
