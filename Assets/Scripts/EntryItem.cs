using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class EntryItem : MonoBehaviour {
	public string title;
	public string allTitles;
	public string subject;
	public string type;
	public string publicationEvent;
	public Texture2D texture = null;

	private YleApi.ImageEvent onImage = new YleApi.ImageEvent();

	void Start() {
		onImage.AddListener(SetTexture);
	}

	void SetTexture(Texture2D tex) {
		texture = tex;
	}

	// Insert spaces between words delimited by Capital letters, for fields that are FormattedLikeThis
	private static Regex matchCapital = new Regex(@"((?<=\p{Ll})\p{Lu}|\p{Lu}(?=\p{Ll}))");
	private static string AddSpacesBetweenCapital(string input) {
		return matchCapital.Replace(input, " $1");
	}

	public void Setup(YleApi yleApi, JSONNode data) {
		// prefer finnish title over the others
		title = data["title"]["fi"].Value;
		if(string.IsNullOrEmpty(title)) {
			title = data["title"][0].Value;
		}
		GetComponentInChildren<Text>().text = title;

		allTitles = BuildTitles(data["title"].AsObject);
		subject = BuildSubjects(data["subject"].AsArray);
		publicationEvent = BuildPublicationEvents(data["publicationEvent"].AsArray);
		type = AddSpacesBetweenCapital(data["type"].Value);

		if(data["image"]) {
			yleApi.GetImage(data["image"].Value + ".jpg", onImage);
		}
	}

	// Generate the Title string, composed by all the titles
	private string BuildTitles(JSONObject data, int level = 1) {
		switch(data.Count) {
			case 0:
				return "-";
			case 1:
				return data[0].Value;
			default:
				var lines = new string[data.Count];
				var i = 0;
				var indent = new string(' ', 2 * level);
				foreach(KeyValuePair<string, JSONNode> entry in data) {
					lines[i] = "\n" + indent + "<i>" + entry.Key + "</i>: " + entry.Value.Value;
					i++;
				}
				return string.Join("", lines);
		}
	}

	// Generate the Subjects string, composed by all the subjects
	private string BuildSubjects(JSONArray data, int level = 1) {
		switch(data.Count) {
			case 0:
				return "-";
			case 1:
				return BuildTitles(data[0]["title"].AsObject);
			default:
				var lines = new string[data.Count];
				var indent = new string(' ', 2 * level);
				for(int i = 0; i < data.Count; i++) {
					lines[i] = "\n" + indent + (i + 1) + ": " + BuildTitles(data[i]["title"].AsObject, level + 1);
				}
				return string.Join("", lines);
		}
	}

	// Generate the Publication Event string, composed by all the publication events
	private string BuildPublicationEvents(JSONArray data, int level = 1) {
		const string pattern = @"yyyy\-MM\-dd\THH\:mm\:sszzz";
		var lines = new string[data.Count];
		var indent = new string(' ', 2 * level);
		for(int i = 0; i < data.Count; i++) {
			var startTime = DateTime.ParseExact(data[i]["startTime"].Value, pattern, null);
			var endTime = DateTime.ParseExact(data[i]["endTime"].Value, pattern, null);
			lines[i] = "\n" + indent + startTime.ToString("g") + " ~ " + endTime.ToString("g");
		}
		return string.Join("", lines);
	}
}
