using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SimpleJSON;

public class ProgramList : MonoBehaviour {
	public YleApi yleApi;
	public int searchLimit = 10;
	public GameObject entryPrefab;
	public Text searchText;
	public ProgramInfo programInfoPanel;

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
		// Delete items and reset content size (don't zero it, as QueryOnScrollEnd would be called),
		// so that the scroll bar disapears
		foreach(Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10);

		// First query should fill up the whole ScrollRect with entries
		currentQuery = searchText.text;
		var viewportHeight = rt.parent.GetComponent<RectTransform>().rect.height;
		QueryNext((int) Mathf.Ceil(viewportHeight / entryHeight) + 1);
	}
	
	public void QueryNext(int limit) {
		yleApi.Get("/v1/programs/items.json", new RequestArguments {
			{"q", currentQuery},
			{"limit", limit.ToString()},
			{"offset", searchOffset.ToString()},
		}, onSuccess);
	}

	private void UpdateList(string res) {
		var json = JSON.Parse(res);
		var data = json["data"].AsArray;
		for(int i = 0; i < data.Count; i++) {
			var obj = GameObject.Instantiate(entryPrefab, transform);
			var entry = obj.GetComponent<EntryItem>();
			entry.Setup(yleApi, data[i]);
			obj.GetComponent<Button>().onClick.AddListener(() => programInfoPanel.PopupInfo(entry));
		}
		searchOffset += data.Count;
		StartCoroutine(rt.SetHeightAfterUpdate());
	}

	public void QueryOnScrollEnd(float scroll) {
		if(currentQuery != null && scroll <= 0.0) {
			QueryNext(searchLimit);
		}
	}
}
