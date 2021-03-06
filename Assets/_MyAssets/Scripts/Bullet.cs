﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

	public Player shooter;

//	[ServerCallback]
	public void OnCollisionEnter(Collision hit){
		if (hit.collider.tag == "Player") {
			Player player = hit.collider.GetComponent<Player> ();

			if (player) {
				player.RpcDamagePlayer (shooter.name);
			}

		}

		Destroy (this.gameObject);
	}


}
