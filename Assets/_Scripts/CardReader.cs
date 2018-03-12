using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReader : MonoBehaviour {

	public int accessLevel;
	public GameObject attachedTo;
	public int pulse;
	private AccessLevel player;
	public Color waiting;
	public Color granted;
	public Color denied;
	private Renderer _renderer;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<AccessLevel>();
		_renderer = GetComponent<Renderer> ();
	}

	void Start(){
		_renderer.material.color = waiting;
	}

	public void InteracBehaviour(){
		if (player.accessLevel >= accessLevel) {
			Debug.Log ("ACCESS GRANTED");
			StartCoroutine ("ChangeColor", true);
			attachedTo.BroadcastMessage ("ButtonBehaviour", pulse);
		} else {
			StartCoroutine ("ChangeColor", false);
			Debug.Log ("ACCESS DENIED");
		}
	}

	IEnumerator ChangeColor(bool isGranted){
		if (isGranted) {
			_renderer.material.color = granted;
			yield return new WaitForSeconds (1);
			_renderer.material.color = waiting;
		} else {
			_renderer.material.color = denied;
			yield return new WaitForSeconds (1);
			_renderer.material.color = waiting;
		}
	}
}
