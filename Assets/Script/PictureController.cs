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
    bool isFirst = true;

    void Start()
    {
        pictureMatchingController = GameObject.Find("PictureMatching").GetComponent<PictureMatchingController>();
    }

    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData e)
	{
        pictureMatchingController.PanelCountChange(true);
        if(targetPanel.name != "NullPanel")
        {
            pictureMatchingController.ParticlePlay(targetNum , true);
            targetController.isHold = false;
        }
	}
    public void OnDrag(PointerEventData e)
	{
        this.transform.position = e.position;
	}
    public void OnDrop(PointerEventData e)
    {
        if(isFirst)
        {
            isFirst = false;
        }
        else pictureMatchingController.PanelCountChange(true);

        var rayCastResult = new List<RaycastResult>();
        EventSystem.current.RaycastAll(e , rayCastResult);

        foreach(var hit in rayCastResult)
		{
            if(hit.gameObject.CompareTag("Drop") && !hit.gameObject.GetComponent<PicturePanelController>().isHold)
			{
                Debug.Log("Drop");
                targetPanel = hit.gameObject;
                targetController = targetPanel.GetComponent<PicturePanelController>();
                targetController.isHold = true;
                this.transform.position = hit.gameObject.transform.position;
                pictureMatchingController.PanelCountChange(false);
                if(targetName == hit.gameObject.name) pictureMatchingController.ParticlePlay(targetNum , false);
            }
		}
	}
}
