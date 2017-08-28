using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SimpleJSON;

public class ProgramInfo : MonoBehaviour {
	public Text titleText;
	public Text bodyText;
	public Image image;
	public UnityEvent onPopup;

	void Start() {
		image.preserveAspect = true;
	}

	const string bodyFormat = @"<b>Title:</b> {0}
<b>Type:</b>{1}
<b>Subject:</b> {2}
<b>Publication</b>: {3}
<b>Description</b>: {4}";

	public void PopupInfo(EntryItem item) {
		titleText.text = item.title;
		bodyText.text = string.Format(bodyFormat,
			item.allTitles,
			item.type,
			item.subject,
			item.publicationEvent,
			item.description
		);

		var tex = item.texture;
		image.gameObject.SetActive(tex != null);
		if(tex) {
			image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
		}

		onPopup.Invoke();
	}
}
