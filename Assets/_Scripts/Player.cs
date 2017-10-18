using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

	bool canTeleport = false;
	Vector3 tDestiny;

	[SyncVar]
	public int health = 10;
	public TextMesh life;
	public Text lifeHUD;

	// Use this for initialization
	void Start () {
		life.text = health.ToString ();
		lifeHUD.text = health.ToString ();
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

		life.transform.LookAt (2 * life.transform.position - Camera.allCameras [0].transform.position);

		if (health == 0){
			health = 10;
			life.text = health.ToString ();
			lifeHUD.text = health.ToString ();
		}
	}
		
	public void RpcTeleportTo(Vector3 tDest){
		tDestiny = tDest;
		canTeleport = true;
	}

	public void SetTimeScale(float t){
		Time.timeScale = t;
	}

	[ClientRpc]
	public void RpcDamagePlayer(){
		health--;
		life.text = health.ToString ();
		lifeHUD.text = health.ToString ();
	}
}
