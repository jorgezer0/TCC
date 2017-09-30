using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour {

	public NavMeshAgent nav;
	public Transform player;
	private Vector3 dest;
	private float dist;

	public bool patrol;
	public bool chase;

	RaycastHit hit;

	// Use this for initialization
	void Start () {
		if (Physics.Raycast (transform.position, transform.forward, out hit, 1000)) {
			dest = hit.point;
		}
	}
	
	// Update is called once per frame
	void Update () {
		dist = (dest - transform.position).magnitude;
//		Debug.Log (dist);
		if (dist < 2f) {
			transform.Rotate (0, -90, 0);
			if (Physics.Raycast (transform.position, transform.forward, out hit, 1000)) {
				dest = hit.point;
			}
		}

		if (patrol) {
			nav.destination = dest;
		}

		if (chase){
			nav.destination = player.position;
		}
	}
}
