using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject tilemap;

    // Start is called before the first frame update
    void Start()
    {
        var b = tilemap.GetComponent<Renderer>().bounds;

        Debug.Log("Scene Center: " + b.center);
        Debug.Log("Scene Extents X: " + b.extents.x);
        Debug.Log("Scene Extents Y: " + b.extents.y);
        Debug.Log("Scene minX: " + b.min.x);
        Debug.Log("Scene maxX: " + b.max.x);
        Debug.Log("Scene minY: " + b.min.y);
        Debug.Log("Scene maxY: " + b.max.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
