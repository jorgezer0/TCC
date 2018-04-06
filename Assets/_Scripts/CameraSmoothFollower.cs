using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollower : MonoBehaviour {

	[SerializeField]
	private Transform target;

	[SerializeField]
	private float followSpeed=1;

	[SerializeField]
	private bool xAxis, yAxis, zAxis;

	public bool canMove = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (canMove) {

			if (target == null) {
				Debug.LogError ("There is no target!");
				return; 
			}


			var newRot = Quaternion.identity;
			Quaternion rotDirection = target.localRotation;

			newRot.w = rotDirection.w;
			if (yAxis) {
				newRot.y = rotDirection.y;

			}
			if (xAxis) {
				newRot.x = rotDirection.x;

			}
			if (zAxis) {
				newRot.z = rotDirection.z;

			}

			transform.localRotation = Quaternion.Lerp (transform.localRotation, newRot, Time.deltaTime * followSpeed);

		}

	}

}
