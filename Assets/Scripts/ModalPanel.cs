using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalPanel : MonoBehaviour {
	void Update() {
		// Close modal on Esc or Android's Back button
		if(Input.GetKeyDown(KeyCode.Escape)) {
			gameObject.SetActive(false);
		}
	}
}
