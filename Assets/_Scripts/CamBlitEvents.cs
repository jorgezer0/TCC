using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBlitEvents : MonoBehaviour {

	private PlayerController player;

	// Use this for initialization
	void Start () {
		player = GetComponentInParent<PlayerController> ();
	}

	public void Teleport(){
		player.Teleport ();
	}
}
