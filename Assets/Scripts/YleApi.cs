using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using SimpleJSON;

public class YleApi : MonoBehaviour {
	// Event to run after a successful call to the Yle API, using the response JSON
	public class JsonEvent : UnityEvent<JSONNode> {}

	// Constant Yle API URI components
	private const string baseUri = "https://external.api.yle.fi";
	private const string baseArgs = "?app_id=66ede07d&app_key=86eadbc843d2b7ee59050e659aafd49d&";

	// Run a GET 
	public void Get(string endpoint, RequestArguments args, JsonEvent onSuccess) {
		StartCoroutine(ProcessRequest(endpoint, args, onSuccess));
	}

	// Build the full URI for an endpoint in Yle API
	public string MakeUri(string endpoint, RequestArguments args) {
		return baseUri + endpoint + baseArgs + args;
	}

	private IEnumerator ProcessRequest(string endpoint, RequestArguments args, JsonEvent onSuccess) {
		using(UnityWebRequest www = UnityWebRequest.Get(MakeUri(endpoint, args))) {
			yield return www.Send();

			//if(www.isNetworkError || www.isHttpError) {
			if(www.isError) {
				Debug.LogError(www.error);
			}
			else {
				onSuccess.Invoke(JSON.Parse(www.downloadHandler.text));
			}
		}
	}
}
