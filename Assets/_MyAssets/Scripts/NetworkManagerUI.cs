using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerUI : MonoBehaviour {

	public NetworkManager networkManager;


	public void CreateServer(){
		//Cria um servidor e se conecta a ele como cliente
		networkManager.StartHost ();

	}
	public void ConnectClient(){

		//Cria um cliente e tenta se conectar ao servidor configurado no NetworkManager
		networkManager.StartClient ();
	}
}
