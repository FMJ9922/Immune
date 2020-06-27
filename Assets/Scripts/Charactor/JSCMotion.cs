using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSCMotion : EnemyMotion
{
    public Sprite[] forwardSprite;
    public Sprite[] turnSprite;
    //整只长度
    public int length = 6;
    //目前部位的下表
    public int place = 0;
    public JSCMotion[] bodys;

    int curWayPoint;
    int frameP = 0;
    int indexN = 0;
    private SpriteRenderer spriteRenderer;

    private JSC jsc;
    private struct JSC
    {
        public Part part { get; set; }
        public Turn turn { get; set; }
        public State state { get; set; }
    }

    private enum Part
    {
        head,
        body,
        tail,

    }
    private enum Turn
    {
        forward,
        left,
        right,
    }
    private enum State
    {
        start,
        end,
    }

    void Start()
    {
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        OnEnemyEscape += LevelManager.Instance.OnScoreEvent;
        speed = originSpeed;
        enemyStatus = EnemyStatus.Idle;
        agent = transform.GetComponent<AStarAgent>();
        InitWayPoint();
        transform.position = wayPointList[0];
        curWayPoint = 0;

        ControlManager.OnPlantCell += ChangeWayPoint;
    }
    private void OnDestroy()
    {
        ControlManager.OnPlantCell -= ChangeWayPoint;
        OnEnemyEscape -= LevelManager.Instance.OnScoreEvent;
        StopAllCoroutines();
    }
    void InitWayPoint()
    {
        agent.FindPathWithStartAndEndPos(startPos, endPos, FindPathType, out bool success);
        if (success)
        {
            agent.SetPath(startPos, endPos);
            wayPointList = agent.wayPointList;
        }


    }
    void ChangeWayPoint()
    {
        //Debug.Log(transform.position.x+","+ transform.position.y);
        agent.FindPathWithStartAndEndPos(new Vector2(transform.position.x, transform.position.y), endPos, FindPathType, out bool success);
        if (success)
        {
            //Debug.Log("success");
            agent.SetPath(transform.position, endPos);
            List<Vector3> tempList = agent.wayPointList;
            tempList.Remove(tempList[0]);
            wayPointList = tempList;
            indexN = 0;
        }
        else
        {
            ControlManager.Instance.PathAvaliable = false;
        }

    }
    void FixedUpdate()//每一帧执行方法
    {

        if (transform.GetComponent<EnemyHealth>().Hp > 0 && enemyStatus == EnemyStatus.Idle)
        {
            PlayAnim();
        }

    }

    private void PlayAnim()
    {
        
        frameP++;
        if (frameP > 2)
        {
            frameP = 0;
            if (indexN > 9)
            {
                indexN = 0;
                MoveNext();
                spriteRenderer.sprite = GetSprite(this.indexN);
                return;
            }
            else
            {
                spriteRenderer.sprite = GetSprite(this.indexN);
                indexN++;
            }

        }
    }

    private void MoveNext()
    {
        ChangeEnum();

        if(curWayPoint == 0)
        {
            //Debug.Log("CreateNewBody2");
            GenerateNewBody();
        }
        curWayPoint++;
        if (curWayPoint < wayPointList.Count)
        {
            transform.position = wayPointList[curWayPoint];

        }
        else
        {
            ReachEnd();
        }

    }
    private void GenerateNewBody()
    {
        //Debug.Log("CreateNewBody1");
        if (place < length - 1)
        {
            GameObject newBody = Instantiate(this.gameObject, transform.parent);
            JSCMotion newJsc = newBody.GetComponent<JSCMotion>();
            newJsc.jsc = GetNewEnum(this.jsc);
            newJsc.place = this.place + 1;
            newJsc.transform.position = this.transform.position;
            newBody.name = "Body" + newJsc.place;
            Debug.Log(jsc.part.ToString() + jsc.state.ToString());
            Debug.Log(newBody.name+":"+ newJsc.jsc.part.ToString()+ newJsc.jsc.state.ToString());
        }
        
    }
    private Turn switchTurn(Vector3 from, Vector3 to)
    {
        float angle0 = Vector3.Angle(from, Vector3.down)-90;
        float dir = Vector3.Cross(from.normalized, to.normalized).z;
       
        if (dir==0)
        {
            spriteRenderer.flipY = false;
            this.transform.eulerAngles = new Vector3(0, 0, angle0);
            return Turn.forward;
        }
        if (dir < 0)
        {
            //Debug.Log("right");
            spriteRenderer.flipY = true;
            this.transform.eulerAngles = new Vector3(0, 0, angle0);
            return Turn.right;
        }
        if (dir > 0)
        {
            //Debug.Log("left");
            spriteRenderer.flipY = false;
            this.transform.eulerAngles = new Vector3(0, 0, angle0);
            return Turn.left;
        }
        return Turn.forward;
    }
    private void ChangeEnum()
    {
        //Debug.Log(curWayPoint);
        int lastWayPoint = wayPointList.Count - 1;
        if (curWayPoint < lastWayPoint - 1)
        {
            int nextWayPoint = curWayPoint + 1;
            int nextnextWayPoint = curWayPoint + 2;
            jsc.turn = switchTurn(wayPointList[nextWayPoint] - wayPointList[curWayPoint],
                                  wayPointList[nextnextWayPoint] - wayPointList[nextWayPoint]);
            
        }
        else
        {
            jsc.turn = Turn.forward;
        }

        /*if (jsc.state == State.start)
        {
            jsc.state = State.end;
            return;
        }
        else if (jsc.part != Part.tail)//如果没有到尾部
        {
            jsc.state = State.start;
            if (place == 0 || place == length - 2)//如果part需要改变
            {
                jsc.part = (Part)((int)jsc.part + 1);
            }
        }
        else
        {
            End();
        }*/

    }
    private JSC GetNewEnum(JSC origin)
    {
        JSC temp = origin;
        temp.turn = Turn.forward;
        if (origin.state == State.start)
        {
            temp.state = State.end;
            return temp;
        }
        else if (origin.part != Part.tail)//如果没有到尾部
        {
            temp.state = State.start;
            Debug.Log(place);
            if (place == 1 || place == this.length - 3)//如果part需要改变
            {
                temp.part = (Part)((int)origin.part + 1);
            }
        }
        return temp;
    }

    /// <summary>
    /// todo 到了尾部销毁
    /// </summary>
    private void ReachEnd()
    {
        
    }

    private Sprite GetSprite(int index)
    {
        int startIndex = 0;
        Sprite[] whatSprites;
        whatSprites = jsc.turn == Turn.forward ? forwardSprite : turnSprite;
        switch (jsc.part)
        {
            case Part.head:
                {
                    startIndex = 0;
                    break;
                }
            case Part.body:
                {
                    startIndex = 20;
                    break;
                }
            case Part.tail:
                {
                    startIndex = 40;
                    break;
                }
        }
        switch (jsc.state)
        {
            case State.end:
                {
                    startIndex += 10;
                    break;
                }
            default:
                {
                    break;
                }
        }
        int curIndex = startIndex + index;
        //Debug.Log(curIndex);
        return whatSprites[curIndex];
    }
}
