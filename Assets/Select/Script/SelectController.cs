using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    [SerializeField]
    GameObject PuzzlePanel;
    GameObject[] PuzzlePanels;
    Text[] PanelTexts;
    Image[] PanelImages;
    GameObject scrol;

    [SerializeField]
    GameObject ResultPanel;

    [SerializeField]
    Text input;

    Transform canvasTransform;

    public GameObject[] Controllers;

    string puzzleName;

    void Start()
    {
        Initialized();
    }

    void Update()
    {
        
    }

    void Initialized()
    {
        canvasTransform = GameObject.Find("Content").transform;
        scrol = canvasTransform.parent.transform.parent.gameObject;
        input.transform.parent.gameObject.SetActive(true);

        string path = "Data/Puzzle";

        string text = Resources.Load<TextAsset>(path).ToString();

        Puzzle puzzle = JsonUtility.FromJson<Puzzle>(text);

        PuzzlePanels = new GameObject[puzzle.Count];
        PanelTexts = new Text[puzzle.Count];
        PanelImages = new Image[puzzle.Count];

        for(int i = 0 ;i < puzzle.Count ;i++)
		{
            PuzzlePanels[i] = Instantiate(PuzzlePanel , canvasTransform);
            PanelTexts[i] = PuzzlePanels[i].transform.GetComponentInChildren<Text>();
            PanelImages[i] = PuzzlePanels[i].transform.GetChild(1).GetComponent<Image>();
            PanelTexts[i].text = puzzle.Data[i].NameJP;
            PanelImages[i].sprite = Resources.Load<Sprite>("Images/Select/star" + puzzle.Data[i].Difficulty.ToString());
            var rect = PuzzlePanels[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0 , i * 120);

            PuzzlePanels[i].GetComponent<PuzzlePanelController>().SetController(Controllers[i]);
            PuzzlePanels[i].GetComponent<PuzzlePanelController>().select = this;
            PuzzlePanels[i].GetComponent<PuzzlePanelController>().puzzleName = puzzle.Data[i].NameUS;
            PuzzlePanels[i].name = puzzle.Data[i].NameJP;
        }
    }
    public void StartPuzzle(string name)
	{
        SetActiveScroll(false);
        puzzleName = name;
	}
    public void EndPuzzle(string resultText)
	{
        ResultPanel.SetActive(true);
        string playerName = input.text;
        if(playerName == "") playerName = "None";
        ResultPanel.GetComponent<ResultController>().SetResult(resultText , puzzleName , this , playerName );
	}
    public void SetActiveScroll(bool isActive)
    {
        scrol.SetActive(isActive);
        input.transform.parent.gameObject.SetActive(isActive);
    }
}
[Serializable]
public class Puzzle
{
    public int Count;
    public DataItem[] Data;
}
[Serializable]
public class DataItem
{
    public string NameUS;
    public string NameJP;
    public int Difficulty;
}


