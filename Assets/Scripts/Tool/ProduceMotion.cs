using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceMotion : MonoBehaviour
{
    public SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        StartCoroutine(DoFade( 0, 0.5f, 2f));
    }

    
    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0.002f, 0);
    }
    public IEnumerator DoFade( float alpha, float time, float wait)
    {
        float dur = 0;
        float startAlpha = sp.color.a;
        yield return new WaitForSeconds(wait);
        while (dur < time)
        {
            dur += 0.0166667f;
            //Debug.Log(alpha);
            alpha = 1 - dur / time;
            sp.color = new Color(sp.color.r,
                                    sp.color.g,
                                    sp.color.b,
                                    alpha);
            //Debug.Log(alpha);
            yield return new WaitForFixedUpdate();
        }
        Destroy(this.gameObject);
    }
}
