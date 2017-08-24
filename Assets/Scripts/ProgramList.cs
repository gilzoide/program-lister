using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SimpleJSON;

public class ProgramList : MonoBehaviour {
	public YleApi api;
	public int searchLimit = 10;
	public GameObject entryPrefab;
	public Text searchText;

	private int searchOffset = 0;
	private string currentQuery = null;
	private YleApi.JsonEvent onSuccess = new YleApi.JsonEvent();
	private RectTransform rt;

	void Start() {
		onSuccess.AddListener(UpdateList);
		rt = GetComponent<RectTransform>();
	}

	public void NewSearch() {
		// Delete all items and reset content size, so that the scroll bar disapears
		foreach(Transform child in transform) {
			Object.Destroy(child.gameObject);
		}
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
		currentQuery = searchText.text;
		QueryNext();
	}
	
	public void QueryNext() {
		api.Get("/v1/programs/items.json", new RequestArguments {
			{"q", currentQuery},
			{"limit", searchLimit.ToString()},
			{"offset", searchOffset.ToString()},
		}, onSuccess);
	}

	private void UpdateList(JSONNode json) {
		var data = json["data"].AsArray;
		for(int i = 0; i < data.Count; i++) {
			var obj = Object.Instantiate(entryPrefab, transform);
			obj.GetComponent<Text>().text = data[i]["title"][0].Value;
		}
		searchOffset += searchLimit;
		StartCoroutine(UpdateHeight());
	}

	// Update content height based on last child's position
	private IEnumerator UpdateHeight() {
		var lastRt = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
		// Vertical Layout will only be applied after Update phase, so wait a little
		yield return new WaitForEndOfFrame();
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -lastRt.anchoredPosition.y + lastRt.rect.height);
	}

	public void QueryOnScrollEnd(float scroll) {
		if(currentQuery != null && scroll <= 0.0) {
			QueryNext();
		}
	}
}
