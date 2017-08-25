using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryItem : MonoBehaviour {
	public void Setup(string title) {
		var textObject = GetComponentInChildren<Text>().text = title;
	}
}
