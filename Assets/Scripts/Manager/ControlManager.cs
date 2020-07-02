using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ControlManager : MonoBehaviour
{
    private static ControlManager instance = null;
    public static ControlManager Instance
    {
        get
        {
            return instance;
        }
    }
    private AStarNode targetNode = null;
    public GameObject CellOnMovePfb;
    private GameObject CellOnMove;
    private PlaceType placeType = PlaceType.Null;
    private CellType cellType;
    public bool PathAvaliable;
    public bool isSelect;

    public delegate void CellButtonName(CellType cellType);
    public delegate void PlantCell();
    public static event PlantCell OnPlantCell;
    public delegate void TakeAction(bool doing);
    public static event TakeAction OnMoveToPlant;
    public static event TakeAction OnClickTile;

    public Text text;

    public AStarNode lastNode = null;
    public CellBase lastCell = null;

    public Transform SelectGlow;
    public bool AllowBJ = false;
    private bool hasInitBJbtn = false;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        CellButtonName OnClick = new CellButtonName(OnPlantButtonSelect);
        OnClick += OnPlantButtonSelect;
        PathAvaliable = true;
        isSelect = false;
    }
    public void OnPlantButtonDown()
    {
        ControlManager.Instance.isSelect = true;
        if (OnMoveToPlant != null)
        {
            OnMoveToPlant(true);
        }
    }
    public void OnPlantButtonSelect(CellType cellType)
    {
        if (placeType == PlaceType.Null && isSelect)
        {

            //Debug.Log(transform.position.x);
            CellOnMove = Instantiate(CellOnMovePfb, transform.position, Quaternion.identity, transform) as GameObject;
            string path = "Cell/" + (int)cellType + cellType.ToString() + "/" + cellType.ToString();
            CellOnMove.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
            CellOnMove.name = cellType.ToString();
            //text.text = transform.position.x+","+ transform.position.y + "," + transform.position.z ;

            placeType = PlaceType.Selected;
            this.cellType = cellType;
            isSelect = false;
            CellOnMove.GetComponentInChildren<RangePicManage>().ChangeLocalScale(JsonIO.GetCellData(cellType).atkRange * 3.3f);


            if (RightCanvasShowAndHide.Instance.regular)
            {
                RightCanvasShowAndHide.Instance.HideCanvas();
            }
        }

    }
    public void OnPlantButtonClick(Transform trans, CellType cellType)
    {
        if (!SelectGlow.gameObject.activeInHierarchy)
        {
            SelectGlow.gameObject.SetActive(true);
            RoleSelectInfo.Instance.transform.position = trans.position;
            RoleSelectInfo.Instance.transform.GetComponent<Animation>().Play("ShowCellInfo");
            RoleSelectInfo.Instance.InvalidateInfo(cellType);
            //Debug.Log("1");
        }
        else
        {
            RoleSelectInfo.Instance.ChangeInstance(trans, cellType);
            //Debug.Log("2");
        }

        SelectGlow.SetParent(trans.parent);
        SelectGlow.localPosition = new Vector3(2, 2.6F, 0);

    }
    public float GetRenderDepth(float x, float y)
    {
        return -(8.5f - y + x - 0.5f) * 0.001f;
    }
    public void HideSelectAndCellInfo()
    {
        if (SelectGlow.gameObject.activeInHierarchy)
        {
            SelectGlow.gameObject.SetActive(false);
            RoleSelectInfo.Instance.transform.GetComponent<Animation>().Play("HideCellInfo");

        }
    }
    public void OnRemoveButtonDown()
    {
        placeType = PlaceType.Remove;
        Instance.isSelect = false;

    }
    public bool CheckPathAvaliable()
    {
        bool isAvaliable = true;
        Vector4[] vectors = JsonIO.GetDefaultPath();
        AStarAgent agent = transform.GetComponent<AStarAgent>();

        for (int i = 0; i < vectors.Length; i++)
        {
            if (targetNode == null)
            {
                //LoggerManager.Instance.ShowOneLog("此处无法放置！");
                return false;
            }
            else if (targetNode == LevelManager.Instance.GetNodeByPos(new Vector2(vectors[i].x, vectors[i].y)))
            {
                LoggerManager.Instance.ShowOneLog("此处无法放置！");
                //Debug.Log("1");
                return false;
            }
            else
            {
                bool issuccess;
                agent.FindPathWithStartAndEndPos(new Vector2(vectors[i].x, vectors[i].y), new Vector2(vectors[i].z, vectors[i].w), FindPathType.Defult, out issuccess);

                if (!issuccess)
                {

                    LoggerManager.Instance.ShowOneLog("此处无法放置！");
                    //Debug.Log("2");
                    isAvaliable = false;
                }
            }


        }
        return isAvaliable;
    }
    public static bool RightPlaceToUpGrade(CellType oldcellType, CellType newcellType)
    {
        if (oldcellType == CellType.BL)
        {
            if ((int)newcellType > 10 && (int)newcellType < 13) return true;
            else return false;
        }
        else if (oldcellType == CellType.TL)
        {
            if ((int)newcellType > 6 && (int)newcellType < 10) return true;
            else return false;
        }
        else return false;
    }
    public void InitOneCell()
    {
        CellData cellData;
        cellData = JsonIO.GetCellData(cellType);
        TileType temp = targetNode.tileType;
        targetNode.tileType = TileType.Occupy;
        if (PathAvaliable && CheckPathAvaliable() && LevelManager.Instance.SpendPoints(PointsType.Deploy, cellData.initCost))
        {
            GameObject cell = Instantiate(PrefabManager.GetCellPrefab(cellType),
            new Vector3(targetNode.pos.x, targetNode.pos.y, GetRenderDepth(targetNode.pos.x, targetNode.pos.y)),
            Quaternion.identity,
            targetNode.transform);
            //Debug.Log(GetRenderDepth(targetNode.pos.x, targetNode.pos.y));
            cell.name = cellType.ToString();
            cell.GetComponent<CellBase>().gridPos = new Vector2Int(targetNode.posX, targetNode.posY);
            LevelManager.Instance.DrawDefaultRoute(LevelManager.Instance.curWave);
            LevelManager.Instance.OnScoreEvent(ScoreType.CellDeployNum, 1);
            targetNode.tileType = TileType.Occupy;

            if (AllowBJ && cellType == CellType.JX)
            {

            }
            //SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_upgrade);
            //To Do 放置成功音效
            return;
        }
        else if (!PathAvaliable || !CheckPathAvaliable())
        {
            targetNode.tileType = temp;
            LoggerManager.Instance.ShowOneLog("此处无法放置！");

        }
        else
        {
            targetNode.tileType = temp;
            LoggerManager.Instance.ShowOneLog("点数不足！");
        }
    }
    public bool CheckCanPlaceCell(CellType _cellType)
    {
        CellData cellData;
        cellData = JsonIO.GetCellData(_cellType);
        TileType temp = targetNode.tileType;
        targetNode.tileType = TileType.Occupy;
        PointsType pointsType = PointsType.Deploy;
        if ((int)_cellType < 7 || (int)_cellType == 10)
        {
            pointsType = PointsType.Deploy;
        }
        else if ((int)_cellType < 10)
        {
            pointsType = PointsType.KangYuan;
        }
        else
        {
            pointsType = PointsType.LinBa;
        }
        if (PathAvaliable && CheckPathAvaliable() && LevelManager.Instance.SpendPoints(pointsType, cellData.initCost))
        {
            return true;
        }
        else if (!PathAvaliable || !CheckPathAvaliable())
        {
            targetNode.tileType = temp;
            LoggerManager.Instance.ShowOneLog("此处无法放置！");

        }
        else
        {
            targetNode.tileType = temp;
            LoggerManager.Instance.ShowOneLog("点数不足！");
        }
        return false;
    }
    public IEnumerator DelayInitOneCell(CellType _cellType, AStarNode astarNode,float time)
    {
        yield return new WaitForSeconds(time);
        InitOneCell(_cellType, astarNode);
    }
    public void InitOneCell(CellType _cellType,AStarNode astarNode)
    {
        GameObject cell = Instantiate(PrefabManager.GetCellPrefab(cellType),
            new Vector3(astarNode.pos.x, astarNode.pos.y, GetRenderDepth(astarNode.pos.x, astarNode.pos.y)),
            Quaternion.identity,
            astarNode.transform);
        //Debug.Log(GetRenderDepth(targetNode.pos.x, targetNode.pos.y));
        cell.name = _cellType.ToString();
        cell.GetComponent<CellBase>().gridPos = new Vector2Int(astarNode.posX, astarNode.posY);
        LevelManager.Instance.DrawDefaultRoute(LevelManager.Instance.curWave);
        LevelManager.Instance.OnScoreEvent(ScoreType.CellDeployNum, 1);
        astarNode.tileType = TileType.Occupy;
        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_upgrade);
    }
    public IEnumerator WaitForTime(System.Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }
    void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButtonUp(0))
        {

#elif UNITY_IOS || UNITY_ANDROID
        if (Input.GetTouch(0).phase == TouchPhase.Ended) {
#endif

            isSelect = false;
            if (CellOnMove != null)
            {
                Destroy(CellOnMove);
                CellOnMove = null;
            }
            //Debug.Log(placeType.ToString());
            switch (placeType)
            {
                case PlaceType.Selected:
                    {
                        if (OnMoveToPlant != null)
                        {
                            OnMoveToPlant(false);
                        }
                        placeType = PlaceType.Null;
                        if (RightCanvasShowAndHide.Instance.regular)
                        {
                            RightCanvasShowAndHide.Instance.ShowCanvas();
                        }
                        if (targetNode == null)
                        {
                            return;
                        }
                        if (targetNode.tileType == TileType.Empty)
                        {

                            if (OnPlantCell != null)
                            {
                                OnPlantCell();
                            }
                            int type = (int)cellType;
                            if ((type > 6 && type < 10) || (type > 10 && type < 13))
                            {
                                LoggerManager.Instance.ShowOneLog("必须放置在前置细胞上");
                            }
                            else 
                            {
                                InitOneCell();
                            }
                        }
                        else if (targetNode.tileType == TileType.Occupy)
                        {
                            if (targetNode.transform.childCount > 0)
                            {
                                Transform oldCell = targetNode.transform.GetChild(0);
                                CellBase cellBase = oldCell.GetComponent<CellBase>();
                                if (RightPlaceToUpGrade(cellBase.cellType, cellType)&&CheckCanPlaceCell(cellType))
                                {
                                    cellBase.StartAction();
                                    if (!hasInitBJbtn)
                                    {
                                        CellButtonUI.Instance.InitOneButton(JsonIO.GetAllowCellType().Length - 1);
                                    }
                                    Debug.Log(cellType.ToString());
                                    StartCoroutine(WaitForTime(() => InitOneCell(cellType,targetNode), cellBase.GetUpgradeTime()));
                                }
                                else
                                {
                                    LoggerManager.Instance.ShowOneLog("放置在了错误的细胞上");
                                }
                            }
                            else
                            {
                                LoggerManager.Instance.ShowOneLog("BUG:此处节点状态没有归empty");
                            }
                        }
                        else
                        {
                            LoggerManager.Instance.ShowOneLog("此处被占用，无法放置！");
                        }
                        PathAvaliable = true;
                        //To Do 放置错误：路径无效音效
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return;

        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (!Input.GetMouseButton(0))//如果没有输入就返回
        {
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.tag != "RoleSelectUI")
            {
                HideSelectAndCellInfo();
            }
        }

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            LoggerManager.Instance.ShowOneLog("触碰UI");
            return;
        }


#elif UNITY_IOS || UNITY_ANDROID
    {
        if (Input.touchCount<=0)
        {
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.tag != "RoleSelectUI")
            {
                HideSelectAndCellInfo();
            }
        }
        transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        int pointerID = Input.GetTouch(0).fingerId;
        if (EventSystem.current.IsPointerOverGameObject(pointerID))
        {
                //LoggerManager.Instance.ShowOneLog("触碰UI");
            return;
        }
    }
#endif

        targetNode = LevelManager.Instance.GetNodeByPos(transform.position);
        switch (placeType)
        {
            case PlaceType.Null:
                {
                    if (lastCell != null && Vector3.Distance(transform.position, lastCell.transform.position) > 1.5F)
                    {
                        lastCell.CloseRangePic();
                        lastCell = null;
                    }
                    if (targetNode != null && targetNode.tileType == TileType.Occupy && lastCell == null)
                    {
                        lastCell = targetNode.GetComponentInChildren<CellBase>();
                        //Debug.Log(lastCell.transform.name);
                        lastCell.ShowRangePic();
                    }
                    lastNode = targetNode;
                    return;
                }
            case PlaceType.Remove:
                {
                    if (lastCell != null)
                    {
                        lastCell.CloseRangePic();
                        lastCell = null;
                    }
                    placeType = PlaceType.Null;
                    if (targetNode.tileType == TileType.Occupy)
                    {
                        string name = targetNode.transform.GetChild(0).gameObject.name;
                        Destroy(targetNode.transform.GetChild(0).gameObject);
                        SoundManager.Instance.PlaySoundEffect(SoundResource.sfx_root_out);
                        targetNode.tileType = TileType.Empty;
                        LoggerManager.Instance.ShowOneLog("已铲除" + name);
                    }
                    ShovelManager.Instance.OnCancelClose();
                    lastNode = targetNode;
                    return;
                }
            case PlaceType.Selected:
                {
                    if (lastCell != null)
                    {
                        lastCell.CloseRangePic();
                        lastCell = null;
                    }
                    if (CellOnMove != null)//如果移动的物体不为空
                    {
                        CellOnMove.transform.position = transform.position;
                        if (lastNode != targetNode)//如果移到了其它节点上
                        {
                            int type = (int)cellType;
                            if ((type > 6 && type < 10) || (type > 10 && type < 13))//如果是需要在其他细胞上放置的细胞
                            {
                                if (targetNode.transform.childCount > 0)
                                {
                                    Transform oldCell = targetNode.transform.GetChild(0);
                                    if (RightPlaceToUpGrade(oldCell.GetComponent<CellBase>().cellType, cellType))
                                    {
                                        CellOnMove.GetComponentInChildren<RangePicManage>().ChangeSpriteColor(Color.white);
                                    }
                                    else
                                    {
                                        CellOnMove.GetComponentInChildren<RangePicManage>().ChangeSpriteColor(Color.red);
                                    }
                                }
                                else
                                {
                                    CellOnMove.GetComponentInChildren<RangePicManage>().ChangeSpriteColor(Color.red);
                                }
                            }
                            else//如果是一般的细胞
                            {
                                if (CheckPathAvaliable() && targetNode.tileType == TileType.Empty)
                                {
                                    CellOnMove.GetComponentInChildren<RangePicManage>().ChangeSpriteColor(Color.white);
                                    //Debug.Log("White");
                                }
                                else
                                {
                                    CellOnMove.GetComponentInChildren<RangePicManage>().ChangeSpriteColor(Color.red);
                                    //Debug.Log("Red");
                                }
                            }
                        }
                    }

                    lastNode = targetNode;
                    return;
                }
        }



    }
}
