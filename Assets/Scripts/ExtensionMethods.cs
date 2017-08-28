using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods {
	// Set RectTransform height after Updating children positions with a VerticalLayoutGroup
	// Should be called as a Coroutine: `StartCoroutine(rt.SetHeightAfterUpdate());`
	public static IEnumerator SetHeightAfterUpdate(this RectTransform rt) {
		if(rt.childCount != 0) {
			yield return new WaitForEndOfFrame();
			var lastChildRt = rt.GetChild(rt.childCount - 1).GetComponent<RectTransform>();
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -lastChildRt.anchoredPosition.y + lastChildRt.rect.height);
		}
	}
}
