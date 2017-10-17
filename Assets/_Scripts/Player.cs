using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	bool canTeleport = false;
	Vector3 tDestiny;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canTeleport) {
			transform.position = Vector3.Lerp (transform.position, tDestiny, Time.deltaTime * 5);
			if (transform.position == tDestiny) {
				canTeleport = false;
				Debug.Log (transform.name + " moving.");
			}
		}
	}
		
	public void RpcTeleportTo(Vector3 tDest){
		tDestiny = tDest;
		canTeleport = true;
	}

	public void SetTimeScale(float t){
		Time.timeScale = t;
	}
}
