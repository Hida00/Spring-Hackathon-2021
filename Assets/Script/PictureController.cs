using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureController : MonoBehaviour , IDragHandler , IDropHandler ,IBeginDragHandler
{
    [NonSerialized]
    public int targetNum;
    [NonSerialized]
    public string targetName;
    [NonSerialized]
    public GameObject targetPanel;
    [NonSerialized]
    public PicturePanelController targetController;

    PictureMatchingController pictureMatchingController;

    [NonSerialized]
    public bool isStart = false;

    bool isClear = false;

    void Start()
    {
        pictureMatchingController = GameObject.Find("PictureMatching").GetComponent<PictureMatchingController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) Debug.Log($"{this.name}:{isClear}");
    }
    public void OnBeginDrag(PointerEventData e)
	{
	}
    public void OnDrag(PointerEventData e)
	{
        this.transform.position = e.position;
	}
    public void OnDrop(PointerEventData e)
    {
        bool isHit = false;

        var rayCastResult = new List<RaycastResult>();
        EventSystem.current.RaycastAll(e , rayCastResult);

        if(targetController.isHold)
        {
            targetController.isHold = false;
            pictureMatchingController.ParticlePlay(targetNum , true);
        }

        foreach(var hit in rayCastResult)
        {
            if(hit.gameObject.CompareTag("Drop") && !hit.gameObject.GetComponent<PicturePanelController>().isHold)
            {
                isHit = true;
                targetPanel = hit.gameObject;
                targetController = targetPanel.GetComponent<PicturePanelController>();
                targetController.isHold = true;
                this.transform.position = hit.gameObject.transform.position;
                if(targetName == hit.gameObject.name)
                {
                    isClear = true;
                    pictureMatchingController.ParticlePlay(targetNum , false);
                    pictureMatchingController.PanelCountChange(false);
                }
                else if(isClear)
                {
                    pictureMatchingController.PanelCountChange(true);
                    isClear = false;
                }
            }
            //else if(!hit.gameObject.CompareTag("Picture"))
            //{
            //    isClear = false;
            //    pictureMatchingController.PanelCountChange(true);
            //    Debug.Log($"else {hit.gameObject.name}");
            //}
        }
        if(!isHit && isClear)
		{
            isClear = false;
			pictureMatchingController.PanelCountChange(true);
			Debug.Log($"else");
		}
    }
}
