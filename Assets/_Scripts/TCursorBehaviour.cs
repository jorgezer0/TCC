using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCursorBehaviour : MonoBehaviour {

//	public Vector3 pos;
	private Vector3 vel = Vector3.zero;
	public float smooth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TCursorAdjustedPos(Vector3 pos){
		transform.position = Vector3.SmoothDamp (transform.position, new Vector3(pos.x, 0, pos.z), ref vel, smooth * Time.timeScale);
	}

	void OnTriggerStay(Collider col){
		if (col.tag == "wall")
			Debug.Log (col.contactOffset);
	}
}
