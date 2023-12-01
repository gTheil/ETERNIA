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
        tilemapSize = tilemap.GetComponent<Renderer>().bounds.size;

        coefficientX = tilemapSize.x / mapImage.rectTransform.rect.width;
        coefficientY = tilemapSize.y / mapImage.rectTransform.rect.height;

        /*
        Debug.Log("Scene Center: " + b.center);
        Debug.Log("Scene Extents X: " + b.extents.x);
        Debug.Log("Scene Extents Y: " + b.extents.y);
        Debug.Log("Scene minX: " + b.min.x);
        Debug.Log("Scene maxX: " + b.max.x);
        Debug.Log("Scene minY: " + b.min.y);
        Debug.Log("Scene maxY: " + b.max.y);
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            if (!mapMenu.activeSelf)
                mapMenu.SetActive(true);
            else
                mapMenu.SetActive(false);
        }

        playerImage.rectTransform.anchoredPosition = new Vector3(GameManager.instance.GetPlayer().transform.position.x / coefficientX, GameManager.instance.GetPlayer().transform.position.y / coefficientY, 0);
    }
}
