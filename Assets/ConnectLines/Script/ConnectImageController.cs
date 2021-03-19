using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConnectImageController : MonoBehaviour , IPointerClickHandler
{
    int[] direction;

    [NonSerialized]
    public int placeNum;

    [NonSerialized]
    public int imageNum;

    [NonSerialized]
    public string imageName;

    [NonSerialized]
    public ConnectLinesController controller;

    Image image;

    [NonSerialized]
    public bool isLighting; 

    void Start()
    {
        image = this.GetComponent<Image>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)) Debug.Log($"{placeNum} : {GetData()}");
    }
    public void LightChange(bool isLit)
    {
        if(isLit)
        {
            isLighting = true;
            image.sprite = ConnectLinesController.GetSprite(imageName);
        }
        else
        {
            isLighting = false;
            image.sprite = ConnectLinesController.GetSprite(imageName + "_No");
        }
    }
    public int CheckNextImage(int dirFrom)
	{
        dirFrom = GetNextDirection(dirFrom);
        Debug.Log($"{placeNum}:{dirFrom}");
        for(int i = 0 ;i < 4 ;i++)
		{
            if(direction[i] == -1) continue;

            if(IsDirectionOK(i) && isLighting)
			{
                controller.LitImage(GetNextNumber(i) , i , true);
			}
		}
        return 0;
	}
    public void ConnectRotate(int angleCount)
    {
        this.transform.Rotate(Vector3.forward * 90 * angleCount);
        switch(angleCount)
		{
            case 0:
                direction = new int[4] { direction[0] , direction[1] , direction[2] , direction[3] };
                break;
            case 1:
                direction = new int[4] { direction[2] , direction[3] , direction[1] , direction[0] };
                break;
            case 2:
                direction = new int[4] { direction[1] , direction[0] , direction[3] , direction[2] };
                break;
            case 3:
                direction = new int[4] { direction[3] , direction[2] , direction[0] , direction[1] };
                break;
			default:
                direction = new int[4] { direction[0] , direction[1] , direction[2] , direction[3] };
                break;
		}
    }
    public void OnPointerClick(PointerEventData e)
    {
        if(imageNum == 6) return;

		ConnectRotate(1);
        CheckNextImage(-1);

        //Debug.Log($"{placeNum} : {GetData()}");
	}
    public void SetConnectData(int num)
    {
        switch(num)
        {
            case 0:     //Line
                direction = new int[4] { -1 , -1 , 0 , 0 };
                break;
            case 1:     //Cross
                direction = new int[4] { 0 , 0 , 0 , 0 };
                break;
            case 2:     //TLine
                direction = new int[4] { -1 , 0 , 0 , 0 };
                break;
            case 3:     //Corner
                direction = new int[4] { -1 , 0 , 0 , -1 };
                break;
            case 5:     //Start
                direction = new int[4] { -1 , 1 , -1 , -1 };
                break;
            case 6:     //End
                direction = new int[4] { -1 , 0 , -1 , -1 };
                break;
            default:    //Cross
                direction = new int[4] { 0 , 0 , 0 , 0 };
                break;
        }
    }
    public string GetData() => $"(U:{direction[0]},D:{direction[1]},R:{direction[2]},L:{direction[3]})";
    int GetNextDirection(int dir)
	{
        switch(dir)
		{
            case 0:
                return 1;
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 2;
            default:
                return -1;
		}
	}
    int GetNextNumber(int dir)
	{
        switch(dir)
		{
            case 0:
                return placeNum - 4;
            case 1:
                return placeNum + 4;
            case 2:
                return placeNum + 1;
            case 3:
                return placeNum - 1;
            default:
                return -1;
        }
	}
    public bool IsDirectionOK()
	{
        bool OK = true; 
        if(placeNum / 4 == 0) if(direction[0] != -1) OK = false;
        if(placeNum / 4 == 3) if(direction[1] != -1) OK = false;
        if(placeNum % 4 == 0) if(direction[3] != -1) OK = false;
        if(placeNum % 4 == 3) if(direction[2] != -1) OK = false;

        return OK;
    }
    public bool IsDirectionOK(int dirFrom)
	{
        switch(dirFrom)
		{
            case 0:
                if(placeNum / 4 == 0) return false;
                break;
            case 1:
                if(placeNum / 4 == 3) return false;
                break;
            case 2:
                if(placeNum % 4 == 3) return false;
                break;
            case 3:
                if(placeNum % 4 == 0) return false;
                break;
            default:
                break;
        }
        return true;
	}
}
public enum Derection
{
    Up,
    Down,
    Right,
    Left,
}
public enum Light
{
    ON = 0,
    OFF = 1,
    NO = -1,
}
