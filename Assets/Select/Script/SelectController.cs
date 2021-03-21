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
    GameObject scrol;

    Transform canvasTransform;

    public GameObject[] Controllers;

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

        string path = "Data/Puzzle";

        string text = Resources.Load<TextAsset>(path).ToString();

        Puzzle puzzle = JsonUtility.FromJson<Puzzle>(text);

        PuzzlePanels = new GameObject[puzzle.Count];
        PanelTexts = new Text[puzzle.Count];

        for(int i = 0 ;i < puzzle.Count ;i++)
		{
            PuzzlePanels[i] = Instantiate(PuzzlePanel , canvasTransform);
            PanelTexts[i] = PuzzlePanels[i].transform.GetComponentInChildren<Text>();
            PanelTexts[i].text = puzzle.Data[i].NameJP;
            var rect = PuzzlePanels[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0 , i * 120);

            PuzzlePanels[i].GetComponent<PuzzlePanelController>().SetController(Controllers[i]);
            PuzzlePanels[i].GetComponent<PuzzlePanelController>().select = this;
		}
    }
    public void StartPuzzle()
	{
        scrol.SetActive(false);
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


