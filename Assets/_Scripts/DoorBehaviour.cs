using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

	Animator anim;
	public GameObject player;
	float distance;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		distance = (transform.position - player.transform.position).magnitude;
		//Debug.Log (distance);
		if (distance < 2) {
			OpenDoor ();
		} else if (distance > 2) {
			CloseDoor ();
		}
		
	}

	public void OpenDoor(){
		anim.SetBool ("open", true);
	}

	public void CloseDoor(){
		anim.SetBool ("open", false);
	}
}
