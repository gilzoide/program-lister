using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramInfo : MonoBehaviour {
	public YleApi.ResponseEvent onInfoLoaded = new YleApi.ResponseEvent();

	public void FillInfo(string res) {
		Debug.Log(res);
	}
}
