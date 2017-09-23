﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

	Animator anim;
	public GameObject player;
	float distance;
	bool wasOpen = false;

	public List<OcclusionPortal> portalsBefore = new List<OcclusionPortal>();
	public List<OcclusionPortal> portalsAfter = new List<OcclusionPortal>();

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		distance = (transform.position - player.transform.position).magnitude;
		//Debug.Log (distance);
		if ((distance < 2) && (!wasOpen)) {
			OpenDoor ();
			wasOpen = true;
		} else if (distance > 2) {
			CloseDoor ();
		}
		
	}

	public void OpenDoor(){
		anim.SetBool ("open", true);
		if (portalsBefore.Count > 0) {
			for (int i = 0; i < portalsBefore.Count; i++) {
				portalsBefore [i].open = true;
			}
		}
		if (portalsAfter.Count > 0) {
			for (int i = 0; i < portalsAfter.Count; i++) {
				portalsAfter [i].open = false;
			}
		}
	}

	public void CloseDoor(){
		anim.SetBool ("open", false);
	}
}

