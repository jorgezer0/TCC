using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCursorBehaviour : MonoBehaviour {

	private Vector3 pos;
	private Vector3 vel = Vector3.zero;
	public float smooth;
	private Vector3 cursorPos;
	private Vector3 colDir;
	private float dist;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TCursorAdjustedPos(Vector3 _pos){
		pos = _pos;
		transform.position = Vector3.SmoothDamp (transform.position, new Vector3(pos.x, 0, pos.z), ref vel, smooth * Time.timeScale);

	}

	void OnTriggerStay(Collider col){
		if (col.GetComponent<Collider> ().tag == "Wall") {
			pos = col.ClosestPointOnBounds (transform.position);
			colDir = pos - transform.position;
			dist = Vector3.Distance(pos, transform.position);
		}
	}


	void Raycasts(Vector3 _pos){
		float size = 1.5f;

		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;
		RaycastHit hit4;

		Physics.Raycast (transform.position, transform.forward, out hit1, size);
		Physics.Raycast (transform.position, transform.right, out hit2, size);
		Physics.Raycast (transform.position, transform.right*(-1), out hit3, size);
		Physics.Raycast (transform.position, transform.forward*(-1), out hit4, size);

		float distF = Vector3.Distance (hit1.point, transform.position);
		float distR = Vector3.Distance (hit2.point, transform.position);
		float distL = Vector3.Distance (hit3.point, transform.position);
		float distB = Vector3.Distance (hit4.point, transform.position);

		Vector3 newPos = transform.position;
		newPos += transform.forward * (size - distF);
		newPos += transform.right * (size - distR);
		newPos += (transform.forward*-1) * (size - distB);
		newPos += (transform.right*-1) * (size - distL);
		Debug.Log (newPos);
		transform.position = newPos;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(pos, 0.1f);
	}
}
