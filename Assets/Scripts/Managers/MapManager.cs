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

    public AudioSource openAudio;

    private Vector3 tilemapSize;
    private Bounds tilemapBounds;
    private float coefficientX, coefficientY;

    // Start is called before the first frame update
    void Start()
    {
        if (tilemap != null && mapImage != null) {
            tilemapBounds = tilemap.GetComponent<Renderer>().bounds;
            tilemapSize = tilemap.GetComponent<Renderer>().bounds.size;
            coefficientX = ((tilemapSize.x * 0.9f) / mapImage.rectTransform.rect.width);
            coefficientY = ((tilemapSize.y * 0.9f) / mapImage.rectTransform.rect.height);

            Debug.Log("Scene Center: " + tilemapBounds.center);
            Debug.Log("Scene Extents X: " + tilemapBounds.extents.x);
            Debug.Log("Scene Extents Y: " + tilemapBounds.extents.y);

            Debug.Log("Scene minX: " + tilemapBounds.min.x);
            Debug.Log("Scene maxX: " + tilemapBounds.max.x);
            Debug.Log("Scene minY: " + tilemapBounds.min.y);
            Debug.Log("Scene maxY: " + tilemapBounds.max.y);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Map") && !GameManager.instance.IsUIOpen() && !GameManager.instance.IsDialogueActive()) {
            if (!mapMenu.activeSelf) {
                mapMenu.SetActive(true);
                Time.timeScale = 0;
                openAudio.Play();
            }
            else {
                mapMenu.SetActive(false);
                Time.timeScale = 1;
                openAudio.Play();
            }
        }

        if (Input.GetButtonDown("Cancel")) {
            if (mapMenu.activeSelf) {
                mapMenu.SetActive(false);
                Time.timeScale = 1;
                openAudio.Play();
            }
        }

        if (GameManager.instance.GetPlayer() != null)
            playerImage.rectTransform.anchoredPosition = new Vector3(GameManager.instance.GetPlayer().transform.position.x / coefficientX, GameManager.instance.GetPlayer().transform.position.y / coefficientY, 0);
    }
}
