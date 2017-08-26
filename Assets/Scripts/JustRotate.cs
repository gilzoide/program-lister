using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustRotate : MonoBehaviour {
	public Vector3 angles;

	void Update() {
		transform.Rotate(angles);
	}
}
