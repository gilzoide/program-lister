using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HyperLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public Text tooltip;
	public string url;

	void Start() {
		tooltip.text = url;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		tooltip.gameObject.SetActive(true);
	}
	public void OnPointerExit(PointerEventData eventData) {
		tooltip.gameObject.SetActive(false);
	}

	public void OpenLink() {
		Application.OpenURL(url);
	}
}
