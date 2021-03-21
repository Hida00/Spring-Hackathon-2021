using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReStartController : MonoBehaviour , IPointerClickHandler
{
	public ConnectLinesController controller;

	public void OnPointerClick(PointerEventData e)
	{
		controller.ReStart();
	}
}
