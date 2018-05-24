using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotator : MonoBehaviour {

	public Transform cam;
	public Transform controller;
	public float followSpeed;

	private float _time;

	// Update is called once per frame
	void Update () {
		_time = Time.deltaTime / Time.timeScale;

		if (Vector3.Angle (transform.forward, cam.forward) > 25f) {
			transform.localRotation = Quaternion.Lerp (transform.localRotation, cam.localRotation, _time * followSpeed);
			controller.localRotation = Quaternion.Lerp (controller.localRotation, Quaternion.Inverse(cam.localRotation), _time * followSpeed);
		}
	}
}
