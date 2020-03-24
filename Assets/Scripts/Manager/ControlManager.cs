using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public GameObject Cell;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonSlecet()
    {
        GameObject obj = Instantiate(Cell, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        obj.GetComponent<CellMotion>().placeType = PlaceType.Selceted;
    }
}
