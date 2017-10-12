using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class MultiplayerPlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] compToDisable;
	public GameObject[] goToDisable;
	public MeshRenderer body;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer){
			DisableComponents ();
		} else {
			Camera.main.gameObject.SetActive (false);
			body.enabled = true;
		}
	}

	void DisableComponents(){
		for (int i = 0; i < compToDisable.Length; i++){
			compToDisable [i].enabled = false;
		}
		for (int i = 0; i < goToDisable.Length; i++){
			goToDisable [i].SetActive (false);
		}
	}

	public override void OnStartClient(){
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString ();
		Player _player = GetComponent<Player> ();

		GameManager.RegisterPlayer (_netID, _player);
	}

	void OnDisable(){
		GameManager.UnRegisterPlayer (transform.name);
	}
		
}
