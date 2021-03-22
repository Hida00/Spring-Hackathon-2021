using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockRotate : MonoBehaviour , IPointerClickHandler
{
	[NonSerialized]
	public PutBlock putBlock;

	public void OnPointerClick(PointerEventData e)
	{
		putBlock.SelectBlock.transform.Rotate(0 , 0 , -90);
	}
}
