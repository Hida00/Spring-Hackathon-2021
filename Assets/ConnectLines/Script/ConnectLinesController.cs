using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectLinesController : MonoBehaviour
{
    [SerializeField]
    Image connectImage;
    Image[] connectImages;
    ConnectImageController[] controllers;

    [SerializeField]
    Transform canvasTransform;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        
    }
    void Initialize()
	{
        connectImages = new Image[16];
        controllers = new ConnectImageController[16];

        int start = UnityEngine.Random.Range(0 , 15);
        int end = UnityEngine.Random.Range(0 , 16);

        if(start == end) end = 15;

        for(int i = 0 ; i < 16 ; i++)
        {
            int x = i % 4 - 2;
            int y = i / 4 - 1;

            int num = UnityEngine.Random.Range(0 , 4);

            connectImages[i] = Instantiate(connectImage , canvasTransform);
            controllers[i] = connectImages[i].GetComponent<ConnectImageController>();
            connectImages[i].rectTransform.anchoredPosition = new Vector2(45 + 95 * x , -( -45 + 95 * y ));
            connectImages[i].name = $"ConnectImage{i}";

            int angleCount = UnityEngine.Random.Range(0 , 4);

            string imageName;

            if(i == start)
			{
                imageName = "Start";
                connectImages[i].transform.Rotate(Vector3.forward * 90 * angleCount);
                connectImages[i].sprite = GetSprite(imageName);
                num = 5;
            }
            else if(i == end)
            {
                imageName = "End";
                connectImages[i].transform.Rotate(Vector3.forward * 90 * angleCount);
                connectImages[i].sprite = GetSprite(imageName + "_No");
                num = 6;
            }
            else
			{
                imageName = GetImageName(num);
                angleCount = 0;
                connectImages[i].sprite = GetSprite(imageName + "_No");
            }
            controllers[i].imageNum = num;
            controllers[i].placeNum = i;
            controllers[i].SetConnectData(num);
            controllers[i].ConnectRotate(angleCount);
            controllers[i].imageName = imageName;
            controllers[i].controller = this;
        }
    }
    public static Sprite GetSprite(string url)
	{
        string path = "Images/ConnectLines/" + url;
        Sprite sp = Resources.Load<Sprite>(path);
        return sp;
	}
    public void LightChange(int dirNum,bool isLight,int dir,string test)
	{
        //Debug.Log($"Connect{connectImages[dirNum]}");
        controllers[dirNum].LightPropagation(dir , isLight);
	}
    string GetImageName(int num)
    {
        switch(num)
        {
            case 0:
                return "Line";
            case 1:
                return "Cross";
            case 2:
                return "TLine";
            case 3:
                return "Corner";
            default:
                return "Cross";
        }
    }
}
