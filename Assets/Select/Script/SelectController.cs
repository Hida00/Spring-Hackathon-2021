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
    Image Pause;

    [SerializeField]
    GameObject PausePanel;

    [SerializeField]
    GameObject ResultPanel;

    [SerializeField]
    Text input;

    Transform canvasTransform;

    public GameObject[] Controllers;
    GameObject pauseObj;

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

            var img = PuzzlePanels[i].transform.GetChild(3).GetComponent<Image>();
            if(!puzzle.Data[i].Phone) img.sprite = Resources.Load<Sprite>("Images/Select/sign");
        }
    }
    public void StartPuzzle(string name, GameObject obj)
	{
        SetActiveScroll(false);
        puzzleName = name;
        pauseObj = obj;
	}
    public void EndPuzzle(string resultText)
	{
        ResultPanel.SetActive(true);
        string playerName = input.text;
        if(playerName == "") playerName = "None";
        ResultPanel.GetComponent<ResultController>().SetResult(resultText , puzzleName , this , playerName);
    }
    public void SetActiveScroll(bool isActive)
    {
        scrol.SetActive(isActive);
        input.transform.parent.gameObject.SetActive(isActive);
        Pause.gameObject.SetActive(!isActive);
    }
    public void ClickPause()
	{
        Time.timeScale = 0;
        PausePanel.SetActive(true);
	}
    public void ReturnGame()
	{
        Time.timeScale = 1;
        PausePanel.SetActive(false);
	}
    public void ReturnTitle()
	{
        Time.timeScale = 1;
        PausePanel.SetActive(false);

        if(puzzleName == "Picture Matching Puzzle") pauseObj.GetComponent<PictureMatchingController>().Finish();
        else if(puzzleName == "Connect Line Puzzle")
        {
            pauseObj.GetComponent<ConnectLinesController>().Finish();
        }
        else if(puzzleName == "Hockey")
        {
            pauseObj.GetComponent<HockeyController>().Finish();
        }
        else if(puzzleName == "BlockPuzzle")
        {
            pauseObj.GetComponent<PutBlock>().Finish();
        }

        SetActiveScroll(true);
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
    public bool Phone;
}


