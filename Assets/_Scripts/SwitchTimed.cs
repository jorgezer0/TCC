using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTimed : MonoBehaviour {

	public GameObject attachedTo;
	public bool on = false;
	public int pulse;
	public float timer;
	public float count;
	public GameObject switchKey;
	private Vector3 offPosition;
	private Vector3 onPosition = new Vector3(0, 0.4f, 0);
	private Animation anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
		offPosition = switchKey.transform.localPosition;
	}

	void Update(){
		if (on) {
			count += Time.deltaTime;
			switchKey.transform.localPosition = Vector3.Lerp (onPosition, offPosition, count / timer);
			if (count >= timer) {
				on = false;
				attachedTo.BroadcastMessage ("SwitchOff");
				count = 0;
			}
		}
	}
		
	public void InteracBehaviour(){
		on = true;
		switchKey.transform.localPosition = onPosition;
		attachedTo.BroadcastMessage ("ButtonBehaviour", pulse);
//		if (on) {
//			anim.Play ("SwitchOn");
//		} else {
//			anim.Play ("SwitchOff");
//			attachedTo.BroadcastMessage ("SwitchOff");
//		}
	}

}
