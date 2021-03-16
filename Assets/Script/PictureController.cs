using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PictureController : MonoBehaviour , IDragHandler , IDropHandler
{
    [NonSerialized]
    public int targetNum;
    [NonSerialized]
    public string targetName;
    [NonSerialized]
    public GameObject targetPanel;

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
        if(isStart && targetPanel.name == targetName)
		{
            isStart = false;
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
            if(hit.gameObject.CompareTag("Drop"))
			{
                targetPanel = hit.gameObject;
                this.transform.position = hit.gameObject.transform.position;
                pictureMatchingController.PanelCountChange(false);
                if(targetName == hit.gameObject.name) pictureMatchingController.ParticlePlay(targetNum);
            }
		}
	}
}
