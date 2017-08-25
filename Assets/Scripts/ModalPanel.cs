using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModalPanel : MonoBehaviour {
	void Update() {
		// Close modal on Esc or Android's Back button
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Close();
		}
	}

	public void Popup() {
		gameObject.SetActive(true);
	}

	public void Close() {
		gameObject.SetActive(false);
	}
}
