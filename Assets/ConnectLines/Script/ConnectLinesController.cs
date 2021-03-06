using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectLinesController : MonoBehaviour
{
    [SerializeField]
    Image connectImage;
    [SerializeField]
    Image reStart;
    Image restart;
    Image[] connectImages;
    ConnectImageController[] controllers;
    SelectController select;
	private Transform canvasTransform;

    float time;
    bool isClear = false;

    //初めから光っている場所
    private int startPos;
    void Start()
    {
        Initialize();
        time = Time.time;
        Invoke(nameof(UpdateLights), 0.01f);
    }

    void Update()
    {
        
    }
    public void Clear()
	{
        isClear = true;
        Invoke(nameof(Finish) , 1.5f);
	}
    public void Finish()
	{
        foreach(var obj in connectImages) Destroy(obj.gameObject);
        if(isClear)
        {
            string sub = "";
            float t = Time.time - time;
            int min = (int)( t / 60 );
            int sec = (int)( t % 60 );
            if(sec < 10) sub = "0";
            string result = min.ToString() + ":" + sub + sec.ToString();
            select.EndPuzzle(result);
        }
        Destroy(restart.gameObject);
        Destroy(this.gameObject);
	}
    public void ReStart()
	{
        foreach(var obj in connectImages) Destroy(obj.gameObject);
        Destroy(restart.gameObject);
        Initialize();
        Invoke(nameof(UpdateLights) , 0.01f);
    }
    void Initialize()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        select = GameObject.Find("PuzzleSelect").GetComponent<SelectController>();

        connectImages = new Image[16];
        controllers = new ConnectImageController[16];

        restart = Instantiate(reStart , canvasTransform);
        restart.GetComponent<ReStartController>().controller = this;

        startPos = UnityEngine.Random.Range(0 , 15);
        int end = UnityEngine.Random.Range(0 , 16);

        if(startPos == end) end = 15;

        for(int i = 0 ; i < 16 ; i++)
        {
            int x = i % 4 - 2;
            int y = i / 4 - 1;

            int num = UnityEngine.Random.Range(0 , 4);
            int angleCount = UnityEngine.Random.Range(0 , 4);

            connectImages[i] = Instantiate(connectImage , canvasTransform);
            connectImages[i].rectTransform.anchoredPosition = new Vector2(45 + 95 * x , -( -45 + 95 * y ));
            controllers[i] = connectImages[i].GetComponent<ConnectImageController>();
            controllers[i].controller = this;
            controllers[i].placeNum = i;

            string imageName;

            if(i == startPos)
            {
                imageName = "Start";
                connectImages[i].sprite = GetSprite(imageName);
                controllers[i].imageNum = 5;
                controllers[i].SetConnectData(5);
                controllers[i].ConnectRotate(angleCount);
                controllers[i].isLighting = true;
                while(!controllers[i].IsDirectionOK()) controllers[i].ConnectRotate(1);
            }
            else if(i == end)
            {
                imageName = "End";
                connectImages[i].sprite = GetSprite(imageName + "_No");
                controllers[i].imageNum = 6;
                controllers[i].SetConnectData(6);
                controllers[i].ConnectRotate(angleCount);
                controllers[i].isLighting = false;
                while(!controllers[i].IsDirectionOK()) controllers[i].ConnectRotate(1);
            }
            else
            {
                angleCount = 0;
                imageName = GetImageName(num);
                controllers[i].SetConnectData(num);
                controllers[i].imageNum = num;
                controllers[i].ConnectRotate(angleCount);
                controllers[i].isLighting = false;
                connectImages[i].sprite = GetSprite(imageName + "_No");
            }
            controllers[i].imageName = imageName;
            connectImages[i].name = controllers[i].GetData();
        }
    }

    public void UpdateLights()
    {
        //電気をすべて消す
        foreach (ConnectImageController controller in controllers)
        {
            controller.isLighting = false;
            controller.image.sprite = GetSprite(controller.imageName + "_No");
            controller.LightTurnStatus(false);
        }

        //光っている場所からつながっているところをすべて光るようにする
        ChangeLights(startPos);
	}

    void ChangeLights(int currentPos)
    {
        ConnectImageController current = controllers[currentPos];

        //光るようにする
        current.isLighting = true;
        current.image.sprite = GetSprite(current.imageName);
        current.LightTurnStatus(true);

        //更新
        controllers[currentPos] = current;

        for (int i = 0; i < 4; i++)
        {
            if (current.direction[i] == -1) continue;
            if (!current.IsDirectionOK(i)) continue;

            int next_num = current.GetNextNumber(i);
            int opposite = current.GetNextDirection(i);

            //隣と繋がっていなかったらcontinue
            if (controllers[next_num].direction[opposite] == -1) continue;


            if (!controllers[next_num].isLighting)
            {
                ChangeLights(next_num);
			}
		}
	}

    /*
    public void LitImage(int litTo,int dirFrom,bool isLighting ,int startNum = -1)
	{
        if(isLighting && !controllers[litTo].isLighting)
        {
            controllers[litTo].LightChange(isLighting , dirFrom);
            controllers[litTo].CheckNextImage(dirFrom);
        }
        else if(!isLighting && controllers[litTo].isLighting)
        {
            controllers[litTo].LightChange(isLighting , dirFrom , startNum);
            controllers[litTo].LightTurnOff(dirFrom , startNum);
        }
	}
    */
    public static Sprite GetSprite(string url)
	{
        string path = "Images/ConnectLines/" + url;
        Sprite sp = Resources.Load<Sprite>(path);
        return sp;
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
    public static Dictionary<int , string> Directions = new Dictionary<int , string>()
    {
        { 0 , "Up" },
        { 1 , "Down" },
        { 2 , "Right" },
        { 3 , "Left" }
    };
}
