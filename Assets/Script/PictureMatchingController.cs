using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureMatchingController : MonoBehaviour
{
    [SerializeField]
    Image picturePart;
    Image[] pictureParts;
    
    [SerializeField]
    GameObject picturePanel;
    GameObject[] picturePanels;

    [SerializeField]
    GameObject nullPanel;
    GameObject nullPanelIns;

    Transform canvasTransform;

    int PanelCount;

    void Start()
    {
        PanelCount = 0;
        this.name = "PictureMatching";
        Initialized();
    }

    void Update()
    {
        if(PanelCount == 9)
        {
            Finish();
        }
        if(Input.GetKeyDown(KeyCode.B))
		{
            Debug.Log($"{PanelCount}");
        }
        if(Input.GetKeyDown(KeyCode.C)) PanelCount++;
    }
    void Initialized()
	{
        canvasTransform = GameObject.Find("Canvas").transform;

        nullPanelIns = Instantiate(nullPanel , canvasTransform);

        pictureParts = new Image[9];
        picturePanels = new GameObject[9];

        for(int i = 0 ;i < 9 ;i++)
        {
            int x = i % 3 - 2;
            int y = i / 3 - 1;

            picturePanels[i] = Instantiate(picturePanel , canvasTransform);
            picturePanels[i].transform.localPosition = new Vector2(120 + 120 * x , -( 120 * y ));
            picturePanels[i].name = $"picturePanel{i + 1}";
        }

        for(int i = 0; i < 9; i++)
		{
            int x = i % 3 - 2;
            int y = i / 3 - 1;

            int dif = UnityEngine.Random.Range(-30 , 30);

            pictureParts[i] = Instantiate(picturePart , canvasTransform);
            pictureParts[i].GetComponent<PictureController>().targetPanel = nullPanelIns;
            pictureParts[i].GetComponent<PictureController>().targetName = picturePanels[i].name;
            pictureParts[i].rectTransform.anchoredPosition = new Vector2(120 + 120 * x + dif, -(120 * y) + dif);
            pictureParts[i].name = $"picture{i + 1}";

            var text = pictureParts[i].transform.GetChild(0).GetComponent<Text>();
            text.text = (i + 1).ToString();

            pictureParts[i].GetComponent<PictureController>().isStart = true;
        }
	}
    void Finish()
	{
        foreach(var obj in picturePanels) Destroy(obj);
        foreach(var obj in pictureParts)
        {
            Destroy(obj.transform.GetChild(0).gameObject);
            Destroy(obj);
        }
        Destroy(nullPanelIns);
        Destroy(this.gameObject);
	}
    public void PanelCountChange(bool isSubtraction)
	{
        if(isSubtraction) PanelCount--;
        else PanelCount++;
	}
}
