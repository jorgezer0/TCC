using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour {

	public bool collected;
	private Transform destiny;
	private Vector3 direction;
	private Vector3 refSpeed = Vector3.zero;
	public float smooth;

	// Use this for initialization
	void Start () {
		destiny = GameObject.FindGameObjectWithTag ("Holder").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (collected) {
			direction = transform.position - destiny.position;
			transform.position = Vector3.SmoothDamp (
				transform.position,
				destiny.position,
				ref refSpeed,
				smooth);
			if (transform.position == destiny.position) {
				transform.SetParent (destiny);
			}
				
		}
	}

	public void InteracBehaviour(){
		if (!collected) {
			collected = true;
		}
	}
}
