using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionsPlayer : NetworkBehaviour {

	//A propriedade SyncVar define que essa variavel será distribuida pelo servidor, quando o valor dela é alterado ela é marcada como "dirty" e sera enviada assim que possível pela rede
	[SyncVar]
	public int health = 2;
	public float force = 1000;
	public GameObject bullet;
	//A propriedade define que o comando rodará APENAS no servidor, o metodo precisa ter o prefixo Cmd
	[Command]
	public void CmdShoot(Vector3 position, Vector3 dir){
		RpcInstantiateBullet (position, dir);
	}
	//A propriedade define que todos os clientes irão rodar este metodo, o metodo precisa ter o prefixo Rpc
	[ClientRpc]
	public void RpcInstantiateBullet(Vector3 position, Vector3 dir){
		
		GameObject obj = (GameObject)Instantiate (bullet, 
			position,Quaternion.identity);
		obj.GetComponent<Rigidbody> ().AddForce (dir * force);
	}

	public void Update(){
		if (isLocalPlayer) {
			if(Input.GetKeyDown(KeyCode.G))
				CmdShoot (transform.position + 
					transform.forward, transform.forward);
		}
	}

	[ClientRpc]
	public void RpcDamagePlayer(){
		health--;
	}
}
