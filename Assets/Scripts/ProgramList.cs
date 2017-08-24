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

	private uint searchOffset = 0;
	private YleApi.JsonEvent onSuccess = new YleApi.JsonEvent();
	private RectTransform rt;

	void Start() {
		onSuccess.AddListener(InitializeList);
		rt = GetComponent<RectTransform>();
	}

	public void Search() {
		foreach(Transform child in transform) {
			Object.Destroy(child.gameObject);
		}
		UpdateList(searchText.text);
	}
	
	public void UpdateList(string query) {
		api.Get("/v1/programs/items.json", new RequestArguments {
			{"q", query},
			{"limit", searchLimit.ToString()},
			{"offset", searchOffset.ToString()},
		}, onSuccess);
	}

	private void InitializeList(JSONNode json) {
		var data = json["data"].AsArray;
		// Debug.Log(data);
		for(int i = 0; i < data.Count; i++) {
			var obj = Object.Instantiate(entryPrefab, transform);
			obj.GetComponent<Text>().text = data[i]["title"][0].Value;
		}

		StartCoroutine(UpdateHeight());
	}

	// Update content height based on last child's position
	private IEnumerator UpdateHeight() {
		var lastRt = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
		// Vertical Layout will only be applied after Update phase, so wait a little
		yield return new WaitForEndOfFrame();
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -lastRt.anchoredPosition.y + lastRt.rect.height);
	}
}
