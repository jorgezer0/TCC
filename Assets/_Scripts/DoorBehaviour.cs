using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DoorBehaviour : MonoBehaviour {

	Animator anim;
	public GameObject player;
	float distance;
	bool isOpen = false;

	public int pulses;
	private int reveicedPulses;

	public bool isBetweenLevels = false;
	public Object nextLevel;
	Scene loadedScene;
	AsyncOperation loading;
	bool loaded = false;

	public float openPos = 0.89f;
	public float dur = 0.5f;
	public Transform[] doors;

	// Use this for initialization
	void Start () {
		doors = GetComponentsInChildren<Transform> ();
		player = GameObject.Find ("Player");
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
//		distance = (transform.position - player.transform.position).magnitude;
		//Debug.Log (distance);
//		if ((distance < 2) && (!wasOpen)) {
//			OpenDoor ();
//			wasOpen = true;
//		} else
//		if ((distance > 3) && (wasOpen)) {
//			CloseDoor ();
//		}

		if (Input.GetKeyUp (KeyCode.B)) {
			if (!loaded) {
				loaded = true;
			}
		}
//		if (loading.isDone) {
//			anim.SetBool ("open", true);
//			wasOpen = true;
//		}
		if (Input.GetKeyUp (KeyCode.C)) {
			if (!isOpen) {
				StartCoroutine ("OpenDoor");
			} else {
				CloseDoor ();
			}
		}
	}

	public IEnumerator OpenDoor(){

		if (isBetweenLevels) {
			if (loading == null) {
				loading = SceneManager.LoadSceneAsync (nextLevel.name, LoadSceneMode.Additive);
			}

			if ((loading.isDone) && (isOpen))
				yield return null;

			yield return new WaitUntil (() => loading.isDone);

			if (loading.isDone) {
				doors [1].DOLocalMoveZ (openPos, dur);
				doors [2].DOLocalMoveZ (-openPos, dur);
				isOpen = true;
			}
		} else {
			doors [1].DOLocalMoveZ (openPos, dur);
			doors [2].DOLocalMoveZ (-openPos, dur);
//			anim.SetBool ("open", true);
			isOpen = true;
		}
	}

	public void CloseDoor(){
		doors [1].DOLocalMoveZ (0f, dur);
		doors [2].DOLocalMoveZ (0f, dur);
//		anim.SetBool ("open", false);
		isOpen = false;
	}

	public void ButtonBehaviour(int p){
		Debug.Log (p);
		if (p >= pulses)
			StartCoroutine(OpenDoor ());
	}

	public void SwitchOff(){
		CloseDoor ();
	}

	void OnTriggerExit(Collider col){
		Debug.Log ("Passed");
		StartCoroutine ("IdleClose");
	}

	IEnumerator IdleClose(){
		yield return new WaitForSeconds (3);
		CloseDoor ();
	}
}

