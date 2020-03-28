using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMotion : MonoBehaviour
{

    public PlaceType placeType = PlaceType.Null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) placeType = PlaceType.Null;
        if (placeType == PlaceType.Selceted)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
