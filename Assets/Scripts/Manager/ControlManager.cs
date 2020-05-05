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
            CellOnMove = Instantiate(CellOnMovePfb, transform.position, Quaternion.identity, transform) as GameObject;
            string path = "Cell/" + (int)cellType + cellType.ToString() + "/" + cellType.ToString();
            CellOnMove.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
            CellOnMove.name = cellType.ToString();
            //text.text = transform.position.x+","+ transform.position.y + "," + transform.position.z ;
            //To do:增加图片
            placeType = PlaceType.Selected;
            this.cellType = cellType;
            isSelect = false;
        }

    }
    public bool CheckPathAvaliable()
    {
        bool isAvaliable = true;
        Vector4[] vectors = JsonIO.GetDefaultPath();
        AStarAgent agent = transform.GetComponent<AStarAgent>();
        
        for (int i = 0; i < vectors.Length; i++)
        {
            if (targetNode == LevelManager.Instance.GetNodeByPos(new Vector2(vectors[i].x, vectors[i].y)))
                return false;
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

            if (targetNode != null && targetNode.tileType == TileType.Empty && placeType == PlaceType.Selected)
            {

                targetNode.tileType = TileType.Occupy;
                if (OnPlantCell != null)
                {
                    OnPlantCell();
                }
                CellData cellData;
                cellData = JsonIO.GetCellData(cellType);
                if (PathAvaliable && CheckPathAvaliable()&&LevelManager.Instance.SpendPoints(cellData.initCost))
                {
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
       
        if (targetNode == lastNode|| placeType == PlaceType.Selected) return;
        else
        {
            if(lastCell!=null)
                lastCell.CloseRangePic();
            lastNode = targetNode;
            //Debug.Log(targetNode.name);
            //if (OnClickTile != null)
            //{
            //    OnClickTile(true);
            //}
            if(targetNode.tileType ==TileType.Occupy)
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
