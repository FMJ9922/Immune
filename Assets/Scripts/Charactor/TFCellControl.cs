using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFCellControl : AssistCellBase
{
    
    public GameObject product;
    public Transform InitPos;
    public Transform TYCell;
    public override void InitCell()
    {
        base.InitCell();
        allowProduce = true;
        transform.GetComponent<CircleCollider2D>().radius = atkRange;
        OnCellStatusChange(CellStatus.SpecialAbility);
    }
    protected override void FixedUpdate()
    {
        if (cellStatus == CellStatus.Die || !allowProduce) { return; }

        ProduceSlider.value = Mathf.Clamp(reloadTime / atkDuration, 0, 1);
        if (LevelManager.Instance.levelPoints.KangYuanPoints > atkDamage)
        {
            if (reloadTime < atkDuration)
            {
                reloadTime += Time.deltaTime;

            }
            else
            {
                Produce();
                
                reloadTime = 0;
            }
        }
        
    }
    public override void Produce()
    {
        if(LevelManager.Instance.SpendPoints(PointsType.KangYuan, atkDamage))
        {
            cellAnimator.CleanFrameData();
            OnCellStatusChange(CellStatus.SpecialAbility);
            LevelManager.Instance.AddPoints(PointsType.LinBa, atkDamage);
            GameObject newproduct = Instantiate(product, transform);
            newproduct.transform.position = InitPos.position;
        }
    }
    public override void StartAction()
    {
        
    }
    public override void StopAction()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckLeftOrRight(collision.transform);
        if(collision.CompareTag("Cell"))
        {
            
            CellBase cellBase = collision.transform.GetComponent<CellBase>();

            if (cellBase!=null&&cellBase.cellType == CellType.TY)
            {
                TYCell = collision.transform;
                TYCellControl tYCellControl = TYCell.GetComponent<TYCellControl>();
                tYCellControl.OnTYTakingAction += HandleTYinfo;
                HandleTYinfo(tYCellControl.isAllow);
                //Debug.Log("guazai");
            }
        }
    }
    private void HandleTYinfo(bool isAllow)
    {
        Debug.Log("?"+ isAllow);
        allowProduce = isAllow;
        if (isAllow)
        {
            OnCellStatusChange(CellStatus.Produce);
        }
        else
        {
            OnCellStatusChange(CellStatus.Idle);
        }
    }
}
