using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkVRUI : MonoBehaviour {

	public NetworkManager netManager;
	public string address;

	public void LanHost(){
		netManager.StartHost ();
	}

	public void LanClient(){
		netManager.networkAddress = address;
		netManager.StartClient ();
	}


}
