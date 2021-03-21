using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    [SerializeField]
    Text resultText;
    SelectController select;

    string result;
    string puzzleName;
    string playerName;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetResult(string puzzleResult ,string _puzzleName , SelectController _select ,string _playerName = "None")
	{
        result = puzzleResult;
        resultText.text = puzzleResult;
        select = _select;
        puzzleName = _puzzleName;
        playerName = _playerName;
	}
    public void ClickReturn()
	{
        select.SetActiveScroll(true);
        this.gameObject.SetActive(false);

        var text = Resources.Load<TextAsset>("Data/Result").ToString();
        var res = JsonUtility.FromJson<Item>(text);

        res.Count++;

        var json = new ResultItem[res.Count];

        for(int i = 0 ;i < res.Count;i++)
		{
            if(i < res.Count - 1) json[i] = res.Result[i];
            else json[i] = new ResultItem(playerName , puzzleName , result);
		}
        res.Result = json;

        var jsonText = JsonUtility.ToJson(res);

        File.WriteAllText(Application.dataPath + "/Resources/Data/Result.json" , jsonText);
	}
}
[Serializable]
public class Item
{
    public int Count;
    public ResultItem[] Result;
}
[Serializable]
public class ResultItem
{
    public string Name;
    public string Puzzle;
    public string Time;

    public ResultItem(string _name,string _puzzle,string _time)
	{
        Name = _name;
        Puzzle = _puzzle;
        Time = _time;
	}
}
