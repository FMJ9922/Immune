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
    public Text text;

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
        CellButtonName OnClick = new CellButtonName(OnButtonSelect);
        OnClick += OnButtonSelect;
        PathAvaliable = true;
        isSelect = false;
    }
    public void OnButtonSelect(CellType cellType)
    {
        
        if (placeType == PlaceType.Null&&isSelect)
        {
            CellOnMove = Instantiate(CellOnMovePfb, transform.position, Quaternion.identity,transform)as GameObject;
            CellOnMove.name = cellType.ToString();
            //text.text = transform.position.x+","+ transform.position.y + "," + transform.position.z ;
            //To do:增加图片
            placeType = PlaceType.Selceted;
            this.cellType = cellType;
            isSelect = false;
        }
        
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
            if (targetNode!=null&&targetNode.tileType ==TileType.Empty&& placeType == PlaceType.Selceted)
            {
                
                if (OnPlantCell != null)
                {
                    targetNode.tileType = TileType.Occupy;
                    OnPlantCell();
                }
                if (PathAvaliable)
                {
                    GameObject cell = Instantiate(CellPfbs[(int)cellType],
                    new Vector3(targetNode.pos.x, targetNode.pos.y),
                    Quaternion.identity,
                    targetNode.transform);
                    cell.name = cellType.ToString();
                    
                    
                    
                    //To Do 放置正确音效，粒子效果
                }
                else
                {
                    targetNode.tileType = TileType.Empty;
                    PathAvaliable = true;
                    //To Do 放置错误：路径无效音效
                }
            }

            if (placeType == PlaceType.Selceted)
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


        transform.position =Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
        /*Ray ray = Camera.main.ScreenPointToRay(transform.position);
        RaycastHit[] touches = Physics.RaycastAll(ray, Mathf.Infinity);

       
        //Debug.Log(transform.position+"=>"+targetNode.name);
        if (touches.Length == 0)
        {
            //Debug.Log("null touch");
            return;
        }

        var hit = touches[0];
        */
    }
}
