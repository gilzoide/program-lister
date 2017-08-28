using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using SimpleJSON;

public class YleApi : MonoBehaviour {
	// Event to run after a successful call to the Yle API, using the response
	[Serializable] public class ImageEvent : UnityEvent<Texture2D> {}
	[Serializable] public class ResponseEvent : UnityEvent<string> {}

	// Default error handler: print
	public ResponseEvent onError;

	// Constant Yle API URI components
	private const string baseUri = "https://external.api.yle.fi";
	private const string imageBaseUri = "http://images.cdn.yle.fi/image/upload/";
	private const string baseArgs = "?app_id=66ede07d&app_key=86eadbc843d2b7ee59050e659aafd49d&";

	// "Processing" circle
	private GameObject circle;

	void Start() {
		circle = transform.GetChild(0).gameObject;
	}

	// Run a GET 
	public void Get(string endpoint, RequestArguments args, ResponseEvent onSuccess) {
		circle.SetActive(true);
		StartCoroutine(ProcessRequest(endpoint, args, onSuccess));
	}
	private IEnumerator ProcessRequest(string endpoint, RequestArguments args, ResponseEvent onSuccess) {
		using(UnityWebRequest www = UnityWebRequest.Get(baseUri + endpoint + baseArgs + args)) {
			yield return www.Send();

			circle.SetActive(false);
			//if(www.isNetworkError || www.isHttpError) {
			if(www.isError) {
				onError.Invoke(www.error);
			}
			else {
				onSuccess.Invoke(www.downloadHandler.text);
			}
		}
	}

	// Run a GET on an image
	public void GetImage(string endpoint, ImageEvent onSuccess) {
		StartCoroutine(ProcessImageRequest(endpoint, onSuccess));
	}
	private IEnumerator ProcessImageRequest(string endpoint, ImageEvent onSuccess) {
		using(UnityWebRequest www = UnityWebRequest.GetTexture(imageBaseUri + endpoint + baseArgs, true)) {
			yield return www.Send();

			//if(www.isNetworkError || www.isHttpError) {
			if(www.isError) {
				onError.Invoke(www.error);
			}
			else {
				onSuccess.Invoke(DownloadHandlerTexture.GetContent(www));
			}
		}
	}
}
