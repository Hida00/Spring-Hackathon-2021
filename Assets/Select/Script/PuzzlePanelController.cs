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

    public void OnPointerClick(PointerEventData e)
	{
		Instantiate(controller);
		select.StartPuzzle();
	}

	public void SetController(GameObject ctr) => controller = ctr;
}
