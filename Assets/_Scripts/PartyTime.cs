using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyTime : MonoBehaviour {

	public GameObject party;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if (!party.activeSelf)
			party.SetActive (true);
	}
}
