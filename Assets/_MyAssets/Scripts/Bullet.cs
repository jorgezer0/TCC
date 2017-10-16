using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

	[ServerCallback]
	public void OnCollisionEnter(Collision hit){

		ActionsPlayer player = 
			hit.collider.GetComponent<ActionsPlayer>();
		if (player) {
			player.RpcDamagePlayer ();
		}

	}


}
