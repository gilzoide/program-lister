using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class AboutPanel : MonoBehaviour {
	public YleApi api;
	public Text versionText;

	private YleApi.ResponseEvent onStart = new YleApi.ResponseEvent();

	void Start() {
		onStart.AddListener(SetApiVersion);
		api.Get("/v1/programs/items.json", new RequestArguments {
			{"limit", "0"},
		}, onStart);
	}

	void SetApiVersion(string res) {
		var json = JSON.Parse(res);
		versionText.text += json["apiVersion"].Value;
	}
}
