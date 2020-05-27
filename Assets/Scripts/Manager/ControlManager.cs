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
    public GameObject[] CellPfbs;
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
    public CellBase lastCell;

    public Transform SelectGlow;

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
        if (placeType == PlaceType.Null && isSelect&&transform.position.x<16.3f)
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
            CellOnMove.GetComponentInChildren<RangePicManage>().ChangeLocalScale(JsonIO.GetCellData(cellType).atkRange*3.3f);
        }

    }
    public void OnPlantButtonClick(Transform trans)
    {
        SelectGlow.gameObject.SetActive(true);
        SelectGlow.SetParent(trans.parent);
        SelectGlow.localPosition = new Vector3(2, 2.6F, 0);
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
                Debug.Log("null");
                return false;
            }
            else if (targetNode == LevelManager.Instance.GetNodeByPos(new Vector2(vectors[i].x, vectors[i].y)))
            {
                //Debug.Log("atInitPos");
                return false;
            }
            else
            {
                bool issuccess;
                agent.FindPathWithStartAndEndPos(new Vector2(vectors[i].x, vectors[i].y), new Vector2(vectors[i].z, vectors[i].w), FindPathType.Defult, out issuccess);

                if (!issuccess)
                {
                    isAvaliable = false;
                }
            }
               

        }
        return isAvaliable;
    }
    void Update()
    {
        /*if (PauseHandler.IsGamePause)
        {
            return;
        }*/

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButtonUp(0))
        {

#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount != 1 || Input.GetTouch(0).phase == TouchPhase.Ended) {
#endif
            //text.text = transform.position.x + "," + transform.position.y + "," + transform.position.z;
            //Debug.Log("!");
            //Debug.Log(targetNode.name);
            if (OnMoveToPlant != null)
            {
                OnMoveToPlant(false);
            }
            /*if (targetNode == null)
            {
                Debug.Log("NULL");
                placeType = PlaceType.Null;
                Destroy(CellOnMove);
                CellOnMove = null;
            }*/
            if (targetNode != null && targetNode.tileType == TileType.Empty && placeType == PlaceType.Selected)
            {
                
                targetNode.tileType = TileType.Occupy;
                if (OnPlantCell != null)
                {
                    OnPlantCell();
                }
                CellData cellData;
                cellData = JsonIO.GetCellData(cellType);
                //Debug.Log(""+PathAvaliable + CheckPathAvaliable() + LevelManager.Instance.SpendPoints(cellData.initCost));
                if (PathAvaliable && CheckPathAvaliable()&&LevelManager.Instance.SpendPoints(cellData.initCost))
                {
                    //Debug.Log("plan1");
                    GameObject cell = Instantiate(CellPfbs[(int)cellType],
                    new Vector3(targetNode.pos.x, targetNode.pos.y),
                    Quaternion.identity,
                    targetNode.transform);
                    cell.name = cellType.ToString();
                    cell.GetComponent<CellBase>().gridPos = new Vector2Int(targetNode.posX, targetNode.posY);
                    LevelManager.Instance.DrawDefaultRoute(LevelManager.Instance.curWave);

                    //To Do 放置正确音效，粒子效果
                }
                else
                {
                    targetNode.tileType = TileType.Empty;
                    PathAvaliable = true;
                    //To Do 放置错误：路径无效音效
                }
            }

            if (placeType == PlaceType.Selected)
            {
                placeType = PlaceType.Null;
                Destroy(CellOnMove);
                CellOnMove = null;
            }
            return;
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (!Input.GetMouseButton(0))
        {
            return;
        }


        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/


#elif UNITY_IOS || UNITY_ANDROID
        {
        transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        int pointerID = Input.GetTouch(0).fingerId;
        }
#endif
        //if (EventSystem.current.IsPointerOverGameObject(pointerID)) {
        //Debug.Log("touch ui");
        //return;
        if (CellOnMove != null)
        {
            CellOnMove.transform.position = transform.position;
        }

        targetNode = LevelManager.Instance.GetNodeByPos(transform.position);
       
        if (placeType ==PlaceType.Remove)//如果是铲除
        {
            placeType = PlaceType.Null;
            if (targetNode.tileType == TileType.Occupy)
            {
                Destroy(targetNode.transform.GetChild(0).gameObject);
                targetNode.tileType = TileType.Empty;
            }
        }
        else if (targetNode == lastNode || targetNode==null)//如果目标结点和上帧结点相同或为空
        {
            return;
        }
        else if ( placeType == PlaceType.Selected)
        {
            if (CellOnMove != null)
            {
                if (CheckPathAvaliable()&&targetNode.tileType==TileType.Empty)
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
        else 
        {
            if(lastCell!=null)
                lastCell.CloseRangePic();
            lastNode = targetNode;

            
            if (targetNode.tileType ==TileType.Occupy)
            {
                lastCell = targetNode.transform.GetChild(0).GetComponent<CellBase>();
                lastCell.ShowRangePic();
                Debug.Log("StartShow");
            }
            

        }
        

        /*Ray ray = Camera.main.ScreenPointToRay(transform.position);
        RaycastHit[] touches = Physics.RaycastAll(ray, Mathf.Infinity);
        if (touches.Length == 0)
        {
            Debug.Log("null touch");
            return;
        }

        var hit = touches[0];
        for (int i = 0; i < touches.Length; i++)
        {
            Debug.Log((touches[i].collider.name));
        }*/
    }
}
