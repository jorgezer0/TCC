using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public GameObject attachedTo;
	private Animation anim;
	public int pulse;

	void Start(){
		anim = GetComponent<Animation> ();
	}

	public void InteracBehaviour(){
		if (!anim.isPlaying) {
			Debug.Log ("Pressed Button!");
			attachedTo.BroadcastMessage ("ButtonBehaviour", pulse);
			anim.Play ();
		}
	}
}
