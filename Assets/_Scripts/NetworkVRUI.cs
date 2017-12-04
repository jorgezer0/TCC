using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkVRUI : MonoBehaviour {

	private NetworkManager netManager;
	public string address;
	public string roomName;
	public uint maxPlayers;

	void Start(){
		netManager = NetworkManager.singleton;
		netManager.StartMatchMaker ();
	}

	public void LanHost(){
		netManager.StartHost ();
	}

	public void LanClient(){
		netManager.networkAddress = address;
		netManager.StartClient ();
	}

	public void CreateRoom(){
		netManager.matchMaker.CreateMatch (roomName, maxPlayers, true, "", "", "", 0, 0, netManager.OnMatchCreate);
	}

	public void JoinRoom(){
		
	}


}
