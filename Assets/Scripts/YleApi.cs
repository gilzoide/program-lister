using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using SimpleJSON;

public class YleApi : MonoBehaviour {
	[Serializable]
	public struct RequestArgument {
		public string key;
		public string value;

		public override string ToString() {
			return string.Format("{0}={1}", key, value);
		}
	}

	public RequestArgument[] args;

	public void Get(string uri, UnityEvent<JSONNode> onSuccess) {
		StartCoroutine(ProcessRequest(uri, onSuccess));
	}

	private IEnumerator ProcessRequest(string uri, UnityEvent<JSONNode> onSuccess) {
		using(UnityWebRequest www = UnityWebRequest.Get(uri)) {
			yield return www.Send();
			//if(www.isNetworkError || www.isHttpError) {
			if(www.isError) {
				Debug.Log(www.error);
			}
			else {
				onSuccess.Invoke(JSON.Parse(www.downloadHandler.text));
			}
		}
	}
}
