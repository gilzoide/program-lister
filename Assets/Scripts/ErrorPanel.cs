using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPanel : MonoBehaviour {
	public Text body;

	public void FillError(string error) {
		body.text = error;
	}
}
