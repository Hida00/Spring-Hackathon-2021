using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePanelController : MonoBehaviour , IPointerClickHandler
{
	private GameObject controller;
	[NonSerialized]
	public SelectController select;
	[NonSerialized]
	public string puzzleName;

    public void OnPointerClick(PointerEventData e)
	{
		Instantiate(controller);
		select.StartPuzzle(puzzleName);
	}

	public void SetController(GameObject ctr) => controller = ctr;
}
