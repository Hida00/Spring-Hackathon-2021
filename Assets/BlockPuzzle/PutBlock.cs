using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutBlock : MonoBehaviour
{
    public const int height = 6;
    public const int width = 6;

    private GameObject[] blocks;

    SelectController select;
    float time;

    void Start()
    {
        //BlockPuzzleが見えるような位置に回転
        GameObject.Find("Main Camera").transform.Rotate(-90, 0, 0);
        Init();
    }

    void Update()
    {
    }

    void Init()
    {
        select = GameObject.Find("PuzzleSelect").GetComponent<SelectController>();

        blocks = new GameObject[9];
        //ボードを設置
        blocks[0] = Instantiate((GameObject)Resources.Load("Images/BlockPuzzle/board"));

        //パズルに使用するブロックをすべて生成
        for (int i = 1; i <= 8; i++)
        {
            blocks[i] = Instantiate((GameObject)Resources.Load("Images/BlockPuzzle/group" + i.ToString()));
        }
    }

    void Finish()
    {
        //カメラの位置を元に戻す
        GameObject.Find("Main Camera").transform.Rotate(90, 0, 0);

        foreach (GameObject block in blocks) Destroy(block);
        Destroy(this.gameObject);

        string sub = "";
        float t = Time.time - time;
        int min = (int)(t / 60);
        int sec = (int)(t % 60);
        if (sec < 10) sub = "0";
        string result = min.ToString() + ":" + sub + sec.ToString();
        select.EndPuzzle(result);
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
        //Finish();
        Invoke(nameof(Finish), 1.5f);
        return true;
    }
}
