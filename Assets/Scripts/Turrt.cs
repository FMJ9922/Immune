using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrt : MonoBehaviour
{
    public List<GameObject> enemys = new List<GameObject>();
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemys.Add(collision.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemys.Remove(collision.gameObject);
        }
    }
}
