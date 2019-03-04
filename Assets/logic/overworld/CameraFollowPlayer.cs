using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Add camera bounding
public class CameraFollowPlayer : MonoBehaviour {

	public Transform followObject;

	// Update is called once per frame
	void FixedUpdate () {
		transform.position = new Vector3( followObject.position.x, followObject.position.y, transform.position.z );
	}
}
