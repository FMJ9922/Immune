using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAgent : MonoBehaviour
{
    //public LevelManager levelManager;

    public List<AStarNode> nodePath;
    public List<Vector3> wayPointList;
   
    
    void Start()
    {
        //levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
       
    }

    public int GetDistance(AStarNode startNode, AStarNode endNode)
    {
        int deltaX = Mathf.Abs(startNode.posX - endNode.posX);
        int deltay = Mathf.Abs(startNode.posY - endNode.posY);
        return deltaX + deltay;
    }

    public void FindPathWithStartAndEndPos(Vector2 startPos,Vector2 endPos,FindPathType type,out bool success)
    {
        //Debug.Log(LevelManager.Instance.name);
        AStarNode startNode = LevelManager.Instance.GetNodeByPos(startPos);
        AStarNode endNode = LevelManager.Instance.GetNodeByPos(endPos);
        List<AStarNode> openList = new List<AStarNode>();
        List<AStarNode> closeList = new List<AStarNode>();
        openList.Add(startNode);
        success = false;
        //Debug.Log(">>start");
        int n = 300;
        while (n>0) 
        {
            n--;
            if (openList.Count <= 0)
            {
                //Debug.LogError("路径被阻挡！");
                return;
            }
            AStarNode curNode = openList[0];
            for (int i = 0; i < openList.Count; i++)//查找一个costF最小的结点作为当前结点
            {
                if (openList[i].CostF < curNode.CostF)
                {

                    curNode = openList[i];
                }
            }
            openList.Remove(curNode);
            closeList.Add(curNode);
            if (curNode == endNode)
            {
                //Debug.Log(curNode.name+">>end");
                //SetPath(startPos,endPos);
                success = true;
                return;
            }
            List<AStarNode> nearByNodes = LevelManager.Instance.GetAroundNodes(curNode);
            foreach(AStarNode nearByNode in nearByNodes)
            {
                if (closeList.Contains(nearByNode)) continue;
                if (type == FindPathType.Over) continue;
                else if (type == FindPathType.Through && nearByNode.tileType == TileType.Occupy) continue;
                else if (type == FindPathType.Defult && nearByNode.tileType != TileType.Empty) continue;
                if (!openList.Contains(nearByNode))
                {
                    nearByNode.costG = curNode.costG + 1;
                    nearByNode.costH = GetDistance(nearByNode, endNode);
                    nearByNode.parentNode = curNode;
                    //Debug.Log(nearByNode.name + nearByNode.CostF + "=>" + curNode.name + curNode.CostF);
                    Debug.DrawLine(nearByNode.pos, curNode.pos, Color.red, 1f);
                    openList.Add(nearByNode);
                }
                else 
                {
                    int costG = curNode.costG + 1;
                    if (costG < nearByNode.costG)
                    {
                        nearByNode.costG = costG;
                        nearByNode.parentNode = curNode;
                        //Debug.Log(nearByNode.name + nearByNode.CostF + "=>" + curNode.name + curNode.CostF);
                        Debug.DrawLine(nearByNode.pos, curNode.pos,Color.red,1f);
                    }
                }
            }
        }
        Debug.LogError("超过遍历次数上限！");
    }
    /*public void Node()
    {
        AStarNode startNode = LevelManager.GetNodeByPos(new Vector2(15,7));
        AStarNode endNode = LevelManager.GetNodeByPos(new Vector2(0,3));
        if (node != startNode)
        {
            Debug.Log(node.name);
            node = node.parentNode;
            //Node();
        }
    }*/

    public void SetPath(Vector2 startPos, Vector2 endPos)
    {
        nodePath = new List<AStarNode>();
        wayPointList = new List<Vector3>();
        AStarNode startNode = LevelManager.Instance.GetNodeByPos(startPos);
        AStarNode endNode = LevelManager.Instance.GetNodeByPos(endPos);
        if (endNode != null)
        {
            AStarNode temp = endNode;
            AStarNode lastNode = endNode;
            wayPointList.Add(new Vector3(2*temp.pos.x-temp.parentNode.pos.x, 2*temp.pos.y- temp.parentNode.pos.y, 0));
            while (temp != startNode)
            {
                nodePath.Add(temp);
                wayPointList.Add(new Vector3(temp.pos.x,temp.pos.y,0));
                //Debug.Log(temp.name);
                lastNode = temp;
                temp = temp.parentNode;
                
            }
            wayPointList.Add(new Vector3(2 * temp.pos.x - lastNode.pos.x, 2 * temp.pos.y - lastNode.pos.y, 0));
            nodePath.Reverse();
            wayPointList.Reverse();
        }
    }
}
