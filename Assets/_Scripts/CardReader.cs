using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReader : MonoBehaviour {

	public int accessLevel;
	public GameObject attachedTo;
	private AccessLevel player;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<AccessLevel>();
	}

	public void CheckCard(){
		if (player.accessLevel >= accessLevel) {
			Debug.Log ("ACCESS GRANTED");
			attachedTo.BroadcastMessage ("ButtonBehaviour");
		} else {
			Debug.Log ("ACCESS DENIED");
		}

	}
}
