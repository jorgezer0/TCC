using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	public GameObject attachedTo;
	public int pulse;
	public bool on = false;
	private Animation anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
	}
	
	public void InteracBehaviour(){
		on = !on;
		if (on) {
			anim.Play ("SwitchOn");
			attachedTo.BroadcastMessage ("ButtonBehaviour", pulse);
		} else {
			anim.Play ("SwitchOff");
			attachedTo.BroadcastMessage ("SwitchOff");
		}
	}

	public void ButtonBehaviour(int p){
		if (on) {
			attachedTo.BroadcastMessage ("ButtonBehaviour", pulse + p);
		}
	}
}
