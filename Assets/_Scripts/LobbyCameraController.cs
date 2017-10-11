using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCameraController : MonoBehaviour {

	RaycastHit visualHit;
	
	// Update is called once per frame
	void Update () {

		if (Physics.Raycast (transform.position, transform.forward, out visualHit, 100)) {
			if (visualHit.collider.name == "client"){
				if (Input.GetMouseButtonDown (0)) {
					visualHit.collider.GetComponent <NetworkVRUI> ().LanClient ();
				}
			}
			if (visualHit.collider.name == "host"){
				if (Input.GetMouseButtonDown (0)) {
					visualHit.collider.GetComponent <NetworkVRUI> ().LanHost ();
				}
			}
		}

	}
}
