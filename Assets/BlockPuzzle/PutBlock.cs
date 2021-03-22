using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutBlock : MonoBehaviour
{
    public const int height = 6;
    public const int width = 6;

    void Start()
    {
        Init();
    }

    void Update()
    {
    }

    void Init()
    {
        //ボードを設置
        GameObject board = (GameObject)Resources.Load("Images/BlockPuzzle/board");
        Instantiate(board);

        //パズルに使用するブロックをすべて生成
        for (int i = 1; i <= 8; i++)
        {
            GameObject prefab = (GameObject)Resources.Load("Images/BlockPuzzle/group" + i.ToString());
            Instantiate(prefab);
        }
    }

    public bool checkIsAllCorrect()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject gameobject in gameObjects)
        {
            foreach (Transform pos in gameobject.transform)
            {
                int x = Mathf.FloorToInt(pos.position.x);
                int y = Mathf.FloorToInt(pos.position.y);
                if (x < 0 || y < 0 || x >= width || y >= height) return false;
            }
        }
        Debug.Log("All Correct!!");
        return true;
    }
}
