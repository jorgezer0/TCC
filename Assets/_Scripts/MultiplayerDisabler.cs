using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerDisabler : NetworkBehaviour {

	[SerializeField]
	Behaviour[] compToDisable;
	public GameObject[] goToDisable;
	public MeshRenderer body;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer){
			for (int i = 0; i < compToDisable.Length; i++){
				compToDisable [i].enabled = false;
			}
			for (int i = 0; i < goToDisable.Length; i++){
				goToDisable [i].SetActive (false);
			}
		} else {
			Camera.main.gameObject.SetActive (false);
			body.enabled = true;
		}
	}

}
