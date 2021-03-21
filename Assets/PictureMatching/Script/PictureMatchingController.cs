using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    Image clearImg;

    SelectController select;
    Transform canvasTransform;

    int PanelCount;
    int ImageNum;

    float time;

    bool isFinish = false;

    void Start()
    {
        PanelCount = 0;
        this.name = "PictureMatching";
        Initialized();
        time = Time.time;
    }

    void Update()
    {
        if(PanelCount == 9 && !isFinish)
        {
            Invoke(nameof(Clear) , 0.5f);
            Invoke(nameof(Finish) , 2.0f);
            isFinish = true;
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
        select = GameObject.Find("PuzzleSelect").GetComponent<SelectController>();

        nullPanelIns = Instantiate(nullPanel , canvasTransform);
        nullPanelIns.name = "NullPanel";

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

        ImageNum = UnityEngine.Random.Range(0 , 10) % 2;

        for(int i = 0; i < 9; i++)
		{
            int x = i % 3 - 2;
            int y = i / 3 - 1;

            int dif1 = UnityEngine.Random.Range(-30 , 30);
            int dif2 = UnityEngine.Random.Range(-30 , 30);

            pictureParts[i] = Instantiate(picturePart , canvasTransform);
            pictureParts[i].GetComponent<PictureController>().targetPanel = nullPanelIns;
            pictureParts[i].GetComponent<PictureController>().targetName = picturePanels[i].name;
            pictureParts[i].GetComponent<PictureController>().targetNum = i;
            pictureParts[i].rectTransform.anchoredPosition = new Vector2(120f * dif1 / 30f, 120f * dif2 / 30f);
            pictureParts[i].name = $"picture{i + 1}";
            pictureParts[i].GetComponent<Image>().sprite = GetSprite(SetImages(ImageNum) + ( i + 1 ).ToString());
            pictureParts[i].GetComponent<PictureController>().isStart = true;
            pictureParts[i].GetComponent<PictureController>().targetController = nullPanelIns.GetComponent<PicturePanelController>();
        }
	}
    Sprite GetSprite(string imagePath)
	{
        string url = "Images/PictureMatching/" + imagePath;
        Sprite sp = Resources.Load<Sprite>(url);
        return sp;
	}
    void Clear()
    {
        foreach(var obj in pictureParts) Destroy(obj);
        clearImg = Instantiate(picturePart , canvasTransform);
        clearImg.rectTransform.sizeDelta = new Vector2(300 , 300);
        clearImg.rectTransform.anchoredPosition = new Vector2(0 , 0);
        clearImg.sprite = GetSprite(SetImages(ImageNum));
    }
    void Finish()
	{
        foreach(var obj in picturePanels) Destroy(obj);
        Destroy(nullPanelIns);
        Destroy(clearImg);
        {
            string sub = "";
            float t = Time.time - time;
            int min = (int)( t / 60 );
            int sec = (int)( t % 60 );
            if(sec < 10) sub = "0";
            string result = min.ToString() + ":" + sub + sec.ToString();
            select.EndPuzzle(result);
        }
        Destroy(this.gameObject);
	}
    public void PanelCountChange(bool isSubtraction)
	{
		if(isSubtraction) PanelCount--;
		else PanelCount++;
	}
    public void ParticlePlay(int particleNum,bool isSubtraction)
	{
        if(isSubtraction)
        {
            picturePanels[particleNum].GetComponent<Image>().color = new Color(255f , 255f , 255f , 100f / 255f);
        }
        else
        {
            picturePanels[particleNum].GetComponent<Image>().color = new Color(255f / 255f , 128f / 255f , 128f / 255f);
        }
	}
    public string SetImages(int num)
	{
        switch(num)
		{
			case 0:
                return "lavender";
			case 1:
                return "mountain";
            default:
                return "mountain";
        }
	}
}
