using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionManager : NetworkBehaviour {

	public NetworkManager manager;
	public void Server(){
		manager.StartServer ();
	}

	public void Client(){

		manager.StartClient ();

	}
	public void Host(){

		manager.StartHost ();
	}
}
