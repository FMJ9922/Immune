using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMotion : MonoBehaviour
{
    private int index = 0;
    private float speed = 2;
    public Vector3[] wayPoints;
    public Sprite[] IdleSprite;
    public Sprite[] ExplosionSprite;
    protected ArrayList enemyInRange;
    private SpriteRenderer spriteRenderer;
    int frameP = 0;
    int indexP = 0;
    public float damage;
    bool explosion;

    void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        explosion = false;
        enemyInRange = new ArrayList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(wayPoints != null);
        if(explosion)
        {
            PlayAnimation(ExplosionSprite, 2, PlayAnimaType.Once);
        }
        else
        {
            PlayAnimation(IdleSprite, 2, PlayAnimaType.Loop);
            if (wayPoints != null)
            {
                Move(wayPoints);
            }
        }
        
        
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            if (enemyHealth.Hp > 0)
            {
                enemyInRange.Add(collision.transform);
                enemyHealth.cellInRange.Add(this.transform);
                enemyHealth.OnEnemyDie += OnInRangeEnemyDie;
                //Debug.Log(collision.transform.name + "加入队列");
            }
            
        }

    }
    protected void OnInRangeEnemyDie(Transform enemyTrans)
    {
        enemyTrans.GetComponent<EnemyHealth>().OnEnemyDie -= OnInRangeEnemyDie;
        if (enemyInRange.Contains(enemyTrans))
        {
            enemyInRange.Remove(enemyTrans);
            //Debug.Log(enemyTrans.name + "因死亡离开队列");
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.transform.GetComponent<EnemyHealth>();
            if (enemyHealth.Hp > 0)
            {
                enemyInRange.Remove(collision.transform);
                enemyHealth.cellInRange.Remove(this.transform);
                enemyHealth.OnEnemyDie -= OnInRangeEnemyDie;
                //Debug.Log(collision.transform.name + "因超出射程离开队列");
            }
        }
    }
    public void PlayAnimation(Sprite[] sprites, int deltaFrame, PlayAnimaType type)
    {
        frameP++;
        if (frameP >= deltaFrame)
        {
            frameP = 0;
            indexP++;
            if (indexP >= sprites.Length - 1)
            {
                indexP = 0;
                if (type == PlayAnimaType.Once)
                {
                    index = 0;
                    indexP = 0;
                    frameP = 0;
                    explosion = false;
                    transform.localScale /= 2;
                    transform.GetComponent<CircleCollider2D>().radius *= 2;
                    transform.position += new Vector3(0, 0.4f);
                    //SetDamageToEnemys();
                    gameObject.SetActive(false);
                    return;
                }
            }
            spriteRenderer.sprite = sprites[indexP];
        }
        else return;

    }
    private void Move(Vector3[] wayPointList)
    {
        if (wayPointList.Length == 0)
        {
            Destroy(gameObject);
        }
        if (index >= wayPointList.Length) return;
        transform.Translate((wayPointList[index] - transform.position).normalized * Time.deltaTime * speed);//移动，节点到当前位置的向量差的单位差*完成上一帧的时间*速度
        if (Vector3.Distance(wayPointList[index], transform.position) < 0.03f)//三维坐标，距离（节点，当前位置）小于0.2f的时候执行
        {
            index++;//增加索引，也就获取到下个节点坐标
            if (index > wayPointList.Length - 1)//如果大于最后一个节点时执行
            {
                explosion = true;
                wayPoints = null;
                indexP = 0;
                frameP = 0;
                spriteRenderer.sprite = null;
                transform.localScale *= 2;
                transform.GetComponent<CircleCollider2D>().radius /= 2;
                transform.position -= new Vector3(0, 0.4f);
                Invoke("SetDamageToEnemys", 0.7f);
                SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_bomb);
            }
        }
    }
    public void SetDamageToEnemys()
    {
        
        for (int i = 0;i<enemyInRange.Count;i++) 
        {
            
            Transform trans = (Transform)enemyInRange[i];
            //Debug.Log(trans.name);
            if (trans != null&&trans.GetComponent<EnemyHealth>().Hp>0)
            {
                trans.GetComponent<EnemyHealth>().TakeDamage(damage, false);
                trans.GetComponent<EnemyMotion>().GetSlowDown(0.5f, 7f);
               
              
                
            }
        }
    }
}
