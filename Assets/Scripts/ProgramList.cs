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
	private YleApi.ResponseEvent onSuccess = new YleApi.ResponseEvent();
	private RectTransform rt;
	private float entryHeight;

	void Start() {
		onSuccess.AddListener(UpdateList);
		rt = GetComponent<RectTransform>();
		entryHeight = entryPrefab.GetComponent<RectTransform>().rect.height;
	}

	public void NewSearch() {
		// Delete all items and reset content size, so that the scroll bar disapears
		foreach(Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

		currentQuery = searchText.text;
		var viewportHeight = rt.parent.GetComponent<RectTransform>().rect.height;
		QueryNext((int) Mathf.Ceil(viewportHeight / entryHeight));
	}
	
	public void QueryNext(int limit) {
		api.Get("/v1/programs/items.json", new RequestArguments {
			{"q", currentQuery},
			{"limit", limit.ToString()},
			{"offset", searchOffset.ToString()},
		}, onSuccess);
	}

	private void UpdateList(string res) {
		var json = JSON.Parse(res);
		var data = json["data"].AsArray;
		for(int i = 0; i < data.Count; i++) {
			var obj = Object.Instantiate(entryPrefab, transform);
			obj.GetComponent<EntryItem>().Setup(data[i]["title"][0].Value);
		}
		searchOffset += data.Count;
		StartCoroutine(UpdateHeight());
	}

	// Update content height based on last child's position
	private IEnumerator UpdateHeight() {
		if(transform.childCount != 0) {
			var lastRt = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
			// Vertical Layout will only be applied after Update phase, so wait a little
			yield return new WaitForEndOfFrame();
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -lastRt.anchoredPosition.y + lastRt.rect.height);
		}
	}

	public void QueryOnScrollEnd(float scroll) {
		if(currentQuery != null && scroll <= 0.0) {
			QueryNext(searchLimit);
		}
	}
}
