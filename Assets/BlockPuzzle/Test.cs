using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool isClick = false;
    private float adjust = 0;
    private Vector3 prepos;

    [NonSerialized]
    public PutBlock putBlock;

    void Start()
    {
        if(transform.position.x - (int)transform.position.x != 0) adjust = 0.5f;
        prepos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(isClick)
            {
                transform.Rotate(0 , 0 , -90);
            }
        }
    }
    public Vector3 screenPoint;
    public Vector3 offset;

    void OnMouseDown()
    {
        this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y , screenPoint.z));
        putBlock.SelectBlock = this.gameObject;
        isClick = true;
    }

    void OnMouseDrag()
    {
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x , Input.mousePosition.y , screenPoint.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
        transform.position = currentPosition;
    }
    private void OnMouseUp()
    {
        Vector3 curpos = transform.position;
        transform.position = new Vector3(Mathf.Round(curpos.x - adjust) + adjust , Mathf.Round(curpos.y - adjust) + adjust , curpos.z);
        isClick = false;
        //ブロックが置けるかの判定
        //ほかのブロックと重なっていた場合は操作する前の位置に戻す
        if(checkIsOnTrigger())
        {
            transform.position = prepos;
            return;
        }

        prepos = transform.position;
        isClick = false;
        FindObjectOfType<PutBlock>().checkIsAllCorrect();
    }
    //自身のブロックとほかのブロックが重なっているかどうかを確認
    private bool checkIsOnTrigger()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Block");
        foreach(GameObject gameobject in gameObjects)
        {
            if(gameobject == this.gameObject) continue;
            foreach(Transform pos in gameobject.transform)
            {
                foreach(Transform this_pos in this.transform)
                {
                    if(Mathf.FloorToInt(pos.position.x) == Mathf.FloorToInt(this_pos.position.x)
                     && Mathf.FloorToInt(pos.position.y) == Mathf.FloorToInt(this_pos.position.y))
                        return true;
                }
            }
        }
        return false;
    }
}
