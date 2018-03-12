using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour {

	public GameObject attachedTo;
	public GameObject fuse;
	public int pulse;
	public bool on;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		if (fuse == null) {
//			on = false;
//		} else {
//			on = true;
//		}
	}

	public void ButtonBehaviour(int p){
		if (on) {
			attachedTo.BroadcastMessage ("ButtonBehaviour", pulse + p);
		}
	}
}
