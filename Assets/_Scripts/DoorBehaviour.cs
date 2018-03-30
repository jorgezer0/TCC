using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DoorBehaviour : MonoBehaviour {

	Animator anim;
	private GameObject player;
	public bool _open = false;
	public bool autoClose = false;
	float distance;
	bool isOpen = false;

	public int pulses;
	public bool aditivePulses;
	private int receivedPulses;

	public bool isBetweenLevels = false;
	public Object nextLevel;
	Scene loadedScene;
	AsyncOperation loading;
	bool loaded = false;

	public float openPos = 0.89f;
	public float dur = 0.5f;
	public Transform[] doors;

	void Awake(){
		doors = GetComponentsInChildren<Transform> ();
		if (_open) {
			doors [1].localPosition = new Vector3 (0, 0, openPos);
			doors [2].localPosition = new Vector3 (0, 0, -openPos);
		}
	}

	void Start () {
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
		if (aditivePulses) {
			receivedPulses += p;
		} else {
			receivedPulses = p;
		}

		if (receivedPulses >= pulses) {
			StartCoroutine (OpenDoor ());
		} else {
			if (isOpen)
				CloseDoor ();
		}
	}

	public void SwitchOff(){
		CloseDoor ();
	}

	void OnTriggerExit(Collider col){
		Debug.Log ("Passed");
		if (autoClose)
			StartCoroutine ("IdleClose");
	}

	IEnumerator IdleClose(){
		yield return null;
		CloseDoor ();
	}
}

