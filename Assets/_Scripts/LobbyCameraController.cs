using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			if (visualHit.collider.name == "createMatch"){
				if (Input.GetMouseButtonDown (0)) {
					visualHit.collider.GetComponent <NetworkVRUI> ().CreateRoom ();
				}
			}
			if (visualHit.collider.name == "single"){
				if (Input.GetMouseButtonDown (0)) {
					SceneManager.LoadScene ("SinglePlayer");
				}
			}
			if (visualHit.collider.name == "multi"){
				if (Input.GetMouseButtonDown (0)) {
					SceneManager.LoadScene ("Multiplayer");
				}
			}
		}

	}
}
