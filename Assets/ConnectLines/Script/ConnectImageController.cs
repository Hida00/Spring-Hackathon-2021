using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConnectImageController : MonoBehaviour , IPointerClickHandler
{
    int Up = 0;
    int Down = 0;
    int Right = 0;
    int Left = 0;

    int[] direction;

    [NonSerialized]
    public int placeNum;

    [NonSerialized]
    public int imageNum;

    [NonSerialized]
    public string imageName;

    [NonSerialized]
    public ConnectLinesController controller;

    bool isLoop = false;

    void Start()
    {
        
    }

    void Update()
    {
        this.name = GetData();
    }
    int CheckNextImage(int oldDir = -1)
	{
        if(isLoop) return -1;
        for(int i = 0 ;i < 4 ;i++)
		{
            if(i != oldDir) IsNext(i);
        }
        return -1;
	}
    void IsNext(int _direction)
	{
        int directionNum;
        switch(_direction)
		{
            case 0:
                directionNum = placeNum - 4;
                break;
            case 1:
                directionNum = placeNum + 4;
                break;
            case 2:
                directionNum = placeNum + 1;
                break;
            case 3:
                directionNum = placeNum - 1;
                break;
            default:
                directionNum = placeNum + 0;
                break;
        }
        if(directionNum < 0) { }
        else if(directionNum >= 16) { }
        else if(directionNum / 4 != placeNum / 4)
		{
            if(directionNum % 4 == placeNum % 4)
            {
                if(direction[_direction] == 1)
                {
                    isLoop = true;
                    controller.LightChange(directionNum , direction[_direction] == 1 , _direction , GetData());
                }
            }
		}
        else
        {
            if(_direction == 0) Debug.Log("Up");
            if(direction[_direction] == 1)
            {
                isLoop = true;
                controller.LightChange(directionNum , direction[_direction] == 1 , _direction , GetData());
            }
        }
	}
    public void ConnectRotate(int angleCount)
    {
        switch(angleCount)
		{
            case 0:
                break;
            case 1:
                direction = new int[4] { direction[2] , direction[3] , direction[1] , direction[0] };
                (Up, Down, Right, Left) = (Right, Left, Down, Up);
                break;
            case 2:
                direction = new int[4] { direction[1] , direction[0] , direction[3] , direction[2] };
                (Up, Down, Right, Left) = (Down, Up, Left, Right);
                break;
            case 3:
                direction = new int[4] { direction[3] , direction[2] , direction[0] , direction[1] };
                (Up, Down, Right, Left) = (Left, Right, Up, Down);
                break;
			default:
                break;
		}
    }
    public void OnPointerClick(PointerEventData e)
    {
        this.transform.Rotate(Vector3.forward * 90);
        ConnectRotate(1);
        CheckNextImage();
	}
    public void SetConnectData(int num)
    {
        switch(num)
        {
            case 0:     //Line
                direction = new int[4] { -1 , -1 , 0 , 0 };
                (Up, Down, Right, Left) = (-1, -1, 0, 0);
                break;
            case 1:     //Cross
                direction = new int[4] { 0 , 0 , 0 , 0 };
                (Up, Down, Right, Left) = (0, 0, 0, 0);
                break;
            case 2:     //TLine
                direction = new int[4] { -1 , 0 , 0 , 0 };
                (Up, Down, Right, Left) = (-1, 0, 0, 0);
                break;
            case 3:     //Corner
                direction = new int[4] { -1 , 0 , 0 , -1 };
                (Up, Down, Right, Left) = (-1, 0, 0, -1);
                break;
            case 5:     //Start
                direction = new int[4] { -1 , 1 , -1 , -1 };
                (Up, Down, Right, Left) = (-1, 1, -1, -1);
                break;
            case 6:     //End
                direction = new int[4] { -1 , 0 , -1 , -1 };
                (Up, Down, Right, Left) = (-1, 0, -1, -1);
                break;
            default:    //Cross
                direction = new int[4] { 0 , 0 , 0 , 0 };
                (Up, Down, Right, Left) = (0, 0, 0, 0);
                break;
        }
    }
    public string GetData() => $"(U:{Up},D:{Down},R:{Right},L:{Left})";
    public void LightPropagation(int _direction,bool isLight)
	{
        switch(_direction)
        {
            case 0:
                if(Down != -1) break;
                else return;
            case 1:
                if(Up != -1) break;
                else return;
            case 2:
                if(Right != -1) break;
                else return;
            case 3:
                if(Left != -1) break;
                else return;
            default:
                return;
        }
        if(isLight)
		{
            this.GetComponent<Image>().sprite = ConnectLinesController.GetSprite(imageName);
        }
        else
		{
            this.GetComponent<Image>().sprite = ConnectLinesController.GetSprite(imageName + "_No");
        }
        for(int i = 0 ;i < 4 ;i++)
		{
            if(direction[i] != -1)
			{
                direction[i] = 1;
                switch(i)
				{
                    case 0:
                        Up = 1;
                        break;
                    case 1:
                        Down = 1;
                        break;
                    case 2:
                        Right = 1;
                        break;
                    case 3:
                        Left = 1;
                        break;
                }
			}
		}
        CheckNextImage(GetNextDirection(_direction));
	}
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
}
