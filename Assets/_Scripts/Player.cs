using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public bool canTeleport = false;
	private static Vector3 tDestiny;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canTeleport) {
			transform.position = Vector3.Lerp (transform.position, tDestiny, Time.deltaTime * 5);
			if (transform.position == tDestiny) {
				canTeleport = false;
			}
		}
	}

	public void TeleportTo(Vector3 tDest){
		tDestiny = tDest;
		canTeleport = true;
	}
}
